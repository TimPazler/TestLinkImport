using System;
using System.Collections.Generic;
using System.Text;

namespace TLTCImport.FolderStorageTestLink
{
    //Класс, содержащий id, externalId и имя тесткейса
    //Также ссылку на информацию о проекте
    public class InfoTestCase
    {
        public int idTestCase { get; set; }
        public int externalIdTestCase { get; set; }
        public string nameTestCase { get; set; }
        public string typeResult { get; set; }

        public Project project { get; set; }

        public InfoTestCase(int idTestCase, int externalIdTestCase, string nameTestCase, string typeResult = "null")
        {
            this.idTestCase = idTestCase;
            this.externalIdTestCase = externalIdTestCase;
            this.nameTestCase = nameTestCase;
            this.typeResult = typeResult;
        }      
    }
}
