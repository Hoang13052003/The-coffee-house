using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCoffeeHouse.Models.EtityFarmwork;

namespace TheCoffeeHouse.Models.ViewModel
{
    public class ProductViewModel
    {
        public List<SelectListItem> Categories { get; set; }
    }
}