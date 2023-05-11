#region #usings
using DevExpress.Compression;
#endregion #usings
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CompressionLibraryExamples {
    class ZipExamples {
        private string startupPath;
        private string[] sourceFiles;

        public ZipExamples(string startupPath, string[] sourceFiles) {
            this.startupPath = startupPath;
            this.sourceFiles = sourceFiles;
        }
        #region #archivedirectory
        public void ArchiveDirectory() {
            string path = this.startupPath;
            using (ZipArchive archive = new ZipArchive()) {
                archive.AddDirectory(path);
                archive.Save("Documents\\ArchiveDirectory.zip");
            }
        }
        #endregion #archivedirectory

        #region #archivefiles
        public void ArchiveFiles() {
            string[] sourcefiles = this.sourceFiles;
            string[] ext = new string[] {".txt", ".docx"};
            using (ZipArchive archive = new ZipArchive()) {
                foreach (string file in sourcefiles) {
                    if (ext.Contains(Path.GetExtension(file)))
                    archive.AddFile(file, "/");
                }
                archive.Save("Documents\\ArchiveFiles.zip");
            }
        }
        #endregion #archivefiles

        #region #archivedirectoryhandlingerrors
        public void ArchiveDirectoryWithError() {
            string path = this.startupPath;
            using (ZipArchive archive = new ZipArchive()) {
                archive.Error += archive_Error;
                archive.AddDirectory(path);
                archive.Save("Documents\\ArchiveDirectoryWithError.zip");
            }
        }

        private void archive_Error(object sender, DevExpress.Compression.ErrorEventArgs args) {
            string errorMessage;
            Exception e = args.GetException();
            if (String.IsNullOrEmpty(args.ItemName)) {
                errorMessage = e.Message;
            }
            else {
                errorMessage = String.Format("Item: {0}\n\nDescription:\n{1}", args.ItemName, e.Message);
            }
            string descriptionMessage = "Click Cancel to abort operation. Click OK to skip the item and continue.";
            string message = String.Format("{0}\n{1}", errorMessage, descriptionMessage);
            if (MessageBox.Show(message, "Error", MessageBoxButtons.OKCancel) == DialogResult.Cancel) {
                args.CanContinue = false;
            }
        }
        #endregion #archivedirectoryhandlingerrors

        #region #filterarchivefiles
        volatile bool stopArchiving = false;
        public void FilterArchiveFiles() {
            string[] sourcefiles = this.sourceFiles;
            using (ZipArchive archive = new ZipArchive()) {
                archive.ItemAdding += archive_ItemAdding;
                foreach (string file in sourceFiles) {
                    archive.AddFile(file, "/");
                }
                archive.Save("Documents\\FilterArchiveFiles.zip");
            }
        }

        private void archive_ItemAdding(object sender, ZipItemAddingEventArgs args) {
            if (args.Item.CreationTime.Date != DateTime.Today)
                args.Action = ZipItemAddingAction.Cancel;
            if (stopArchiving) args.Action = ZipItemAddingAction.Stop;
        }
        #endregion #filterarchivefiles

        #region #archivefilesbatch
        public void ArchiveFilesBatch() {
            string path = this.startupPath;
            using (ZipArchive archive = new ZipArchive()) {
                var files = from file in System.IO.Directory.EnumerateFiles(path, "*.xml",
                                System.IO.SearchOption.AllDirectories)
                            from line in System.IO.File.ReadLines(file)
                            where line.Contains("DevExpress")
                            select file;
                archive.AddFiles(files);
                archive.Save("Documents\\ArchiveFilesBatch.zip");
            }
        }
        #endregion #archivefilesbatch

        #region #cancelarchiveprogress
        volatile bool stopProgress = false;

        public void CancelArchiveProgress() {
            string[] sourcefiles = this.sourceFiles;
            using (ZipArchive archive = new ZipArchive()) {
                archive.Progress += archive_Progress;
                foreach (string file in sourceFiles) {
                    archive.AddFile(file, "/");
                }
                archive.Save("Documents\\CancelArchiveProgress.zip");
            }
        }

        private void archive_Progress(object sender, ProgressEventArgs args) {
            args.CanContinue = !this.stopProgress;
        }
        #endregion #cancelarchiveprogress

        #region #protectpassword
        public void ProtectPassword() {
            string[] sourcefiles = this.sourceFiles;
            string password = "123";
            using (ZipArchive archive = new ZipArchive()) {
                foreach (string file in sourceFiles) {
                    ZipFileItem zipFI = archive.AddFile(file, "/");
                    zipFI.EncryptionType = EncryptionType.Aes128;
                    zipFI.Password = password;
                }
                archive.Save("Documents\\ProtectPassword.zip");
            }
        }
        #endregion #protectpassword

        #region #addcomment
        public void ArchiveWithComment() {
            string path = this.startupPath;
            using (ZipArchive archive = new ZipArchive()) {
                foreach (string file in System.IO.Directory.EnumerateFiles(path)) {
                    ZipFileItem zipFI = archive.AddFile(file, "/");
                    zipFI.Comment = "Archived by " + Environment.UserName;
                }
                archive.Save("Documents\\ArchiveWithComment.zip");
                
            }
        }
        #endregion #addcomment

        #region #archivestream
        public void ArchiveStream() {
            using (Stream myStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("DevExpress"))) {
                using (Stream myZippedStream = new FileStream("Documents\\ArchiveStream.zip", System.IO.FileMode.Create)) {
                    using (ZipArchive archive = new ZipArchive()) {
                        archive.AddStream("myStream", myStream);
                        archive.Save(myZippedStream);
                    }
                }
            }
        }
        #endregion #archivestream

        #region #archivebytearray
        public void ArchiveByteArray() {
            byte[] myByteArray = Enumerable.Repeat((byte)0x78, 10000).ToArray();
            using (Stream myZippedStream = new FileStream("Documents\\ArchiveByteArray.zip", FileMode.Create)) {
                using (ZipArchive archive = new ZipArchive()) {
                    archive.AddByteArray("myByteArray", myByteArray);
                    archive.Save(myZippedStream);
                }
            }
        }
        #endregion #archivebytearray

        #region #unziparchive
        public void UnzipArchive() {
            string pathToZipArchive = "Documents\\Example.zip";
            string pathToExtract = "Documents\\!Extracted";
            using (ZipArchive archive = ZipArchive.Read(pathToZipArchive)) {
                foreach (ZipItem item in archive) {
                    item.Extract(pathToExtract);
                }
            }
        }
        #endregion #unziparchive

        #region #unziparchiveconflict
        public void UnzipArchiveConflict() {
            string pathToZipArchive = "Documents\\Example.zip";
            string pathToExtract = "Documents\\!Extracted";
            using (ZipArchive archive = ZipArchive.Read(pathToZipArchive)) {
                archive.OptionsBehavior.AllowFileOverwrite = AllowFileOverwriteMode.Custom;
                archive.AllowFileOverwrite += archive_AllowFileOverwrite;
                foreach (ZipItem item in archive) {
                    item.Extract(pathToExtract);
                }
            }
        }

        private void archive_AllowFileOverwrite(object sender, AllowFileOverwriteEventArgs e) {
            FileInfo fi = new FileInfo(e.TargetFilePath);
            if (e.ZipItem.LastWriteTime < fi.LastWriteTime) e.Cancel = true;
        }
        #endregion #unziparchiveconflict

        #region #archivetext
        public void ArchiveText() {
            using (ZipArchive archive = new ZipArchive()) {
                archive.AddText("Text_DE.txt", "Komprimieren großer Dateien mühelos");
                archive.Save("Documents\\ArchiveText.zip");
            }
        }
        #endregion #archivetext

        #region #addfiletoarchive
        public void AddFileToArchive() {
            MemoryStream stream = new MemoryStream();
            string[] sourcefiles = this.sourceFiles;
            string pathToZipArchive = "Documents\\Example.zip";

            using (FileStream fs = File.Open(pathToZipArchive, FileMode.Open)) {
                fs.CopyTo(stream);
                fs.Close();
            }
            stream.Seek(0, SeekOrigin.Begin);
            using (ZipArchive archive = ZipArchive.Read(stream, System.Text.Encoding.Default, false)) {
                foreach (string sfile in sourcefiles) {
                    archive.AddFile(sfile, "/");
                }
                archive.Save(pathToZipArchive);
            }
        }
        #endregion #addfiletoarchive

        #region #Invoke_Methods
        public MethodInfo[] GetMethods()
        {
            return GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public void InvokeMethod(string methodName, List<object> args)
        {
            GetType().GetMethod(methodName).Invoke(this, args.ToArray());
        }
        #endregion #InvokeMethods
    }
}
