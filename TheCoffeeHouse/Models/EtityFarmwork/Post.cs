using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheCoffeeHouse.Models.EtityFarmwork
{
    [Table("tb_Post")]
    public class Post : commonAbstract
    {
        public Post()
        {
            this.PostDetail = new HashSet<PostDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostID { get; set; }
        [Required(ErrorMessage = "Tên bài viết không được để trống")]
        [StringLength(350)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Tiêu đề seo không được để trống")]
        public string SeoTitle { get; set; }
        [Required(ErrorMessage = "Ảnh không được để trống")]
        [StringLength(250)]
        public string Image { get; set; }
        public string Description { get; set; }
        public int CateID { get; set; }
        public virtual PostCategory PostCategory { get; set; }
        public virtual ICollection<PostDetail> PostDetail { get; set; }
    }
}