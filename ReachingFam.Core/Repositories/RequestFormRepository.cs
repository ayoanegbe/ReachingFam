using Microsoft.Extensions.Logging;
using ReachingFam.Core.Data;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models;

namespace ReachingFam.Core.Repositories
{
    public class RequestFormRepository(ApplicationDbContext context, ILogger<RequestFormRepository> logger) : IRequestFormRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<RequestFormRepository> _logger = logger;

        public async Task<bool> SetRequestForm(RequestForm requestForm)
        {
            try
            {
                await _context.RequestForms.AddAsync(requestForm);
                await _context.SaveChangesAsync();

                foreach (string item in requestForm.Coffees)
                {
                    CoffeeType coffeeType = new()
                    {
                        Name = item,
                        RequestFormId = requestForm.Id
                    };

                    await _context.CoffeeTypes.AddAsync(coffeeType);
                }

                foreach (string item in requestForm.VoiceTextMessages)
                {
                    VoiceTextMessage voiceTextMessage = new()
                    {
                        RequestFormId = requestForm.Id,
                        Name = item
                    };

                    await _context.VoiceTextMessages.AddAsync(voiceTextMessage);
                }

                foreach (string item in requestForm.Disabilities)
                {
                    Disability disability = new()
                    {
                        Name = item,
                        RequestFormId = requestForm.Id
                    };

                    await _context.Disabilities.AddAsync(disability);
                }

                foreach (string item in requestForm.CookingItemsAvailable)
                {
                    CookingItem cookingItem = new()
                    {
                        Name = item,
                        RequestFormId = requestForm.Id
                    };

                    await _context.CookingItems.AddAsync(cookingItem);
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"An error has occurred when trying to enter Request Form data => {ex}");
            }
            
            return false;
        }
    }
}
