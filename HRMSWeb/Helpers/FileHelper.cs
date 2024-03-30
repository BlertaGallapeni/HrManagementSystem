using HRMS.DataAccess.Repository.IRepository;
using HRMS.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HRMSWeb.Helpers
{
    public class FileHelper : IFileHelper
    {
        private IWebHostEnvironment _env;
        private IThumbnailService _thumbnailService;

        public FileHelper(IWebHostEnvironment env, IThumbnailService thumbnailService)
        {
            _env = env;
            _thumbnailService = thumbnailService;
        }

        public string GetProperFilePath(FileTypes type, Thumbnails thumbnail, string path)
        {
            var properPath = "";
            try
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                var fileExtension = Path.GetExtension(path);

                var pathWithoutFileName = Path.GetDirectoryName(path);
                var test = Path.GetFullPath(path);

                var newFileName = $"{fileNameWithoutExtension}_{(int)thumbnail}{fileExtension}";


                var pathArray = path.Split('/');

                pathArray[pathArray.Length - 1] = newFileName;

                properPath = string.Join('/', pathArray);

                var absolutePath = pathWithoutFileName.Substring(1) + "\\" + newFileName;
                absolutePath = _env.WebRootPath + absolutePath;
                if (!File.Exists(absolutePath))
                {
                    throw new Exception();
                }

            }
            catch (Exception)
            {
                properPath = $"~/images/default-images/product-default_{(int)thumbnail}.jpg";
            }


            return properPath;

        }
        public string GetProperFilePath(FileTypes type, Thumbnails thumbnail, string path, bool forLogo)
        {
            var properPath = "";
            try
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                var fileExtension = Path.GetExtension(path);

                var pathWithoutFileName = Path.GetDirectoryName(path);
                var test = Path.GetFullPath(path);

                var newFileName = $"{fileNameWithoutExtension}_{(int)thumbnail}{fileExtension}";


                var pathArray = path.Split('/');

                pathArray[pathArray.Length - 1] = newFileName;

                properPath = string.Join('/', pathArray);

                var absolutePath = pathWithoutFileName.Substring(1) + "\\" + newFileName;
                absolutePath = _env.WebRootPath + absolutePath;
                if (!File.Exists(absolutePath))
                {
                    throw new Exception();
                }

            }
            catch (Exception)
            {
                properPath = null;
            }


            return properPath;

        }
        public string GetFavIconFilePath(string path)
        {
            string properPath;
            try
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                var fileExtension = Path.GetExtension(path);

                var pathWithoutFileName = Path.GetDirectoryName(path);
                var test = Path.GetFullPath(path);
                var newFileName = Path.GetFileName(path);

                //var newFileName = $"{fileNameWithoutExtension}_{(int)thumbnail}{fileExtension}";


                var pathArray = path.Split('/');

                pathArray[pathArray.Length - 1] = newFileName;

                properPath = string.Join('/', pathArray);

                var absolutePath = pathWithoutFileName.Substring(1) + "\\" + newFileName;
                absolutePath = _env.WebRootPath + absolutePath;
                if (!File.Exists(absolutePath))
                {
                    throw new Exception();
                }

            }
            catch (Exception)
            {
                properPath = null;
            }


            return properPath;

        }
        public void SaveFile(FileTypes type, IFormFile file, string folderName, string id, params int[] thumbnails)
        {
            var filePath = Path.Combine(_env.WebRootPath, "images", folderName, id);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            else
            {
                Directory.Delete(filePath, true);
                Directory.CreateDirectory(filePath);

            }

            new FileInfo(filePath).Directory.Create();


                using (var fileStream = new FileStream(Path.Combine(filePath, file.FileName), FileMode.Create))
            {
                file.CopyTo(fileStream);

                fileStream.Close();
            }
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
            var fileExtenstion = Path.GetExtension(file.FileName);


            foreach (var item in thumbnails)
            {
                string thumbnailPath = Path.Combine(filePath, $"{fileNameWithoutExtension}_{item}{fileExtenstion}");
                _thumbnailService.GenerateThumbnail(item, Path.Combine(filePath, file.FileName), thumbnailPath);
            }
        }

        public void SaveFavIcon(FileTypes type, IFormFile file, string folderName, string id)
        {
            var filePath = Path.Combine(_env.WebRootPath, "images/userProfilePic", folderName, id);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
           new FileInfo(filePath).Directory.Create();
            using (var fileStream = new FileStream(Path.Combine(filePath, file.FileName), FileMode.Create))
            {
                file.CopyTo(fileStream);

                fileStream.Close();
            }
        }
        public void SaveImage(FileTypes type, IFormFile file, string folderName, string id)
        {
            var filePath = Path.Combine(_env.WebRootPath, "images/userProfilePic", folderName, id);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            new FileInfo(filePath).Directory.Create();
            using (var fileStream = new FileStream(Path.Combine(filePath, file.FileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
                fileStream.Close();
            }

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
            var fileExtenstion = Path.GetExtension(file.FileName);

        }

    }
}
