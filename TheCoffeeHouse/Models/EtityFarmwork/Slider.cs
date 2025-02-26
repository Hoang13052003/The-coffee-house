using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheCoffeeHouse.Models.EtityFarmwork
{
    [Table("tb_Slider")]
    public class Slider
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required(ErrorMessage = "Tên slider không được để trống")]
        [StringLength(350)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ảnh không được để trống")]
        [StringLength(250)]
        public string Image { get; set; }
        [Required(ErrorMessage = "Link không được để trống")]
        [StringLength(250)]
        public string Link { get; set; }
    }
}