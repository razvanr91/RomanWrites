using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RomanWrites.Services
{
    public interface IImageService
    {
        Task<byte[]> EncodeImageAsync(IFormFile file);

        Task<byte[]> EncodeImageAsync(string fileName);

        string DecodeImage(byte[] imageData, string contentType);

        string ContentType(IFormFile file);

        int Size(IFormFile file);
    }
}
