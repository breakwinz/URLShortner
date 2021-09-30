using System;

namespace DataLibrary.Interfaces
{
    public interface IShortUrlProcessor
    {
        string GetOriginalUrl(string shortUrl);
        void UpdateClickCount(string shortUrl);
        Tuple<bool, string> CreateShortUrl(string originalUrl, string shortUrl);
        string ValidateSchemaOnURL(string url);
        bool CheckIfCustomUrlExists(string shortUrl);

    }
}
