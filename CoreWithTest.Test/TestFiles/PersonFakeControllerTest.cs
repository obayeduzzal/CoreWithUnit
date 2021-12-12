using CoreWithTest.BA.Interface;
using CoreWithTest.BA.Service;
using CoreWithTest.DAL.Model;
using CoreWithUnit.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreWithTest.Test.TestFiles
{
    public class PersonFakeControllerTest
    {
        private readonly PersonFakeController _controller;
        private readonly IPersonFake _service;

        public PersonFakeControllerTest()
        {
            _service = new PersonServiceFake();
            _controller = new PersonFakeController(_service);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.Get();
            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        //[Fact]
        //public void Get_WhenCalled_ReturnsAllItems()
        //{
        //    // Act
        //    var okResult = _controller.Get() as OkObjectResult;
        //    // Assert
        //    var items = Assert.IsType<List<Person>>(okResult.Value);
        //    Assert.Equal(3, items.Count);
        //}

        [Fact]
        public void GetById_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            // Act
            var random = new System.Random();
            var notFoundResult = _controller.Get(random.Next());

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        //[Fact]
        //public void GetById_ExistingIdPassed_ReturnsOkResult()
        //{
        //    // Arrange
        //    int id = 3;

        //    // Act
        //    var okResult = _controller.Get(id);

        //    // Assert
        //    Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        //}

        [Fact]
        public void GetById_ExistingidPassed_ReturnsRightItem()
        {
            // Arrange
            int testid = 3;

            // Act
            var okResult = _controller.Get(testid) as OkObjectResult;

            // Assert
            Assert.IsType<Person>(okResult.Value);
            Assert.Equal(testid, (okResult.Value as Person).Id);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new Person()
            {
                UserPassword = "12345",
                CreatedOn = DateTime.Now,
                IsDeleted = false,
                UserEmail = "a@b.c"
            };
            _controller.ModelState.AddModelError("UserName", "Required");

            // Act
            var badResponse = _controller.Post(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var testItem = new Person()
            {
                UserName = "Name10",
                UserPassword = "12345",
                CreatedOn = DateTime.Now,
                IsDeleted = false,
                UserEmail = "a10@b.c"
            };

            // Act
            var createdResponse = _controller.Post(testItem) as CreatedAtActionResult;
            var item = createdResponse.Value as Person;

            // Assert
            Assert.IsType<Person>(item);
            Assert.Equal("Name10", item.UserName);
        }
    }
}
