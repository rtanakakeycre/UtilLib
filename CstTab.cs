using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilLib
{
    public class CstTab : TabControl
    {
        private int mouseDownPointX = 0;
        private int mouseDownPointY = 0;
        private Rectangle dragBoxFromMouseDown = Rectangle.Empty;
        public sDD_CTL m_sDdCtl;
        // m_sTbc用のコンテキストメニュー
        ContextMenu menuTab = new ContextMenu();
        MenuItem menuItemAdd = new MenuItem();
        MenuItem menuItemClose = new MenuItem();
        MenuItem menuItemNameEdit = new MenuItem();

        public Type m_sUcType;

        public event EventHandler TabPageAddEvntHndl;
        public event EventHandler TabPageAddedEvntHndl;
        public event EventHandler TabPageDelEvntHndl;
        public event EventHandler TabPageRenameEvntHndl;
        public event EventHandler TabPageShiftEvntHndl;

        public CstTab()
        {
            AllowDrop = true;
            ClearDragTarget();

            Init();
        }

        private void Init()
        {
            // [挿入]と[削除]項目の設定
            menuItemAdd.Text = "追加";
            menuItemAdd.Click += menuItemAdd_Click;
            menuTab.MenuItems.Add(menuItemAdd);
            menuItemClose.Text = "削除";
            menuItemClose.Click += menuItemDel_Click;
            menuTab.MenuItems.Add(menuItemClose);
            menuItemNameEdit.Text = "名称変更";
            menuItemNameEdit.Click += menuItemNameEdit_Click;
            menuTab.MenuItems.Add(menuItemNameEdit);

            // 作成したコンテキストメニューをm_sTbcに設定
            this.ContextMenu = menuTab;

            // ドラッグ情報
            m_sDdCtl = new sDD_CTL();
        }

        public void Init(Type sUcType1)
        {
            m_sUcType = sUcType1;
            TabPages.Clear();
        }

        public Control AddPage(int idPage1, string txName1)
        {
            if(0 <= idPage1 && idPage1 < TabPages.Count)
            {
                TabPages.Insert(idPage1, txName1);
            }
            else
            {
                TabPages.Add(txName1);
            }

            Control sUc1 = null;
            if (m_sUcType != null)
            {
                sUc1 = (Control)Activator.CreateInstance(m_sUcType);
                TabPages[idPage1].Controls.Add(sUc1);
                sUc1.Dock = DockStyle.Fill;
            }

            return (sUc1);
        }

        private void menuItemAdd_Click(object sender, EventArgs e)
        {
            int idTabSel1 = GetTabIndex(m_sDdCtl.m_sPnt.X, m_sDdCtl.m_sPnt.Y);

            if (idTabSel1 >= 0)
            {
                // ページの追加
                NameEditFrm frmNameEdit1 = new NameEditFrm();
                frmNameEdit1.m_sTbName1.Text = "";
                if (frmNameEdit1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (frmNameEdit1.m_sTbName1.Text != "")
                    {
                        if (TabPageAddEvntHndl != null)
                        {
                            sTAB_ADD_EVNT_ARG sArg1 = new sTAB_ADD_EVNT_ARG();
                            sArg1.m_idPage = idTabSel1;
                            sArg1.m_txName = frmNameEdit1.m_sTbName1.Text;
                            TabPageAddEvntHndl(sArg1, e);
                        }

                        AddPage(idTabSel1, frmNameEdit1.m_sTbName1.Text);

                        if (TabPageAddedEvntHndl != null)
                        {
                            sTAB_ADD_EVNT_ARG sArg1 = new sTAB_ADD_EVNT_ARG();
                            sArg1.m_idPage = idTabSel1;
                            sArg1.m_txName = frmNameEdit1.m_sTbName1.Text;
                            TabPageAddedEvntHndl(sArg1, e);
                        }

                    }
                }
            }
        }

        private void menuItemDel_Click(object sender, EventArgs e)
        {
            int idTabSel1 = GetTabIndex(m_sDdCtl.m_sPnt.X, m_sDdCtl.m_sPnt.Y);

            if (idTabSel1 >= 0)
            {
                if (TabPageDelEvntHndl != null)
                {
                    TabPageDelEvntHndl(idTabSel1, e);
                }

                // ページの削除          
                this.TabPages.Remove(this.TabPages[idTabSel1]);
            }

        }

        private void menuItemNameEdit_Click(object sender, EventArgs e)
        {
            int idTabSel1 = GetTabIndex(m_sDdCtl.m_sPnt.X, m_sDdCtl.m_sPnt.Y);
            if (idTabSel1 >= 0)
            {
                // ページの名称変更
                NameEditFrm frmNameEdit1 = new NameEditFrm();
                frmNameEdit1.m_sTbName1.Text = TabPages[m_sDdCtl.m_aidItem[0]].Text;
                if (frmNameEdit1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (frmNameEdit1.m_sTbName1.Text != "")
                    {
                        if(TabPageRenameEvntHndl != null)
                        {
                            sTAB_ADD_EVNT_ARG sArg1 = new sTAB_ADD_EVNT_ARG();
                            sArg1.m_idPage = idTabSel1;
                            sArg1.m_txName = frmNameEdit1.m_sTbName1.Text;
                            TabPageRenameEvntHndl(sArg1, e);
                        }

                        TabPages[m_sDdCtl.m_aidItem[0]].Text = frmNameEdit1.m_sTbName1.Text;
                    }
                }
            }
        }

        protected override void OnDragOver(System.Windows.Forms.DragEventArgs e)
        {
            base.OnDragOver(e);

            int idTab1 = GetTabIndex(e.X, e.Y);

            //■タブがキチンと取れているかどうかで条件分岐
            if (idTab1 >= 0 && e.Data.GetDataPresent(typeof(TabPage)))
            {
                //タブが取得できた場合の処理
                e.Effect = DragDropEffects.Move;
                TabPage draggedTab = (TabPage)e.Data.GetData(typeof(TabPage));

                int srcTabIndex = FindIndex(draggedTab);

                if (srcTabIndex != idTab1)
                {
                    this.SuspendLayout();//★これ大事
                    TabPage tmp = TabPages[srcTabIndex];
                    TabPages[srcTabIndex] = TabPages[idTab1];
                    TabPages[idTab1] = tmp;

                    SelectedTab = draggedTab;

                    this.ResumeLayout();//★これも大事

                    if(TabPageShiftEvntHndl != null)
                    {
                        sTAB_SHIFT_EVNT_ARG sArg1 = new sTAB_SHIFT_EVNT_ARG();
                        sArg1.m_idPageSrc = srcTabIndex;
                        sArg1.m_idPageDst = idTab1;
                        TabPageShiftEvntHndl(sArg1, e);
                    }
                }
            }
            else
            {
                //タブが取得できなかった場合の処理
                e.Effect = DragDropEffects.None;//何もしなくて良い
            }
        }

        private int GetTabIndex(int X, int Y)
        {
            int idTab1 = -1;
            //■イベントが起きた位置をクライアント座標ポイントptに変換する．
            Point pt = PointToClient(new Point(X, Y));

            //■ptからhovering overタブを得る．
            TabPage hoverTab = GetTabPageByTab(pt);

            //■タブがキチンと取れているかどうかで条件分岐
            if (hoverTab != null)
            {
                idTab1 = FindIndex(hoverTab);
            }

            return (idTab1);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (dragBoxFromMouseDown != Rectangle.Empty
                &&
                !dragBoxFromMouseDown.Contains(e.X, e.Y))
            {
                if (this.TabCount <= 1)
                {
                    return;
                }

                Point pt = new Point(mouseDownPointX, mouseDownPointY);
                TabPage tp = GetTabPageByTab(pt);

                if (tp != null)
                {
                    DoDragDrop(tp, DragDropEffects.All);
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ClearDragTarget();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ClearDragTarget();
        }

        private void ClearDragTarget()
        {
            dragBoxFromMouseDown = Rectangle.Empty;
            mouseDownPointX = 0;
            mouseDownPointY = 0;
        }

        private void SetupDragTarget(int x, int y)
        {
            Size dragSize = SystemInformation.DragSize;

            dragBoxFromMouseDown =
                new Rectangle(new Point(x - (dragSize.Width / 2),
                                        y - (dragSize.Height / 2)), dragSize);
            mouseDownPointX = x;
            mouseDownPointY = y;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            ClearDragTarget();

            if(e.Clicks >= 2)
            {

            }
            else if (e.Button == MouseButtons.Left)
            {
                SetupDragTarget(e.X, e.Y);
            }
            else
            {
                // ドラッグする変数を保持
                m_sDdCtl.m_nbMode = sDD_CTL.eDD_MODE.WAIT;
                m_sDdCtl.m_sPnt = Cursor.Position;
                m_sDdCtl.m_aidItem.Clear();
                m_sDdCtl.m_aidItem.Add(this.SelectedIndex);
            }

        }

        //■GetTabPageByTab : Point --> TabPage + {null}
        private TabPage GetTabPageByTab(Point pt)
        {
            for (int i = 0; i < TabPages.Count; i++)
            {
                if (GetTabRect(i).Contains(pt))
                {
                    return TabPages[i];
                }
            }

            return null;
        }

        //■FindIndex: TabPage --> Int + { null }
        private int FindIndex(TabPage page)
        {
            for (int i = 0; i < TabPages.Count; i++)
            {
                if (TabPages[i] == page)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public class sTAB_ADD_EVNT_ARG
    {
        public int m_idPage;
        public string m_txName;

        public sTAB_ADD_EVNT_ARG()
        {
            m_idPage = -1;
            m_txName = "";
        }
    }

    public class sTAB_SHIFT_EVNT_ARG
    {
        public int m_idPageSrc;
        public int m_idPageDst;

        public sTAB_SHIFT_EVNT_ARG()
        {
            m_idPageSrc = -1;
            m_idPageDst = -1;
        }
    }

    public class sDD_CTL
    {
        // ドラッグモード
        public enum eDD_MODE
        {
            NON,        // 非ドラッグ中
            WAIT,       // マウスダウンしてからドラッグ検知するまで
            DRAG,       // ドラッグ中
            LMT
        };

        public eDD_MODE m_nbMode;
        public Point m_sPnt;
        public List<int> m_aidItem;

        public sDD_CTL()
        {
            m_nbMode = eDD_MODE.NON;
            m_sPnt = new Point();
            m_aidItem = new List<int>();
        }
    }
}
