using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Attribute
{
    public class ValidateImageAttribute
    {
        public static bool IsValid(object value)
        {
            int MaxContentLength = 1024 * 1024 * 20; //Max 2 MB file
            string[] AllowedFileExtensions = new
            string[] {
                ".jpg",
                ".gif",
                ".png",
                ".bmp"
            };
            var file = value as IFormFile;
            if (file == null)
            {
                return false;
            }
            else if (!AllowedFileExtensions.Contains((file != null) ?
                file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower() : string.Empty))
            {
                return false;
            }
            else if (file.Length > MaxContentLength)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    
    }
}
