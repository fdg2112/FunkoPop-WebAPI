namespace Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("location")]
    public partial class Location
    {
        public Location()
        {
            Address = new HashSet<Address>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(8)]
        public string Postal_code { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int Province_id { get; set; }

        public virtual ICollection<Address> Address { get; set; }

        public virtual Province Province { get; set; }
    }
}
