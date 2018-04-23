Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
#Region "#usings"
Imports DevExpress.Compression
#End Region ' #usings

Namespace CompressionLibraryExamples
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
		End Sub

		#Region "#archivedirectory"
		Private Sub ArchiveDirectory(ByVal path As String)
			Using archive As New ZipArchive()
				archive.AddDirectory(path)
				archive.Save("ZipDirectory.zip")
			End Using
		End Sub
		#End Region ' #archivedirectory

		#Region "#archivefiles"
		Private Sub ArchiveFiles(ByVal sourceFiles() As String)
			Using archive As New ZipArchive()
				For Each file As String In sourceFiles
					archive.AddFile(file, "/")
				Next file
				archive.Save("ZipFiles.zip")
			End Using
		End Sub
		#End Region ' #archivefiles

		#Region "#archivedirectoryhandlingerrors"
		Private Sub ArchiveDirectoryWithError(ByVal path As String)
			Using archive As New ZipArchive()
				AddHandler archive.Error, AddressOf archive_Error
				archive.AddDirectory(path)
				archive.Save("ZipDirectory.zip")
			End Using
		End Sub

		Private Sub archive_Error(ByVal sender As Object, ByVal args As ErrorEventArgs)
			Dim errorMessage As String
			Dim e As Exception = args.GetException()
			If String.IsNullOrEmpty(args.ItemName) Then
				errorMessage = e.Message
			Else
				errorMessage = String.Format("Item: {0}" & Constants.vbLf + Constants.vbLf & "Description:" & Constants.vbLf & "{1}", args.ItemName, e.Message)
			End If
			Dim descriptionMessage As String = "Click Cancel to abort operation. Click OK to skip the item and continue."
			Dim message As String = String.Format("{0}" & Constants.vbLf & "{1}", errorMessage, descriptionMessage)
			If MessageBox.Show(message, "Error", MessageBoxButtons.OKCancel) = System.Windows.Forms.DialogResult.Cancel Then
				args.CanContinue = False
			End If
		End Sub
		#End Region ' #archivedirectoryhandlingerrors


		#Region "#filterarchivefiles"
'INSTANT VB TODO TASK: There is no VB.NET equivalent to 'volatile':
'ORIGINAL LINE: volatile bool stopArchiving = False;
		Private stopArchiving As Boolean = False
		Private Sub FilterArchiveFiles(ByVal sourceFiles() As String)
			Using archive As New ZipArchive()
				AddHandler archive.ItemAdding, AddressOf archive_ItemAdding
				For Each file As String In sourceFiles
					archive.AddFile(file, "/")
				Next file
				archive.Save("ZipFilterFiles.zip")
			End Using
		End Sub

		Private Sub archive_ItemAdding(ByVal sender As Object, ByVal args As ZipItemAddingEventArgs)
			If args.Item.CreationTime.Date <> DateTime.Today Then
				args.Action = ZipItemAddingAction.Cancel
			End If
			If stopArchiving Then
				args.Action = ZipItemAddingAction.Stop
			End If
		End Sub
		#End Region ' #filterarchivefiles

		#Region "#archivefilesbatch"
		Private Sub ArchiveFilesBatch(ByVal path As String)
			Using archive As New ZipArchive()
				Dim files = _
					From file In System.IO.Directory.EnumerateFiles(path, "*.txt", System.IO.SearchOption.AllDirectories) , line In System.IO.File.ReadLines(file) _
					Where line.Contains("DevExpress") _
					Select file
				archive.AddFiles(files)
				archive.Save("ZipFilesDX.zip")
			End Using
		End Sub
		#End Region ' #archivefilesbatch

		#Region "#cancelarchiveprogress"
