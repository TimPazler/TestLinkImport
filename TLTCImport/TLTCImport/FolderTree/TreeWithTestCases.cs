using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TestLinkApi;
using TLTCImport.FolderStorageTestLink;

namespace TLTCImport
{
    //Класс для работы с деревом, состоящим из папок и тесткейсов
    public class TreeWithTestCases
    {
        private TestLink testLinkApi = TestLinkResult.testLinkApi;

        //Заполнение массива папками и тесткейсами по проекту
        //Можно выбрать тольео те кейсы которые есть в тест плане
        //displayCasesAccordingTestPlan = true отвечает за выбор кейсов в тест плане
        public Folder[] FillArrayWithData(int projectId, int testPlanId, string projectName, bool displayCasesAccordingTestPlan = true)
        {
            string prefixName = TestLinkResult.GetPrefixProjectByName(projectName);
            Dictionary<string, int> allTestCasesTestPlan = new Dictionary<string, int>();
            
            //Получение кейсов по тест плану
            if (displayCasesAccordingTestPlan == true)            
                allTestCasesTestPlan = TestLinkResult.GetTestCasesToTestPlan(testPlanId, projectName);            

            //Сначала массив заполняем только папками продукта
            var folder = GetArrayAllFolders(projectId);
            //Потом вставляем все кейсы продукта
            var folders = GetTestCasesForFolders(folder);

            //Для переработки массива и отображения только тех кейсов, которые есть в тест плане
            if (displayCasesAccordingTestPlan == true)            
                ArrayTestsCasesByTestPlan(folders, allTestCasesTestPlan, prefixName);
            
            return folders;
        }        

        //Получение всех тест кейсов (заполнение массива только тест кейсами)
        private Folder[] GetTestCasesForFolders(Folder[] folders)
        {
            try
            {
                return GetArrayAllTestCasesInFolders(folders);
            }
            catch (System.Net.WebException)
            {
                return GetArrayAllTestCasesInFolders(folders);
            }
            catch (XmlRpcIllFormedXmlException)
            {
                return GetArrayAllTestCasesInFolders(folders);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Непредвиденная ошибка! Попробуйте перезапустить программу! \r\nИли обратитесь к разработчику! \r\n Ошибка: {e.Message}");
            }
            return folders;
        }

        //Рекурсия
        //Перезаполнение массива, в котором содержаться только те кейсы, которые есть в тест плане
        //Пустые папки удаляются из массива
        private Folder[] ArrayTestsCasesByTestPlan(Folder[] folders, Dictionary<string, int> allTestCasesTestPlan, string prefixName)
        {
            foreach (var folder in folders)
            {
                Dictionary<string, int> testCasesField = new Dictionary<string, int>();

                if (folder.testCases != null)
                {
                    //Получение тест кейсов по папке первого уровня 
                    foreach (var testCase in folder.testCases)
                    {
                        if(testCase != null)
                            testCasesField.Add(prefixName + "-" + testCase.externalIdTestCase, testCase.idTestCase);
                    }

                    //Если в тест плане нет тест кейса, такого же, который есть в проекте, то удаляем кейс из папки
                    var newTestCasesField = DictionaryWithCasesForOneFoldersBasedOnTestPlan(folder, allTestCasesTestPlan, testCasesField);

                    //Проверка что в папке есть кейсы
                    FillArrayWithCasesBasedOnTestPlan(folder, newTestCasesField);
                }

                //Проходим подпапки
                ArrayTestsCasesByTestPlan(folder.folders, allTestCasesTestPlan, prefixName);
            }
            return folders;
        }

        //Возвращаем новый словарь, содержащий кейсы, которые есть в папке и тест плане
        private Dictionary<string, int> DictionaryWithCasesForOneFoldersBasedOnTestPlan(Folder folder, Dictionary<string, int> allTestCasesTestPlan, Dictionary<string, int> testCasesField)
        {
            Dictionary<string, int> newTestCasesField = new Dictionary<string, int>();

            var numberCasesViewedInField = 0;
            //Просмотр тест кейсов в тест плане
            foreach (var testCaseTestPlan in allTestCasesTestPlan)
            {
                //Просмотр тест кейсов в папки
                foreach (var testCaseField in testCasesField)
                {
                    //Если в тест плане есть такой же тест кейс, который есть в проекте                    
                    if (testCaseTestPlan.Key == testCaseField.Key)
                    {
                        numberCasesViewedInField++;
                        newTestCasesField.Add(testCaseField.Key, testCaseField.Value);
                        break;
                    }
                }
                //если в папка все тест кейсы относятся к тест плану, то
                //прекращаем поиск
                if (numberCasesViewedInField == testCasesField.Count)
                    break;
            }

            //Возвращаем новый словарь, содержащий кейсы, которые есть в папке и тест плане
            return newTestCasesField;
        }

