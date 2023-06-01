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
                    Cpf = "00891528246545"
                },
                new Customer
                {
                    Id = 2,
                    Name = "Jundiado",
                    Cpf = "00791528237"
                },
                new Customer
                {
                    Id = 3,
                    Name = "Rodrigo",
                    Cpf = "865412385"
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