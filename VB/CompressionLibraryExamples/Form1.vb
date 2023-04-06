Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms
'#Region "#usings"
Imports DevExpress.Compression

'#End Region  ' #usings
Namespace CompressionLibraryExamples

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

'#Region "#archivedirectory"
        Private Sub ArchiveDirectory(ByVal path As String)
            Using archive As ZipArchive = New ZipArchive()
                archive.AddDirectory(path)
                archive.Save("ZipDirectory.zip")
            End Using
        End Sub

'#End Region  ' #archivedirectory
'#Region "#archivefiles"
        Private Sub ArchiveFiles(ByVal sourceFiles As String())
            Using archive As ZipArchive = New ZipArchive()
                For Each file As String In sourceFiles
                    archive.AddFile(file, "/")
                Next

                archive.Save("ZipFiles.zip")
            End Using
        End Sub

'#End Region  ' #archivefiles
'#Region "#archivedirectoryhandlingerrors"
        Private Sub ArchiveDirectoryWithError(ByVal path As String)
            Using archive As ZipArchive = New ZipArchive()
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
                errorMessage = String.Format("Item: {0}" & Microsoft.VisualBasic.Constants.vbLf & Microsoft.VisualBasic.Constants.vbLf & "Description:" & Microsoft.VisualBasic.Constants.vbLf & "{1}", args.ItemName, e.Message)
            End If

            Dim descriptionMessage As String = "Click Cancel to abort operation. Click OK to skip the item and continue."
            Dim message As String = String.Format("{0}" & Microsoft.VisualBasic.Constants.vbLf & "{1}", errorMessage, descriptionMessage)
            If MessageBox.Show(message, "Error", MessageBoxButtons.OKCancel) = DialogResult.Cancel Then
                args.CanContinue = False
            End If
