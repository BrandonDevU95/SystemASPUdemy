using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemASPUdemy.Data;
using SystemASPUdemy.Entities.Warehouse;
using SystemASPUdemy.Web.Models.Warehouse.Category;

namespace SystemASPUdemy.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CategoryViewModel>> List()
        {
            var categoria = await _context.Categories.ToListAsync();
            return categoria.Select( c => new CategoryViewModel
            {
                id = c.id,
                Name = c.Name,
                Description = c.Description,
                Status = c.Status
            });
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Show([FromRoute] int id)
        {           
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(new CategoryViewModel { 
                id = category.id,
                Name = category.Name,
                Description = category.Description,
                Status = category.Status
            });
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.id <= 0)
            {
                return BadRequest();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.id == model.id);

            if (category == null)
            {
                return BadRequest();
            }

            category.Name = model.Name;
            category.Description = model.Description;

            try
            {
                await _context.SaveChangesAsync();

            } catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = new Category { 
                Name = model.Name,
                Description = model.Description,
                Status = true
            };

            _context.Categories.Add(category);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }            

            return Ok(category);
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Lock([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.id == id);

            if (category == null)
            {
                return BadRequest();
            }

            category.Status = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Unlock([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.id == id);

            if (category == null)
            {
                return BadRequest();
            }

            category.Status = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.id == id);
        }
    }
}