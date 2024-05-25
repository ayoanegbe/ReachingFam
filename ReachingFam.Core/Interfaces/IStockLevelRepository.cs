using ReachingFam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Interfaces
{
    public interface IStockLevelRepository
    {
        Task<bool> UpdateStocHistory(Stock stock);
        Task<bool> UpdateStocHistory(HamperItem hamperItem);
        Task<bool> UpdateStocHistory(PartnerHamperItem hamperItem);
        Task<bool> UpdateStocHistory(VolunteerHamperItem hamperItem);
        Task<List<StockHistory>> FoodItemStockHistory(FoodItem foodItem);
        Task<List<StockHistory>> FoodItemStockHistory(FoodItem foodItem, DateTime startDate, DateTime endDate);
        Task<decimal> FoodItemStockLevel(FoodItem foodItem);
        Task<decimal> FoodItemStockLevel(FoodItem foodItem, DateTime startDate, DateTime endDate);
        Task<decimal> StockReorderLevel(int foodItemId);
    }
}
