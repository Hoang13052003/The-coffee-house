using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCoffeeHouse.Models.EtityFarmwork;

namespace TheCoffeeHouse.Models.ViewModel
{
    public class PostCategoryViewModel
    {
        public List<SelectListItem> postCategory { get; set; }
    }
    public class PostViewModel
    {
        public List<SelectListItem> post { get; set; }
    }
}