using DevExpress.XtraTreeList.Columns;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;

namespace CompressionLibraryExamples {
    public partial class Form1 : DevExpress.XtraEditors.XtraForm {
        static string startupPath = Application.StartupPath;
        ZipExamples zExamples = new ZipExamples(startupPath, new string[] { Application.ExecutablePath, "Documents\\SampleDocument.docx" });

        public Form1()
        {
            InitializeComponent();
            InitData();
        }

        void InitData() {
            MyTreeData treeDataSource = new MyTreeData(null, null);
            MyTreeData rootNode1 = new MyTreeData(treeDataSource, new string[] { zExamples.GetType().ToString(), "All Examples" });
            foreach (MethodInfo mi in zExamples.GetMethods()) {
                if (mi.Name == "GetMethods" || mi.Name == "InvokeMethod") continue;
                string[] nodeValue = new string[2];
                nodeValue[0] = mi.Name;
                MyTreeData data = new MyTreeData(rootNode1, nodeValue);
            }

            treeList1.Columns.Add(new TreeListColumn() { Caption = "Action", VisibleIndex = 0, SortOrder = SortOrder.Ascending });
            treeList1.DataSource = treeDataSource;
            treeList1.ExpandAll();
        }

        //public bool IsRootNode(TreeListNode node) {
        //    return (node != null) && (node.owner == treeList1.Nodes);
        //}

        private void treeList1_DoubleClick(object sender, EventArgs e) {
            if(treeList1.FocusedNode != null && treeList1.FocusedNode.ParentNode != null) {
                string s = treeList1.FocusedNode.GetValue(0).ToString();
                Cursor.Current = Cursors.WaitCursor;
                zExamples.InvokeMethod(s, new List<object>());
                Cursor.Current = Cursors.Default;
                System.Diagnostics.Process.Start(startupPath + "\\Documents");
            }
        }
    }
}
