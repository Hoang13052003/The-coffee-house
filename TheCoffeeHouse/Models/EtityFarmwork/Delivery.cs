using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheCoffeeHouse.Models.EtityFarmwork
{
    [Table("tb_Delivery")]
    public class Delivery
    {
        public Delivery()
        {
            this.Order = new HashSet<Order>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên không được để trống!")]
        public string Name { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}