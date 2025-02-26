using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TheCoffeeHouse.Models.EtityFarmwork
{
    [Table("tb_Order")]
    public class Order : commonAbstract
    {
        public Order()
        {
            this.OrderDetail = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusID { get; set; }
        public int DeliveredID { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public decimal TotalMoney { get; set; }
        public virtual Delivery Delivery { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
        public virtual Status Status { get; set; }
    }
}