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

    public static class TestLinkResult
    {
        private static string baseUrl = "http://93.170.52.203:80";

        private static string CSRFName = "";
        private static string CSRFToken = "";
        
        //Просто рандомный набор символов, можно указать что угодно
        private static string WebKitFormBoundary = "WebKitFormBoundarywnCYxHgmP97d3sQW";

        public static TestLink testLinkApi;

        private static RestClient ClientTl = new RestClient(baseUrl)
        {
            FollowRedirects = true,
            CookieContainer = new CookieContainer()
        };

        public struct TestCaseValues
        {
            public string testCaseId;
            public string resultRun;
        }

        public static bool Authorization(string apiDevKey)
        {
            //Получение нужных данных для входа в TestLink
            var request = new RestRequest("/login.php", Method.GET);
            var response = ClientTl.Execute(request);
            Console.WriteLine(response.ErrorException + " " + response.ErrorMessage);

            Document doc = Dcsoup.Parse(response.Content);
            CSRFName = doc.GetElementById("CSRFName").Val;
            CSRFToken = doc.GetElementById("CSRFToken").Val;
            CSRFName = doc.GetElementById("CSRFName").Val;

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

        /// <summary>
        /// Получить id сборки по id тест плана
        /// </summary>
        public static int GetIdBuildByTestPlanId(int testPlanId)
        {
            List<Build> lb = testLinkApi.GetBuildsForTestPlan(testPlanId);
            return lb.FirstOrDefault().id;
        }

        private static string GetApiDevKey()
        {
            var request = new RestRequest("/lib/usermanagement/userInfo.php", Method.GET);

            var response = ClientTl.Execute(request);

            string result = response.Content;

            string startString = "Personal API access key = ";

            string apiValue = result.Substring(result.IndexOf(startString) + startString.Length, 32);

            //if (apiValue == "нет" || apiValue == "Нет")
            //{
                result = CreateNewApiDevKey();

                apiValue = result.Substring(result.IndexOf(startString) + startString.Length, 32);
            //}
            
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
        /// Импортирует иформацию о прогоне в тестлинк. Если объём импортируемых данных превышает 400 КБ, они отправляются частями. 
        /// </summary>
        /// <param name="importData">Импортируемые тесткейсы</param>
        /// <param name="projectName">Название тестируемого продукта</param>
        /// <returns></returns>
        public static int ImportsRunInfoInTestLink(string pathToImport, int testPlanId, Dictionary<string, string> valuesCases)
        {
            var countSubmittedЕestСases = 0;
            var buildId = GetIdBuildByTestPlanId(testPlanId);

            string testcases = File.ReadAllText(pathToImport + "TestsResults.xml");
            List<string> chunks = new List<string>();
            if (File.ReadAllBytes(pathToImport + "TestsResults.xml").Length >= 409600)
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
                //С крупными файлами пока не работает, но реализовано                               
                countSubmittedЕestСases = ResultImport_Send(buildId, testPlanId, valuesCases);                               
            }
            return countSubmittedЕestСases;
        }

        /// <summary>
        /// Импорт результатов в TestLink
        /// </summary>
        /// <param name="testCases">Импортируемые тесткейсы</param>
        /// <returns></returns>
        public static int ResultImport_Send(int buildId, int testPlanId, Dictionary<string, string> valuesCasesJenkins)
        {
            //Получаем External Id и TestCaseId из всех Suites. Взято с TestLink.
            Dictionary<string, string> testCaseExternalIDAndTestCaseId;
            testCaseExternalIDAndTestCaseId = GetExternalIDAndTestCaseIdAllSuitesByTestPlanId(testPlanId);

            //Получаем externalId, testCaseId и resultRun.
            //Файлы объединены на основе данных с TestLink и Jenkins.
            Dictionary<string, TestCaseValues> ExternalId_TestCaseId_ResultRun;
            ExternalId_TestCaseId_ResultRun = GetExternalId_TestCaseId_ResultRun(testCaseExternalIDAndTestCaseId, valuesCasesJenkins);

            foreach (var param in ExternalId_TestCaseId_ResultRun)
                testLinkApi.ReportTCResult(Int32.Parse(param.Value.testCaseId), testPlanId, param.Value.resultRun, 0, buildId.ToString());

            return ExternalId_TestCaseId_ResultRun.Count;
        }

        //Получаем External Id и TestCaseId из всех Suites. Взято с TestLink.
        private static Dictionary<string, string> GetExternalIDAndTestCaseIdAllSuitesByTestPlanId(int testPlanId)
        {
            var suitesId = testLinkApi.GetTestSuitesForTestPlan(testPlanId);
            
            var testCaseExternalIDAndName = new Dictionary<string, string>();
            foreach (var item in suitesId)
            {
                try
                {
                    var testCases = testLinkApi.GetTestCasesForTestSuite(item.id, false);
                    for (int i = 0; i < testCases.Count; i++)
                    {
                        testCaseExternalIDAndName.Add("alphabi-" + testCases[i].external_id, testCases[i].id.ToString());
                    }
                }
                catch(CookComputing.XmlRpc.XmlRpcIllFormedXmlException)
                {
                    var testCases = testLinkApi.GetTestCasesForTestSuite(item.id, false);
                    for (int i = 0; i < testCases.Count; i++)
                    {
                        testCaseExternalIDAndName.Add("alphabi-" + testCases[i].external_id, testCases[i].id.ToString());
                    }
                }                
            }

            return testCaseExternalIDAndName;
        }

        //Получение External Id, TestCaseId и ResultRun для последующего переноса в TestLink
        //Инфа собирается на основе информации взятой из TestLink и Jenkins.
        private static Dictionary<string, TestCaseValues> GetExternalId_TestCaseId_ResultRun(Dictionary<string, string> valuesSuites, Dictionary<string, string> valuesJenkins)
        {
            var ExternalIDAndTestCaseIdAndResults = new DictionaryForTestCase();
            foreach (var valueSuite in valuesSuites)
            {
                foreach (var value in valuesJenkins)
                {
                    //Проверяем, что ключи, указанные в Json файле и в TestLink одинаковые
                    if (valueSuite.Key == value.Key)
                    {
                        ExternalIDAndTestCaseIdAndResults.AddTestCase(valueSuite.Key, valueSuite.Value, value.Value);
                    }
                }
            }
            return ExternalIDAndTestCaseIdAndResults;
        }

        //Почему-то не работает
        ///// <summary>
        ///// Отправить запрос за импорт тесткейсов
        ///// </summary>
        ///WebKitFormBoundary - просто рандомный набор символов, можно указать что угодно
        private static IRestResponse TcImport_Send(string resultsRun, int buildId, int testPlanId)
        {
            RestRequest request = new RestRequest("lib/results/resultsImport.php", Method.POST);
            //Неуверен, что таймеры нужны
            request.Timeout = 600000;
            request.ReadWriteTimeout = 600000;

            request.AddHeader("content-type", $"multipart/form-data;boundary=----{WebKitFormBoundary}");
            StringBuilder sb = new StringBuilder();
            sb.Append(AddParameter("CSRFName", CSRFName));
            sb.Append(AddParameter("CSRFToken", CSRFToken));
            sb.Append(AddParameter("importType", "XML"));
            sb.Append(AddParameter("MAX_FILE_SIZE", "409600"));
            sb.Append(AddFile(resultsRun));
            sb.Append(AddParameter("buildID", buildId.ToString()));
            sb.Append(AddParameter("platformID", "0"));
            sb.Append(AddParameter("tplanID", testPlanId.ToString()));
            sb.Append(AddParameter("UploadFile", "Загрузить файл"));
            sb.Append($"------{WebKitFormBoundary}--");

            request.AddParameter($"multipart/form-data;boundary=----{WebKitFormBoundary}",
                            sb.ToString(), ParameterType.RequestBody);

            return ClientTl.Execute(request);
        }

        private static string AddParameter(string name, string value)
        {
            return $"------{WebKitFormBoundary}\r\nContent-Disposition: form-data; name=\"{name}\"\r\n\r\n{value}\r\n";
        }

        private static string AddFile(string value)
        {
            return $"------{WebKitFormBoundary}\r\nContent-Disposition: " +
                $"form-data; name=\"uploadedFile\"; filename=\"TestsResults.xml\"\n" +
                $"Content-Type: text/xml" +
                $"\r\n\r\n{value}\r\n";
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
    }
}
