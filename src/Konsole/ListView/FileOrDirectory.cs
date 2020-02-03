using System;
using System.IO;
using System.Linq;
using Konsole.Internal;

namespace Konsole
{
    public class FileOrDirectory
    {
        public static string DefaultDateTimeFormat = "dd MMM yyyy hh:mm"; 
        public enum Me {  File, Directory }
        
        /// <summary>
        /// file or directory extension
        /// </summary>
        public string Ext { get; }

        /// <summary>
        /// full name including path
        /// </summary>
        public string FullName { get; }
        
        /// <summary>
        /// name only, no path
        /// </summary>
        public String Name { get; }
        
        /// <summary>
        /// size in bytes
        /// </summary>
        public long Size { get; }

        /// <summary>
        /// display text for size, in bytes, Kb, Mb, Terabytes etc.
        /// </summary>
        public string SizeText { get; }
        public DateTime LastModifiedUTC { get; }

        /// <summary>
        /// 17 char display text
        /// </summary>
        /// <example>25 May 2020 21:41</example>
        public string LastModifiedText { get; }

        public Me Is { get; }
        public FileOrDirectory(FileInfo file, DirectoryInfo dir)
        {
            if (file == null && dir == null) throw new ArgumentNullException("Please supply either a file or a directory.");
            if (file != null)
            {
                Ext = file.Extension;
                FullName = file.FullName;
                Name = file.Name;
                Size = file.Length;
                SizeText = Size.BytesToSize();
                LastModifiedUTC = file.LastWriteTimeUtc;
                LastModifiedText = DefaultDateTimeFormat;
                Is = Me.File;
            }
            else
            {
                Ext = dir.Extension;
                FullName = dir.FullName;
                Name = dir.Name;
                Size = dir.GetFiles("*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t.FullName).Length));
                SizeText = Size.BytesToSize();
                LastModifiedUTC = dir.LastWriteTimeUtc;
                LastModifiedText = DefaultDateTimeFormat;
                Is = Me.Directory;
            }
        }

        public FileOrDirectory(Me @is, string ext, string fullName, string name, long size, DateTime lastModifiedUTC)
        {
            Ext = ext;
            FullName = fullName;
            Name = name;
            Size = size;
            SizeText = Size.BytesToSize();
            LastModifiedUTC = lastModifiedUTC;
            LastModifiedText = DefaultDateTimeFormat;
            Is = @is;
        }
    }
}

