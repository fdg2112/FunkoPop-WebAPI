using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("cart")]
    public partial class Cart
    {
        public Cart()
        {
            Line_cart = new HashSet<Line_cart>();
        }

        public int Id { get; set; }

        public int User_id { get; set; }

        public bool Active { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Line_cart> Line_cart { get; set; }
    }
}
