using System;
using System.Collections.Generic;
using System.Text;

namespace TLTCImport.FolderStorageTestLink
{
    //Класс, содержащий id и имя тесткейса
    public class InfoTestCase
    {
        public int idTestCase { get; set; }
        public int externalIdTestCase { get; set; }
        public string nameTestCase { get; set; }

        public InfoTestCase(int idTestCase, int externalIdTestCase, string nameTestCase)
        {
            this.idTestCase = idTestCase;
            this.externalIdTestCase = externalIdTestCase;
            this.nameTestCase = nameTestCase;
        }
    }
}
