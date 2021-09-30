using System.Collections.Generic;
using DataLibrary.Models;

namespace DataLibrary.Interfaces
{
    public interface IUrlShortnerDbRepository
    {
        string GetConnectionString(string connectionName = "URLShortnerDB");
        int IncrementClicksFromShortUrl(ShortUrlModel shortUrlModel);
        int InsertNewRecord(ShortUrlModel shortUrlModel);
        List<ShortUrlModel> LoadOriginalUrlFromShortUrl(ShortUrlModel shortUrlModel);
    }
}