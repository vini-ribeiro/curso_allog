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
                    Name = "Robssom",
                    Cpf = "00891528246545",
                    Addresses = new List<Address>()
                    {
                        new Address()
                        {
                            Id = 1,
                            Street = "Verao do cometa",
                            City = "Elvira"
                        },
                        new Address()
                        {
                            Id = 1,
                            Street = "Inverno do aster",
                            City = "ouvira"
                        }
                    }
                },
                new Customer
                {
                    Id = 2,
                    Name = "Jundiado",
                    Cpf = "00791528237",
                    Addresses = new List<Address>()
                    {
                        new Address()
                        {
                            Id = 1,
                            Street = "Outono do cometa",
                            City = "city do 2"
                        },
                        new Address()
                        {
                            Id = 1,
                            Street = "Outono do aster",
                            City = "outra city do 2"
                        }
                    }
                },
                new Customer
                {
                    Id = 3,
                    Name = "Rodrigo",
                    Cpf = "865412385",
                    Addresses = new List<Address>()
                    {
                        new Address()
                        {
                            Id = 1,
                            Street = "Primavera do cometa",
                            City = "city do 3"
                        },
                        new Address()
                        {
                            Id = 1,
                            Street = "Primavera do aster",
                            City = "outra city do 3"
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