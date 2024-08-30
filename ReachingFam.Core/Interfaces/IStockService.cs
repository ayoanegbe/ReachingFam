using ReachingFam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Interfaces
{
    public interface IStockService
    {
        Task<List<FoodItem>> GetFooditemsBelowReorderLevel();
        Task<decimal> FoodItemReorderLevel(int id);
        Task<bool> IsFoodItemInStock(int id);
        Task<bool> SetFoodItemStatus(int id);
        Task<List<StockTransaction>> FoodItemStockHistory(FoodItem foodItem, DateTime startDate, DateTime endDate);
        Task<decimal> FoodItemStockLevel(FoodItem foodItem, DateTime startDate, DateTime endDate);
        Task<string> AddToStock(Stock stock);
        Task<string> AddNewStock(Stock stock);
        Task<string> IssueStock(Stock stock);
        Task<List<StockTransaction>> GetStockHistory(int foodItemId);
        Task<decimal> PriorBalance(FoodItem foodItem, DateTime startDate);
        Task<decimal> StockBalance(FoodItem foodItem);
        Task<decimal> StockBalance(FoodItem foodItem, DateTime startDate, DateTime endDate);
        Task<string> AddFoodItemSubstitute(FoodItemSubstitute substitute);
    }
}
