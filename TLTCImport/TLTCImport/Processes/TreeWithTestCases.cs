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

        //Заполнение массива папками и тесткейсами
        public Folder[] FillArrayWithData(int projectId)
        {           
            var folder = GetArrayAllFolders(projectId);         
            var testCasesForfolders = GetTestCasesForfolders(folder);
            return testCasesForfolders;
        }      

        //Получение всех тест кейсов (заполнение массива только тест кейсами)
        private Folder[] GetTestCasesForfolders(Folder[] folders)
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
