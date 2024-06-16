namespace PTUDTMDT.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("customer")]
    public partial class customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public customer()
        {
            carts = new HashSet<cart>();
            order_ = new HashSet<order_>();
            payments = new HashSet<payment>();
            shipments = new HashSet<shipment>();
            wishlists = new HashSet<wishlist>();
        }

        [Key]
        public int customer_id { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập trường này")]
        [StringLength(100)]
        public string first_name { get; set; }
        [StringLength(100)]
        public string last_name { get; set; }
        [StringLength(100)]
        public string email { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập trường này")]
        [StringLength(100)]
        public string password { get; set; }

        [StringLength(100)]
        public string address { get; set; }

        [StringLength(100)]
        public string phone_number { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cart> carts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_> order_ { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<payment> payments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<shipment> shipments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<wishlist> wishlists { get; set; }
    }
}
