namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("sale")]
    public partial class Sale
    {
        public Sale()
        {
            Sale_product = new HashSet<Sale_product>();
        }

        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Operation_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Payment_date { get; set; }

        [Required]
        [StringLength(10)]
        public string Sale_type { get; set; }

        public int? Address_id { get; set; }

        public int? Shipment_id { get; set; }

        public int User_id { get; set; }

        public bool Active { get; set; }

        public int Payment_id { get; set; }

        public double Payment_total { get; set; }

        [Required]
        [StringLength(10)]
        public string Sale_status { get; set; }

        public virtual Address Address { get; set; }

        public virtual Shipment Shipment { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Sale_product> Sale_product { get; set; }
    }
}
