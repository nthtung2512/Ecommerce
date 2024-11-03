﻿using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class BillPromotionRepository : IBillPromotionRepository
    {
        private readonly MealMateDbContext _context;

        public BillPromotionRepository(MealMateDbContext context)
        {
            _context = context;
        }

        #region dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    _context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        public async Task CreateAsync(BillPromotion newPromotion)
        {
            await _context.BillPromotions.AddAsync(newPromotion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(BillPromotion billPromotion)
        {
            billPromotion.IsDeleted = true;
            _context.Entry(billPromotion).State = EntityState.Modified;
            foreach (var promoteBill in billPromotion.PromoteBills)
            {
                promoteBill.IsDeleted = true;
                _context.Entry(promoteBill).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<BillPromotion>> GetAllBillPromotionsAsync()
        {
            return await _context.BillPromotions.Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<BillPromotion?> GetBillPromotionByIdAsync(Guid id)
        {
            return await _context.BillPromotions.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<List<BillPromotion>> GetPromotionByBillId(Guid billId)
        {
            var promotions = await _context.PromoteBills
                .Include(p => p.BillPromotion)
                .Where(p => p.TransactionId == billId && p.BillPromotion.ApplyPrice >= p.Bill.TotalPrice && !p.IsDeleted)
                .Select(p => p.BillPromotion)
                .ToListAsync();
            return promotions;
        }
    }
}