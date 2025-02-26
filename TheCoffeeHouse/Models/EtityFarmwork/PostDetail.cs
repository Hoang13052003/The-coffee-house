using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheCoffeeHouse.Models.EtityFarmwork
{
    [Table("tb_PostDetail")]
    public class PostDetail : commonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostDetailID { get; set; }
        public int PostID { get; set; } 
        public string Name { get; set; }
        public string SeoTitle { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public virtual Post Post { get; set; }
    }
}