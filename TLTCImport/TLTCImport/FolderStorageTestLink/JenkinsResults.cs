using System;
using System.Collections.Generic;
using System.Text;

namespace TLTCImport.FolderStorageTestLink
{
    //Собирает информацию о кейсах, взятых с Jenkins
    class JenkinsResults
    {
        public string name { get; set; }
        public string status { get; set; }
        public JenkinsResults(string nameTestCase, string statusTestCase)
        {
            this.name = nameTestCase;
            this.status = statusTestCase;
        }
    }
}
