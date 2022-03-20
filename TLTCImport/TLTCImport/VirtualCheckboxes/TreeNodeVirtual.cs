using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TLTCImport
{
    public class TreeNodeVirtual : TreeNode
    {
        public string Label { get; set; }
        public bool Check1 { get; set; }
        public bool Check2 { get; set; }
        public bool Check3 { get; set; }

        public new string Text
        {
            get { return Label; }
            set { Label = value; base.Text = ""; }
        }

        public TreeNodeVirtual() { }

        public TreeNodeVirtual(string text) { Label = text; }

        public TreeNodeVirtual(string text, bool check1, bool check2, bool check3)
        {
            Label = text;
            Check1 = check1; Check2 = check2; Check3 = check3;
        }

        public TreeNodeVirtual(string text, TreeNodeVirtual[] children)
        {
            Label = text;
            foreach (TreeNodeVirtual node in children) this.Nodes.Add(node);
        }

        public TreeNodeVirtual(string text, int imageIndex, int selectedImageIndex) : base(text, imageIndex, selectedImageIndex)
        {
            Label = text;
        }
    }
}
