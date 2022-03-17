using System;
using System.Collections.Generic;
using System.Text;

namespace TLTCImport
{
    public class Subfolder
    {
        public int idSubfolder { get; set; }
        public string nameSubfolder { get; set; }

        public Subfolder(int idSubfolder, string nameSubfolder)
        {
            this.idSubfolder = idSubfolder;
            this.nameSubfolder = nameSubfolder;
        }
    }
}
