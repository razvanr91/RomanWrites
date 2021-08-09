﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RomanWrites.Services
{
    public class ImageService : IImageService
    {
        public string ContentType(IFormFile file)
        {
            return file?.ContentType;
        }

        public string DecodeImage(byte[] imageData, string contentType)
        {
            if ( imageData is null || contentType is null )
                return null;

            return $"data:image/{contentType};base64,{Convert.ToBase64String(imageData)}";
        }

        public async Task<byte[]> EncodeImageAsync(IFormFile file)
        {
            if ( file is null )
                return null;

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> EncodeImageAsync(string fileName)
        {
            var file = $"{Directory.GetCurrentDirectory()}/wwwroot/assets/img/{fileName}";

            return await File.ReadAllBytesAsync(file);
        }

        public int Size(IFormFile file)
        {
            return Convert.ToInt32(file?.Length);
        }
    }
}
