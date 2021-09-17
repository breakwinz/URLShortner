using Dapper;
using DataLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess
{
    public class URLShortnerDBRepository : IURLShortnerDBRepository
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Repository for database commands for URLShortnerDB
        /// </summary>
        /// <param name="configuration"></param>
        public URLShortnerDBRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Selects a list of records filtered by shortUrl column
        /// </summary>
        /// <param name="shortURLModel">ShortURLModel to fetch shortUrl property from</param>
        /// <returns></returns>
        public List<ShortURLModel> LoadOriginalURLFromShortURL(ShortURLModel shortURLModel)
        {
            string sql = $@"SELECT * FROM dbo.shorturl WHERE shortUrl = @shortUrl";
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<ShortURLModel>(sql, shortURLModel).ToList();
            }
        }

        /// <summary>
        ///Increments clicks column for records filtered by shortUrl column
        /// </summary>
        /// <param name="shortURLModel">ShortURLModel to fetch shortUrl property from</param>
        /// <returns>The number of rows affected</returns>
        public int IncrementClicksFromShortUrl(ShortURLModel shortURLModel)
        {
            string sql = @"UPDATE dbo.shorturl SET clicks = clicks + 1 WHERE shortUrl = @shortUrl";
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Execute(sql, shortURLModel);
            }
        }

        /// <summary>
        /// Insert a new record into shortUrl table
        /// </summary>
        /// <param name="shortUrlModel">Model of the table columns</param>
        /// <returns>The number of rows affected</returns>
        public int InsertNewRecord(ShortURLModel shortUrlModel)
        {
            string sql = @"INSERT INTO dbo.shorturl (originalUrl, shortUrl, clicks, created)
                           VALUES (@originalUrl, @shortUrl, @clicks, @created);";
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Execute(sql, shortUrlModel);
            }
        }

        public string GetConnectionString(string connectionName = "URLShortnerDB")
        {
            return this._configuration.GetConnectionString(connectionName);
        }
    }
}
