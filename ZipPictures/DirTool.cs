using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace ZipPictures
{
    class DirTool
    {
        public string[] GetAllSubDirectories(string stRootPath)
        {
            System.Collections.Specialized.StringCollection hStringCollection = (
                new System.Collections.Specialized.StringCollection()
            );

            hStringCollection.Add(stRootPath);

            try
            {
                // このディレクトリ内のすべてのサブディレクトリを検索する (再帰)
                foreach (string stDirPath in System.IO.Directory.GetDirectories(stRootPath))
                {
                    string[] stFilePathes = GetAllSubDirectories(stDirPath);

                    // 条件に合致したファイルがあった場合は、ArrayList に加える
                    if (stFilePathes != null)
                    {
                        hStringCollection.AddRange(stFilePathes);
                    }
                }
            }
            catch (Exception)
            {

            }

            // StringCollection を 1 次元の String 配列にして返す
            string[] stReturns = new string[hStringCollection.Count];
            hStringCollection.CopyTo(stReturns, 0);

            return stReturns;
        }

        public bool IsPicturesFolder(string folder)
        {
            bool bRet = false;

            int iCount = 0;
            foreach (string stFilePath in System.IO.Directory.GetFiles(folder))
            {
                string strExt = System.IO.Path.GetExtension(stFilePath).ToLower();
                if (strExt == ".jpg" || strExt == ".png" || strExt == ".jpeg" || strExt == ".avif")
                {
                    iCount++;
                }
            }

            if (iCount >= 10)
            {
                bRet = true;
            }

            string[] subFolders = System.IO.Directory.GetDirectories(folder);

            if (subFolders.Length > 0)
            {
                bRet = false;
            }

            return bRet;
        }

        public void createZip(string folder)
        {
            try
            {
                ZipFile.CreateFromDirectory(folder, createZipFileName(folder));
            }
            catch (Exception)
            {

            }
        }

        private string createZipFileName(string folder)
        {
            return folder + ".zip";
        }

    }
}
