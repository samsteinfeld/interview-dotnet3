using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Models
{
    public interface ICustomerDataStore
    {
        IEnumerable<Customer> Get();
        Customer Get(int id);
        void Add(Customer model);
        void Update(Customer model);
        void Delete(int customerId);
    }
}
