Imports Microsoft.VisualBasic
#Region "#usings"
Imports DevExpress.Compression
#End Region ' #usings
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Windows.Forms

Namespace CompressionLibraryExamples
	Friend Class ZipExamples
		Private startupPath As String
		Private sourceFiles() As String

		Public Sub New(ByVal startupPath As String, ByVal sourceFiles() As String)
			Me.startupPath = startupPath
			Me.sourceFiles = sourceFiles
		End Sub

		Public Function GetMethods() As MethodInfo()
			Return Me.GetType().GetMethods(BindingFlags.Public Or BindingFlags.Instance Or BindingFlags.DeclaredOnly)
		End Function

		Public Sub InvokeMethod(ByVal methodName As String, ByVal args As List(Of Object))
			Me.GetType().GetMethod(methodName).Invoke(Me, args.ToArray())
		End Sub


		#Region "#archivedirectory"
		Public Sub ArchiveDirectory()
			Dim path As String = Me.startupPath
			Using archive As New ZipArchive()
				archive.AddDirectory(path)
				archive.Save("Documents\ArchiveDirectory.zip")
			End Using
		End Sub
		#End Region ' #archivedirectory

		#Region "#archivefiles"
		Public Sub ArchiveFiles()
			Dim sourcefiles() As String = Me.sourceFiles
			Using archive As New ZipArchive()
				For Each file As String In sourcefiles
					archive.AddFile(file, "/")
				Next file
				archive.Save("Documents\ArchiveFiles.zip")
			End Using
		End Sub
		#End Region ' #archivefiles

		#Region "#archivedirectoryhandlingerrors"
		Public Sub ArchiveDirectoryWithError()
			Dim path As String = Me.startupPath
			Using archive As New ZipArchive()
				AddHandler archive.Error, AddressOf archive_Error
				archive.AddDirectory(path)
				archive.Save("Documents\ArchiveDirectoryWithError.zip")
			End Using
		End Sub

		Private Sub archive_Error(ByVal sender As Object, ByVal args As DevExpress.Compression.ErrorEventArgs)
			Dim errorMessage As String
			Dim e As Exception = args.GetException()
			If String.IsNullOrEmpty(args.ItemName) Then
				errorMessage = e.Message
			Else
				errorMessage = String.Format("Item: {0}" & Constants.vbLf + Constants.vbLf & "Description:" & Constants.vbLf & "{1}", args.ItemName, e.Message)
			End If
			Dim descriptionMessage As String = "Click Cancel to abort operation. Click OK to skip the item and continue."
			Dim message As String = String.Format("{0}" & Constants.vbLf & "{1}", errorMessage, descriptionMessage)
			If MessageBox.Show(message, "Error", MessageBoxButtons.OKCancel) = DialogResult.Cancel Then
				args.CanContinue = False
			End If
		End Sub
		#End Region ' #archivedirectoryhandlingerrors


		#Region "#filterarchivefiles"
'INSTANT VB TODO TASK: There is no VB.NET equivalent to 'volatile':
'ORIGINAL LINE: volatile bool stopArchiving = False;
		Private stopArchiving As Boolean = False
		Public Sub FilterArchiveFiles()
			Dim sourcefiles() As String = Me.sourceFiles
			Using archive As New ZipArchive()
				AddHandler archive.ItemAdding, AddressOf archive_ItemAdding
				For Each file As String In Me.sourceFiles
					archive.AddFile(file, "/")
				Next file
				archive.Save("Documents\FilterArchiveFiles.zip")
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
		Public Sub ArchiveFilesBatch()
			Dim path As String = Me.startupPath
			Using archive As New ZipArchive()
				Dim files = _
					From file In System.IO.Directory.EnumerateFiles(path, "*.xml", System.IO.SearchOption.AllDirectories) , line In System.IO.File.ReadLines(file) _
					Where line.Contains("DevExpress") _
					Select file
				archive.AddFiles(files)
				archive.Save("Documents\ArchiveFilesBatch.zip")
			End Using
		End Sub
		#End Region ' #archivefilesbatch

		#Region "#cancelarchiveprogress"
