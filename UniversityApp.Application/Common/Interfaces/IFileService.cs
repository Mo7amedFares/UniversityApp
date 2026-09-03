using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Application.Common.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName,string folderName, CancellationToken cancellationToken = default);
        void DeleteFile(string fileUrl);
    }
}
