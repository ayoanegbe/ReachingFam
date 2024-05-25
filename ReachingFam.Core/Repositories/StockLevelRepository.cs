using DnsClient.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReachingFam.Core.Data;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Repositories
{
    public class StockLevelRepository(ApplicationDbContext context, ILogger<StockLevelRepository> logger) : IStockLevelRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<StockLevelRepository> _logger = logger;

        public async Task<bool> UpdateStocHistory(Stock stock)
        {
            try
            {
                decimal priorBalance = await _context.StockHistories.SumAsync(x => x.Quantity);

                StockHistory stockHistory = new()
                {
                    FoodItemId = stock.FoodItemId,
                    Quantity = stock.Quantity,
                    AdjustedQuantity = priorBalance + stock.Quantity,
                    TransactionType = TransactionType.Add,
                    Date = DateTime.Now,
                };

                await _context.AddAsync(stockHistory);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occurred in {new StackTrace().GetFrame(0).GetMethod()}: {ex}");
            }

            return false;
        }

        public async Task<bool> UpdateStocHistory(HamperItem hamperItem)
        {
            try
            {

                decimal priorBalance = await _context.StockHistories.SumAsync(x => x.Quantity);

                StockHistory stockHistory = new()
                {
                    FoodItemId = hamperItem.FoodItemId,
                    Quantity = hamperItem.Quantity * -1,
                    AdjustedQuantity = priorBalance - hamperItem.Quantity,
                    TransactionType = TransactionType.Remove,
                    Date = DateTime.Now,
                };

                await _context.AddAsync(stockHistory);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occurred in {new StackTrace().GetFrame(0).GetMethod()}: {ex}");
            }

            return false;
        }

        public async Task<bool> UpdateStocHistory(PartnerHamperItem hamperItem)
        {
            try
            {

                decimal priorBalance = await _context.StockHistories.SumAsync(x => x.Quantity);

                StockHistory stockHistory = new()
                {
                    FoodItemId = hamperItem.FoodItemId,
                    Quantity = hamperItem.Quantity * -1,
                    AdjustedQuantity = priorBalance - hamperItem.Quantity,
                    TransactionType = TransactionType.Remove,
                    Date = DateTime.Now,
                };

                await _context.AddAsync(stockHistory);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occurred in {new StackTrace().GetFrame(0).GetMethod()}: {ex}");
            }

            return false;
        }

        public async Task<bool> UpdateStocHistory(VolunteerHamperItem hamperItem) 
        {
            try
            {
                decimal priorBalance = await _context.StockHistories.SumAsync(x => x.Quantity);

                StockHistory stockHistory = new()
                {
                    FoodItemId = hamperItem.FoodItemId,
                    Quantity = hamperItem.Quantity * -1,
                    AdjustedQuantity = priorBalance - hamperItem.Quantity,
                    TransactionType = TransactionType.Remove,
                    Date = DateTime.Now,
                };

                await _context.AddAsync(stockHistory);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occurred in {new StackTrace().GetFrame(0).GetMethod()}: {ex}");
            }

            return false;
        }

        public async Task<List<StockHistory>> FoodItemStockHistory(FoodItem foodItem)
        {
            return await _context.StockHistories.Where(x => x.FoodItemId == foodItem.FoodItemId).ToListAsync();
        }

        public async Task<List<StockHistory>> FoodItemStockHistory(FoodItem foodItem, DateTime startDate, DateTime endDate)
        {
            return await _context.StockHistories.Where(x => x.FoodItemId == foodItem.FoodItemId && (x.Date >= startDate || x.Date <= endDate)).ToListAsync();
        }

        public async Task<decimal> FoodItemStockLevel(FoodItem foodItem)
        {
            return await _context.StockHistories.Where(x => x.FoodItemId == foodItem.FoodItemId).SumAsync(x => x.Quantity);
        }

        public async Task<decimal> FoodItemStockLevel(FoodItem foodItem, DateTime startDate, DateTime endDate)
        {
            decimal priorBalance = await _context.StockHistories.Where(x => x.FoodItemId == foodItem.FoodItemId && x.Date < startDate).SumAsync(x => x.Quantity);
            decimal periodBalance = await _context.StockHistories.Where(x => x.FoodItemId == foodItem.FoodItemId && (x.Date >= startDate || x.Date <= endDate)).SumAsync(x => x.Quantity);

            return priorBalance + periodBalance;
        }

        public async Task<decimal> StockReorderLevel(int foodItemId)
        {
            return await _context.Stocks.Where(x => x.FoodItemId == foodItemId).SumAsync(x => x.Quantity);
        }
    }
}
