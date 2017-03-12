﻿using System.Collections.Generic;

namespace Fancy.Data.Models.Models
{
    public class MainMaterial
    {
        private ICollection<Item> items;

        public MainMaterial()
        {
            this.items = new HashSet<Item>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Item> Items
        {
            get
            {
                return this.items;
            }

            set
            {
                this.items = value;
            }
        }
    }
}