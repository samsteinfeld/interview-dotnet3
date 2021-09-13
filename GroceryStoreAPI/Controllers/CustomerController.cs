using GroceryStoreAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerDataStore CustomerDataStore;

        public CustomerController(ICustomerDataStore customerDataStore)
        {
            this.CustomerDataStore = customerDataStore;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            try
            {
                return Ok(this.CustomerDataStore.Get());
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Customer>> Get(int id)
        {
            try
            {
                return Ok(this.CustomerDataStore.Get(id));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<Customer> Post(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.CustomerDataStore.Add(customer);
                }
                else
                {
                    return BadRequest(ModelState.Values.First());
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
        }

        public IActionResult Put(Customer customer)
        {
            try
            {
               this.CustomerDataStore.Update(customer);
            }
            catch
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                this.CustomerDataStore.Delete(id);
            }
            catch 
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
