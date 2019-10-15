using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial.Models
{
    public class Modelreg
    {
        public int Id { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public string idTraza { get; set; }

        public bool busy { get; set; }

        public int attender { get; set; }
    }
}