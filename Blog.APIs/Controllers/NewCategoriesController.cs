using Blog.Core.DTos;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewCategoriesController : ControllerBase
    {
        // DI
        //private readonly IGenaricRepository<Category> _unitOfWork.Categories;

        //public NewCategoriesController(IGenaricRepository<Category> category)
        //{
        //    _unitOfWork.Categories = category;
        //}

        private readonly IUnitOfWork _unitOfWork;
        public NewCategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAllAsync();
                if (categories is null || !categories.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",
                        Data = new List<Category>()
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Data Retrived Successfully",
                        Data = categories
                    });
                }
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, new
                    {
                        StatusCode = 500,
                        Message = "An Error Occured While Retraving The Data",
                        Error = ex.Message
                    });
                }
            }
        }


        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category is null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found",
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Data Retrived Successfully",
                        Data = category
                    });
                }
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, new
                    {
                        StatusCode = 500,
                        Message = "An Error Occured While Retraving The Data",
                        Error = ex.Message
                    });
                }
            }
        }


        [HttpPost]

        public async Task<IActionResult> Create(CategoryDTo categoryDTo)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invaild Category Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });

                var category = new Category
                {
                    Name = categoryDTo.Name,
                };
                await _unitOfWork.Categories.CreateAsync(category);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return StatusCode(201, new
                    {
                        StatusCode = 201,
                        Message = "Data Added Sucessufully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Data Not Added"
                    });
                }
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, new
                    {
                        StatusCode = 500,
                        Message = "An Error Occured While Retraving The Data",
                        Error = ex.Message
                    });
                }
            }
        }


        [HttpPut]

        public async Task<IActionResult> Update(CategoryDTo categoryDTo)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invaild Category Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });

                var oldCategory = await _unitOfWork.Categories.GetByIdAsync(categoryDTo.Id);
                if (oldCategory == null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Data Not Found"
                    });
                oldCategory.Name = categoryDTo.Name;

                _unitOfWork.Categories.Update(oldCategory);

                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return StatusCode(200, new
                    {
                        StatusCode = 200,
                        Message = "Data Updated Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        Message = "Data Not Updated"
                    });
                }
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, new
                    {
                        StatusCode = 500,
                        Message = "An Error Occured While Retraving The Data",
                        Error = ex.Message
                    });
                }
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(CategoryDTo categoryDTo)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(categoryDTo.Id);
                if (category == null)
                    return NotFound(
                        new
                        {
                            StatusCode = StatusCodes.Status404NotFound,
                            Message = "Data Not Found"
                        }
                    );
                _unitOfWork.Categories.Delete(category);
                int result = await _unitOfWork.SaveAsync();
                Console.WriteLine(result);
                if (result > 0)
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Data Deleted Successfully",
                        Data = category
                    });
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Data Not Removed Successfully"
                });
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, new
                    {
                        StatusCode = 500,
                        Message = "An Error Occured While Retraving The Data",
                        Error = ex.Message
                    });
                }
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category == null)
                    return NotFound(
                        new
                        {
                            StatusCode = StatusCodes.Status404NotFound,
                            Message = "Data Not Found"
                        }
                    );
                _unitOfWork.Categories.Delete(category);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Data Deleted Successfully",
                        Data = category
                    });
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Data Not Deleted Successfully"
                });
            }
            catch (Exception ex)
            {
                {
                    return StatusCode(500, new
                    {
                        StatusCode = 500,
                        Message = "An Error Occured While Retraving The Data",
                        Error = ex.Message
                    });
                }
            }
        }
    }
}