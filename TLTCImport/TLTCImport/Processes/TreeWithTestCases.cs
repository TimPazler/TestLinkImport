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

        public Folder[] NamesFoldersAndfolders(int projectId)
        {           
            var Folder = GetArrayAllFolders(projectId);

            //var newArrFoldersAndfolders = RemoveEmptyArrayElements(Subfolder);

            //Исключить лишние
            //var testCasesForFolders = GetTestCasesForFolders(Folder);

            var testCasesForfolders = GetTestCasesForfolders(Folder);
            return testCasesForfolders;
        }

        //Удаление лишних элементов массива
        private Folder[] RemoveEmptyArrayElements(Folder[] arrFolders)
        {
            int count = 0;

            for (int i = 0; i < arrFolders.Length; i++)
            {
                if (arrFolders[i].folders.Length != 0)
                    count++;
            }

            Folder[] newArr = new Folder[count];

            for (int i = 0; i < newArr.Length; i++)
            {
                if (arrFolders[i] != null)
                    newArr[i] = arrFolders[i];
            }
            return newArr;
        }

        private Folder[] GetTestCasesForFolders(Folder[] folders)
        {
            try
            {
                InfoTestCase[] testCase;

                List<TestCaseFromTestSuite> testCaseAllInfo;
                foreach (var folder in folders)
                {
                    for (int i = 0; i < folder.folders.Length; i++)
                    {
                        var nameSubfolder = folder.nameFolder;
                        var idSubfolder = folder.idFolder;

                        testCaseAllInfo = testLinkApi.GetTestCasesForTestSuite(idSubfolder, true);
                        testCase = new InfoTestCase[testCaseAllInfo.Count];
                        int j = 0;
                        foreach (var testCaseInfo in testCaseAllInfo)
                        {
                            testCase[j] = new InfoTestCase(testCaseInfo.id, Int32.Parse(testCaseInfo.external_id), testCaseInfo.name);
                            j++;
                        }
                        folder.testCases = testCase;
                    }
                }
                return folders;
            }
            catch (System.Net.WebException)
            {
                //Повтор того что в try
            }
           
            return folders;
        }

        //тест кейсы
        private Folder[] GetTestCasesForfolders(Folder[] folders)
        {
            try
            {
                return GetTestCases(folders);
            }
            catch (System.Net.WebException)
            {
                return GetTestCases(folders);
            }
            catch (XmlRpcIllFormedXmlException)
            {
                return GetTestCases(folders);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Непредвиденная ошибка! Попробуйте перезапустить программу! \r\nИли обратитесь к разработчику! \r\n Ошибка: {e.Message}");
            }
            return folders;
        }
        private Folder[] GetTestCases(Folder[] folders)
        {
            InfoTestCase[] testCase;

            List<TestCaseFromTestSuite> testCaseAllInfo;
            foreach (var subfolder in folders)
            {
                for (int i = 0; i < subfolder.folders.Length; i++)
                {
                    var nameSubfolder = subfolder.folders[i].nameFolder;
                    var idSubfolder = subfolder.folders[i].idFolder;

                    testCaseAllInfo = testLinkApi.GetTestCasesForTestSuite(idSubfolder, true);
                    testCase = new InfoTestCase[testCaseAllInfo.Count];
                    int j = 0;
                    foreach (var testCaseInfo in testCaseAllInfo)
                    {
                        testCase[j] = new InfoTestCase(testCaseInfo.id, Int32.Parse(testCaseInfo.external_id), testCaseInfo.name);
                        j++;
                    }
                    subfolder.folders[i].testCases = testCase;
                }
            }
            return folders;
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
