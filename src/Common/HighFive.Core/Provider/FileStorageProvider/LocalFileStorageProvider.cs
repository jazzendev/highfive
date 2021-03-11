using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Core.Provider
{
    public class LocalFileStorageProvider : IFileStorageProvider
    {
        private const string ROOT_DIRECTORY = "wwwroot/";
        private const string LOCAL_FILE_DIRECTORY = "FileStorage";
        public LocalFileStorageProvider()
        {
            var di = new DirectoryInfo(ROOT_DIRECTORY + LOCAL_FILE_DIRECTORY);
            if (!di.Exists)
            {
                di.Create();
            }
        }

        public Task<bool> DeleteFileAsync(string filename)
        {
            FileInfo fi = new FileInfo(ROOT_DIRECTORY + filename);
            fi.Delete();

            return Task.FromResult(true);
        }

        public Task<IEnumerable<string>> ListFileUrlsAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SaveFileAsync(string filename, string path, Stream stream, long length)
        {
            var filePath = LOCAL_FILE_DIRECTORY + '/' + (path.Last() == '/' ? path : path + "/");

            var di = new DirectoryInfo(ROOT_DIRECTORY + filePath);
            if (!di.Exists)
            {
                di.Create();
            }

            return await SaveFileAsync(filePath + filename, stream, length);
        }

        public async Task<string> SaveFileAsync(string path, Stream stream, long length)
        {
            using (var fs = new FileStream(ROOT_DIRECTORY + path, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fs);
            }

            return path;
        }
    }
}
