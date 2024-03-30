using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.DataAccess.Repository.IRepository
{
    public interface IThumbnailService
    {
        void GenerateThumbnail(int size, string inputPath, string outputPath);
        //void ResizeImage(string inputPath, string outputPath);
    }
}
