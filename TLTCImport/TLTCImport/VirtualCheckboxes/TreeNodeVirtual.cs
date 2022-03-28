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
        public bool CheckFailed { get; set; }
        public bool CheckPassed { get; set; }
        public bool CheckBlocked { get; set; }

        public new string Text
        {
            get { return Label; }
            set { Label = value; base.Text = ""; }
        }

        public TreeNodeVirtual() { }

        public TreeNodeVirtual(string text) { Label = text; }

        public TreeNodeVirtual(string text, bool checkFailed, bool checkPassed, bool checkBlocked)
        {
            Label = text;
            CheckFailed = checkFailed; CheckPassed = checkPassed; CheckBlocked = checkBlocked;
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
