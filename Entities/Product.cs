namespace Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("product")]
    public partial class Product
    {
        public Product()
        {
            Consultation_product = new HashSet<Consultation_product>();
            Favorite = new HashSet<Favorite>();
            Line_cart = new HashSet<Line_cart>();
            Sale_product = new HashSet<Sale_product>();
        }

        public int Id { get; set; }

        public int Catalog_number { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(999)]
        public string Description { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }

        public bool Shine { get; set; }

        public int? Collection_id { get; set; }

        public bool Active { get; set; }

        [StringLength(400)]
        public string Url_image { get; set; }

        [StringLength(100)]
        public string Ref_image { get; set; }

        public virtual Collection Collection { get; set; }

        public virtual ICollection<Consultation_product> Consultation_product { get; set; }

        public virtual ICollection<Favorite> Favorite { get; set; }

        public virtual ICollection<Line_cart> Line_cart { get; set; }

        public virtual ICollection<Sale_product> Sale_product { get; set; }
    }
}
