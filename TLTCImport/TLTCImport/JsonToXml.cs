using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TLTCImport
{
    //Переработка json и создании на его основе xml файла
    class JsonToXml
    {
        private string pathJsonFile = "../../../JsonFiles/";

        private async Task<string> WriteJsonFile(string nameFile)
        {
            if (File.Exists(pathJsonFile + nameFile + ".json"))
            {
                FileStream file = File.OpenRead(pathJsonFile + nameFile + ".json");
                byte[] buffer = new byte[file.Length];
                // считываем данные
                await file.ReadAsync(buffer, 0, buffer.Length);
                // декодируем байты в строку
                return Encoding.Default.GetString(buffer);
                //MessageBox.Show($"Текст из файла: {textFromFile}");
            }
            return "Ошибка! Файла не существует! Попробуйте перезапустить программу!";
        }

        public async void TransformJsonToXml(string nameFile)
        {
            string result = await WriteJsonFile(nameFile);

            XmlDocument xml = JsonConvert.DeserializeXmlNode(result, "results");
            xml.Save(pathJsonFile + nameFile + ".xml");
        }
    }
}
