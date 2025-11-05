using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Helpers;
using PrideLink.Server.Interfaces;
using PrideLink.Shared.Location;
using PrideLink.Shared.UserInfo;
using System.Data;

namespace PrideLink.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLocation : ControllerBase
    {
        private readonly ILocationInterface _locationInterface;
        private readonly JWTHelper _jWTHelper;

        public UserLocation(ILocationInterface locationInterface, JWTHelper jWTHelper)
        {
            _locationInterface = locationInterface;
            this._jWTHelper = jWTHelper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,General")]
        [Route("AddUpdateUserLocation")]
        public IActionResult AddUpdateUserLocation(UserLocationData userLocation)
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            string locationUri = "api/UserAge/GetUserAge";

            bool response = _locationInterface.AddUpdateUserLocation(userLocation, userNo);
            if (response)
            {
                return Created(locationUri, response);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("GetLocationFromCityAndTown")]
        public IActionResult GetLocationFromCityAndTown()
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            UserLocationData location = _locationInterface.GetUserLocationFromTownAndCity(userNo);
            if (location != null)
            {
                return Ok(location);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("AddTownAndCityToUser")]
        public IActionResult AddTownAndCityToUser(int cityNo)
        {
            var jwtToken = Request.Cookies["AuthToken"];

            int userNo = int.Parse(_jWTHelper.GetUserNo(jwtToken));

            bool success = _locationInterface.AddTownAndCityToUser(userNo, cityNo);
            if (success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
                
        }

        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("GetCityAndTowns")]
        public IActionResult GetCityAndTowns()
        {
            return Ok(_locationInterface.GetTownCityLocations());
        }
    }
}
