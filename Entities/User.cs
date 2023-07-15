namespace Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("user")]
    public partial class User
    {
        public User()
        {
            Address = new HashSet<Address>();
            Cart = new HashSet<Cart>();
            Consultation_product = new HashSet<Consultation_product>();
            Favorite = new HashSet<Favorite>();
            Sale = new HashSet<Sale>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Uid { get; set; }

        [Required]
        [StringLength(255)]
        public string Lastname { get; set; }

        [Required]
        [StringLength(255)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(45)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(10)]
        public string Role { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<Address> Address { get; set; }

        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<Consultation_product> Consultation_product { get; set; }

        public virtual ICollection<Favorite> Favorite { get; set; }

        public virtual ICollection<Sale> Sale { get; set; }
    }
}
