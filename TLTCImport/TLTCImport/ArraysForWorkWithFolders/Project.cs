using System;
using System.Collections.Generic;
using System.Text;
using TLTCImport.FolderStorageTestLink;

namespace TLTCImport
{
    //Класс, содержащий id,имя и префикс проекта    
    public class Project
    {
        public int projectId { get; set; }
        public string projectName { get; set; }
        public string prefixName { get; set; }

        public Project(int projectId, string projectName, string prefixName)
        {
            this.projectId = projectId;
            this.projectName = projectName;
            this.prefixName = prefixName;
        }
    }
}
