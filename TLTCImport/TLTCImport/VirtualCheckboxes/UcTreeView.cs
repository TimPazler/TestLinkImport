using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TLTCImport
{    
    public partial class UcTreeView : TreeView
    {
        private string pathContent = "../../../Content/";

        [DisplayName("Checkbox Spacing"), CategoryAttribute("Appearance"),
         Description("Number of pixels between the checkboxes.")]
        public int Spacing { get; set; }

        [DisplayName("Text Padding"), CategoryAttribute("Appearance"),
         Description("Left padding of text.")]
        public int LeftPadding { get; set; }

        public UcTreeView()
        {
            DrawMode = TreeViewDrawMode.OwnerDrawText;
            HideSelection = false;    // I like that better
            CheckBoxes = false;       // necessary!
            FullRowSelect = false;    // necessary!
            Spacing = 4;              // default checkbox spacing
            LeftPadding = 7;          // default text padding
        }

        public TreeNodeVirtual AddNode(string label, bool check1, bool check2, bool check3)
        {
            TreeNodeVirtual node = new TreeNodeVirtual(label, check1, check2, check3);
            this.Nodes.Add(node);
            return node;
        }

        private Size glyph = Size.Empty;

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            TreeNodeVirtual n = e.Node as TreeNodeVirtual;
            if (n == null) { e.DrawDefault = true; return; }

            CheckBoxState cbsTrue = CheckBoxState.CheckedNormal;
            CheckBoxState cbsFalse = CheckBoxState.UncheckedNormal;
          
            Rectangle rect = new Rectangle(e.Bounds.Location,
                                 new Size(ClientSize.Width, e.Bounds.Height));
            glyph = CheckBoxRenderer.GetGlyphSize(e.Graphics, cbsTrue);
            int offset = glyph.Width * 3 + Spacing * 2 + LeftPadding;

            if (n.IsSelected)
            {
                e.Graphics.FillRectangle(SystemBrushes.MenuHighlight, rect);
                e.Graphics.DrawString(n.Label, Font, Brushes.White,
                                      e.Bounds.X + offset, e.Bounds.Y);
            }
            else
            {
                //тут баг (исправлен в MainForm открытием и закрытием всех узлов)
                CheckBoxRenderer.DrawParentBackground(e.Graphics, e.Bounds, this);
                //new MainForm().treeView.Refresh();
                e.Graphics.DrawString(n.Label, Font, Brushes.Black,
                                      e.Bounds.X + offset, e.Bounds.Y);
            }
            
            CheckBoxState bs1 = n.Check1 ? cbsTrue : cbsFalse;
            CheckBoxState bs2 = n.Check2 ? cbsTrue : cbsFalse;
            CheckBoxState bs3 = n.Check3 ? cbsTrue : cbsFalse;

            CheckBoxRenderer.DrawCheckBox(e.Graphics, cbx(e.Bounds, 0).Location, bs1);
            CheckBoxRenderer.DrawCheckBox(e.Graphics, cbx(e.Bounds, 1).Location, bs2);
            CheckBoxRenderer.DrawCheckBox(e.Graphics, cbx(e.Bounds, 2).Location, bs3);            
        }
       
        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            TreeNodeVirtual n = e.Node as TreeNodeVirtual;
            if (e == null) return;

            if (n != null)
            {                        
                if (cbx(n.Bounds, 0).Contains(e.Location)) n.Check1 = !n.Check1;
                else if (cbx(n.Bounds, 1).Contains(e.Location)) n.Check2 = !n.Check2;
                else if (cbx(n.Bounds, 2).Contains(e.Location)) n.Check3 = !n.Check3;
                else
                {
                    if (SelectedNode == n && Control.ModifierKeys == Keys.Control)
                        SelectedNode = SelectedNode != null ? null : n;
                    else SelectedNode = n;
                }

                Invalidate();
            }
        }

        Rectangle cbx(Rectangle bounds, int check)
        {            
            return new Rectangle(bounds.Left + 2 + (glyph.Width + Spacing) * check,
                                 bounds.Y + 2, glyph.Width, glyph.Height);
        }

    }
}
