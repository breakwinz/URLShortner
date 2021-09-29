using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Interfaces;

namespace DataLibrary.Logic
{
    public class ShortUrlProcessor : IShortUrlProcessor
    {
        private static int minLengthOfShortLink = 4;
        private static int maxLengthOfShortLink = 10;

        private readonly IURLShortnerDBRepository uRLShortnerDBRepository;

        public ShortUrlProcessor(IURLShortnerDBRepository uRLShortnerDBRepository)
        {
            this.uRLShortnerDBRepository = uRLShortnerDBRepository;
        }

        /// <summary>
        /// Attempts to fetch originalUrl filtered by shortUrl and increments clicks if found
        /// </summary>
        /// <param name="shortUrl">shortUrl to filter by</param>
        /// <returns></returns>
        public string GetOriginalURL(string shortUrl)
        {
            var lengthOfShortUrl = shortUrl.Length;

            if (minLengthOfShortLink <= lengthOfShortUrl && lengthOfShortUrl >= maxLengthOfShortLink)
            {
                return null;
            }
            ShortURLModel data = new ShortURLModel
            {
                shortUrl = shortUrl
            };
            var recordsFetched = uRLShortnerDBRepository.LoadOriginalURLFromShortURL(data);
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
            ShortURLModel data = new ShortURLModel
            {
                shortUrl = shortUrl
            };
            uRLShortnerDBRepository.IncrementClicksFromShortUrl(data);
        }

        /// <summary>
        /// Creates a record for a new shortUrl entry
        /// </summary>
        /// <param name="originalUrl">The originalUrl to store</param>
        /// <param name="shortUrl">The shortUrl to store</param>
        /// <returns></returns>
        public Tuple<bool, string> CreateShortURL(string originalUrl, string shortUrl)
        {
            originalUrl = ValidateSchemaOnURL(originalUrl);

            if (CheckIfCustomURLExists(shortUrl))
            {
                return new Tuple<bool, string>(false, shortUrl);
            }

            if (shortUrl == null || shortUrl.Length == 0)
            {
                shortUrl = GenerateShortUrl();
            }

            ShortURLModel newData = new ShortURLModel
            {
                originalUrl = originalUrl,
                shortUrl = shortUrl,
                clicks = 0,
                created = DateTime.Now,
            };
            var rowsAffected = uRLShortnerDBRepository.InsertNewRecord(newData);
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
        public bool CheckIfCustomURLExists(string shortUrl)
        {
            ShortURLModel data = new ShortURLModel
            {
                shortUrl = shortUrl
            };
            var recordsFetched = uRLShortnerDBRepository.LoadOriginalURLFromShortURL(data);
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
            var lengthOfShortUrl = random.Next(minLengthOfShortLink, maxLengthOfShortLink);
            string generatedShortUrl = RandomString(lengthOfShortUrl, random);

            ShortURLModel data = new ShortURLModel
            {
                shortUrl = generatedShortUrl
            };

            while (uRLShortnerDBRepository.LoadOriginalURLFromShortURL(data).Count > 0)
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
