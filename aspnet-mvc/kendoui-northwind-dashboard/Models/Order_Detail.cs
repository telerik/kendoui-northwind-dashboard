using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KendoUI.Northwind.Dashboard.Models
{
    
    public partial class OrderDetail
    {
        [Key, Column(Order = 0)]
        public int OrderID { get; set; }
        [Key, Column(Order = 1)]
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
