using System;
// ReSharper disable InconsistentNaming

namespace DataLibrary.Models
{
    public class ShortUrlModel
    {
        protected int id { get; set; }
        public string originalUrl { get; set; }
        public string shortUrl { get; set; }
        public int clicks { get; set; }
        public DateTime created { get; set; }

    }
}
