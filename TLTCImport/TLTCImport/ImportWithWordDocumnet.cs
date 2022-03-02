using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Drawing;

namespace TLTCImport
{
    public class ImportWithWordDocumnet
    {
        private Word.Application wordApplication;

        Dictionary<string, List<string>> dictionaryTestCasesWithSteps = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> dictionaryTestCasesWithExpectedResults = new Dictionary<string, List<string>>();
        Dictionary<string, string> dictionaryTestCasesWithBCInfo = new Dictionary<string, string>();

        List<string> listForBadTestCases = new List<string>();
        List<string> listForTestCasesWithBC = new List<string>();
        List<string> listForTestCasesWithImages = new List<string>();

        public void ReadAndWrightTestCasesOutWordDocument()
        {
            wordApplication = new Word.Application();
            wordApplication.Visible = false;

            var tempDocument = wordApplication.Documents.Open("C:\\Anoth\\WordForImport\\TestMap.docx");
            int countTables = tempDocument.Tables.Count;
            int countPriorityTable = 4;
            //16 таблица это 48 страница
            //ТАБЛИЦУ ПРИОРИТЕТОВ ИГНОРИМ!!!! ЭТО 4 ПОСЛЕДНИЕ ТАБЛИЦЫ

            int countRows, countColumns;

            for (int tableNumber = 1; tableNumber <= countTables - countPriorityTable; tableNumber++)
            {
                countRows = tempDocument.Tables[tableNumber].Rows.Count;
                countColumns = tempDocument.Tables[tableNumber].Columns.Count;

                System.Diagnostics.Debug.WriteLine("\nОбработка таблицы №" + tableNumber + "\nКоличество строк = " + countRows + "\nКоличество столбцов = " + countColumns + "\n");


                //Определение того, какой столбец является для реестров, БЦ и этапов, для шагов и для ожидаемого результата
                int ce1 = -1; //Реестры
                int ce2 = -1; //БЦ и этап
                int ce3 = -1; //Шаги
                int ce4 = -1; //Ожидаемый результат

                for (int i = 1; i <= countColumns; i++)
                {
                    string columnName = tempDocument.Tables[tableNumber].Cell(1, i).Range.Text;

                    if (columnName.Contains("Пункт") || columnName.Contains("Реестр")) ce1 = i;

                    if (columnName.Contains("БЦ")) ce2 = i;

                    if (columnName.Contains("Дейст")) ce3 = i;

                    if (columnName.Contains("Ожидаем")) ce4 = i;
                }

                if (ce1 == -1 || ce3 == -1)
                {
                    System.Diagnostics.Debug.WriteLine("\n!!!\nНе получилось определить номера главных ячеек (будут установлены по умолчанию 1 и 2)\nce1 = " + ce1 + "\nce3 = " + ce3 + "\n!!!");

                    if (ce1 == -1) ce1 = 1;
                    if (ce3 == -1) ce3 = 2;
                }



                //Проход в цикле по строкам ячейки
                for (int i = 2; i <= countRows; i++)
                {
                    //Определение названия реестра и удаление лишних символов
                    string registryName = tempDocument.Tables[tableNumber].Cell(i, ce1).Range.Text;
                    registryName = registryName.Replace("#", "");
                    registryName = registryName.Replace("", "");
                    registryName = registryName.Replace("", "");
                    registryName = registryName.Replace("\r", "");

                    //Если в списке уже есть расматриваемый реестр или если в ячейке таблицы он вообще не указан, то скипаем
                    if (dictionaryTestCasesWithSteps.ContainsKey(registryName) || string.IsNullOrEmpty(registryName) || registryName.Length < 2)
                    {
                        System.Diagnostics.Debug.WriteLine($"В словаре уже имеется реестр {registryName}! (или он пустой)");
                        continue;
                    }

                    System.Diagnostics.Debug.WriteLine("Начата обработка реестра " + registryName);

                    //Определение БЦ и этапа для условий кейса
                    string BCInfo = null;
                    if (ce2 != -1)
                    {
                        BCInfo = tempDocument.Tables[tableNumber].Cell(i, ce2).Range.Text;

                        //Удаление недопустимых символов в строке
                        BCInfo = BCInfo.Replace("", "");
                        BCInfo = BCInfo.Replace("", "");


                        if (BCInfo.Length < 3)
                            BCInfo = null;
                        else
                        {
                            //Разделение условий
                            BCInfo = BCInfo.Replace("БЦ", "<br><br/>БЦ");
                            if (BCInfo[0] == '<')
                                BCInfo = BCInfo.Remove(0, "<br><br/>".Length);
                        }
                    }


                    //Начало разбора действий на шаги
                    List<string> stepsList = new List<string>();
                    string cellValue = tempDocument.Tables[tableNumber].Cell(i, ce3).Range.Text;


                    List<string> exResultsList = null;
                    string cellExResultsValue = null;
                    if (ce4 != -1)
                    {
                        exResultsList = new List<string>();
                        cellExResultsValue = tempDocument.Tables[tableNumber].Cell(i, ce4).Range.Text;
                    }

                    string tempStepValue = "";
                    int currentStepNumber = 1;
                    bool startStepForBC = false;


                    //Определение типа перечисления шагов - вида 1) или вида 1.
                    string strForSearchStep = ")";
                    if (cellValue.IndexOf(currentStepNumber + strForSearchStep) == -1) strForSearchStep = ".";

                    if (cellValue[0] != '1')
                    {
                        System.Diagnostics.Debug.WriteLine($"В начале нет номера шага\nКейс будет помечен, реестр = {registryName}");
                        listForBadTestCases.Add(registryName);
                    }



                    //Разбиение содержимого ячейки на шаги в цикле
                    while (cellValue.Length != 0)
                    {
                        //Картинки еще


                        int startStepIndex = 0;//cellValue.IndexOf($"{currentStepNumber})");
                        int nextStartStepIndex = cellValue.IndexOf($"{currentStepNumber + 1}" + strForSearchStep);

                        if (listForBadTestCases.Contains(registryName))
                        {
                            if (cellValue[0] == '\r') cellValue = cellValue.Remove(0, 1);
                            nextStartStepIndex = cellValue.IndexOf("\r");
                        }


                        if (nextStartStepIndex != -1)
                        {
                            if (strForSearchStep == "." && nextStartStepIndex + 2 < cellValue.Length)
                            {
                                //Проверка на то, находится ли после символа перечисления шага число, если да, то значит, что подпункт, например 2.1
                                //Тогда либо - в данной строке некорректное обозначение шагов
                                //Либо главных шагов уже больше нет и этот последний
                                while (cellValue[nextStartStepIndex + 2] >= '0' && cellValue[nextStartStepIndex + 2] <= '9')
                                {
                                    //Попытка найти шаг после подпункта
                                    nextStartStepIndex = cellValue.IndexOf($"{currentStepNumber + 1}" + strForSearchStep, nextStartStepIndex + 1);

                                    if (nextStartStepIndex == -1)
                                    {
                                        nextStartStepIndex = cellValue.Length;
                                        break;
                                    }
                                }
                            }
                            else if (strForSearchStep == ")" && nextStartStepIndex - 2 >= 0)
                            {
                                //Проверка на то, находится ли до символа перечисления шага число, если да, то значит, что подпункт, например 2.1
                                //Тогда либо - в данной строке некорректное обозначение шагов
                                //Либо главных шагов уже больше нет и этот последний
                                while (cellValue[nextStartStepIndex - 2] >= '0' && cellValue[nextStartStepIndex - 2] <= '9')
                                {
                                    //Попытка найти шаг после подпункта
                                    nextStartStepIndex = cellValue.IndexOf($"{currentStepNumber + 1}" + strForSearchStep, nextStartStepIndex + 1);

                                    if (nextStartStepIndex == -1)
                                    {
                                        nextStartStepIndex = cellValue.Length;
                                        break;
                                    }
                                }
                            }
                        }
                        else //Если начало след шага не найдено
                        {
                            /**
                            //Значит следующего шага нет, казалось бы легко все
                            //Но не тут-то было! ведь возможна ситуация, что есть пометка для БЦ
                            //и там есть шаги после нее 
                            //+
                            //Может быть так, что там будет шаг следующий! Например, 2ой, но не в рамках главной последовательности, а уже в рамках как раз БЦ!
                            //Но с этим ничего не поделать да и ни 1 такого случая обнаружено не было
                            **/

                            //То попытка найти начало строки по типу Для БЦ 2021-2023
                            int startIndexForBC = cellValue.IndexOf($"ля БЦ 20");

                            if (startIndexForBC == -1) //Если такая не найдена, то в качестве начала след шага устанавливается длина остатка данной строки (предполагается, что весь текст какой остался - это шаг)
                            {
                                nextStartStepIndex = cellValue.Length;
                            }
                            else //Если строка с БЦ найдена, то
                            {
                                //1 условие - Если после строки с БЦ нет шага под номером 1, то предполагается что эта часть строки с БЦ в контексте текущего шага, поэтому началом след шага будет считаться конец всей оставшейся ячейки
                                //2 условие - если true, то значит, что на предыдущей итерации была ситуация, когда был найден последний шаг и была найдена строка БЦ после которой есть 1ый шаг - поэтому на данной итерации строка с БЦ и все шаги после нее - это шаг
                                //3 условие - Ситуация, когда изначально в ячейке всего 1 шаг и в нем есть срока с БЦ (т.е. номер текущей строки как раз и есть 1ый шаг)
                                if (cellValue.IndexOf("1" + strForSearchStep) == -1 || startStepForBC || currentStepNumber == 1)
                                {
                                    nextStartStepIndex = cellValue.Length;
                                }
                                //Иначе же - предполагается, что начало следующего шага - это та самая строка с БЦ
                                else
                                {
                                    nextStartStepIndex = startIndexForBC - 1; //Минус 1 - т.к. буква Д при поиске не учитывалась и может быть разных размеров

                                    startStepForBC = true; // И пометка начала режима, когда следующие шаги будут в рамках строки по типу Для БЦ 2021-2023

                                    listForTestCasesWithBC.Add(registryName);
                                }
                            }
                        }

                        //УДАЛЕНИЕ ДОБАВЛЕННОГО ШАГА ИЗ ЗНАЧЕНИЯ ЯЧЕЙКИ
                        tempStepValue = cellValue.Substring(startStepIndex, nextStartStepIndex);

                        cellValue = cellValue.Remove(0, tempStepValue.Length);


                        //ДОБАВЛЕНИЕ ШАГА В СПИСОК
                        //Удаление номера шага из самого шага
                        string tempForRemove = currentStepNumber + strForSearchStep;
                        if (tempStepValue.IndexOf(tempForRemove) == 0)
                        {
                            if (tempStepValue[tempForRemove.Length] == ' ')
                                tempStepValue = tempStepValue.Remove(0, tempForRemove.Length + 1);
                            else
                                tempStepValue = tempStepValue.Remove(0, tempForRemove.Length);
                        }

                        //Удаление недопустимого символов в шаге
                        tempStepValue = tempStepValue.Replace("", "");
                        tempStepValue = tempStepValue.Replace("", "");
                        //tempStepValue = tempStepValue.Replace("\a", "");

                        //Добавление шага в список
                        System.Diagnostics.Debug.WriteLine($"\nSTEP({currentStepNumber})\n" + tempStepValue);
                        stepsList.Add(tempStepValue);



                        //Ожидаемый результат
                        if (ce4 != -1)
                        {
                            int startExResIndex = 0;
                            int nextStartExResIndex = cellExResultsValue.IndexOf($"{currentStepNumber + 1}" + strForSearchStep);

                            if (nextStartExResIndex == -1) nextStartExResIndex = cellExResultsValue.Length;

                            //УДАЛЕНИЕ ДОБАВЛЕННОГО ОЖ.РЕЗУЛЬТАТА ИЗ ЗНАЧЕНИЯ ЯЧЕЙКИ
                            string tempExResValue = cellExResultsValue.Substring(startExResIndex, nextStartExResIndex);

                            cellExResultsValue = cellExResultsValue.Remove(0, tempExResValue.Length);

                            //ДОБАВЛЕНИЕ ОЖ.РЕЗУЛЬТАТА В СПИСОК
                            //Удаление номера ож. результата
                            if (tempExResValue.IndexOf(tempForRemove) == 0)
                            {
                                if (tempExResValue[tempForRemove.Length] == ' ')
                                    tempExResValue = tempExResValue.Remove(0, tempForRemove.Length + 1);
                                else
                                    tempExResValue = tempExResValue.Remove(0, tempForRemove.Length);
                            }

                            //Удаление недопустимого символов в ож. результате
                            tempExResValue = tempExResValue.Replace("", "");
                            tempExResValue = tempExResValue.Replace("", "");

                            //Добавление шага в список
                            exResultsList.Add(tempExResValue);
                        }


                        currentStepNumber++;
                    }


                    //Установка ожидаемого результата
                    string expectedResult = null;
                    if (ce4 != -1) expectedResult = tempDocument.Tables[tableNumber].Cell(i, ce4).Range.Text;


                    //Проверка наличия изображений в ячейке
                    if (tempDocument.Tables[tableNumber].Cell(i, ce3).Range.InlineShapes.Count > 0)
                        listForTestCasesWithImages.Add(registryName);


                    //Добавление реестра с его шагами в словарь
                    dictionaryTestCasesWithSteps.Add(registryName, stepsList);

                    //Добавление реестра с его условиями в словарь
                    dictionaryTestCasesWithBCInfo.Add(registryName, BCInfo);

                    //Добавление реестра с его ожидаемыми результатми в словарь
                    dictionaryTestCasesWithExpectedResults.Add(registryName, exResultsList);
                }

            }

            tempDocument.Close();
            wordApplication.Quit();
            wordApplication = null;
        }

