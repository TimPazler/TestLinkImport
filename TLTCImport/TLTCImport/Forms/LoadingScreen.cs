using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TLTCImport
{
    public partial class LoadingScreen : Form
    {
        public Label lblLoading;

        //Форма для отображения загрузки, во переноса рез-ов в TestLink
        public LoadingScreen(string message = "Выполняется перенос результатов в TestLink...")
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(350, 100);
            lblLoading = new Label() { Text = message };
            lblLoading.Size = new Size(350, 100);
            lblLoading.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblLoading);           
        }       
    }
}
