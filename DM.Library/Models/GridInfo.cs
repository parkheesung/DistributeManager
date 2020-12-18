using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DM.Library
{
    public class GridInfo
    {
        public string FullPath { get; set; } = string.Empty;

        public DateTime LastUpdate { get; set; }

        public DateTime RegistDate { get; set; }

        public string FileName { get; set; } = string.Empty;

        public long Size { get; set; } = 0;

        public string Ext
        {
            get
            {
                string result = string.Empty;
                if (!string.IsNullOrWhiteSpace(this.FileName))
                {
                    if (this.FileName.LastIndexOf('.') > -1)
                    {
                        result = this.FileName.Substring(this.FileName.LastIndexOf('.')).Replace(".","");

                    }
                }
                return result;
            }
        }

        public GridInfo()
        {

        }

        public GridInfo(FileInfo fi)
        {
            this.FullPath = fi.FullName;
            this.LastUpdate = fi.LastWriteTime;
            this.RegistDate = fi.CreationTime;
            this.FileName = fi.Name;
            this.Size = fi.Length;
        }
    }
}
