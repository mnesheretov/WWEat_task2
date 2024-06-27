using System;
namespace WWEat.Models
{
    public class Meal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DishId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual User User { get; set; }
        public virtual Dish Dish { get; set; }
    }
}
