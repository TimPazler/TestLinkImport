using System;
using System.Collections.Generic;
using System.Text;
using static TLTCImport.TlReportTcResult;

namespace TLTCImport
{    
    public class DictionaryWtihThreeValues : Dictionary<string, TestCaseValues>
    {       
        public void Add(string key, string testCaseId, string resultRun)
        {
            TestCaseValues val;
            val.testCaseId = testCaseId;
            val.resultRun = resultRun;
            this.Add(key, val);
        }      
    }
}
