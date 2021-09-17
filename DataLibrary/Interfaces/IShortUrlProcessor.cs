using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Models;

namespace DataLibrary.Interfaces
{
    public interface IShortUrlProcessor
    {
        string GetOriginalURL(string shortUrl);
        void UpdateClickCount(string shortUrl);
        Tuple<bool, string> CreateShortURL(string originalUrl, string shortUrl);
        string ValidateSchemaOnURL(string url);
        bool CheckIfCustomURLExists(string shortUrl);

    }
}
