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

        private static string apiDevKey = "";

        private static TestLink testLink;

        private static RestClient ClientTl = new RestClient(baseUrl)
        {
            FollowRedirects = true,
            CookieContainer = new CookieContainer()
//#if DEBUG
//            ,Proxy = new WebProxy("http://localhost:8888")
//#endif
        };

        //        /// <summary>
        //        /// Очищает старые результаты сгенерированые Allure
        //        /// </summary>
        //        /// <param name="path">Путь к папке с отчетами allure</param>
        //        public static void ClearOldAllureResults(string path)
        //        {
        //            DirectoryInfo di = new DirectoryInfo(path);

        //            foreach (FileInfo file in di.GetFiles())
        //                file.Delete();
        //        }

        static TlReportTcResult()
        {
            ClientTl.AddDefaultHeader("User-Agent",
                            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36");
            ClientTl.AddDefaultHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            ClientTl.AddDefaultHeader("Accept-Encoding", "gzip, deflate");
            ClientTl.AddDefaultHeader("Accept", "*/*");
        }

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
                testLink = new TestLink(apiDevKey, baseUrl + "/lib/api/xmlrpc/v1/xmlrpc.php");
            return true;
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
            string projectId = "" + testLink.GetProject(projectName).id;

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
            return testLink.GetProjects();
        }

        public static int GetProjectIdByName(string projectName)
        {
            return Convert.ToInt32(testLink.GetProject(projectName).id);
        }

        public static List<TestPlan> GetAllProjectTestPlans(int projectId)
        {
            return testLink.GetProjectTestPlans(projectId);
        }

        public static List<TestSuite> GetAllTestProjectSuites(int projectId)
        {
            return testLink.GetFirstLevelTestSuitesForTestProject(projectId);
        }
   
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
#region old
//        /// <summary>
//        /// Отправка сгенерированных результатов в Test Link
//        /// </summary>
//        /// <param name="projectName">Имя проекта</param>
//        /// <param name="planName">Имя тест-плана</param>
//        /// <param name="buildName">Имя сборки (Если такая сборка уже присутствует в Test Link, то новые результаты проставляются поверх старых)</param>
//        /// <param name="path">Путь к папке с отчетами Allure</param>
//        public static void ReportResutls(string projectName, string planName, string buildName, string path)
//        {
//            TestLink testLink = new TestLink(apiDevKey, baseUrl + "/lib/api/xmlrpc/v1/xmlrpc.php");

//            int tPlanId = testLink.getTestPlanByName(projectName, planName).id;

//            int tBuildId = -1;

//            // Проверяем наличие сборки с указанными имененм
//            foreach (var build in testLink.GetBuildsForTestPlan(tPlanId))
//            {
//                if (build.name == buildName)
//                {
//                    tBuildId = build.id;
//                    break;
//                }
//            }

//            // Если сборка с указанным именен отсутствует, то создаем новую
//            if(tBuildId == -1)
//            {
//                testLink.CreateBuild(tPlanId, buildName, "");
//                tBuildId = testLink.GetLatestBuildForTestPlan(tPlanId).id;
//            }

//            string results = ReadResults(path);
//            PostResults(results, Convert.ToString(tPlanId), Convert.ToString(tBuildId));
//        }

//        private static string ReadResults(string path)
//        {
//            // Создаем StringBuilder для создания xml "файла"
//            StringBuilder resultsSB = new StringBuilder();

//            resultsSB.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
//            resultsSB.Append("<results>");

//            // Считываем все Allure отчеты по тестам
//            foreach (string file in Directory.EnumerateFiles(path, "*-result.json"))
//            {
//                var jobject = JObject.Parse(File.ReadAllText(file));

//                // Id теста должен быть проставлен в атрибуте Author
//                string testCaseId = "";

//                foreach (var item in jobject["labels"])
//                {
//                    if (item["name"].Value<string>().Equals("owner"))
//                    {
//                        testCaseId = item["value"].Value<string>();
//                        break;
//                    }
//                }

//                if (string.IsNullOrEmpty(testCaseId))
//                    continue;

//                // Парсим json файл отчета Allure
//                resultsSB.Append($"<testcase id=\"{testCaseId}\">");

