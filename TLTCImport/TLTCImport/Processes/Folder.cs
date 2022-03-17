using System;
using System.Collections.Generic;
using System.Text;

namespace TLTCImport
{
    public class Folder
    {
        public int idFolder { get; set; } 
        public string nameFolder { get; set; }

        public Subfolder[] arr;

        public Folder(int idFolder, string nameFolder)
        {
            this.idFolder = idFolder;
            this.nameFolder = nameFolder;
        }
    }    
}
