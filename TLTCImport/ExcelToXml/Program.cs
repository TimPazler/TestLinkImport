using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO.Packaging;
using System.Text.RegularExpressions;

namespace ExcelToXml
{
    class Program
    {
        public static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }
        public static XElement CreateTestcase(XDocument XmlDocument, string sheetName, string summary, string precondition, List<Tuple<string, string, string>> steps)
        {
            XElement stepsElement = new XElement("steps");
            for (int i = 0; i < steps.Count; i++)
            {
                stepsElement.Add(
                        new XElement("step",
                        new XElement("step_number", steps[i].Item1),
                        new XElement("actions", steps[i].Item2),
                        new XElement("expectedresults", steps[i].Item3)));
            }

            XElement testcase = new XElement("testcase", new XAttribute("name", $"{sheetName}"));
            testcase.Add(
                new XElement("summary", summary),
                new XElement("preconditions", precondition),
                stepsElement);
            return testcase;
        }
        static void Main(string[] args)
        {
            //Console.WriteLine("Введите путь к файлам:");
            //string path = Console.ReadLine();

            string AssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
            string path = AssemblyPath;
            var XmlDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
               new XElement("testsuite", new XAttribute("name", "ЭБ.Бюджетное планирование")));
            string[] xlsxFiles = Directory.GetFiles(path + "\\", "*.xlsx");
            string[] xlsFiles = Directory.GetFiles(path + "\\", "*.xls");
            var files = xlsxFiles.Concat(xlsFiles).Distinct().ToArray();
            foreach (var file in files)
            {
                string filename = null;
                if (file.Contains(".xlsx"))
                {
                    Regex reg = new Regex($@"({path.Replace("\\", "\\\\")}\\)(.+).xlsx");
                    Match _match = reg.Match(file);
                     filename = _match.Groups[2].Value;
                }
                else if (file.Contains(".xls"))
                {
                    Regex reg = new Regex($@"({path.Replace("\\", "\\\\")}\\)(.+).xls");
                    Match _match = reg.Match(file);
                     filename = _match.Groups[2].Value;
                }

                XElement testsuite = new XElement("testsuite", new XAttribute("name", $"{filename}"));
                using (var document = SpreadsheetDocument.Open(file, false))
                {
                    var workbookPart = document.WorkbookPart;
                    var workbook = workbookPart.Workbook;

                    var sheets = workbook.Descendants<Sheet>();
                    foreach (var sheet in sheets)
                    {
                        List<string> values = new List<string>();
                        List<Tuple<string, string, string>> steps = new List<Tuple<string, string, string>>();
                        string registry = null;
                        string summary = null;
                        string precondition = null;

                        string stepnum = null;
                        string action = null;
                        string result = null;

                        var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                        var sharedStringPart = workbookPart.SharedStringTablePart;

                        var rows = worksheetPart.Worksheet.Descendants<Row>();
                        int rowCount = -1;
                        foreach (var row in rows)
                        {
                            rowCount++;
                            if (rowCount == 1 || rowCount == 3 || rowCount == 4)
                                continue;
                            if (rowCount == 0)
                            {
                                foreach (Cell c in row.Elements<Cell>())
                                {
                                    int id = -1;
                                    if (Int32.TryParse(c.InnerText, out id))
                                    {
                                        string cellValue = null;
                                        SharedStringItem item = GetSharedStringItemById(workbookPart, id);
                                        if (item.Text != null)
                                        {
                                            cellValue = item.Text.Text;
                                        }
                                        else if (item.InnerText != null)
                                        {
                                            cellValue = item.InnerText;
                                        }
                                        else if (item.InnerXml != null)
                                        {
                                            cellValue = item.InnerXml;
                                        }
                                        Regex regex = new Regex(@"(#.+)");
                                        Match match = regex.Match(cellValue);
                                        registry = match.Groups[0].Value;
                                    }
                                }
                            }
                            if (rowCount == 2)
                            {
                                int cellNum = 0;
                                foreach (Cell c in row.Elements<Cell>())
                                {
                                    int id = -1;
                                    if (Int32.TryParse(c.InnerText, out id))
                                    {
                                        SharedStringItem item = GetSharedStringItemById(workbookPart, id);
                                        if (cellNum == 0)
                                            summary = item.Text.Text;
                                        else if (cellNum == 2)
                                            precondition = item.InnerText + registry;
                                    }
                                    cellNum++;
                                }
                            }

                            if (rowCount > 4)
                                foreach (Cell c in row.Elements<Cell>())
                                {
                                    int id = -1;
                                    if (Int32.TryParse(c.InnerText, out id))
                                    {
                                        SharedStringItem item = GetSharedStringItemById(workbookPart, id);
                                        if (c.CellReference.Value.Contains("A") && rowCount > 4)
                                        {
                                            stepnum = c.CellValue.Text;
                                        }
                                        else if (c.CellReference.Value.Contains("B") && rowCount > 4)
                                        {
                                            string cellValue = null;
                                            if (item.Text != null)
                                            {
                                                cellValue = item.Text.Text;
                                            }
                                            else if (item.InnerText != null)
                                            {
                                                cellValue = item.InnerText;
                                            }
                                            else if (item.InnerXml != null)
                                            {
                                                cellValue = item.InnerXml;
                                            }

                                            if (stepnum != null)
                                                action = cellValue;
                                            else
                                                action += Environment.NewLine + cellValue;
                                        }
                                        else if (c.CellReference.Value.Contains("C") && rowCount > 4)
                                        {
                                            if (stepnum != null)
                                                result = item.InnerText;
                                            else
                                                result += Environment.NewLine + item.InnerText;
                                        }
                                    }
                                }
                            if (stepnum != null && action != null && result != null)
                                steps.Add(new Tuple<string, string, string>(stepnum, action, result));

                            if (stepnum == null && (action != null || result != null))
                            {
                                (string, string, string) tuple = steps.Last().ToValueTuple();
                                steps.Remove(steps.Last());

                                if (action != null)
                                {
                                    tuple.Item2 = action;
                                }
                                if (result != null)
                                {
                                    tuple.Item3 = result;
                                }
                                steps.Add(new Tuple<string, string, string>(tuple.Item1, tuple.Item2, tuple.Item3));
                            }
                            stepnum = null;
                        }
                        testsuite.Add(CreateTestcase(XmlDocument, sheet.Name, summary, precondition, steps));
                    }
                }
                XmlDocument.Element("testsuite").Add(testsuite);
            }
            string importpath = path + @"\Import";
            System.IO.Directory.CreateDirectory(importpath);
            string xmlFile = "FileToImport.xml";
            XmlDocument.Save($@"{importpath}\{xmlFile}");
            Console.WriteLine($"Файл {xmlFile} сохранен; Путь: {importpath}");
            Console.ReadKey();

        }
    }
}
