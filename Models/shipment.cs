namespace PTUDTMDT.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("shipment")]
    public partial class shipment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public shipment()
        {
            order_ = new HashSet<order_>();
        }

        [Key]
        public int shipment_id { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập trường này")]
        public DateTime? shipment_date { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập trường này")]
        [StringLength(100)]
        public string address { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập trường này")]
        [StringLength(100)]
        public string city { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập trường này")]
        [StringLength(100)]
        public string status { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập trường này")]
        [StringLength(100)]
        public string country { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập trường này")]
        [StringLength(100)]
        public string zip_code { get; set; }

        public int? customer_id { get; set; }

        public virtual customer customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_> order_ { get; set; }
    }
}
