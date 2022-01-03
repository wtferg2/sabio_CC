using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Addresses;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressApiController : BaseApiController
    {
        private IAddressesService _service = null;
        private IAuthenticationService<int> _authService = null;

        public AddressApiController(IAddressesService service, 
            ILogger<PingApiController> logger, 
            IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        // GET api/addresses/""
        [HttpGet]
        public ActionResult<ItemsResponse<Address>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {

                List<Address> list = _service.GetTop();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Address>() { Items = list };
                }
                
            }
            catch (Exception ex)
            {
                //base.Logger.LogError(ex.ToString());
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
        // api/addresses/{id:int}
        [HttpGet("{id:int}")] //route pattern
        public ActionResult<ItemResponse<Address>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {

                Address add = _service.Get(id);

                //ItemResponse<Address> response = new ItemResponse<Address>();

                //response.Item = add;

                if (add == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Address>() { Item = add };
                }
            }
            catch (SqlException sqlEx)
            {
                iCode = 500;
                response = new ErrorResponse(sqlEx.Message);
            }
            catch (ArgumentException argEx)
            {
                iCode = 500;
                response = new ErrorResponse(argEx.Message);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                iCode = 500;
                response = new ErrorResponse(ex.Message);                
            }            
            return StatusCode(iCode, response);
        }
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {

                _service.Delete(id);

                response = new SuccessResponse();

            }
            catch (Exception ex)
            {

                code = 500;
                response= new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(AddressAddRequest model)
        {
            // of new address
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                IUserAuthData user = _authService.GetCurrentUser();
                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);

            }
            return result;
        }
        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(AddressUpdateRequest model)
        {
            
            
            int userId = _authService.GetCurrentUserId();
            _service.Update(model, userId);
            IUserAuthData user = _authService.GetCurrentUser();

            SuccessResponse response = new SuccessResponse();

            return Ok(response);

        }

    }
}
