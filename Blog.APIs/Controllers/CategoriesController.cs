using Blog.Core.DTos;
using Blog.Core.Interfaces;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Root URL => https://localhost:7241
    // Base URL => Root + Route => https://localhost:7241/api/Categories
    // Spatial URL => https://localhost:7241/api/Categories/GetById/{id}
    public class CategoriesController : ControllerBase
    {
        // DI
        private readonly ICategoryService _category;

        public CategoriesController(ICategoryService category)
        {
            _category = category;
        }

        [HttpGet("Old")] // https://localhost:7241/api/Categories/Old
        public async Task<IEnumerable<Category>> GetAll1()
        {
            var Categories = await _category.GetAllAsync();
            if (Categories is null || !Categories.Any())
                return new List<Category>();
            return Categories;
        }


        [HttpGet] // https://localhost:7241/api/Categories
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _category.GetAllAsync();
                if (!categories.Any() || categories is null)
                {
                    return NotFound(new
                    {
                        //StatusCode = 404,
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "No Category Found",
                        Data = new List<Category>()

                    }); // Status 404
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Categories Retrived Successfully",
                        Data = categories
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "An Error Occurred while Retrieving Data",
                        Error = ex.Message
                    });
            }
        }


        [HttpGet("{id}")] // https://localhost:7241/api/Categories/1
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _category.GetByIdAsync(id);

                if (category is null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = $"Category With {id} Not Found",
                    });
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Category Retrived Successfully",
                    Data = category
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "An Error Occurred while Retrieving Data",
                        Error = ex.Message
                    });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTo categoryDTo)
        {
            try
            {
                // validate with dataanotation 
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invaild Category Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                await _category.CreateAsync(new Category()
                {
                    Name = categoryDTo.Name,
                });
                return StatusCode(StatusCodes.Status201Created, new
                {
                    StatusCode = 201,
                    Message = "Category Created Successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "An Error Occurred while Creating Category",
                        Error = ex.Message
                    });
            }
        }




        [HttpPut]

        public async Task<IActionResult> Update(CategoryDTo categoryDTo)
        {
            try
            {
                var oldCategory = await _category.GetByIdAsync(categoryDTo.Id);
                if (oldCategory is null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "No Category Found",
                    });

                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invaild Category Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                oldCategory.Name = categoryDTo.Name;
                await _category.UpdateAsync(oldCategory);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Category Updated successfully",
                    //Data = oldCategory
                    Data = await _category.GetByIdAsync(categoryDTo.Id)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "An Error Occurred while Updating Data",
                        Error = ex.Message
                    });
            }
        }


        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, CategoryDTo categoryDTo)
        {
            try
            {
                var oldCategory = await _category.GetByIdAsync(id);
                if (oldCategory is null)
                    return NotFound(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "No Category Found",
                    });

                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invaild Category Data",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    });
                oldCategory.Name = categoryDTo.Name;
                await _category.UpdateAsync(id, oldCategory);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Category Updated successfully",
                    //Data = oldCategory
                    Data = await _category.GetByIdAsync(id)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "An Error Occurred while Updating Data",
                        Error = ex.Message
                    });
            }
        }




        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _category.GetByIdAsync(id);
                if (category == null) return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Category With Id {id} Not Found",
                });

                await _category.DeleteAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Category Deleted Successfully",
                    DeletedData = category
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "An Error Occurred while Deleting Data",
                        Error = ex.Message
                    });
            }
        }
    }
}
