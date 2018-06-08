using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack.Redis;

namespace ps
{
    class Program
    {
        static void Main(string[] args)
        {
            var lastId = 0L;

            using (IRedisClient client = new RedisClient("redis://192.241.154.174:6379")) 
            {
                var customerClient = client.As<Customer>();
                var customer = new Customer 
                {
                    Id = customerClient.GetNextSequence(),
                    Address = "123 Main Street",
                    Name = "Bob Green",
                    Orders = new List<Order> 
                    {
                        new Order { OrderNumber = "AB123" },
                        new Order { OrderNumber = "AB124" }
                    }
                };

                var storedCustomer = customerClient.Store(customer);
                lastId = storedCustomer.Id;
            }

            using (IRedisClient client = new RedisClient("redis://192.241.154.174:6379")) 
            {
                var customerClient = client.As<Customer>();
                var customer = customerClient.GetById(lastId);
                Console.WriteLine($"Got Customer {customer.Id}, with name: {customer.Name}");
            }
        }
    }


    public class Customer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class Order
    {
        public string OrderNumber { get; set; }
    }
}

