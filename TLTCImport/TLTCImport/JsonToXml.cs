using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace TLTCImport
{
    //Переработка json и создании на его основе xml файла
    class JsonToXml
    {
        private string pathFile = "../../../Files/";

        public Dictionary<string, string> GetDataJson(string nameFile, ref bool jsonFileCorrect, int projectId)
        {
            var valuesCases = new Dictionary<string, string>();

            string jsonFileContent = ReadJsonFile(nameFile);
                      
            //Перевод json файла в JObject
            JObject jsonListCase = JObject.Parse(jsonFileContent);

            //Не работает с json файлами ввиде массива. Отдельно проработать
            if (jsonFileCorrect)
            {
                int count = jsonListCase["children"].Count();
                for (int i = 0; i < count-1; i++)
                {
                    foreach (JToken data in jsonListCase["children"][i]["children"])
                    {                      
                        string name = "", status = "";
                                                
                        if (data.Value<JArray>("children") == null)
                        {
                            name = data["name"].ToString().Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0];
                            status = data["status"].ToString().Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        }
                        else if (data.Value<JArray>("children").Count == 1)
                        {
                            var dataNew = data.Value<JArray>("children");
                            name = dataNew[0]["name"].ToString().Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0];
                            status = dataNew[0]["status"].ToString().Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        }

                        if (status == "passed")
                            status = "p";
                        else if (status == "failed")
                            status = "f";
                        else if (status == "skipped")
                            status = "b";
                        valuesCases.Add(name, status);
                    }
                }
            }
            return valuesCases;
        }

        //Проверяем что Json файл корректный
        //Json файл ввиде массива данных, не поддерживается
        public bool JsonValidation(string nameFile)
        {
            try
            {
                string jsonFileContent = ReadJsonFile(nameFile);
                if (JObject.Parse(jsonFileContent) != null)
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

        //Читаем json файл
        private string ReadJsonFile(string nameFile)
        {
            if (File.Exists(pathFile + nameFile + ".json"))
            {
                FileStream file = File.OpenRead(pathFile + nameFile + ".json");
                byte[] buffer = new byte[file.Length];
                // считываем данные
                file.Read(buffer, 0, buffer.Length);
                // декодируем байты в строку
                return Encoding.Default.GetString(buffer);
            }
            return "Ошибка! Файла не существует! Попробуйте перезапустить программу!";
        }     

        //Преобразует полученные рез-ты из Json в XML
        //XML файл содержит в себе название тестплана и список тесткейсов
        public void FillXmlFile(string projectName, string testPlanName, int testPlanId, Dictionary<string, string> valuesCases, out int countValuesCases)
        {
            countValuesCases = valuesCases.Count();
            var buildName = TlReportTcResult.GetBuildByTestPlanId(testPlanId);            
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
