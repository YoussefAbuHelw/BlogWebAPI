using Blog.Core.DTos;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        // DI
        private readonly IUnitOfWork _unitOfWork;
        public CommentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var comments = await _unitOfWork.Comments.GetAllAsync();
                if (comments is null || !comments.Any())
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",
                        Data = new List<Comment>()
                    });

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Data Retrived Successfully",
                    Data = comments
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
                var comment = await _unitOfWork.Comments.GetByIdAsync(id);
                if (comment is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found"
                    });
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Data Retrieved Successfully",
                    Data = comment
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
        public async Task<IActionResult> Create(CommentDTo commentDTo)
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

                var postId = await _unitOfWork.Posts.GetByIdAsync(commentDTo.PostId);
                var userId = await _unitOfWork.Users.GetByIdAsync(commentDTo.UserId);
                if (postId is null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Invalid PostId. Post does not exist."
                    });
                }
                if (userId is null)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Invalid UserId. User does not exist."
                    });
                }

                Comment comment = new Comment()
                {
                    Content = commentDTo.Content,
                    UserId = commentDTo.UserId,
                    PostId = commentDTo.PostId
                };


                await _unitOfWork.Comments.CreateAsync(comment);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Created($"http://localhost:5237/api/Comment/{comment.Id}", new
                    {
                        StatusCode = 201,
                        Message = "Comment Added Successfully"
                    });

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
        public async Task<IActionResult> Update(CommentDTo commentDTo)
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

                var comment = await _unitOfWork.Comments.GetByIdAsync(commentDTo.Id);
                if (comment is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found"
                    });

                comment.Content = commentDTo.Content;
               
                _unitOfWork.Comments.Update(comment);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return StatusCode(200, new
                    {
                        StatusCode = 200,
                        Message = "Comment Updated  Successfully"
                    });

                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Comment Not Updated"
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
                var comment = await _unitOfWork.Comments.GetByIdAsync(id);
                if (comment is null) return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Data Not Found"
                });
                _unitOfWork.Comments.Delete(comment);

                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                    return StatusCode(200,

                        new
                        {
                            StatusCode = 200,
                            Message = "Comment Deleted Successfully"
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
