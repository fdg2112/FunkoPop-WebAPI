namespace Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Consultation_product
    {
        private string message1;

        public Consultation_product(int id, string message)
        {
            this.Id = id;
            this.Message = message;
        }

        public Consultation_product(string message)
        {
            this.Message = message;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(999)]
        public string Message { get => message1; set => message1 = value; }

        [StringLength(999)]
        public string Response { get; set; }

        public int Product_id { get; set; }

        public int User_id { get; set; }

        public bool Active { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
