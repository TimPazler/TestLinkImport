using System;
using System.Collections.Generic;
using System.Text;
using static TLTCImport.TestLinkResult;

namespace TLTCImport
{    
    //Словарь для хранения трех значений
    public class DictionaryForTestCase : Dictionary<string, TestCaseValues>
    {       
        public void AddTestCase(string key, string testCaseId, string resultRun)
        {
            TestCaseValues val;
            val.testCaseId = testCaseId;
            val.resultRun = resultRun;
            this.Add(key, val);
        }      
    }

    public class DictionaryForWorkWithFolders : Dictionary<int, Dictionary<string, string[]>>
    {
        public void AddFolder(int idFolder, string nameFolder, string[] subfolders)
        {
            var folders = new Dictionary<string, string[]>();
            folders.Add(nameFolder, subfolders);
            this.Add(idFolder, folders);
        }
    }

    //public class DictionaryForWorkWithFolders : Dictionary<Dictionary<int, string>, Dictionary<int, string>>
    //{
    //    public void AddFolder(int idFolder, string nameFolder, int idSubfolder, string nameSubfolder)
    //    {
    //        var folders = new Dictionary<int, string>();
    //        folders.Add(idFolder, nameFolder);

    //        var subfolders = new Dictionary<int, string>();
    //        subfolders.Add(idSubfolder, nameSubfolder);

    //        this.Add(folders, subfolders);
    //    }
    //}
}
