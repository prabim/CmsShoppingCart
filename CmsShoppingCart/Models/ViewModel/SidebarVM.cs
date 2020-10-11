using CmsShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Models.ViewModel
{
    public class SidebarVM
    {
        public SidebarVM()
        {

        }

        public SidebarVM(SidebarDTO dto)
        {
            Id = dto.Id;
            Body = dto.Body;
        }

        public int Id { get; set; }
        [AllowHtml]
        public string Body { get; set; }
    }
}