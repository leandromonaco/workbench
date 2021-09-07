using System;
using System.Collections.Generic;

namespace TeamAlignment.Core.Model.Database
{
    public partial class Setting
    {
        public Setting()
        {
            Products = new HashSet<Product>();
        }

        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
