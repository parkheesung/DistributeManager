using DM.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributeTool
{
    public class FolderFileHelper : IDisposable
    {

        public List<FileInfo> Files { get; set; } = new List<FileInfo>();

        public List<GridInfo> GetFiles

        {
            get
            {
                List<GridInfo> result = new List<GridInfo>();
                if (this.Files != null && this.Files.Count > 0)
                {
                    foreach (FileInfo fi in this.Files)
                    {
                        result.Add(new GridInfo(fi));
                    }
                }
                return result.OrderByDescending(x => x.LastUpdate).ToList();
            }
        }

        private bool disposedValue;

        private MainWindow Main { get; set; }

        public FolderFileHelper(MainWindow main)
        {
            this.Main = main;
        }

        public static void WriteFile(string originalFile, string targetFile)
        {
            if (!string.IsNullOrWhiteSpace(originalFile))
            {
                try
                {
                    using(var sw = new StreamWriter(originalFile, true))
                    {
                        sw.WriteLine(targetFile);
                        sw.Close();
                    }
                }
                catch
                {

                }
            }
        }


        public async Task GetAllFilesAsync(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (di.Exists)
                {
                    Files.AddRange(di.GetFiles());
                    foreach(var dd in di.GetDirectories())
                    {
                        await GetAllFilesAsync(dd.FullName);
                    }
                }
            }
        }

        public void GetAllFiles(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (di.Exists)
                {
                    Files.AddRange(di.GetFiles());
                    foreach (var dd in di.GetDirectories())
                    {
                        GetAllFiles(dd.FullName);
                    }
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                disposedValue = true;
            }
        }

        ~FolderFileHelper()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
