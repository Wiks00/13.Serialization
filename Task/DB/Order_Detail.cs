using System.Data.Entity.Core.Objects;
using System.Runtime.Serialization;

namespace Task.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order Details")]
    [Serializable]
    public partial class Order_Detail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }

    public class Order_DetailSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var order_Detail = (Order_Detail)obj;

            info.AddValue("OrderID", order_Detail.OrderID);
            info.AddValue("ProductID", order_Detail.ProductID);
            info.AddValue("UnitPrice", order_Detail.UnitPrice);
            info.AddValue("Quantity", order_Detail.Quantity);
            info.AddValue("Discount", order_Detail.Discount);


            var objContext = (ObjectContext)context.Context;
            objContext.LoadProperty(order_Detail, f => f.Order);
            objContext.LoadProperty(order_Detail, f => f.Product);

            info.AddValue("Order", order_Detail.Order);
            info.AddValue("Product", order_Detail.Product);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var order_Detail = (Order_Detail)obj;

            order_Detail.OrderID = info.GetInt32("OrderID");
            order_Detail.ProductID = info.GetInt32("ProductID");
            order_Detail.UnitPrice = info.GetDecimal("UnitPrice");
            order_Detail.Quantity = info.GetInt16("Quantity");
            order_Detail.Discount = info.GetSingle("Discount");

            order_Detail.Order = info.GetValue("Order", typeof(Order)) as Order;
            order_Detail.Product = info.GetValue("Product", typeof(Product)) as Product;

            return order_Detail;
        }
    }
}
