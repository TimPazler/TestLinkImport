using System;
using System.Collections.Generic;
using System.Text;

namespace TLTCImport
{
    //Класс, содержащий id и имя подпапки
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
