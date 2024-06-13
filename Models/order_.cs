namespace PTUDTMDT.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class order_
    {


        public int order_id { get; set; }

        public DateTime? order_date { get; set; }

        public decimal? order_price { get; set; }

        public int? payment_id { get; set; }

        public int? customer_id { get; set; }

        public int? shipment_id { get; set; }

        public virtual customer customer { get; set; }

        public virtual payment payment { get; set; }

        public virtual shipment shipment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_item> order_item { get; set; }

      
    }
}
