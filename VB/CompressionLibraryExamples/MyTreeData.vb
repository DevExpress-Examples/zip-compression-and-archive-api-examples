Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports DevExpress.XtraTreeList
Imports DevExpress.XtraTreeList.Columns
Imports System.Collections

Namespace CompressionLibraryExamples

	' Represents a sample Business Object 
	Public Class MyTreeData
		Implements TreeList.IVirtualTreeListData
		Protected parentCore As MyTreeData
		Protected childrenCore As New ArrayList()
		Protected cellsCore() As Object

		Public Sub New(ByVal parent As MyTreeData, ByVal cells() As Object)
			' Specifies the parent node for the new node. 
			Me.parentCore = parent
			' Provides data for the node's cell. 
			Me.cellsCore = cells
			If Me.parentCore IsNot Nothing Then
				Me.parentCore.childrenCore.Add(Me)
			End If
		End Sub
		Private Sub VirtualTreeGetChildNodes(ByVal info As VirtualTreeGetChildNodesInfo) Implements TreeList.IVirtualTreeListData.VirtualTreeGetChildNodes
			info.Children = childrenCore
		End Sub
		Private Sub VirtualTreeGetCellValue(ByVal info As VirtualTreeGetCellValueInfo) Implements TreeList.IVirtualTreeListData.VirtualTreeGetCellValue
			info.CellData = cellsCore(info.Column.AbsoluteIndex)
		End Sub

		Private Sub VirtualTreeSetCellValue(ByVal info As VirtualTreeSetCellValueInfo) Implements TreeList.IVirtualTreeListData.VirtualTreeSetCellValue
			cellsCore(info.Column.AbsoluteIndex) = info.NewCellData
		End Sub
	End Class
End Namespace
