using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Core.Provider
{
    public interface IFileStorageProvider
    {
        Task<string> SaveFileAsync(string filename, string path, Stream stream, long length);
        Task<string> SaveFileAsync(string path, Stream stream, long length);
        Task<bool> DeleteFileAsync(string filename);
        Task<IEnumerable<string>> ListFileUrlsAsync(string path);
    }
}