//                string status = jobject["status"].Value<string>();
//                switch (status[0])
//                {
//                    case 'p':
//                        resultsSB.Append($"<result>p</result>");
//                        break;
//                    case 'f':
//                        resultsSB.Append($"<result>f</result>");
//                        break;
//                    case 's':
//                        resultsSB.Append($"<result>b</result>");
//                        break;
//                }

//                var statusDetails = jobject["statusDetails"];
//                if (statusDetails != null)
//                {
//                    resultsSB.Append("<notes>");

//                    var message = statusDetails["message"];
//                    if (message != null)
//                    {
//                        string messageText = message.Value<string>();
//                        messageText = messageText.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;").Replace("'", "&apos;").Replace("\"", "&quot;");
//                        resultsSB.Append(messageText);
//                    }

//                    resultsSB.Append(Environment.NewLine);

//                    var trace = statusDetails["trace"];
//                    if (trace != null)
//                    {
//                        string traceText = trace.Value<string>();
//                        traceText = traceText.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;").Replace("'", "&apos;").Replace("\"", "&quot;");
//                        resultsSB.Append(traceText);
//                    }

//                    resultsSB.Append("</notes>");
//                    resultsSB.Append("</testcase>");
//                }

//            }

//            resultsSB.Append("</results>");


//            string xml = resultsSB.ToString();

////#if DEBUG_LOCAL
////            // Сохраняем xml файл с кейсами в Content
////            File.WriteAllText("../../../Content/tlcases.xml", xml);
////#endif

//            return xml;
//        }

//        private static void PostResults(string results, string planId, string buildId)
//        {
//            // Получение нужных данных для входа в TestLink
//            var request = new RestRequest("/login.php", Method.GET);

//            RestResponse response = (RestResponse)ClientTl.Execute(request);

//            Console.WriteLine(response.ErrorException + " " + response.ErrorMessage);

//            Document doc = Dcsoup.Parse(response.Content);
//            string CSRFName = doc.GetElementById("CSRFName").Val;
//            string CSRFToken = doc.GetElementById("CSRFToken").Val;

//            // Вход в TestLink
//            request = new RestRequest("/login.php", Method.POST);


//            request.AddHeader("content-type", "application/x-www-form-urlencoded");
//            request.AddParameter("application/x-www-form-urlendcoded",
//                $"CSRFName={CSRFName}&" +
//                $"CSRFToken={CSRFToken}&" +
//                $"reqURI=&" +
//                $"destination=&" +
//                $"tl_login={tlLogin}&" +
//                $"tl_password={tlPassword}&" +
//                $"login_submit=Войти", ParameterType.RequestBody);

//            ClientTl.Execute(request);

//            // Отправка результатов в TestLink
//            request = new RestRequest("/lib/results/resultsImport.php", Method.POST);

//            request.AddHeader("content-type", "multipart/form-data;boundary=----WebKitFormBoundary6Ghw83UDkbQ0jSRp");

//            StringBuilder sb = new StringBuilder();
//            sb.Append(AddParameter("CSRFName", CSRFName));
//            sb.Append(AddParameter("CSRFToken", CSRFToken));

//            sb.Append(AddParameter("importType", "XML"));
//            sb.Append(AddParameter("MAX_FILE_SIZE", "409600"));
//            sb.Append(AddParameter("buildID", buildId));
//            sb.Append(AddParameter("platformID", "0"));
//            sb.Append(AddParameter("tplanID", planId));
//            sb.Append(AddParameter("UploadFile", "Загрузить файл"));

//            sb.Append(AddFile(results));

//            sb.Append("------WebKitFormBoundary6Ghw83UDkbQ0jSRp--");

//            request.AddParameter("multipart/form-data;boundary=----WebKitFormBoundary6Ghw83UDkbQ0jSRp",
//                sb.ToString(), ParameterType.RequestBody);

//            ClientTl.Execute(request);
//        }

//        private static string AddParameter(string name, string value)
//        {
//            return $"------WebKitFormBoundary6Ghw83UDkbQ0jSRp\r\nContent-Disposition: form-data; name=\"{name}\"\r\n\r\n{value}\r\n";
//        }

//        private static string AddFile(string value)
//        {
//            return $"------WebKitFormBoundary6Ghw83UDkbQ0jSRp\r\nContent-Disposition: " +
//                $"form-data; name=\"uploadedFile\"; filename=\"TestsResults.xml\"\n" +
//                $"Content-Type: text/xml" +
//                $"\r\n\r\n{value}\n";
//        }

//    }
//}
#endregion
