using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheCoffeeHouse.Models.EtityFarmwork
{
    [Table("tb_Sale")]
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleID { get; set; }
        [Required(ErrorMessage = "Tên Sale không được để trống")]
        [StringLength(350)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Tiêu đề seo không được để trống")]
        [StringLength(350)]
        public string SeoTitle { get; set; }
        [Required(ErrorMessage = "Ảnh không được để trống")]
        [StringLength(250)]
        public string Image { get; set; }
        [Required(ErrorMessage = "Mô tả không được để trống")]
        [StringLength(500)]
        public string Description { get; set; }
    }
}