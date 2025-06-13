using System.Net;
using System.Web.Http;
using Entities;
using Logic;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly UserLogic _logic = new UserLogic();

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var users = _logic.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetUserById")]
        public IHttpActionResult Get(int id)
        {
            var user = _logic.Get(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _logic.Add(user);
            return CreatedAtRoute("GetUserById", new { id = user.Id }, user);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != user.Id)
                return BadRequest();
            if (!_logic.Finded(id))
                return NotFound();
            _logic.Update(user);
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