        public XElement GetXmlElementForRegistry(string testName)
        {
            XElement testCaseEl = null;

            if (dictionaryTestCasesWithSteps.ContainsKey(testName))
            {
                string newTestName = testName;
                if (listForTestCasesWithBC.Contains(testName)) newTestName = "_haveBC_" + newTestName;
                if (listForBadTestCases.Contains(testName)) newTestName = "_bad_" + newTestName;
                if (listForTestCasesWithImages.Contains(testName)) newTestName = "_haveImg_" + newTestName;

                testCaseEl = new XElement("testcase", new XAttribute("name", newTestName));

                testCaseEl = EnterValuesInXElementForTestCase(testCaseEl, testName);

                //Удаление реестра из словаря
                dictionaryTestCasesWithSteps.Remove(testName);
            }

            return testCaseEl;
        }

        public List<XElement> GetXmlElementForAnothRegistry()
        {
            List<XElement> tempXmlElementsList = new List<XElement>();

            foreach (var tempRegistry in dictionaryTestCasesWithSteps)
            {
                string testName = tempRegistry.Key;

                string newTestName = testName;
                if (listForTestCasesWithBC.Contains(testName)) newTestName = "_beBC_" + newTestName;
                if (listForBadTestCases.Contains(testName)) newTestName = "_bad_" + newTestName;
                if (listForTestCasesWithImages.Contains(testName)) newTestName = "_haveImg_" + newTestName;

                XElement testCaseEl = new XElement("testcase", new XAttribute("name", newTestName));

                testCaseEl = EnterValuesInXElementForTestCase(testCaseEl, testName);

                tempXmlElementsList.Add(testCaseEl);
            }

            return tempXmlElementsList;
        }

