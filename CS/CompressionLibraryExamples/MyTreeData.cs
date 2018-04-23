using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using System.Collections;

namespace CompressionLibraryExamples {

    // Represents a sample Business Object 
    public class MyTreeData : TreeList.IVirtualTreeListData {
        protected MyTreeData parentCore;
        protected ArrayList childrenCore = new ArrayList();
        protected object[] cellsCore;

        public MyTreeData(MyTreeData parent, object[] cells) {
            // Specifies the parent node for the new node. 
            this.parentCore = parent;
            // Provides data for the node's cell. 
            this.cellsCore = cells;
            if (this.parentCore != null) {
                this.parentCore.childrenCore.Add(this);
            }
        }
        void TreeList.IVirtualTreeListData.VirtualTreeGetChildNodes(
        VirtualTreeGetChildNodesInfo info) {
            info.Children = childrenCore;
        }
        void TreeList.IVirtualTreeListData.VirtualTreeGetCellValue(
        VirtualTreeGetCellValueInfo info) {
            info.CellData = cellsCore[info.Column.AbsoluteIndex];
        }

        void TreeList.IVirtualTreeListData.VirtualTreeSetCellValue(VirtualTreeSetCellValueInfo info) {
            cellsCore[info.Column.AbsoluteIndex] = info.NewCellData;
        }
    }
}
