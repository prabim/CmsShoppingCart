﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.Data
{
    [Table("tblPages")]
    public class PageDTO
    {
        public  int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public int Sorting { get; set; }
        public bool HasSidebar { get; set; }
    }
}