        //Удаление всего массива тест кейсов или частично, если есть пустые элементы
        private void FillArrayWithCasesBasedOnTestPlan(Folder folder, Dictionary<string, int> newTestCasesField)
        {
            List<InfoTestCase> newTestCases = new List<InfoTestCase>();

            //если кейсов в словаре нет
            if (newTestCasesField.Count == 0)
                folder.testCases = null;
            //если есть кейсы в словаре, то убираем из папки те тесты, 
            //которых нет в словаре
            else if (newTestCasesField.Count > 0)
            {
                int elementArr = 0;
                foreach (var testCaseFolder in folder.testCases)
                {
                    if (testCaseFolder != null)
                    {
                        var numberCasesViewedInField = 0;
                        foreach (var newTestCaseField in newTestCasesField)
                        {
                            //Если в тест плане есть такой же тест кейс, который есть в проекте                    
                            if (testCaseFolder.idTestCase == newTestCaseField.Value)
                            {
                                newTestCases.Add(testCaseFolder);
                                elementArr++;
                                numberCasesViewedInField++;
                                break;
                            }
                        }

                        //Если кейса нет в словаре, то очищаем его из папки
                        if (numberCasesViewedInField == 0)
                            folder.testCases[elementArr] = null;
                    }
                }
            }

            //Перенос кейсов из массива в папку
            folder.testCases = newTestCases.ToArray();
        }        

        //Получить массив всех тесткейсов в папках
        private Folder[] GetArrayAllTestCasesInFolders(Folder[] folders)
        {
            InfoTestCase[] testCase;

            List<TestCaseFromTestSuite> testCaseAllInfo;

            //Сначала заполняем подпапки
            FillArrayAllTestCasesInSubfolders(folders);

            //Потом папки
            foreach (var folder in folders)
            {
                var nameFolder = folder.nameFolder;
                var idFolder = folder.idFolder;

                testCaseAllInfo = testLinkApi.GetTestCasesForTestSuite(idFolder, true);

                testCase = new InfoTestCase[testCaseAllInfo.Count];
                int j = 0;
                foreach (var testCaseInfo in testCaseAllInfo)
                {
                    //Проверка, что в подпапках такого кейса нет                   
                    if (TestCaseExistsInSubfolder(folder.folders, testCaseInfo.name))
                    {
                        testCase[j] = new InfoTestCase(testCaseInfo.id, Int32.Parse(testCaseInfo.external_id), testCaseInfo.name);                        
                        j++;
                    }
                }

                //Проверка массива на пустоту
                testCase = CheckArrayThatEmpty(testCase);

                folder.testCases = testCase;
            }
            return folders;
        }

        //Проверка массива на пустоту
        private InfoTestCase[] CheckArrayThatEmpty(InfoTestCase[] testCase)
        {
            int countCases = 0;
            foreach (var tCase in testCase)
            {
                if (tCase == null)
                    countCases++;
            }

            if (countCases == testCase.Length)
                testCase = null;
            return testCase;
        }

        //Рекурсия
        //Проверка, что в подпапках нет кейса, который есть в папке
        //иначе кейс не добавляем в папку
        private bool TestCaseExistsInSubfolder(Folder[] folders, string nameTestCase)
        {
            foreach (var folder in folders)
            {
                foreach (var testCase in folder.testCases)
                {
                    if (testCase != null)
                    {
                        if (nameTestCase == testCase.nameTestCase)
                        {
                            return false;
                        }
                    }
                }
                TestCaseExistsInSubfolder(folder.folders, nameTestCase);
            }
            return true;
        }

        //Рекурсия
        //Заполнить массив тесткейсов в подпапки
        private void FillArrayAllTestCasesInSubfolders(Folder[] folders)
        {
            foreach (var folder in folders)
            {
                var nameFolder = folder.nameFolder;
                var idFolder = folder.idFolder;

                //баг CookComputing.XmlRpc.XmlRpcIllFormedXmlException: "Response from server does not contain valid XML."
                var testCaseAllInfo = testLinkApi.GetTestCasesForTestSuite(idFolder, true);
                folder.testCases = new InfoTestCase[testCaseAllInfo.Count];

                int j = 0;
                foreach (var testCaseInfo in testCaseAllInfo)
                {
                    folder.testCases[j] = new InfoTestCase(testCaseInfo.id, Int32.Parse(testCaseInfo.external_id), testCaseInfo.name);
                    j++;
                }
                FillArrayAllTestCasesInSubfolders(folder.folders);
            }
        }
       
        //Получить массив всех папок
        private Folder[] GetArrayAllFolders(int projectId)
        {
            List<TestSuite> suites = testLinkApi.GetFirstLevelTestSuitesForTestProject(projectId);
            Folder[] folders = new Folder[suites.Count];

            int i = 0;
            foreach (var suite in suites)
            {
                folders[i] = new Folder(suite.id, suite.name);
                i++;
            }

            FillArraySubfolders(folders);

            return folders;
        }       

        //Рекурсия
        //Заполнить массив подпапками
        private void FillArraySubfolders(Folder[] folders)
        {
            foreach (var folder in folders)
            {
                var suites = testLinkApi.GetTestSuitesForTestSuite(folder.idFolder);
                folder.folders = new Folder[suites.Count];

                int j = 0;
                foreach (var suite in suites)
                {
                    folder.folders[j] = new Folder(suite.id, suite.name);
                    j++;
                }
                FillArraySubfolders(folder.folders);
            }
        }       
    }
}
