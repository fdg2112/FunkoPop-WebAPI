namespace Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("favorite")]
    public partial class Favorite
    {
        public int Id { get; set; }

        public int Product_id { get; set; }

        public int User_id { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
