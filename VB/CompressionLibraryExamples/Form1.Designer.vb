Imports Microsoft.VisualBasic
Imports System
Namespace CompressionLibraryExamples
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.treeList1 = New DevExpress.XtraTreeList.TreeList()
			Me.memoEdit1 = New DevExpress.XtraEditors.MemoEdit()
			CType(Me.treeList1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.memoEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' treeList1
			' 
			Me.treeList1.Location = New System.Drawing.Point(12, 113)
			Me.treeList1.Name = "treeList1"
			Me.treeList1.OptionsBehavior.Editable = False
			Me.treeList1.Size = New System.Drawing.Size(335, 319)
			Me.treeList1.TabIndex = 2
'			Me.treeList1.DoubleClick += New System.EventHandler(Me.treeList1_DoubleClick);
			' 
			' memoEdit1
			' 
			Me.memoEdit1.EditValue = "This project " & Constants.vbCrLf & "contains sample actions" & Constants.vbCrLf & " to illustrate " & Constants.vbCrLf & "Compression Library API." & Constants.vbCrLf & "Double-click the node to execute."
			Me.memoEdit1.Location = New System.Drawing.Point(86, 23)
			Me.memoEdit1.Name = "memoEdit1"
			Me.memoEdit1.Properties.AllowFocused = False
			Me.memoEdit1.Properties.Appearance.Options.UseTextOptions = True
			Me.memoEdit1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			Me.memoEdit1.Properties.ReadOnly = True
			Me.memoEdit1.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None
			Me.memoEdit1.Size = New System.Drawing.Size(182, 71)
			Me.memoEdit1.TabIndex = 3
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(359, 459)
			Me.Controls.Add(Me.memoEdit1)
			Me.Controls.Add(Me.treeList1)
			Me.Name = "Form1"
			Me.Text = "Compression Library Examples"
			CType(Me.treeList1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.memoEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region
		Private WithEvents treeList1 As DevExpress.XtraTreeList.TreeList
		Private memoEdit1 As DevExpress.XtraEditors.MemoEdit
	End Class
End Namespace

