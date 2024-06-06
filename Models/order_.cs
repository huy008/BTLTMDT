namespace PTUDTMDT.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class order_
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public order_()
        {
            order_item = new HashSet<order_item>();
        }

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

        public order_(DateTime? order_date, decimal? order_price, int? customer_id)
        {
            this.order_date=order_date;
            this.order_price=order_price;
            this.customer_id=customer_id;
        }
    }
}
