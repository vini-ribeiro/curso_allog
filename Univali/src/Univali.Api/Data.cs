using Univali.Api.Entities;

namespace Univali.Api
{
    public class Data
    {
        public List<Customer> Customers { set; get; }
        private static Data? _instance;

        private Data()
        {
            Customers = new List<Customer>
            {
                new  Customer
                {
                    Id = 1,
                    Name = "Customer 1",
                    Cpf = "11111111111",
                    Addresses = new List<Address>()
                    {
                        new Address()
                        {
                            Id = 1,
                            Street = "Primeira rua do Customer 1",
                            City = "Primeira cidade do Customer 1"
                        },
                        new Address()
                        {
                            Id = 2,
                            Street = "Segunda rua do Customer 1",
                            City = "Segunda cidade do Customer 1"
                        }
                    }
                },
                new Customer
                {
                    Id = 2,
                    Name = "Customer 2",
                    Cpf = "22222222222",
                    Addresses = new List<Address>()
                    {
                        new Address()
                        {
                            Id = 1,
                            Street = "Primeira rua do Customer 2",
                            City = "Primeira cidade do Customer 2"
                        },
                        new Address()
                        {
                            Id = 2,
                            Street = "Segunda rua do Customer 2",
                            City = "Segunda cidade do Customer 2"
                        }
                    }
                },
                new Customer
                {
                    Id = 3,
                    Name = "Customer 3",
                    Cpf = "33333333333",
                    Addresses = new List<Address>()
                    {
                        new Address()
                        {
                            Id = 1,
                            Street = "Primeira rua do Customer 3",
                            City = "Primeira cidade do Customer 3"
                        },
                        new Address()
                        {
                            Id = 2,
                            Street = "Segunda rua do Customer 3",
                            City = "Segunda cidade do Customer 3"
                        }
                    }
                }
            };
        }

        public static Data Instance
        {
            get
            {
                /*if (_instance == null)
                {
                    _instance = new Data();
                }

                return _instance;*/

                return _instance ??= new Data();
            }
        }
    }
}