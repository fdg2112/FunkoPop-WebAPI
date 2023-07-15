namespace Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("province")]
    public partial class Province
    {
        public Province()
        {
            Location = new HashSet<Location>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        public virtual ICollection<Location> Location { get; set; }
    }
}
