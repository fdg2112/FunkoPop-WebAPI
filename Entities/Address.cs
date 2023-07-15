using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("address")]
    public partial class Address
    {
        public Address()
        {
            Sale = new HashSet<Sale>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Street_name { get; set; }

        public int Street_number { get; set; }

        public int? Floor { get; set; }

        [StringLength(5)]
        public string Department { get; set; }

        public int Location_id { get; set; }

        public int User_id { get; set; }

        public bool Active { get; set; }

        public virtual Location Location { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Sale> Sale { get; set; }
    }
}
