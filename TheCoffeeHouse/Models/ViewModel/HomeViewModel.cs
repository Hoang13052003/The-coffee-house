using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheCoffeeHouse.Models.EtityFarmwork;

namespace TheCoffeeHouse.Models.ViewModel
{
    public class HomeViewModel
    {
        public List<Slider> sliders { get; set; }
        //public List<Sale> sale { get; set; }    
        //public List<Product> product { get; set; }
    }
}