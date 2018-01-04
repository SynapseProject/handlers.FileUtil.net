﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Synapse.Filesystem
{
    public abstract class SynapseFile
    {
        public abstract String Name { get; }
        public abstract String FullName { get; set; }

        public SynapseFile() { }

        public SynapseFile(string fullName)
        {
            FullName = fullName;
        }

        public abstract SynapseFile Create(string fileName = null, String callbackLabel = null, Action<string, string> callback = null);
        public abstract void Delete(string fileName = null, bool verbose = true, String callbackLabel = null, Action<string, string> callback = null);
        public abstract bool Exists(string fileName = null);

        public abstract SynapseDirectory CreateDirectory(string dirName, String callbackLabel = null, Action<string, string> callback = null);

        public abstract Stream OpenStream(AccessType access, String callbackLabel = null, Action<string, string> callback = null);
        public abstract void CloseStream(String callbackLabel = null, Action<string, string> callback = null);

        public void CopyTo(SynapseFile file, bool overwrite = true, bool verbose = true, String callbackLabel = null, Action<string, string> callback = null)
        {
            Stream source = this.OpenStream(AccessType.Read);
            Stream target = file.OpenStream(AccessType.Write);

            source.CopyTo(target);

            this.CloseStream();
            file.CloseStream();

            if (verbose)
                Logger.Log($"Copied File [{this.FullName}] to [{file.FullName}].", callbackLabel, callback);
        }

        public void MoveTo(SynapseFile file, bool overwrite = true, bool verbose = true, String callbackLabel = null, Action<string, string> callback = null)
        {
            CopyTo(file, true, false);
            this.Delete();
            if (verbose)
                Logger.Log($"Moved File [{this.FullName}] to [{file.FullName}].", callbackLabel, callback);
        }

        public void CopyTo(SynapseDirectory dir, bool overwrite = true, bool verbose = true, String callbackLabel = null, Action<string, string> callback = null)
        {
            String targetFilePath = dir.PathCombine(dir.FullName, this.Name);
            SynapseFile targetFile = dir.CreateFile(targetFilePath);
            CopyTo(targetFile, overwrite, false);
            if (verbose)
                Logger.Log($"Copied File [{this.FullName}] to [{dir.FullName}].", callbackLabel, callback);
        }

        public void MoveTo(SynapseDirectory dir, bool overwrite = true, bool verbose = true, String callbackLabel = null, Action<string, string> callback = null)
        {
            CopyTo(dir, overwrite, false);
            this.Delete();
            if (verbose)
                Logger.Log($"Moved File [{this.FullName}] to [{dir.FullName}].", callbackLabel, callback);
        }
    }
}

