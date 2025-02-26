using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheCoffeeHouse.Models.EtityFarmwork
{
    [Table("tb_PostCategory")]
    public class PostCategory : commonAbstract
    {
        public PostCategory()
        {
            this.Post = new HashSet<Post>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CateID { get; set; }
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(150)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Đường dẩn không được để trống")]
        [StringLength(150)]
        public string Link { get; set; }
        [StringLength(150)]
        public string SeoTitle { get; set; }
        [Required(ErrorMessage = "Cấp danh mục không được để trống")]
        public int Parent { get; set; }
        public virtual ICollection<Post> Post { get; set; }
    }
}