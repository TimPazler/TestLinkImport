using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TestLinkApi;

namespace TLTCImport
{
    //Класс для работы с деревов, состоящим из папок и тесткейсов
    public class TreeWithTestCases
    {
        private TestLink testLinkApi = TestLinkResult.testLinkApi;
        List<TestSuite> suitesIdFirst;

        //public DictionaryForWorkWithFolders NamesFoldersAndSubfolders(int projectId)
        //{
        //    var firstLevelFolder = GetFirstLevelFolder(projectId);
        //    var secondLevelFolder = GetSecondLevelFolder(GetFirstLevelFolder(projectId));
        //    return DeleteFoldersWithEmptySubfolders(firstLevelFolder, secondLevelFolder);
        //}

        public DictionaryForWorkWithFolders NamesFoldersAndSubfolders(int projectId)
        {
            var firstLevelFolder = GetFirstLevelFolder(projectId);
            var secondLevelFolder = GetSecondLevelFolder(GetFirstLevelFolder(projectId));
            var a = DeleteFoldersWithEmptySubfolders(firstLevelFolder, secondLevelFolder);
            return GetAllTestCase(a);
        }

        //Получить имена папок первого уровня и их id
        private Dictionary<int, string> GetFirstLevelFolder(int projectId)
        {
            suitesIdFirst = testLinkApi.GetFirstLevelTestSuitesForTestProject(projectId);
            Dictionary<int, string> IdAndNameFolder = new Dictionary<int, string>();

            foreach (var suite in suitesIdFirst) 
                IdAndNameFolder.Add(suite.id, suite.name);                     

            return IdAndNameFolder;
        }

        ////Получить имена папок второго уровня
        //private string[][] GetSecondLevelFolder(Dictionary<int, string> IdAndNameFolder)
        //{
        //    int i = 0;
        //    string[][] subfolders = new string[IdAndNameFolder.Count][];         
        //    List<TestSuite> suitesIdFirst;
        //    foreach (var testSuiteId in IdAndNameFolder.Keys)
        //    {
        //        suitesIdFirst = testLinkApi.GetTestSuitesForTestSuite(testSuiteId);
        //        int j = 0;
        //        subfolders[i] = new string[suitesIdFirst.Count];
        //        foreach (var suite in suitesIdFirst)
        //        {
        //            subfolders[i][j] = suite.name;                    
        //            j++;
        //        }
        //        i++;
        //    }
        //    return subfolders;
        //}

          //Получить имена папок второго уровня
        private string[][] GetSecondLevelFolder(Dictionary<int, string> IdAndNameFolder)
        {
            int i = 0;
            string[][] subfolders = new string[IdAndNameFolder.Count][];         
            List<TestSuite> suitesIdFirst;
            foreach (var testSuiteId in IdAndNameFolder.Keys)
            {
                suitesIdFirst = testLinkApi.GetTestSuitesForTestSuite(testSuiteId);
                int j = 0;
                subfolders[i] = new string[suitesIdFirst.Count];
                foreach (var suite in suitesIdFirst)
                {
                    subfolders[i][j] = suite.name;                    
                    j++;
                }
                i++;
            }
            return subfolders;
        }

        //Удаление папок с пустыми подпапками
        private DictionaryForWorkWithFolders DeleteFoldersWithEmptySubfolders(Dictionary<int, string> IdAndNameFolder, string[][] subfolders)
        {
            var folders = new DictionaryForWorkWithFolders();

            int i = 0;
            //Добавление в словарь всех значений
            foreach (var nameFolder in IdAndNameFolder)
            {
                folders.AddFolder(nameFolder.Key, nameFolder.Value, subfolders[i]);
                i++;
            }

            //Удаление пустых папок
            foreach (var folder in folders)
            {
                var folderValue = folder.Value;
                foreach (var folderVal in folderValue)
                {
                    if (folderVal.Value.Length == 0)                    
                        folders.Remove(folder.Key);                    
                }
            }
            return folders;
        }       

        //Получить имена папок второго уровня
        public DictionaryForWorkWithFolders GetAllTestCase(DictionaryForWorkWithFolders secondLevelFolder)
        {
            //Получаем External Id и TestCaseId из всех Suites. Взято с TestLink.
            DictionaryForWorkWithFolders testCaseExternalIDAndTestCaseId = GetExternalIDAndTestCaseIdAllSuitesByTestPlanId(secondLevelFolder);

            return testCaseExternalIDAndTestCaseId;
        }

        ////Получаем Id и Name тест кейсов из всех Suites. Взято с TestLink.
        //private Dictionary<string, string> GetExternalIDAndTestCaseIdAllSuitesByTestPlanId(int testPlanId)
        //{
        //    var suitesId = testLinkApi.GetTestSuitesForTestPlan(testPlanId);

        //    var testCaseExternalIDAndName = new Dictionary<string, string>();
        //    foreach (var item in suitesId)
        //    {
        //        try
        //        {
        //            var testCases = testLinkApi.GetTestCasesForTestSuite(item.id, false);
        //            for (int i = 0; i < testCases.Count; i++)
        //            {
        //                var testCaseName = "alphabi-" + testCases[i].external_id + ":" + testCases[i].name;
        //                testCaseExternalIDAndName.Add(testCaseName, testCases[i].id.ToString());
        //            }
        //        }
        //        catch (CookComputing.XmlRpc.XmlRpcIllFormedXmlException)
        //        {
        //            var testCases = testLinkApi.GetTestCasesForTestSuite(item.id, false);
        //            for (int i = 0; i < testCases.Count; i++)
        //            {
        //                var testCaseName = "alphabi-" + testCases[i].external_id + ":" + testCases[i].name;
        //                testCaseExternalIDAndName.Add(testCaseName, testCases[i].id.ToString());
        //            }
        //        }
        //    }

        //    return testCaseExternalIDAndName;
        //}


        //Получаем Id и Name тест кейсов из всех Suites. Взято с TestLink.
        private DictionaryForWorkWithFolders GetExternalIDAndTestCaseIdAllSuitesByTestPlanId(DictionaryForWorkWithFolders secondLevelFolder)
        {
            List<TestCaseFromTestSuite> testCaseExternalIDAndTestCaseId;
            foreach (var folder in secondLevelFolder)
            {
                testCaseExternalIDAndTestCaseId = testLinkApi.GetTestCasesForTestSuite(folder.Key, true);
            }
            //foreach (var item in suitesId)
            //{
            //    try
            //    {
            //        var testCases = testLinkApi.GetTestCasesForTestSuite(item.id, false);
            //        for (int i = 0; i < testCases.Count; i++)
            //        {
            //            var testCaseName = "alphabi-" + testCases[i].external_id + ":" + testCases[i].name;
            //            testCaseExternalIDAndName.Add(testCaseName, testCases[i].id.ToString());
            //        }
            //    }
            //    catch (CookComputing.XmlRpc.XmlRpcIllFormedXmlException)
            //    {
            //        var testCases = testLinkApi.GetTestCasesForTestSuite(item.id, false);
            //        for (int i = 0; i < testCases.Count; i++)
            //        {
            //            var testCaseName = "alphabi-" + testCases[i].external_id + ":" + testCases[i].name;
            //            testCaseExternalIDAndName.Add(testCaseName, testCases[i].id.ToString());
            //        }
            //    }
            //}

            return secondLevelFolder;
        }
    }
}
