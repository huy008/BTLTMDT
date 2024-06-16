namespace PTUDTMDT.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("product")]
    public partial class product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public product()
        {
            carts = new HashSet<cart>();
            order_item = new HashSet<order_item>();
            wishlists = new HashSet<wishlist>();
        }

        [Key]
        public int product_id { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập trường này")]
        [StringLength(100)]
        public string SKU { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập trường này")]
        [StringLength(255)]
        public string description { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập trường này")]
        public decimal price { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập trường này")]
        public int stock { get; set; }

        public int? category_id { get; set; }

        [StringLength(50)]
        public string product_image { get; set; }

        public DateTime? created_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cart> carts { get; set; }

        public virtual category category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_item> order_item { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<wishlist> wishlists { get; set; }
    }
}