'INSTANT VB TODO TASK: There is no VB.NET equivalent to 'volatile':
'ORIGINAL LINE: volatile bool stopProgress = False;
		Private stopProgress As Boolean = False
		Private Sub CancelArchiveProgress(ByVal sourceFiles() As String)
			Using archive As New ZipArchive()
				AddHandler archive.Progress, AddressOf archive_Progress
				For Each file As String In sourceFiles
					archive.AddFile(file, "/")
				Next file
				archive.Save("ZipCancelFiles.zip")
			End Using
		End Sub

		Private Sub archive_Progress(ByVal sender As Object, ByVal args As ProgressEventArgs)
			args.CanContinue = Not Me.stopProgress
		End Sub
		#End Region ' #cancelarchiveprogress

		#Region "#protectpassword"
		Private Sub ProtectPassword(ByVal sourceFiles() As String, ByVal password As String)
			Using archive As New ZipArchive()
				For Each file As String In sourceFiles
					Dim zipFI As ZipFileItem = archive.AddFile(file, "/")
					zipFI.EncryptionType = EncryptionType.Aes128
					zipFI.Password = password & System.IO.Path.GetFileName(file).Substring(0, 1)
				Next file
				archive.Save("ZipEncryptedFiles.zip")
			End Using
		End Sub
		#End Region ' #protectpassword

		#Region "#addcomment"
		Private Sub ArchiveWithComment(ByVal path As String)
			Using archive As New ZipArchive()
				For Each file As String In System.IO.Directory.EnumerateFiles(path)
					Dim zipFI As ZipFileItem = archive.AddFile(file, "/")
					zipFI.Comment = "Archived by " & Environment.UserName
				Next file
				archive.Save("ZipCommentedFiles.zip")
			End Using
		End Sub
		#End Region ' #addcomment

		#Region "#archivestream"
		Private Sub ArchiveStream(ByVal myStream As System.IO.Stream, ByVal myZippedStream As System.IO.Stream)
			Using archive As New ZipArchive()
				archive.AddStream("myStream", myStream)
				archive.Save(myZippedStream)
			End Using
		End Sub
		#End Region ' #archivestream

		#Region "#archivebytearray"
		Private Sub ArchiveByteArray(ByVal myByteArray() As Byte, ByVal myZippedStream As System.IO.Stream)
			Using archive As New ZipArchive()
				archive.AddByteArray("myByteArray", myByteArray)
				archive.Save(myZippedStream)
			End Using
		End Sub
		#End Region ' #archivebytearray

		#Region "#unziparchive"
		Private Sub UnzipArchive(ByVal pathToZipArchive As String, ByVal pathToExtract As String)
			Using archive As ZipArchive = ZipArchive.Read(pathToZipArchive)
				For Each item As ZipItem In archive
					item.Extract(pathToExtract)
				Next item
			End Using
		End Sub
		#End Region ' #unziparchive

		#Region "#unziparchiveconflict"
		Private Sub UnzipArchiveConflict(ByVal pathToZipArchive As String, ByVal pathToExtract As String)
			Using archive As ZipArchive = ZipArchive.Read(pathToZipArchive)
				archive.OptionsBehavior.AllowFileOverwrite = AllowFileOverwriteMode.Custom
				AddHandler archive.AllowFileOverwrite, AddressOf archive_AllowFileOverwrite
				For Each item As ZipItem In archive
					item.Extract(pathToExtract)
				Next item
			End Using
		End Sub

		Private Sub archive_AllowFileOverwrite(ByVal sender As Object, ByVal e As AllowFileOverwriteEventArgs)
			Dim fi As New System.IO.FileInfo(e.TargetFilePath)
			If e.ZipItem.LastWriteTime < fi.LastWriteTime Then
				e.Cancel = True
			End If
		End Sub
		#End Region ' #unziparchiveconflict

		#Region "#archivetext"
		Private Sub ArchiveText()
			Using archive As New ZipArchive()
				archive.AddText("Text_DE.txt", "Komprimieren großer Dateien mühelos")
				archive.Save("ZipText.zip")
			End Using
		End Sub
		#End Region ' #archivetext
	End Class
End Namespace
