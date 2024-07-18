using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReachingFam.Core.Data;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Services
{
    public class ApprovalService(ApplicationDbContext context, ILogger<ApprovalService> logger) : IApprovalService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<ApprovalService> _logger = logger;

        public async Task<(T, T)> GetItemForApproval<T>(int id) where T : class
        {
            try
            {
                ApprovalQueue approval = await _context.Approvals.Where(x => x.ApprovalQueueId == id).FirstOrDefaultAsync();
                var oldValue = approval.OldValue as T;
                var newValue = approval.NewValue as T;

                return (oldValue, newValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in getting data for: {typeof(T).Name}");
            }

            return (null, null);
        }

        public async Task<bool> UpdateApprovalQueue(string className, string moduleName, UpdateAction action, string oldValue, string newValue, string userName)
        {
            ApprovalQueue approvalQueue = new()
            {
                TableName = className,
                ModuleName = moduleName,
                Action = action,
                OldValue = oldValue,
                NewValue = newValue,
                ChangedBy = userName
            };

            ApprovalNotification notification = new() { Message = $"{moduleName} has been updated by {userName} on {DateTime.Now}. You might need to approve." };

            try
            {
                await _context.AddAsync(approvalQueue);
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"An error has occurred when trying to write audit trail for Table: {className} | Old Value: {oldValue} | New Value: {newValue}", ex);
            }

            return false;
        }

        public async Task<bool> UpdateTable(string obj, string objName)
        {
            try
            {
                switch (objName)
                {
                    case "Donor":
                        Donor donor = JsonConvert.DeserializeObject<Donor>(obj);
                        _context.Update(donor);
                        break;
                    case "Family":
                        Family family = JsonConvert.DeserializeObject<Family>(obj);
                        _context.Update(family);
                        break;
                    case "Hamper":
                        Hamper hamper = JsonConvert.DeserializeObject<Hamper>(obj);
                        _context.Update(hamper);
                        break;
                    case "InwardItem":
                        InwardItem inward = JsonConvert.DeserializeObject<InwardItem>(obj);
                        _context.Update(inward);
                        break;
                    case "Partner":
                        Partner partner = JsonConvert.DeserializeObject<Partner>(obj);
                        _context.Update(partner);
                        break;
                    case "PartnerGiveOut":
                        PartnerGiveOut partnerGive = JsonConvert.DeserializeObject<PartnerGiveOut>(obj);
                        _context.Update(partnerGive);
                        break;
                    case "VolunteerGiveOut":
                        VolunteerGiveOut volunteer = JsonConvert.DeserializeObject<VolunteerGiveOut>(obj);
                        _context.Update(volunteer);
                        break;
                    case "Waste":
                        Waste waste = JsonConvert.DeserializeObject<Waste>(obj);
                        _context.Update(waste);
                        break;
                    case "UnitOfMeasure":
                        UnitOfMeasure unitOfMeasure = JsonConvert.DeserializeObject<UnitOfMeasure>(obj);
                        _context.Update(unitOfMeasure);
                        break;
                    case "FoodItem":
                        FoodItem foodItem = JsonConvert.DeserializeObject<FoodItem>(obj);
                        _context.Update(foodItem);
                        break;
                    case "FoodItemOption":
                        FoodItemOption foodItemOption = JsonConvert.DeserializeObject<FoodItemOption>(obj);
                        _context.Update(foodItemOption);
                        break;
                    case "ItemCategory":
                        ItemCategory itemCategory = JsonConvert.DeserializeObject<ItemCategory>(obj);
                        _context.Update(itemCategory);
                        break;
                    default:
                        break;
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch { return false; }
        }
    }
}
