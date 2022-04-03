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
    //Рисует тройные чекбоксы
    public partial class UcTreeView : TreeView
    {
        [DisplayName("Checkbox Spacing"), CategoryAttribute("Appearance"),
         Description("Number of pixels between the checkboxes.")]
        public int Spacing { get; set; }

        [DisplayName("Text Padding"), CategoryAttribute("Appearance"),
         Description("Left padding of text.")]
        public int LeftPadding { get; set; }

        public Dictionary<string, string> SelectedResultsForManualMode = null;       

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

            //Работа с текстом
            if (n.IsSelected)
            {
                //Выделение строки (тесткейс)
                e.Graphics.FillRectangle(SystemBrushes.MenuHighlight, rect);
                e.Graphics.DrawString(n.Label, Font, Brushes.White,
                                      e.Bounds.X + offset, e.Bounds.Y);
            }
            else
            {
                //Отображает текст
                //тут баг (исправлен в MainForm открытием и закрытием всех узлов)               
                CheckBoxRenderer.DrawParentBackground(e.Graphics, e.Bounds, this);
                e.Graphics.DrawString(n.Label, Font, Brushes.Black,
                                      e.Bounds.X + offset, e.Bounds.Y);
            }
            CheckBoxState bs1 = n.CheckFailed ? cbsTrue : cbsFalse;
            CheckBoxState bs2 = n.CheckPassed ? cbsTrue : cbsFalse;
            CheckBoxState bs3 = n.CheckBlocked ? cbsTrue : cbsFalse;

            Pen cbx1False = new Pen(Color.Red, 4);
            Pen cbx2True = new Pen(Color.Green, 4);
            Pen cbx3Blocked = new Pen(Color.Purple, 4);

            e.Graphics.DrawRectangle(cbx1False, cbx(e.Bounds, 0));
            e.Graphics.DrawRectangle(cbx2True, cbx(e.Bounds, 1));
            e.Graphics.DrawRectangle(cbx3Blocked, cbx(e.Bounds, 2));

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
                HandlerSelectOneOfCheckboxes(n, e);
                AddProjectInfoForArrFolders(MainForm.folders, n.Label, n.CheckPassed, n.CheckFailed, n.CheckBlocked);
            }
        }
       
        //Обработчик, отвечающий за выбор чекбокса
        //Из трех чекбоксов можно выбрать лишь один, либо снять его
        private void HandlerSelectOneOfCheckboxes(TreeNodeVirtual n, TreeNodeMouseClickEventArgs e)
        {
            if (n.CheckFailed == true || n.CheckPassed == true || n.CheckBlocked == true)
            {
                if (cbx(n.Bounds, 0).Contains(e.Location) && (n.CheckPassed == true || n.CheckBlocked == true))
                {
                    n.CheckPassed = false;
                    n.CheckBlocked = false;
                    n.CheckFailed = !n.CheckFailed;
                }
                else if (cbx(n.Bounds, 1).Contains(e.Location) && (n.CheckFailed == true || n.CheckBlocked == true))
                {
                    n.CheckFailed = false;
                    n.CheckBlocked = false;
                    n.CheckPassed = !n.CheckPassed;
                }
                else if (cbx(n.Bounds, 2).Contains(e.Location) && (n.CheckFailed == true || n.CheckPassed == true))
                {
                    n.CheckFailed = false;
                    n.CheckPassed = false;
                    n.CheckBlocked = !n.CheckBlocked;
                }
                else
                {
                    if (n.CheckFailed == true) n.CheckFailed = false;
                    if (n.CheckPassed == true) n.CheckPassed = false;
                    if (n.CheckBlocked == true) n.CheckBlocked = false;
                }
            }
            else if (n.CheckFailed == false && n.CheckPassed == false && n.CheckBlocked == false)
            {
                if (cbx(n.Bounds, 0).Contains(e.Location)) n.CheckFailed = !n.CheckFailed;
                else if (cbx(n.Bounds, 1).Contains(e.Location)) n.CheckPassed = !n.CheckPassed;
                else if (cbx(n.Bounds, 2).Contains(e.Location)) n.CheckBlocked = !n.CheckBlocked;
            }

            Invalidate();
        }

        //Рекурсия
        //Добавление в массив папок выбранные рез-ат из чекбокса (пройдено, провалено или заблокировано)
        public void AddProjectInfoForArrFolders(Folder[] folders, string receivedNameTestCase, bool CheckPassed, bool CheckFailed, bool CheckBlocked)
        {            
            receivedNameTestCase = receivedNameTestCase.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0];

            //Проходим папки первого уровня
            foreach (var folder in folders)
            {
                //если в папке есть тесткейсы
                if (folder.testCases != null)
                {                    
                    foreach (var testCase in folder.testCases)
                    {
                        if(testCase != null) {
                            if (testCase.nameTestCase != "" || testCase.nameTestCase != null)
                            {
                                var prefixName = testCase.project.prefixName;
                                var externalId = testCase.externalIdTestCase;

                                var externalIdTestCase = prefixName + "-" + externalId;
                                if (externalIdTestCase.Contains(receivedNameTestCase))
                                {
                                    //f-Failed 
                                    if (CheckFailed == true)
                                    {
                                        testCase.typeResult = "f";
                                        break;
                                    }
                                    //p-Passed 
                                    else if (CheckPassed == true)
                                    {
                                        testCase.typeResult = "p";
                                        break;
                                    }
                                    //b-Blocked 
                                    else if (CheckBlocked == true)
                                    {
                                        testCase.typeResult = "b";
                                        break;
                                    }
                                    else
                                    {
                                        testCase.typeResult = "null";
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                //иначе смотрим подпапки
                else
                    //Просмотр подпапок
                    AddProjectInfoForArrFolders(folder.folders, receivedNameTestCase, CheckPassed, CheckFailed, CheckBlocked);                
            }
        }        

        Rectangle cbx(Rectangle bounds, int check)
        {
            return new Rectangle(bounds.Left + 2 + (glyph.Width + Spacing) * check,
                                 bounds.Y + 2, glyph.Width, glyph.Height);
        }
    }
}