'#End Region  ' #archivedirectoryhandlingerrors
'#Region "#filterarchivefiles"
        End Sub

         ''' Cannot convert FieldDeclarationSyntax, System.NotSupportedException: VolatileKeyword is not supported!
'''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass30_0.<ConvertModifiersCore>b__3(SyntaxToken x)
'''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
'''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
'''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
'''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor, Boolean isNestedType)
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitFieldDeclaration(FieldDeclarationSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''         #endregion #archivedirectoryhandlingerrors
''' 
''' 
'''         #region #filterarchivefiles
'''         volatile bool stopArchiving = false; 
''' 
'''  Private Sub FilterArchiveFiles(ByVal sourceFiles As String())
            Using archive As ZipArchive = New ZipArchive()
                AddHandler archive.ItemAdding, AddressOf archive_ItemAdding
                For Each file As String In sourceFiles
                    archive.AddFile(file, "/")
                Next

                archive.Save("ZipFilterFiles.zip")
            End Using
        End Sub

        Private Sub archive_ItemAdding(ByVal sender As Object, ByVal args As ZipItemAddingEventArgs)
            If args.Item.CreationTime.Date <> Date.Today Then args.Action = ZipItemAddingAction.Cancel
            If Me.stopArchiving Then args.Action = ZipItemAddingAction.Stop
        End Sub

'#End Region  ' #filterarchivefiles
'#Region "#archivefilesbatch"
        Private Sub ArchiveFilesBatch(ByVal path As String)
            Using archive As ZipArchive = New ZipArchive()
                Dim files = From file In IO.Directory.EnumerateFiles(path, "*.txt", IO.SearchOption.AllDirectories) From line In IO.File.ReadLines(file) Where line.Contains("DevExpress") Select file
                archive.AddFiles(files)
                archive.Save("ZipFilesDX.zip")
            End Using
'#End Region  ' #archivefilesbatch
'#Region "#cancelarchiveprogress"
        End Sub

         ''' Cannot convert FieldDeclarationSyntax, System.NotSupportedException: VolatileKeyword is not supported!
'''    at ICSharpCode.CodeConverter.VB.SyntaxKindExtensions.ConvertToken(SyntaxKind t, TokenContext context)
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifier(SyntaxToken m, TokenContext context)
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.<>c__DisplayClass30_0.<ConvertModifiersCore>b__3(SyntaxToken x)
'''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
'''    at System.Linq.Enumerable.WhereSelectEnumerableIterator`2.MoveNext()
'''    at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
'''    at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.ConvertModifiersCore(IReadOnlyCollection`1 modifiers, TokenContext context, Boolean isConstructor, Boolean isNestedType)
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitFieldDeclaration(FieldDeclarationSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''         #endregion #archivefilesbatch
''' 
'''         #region #cancelarchiveprogress
'''         volatile bool stopProgress = false;
''' 
'''  Private Sub CancelArchiveProgress(ByVal sourceFiles As String())
            Using archive As ZipArchive = New ZipArchive()
                AddHandler archive.Progress, AddressOf archive_Progress
                For Each file As String In sourceFiles
                    archive.AddFile(file, "/")
                Next

                archive.Save("ZipCancelFiles.zip")
            End Using
        End Sub

        Private Sub archive_Progress(ByVal sender As Object, ByVal args As ProgressEventArgs)
            args.CanContinue = Not Me.stopProgress
        End Sub

'#End Region  ' #cancelarchiveprogress
'#Region "#protectpassword"
        Private Sub ProtectPassword(ByVal sourceFiles As String(), ByVal password As String)
            Using archive As ZipArchive = New ZipArchive()
                For Each file As String In sourceFiles
                    Dim zipFI As ZipFileItem = archive.AddFile(file, "/")
                    zipFI.EncryptionType = EncryptionType.Aes128
                    zipFI.Password = password & IO.Path.GetFileName(file).Substring(0, 1)
                Next

                archive.Save("ZipEncryptedFiles.zip")
            End Using
        End Sub

'#End Region  ' #protectpassword
'#Region "#addcomment"
        Private Sub ArchiveWithComment(ByVal path As String)
            Using archive As ZipArchive = New ZipArchive()
                For Each file As String In IO.Directory.EnumerateFiles(path)
                    Dim zipFI As ZipFileItem = archive.AddFile(file, "/")
                    zipFI.Comment = "Archived by " & Environment.UserName
                Next

                archive.Save("ZipCommentedFiles.zip")
            End Using
        End Sub

'#End Region  ' #addcomment
'#Region "#archivestream"
        Private Sub ArchiveStream(ByVal myStream As IO.Stream, ByVal myZippedStream As IO.Stream)
            Using archive As ZipArchive = New ZipArchive()
                archive.AddStream("myStream", myStream)
                archive.Save(myZippedStream)
            End Using
        End Sub

'#End Region  ' #archivestream
'#Region "#archivebytearray"
        Private Sub ArchiveByteArray(ByVal myByteArray As Byte(), ByVal myZippedStream As IO.Stream)
            Using archive As ZipArchive = New ZipArchive()
                archive.AddByteArray("myByteArray", myByteArray)
                archive.Save(myZippedStream)
            End Using
        End Sub

'#End Region  ' #archivebytearray
'#Region "#unziparchive"
        Private Sub UnzipArchive(ByVal pathToZipArchive As String, ByVal pathToExtract As String)
            Using archive As ZipArchive = ZipArchive.Read(pathToZipArchive)
                For Each item As ZipItem In archive
                    item.Extract(pathToExtract)
                Next
            End Using
        End Sub

'#End Region  ' #unziparchive
'#Region "#unziparchiveconflict"
        Private Sub UnzipArchiveConflict(ByVal pathToZipArchive As String, ByVal pathToExtract As String)
            Using archive As ZipArchive = ZipArchive.Read(pathToZipArchive)
                archive.OptionsBehavior.AllowFileOverwrite = AllowFileOverwriteMode.Custom
                AddHandler archive.AllowFileOverwrite, AddressOf archive_AllowFileOverwrite
                For Each item As ZipItem In archive
                    item.Extract(pathToExtract)
                Next
            End Using
        End Sub

        Private Sub archive_AllowFileOverwrite(ByVal sender As Object, ByVal e As AllowFileOverwriteEventArgs)
            Dim fi As IO.FileInfo = New IO.FileInfo(e.TargetFilePath)
            If e.ZipItem.LastWriteTime < fi.LastWriteTime Then e.Cancel = True
        End Sub

'#End Region  ' #unziparchiveconflict
'#Region "#archivetext"
        Private Sub ArchiveText()
            Using archive As ZipArchive = New ZipArchive()
                archive.AddText("Text_DE.txt", "Komprimieren großer Dateien mühelos")
                archive.Save("ZipText.zip")
            End Using
        End Sub
'#End Region  ' #archivetext
    End Class
End Namespace
