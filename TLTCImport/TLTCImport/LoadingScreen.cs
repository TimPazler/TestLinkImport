using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TLTCImport
{
    public partial class LoadingScreen : Form
    {
        //Форма для отображения загрузки, во переноса рез-ов в TestLink
        public LoadingScreen()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(350, 100);
            var lbl = new Label() { Text = "Выполняется перенос результатов в TestLink..." };
            lbl.Size = new Size(350, 100);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lbl);           
        }       
    }
}
