using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DTO;
using Contracts.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private IProductService _productSvc;

        public ProductsController(IProductService productService)
        {
            _productSvc = productService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<ProductDTO> Get()
        {
            return _productSvc.GetProducts();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var product = _productSvc.FindProduct(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]ProductDTO product)
        {
            if (product == null)
                return BadRequest("Product is null");

            var productId = _productSvc.SaveProduct(product);

            if (productId == null)
                return StatusCode(500, "Error creating product");

            return Ok(new { id = productId });

        }
    }
}
