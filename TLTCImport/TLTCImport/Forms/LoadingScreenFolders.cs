using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TLTCImport.Forms
{
    public partial class LoadingScreenFolders : Form
    {
        public Label lblTypeFolder, lblNameFolder;
       
        public LoadingScreenFolders()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(350, 100);

            lblNameFolder = new Label();
            lblNameFolder.Size = new Size(300, 50);
            lblNameFolder.Location = new Point(10, 50);
            //lblNameFolder.TextAlign = ContentAlignment.MiddleCenter;

            lblTypeFolder = new Label();
            lblTypeFolder.Size = new Size(300, 50);
            lblTypeFolder.Location = new Point(20, 10);
            lblTypeFolder.Font = new System.Drawing.Font(DefaultFont, FontStyle.Bold);
            //lblTypeFolder.TextAlign = ContentAlignment.BottomLeft;

            this.Controls.Add(lblNameFolder);
            this.Controls.Add(lblTypeFolder);
        }

        public LoadingScreenFolders OpenFormLoadingScreen(string message, string nameFolder = null, LoadingScreenFolders loadingScreenFolders = null)
        {
            LoadingScreenFolders loadingScreen;
            if (loadingScreenFolders == null)
            {
                loadingScreen = new LoadingScreenFolders();
                loadingScreen.Location = Location;
                loadingScreen.StartPosition = FormStartPosition.CenterScreen;
                loadingScreen.FormClosing += delegate { Close(); };
                loadingScreen.lblNameFolder.Text = nameFolder;
                loadingScreen.lblTypeFolder.Text = message;
                loadingScreen.Show();
            }
            else
            {
                loadingScreen = loadingScreenFolders;
                loadingScreen.Location = Location;
                loadingScreen.StartPosition = FormStartPosition.CenterScreen;
                loadingScreen.lblNameFolder.Text = nameFolder;
                loadingScreen.lblTypeFolder.Text = message;
            }
            return loadingScreen;
        }
    }
}
