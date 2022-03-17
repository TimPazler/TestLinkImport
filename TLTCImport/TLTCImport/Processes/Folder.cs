using System;
using System.Collections.Generic;
using System.Text;

namespace TLTCImport
{
    //Класс, содержащий id и имя папки
    //А также ссылку на другой массив с данными о подпапки
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
