using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DMTools.Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public string Ping()
        {
            return "Pong";
        }
        
        // [HttpPost]
        // public ApplicationUser Create()
        // {
        //     var rng = new Random();
        //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //         {
        //             Date = DateTime.Now.AddDays(index),
        //             TemperatureC = rng.Next(-20, 55),
        //             Summary = Summaries[rng.Next(Summaries.Length)]
        //         })
        //         .ToArray();
        // }
    }
}