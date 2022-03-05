using RestSharp;
using System.Net;
using System;
using Supremes;
using Supremes.Nodes;
using System.Text;
using TestLinkApi;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Linq;

namespace TLTCImport
{

    public static class TlReportTcResult
    {
        private static string baseUrl = "http://93.170.52.203:80";

        private static string CSRFName = "";
        private static string CSRFToken = "";

        private static TestLink testLinkApi;

        private static RestClient ClientTl = new RestClient(baseUrl)
        {
            FollowRedirects = true,
            CookieContainer = new CookieContainer()
        };                

        public static bool Authorization(string apiDevKey)
        {
            ////Получение нужных данных для входа в TestLink
            //var request = new RestRequest("/login.php", Method.GET);

            //var response = ClientTl.Execute(request);

            //Console.WriteLine(response.ErrorException + " " + response.ErrorMessage);

            //Document doc = Dcsoup.Parse(response.Content);
            //CSRFName = doc.GetElementById("CSRFName").Val;
            //CSRFToken = doc.GetElementById("CSRFToken").Val;

            //// Вход в TestLink
            //request = new RestRequest("/login.php", Method.POST);

            //request.AddHeader("content-type", "application/x-www-form-urlencoded");
            //request.AddParameter("application/x-www-form-urlendcoded",
            //    $"CSRFName={CSRFName}&" +
            //    $"CSRFToken={CSRFToken}&" +
            //    $"reqURI=&" +
            //    $"destination=&" +
            //    $"tl_login={username}&" +
            //    $"tl_password={password}&" +
            //    $"login_submit=Войти", ParameterType.RequestBody);

            //response = ClientTl.Execute(request);

            //if (response.Content.Contains("Try again! Wrong login name or password!") ||
            //    response.Content.Contains("Попробуйте снова! Вы ввели неверное имя или пароль!"))
            //    return false;

            if (string.IsNullOrEmpty(apiDevKey))
                return false;
            else
                testLinkApi = new TestLink(apiDevKey, baseUrl + "/lib/api/xmlrpc/v1/xmlrpc.php");

            try
            {
                if(testLinkApi.GetProjects().Count > 0)                 
                    return true;                
            }
            catch (TestLinkException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Получить сборку по id тест плана
        /// </summary>
        public static string GetBuildByTestPlanId(int testPlanId)
        {
            List<Build> lb = testLinkApi.GetBuildsForTestPlan(testPlanId);
            return lb.FirstOrDefault().name;
        }


        private static string GetApiDevKey()
        {
            var request = new RestRequest("/lib/usermanagement/userInfo.php", Method.GET);

            var response = ClientTl.Execute(request);

            string result = response.Content;

            string startString = "Personal API access key = ";

            string apiValue = result.Substring(result.IndexOf(startString) + startString.Length, 32);

            if (apiValue == "нет" || apiValue == "Нет")
            {
                result = CreateNewApiDevKey();

                apiValue = result.Substring(result.IndexOf(startString) + startString.Length, 32);
            }
            
            return apiValue;
        }

        private static string CreateNewApiDevKey()
        {
            var request = new RestRequest("/lib/usermanagement/userInfo.php", Method.POST);

            request.AddHeader("content-type", "multipart/form-data;boundary=----WebKitFormBoundary6Ghw83UDkbQ0jSRp");
            StringBuilder sb = new StringBuilder();

            sb.Append(AddParameter("CSRFName", CSRFName));
            sb.Append(AddParameter("CSRFToken", CSRFToken));
            sb.Append(AddParameter("doAction", "genAPIKey"));

            sb.Append("------WebKitFormBoundary6Ghw83UDkbQ0jSRp--");

            request.AddParameter("multipart/form-data;boundary=----WebKitFormBoundary6Ghw83UDkbQ0jSRp",
            sb.ToString(), ParameterType.RequestBody);

            var response = ClientTl.Execute(request);

            return response.Content;
        }

        /// <summary>
        /// Импортирует тесткейсы в тестлинк. Если объём импортируемых данных превышает 400 КБ, они отправляются частями. 
        /// </summary>
        /// <param name="importData">Импортируемые тесткейсы</param>
        /// <param name="projectName">Название тестируемого продукта</param>
        /// <returns></returns>
        public static bool ImportTestCases(string pathToImport, string projectName)
        {
            string testcases = File.ReadAllText(pathToImport);
            List<string> chunks = new List<string>();
            if (File.ReadAllBytes(pathToImport).Length >= 409600)
            {
                var dataDoc = XDocument.Parse(testcases);
                var xmls = dataDoc.Root.Elements().ToArray();

                for (int i = 0; i < xmls.Length; i++)
                {
                    using (var file = File.CreateText(string.Format("Content\\xml{0}.xml", i + 1))) 
                    {
                        file.Write(xmls[i].ToString());
                        file.Close();
                        testcases = File.ReadAllText($"Content\\xml{i + 1}.xml");
                        chunks.Add(testcases);
                    }
                }
            }
            else 
            {
                chunks.Add(testcases);
            }
            foreach (string chunk in chunks) 
            {
                TcImport_Send(chunk, projectName);
                System.Threading.Thread.Sleep(1000);
            }
            return true;
        }

        /// <summary>
        /// Отправить запрос за импорт тесткейсов
        /// </summary>
        /// <param name="testCases">Импортируемые тесткейсы</param>
        /// <returns></returns>
        private static IRestResponse TcImport_Send(string testCases, string projectName) 
        {
            string projectId = "" + testLinkApi.GetProject(projectName).id;

            RestRequest request = new RestRequest("/lib/testcases/tcImport.php", Method.POST);
            request.Timeout = 600000;
            request.ReadWriteTimeout = 600000;

            request.AddHeader("content-type", "multipart/form-data;boundary=----WebKitFormBoundary6Ghw83UDkbQ0jSRp");
            StringBuilder sb = new StringBuilder();
            sb.Append(AddParameter("CSRFName", CSRFName));
            sb.Append(AddParameter("CSRFToken", CSRFToken));
            sb.Append(AddParameter("importType", "XML"));
            sb.Append(AddFile(testCases));
            sb.Append(AddParameter("hit_criteria", "name"));
            sb.Append(AddParameter("action_on_duplicated_name", "generate_new"));
            sb.Append(AddParameter("useRecursion", "1"));
            sb.Append(AddParameter("bIntoProject", "1"));
            sb.Append(AddParameter("containerID", projectId));
            sb.Append(AddParameter("MAX_FILE_SIZE", "409600"));
            sb.Append(AddParameter("UploadFile", "Загрузить файл"));
            sb.Append("------WebKitFormBoundary6Ghw83UDkbQ0jSRp--");

            request.AddParameter("multipart/form-data;boundary=----WebKitFormBoundary6Ghw83UDkbQ0jSRp",
            sb.ToString(), ParameterType.RequestBody);

            return ClientTl.Execute(request);
        }

        private static string AddParameter(string name, string value)
        {
            return $"------WebKitFormBoundary6Ghw83UDkbQ0jSRp\r\nContent-Disposition: form-data; name=\"{name}\"\r\n\r\n{value}\r\n";
        }

        private static string AddFile(string value)
        {
            return $"------WebKitFormBoundary6Ghw83UDkbQ0jSRp\r\nContent-Disposition: " +
                $"form-data; name=\"uploadedFile\"; filename=\"TestsResults.xml\"\n" +
                $"Content-Type: text/xml" +
                $"\r\n\r\n{value}\n";
        }

        public static List<TestProject> GetAllProjects()
        {
            return testLinkApi.GetProjects();
        }

        public static int GetProjectIdByName(string projectName)
        {
            return Convert.ToInt32(testLinkApi.GetProject(projectName).id);
        }

        public static int GetTestPlanIdByName(string projectName, string testPlanName)
        {
            return Convert.ToInt32(testLinkApi.getTestPlanByName(projectName, testPlanName).id);
        }

        public static List<TestPlan> GetAllProjectTestPlans(int projectId)
        {
            return testLinkApi.GetProjectTestPlans(projectId);
        }

        public static List<TestSuite> GetAllTestProjectSuites(int projectId)
        {
            return testLinkApi.GetFirstLevelTestSuitesForTestProject(projectId);
        }

        //Экспорт тест кейсов в тестлинк
        public static XElement ExportTestSuite(int testSuiteId)
        {
            // Импорт тест кейсов в TestLink
            RestRequest request = new RestRequest("/lib/testcases/tcExport.php", Method.POST);

            request.AddHeader("content-type", "multipart/form-data;boundary=----WebKitFormBoundary6Ghw83UDkbQ0jSRp");

            StringBuilder sb = new StringBuilder();
            sb.Append(AddParameter("CSRFName", CSRFName));
            sb.Append(AddParameter("CSRFToken", CSRFToken));
            sb.Append(AddParameter("export_filename", "TLTC import.xml"));
            sb.Append(AddParameter("exportType", "XML"));
            sb.Append(AddParameter("exportTestCaseExternalID", "1"));
            sb.Append(AddParameter("exportReqs", "1"));
            sb.Append(AddParameter("exportCFields", "1"));
            sb.Append(AddParameter("testcase_id", "0"));
            sb.Append(AddParameter("tcversion_id", "0"));
            sb.Append(AddParameter("containerID", "" + testSuiteId));
            sb.Append(AddParameter("useRecursion", "1"));
            sb.Append(AddParameter("export", "Export"));
            sb.Append("------WebKitFormBoundary6Ghw83UDkbQ0jSRp--");

            request.AddParameter("multipart/form-data;boundary=----WebKitFormBoundary6Ghw83UDkbQ0jSRp",
                sb.ToString(), ParameterType.RequestBody);

            var response = ClientTl.Execute(request);

            return XElement.Parse(response.Content);
        }
    }
}
