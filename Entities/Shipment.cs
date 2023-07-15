namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("shipment")]
    public partial class Shipment
    {
        public Shipment()
        {
            Sale = new HashSet<Sale>();
        }

        public int Id { get; set; }

        public DateTime Shipment_date { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<Sale> Sale { get; set; }
    }
}
