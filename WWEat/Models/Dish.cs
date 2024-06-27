using System.Collections.Generic;

namespace WWEat.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }
    }
}
