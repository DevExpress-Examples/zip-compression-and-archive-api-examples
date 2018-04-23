Imports Microsoft.VisualBasic
Imports DevExpress.XtraTreeList.Columns
Imports System
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Windows.Forms
Imports DevExpress.XtraTreeList.Nodes

Namespace CompressionLibraryExamples
	Partial Public Class Form1
		Inherits DevExpress.XtraEditors.XtraForm
		Private Shared startupPath As String = Application.StartupPath
		Private zExamples As New ZipExamples(startupPath, New String() { Application.ExecutablePath, "Documents\SampleDocument.docx" })

		Public Sub New()
			InitializeComponent()
			InitData()
		End Sub

		Private Sub InitData()
			Dim treeDataSource As New MyTreeData(Nothing, Nothing)
			Dim rootNode1 As New MyTreeData(treeDataSource, New String() { zExamples.GetType().ToString(), "All Examples" })
			For Each mi As MethodInfo In zExamples.GetMethods()
				If mi.Name = "GetMethods" OrElse mi.Name = "InvokeMethod" Then
					Continue For
				End If
				Dim nodeValue(1) As String
				nodeValue(0) = mi.Name
				Dim data As New MyTreeData(rootNode1, nodeValue)
			Next mi

			treeList1.Columns.Add(New TreeListColumn() With {.Caption = "Action", .VisibleIndex = 0, .SortOrder = SortOrder.Ascending})
			treeList1.DataSource = treeDataSource
			treeList1.ExpandAll()
		End Sub

		'public bool IsRootNode(TreeListNode node) {
		'    return (node != null) && (node.owner == treeList1.Nodes);
		'}

		Private Sub treeList1_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles treeList1.DoubleClick
			If treeList1.FocusedNode IsNot Nothing AndAlso treeList1.FocusedNode.ParentNode IsNot Nothing Then
				Dim s As String = treeList1.FocusedNode.GetValue(0).ToString()
				Cursor.Current = Cursors.WaitCursor
				zExamples.InvokeMethod(s, New List(Of Object)())
				Cursor.Current = Cursors.Default
				System.Diagnostics.Process.Start(startupPath & "\Documents")
			End If
		End Sub
	End Class
End Namespace
