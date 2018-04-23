using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#region #usings
using DevExpress.Compression;
#endregion #usings

namespace CompressionLibraryExamples
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        #region #archivedirectory
        void ArchiveDirectory(string path)
        {
            using (ZipArchive archive = new ZipArchive())
            {
                archive.AddDirectory(path);
                archive.Save("ZipDirectory.zip");
            }
        }
        #endregion #archivedirectory

        #region #archivefiles
        void ArchiveFiles(string[] sourceFiles)
        {
            using (ZipArchive archive = new ZipArchive())
            {
                foreach (string file in sourceFiles)
                {
                    archive.AddFile(file, "/");
                }
                archive.Save("ZipFiles.zip");
            }
        }
        #endregion #archivefiles

        #region #archivedirectoryhandlingerrors
        void ArchiveDirectoryWithError(string path)
        {
            using (ZipArchive archive = new ZipArchive())
            {
                archive.Error += archive_Error;
                archive.AddDirectory(path);
                archive.Save("ZipDirectory.zip");
            }
        }

        void archive_Error(object sender, ErrorEventArgs args)
        {
            string errorMessage;
            Exception e = args.GetException();
            if (String.IsNullOrEmpty(args.ItemName))
            {
                errorMessage = e.Message;
            }
            else
            {
                errorMessage = String.Format("Item: {0}\n\nDescription:\n{1}", args.ItemName, e.Message);
            }
            string descriptionMessage = "Click Cancel to abort operation. Click OK to skip the item and continue.";
            string message = String.Format("{0}\n{1}", errorMessage, descriptionMessage);
            if (MessageBox.Show(message, "Error", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                args.CanContinue = false;
            }
        }
        #endregion #archivedirectoryhandlingerrors


        #region #filterarchivefiles
        volatile bool stopArchiving = false; 
        void FilterArchiveFiles(string[] sourceFiles)
        {
            using (ZipArchive archive = new ZipArchive())
            {
                archive.ItemAdding += archive_ItemAdding;
                foreach (string file in sourceFiles)
                {
                    archive.AddFile(file, "/");
                }
                archive.Save("ZipFilterFiles.zip");
            }
        }

        void archive_ItemAdding(object sender, ZipItemAddingEventArgs args)
        {
            if (args.Item.CreationTime.Date != DateTime.Today)
                args.Action = ZipItemAddingAction.Cancel;
            if (stopArchiving) args.Action = ZipItemAddingAction.Stop;
        }
        #endregion #filterarchivefiles

        #region #archivefilesbatch
        void ArchiveFilesBatch(string path)
        {
            using (ZipArchive archive = new ZipArchive())
            {
                var files = from file in System.IO.Directory.EnumerateFiles(path, "*.txt",
                                System.IO.SearchOption.AllDirectories)
                            from line in System.IO.File.ReadLines(file)
                            where line.Contains("DevExpress")
                            select file;
                archive.AddFiles(files);
                archive.Save("ZipFilesDX.zip");
            }
        }
        #endregion #archivefilesbatch

        #region #cancelarchiveprogress
        volatile bool stopProgress = false;
        void CancelArchiveProgress(string[] sourceFiles)
        {
            using (ZipArchive archive = new ZipArchive())
            {
                archive.Progress += archive_Progress;
                foreach (string file in sourceFiles)
                {
                    archive.AddFile(file, "/");
                }
                archive.Save("ZipCancelFiles.zip");
            }
        }

        void archive_Progress(object sender, ProgressEventArgs args)
        {
            args.CanContinue = !this.stopProgress;
        }
        #endregion #cancelarchiveprogress

        #region #protectpassword
        void ProtectPassword(string[] sourceFiles, string password)
        {
            using (ZipArchive archive = new ZipArchive())
            {
                foreach (string file in sourceFiles)
                {
                    ZipFileItem zipFI = archive.AddFile(file, "/");
                    zipFI.EncryptionType = EncryptionType.Aes128;
                    zipFI.Password = password + System.IO.Path.GetFileName(file).Substring(0, 1); 
                }
                archive.Save("ZipEncryptedFiles.zip");
            }
        }
        #endregion #protectpassword

        #region #addcomment
        void ArchiveWithComment(string path)
        {
            using (ZipArchive archive = new ZipArchive())
            {
                foreach (string file in System.IO.Directory.EnumerateFiles(path))
                {
                    ZipFileItem zipFI = archive.AddFile(file, "/");
                    zipFI.Comment = "Archived by " + Environment.UserName;
                }
                archive.Save("ZipCommentedFiles.zip");
            }
        }
        #endregion #addcomment

        #region #archivestream
        void ArchiveStream(System.IO.Stream myStream, System.IO.Stream myZippedStream)
        {
            using (ZipArchive archive = new ZipArchive())
            {
                archive.AddStream("myStream", myStream);
                archive.Save(myZippedStream);
            }
        }
        #endregion #archivestream

        #region #archivebytearray
        void ArchiveByteArray(byte[] myByteArray, System.IO.Stream myZippedStream)
        {
            using (ZipArchive archive = new ZipArchive())
            {
                archive.AddByteArray("myByteArray", myByteArray);
                archive.Save(myZippedStream);
            }
        }
        #endregion #archivebytearray

        #region #unziparchive
        void UnzipArchive(string pathToZipArchive, string pathToExtract)
        {
            using (ZipArchive archive = ZipArchive.Read(pathToZipArchive))
            {
                foreach (ZipItem item in archive)
                {
                    item.Extract(pathToExtract);
                }
            }
        }
        #endregion #unziparchive

        #region #unziparchiveconflict
        void UnzipArchiveConflict(string pathToZipArchive, string pathToExtract)
        {
            using (ZipArchive archive = ZipArchive.Read(pathToZipArchive))
            {
                archive.OptionsBehavior.AllowFileOverwrite = AllowFileOverwriteMode.Custom;
                archive.AllowFileOverwrite += archive_AllowFileOverwrite;
                foreach (ZipItem item in archive)
                {
                    item.Extract(pathToExtract);
                }
            }
        }

        void archive_AllowFileOverwrite(object sender, AllowFileOverwriteEventArgs e)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(e.TargetFilePath);
            if (e.ZipItem.LastWriteTime <  fi.LastWriteTime) e.Cancel = true;
        }
        #endregion #unziparchiveconflict

        #region #archivetext
        void ArchiveText()
        {
            using (ZipArchive archive = new ZipArchive())
            {
                archive.AddText("Text_DE.txt", "Komprimieren großer Dateien mühelos");
                archive.Save("ZipText.zip");
            }
        }
        #endregion #archivetext
    }
}
