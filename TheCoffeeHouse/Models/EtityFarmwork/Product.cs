using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace TheCoffeeHouse.Models.EtityFarmwork
{
    [Table("tb_Product")]
    public class Product : commonAbstract
    {
        public Product()
        {
            this.OrderDetail = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(150)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ảnh không được để trống")]
        [StringLength(150)]
        public string Image { get; set; }
        [Required(ErrorMessage = "Giá không được để trống")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Danh mục không được để trống")]
        public int CateID { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}