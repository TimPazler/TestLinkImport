using NUnit.Framework.Internal;
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
        public LoadingScreen()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(350, 100);
            lblLoading = new Label();
            lblLoading.Size = new Size(350, 100);
            lblLoading.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblLoading);
        }     


        public LoadingScreen OpenFormLoadingScreen(string message)
        {
            var loadingScreen = new LoadingScreen();
            loadingScreen.Location = Location;
            loadingScreen.StartPosition = FormStartPosition.CenterScreen;
            loadingScreen.FormClosing += delegate { Close(); };
            loadingScreen.lblLoading.Text = message;

            loadingScreen.Show();

            return loadingScreen;
        }        
    }
}
