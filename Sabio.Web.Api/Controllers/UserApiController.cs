using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserApiController : BaseApiController
    {
        private IUsersService _service = null;
        private IAuthenticationService<int> _authService = null;

        public UserApiController(IUsersService service,
            ILogger<PingApiController> logger,
            IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        // GET api/Users/""
        [HttpGet]
        public ActionResult<ItemsResponse<User>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {

                List<User> list = _service.GetAll();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<User>() { Items = list };
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
        // api/Users/{id:int}
        [HttpGet("{id:int}")] //route pattern
        public ActionResult<ItemResponse<User>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {

                User user = _service.Get(id);

                //ItemResponse<User> response = new ItemResponse<User>();

                //response.Item = add;

                if (user == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<User>() { Item = user };
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
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(UsersAddRequest model)
        {
            // of new User
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
        public ActionResult<ItemResponse<int>> Update(UsersUpdateRequest model)
        {


            int userId = _authService.GetCurrentUserId();
            _service.Update(model, userId);
            IUserAuthData user = _authService.GetCurrentUser();

            SuccessResponse response = new SuccessResponse();

            return Ok(response);

        }

    }
}