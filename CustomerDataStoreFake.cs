using GroceryStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreUnitTests
{
    public class CustomerDataStoreFake : ICustomerDataStore
    {
        private readonly IList<Customer> data;

        public CustomerDataStoreFake()
        {
            data = new List<Customer>()
            {
                new Customer(1, "Bob"),
                new Customer(2, "Mary"),
                new Customer(3, "Joe")
            };
        }

        public IEnumerable<Customer> Get()
        {
            return data;
        }

        public Customer Get(int id)
        {
            Customer customer = this.data.FirstOrDefault(x => x.Id == id);
            if (customer != null)
                return customer;
            else
                throw new Exception("Customer not found");
        }

        public void Add(Customer newItem)
        {
            data.Add(newItem);
        }

        public void Update(Customer model)
        {
            Customer customer = Get(model.Id);
            if (customer != null)
            {
                Delete(model.Id);
                Add(model);
            }
            else
                throw new Exception("Customer not found");
        }

        public void Delete(int customerId)
        {
            Customer customer = Get(customerId);
            if (customer != null)
            {
                this.data.Remove(customer);
            }
            else
                throw new Exception("Customer not found");
        }
    }
}
