using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReachingFam.Core.Data;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReachingFam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestFormController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RequestFormController> _logger;
        private readonly IRequestFormRepository _repository;

        public RequestFormController(ApplicationDbContext context, ILogger<RequestFormController> logger, IRequestFormRepository repository)
        {
            _context = context;
            _logger = logger;
            _repository = repository;
        }

        // POST api/<RequestFormController>
        [HttpPost("SendRequest")]
        //[Authorize]
        public async Task<Response> Post([FromBody] RequestForm request)
        {
            Response response = new();

            bool result = await _repository.SetRequestForm(request);

            if (result)
            { 
                response.Status = ResponseStatus.Success;
                response.Message = "Successful";
            }
            else 
            { 
                response.Status = ResponseStatus.Failed;
                response.Message = "Not successful";
            }

            return response;
        }

    }
}
