using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.CodingChallenge;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sabio.Web.Api.Controllers
{

    [Route("api/School")]
    [ApiController]
    public class SchoolApiController : BaseApiController

    {
        private ISchoolService _service = null;
        private IAuthenticationService<int> _authService = null;


        public SchoolApiController(ISchoolService service,
            ILogger<PingApiController> logger,
            IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        // GET api/Courses/""
        [HttpGet]
        
        // api/Courses/{id:int}
        [HttpGet("{id:int}")] //route pattern
        public ActionResult<ItemResponse<Course>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {

                Course Course = _service.Get(id);

                //ItemResponse<Course> response = new ItemResponse<Course>();

                //response.Item = add;

                if (Course == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Course>() { Item = Course };
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
        [HttpDelete("Student/{id:int}")]
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

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Course>>> Pagination(int pageIndex, int pageSize)
        {

            ActionResult result = null;
            try
            {

                Paged<Course> paged = _service.Pagination(pageIndex, pageSize);

                if (paged == null)
                {

                    result = NotFound404(new ErrorResponse("Application resource not found."));
                }
                else
                {

                    ItemResponse<Paged<Course>> response = new ItemResponse<Paged<Course>>() { Item = paged };
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
        public ActionResult<ItemResponse<int>> Create(CourseAddRequest model)
        {
            // of new Course
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                IUserAuthData user = _authService.GetCurrentUser();
                int id = _service.Add(model);
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
        public ActionResult<ItemResponse<int>> Update(CourseUpdateRequest model)
        {


            int userId = _authService.GetCurrentUserId();
            _service.Update(model);
            IUserAuthData user = _authService.GetCurrentUser();

            SuccessResponse response = new SuccessResponse();

            return Ok(response);

        }


    }
}