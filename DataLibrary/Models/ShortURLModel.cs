using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class ShortURLModel
    {
        protected int id { get; set; }
        public string originalUrl { get; set; }
        public string shortUrl { get; set; }
        public int clicks { get; set; }
        public DateTime created { get; set; }

    }
}
