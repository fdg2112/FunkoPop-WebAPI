namespace Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("collection")]
    public partial class Collection
    {
        public Collection()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(999)]
        public string Description { get; set; }

        public bool Active { get; set; }

        [StringLength(400)]
        public string Url_image { get; set; }

        [StringLength(100)]
        public string Ref_image { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
