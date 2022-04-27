using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TLTCImport.Forms;

namespace TLTCImport
{
    public partial class AuthorizationForm : Form
    {
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
            string apiDevKey = tbApiDevKey.Text;
            string urlTestLink = tbUrlTestLink.Text;

            if (urlTestLink != "" && apiDevKey != "")
            {
                lblLoginResult.Visible = false;
                Thread.Sleep(500);
                if (TestLinkResult.Authorization(urlTestLink, apiDevKey))
                {
                    pbLoading.Visible = true;
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
            else
                MessageBox.Show("Поля не были заполнены!", "Ошибка!");
        }

        private void instruction_Click(object sender, EventArgs e)
        {
            var frm = new Instruction();
            frm.ShowDialog();
        }
    }
}
