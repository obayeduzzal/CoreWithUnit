using CoreWithTest.BA.Interface;
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
    public class PersonFakeController : ControllerBase
    {
        private readonly IPersonFake _fakeService;
        public PersonFakeController(IPersonFake fakeService)
        {
            _fakeService = fakeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var items = _fakeService.GetAllItems();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = _fakeService.GetById(id);
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
            var item = _fakeService.Add(value);
            return CreatedAtAction("Get", new { id = item.Id }, item);
        }
        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var existingItem = _fakeService.GetById(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            _fakeService.Remove(id);
            return NoContent();
        }
    }
}
