using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TLTCImport
{
    public partial class AuthorizationForm : Form
    {       
        //Добавить поле для указания url тестлинка
        public AuthorizationForm()
        {
            InitializeComponent();                                                                        
        }       

        private void llHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start https://t.me/TimPazler"));
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string apiDevKey = "bf9a98dc1791a5c7115b9e4322b4f3bc";

            //apiDevKey = tbApiDevKey.Text;
            lblLoginResult.Visible = false;
            Thread.Sleep(500);
            pbLoading.Visible = true;
            if (TestLinkResult.Authorization(apiDevKey))
            {
                pbLoading.Visible = false;
                lblLoginResult.Visible = false;

                //Открытие нового окна
                var frm = new MainForm();
                frm.Location = Location;
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.FormClosing += delegate { Show(); };
                frm.Show();
                Hide();
            }
            else
            {
                pbLoading.Visible = false;
                lblLoginResult.Visible = true;
            }            
        }       
    }
}