'INSTANT VB TODO TASK: There is no VB.NET equivalent to 'volatile':
'ORIGINAL LINE: volatile bool stopProgress = False;
		Private stopProgress As Boolean = False

		Public Sub CancelArchiveProgress()
			Dim sourcefiles() As String = Me.sourceFiles
			Using archive As New ZipArchive()
				AddHandler archive.Progress, AddressOf archive_Progress
				For Each file As String In Me.sourceFiles
					archive.AddFile(file, "/")
				Next file
				archive.Save("Documents\CancelArchiveProgress.zip")
			End Using
		End Sub

		Private Sub archive_Progress(ByVal sender As Object, ByVal args As ProgressEventArgs)
			args.CanContinue = Not Me.stopProgress
		End Sub
		#End Region ' #cancelarchiveprogress

		#Region "#protectpassword"
		Public Sub ProtectPassword()
			Dim sourcefiles() As String = Me.sourceFiles
			Dim password As String = "123"
			Using archive As New ZipArchive()
				For Each file As String In Me.sourceFiles
					Dim zipFI As ZipFileItem = archive.AddFile(file, "/")
					zipFI.EncryptionType = EncryptionType.Aes128
					zipFI.Password = password
				Next file
				archive.Save("Documents\ProtectPassword.zip")
			End Using
		End Sub
		#End Region ' #protectpassword

		#Region "#addcomment"
		Public Sub ArchiveWithComment()
			Dim path As String = Me.startupPath
			Using archive As New ZipArchive()
				For Each file As String In System.IO.Directory.EnumerateFiles(path)
					Dim zipFI As ZipFileItem = archive.AddFile(file, "/")
					zipFI.Comment = "Archived by " & Environment.UserName
				Next file
				archive.Save("Documents\ArchiveWithComment.zip")

			End Using
		End Sub
		#End Region ' #addcomment

		#Region "#archivestream"
		Public Sub ArchiveStream()
			Using myStream As Stream = New MemoryStream(System.Text.Encoding.UTF8.GetBytes("DevExpress"))
				Using myZippedStream As Stream = New FileStream("Documents\ArchiveStream.zip", System.IO.FileMode.Create)
					Using archive As New ZipArchive()
						archive.AddStream("myStream", myStream)
						archive.Save(myZippedStream)
					End Using
				End Using
			End Using
		End Sub
		#End Region ' #archivestream

		#Region "#archivebytearray"
		Public Sub ArchiveByteArray()
			Dim myByteArray() As Byte = Enumerable.Repeat(CByte(&H78), 10000).ToArray()
			Using myZippedStream As Stream = New FileStream("Documents\ArchiveByteArray.zip", FileMode.Create)
				Using archive As New ZipArchive()
					archive.AddByteArray("myByteArray", myByteArray)
					archive.Save(myZippedStream)
				End Using
			End Using
		End Sub
		#End Region ' #archivebytearray

		#Region "#unziparchive"
		Public Sub UnzipArchive()
			Dim pathToZipArchive As String = "Documents\Example.zip"
			Dim pathToExtract As String = "Documents\!Extracted"
			Using archive As ZipArchive = ZipArchive.Read(pathToZipArchive)
				For Each item As ZipItem In archive
					item.Extract(pathToExtract)
				Next item
			End Using
		End Sub
		#End Region ' #unziparchive

		#Region "#unziparchiveconflict"
		Public Sub UnzipArchiveConflict()
			Dim pathToZipArchive As String = "Documents\Example.zip"
			Dim pathToExtract As String = "Documents\!Extracted"
			Using archive As ZipArchive = ZipArchive.Read(pathToZipArchive)
				archive.OptionsBehavior.AllowFileOverwrite = AllowFileOverwriteMode.Custom
				AddHandler archive.AllowFileOverwrite, AddressOf archive_AllowFileOverwrite
				For Each item As ZipItem In archive
					item.Extract(pathToExtract)
				Next item
			End Using
		End Sub

		Private Sub archive_AllowFileOverwrite(ByVal sender As Object, ByVal e As AllowFileOverwriteEventArgs)
			Dim fi As New FileInfo(e.TargetFilePath)
			If e.ZipItem.LastWriteTime < fi.LastWriteTime Then
				e.Cancel = True
			End If
		End Sub
		#End Region ' #unziparchiveconflict

		#Region "#archivetext"
		Public Sub ArchiveText()
			Using archive As New ZipArchive()
				archive.AddText("Text_DE.txt", "Komprimieren großer Dateien mühelos")
				archive.Save("Documents\ArchiveText.zip")
			End Using
		End Sub
		#End Region ' #archivetext

		#Region "#addfiletoarchive"
		Public Sub AddFileToArchive()
			Dim stream As New MemoryStream()
			Dim sourcefiles() As String = Me.sourceFiles
			Dim pathToZipArchive As String = "Documents\Example.zip"

			Using fs As FileStream = File.Open(pathToZipArchive, FileMode.Open)
				fs.CopyTo(stream)
				fs.Close()
			End Using
			stream.Seek(0, SeekOrigin.Begin)
			Using archive As ZipArchive = ZipArchive.Read(stream, System.Text.Encoding.Default, False)
				For Each sfile As String In sourcefiles
					archive.AddFile(sfile, "/")
				Next sfile
				archive.Save(pathToZipArchive)
			End Using
		End Sub
		#End Region ' #addfiletoarchive
	End Class
End Namespace
