using GroceryStoreAPI.Controllers;
using GroceryStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GroceryStoreUnitTests
{
    public class GroceryStoreTests
    {
        private readonly CustomerController CustomerController;
        private readonly ICustomerDataStore CustomerDataStore;
        private readonly IEnumerable<Customer> ExistingCustomers;

        public GroceryStoreTests()
        {
            CustomerDataStore = new CustomerDataStoreFake();
            CustomerController = new CustomerController(CustomerDataStore);
            ExistingCustomers = CustomerDataStore.Get();
        }

        private int UniqueCustomerId => ExistingCustomers.Last().Id + 1;
        private int ExistingCustomerId => ExistingCustomers.First().Id;

        [Fact]
        public void Get_WhenCalled_ReturnsAllCustomers()
        {
            IActionResult okResult = CustomerController.Get().Result;
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            OkObjectResult okResult = CustomerController.Get().Result as OkObjectResult;
            IEnumerable<Customer> customers = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Equal(3, customers.ToList().Count);
        }

        [Fact]
        public void GetById_WhenCalled_ReturnsOkResult()
        {
            IActionResult okResult = CustomerController.Get(ExistingCustomerId).Result;
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void GetById_Unknown_ReturnsNotFoundResult()
        {
            IActionResult notFoundResult = CustomerController.Get(UniqueCustomerId).Result;
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ReturnsRightItem()
        {
            OkObjectResult okResult = CustomerController.Get(ExistingCustomerId).Result as OkObjectResult;
            Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(ExistingCustomerId, (okResult.Value as Customer).Id);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            Customer customerNameMissing = new Customer(0, null);
            CustomerController.ModelState.AddModelError("Name", "Required");
            CustomerController.ModelState.AddModelError("Id", "Must be greater than 0");
            ActionResult<Customer> badResponse = CustomerController.Post(customerNameMissing);
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            Customer testCustomer = new Customer(UniqueCustomerId, "Test");
            ActionResult<Customer> createdResponse = CustomerController.Post(testCustomer);
            Assert.IsType<CreatedAtActionResult>(createdResponse.Result);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            Customer testCustomer = new Customer(UniqueCustomerId, "Test");
            ActionResult<Customer> createdResponse = CustomerController.Post(testCustomer);
            Assert.Equal(4, ExistingCustomers.ToList().Count);
        }

        [Fact]
        public void Update_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            Customer testCustomer = new Customer(UniqueCustomerId, "Test");
            IActionResult badResponse = CustomerController.Put(testCustomer);
            Assert.IsType<NotFoundResult>(badResponse);
        }

        [Fact]
        public void Update_ExistingIdPassed_ReturnsOkResult()
        {
            Customer updateCustomer = ExistingCustomers.ToList()[ExistingCustomerId];
            updateCustomer.Name = updateCustomer + DateTime.Now.ToShortTimeString();
            IActionResult okResponse = CustomerController.Put(updateCustomer);
            Assert.IsType<OkResult>(okResponse);
        }

        [Fact]
        public void Update_ExistingIdPassed_RemovesOneItem()
        {
            IActionResult okResponse = CustomerController.Delete(ExistingCustomerId);
            Assert.Equal(2, CustomerDataStore.Get().Count());
        }

        [Fact]
        public void Remove_ExistingIdPassed_ReturnsOkResult()
        {
            IActionResult okResponse = CustomerController.Delete(ExistingCustomerId);
            Assert.IsType<OkResult>(okResponse);
        }

        [Fact]
        public void Remove_ExistingIdPassed_RemovesOneItem()
        {
            IActionResult okResponse = CustomerController.Delete(ExistingCustomerId);
            Assert.Equal(2, CustomerDataStore.Get().Count());
        }
    }
}
