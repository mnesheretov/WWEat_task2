﻿using System.Collections.Generic;

namespace WWEat.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }
    }
}
