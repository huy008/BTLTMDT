namespace PTUDTMDT.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cart")]
    public partial class cart
    {
        [Key]
        public int cart_id { get; set; }

        public int? quantity { get; set; }

        public int? customer_id { get; set; }

        public int? product_id { get; set; }

        public virtual customer customer { get; set; }

        public virtual product product { get; set; }
    }
}
