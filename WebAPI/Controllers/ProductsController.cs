using System.Net;
using System.Web.Http;
using Entities;
using Logic;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly ProductLogic _logic = new ProductLogic();

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var products = _logic.GetAll();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetProductById")]
        public IHttpActionResult Get(int id)
        {
            var product = _logic.Get(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _logic.Add(product);
            return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != product.Id)
                return BadRequest();
            if (!_logic.Finded(id))
                return NotFound();
            _logic.Update(product);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            if (!_logic.Finded(id))
                return NotFound();
            _logic.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
