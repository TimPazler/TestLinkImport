using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using TLTCImport.FolderStorageTestLink;

namespace TLTCImport
{
    //Переработка json и создании на его основе xml файла
    class JsonToXml
    {
        private string pathFile = "../../../Files/";      

        //Работает с файлом duration.json в папке widgets
        public Dictionary<string, string> GetDataJson(string nameFile, ref bool jsonFileCorrect, int projectId, string projectName,
            bool cbPassed, bool cbFailed, bool cbBlocked)
        {
            var valuesCases = new Dictionary<string, string>();

            var nameAndStatusTestCases = ReadJsonFile(nameFile);

            if (jsonFileCorrect)
            {
                for (int i = 0; i < nameAndStatusTestCases.Count; i++)
                {
                    //Обрезаем название кейса
                    string name = nameAndStatusTestCases[i].name.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    string status = nameAndStatusTestCases[i].status;

                    if (status == "passed")
                        status = "p";
                    else if (status == "failed" )
                        status = "f";
                    else if (status == "skipped")
                        status = "b";

                    CheckSameNameTestCase(valuesCases, name, status);
                    //valuesCases.Add(name, status);
                }
            }

            if (!(cbPassed == true && cbFailed == true && cbBlocked == true))
            {
                foreach (var value in valuesCases)
                {
                    if (cbPassed == false && value.Value == "p")
                        valuesCases.Remove(value.Key);
                    if (cbFailed == false && value.Value == "f")
                        valuesCases.Remove(value.Key);
                    if (cbBlocked == false && value.Value == "b")
                        valuesCases.Remove(value.Key);
                }
            }

            return valuesCases;
        }

        //Проверяем что Json файл корректный
        public bool JsonValidation(string nameFile)
        {
            try
            {
                var jsonFileContent = ReadJsonFile(nameFile);
                if (jsonFileContent != null)
                {
                    return true;
                }
                return false;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }

        //Проверяет кейсы с одинаковым именем
        public void CheckSameNameTestCase(Dictionary<string, string> valuesCases, string name, string status)
        {
            if (valuesCases.Count > 0)
            {
                for (int i = 0; i < valuesCases.Count; i++)
                {
                    //если в словаре нет строки с таким ключем и новое значение p
                    if (!valuesCases.ContainsKey(name) && status == "p")
                        valuesCases.Add(name, status);
                    else //если в словаре нет строки с таким ключем и новое значение f
                    if (!valuesCases.ContainsKey(name) && status == "f")
                        valuesCases.Add(name, status);
                    else //если в словаре нет строки с таким ключем и новое значение b
                    if (!valuesCases.ContainsKey(name) && status == "b")
                        valuesCases.Add(name, status);

                    //если в словаре есть строка с таким ключем, а новое значение false
                    else if (valuesCases.ContainsKey(name) && status == "f")
                    {
                        valuesCases.Remove(name);
                        valuesCases.Add(name, status);
                    }
                    //если в словаре есть строка с таким  ключем, новое значение skipped
                    //а старое значение не равно false
                    else if (valuesCases.ContainsKey(name) && !(valuesCases.ContainsValue("f")) && status == "b")
                    {
                        valuesCases.Remove(name);
                        valuesCases.Add(name, status);
                    }                   
                }
            }
            else
                valuesCases.Add(name, status);

        }

        //Читаем json файл
        private List<JenkinsResults> ReadJsonFile(string nameFile)
        {            
            if (File.Exists(pathFile + nameFile + ".json"))            
                return JsonConvert.DeserializeObject<List<JenkinsResults>>(File.ReadAllText(pathFile + nameFile + ".json"));                            
            return null;
        }

        //Преобразует полученные рез-ты из Json в XML
        //XML файл содержит в себе название тестплана и список тесткейсов
        public void FillXmlFile(string projectName, string testPlanName, int testPlanId, Dictionary<string, string> valuesCases, out int countValuesCases)
        {
            countValuesCases = valuesCases.Count();
            var buildName = TestLinkResult.GetBuildByTestPlanId(testPlanId);            
            var XmlDocument = new XDocument(new XElement("results"));

            XElement testproject = new XElement("testproject",
                                        new XAttribute("name", $"{projectName}"),
                                           new XAttribute("prefix", $"{projectName.ToLower()}"));
            XElement testplan = new XElement("testplan", new XAttribute("name", $"{testPlanName}"));
            XElement build = new XElement("build", new XAttribute("name", $"{buildName}"));

            XmlDocument.Root.Add(testproject);
            XmlDocument.Root.Add(testplan);
            XmlDocument.Root.Add(build);

            foreach (var item in valuesCases)
            {
                XmlDocument.Root.Add(new XElement("testcase", new XAttribute("external_id", $"{item.Key}"), new XElement("result", item.Value)));
            }
            
            XmlDocument.Save($@"{pathFile}\TestsResults.xml");
        }      
    }
}
