﻿using Microsoft.EntityFrameworkCore;

namespace MealMate.BLL.Dtos.Promotion
{
    public class PromotionCreationDto
    {
        public Guid PromotionID { get; init; }
        [Precision(3, 2)]
        public required decimal Discount { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required DateTime StartDay { get; init; }
        public required DateTime EndDay { get; init; }
    }
}