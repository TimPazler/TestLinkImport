using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

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

        public TreeNodeVirtual treeNode { get; set; }
        public TreeNodeMouseClickEventArgs eventsTreeNode  { get; set; }

        public Project project { get; set; }

        public InfoTestCase(int idTestCase, int externalIdTestCase, string nameTestCase, string typeResult = "null",
            TreeNodeVirtual treeNode = null, TreeNodeMouseClickEventArgs eventsTreeNode = null)
        {
            this.idTestCase = idTestCase;
            this.externalIdTestCase = externalIdTestCase;
            this.nameTestCase = nameTestCase;
            this.typeResult = typeResult;
            this.treeNode = treeNode;
            this.eventsTreeNode = eventsTreeNode;
        }      
    }
}
