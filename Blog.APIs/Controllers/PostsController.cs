using Blog.Core.DTos;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        // DI
        private readonly IUnitOfWork _unitOfWork;
        public PostsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var posts = await _unitOfWork.Posts.GetAllAsync();
                if (posts is null || !posts.Any())
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",
                        Data = new List<Post>()
                    });

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Data Retrived Successfully",
                    Data = posts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Error = ex.Message,
                    Message = "An Error Occurred While Processing Request"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var post = await _unitOfWork.Posts.GetByIdAsync(id);
                if (post is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found"
                    });
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Data Retrieved Successfully",
                    Data = post
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Error = ex.Message,
                    Message = "An Error Occurred While Processing Request"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostDTo postDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invaild Post Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });

                var category = await _unitOfWork.Categories.GetByIdAsync(postDTo.CategoryId);
                var user = await _unitOfWork.Users.GetByIdAsync(postDTo.UserID);
                if (category == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Invalid CategoryId. Category does not exist."
                    });
                }
                if (user == null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Invalid UserId. User does not exist."
                    });
                }

                Post post = new Post()
                {
                    Title = postDTo.Title,
                    Content = postDTo.Content,
                    CategoryId = postDTo.CategoryId,
                    UserID = postDTo.UserID,
                };


                await _unitOfWork.Posts.CreateAsync(post);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Created($"http://localhost:5237/api/Post/{post.Id}", new
                    {
                        StatusCode = 201,
                        Message = "Post Added Successfully"
                    });
                //return CreatedAtAction(nameof(GetById), new { id = post.Id }, new
                //{
                //    StatusCode = 201,
                //    Message = "Post Added Successfully"
                //});
                //return StatusCode(201,

                //new
                //{
                //    StatusCode = 201,
                //    Message = "Post Added Successfully"
                //});

                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Data Not Added"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Error = ex.Message,
                    Message = "An Error Occurred While Processing Request"
                });
            }
        }


        [HttpPut]
        public async Task<IActionResult> Update(PostDTo postDTo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invaild Post Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });

                var post = await _unitOfWork.Posts.GetByIdAsync(postDTo.Id);
                if (post is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found"
                    });

                post.Title = postDTo.Title;
                post.Content = postDTo.Content;
                post.CategoryId = postDTo.CategoryId;

                _unitOfWork.Posts.Update(post);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return StatusCode(200, new
                    {
                        StatusCode = 200,
                        Message = "Post Updated  Successfully"
                    });

                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Post Not Updated"
                });


            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Error = ex.Message,
                    Message = "An Error Occurred While Processing Request"
                });
            }
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var post = await _unitOfWork.Posts.GetByIdAsync(id);
                if (post is null) return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Data Not Found"
                });
                _unitOfWork.Posts.Delete(post);

                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                    return StatusCode(200,

                        new
                        {
                            StatusCode = 200,
                            Message = "Post Deleted Successfully"
                        });

                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Data Not Deleted"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Error = ex.Message,
                    Message = "An Error Occurred While Processing Request"
                });
            }
        }
    }
}
