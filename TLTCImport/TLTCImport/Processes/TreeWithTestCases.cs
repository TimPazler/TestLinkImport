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
        List<TestSuite> suitesIdFirst;        

        public Folder[][] NamesFoldersAndSubfolders(int projectId)
        {           
            var firstLevelFolder = GetFirstLevelFolder(projectId);
            var arrFoldersAndSubfolders = FillArrayFoldersAndSubfolders(firstLevelFolder);
            var newArrFoldersAndSubfolders = RemoveEmptyArrayElements(arrFoldersAndSubfolders);
            return GetTestCases(newArrFoldersAndSubfolders);
        }

        //Удаление лишних элементов массива
        private Folder[][] RemoveEmptyArrayElements(Folder[][] arrFoldersAndSubfolders)
        {
            int count = 0;

            for (int i = 0; i < arrFoldersAndSubfolders.Length; i++)            
                if (arrFoldersAndSubfolders[i] != null)
                    count++;

            Folder[][] newArr = new Folder[count][];

            for (int i = 0; i < arrFoldersAndSubfolders.Length; i++)
            {
                if (arrFoldersAndSubfolders[i] != null)
                    newArr[i] = arrFoldersAndSubfolders[i];
            }
            return newArr;
        }

        public Folder[] FillArrays(Dictionary<int, string> firstLevelFolder, Dictionary<int, string> secondLevelFolder, int numberArrayFirstLevelFolder)
        {
            if (secondLevelFolder.Count != 0)
            {
                Subfolder[] subfolders = new Subfolder[secondLevelFolder.Count];
                Folder folderValue = null;

                int i = 0, j = 0;
                foreach (var subfolder in secondLevelFolder)
                {
                    subfolders[i] = new Subfolder(subfolder.Key, subfolder.Value);
                    foreach (var folder in firstLevelFolder)
                    {
                        if (j == numberArrayFirstLevelFolder)
                        {
                            folderValue = new Folder(folder.Key, folder.Value);
                            break;
                        }
                        j++;
                    }
                    j = 0;
                    i++;
                }
                folderValue.subfolders = subfolders;
                return new Folder[] { folderValue };
            }
            return null;
        }

        //тест кейсы
        private Folder[][] GetTestCases(Folder[][] arrFoldersAndSubfolders)
        {
            InfoTestCase[] testCase;

            List<TestCaseFromTestSuite> testCaseAllInfo;
            foreach (var foldersAndSubfolders in arrFoldersAndSubfolders)
            {                
                foreach (var subfolder in foldersAndSubfolders)
                {
                    for (int i = 0; i < subfolder.subfolders.Length; i++)
                    {
                        var nameSubfolder = subfolder.subfolders[i].nameSubfolder;
                        var idSubfolder = subfolder.subfolders[i].idSubfolder;

                        testCaseAllInfo = testLinkApi.GetTestCasesForTestSuite(idSubfolder, true);
                        testCase = new InfoTestCase[testCaseAllInfo.Count];
                        int j = 0;
                        foreach (var testCaseInfo in testCaseAllInfo)
                        {
                            testCase[j] = new InfoTestCase(testCaseInfo.id, Int32.Parse(testCaseInfo.external_id), testCaseInfo.name);
                            j++;
                        }
                        subfolder.subfolders[i].testCases = testCase;
                    }
                }
            }

            return arrFoldersAndSubfolders;
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
                if (FillArrays(IdAndNameFolder, subfolders, i) != null)
                {
                    root[i] = FillArrays(IdAndNameFolder, subfolders, i);
                    subfolders.Clear();
                    i++;
                }
                else                
                    subfolders.Clear();                
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
    }
}
