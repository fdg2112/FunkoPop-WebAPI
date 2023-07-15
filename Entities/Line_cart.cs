namespace Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Line_cart
    {
        public int Id { get; set; }

        public int? Quantity { get; set; }

        public double? Total { get; set; }

        public int Cart_id { get; set; }

        public int Product_id { get; set; }

        public bool Active { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual Product Product { get; set; }
    }
}
