﻿using MealMate.DAL.Entities.Products;
using MealMate.DAL.Utils;
using MealMate.DAL.Utils.EFCore;

namespace MealMate.DAL.Entities.Stores
{
    public class Store(Guid id) : Entity<Guid>(id), IDeletableEntity
    {
        public required string Name { get; set; }
        public DateTime OpeningDate { get; set; }
        public required string ContactInfo { get; set; }
        public required string Location { get; set; }
        public ICollection<AT> ATs { get; } = [];
        public ICollection<Bill> Bills { get; } = [];
        public bool IsDeleted { get; set; }
    }
}
