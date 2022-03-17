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

        public void NamesFoldersAndSubfolders(int projectId)
        {           
            var firstLevelFolder = GetFirstLevelFolder(projectId);
            var arrFoldersAndSubfolders = FillArrayFoldersAndSubfolders(firstLevelFolder);
            var a = RemoveEmptyArrayElements(arrFoldersAndSubfolders);
            //return GetAllTestCase(a);
        }

        //Удаление лишних элементов массива
        private Folder[][] RemoveEmptyArrayElements(Folder[][] arrFoldersAndSubfolders)
        {
            //for (int i = 0; i < arrFoldersAndSubfolders.Length; i++)
            //{
            //    if(arrFoldersAndSubfolders[i] == null)
            //        arrFoldersAndSubfolders.
            //}

                return arrFoldersAndSubfolders;
        }

        public Folder[] FillArrays(Dictionary<int, string> firstLevelFolder, Dictionary<int, string> secondLevelFolder, int numberArrayFirstLevelFolder)
        {
            if (secondLevelFolder.Count != 0)
            {
                Subfolder[] arr1 = new Subfolder[secondLevelFolder.Count];
                Folder class1 = null;

                int i = 0, j = 0;
                foreach (var subfolder in secondLevelFolder)
                {
                    arr1[i] = new Subfolder(subfolder.Key, subfolder.Value);
                    foreach (var folder in firstLevelFolder)
                    {
                        if (j == numberArrayFirstLevelFolder)
                        {
                            class1 = new Folder(folder.Key, folder.Value);
                            break;
                        }
                        j++;
                    }
                    j = 0;
                    i++;
                }
                class1.arr = arr1;
                return new Folder[] { class1 };
            }
            return null;
        }

        //Заполнение массива папками и подпапками
        private Folder[][] FillArrayFoldersAndSubfolders(Dictionary<int, string> IdAndNameFolder)
        {
            Folder[][] root = new Folder[IdAndNameFolder.Count][];           

            Dictionary<int, string> subfolders = new Dictionary<int, string>();
            List<TestSuite> suitesIdFirst;
            int i = 0;
            foreach (var testSuiteId in IdAndNameFolder.Keys)
            {
                suitesIdFirst = testLinkApi.GetTestSuitesForTestSuite(testSuiteId);
                foreach (var suite in suitesIdFirst)
                {
                    subfolders.Add(suite.id, suite.name);
                }
                root[i] = FillArrays(IdAndNameFolder, subfolders, i);
                subfolders.Clear();
                i++;
            }

            return root;
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
