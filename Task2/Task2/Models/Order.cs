using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Task2.Models
{
    [Table("tblOrders")]
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductCount { get; set; }
        public int UserId { get; set; }
        public DateTime OrderTime { get; set; }
    }
}