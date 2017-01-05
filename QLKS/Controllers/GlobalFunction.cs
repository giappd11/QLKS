using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using QLKS.Models;

namespace QLKS.Controllers
{
    public class GlobalFunction
    {
        private Entities db = new Entities();
        public static string addDateToID(string id)
        {
            string date = DateTime.Now.ToShortDateString();
            date = date.Replace("-", "");
            date = date.Replace(" ", "");
            date = date.Replace("/", "");
            date = date.Replace(":", "");
            string idAddZero = "0000000".Substring(0, 7 - id.Length) + id;
            return date + idAddZero;
        }

    }
}