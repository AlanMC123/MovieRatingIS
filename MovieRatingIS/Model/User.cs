using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieRatingIS.Model
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Tele { get; set; }
    }
}