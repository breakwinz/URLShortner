using DataLibrary.Models;
using System.Collections.Generic;

namespace DataLibrary.DataAccess
{
    public interface IURLShortnerDBRepository
    {
        string GetConnectionString(string connectionName = "URLShortnerDB");
        int IncrementClicksFromShortUrl(ShortURLModel shortURLModel);
        int InsertNewRecord(ShortURLModel shortUrlModel);
        List<ShortURLModel> LoadOriginalURLFromShortURL(ShortURLModel shortURLModel);
    }
}