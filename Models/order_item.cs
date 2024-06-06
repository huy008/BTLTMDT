namespace PTUDTMDT.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class order_item
    {
        [Key]
        public int order_item_id { get; set; }

        public int? quantity { get; set; }

        public decimal price { get; set; }

        public int? product_id { get; set; }

        public int? order_id { get; set; }

        public virtual order_ order_ { get; set; }

        public virtual product product { get; set; }

        public order_item()
        {
        }

        public order_item(int? quantity, decimal price, int? product_id,int? order_id)
        {
            this.quantity=quantity;
            this.price=price;
            this.product_id=product_id;
            this.order_id = order_id;
        }
    }
}
