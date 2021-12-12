using CoreWithTest.BA.Service;
using CoreWithTest.DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWithUnit.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonService _personService;
        public PersonController(PersonService ProductService)
        {
            _personService = ProductService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var items = _personService.GetAllPersons();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _personService.GetPersonByUserId(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Person value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = _personService.AddPerson(value);
            return CreatedAtAction("Get", new { id = item.Id }, item);
        }
        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var existingItem = _personService.GetPersonByUserId(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            _personService.DeletePerson(id);
            return NoContent();
        }
    }
}
