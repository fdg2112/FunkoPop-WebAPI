namespace Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Sale_product
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public int Product_id { get; set; }

        public int Sale_id { get; set; }

        public bool Active { get; set; }

        public double Unitary_total { get; set; }

        public virtual Product Product { get; set; }

        public virtual Sale Sale { get; set; }
    }
}
