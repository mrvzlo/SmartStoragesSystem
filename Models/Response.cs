using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace SmartKitchen.Models
{
    public class Response
    {
        public bool Successfull { get; set; }
        public int Error { get; set; }

        public Response() { }

        public Response(int error)
        {
            Successfull = false;
            Error = error;
        }

        public static Response Success()
        {
            return new Response
            {
                Successfull = true,
                Error = 0
            };
        }
    }
}