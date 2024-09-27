using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReachingFam.Core.Data;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models;
using System.Diagnostics;

namespace ReachingFam.Core.Services
{
    public class StockService(ApplicationDbContext context, ILogger<StockService> logger) : IStockService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<StockService> _logger = logger;

        public async Task<List<FoodItem>> GetFooditemsBelowReorderLevel()
        {
            return await _context.FoodItems.Where(p => p.Stocks.Sum(s => s.Quantity) <= p.ReorderLevel).ToListAsync();
        }

        public async Task<decimal> FoodItemReorderLevel(int id)
        {
            FoodItem item = await _context.FoodItems.FindAsync(id);
            return (decimal)item.ReorderLevel;
        }

        public async Task<bool> IsFoodItemInStock(int id)
        {
            FoodItem item = await _context.FoodItems.FindAsync(id);
            return item.InStock;
        }

        public async Task<bool> SetFoodItemStatus(int id)
        {
            try
            {
                FoodItem item = await _context.FoodItems.FindAsync(id);
                if (item.Stocks.Sum(s => s.Quantity) == 0)
                {
                    item.InStock = false;
                }
                else { item.InStock = true; }

                _context.Update(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occurred in {new StackTrace().GetFrame(0).GetMethod()}: {ex}");
            }

            return false;
        }

        public async Task<List<StockTransaction>> FoodItemStockHistory(FoodItem foodItem, DateTime startDate, DateTime endDate)
        {
            return await _context.StockTransactions.Include(x => x.Stock).Where(x => x.Stock.FoodItemId == foodItem.FoodItemId && (x.TransactionDate >= startDate || x.TransactionDate <= endDate)).ToListAsync();
        }

        public async Task<decimal> FoodItemStockLevel(FoodItem foodItem, DateTime startDate, DateTime endDate)
        {
            decimal priorBalance = await _context.StockTransactions.Include(x => x.Stock).Where(x => x.Stock.FoodItemId == foodItem.FoodItemId && x.TransactionDate < startDate).SumAsync(x => x.Quantity);
            decimal periodBalance = await _context.StockTransactions.Include(x => x.Stock).Where(x => x.Stock.FoodItemId == foodItem.FoodItemId && (x.TransactionDate >= startDate || x.TransactionDate <= endDate)).SumAsync(x => x.Quantity);

            return priorBalance + periodBalance;
        }

        public async Task<decimal> FoodItemStockLevel(int foodItemId)
        {
            return await _context.Stocks.Where(x => x.FoodItemId == foodItemId).SumAsync(x => x.Quantity);
        }
        
        public async Task<string> AddToStock(Stock stock)
        {
            var existingStock = _context.Stocks.FirstOrDefault(s => s.FoodItemId == stock.FoodItemId && s.DonorId == stock.DonorId);

            if (existingStock != null)
            {
                return "Stock entry already exists for this food item and donor.";
            }

            existingStock.Quantity += stock.Quantity;
            existingStock.UpdatedBy = stock.UpdatedBy;
            existingStock.DateUpdated = DateTime.Now;

            _context.Update(existingStock);
            await _context.SaveChangesAsync();

            var stockTransaction = new StockTransaction
            {
                StockId = existingStock.StockId,
                TransactionType = TransactionType.Add,
                Quantity = stock.Quantity,
                TransactionDate = DateTime.Now,
                AddedBy = stock.UpdatedBy,
                DateAdded = DateTime.Now
            };

            await _context.StockTransactions.AddAsync(stockTransaction);
            await _context.SaveChangesAsync();

            return "Successful";
        }

        public async Task<string> AddNewStock(Stock stock)
        {
            try
            {
                var existingStock = _context.Stocks.FirstOrDefault(s => s.FoodItemId == stock.FoodItemId && s.DonorId == stock.DonorId);

                if (existingStock != null)
                {
                    return "Stock entry already exists for this food item and donor.";
                }

                await _context.Stocks.AddAsync(stock);
                await _context.SaveChangesAsync();

                var stockTransaction = new StockTransaction
                {
                    StockId = stock.StockId,
                    TransactionType = TransactionType.Add,
                    Quantity = stock.Quantity,
                    TransactionDate = DateTime.Now,
                    AddedBy = stock.AddedBy,
                    DateAdded = DateTime.Now
                };

                await _context.StockTransactions.AddAsync(stockTransaction);
                await _context.SaveChangesAsync();

                return "Successful";
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occurred in {new StackTrace().GetFrame(0).GetMethod()}: {ex}");
            }

            return "Failed";
        }
       
        public async Task<string> IssueStock(Stock stock)
        {
            try
            {
                var existingStock = await _context.Stocks.FindAsync(stock.StockId);
                if (existingStock != null && existingStock.Quantity >= stock.Quantity)
                {
                    existingStock.Quantity -= stock.Quantity;
                    existingStock.UpdatedBy = stock.UpdatedBy;
                    existingStock.DateUpdated = DateTime.Now;

                    _context.Update(existingStock);
                    await _context.SaveChangesAsync();

                    var stockTransaction = new StockTransaction
                    {
                        StockId = stock.StockId,
                        TransactionType = TransactionType.Issue,
                        Quantity = stock.Quantity * -1,
                        TransactionDate = DateTime.Now,
                        AddedBy = stock.UpdatedBy,
                        DateAdded = DateTime.Now
                    };

                    await _context.StockTransactions.AddAsync(stockTransaction);
                    await _context.SaveChangesAsync();
                }

                return "Successful";
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occurred in {new StackTrace().GetFrame(0).GetMethod()}: {ex}");
            }

            return "Failed";
        }

        public async Task<List<StockTransaction>> GetStockHistory(int foodItemId)
        {
            return await _context.StockTransactions
                .Include(st => st.Stock)
                .Where(st => st.Stock.FoodItemId == foodItemId)
                .OrderBy(st => st.TransactionDate)
                .ToListAsync();
        }

        public async Task<decimal> PriorBalance(FoodItem foodItem, DateTime startDate)
        {
            return await _context.StockTransactions.Include(x => x.Stock).Where(x => x.Stock.FoodItemId == foodItem.FoodItemId && x.TransactionDate < startDate).SumAsync(x => x.Quantity);
        }

        public async Task<decimal> StockBalance(FoodItem foodItem)
        {
            return await _context.StockTransactions.Include(x => x.Stock).Where(x => x.Stock.FoodItemId == foodItem.FoodItemId).SumAsync(x => x.Quantity);
        }

        public async Task<decimal> StockBalance(FoodItem foodItem, DateTime startDate, DateTime endDate)
        {
            return await _context.StockTransactions.Include(x => x.Stock).Where(x => x.Stock.FoodItemId == foodItem.FoodItemId && (x.TransactionDate >= startDate || x.TransactionDate <= endDate)).SumAsync(x => x.Quantity);
        }

        public async Task<string> AddFoodItemSubstitute(FoodItemSubstitute substitute)
        {
            try
            {
                var existingSubstitude = _context.FoodItemSubstitutes.FirstOrDefault(s => s.FoodItemId == substitute.FoodItemId && s.FoodItemSubstituteId == substitute.FoodItemSubstituteId);

                if (existingSubstitude != null)
                {
                    return "Substitude entry already exists for this food item and donor.";
                }

                await _context.FoodItemSubstitutes.AddAsync(substitute);
                await _context.SaveChangesAsync();                

                return "Successful";
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occurred in {new StackTrace().GetFrame(0).GetMethod()}: {ex}");
            }

            return "Failed";
        }
    }
}
