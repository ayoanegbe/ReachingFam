using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace ReachingFam.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {

            _logger = logger;
        }

        //[Route("Error/404")]
        //public IActionResult Error404()
        //{
        //    _logger.LogInformation("A 404 Error Has occurred");
        //    return View();
        //}

        [Route("Error/{code}")]
        public IActionResult Error(int code = 0)
        {
            try
            {
                if (code == 401)//change code to enumerations
                {
                    ViewData["ErrorMessage"] = "Am sorry, you are not authorize to view this Module, Kindly Contact your Administrator";
                    ViewData["ErrorCode"] = "401 Unauthorized";
                    _logger.LogInformation("401 Unauthorized error Has occurred");

                }

                else if (code == 403)
                {
                    ViewData["ErrorMessage"] = "Forbidden Request, please make sure you have access to this path and try again";
                    ViewData["ErrorCode"] = "403 Forbidden";
                    _logger.LogInformation("403 Forbidden error Has occurred");
                }



                else if (code == 400)
                {
                    ViewData["ErrorMessage"] = "UnAuthenticated request, Please Authenticate to get the requested response";
                    ViewData["ErrorCode"] = "400 Bad Request";
                    _logger.LogInformation("400 Bad Request error Has occurred");
                }

                else if (code == 404)
                {
                    ViewData["ErrorMessage"] = "Not Found, The requested resource cannot be found";
                    ViewData["ErrorCode"] = "404 Not Found";
                    _logger.LogInformation("404 Not Found error Has occurred");
                }

                else if (code == 408)
                {
                    ViewData["ErrorMessage"] = "Request Timeout, please try again and if it still persist, contact our administrator";
                    ViewData["ErrorCode"] = "408 Request Timeout";
                    _logger.LogInformation("408 Request Timeout error Has occurred");
                }


                else if (code == 500)
                {
                    ViewData["ErrorMessage"] = "This is so Embarrassing; please try again and contact our administrator if this persists";
                    ViewData["ErrorCode"] = "500 Internal Server Error";
                    _logger.LogInformation("500 Internal server error Has occurred");
                }


                else if (code == 501)
                {
                    ViewData["ErrorMessage"] = "The request method is not supported by the server and cannot be handled";
                    ViewData["ErrorCode"] = "501 Not Implemented";
                    _logger.LogInformation("501 Not Implemented error Has occurred");
                }
                else if (code == 502)
                {
                    ViewData["ErrorMessage"] = "invalid response from gateway server";
                    ViewData["ErrorCode"] = "502 Bad Gateway";
                    _logger.LogInformation("502 Bad Gateway error Has occurred");
                }
                else if (code == 503)
                {
                    ViewData["ErrorMessage"] = "We are Getting serious Traffic today, The server is not ready to handle the request.Please try again after some minutes";
                    ViewData["ErrorCode"] = "503 Service Unavailable";
                    _logger.LogInformation("503 Service Unavailable error Has occurred");
                }


                else
                {
                    ViewData["ErrorMessage"] = "Something is not right at the moment, Please hold while our technical team is fixing it";

                    ViewData["ErrorCode"] = "Something Broke";
                    _logger.LogInformation("An Unknown Error Has occurred");
                }



            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"An error has occurred - {ex}");

            }
            return View();
        }
    }
}
