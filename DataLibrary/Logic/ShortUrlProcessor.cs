using DataLibrary.Models;
using System;
using System.Linq;
using DataLibrary.Interfaces;

namespace DataLibrary.Logic
{
    public class ShortUrlProcessor : IShortUrlProcessor
    {
        private const int MinLengthOfShortLink = 4;
        private const int MaxLengthOfShortLink = 10;

        private readonly IUrlShortnerDbRepository _uRlShortnerDbRepository;

        public ShortUrlProcessor(IUrlShortnerDbRepository uRlShortnerDbRepository)
        {
            this._uRlShortnerDbRepository = uRlShortnerDbRepository;
        }

        /// <summary>
        /// Attempts to fetch originalUrl filtered by shortUrl and increments clicks if found
        /// </summary>
        /// <param name="shortUrl">shortUrl to filter by</param>
        /// <returns></returns>
        public string GetOriginalUrl(string shortUrl)
        {
            var lengthOfShortUrl = shortUrl.Length;

            if (MinLengthOfShortLink <= lengthOfShortUrl && lengthOfShortUrl >= MaxLengthOfShortLink)
            {
                return null;
            }
            ShortUrlModel data = new ShortUrlModel
            {
                shortUrl = shortUrl
            };
            var recordsFetched = _uRlShortnerDbRepository.LoadOriginalUrlFromShortUrl(data);
            if (recordsFetched.Count == 1)
            {
                UpdateClickCount(shortUrl);
                return recordsFetched.First().originalUrl;
            }
            return null;
        }

        /// <summary>
        /// Updates the click count column for a specified shortUrl
        /// </summary>
        /// <param name="shortUrl"></param>
        public void UpdateClickCount(string shortUrl)
        {
            ShortUrlModel data = new ShortUrlModel
            {
                shortUrl = shortUrl
            };
            _uRlShortnerDbRepository.IncrementClicksFromShortUrl(data);
        }

        /// <summary>
        /// Creates a record for a new shortUrl entry
        /// </summary>
        /// <param name="originalUrl">The originalUrl to store</param>
        /// <param name="shortUrl">The shortUrl to store</param>
        /// <returns></returns>
        public Tuple<bool, string> CreateShortUrl(string originalUrl, string shortUrl)
        {
            originalUrl = ValidateSchemaOnURL(originalUrl);

            if (CheckIfCustomUrlExists(shortUrl))
            {
                return new Tuple<bool, string>(false, shortUrl);
            }

            if (shortUrl == null || shortUrl.Length == 0)
            {
                shortUrl = GenerateShortUrl();
            }

            ShortUrlModel newData = new ShortUrlModel
            {
                originalUrl = originalUrl,
                shortUrl = shortUrl,
                clicks = 0,
                created = DateTime.Now,
            };
            var rowsAffected = _uRlShortnerDbRepository.InsertNewRecord(newData);
            if(rowsAffected == 0)
            {
                return new Tuple<bool, string>(false, shortUrl);
            }
            return new Tuple<bool, string>(true, shortUrl);
        }

        /// <summary>
        /// Validates and returns a url strictly with a schema
        /// </summary>
        /// <param name="url">Url to verify</param>
        /// <returns></returns>
        public string ValidateSchemaOnURL(string url)
        {
            var uri = new UriBuilder(url);
            return uri.Uri.AbsoluteUri;
        }

        /// <summary>
        /// Checks database if shortUrl already exists
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <returns></returns>
        public bool CheckIfCustomUrlExists(string shortUrl)
        {
            ShortUrlModel data = new ShortUrlModel
            {
                shortUrl = shortUrl
            };
            var recordsFetched = _uRlShortnerDbRepository.LoadOriginalUrlFromShortUrl(data);
            if (recordsFetched.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Generates a shortUrl from random characters
        /// </summary>
        /// <returns></returns>
        private string GenerateShortUrl()
        {
            Random random = new Random();
            var lengthOfShortUrl = random.Next(MinLengthOfShortLink, MaxLengthOfShortLink);
            string generatedShortUrl = RandomString(lengthOfShortUrl, random);

            ShortUrlModel data = new ShortUrlModel
            {
                shortUrl = generatedShortUrl
            };

            while (_uRlShortnerDbRepository.LoadOriginalUrlFromShortUrl(data).Count > 0)
            {
                generatedShortUrl = RandomString(lengthOfShortUrl, random);
            }
            return generatedShortUrl;
        }


        private string RandomString(int length, Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


    }
}
