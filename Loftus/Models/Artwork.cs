using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loftus.Models
{
    public class Artwork
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Filename { get; set; }
        public string Year { get; set; }
        public string Deminsions { get; set; }
        public string Medium { get; set; }
        public string Category { get; set; }
        public string AddInfo { get; set; }
    }
}