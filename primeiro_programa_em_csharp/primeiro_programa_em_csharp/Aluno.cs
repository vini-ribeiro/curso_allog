using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace primeiro_programa_em_csharp
{
    internal class Aluno
    {
        private int id;
        private string nome;
        private string endereco;
        private string telefone;
        private string email;

        public Aluno()
        {
            this.id = 0;
            this.nome = "";
            this.endereco = "";
            this.telefone = "";
            this.email = "";
        }

        public Aluno(int id, string nome, string endereco, string telefone, string email)
        {
            this.id = id;
            this.nome = nome;
            this.endereco = endereco;
            this.telefone = telefone;
            this.email = email;
        }

        public string toString()
        {
            return id + " | " + nome + " | " + endereco + " | " + telefone + " | " + email;
        }

        public int Id { get => id; }
        public string Nome { get => nome; set => nome = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string Telefone { get => telefone; set => telefone = value; }
        public string Email { get => email; set => email = value; }
    }
}
