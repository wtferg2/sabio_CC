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
    
    [Route("api/Friends")]
    [ApiController]
    public class FriendApiController : BaseApiController
                
    {
        private IFriendsService _service = null;
        private IAuthenticationService<int> _authService = null;


        public FriendApiController(IFriendsService service,
            ILogger<PingApiController> logger,
            IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        // GET api/Friends/""
        [HttpGet]
        public ActionResult<ItemsResponse<Friend>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {

                List<Friend> list = _service.GetAll();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Friend>() { Items = list };
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
        // api/Friends/{id:int}
        [HttpGet("{id:int}")] //route pattern
        public ActionResult<ItemResponse<Friend>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {

                Friend friend = _service.Get(id);

                //ItemResponse<Friend> response = new ItemResponse<Friend>();

                //response.Item = add;

                if (friend == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Friend>() { Item = friend };
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
        //public ActionResult<SuccessResponse> Delete(int id)
        //{
        //    int code = 200;
        //    BaseResponse response = null;
        //    try
        //    {

        //        _service.Delete(id);

        //        response = new SuccessResponse();

        //    }
        //    catch (Exception ex)
        //    {

        //        code = 500;
        //        response = new ErrorResponse(ex.Message);
        //    }
        //    return StatusCode(code, response);
        //}

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Friend>>> Pagination(int pageIndex, int pageSize)
        {
            
            ActionResult result = null;
            try
            {

                Paged<Friend> paged = _service.Pagination(pageIndex, pageSize);

                if (paged == null)
                {
                    
                    result = NotFound404(new ErrorResponse("Application resource not found."));
                }
                else
                {

                    ItemResponse<Paged<Friend>> response = new ItemResponse<Paged<Friend>>() { Item = paged };
                    result = Ok200(response);
                }

            }
            catch (Exception ex)
            {
                //base.Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message));
                
            }
            return result;
        }



        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(FriendsAddRequest model)
        {
            // of new Friend
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
        public ActionResult<ItemResponse<int>> Update(FriendsUpdateRequest model)
        {


            int userId = _authService.GetCurrentUserId();
            _service.Update(model, userId);
            IUserAuthData user = _authService.GetCurrentUser();

            SuccessResponse response = new SuccessResponse();

            return Ok(response);

        }


    }
}