        public XElement EnterValuesInXElementForTestCase(XElement testCaseEl, string testName)
        {
            XElement newTestCaseEl = new XElement(testCaseEl);


            if (!string.IsNullOrEmpty(dictionaryTestCasesWithBCInfo[testName]))
            {
                newTestCaseEl.Add(new XElement("preconditions", dictionaryTestCasesWithBCInfo[testName]));
            }

            XElement stepsEl = new XElement("steps");
            List<string> tempStepsList = dictionaryTestCasesWithSteps[testName];
            List<string> tempExpectedResultsList = dictionaryTestCasesWithExpectedResults[testName];

            for (int i = 0; i < tempStepsList.Count; i++)
            {
                XElement tempStepEl = new XElement("step");

                XElement stepNumberEl = new XElement("step_number");
                stepNumberEl.Add(i + 1);

                //Действие
                XElement actionsEl = new XElement("actions");
                actionsEl.Add(tempStepsList[i]);

                //Ож. результат
                if (tempExpectedResultsList != null && !string.IsNullOrEmpty(tempExpectedResultsList[i]))
                {
                    XElement expectedresultsEl = new XElement("expectedresults");
                    expectedresultsEl.Add(tempExpectedResultsList[i]);

                    tempStepEl.Add(expectedresultsEl);
                }

                tempStepEl.Add(stepNumberEl);
                tempStepEl.Add(actionsEl);

                stepsEl.Add(tempStepEl);
            }

            newTestCaseEl.Add(stepsEl);


            return newTestCaseEl;
        }
    }
}
