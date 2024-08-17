using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.DataAccess.Interfaces
{
    public interface IImagesRepository
    {
        Task<string> AddImage(byte[] bytes);
        Task<byte[]> GetImageAsync(string id);
        Task UpdateImageAsync(string id, byte[] bytes);
        Task DeleteImage(string id);
        bool IsExists(string imageId);
    }
}
