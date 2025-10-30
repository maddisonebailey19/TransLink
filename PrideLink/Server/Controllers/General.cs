using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrideLink.Server.Helpers;
using PrideLink.Server.Interfaces;
using PrideLink.Shared.General;
using PrideLink.Shared.UserInfo;
using System.Data;

namespace PrideLink.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class General : ControllerBase
    {
        private readonly IGeneralInterface _generalInterface;
        private readonly JWTHelper _jWTHelper;

        public General(IGeneralInterface generalInterface, JWTHelper jWTHelper)
        {
            _generalInterface = generalInterface;
            this._jWTHelper = jWTHelper;
        }


        [HttpGet]
        [Authorize(Roles = "Admin,General")]
        [Route("GetHobbies")]
        public IActionResult GetHobbies()
        {
            List<Hobbys> response = _generalInterface.GetHobbies();
            return Ok(response);
        }
    }
}
