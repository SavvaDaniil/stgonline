using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Attribute
{
    public class ValidateVideoAttribute
    {
        public static bool IsValid(object value)
        {
            long MaxContentLength = 1024L * 1024L * 1024L * 10L; //Max 10 GB file
            string[] AllowedFileExtensions = new
            string[] {
                ".mp4",
                ".mov",
                ".webm"
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
