using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _unitOfWork.CategoryRepository.FindAll().ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                return BadRequest("Invalid category id");
            }
            var category = _unitOfWork.CategoryRepository.Find(c => c.Id == guid)
                .FirstOrDefault();

            return Ok(category);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string name)
        {
            //Check already existing category
            var existing = _unitOfWork.CategoryRepository.Find(c => c.Name == name)
                .FirstOrDefault();
            if (existing != null)
            {
                return BadRequest("Category already existing");
            }

            var newId = Guid.NewGuid();
            var toSave = new Category
            {
                Id = newId,
                Name = name
            };
            _unitOfWork.CategoryRepository.Create(toSave);
            _unitOfWork.Save();

            return Ok(toSave);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                return BadRequest("Invalid category id");

            var toRemove = _unitOfWork.CategoryRepository.Find(c => c.Id == guid)
                .FirstOrDefault();
            if (toRemove == null)
                return BadRequest("No category found");

            _unitOfWork.CategoryRepository.Delete(toRemove);
            _unitOfWork.Save();

            return Ok(toRemove);
        }
    }
}
