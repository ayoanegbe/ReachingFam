using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Interfaces
{
    public interface IFileService
    {
        bool SaveFile(List<IFormFile> files, string subDirectory);
        Task<bool> SaveFile(IFormFile file, string subDirectory);
        Task<string> SaveFile(IFormFile file, string mainDirectory, string subDirectory);
        Task<(string filePath, string message)> SaveFile(IFormFile file, string mainDirectory, string subDirectory, string[] extensions = null);
        (string fileType, byte[] archiveData, string archiveName) FetchFiles(string subDirectory);
        string SizeConverter(long bytes);
        byte[] ToZip(List<IFormFile> files);
        string CheckInvalidChars(string fileNme);
        string FileResize(string fileName, string filePath, int width, int height);
        string GetContentType(string path);
        Task<(string filePath, string thumbFilePath, string message)> ProcessFile(IFormFile file, string mainDirectory, string subDirectory);
    }
}
