using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Models
{
    public class CustomerDataStore : ICustomerDataStore
    {
        private readonly IList<Customer> data;
        private readonly string filePath;

        public CustomerDataStore(string filePath, IList<Customer> customers)
        {
            this.filePath = filePath;
            this.data = customers;
        }

        public IEnumerable<Customer> Get()
        {
            return this.data;
        }

        public Customer Get(int id)
        {
            Customer customer = this.data.FirstOrDefault(x => x.Id == id);
            if (customer != null)
                return customer;
            else
                throw new Exception("Customer not found");
        }

        public void Add(Customer model)
        {
            if (!this.data.Select(x => x.Id).Contains(model.Id))
            {
                this.data.Add(model);
            }
            else
                throw new Exception($"Customer with id of {model.Id} already found. Id must be unique");
            WriteToJson();
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
            WriteToJson();
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
            WriteToJson();
        }

        private void WriteToJson()
        {
            JsonCustomerData jsonCustomerData = new JsonCustomerData(this.data.OrderBy(x => x.Id));
            string json = JsonConvert.SerializeObject(jsonCustomerData, Formatting.Indented);
            File.WriteAllText(this.filePath, json);
        }

        private class JsonCustomerData
        {
            public JsonCustomerData(IEnumerable<Customer> customers)
            {
                Customers = customers;
            }
            [JsonProperty("customers")]
            public IEnumerable<Customer> Customers { get; set; }
        }
    }
}
