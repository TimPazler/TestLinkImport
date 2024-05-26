using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TestLinkApi;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TLTCImport.Forms
{
    public partial class Results_Manual : Form
    {
        System.Windows.Forms.ListView listView1;

        public Results_Manual(Dictionary<string, string> results)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Size = new Size(500, 250);
            this.Text = "Список перенесенных тест кейсов";

            //var lblHint = new Label();
            //lblHint.Size = new Size(250, 50);
            //lblHint.Location = new Point(10, 20);
            //lblHint.Text = "где,  p - Пройдено, f - Провалено и s - Пропущено";

            listView1 = new System.Windows.Forms.ListView();
            listView1.Location = new Point(10, 100);
            listView1.Size = new Size(250, 50);
            listView1.Columns.Add("Тест кейс", 100);
            listView1.Columns.Add("Результат", 100);
            listView1.Dock = DockStyle.Fill;
            listView1.View = View.Details;
            Controls.Add(listView1);

            OutputResults(results);
        }

        private void OutputResults(Dictionary<string, string> results)
        {
            foreach (var result in results)
            {
                ListViewItem item = new ListViewItem(result.Key);
                if (result.Value == "f")
                    item.SubItems.Add("Провалено");
                else if (result.Value == "p")
                    item.SubItems.Add("Пройдено");
                else if (result.Value == "b")
                    item.SubItems.Add("Блокировано");
                listView1.Items.Add(item);
            }

            this.Location = Location;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Show();
        }
    }
}
