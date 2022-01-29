using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Attribute
{
    public class ValidateHomeworkVideoAttribute
    {
        public static bool IsValid(object value)
        {
            long MaxContentLength = 1024L * 1024L * 200L; //Max 200Мб file
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

        public static bool IsValidChunk(object value)
        {
            long MaxContentLength = 1024L * 1024L * 200L; //Max 200Мб file
            var file = value as IFormFile;
            if (file == null)
            {
                System.Diagnostics.Debug.WriteLine("IsValidChunk file = null");
                return false;
            }
            else if (file.Length > MaxContentLength)
            {
                System.Diagnostics.Debug.WriteLine("IsValidChunk MaxContentLength");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
