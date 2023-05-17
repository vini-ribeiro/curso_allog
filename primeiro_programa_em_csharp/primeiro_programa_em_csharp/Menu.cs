using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;
/// Cadastrar, Editar, Excluir, Listar

namespace primeiro_programa_em_csharp
{
    public class Menu
    {
        private List<Aluno> alunos;
        private int geradorId;

        public Menu()
        {
            alunos = new List<Aluno>();
            geradorId = 0;
        }

        public void inicio()
        {
            imprimeOpcoes();
            int opcao;

            do
            {
                opcao = lerInt("Opcao: ", 1, 6);

                switch (opcao)
                {
                    case 1:
                        cadastrar();
                        break;

                    case 2:
                        editar();
                        break;

                    case 3:
                        excluir();
                        break;

                    case 4:
                        listarTodos();
                        break;

                    case 5:
                        lerArquivo("save.txt");
                        break;
                }

            } while (opcao != 6);
        }
        private void imprimeOpcoes()
        {
            Console.WriteLine("Escolha uma das opcoes abaixo");
            Console.WriteLine("1 - Cadastrar");
            Console.WriteLine("2 - Editar");
            Console.WriteLine("3 - Excluir");
            Console.WriteLine("4 - Listar todos");
            Console.WriteLine("5 - Listar gravados no arquivo");
            Console.WriteLine("6 - Encerrar");
        }

        private int lerInt(string rotulo, int limiteInferior, int limiteSuperior)
        {
            int valor = 0;
            bool valorInserido = false;
            do
            {
                try
                {
                    Console.Write(rotulo);
                    valor = int.Parse(Console.ReadLine().Replace('.', ','));
                    if (valor < limiteInferior || valor > limiteSuperior) throw new Exception();
                    valorInserido = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Valor invalido!");
                }
            } while (!valorInserido);

            return valor;
        }

        private string lerString(string rotulo)
        {
            Console.Write(rotulo);
            return Console.ReadLine();
        }

        private void cadastrar()
        {
            Console.WriteLine("Cadastrando um aluno");
            Console.WriteLine("Preencha os dados abaixo");
            string nome = lerString("Nome: ");
            string endereco = lerString("Endereco: ");
            string telefone = lerString("Telefone: ");
            string email = lerString("Email: ");

            Aluno aluno = new Aluno(++geradorId, nome, endereco, telefone, email);
            alunos.Add(aluno);
            escreverNoArquivo("save.txt");
        }

        private Aluno localizarAluno(int id)
        {
            foreach (Aluno aluno in alunos)
            {
                if (aluno.Id == id) return aluno;
            }

            return null;
        }

        private void editar()
        {
            int id = lerInt("Id do aluno: ", 1, int.MaxValue);

            Aluno aluno = localizarAluno(id);

            Console.WriteLine("1 - nome");
            Console.WriteLine("2 - endereco");
            Console.WriteLine("3 - telefone");
            Console.WriteLine("4 - email");
            int opcao = lerInt("Opcao: ", 1, 4);

            switch (opcao)
            {
                case 1:
                    string novoNome = lerString("Digite o novo nome: ");
                    aluno.Nome = novoNome;
                    break;

                case 2:
                    string novoEndereco = lerString("Digite o novo endereco: ");
                    aluno.Endereco = novoEndereco;
                    break;

                case 3:
                    string novoTelefone = lerString("Digite o novo telefone: ");
                    aluno.Telefone = novoTelefone;
                    break;

                case 4:
                    string novoEmail = lerString("Digite o novo email: ");
                    aluno.Email = novoEmail;
                    break;
            }

            escreverNoArquivo("save.txt");
        }
        private void excluir()
        {
            int id = lerInt("Numero da conta: ", 1, int.MaxValue);
            Aluno aluno = localizarAluno(id);
            if (aluno != null) alunos.Remove(aluno);
        }
        private void listarTodos()
        {
            Console.WriteLine("id | nome | endereco | telefone | email");
            foreach (Aluno aluno in alunos)
            {
                Console.WriteLine(aluno.toString());
            }
        }

        private bool escreverNoArquivo(string caminhoArquivo)
        {
            StreamWriter sw = new StreamWriter(caminhoArquivo, true);
            if (sw == null) return false;

            foreach (Aluno aluno in alunos)
            {
                sw.WriteLine(aluno.toString());
            }
            sw.Close();

            return true;
        }

        private bool lerArquivo(string caminhoArquivo)
        {
            StreamReader sr = new StreamReader(caminhoArquivo);
            if (sr == null) return false;

            Console.WriteLine(sr.ReadToEnd());

            sr.Close();

            return true;
        }

        
    }

}