using System.Data.SqlClient;

namespace AdministracaoDeLoja
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //menu de opções
            Console.WriteLine("Administração da Loja do Ricardo\n");
            Console.WriteLine("********************************************************\n");
            Console.WriteLine("********************************************************\n");
            Console.WriteLine("Digite uma opção do Menu:\n\n");
            Console.WriteLine("1 - Cadastrar Produto\n");
            Console.WriteLine("2 - Vender Produto\n");
            Console.WriteLine("3 - Listar Produtos em Estoque\n");
            Console.WriteLine("4 - Deletar Produto\n");
            Console.WriteLine("5 - Encerrar Programa\n");

            //while loop para ficar repetindo o submenu de opçoes ate ser escolhida a opção 5 (saida)
            while (true)
            {
                Console.WriteLine("Escolha uma opção (1-5):");
                //int parse da variavel menu, pois readline le string
                int menu = int.Parse(Console.ReadLine());

                //switch case com as opções
                switch (menu)
                {

                    case 1:
                        {
                            // cadastra produto
                            InserirProduto();
                            break;
                        }

                    case 2:
                        {
                            //vende o produto, nao calcula o preço a pagar ainda
                            VenderProduto();
                            break;
                        }


                    case 3:
                        {
                            // lista os produtos cadastrados
                            MostraProdutos();
                            break;
                        }


                    case 4:
                        {
                            //deletar o produto
                            DeletarProduto();
                            break;
                        }

                    case 5:
                        {
                            //retorna e sai do while loop
                            Console.WriteLine("Saindo da Aplicação");
                            return;
                        }

                    default:
                        {
                            //default case, caso escolha algo invalido
                            Console.WriteLine("Escolha inválida, tente novamente.");
                            break;
                        }
                }
            }

            //metodo inserir
            static void InserirProduto()
            {

                Console.WriteLine("Digite o NOME do produto que está cadastrando:");
                string produtoNome = Console.ReadLine();

                Console.WriteLine("Digite a QUANTIDADE do produto que está cadastrando:");
                int produtoQuantidade = int.Parse(Console.ReadLine());

                Console.WriteLine("Digite o PREÇO do produto que está cadastrando:");
                double produtoPreco = double.Parse(Console.ReadLine());

                Console.WriteLine("Digite o CÓDIGO ID do produto que está cadastrando:");
                int produtoID = int.Parse(Console.ReadLine());

                string query = "INSERT INTO Produtos (Nome, Quantidade, Preco, IDProduto) VALUES (@produtoNome, @produtoQuantidade, @produtoPreco, @produtoID)";

                try
                {
                    using SqlConnection cn = new SqlConnection(Conn.StrCon);
                    SqlCommand command = new SqlCommand(query, cn);

                    command.Parameters.AddWithValue("@produtoNome", produtoNome);
                    command.Parameters.AddWithValue("@produtoQuantidade", produtoQuantidade);
                    command.Parameters.AddWithValue("@produtoPreco", produtoPreco);
                    command.Parameters.AddWithValue("@produtoID", produtoID);

                    cn.Open();
                    int linhasAlteradas = command.ExecuteNonQuery();
                    if (linhasAlteradas > 0)
                    {
                        Console.WriteLine("Produto inserido com sucesso.");
                    }
                    else
                    {
                        Console.WriteLine("Falha na inserção do produto.");
                    }
                    cn.Close();
                }

                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Erro ao conectar DB:\n\n " + sqlEx.Message);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Falha ao conectar\n\n" + ex.Message);
                }
            }

            //metodo vender
            static void VenderProduto()
            {
                //deve ser digitado literalmente da forma que foi inserido
                Console.WriteLine("Digite o CODIGO ID do produto que está vendendo:");
                int produtoID = int.Parse(Console.ReadLine());

                Console.WriteLine("Digite a QUANTIDADE do produto que está vendendo:");
                int produtoQuantidade = int.Parse(Console.ReadLine());

                //query p atualizar a quantidade
                string query = "UPDATE Produtos SET quantidade = quantidade - @quantVendida WHERE IDProduto = @produtoID";

                try
                {
                    //cria conexao ao DB
                    using (SqlConnection cn = new SqlConnection(Conn.StrCon))
                    {
                        // Cria comando para fazer a query
                        SqlCommand command = new SqlCommand(query, cn);
                        command.Parameters.AddWithValue("@quantVendida", produtoQuantidade);
                        command.Parameters.AddWithValue("@produtoID", produtoID);

                        cn.Open();

                        //executa query e ve numero de linhas afetadas
                        int linhasAlteradas = command.ExecuteNonQuery();

                        // verifica se a query funcionou
                        if (linhasAlteradas > 0)
                        {
                            Console.WriteLine("Produto vendido!");
                        }
                        else
                        {
                            Console.WriteLine("A venda do Produto micou.");
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Erro ao conectar DB:\n\n " + sqlEx.Message);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Falha ao conectar\n\n" + ex.Message);
                }
            }

        }

        //metodo mostrar
        static void MostraProdutos()
        {

            Console.WriteLine("Lista de produtos disponíveis no momento:");
            string query = "SELECT * FROM produtos ORDER BY IDProduto ASC";

            try
            {
                using SqlConnection cn = new SqlConnection(Conn.StrCon);
                SqlCommand command = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("Quantidade\tPreço\t\tIDProduto\tNome");
                    Console.WriteLine("----------\t-----\t\t---------\t----");

                    // mostra o conteudo do DB
                    while (reader.Read())
                    {

                        Console.WriteLine(

                            reader.GetString(1) + "\t\t" +
                            reader.GetString(2) + "\t\t" +
                            reader.GetString(3) + "\t\t" +
                            reader.GetString(0)

                        );
                    }
                }
                else
                {
                    Console.WriteLine("Nenhum produto cadastrado.");
                }

                reader.Close();

            }

            catch (SqlException sqlEx)
            {
                Console.WriteLine("Erro ao conectar DB:\n\n " + sqlEx.Message);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Falha ao conectar\n\n" + ex.Message);
            }

        }

        //metodo deletar
        static void DeletarProduto()
        {
            Console.Write("Qual a ID do Produto que quer Deletar? ");
            int produtoID = int.Parse(Console.ReadLine());

            // SQL query que pega o ID e deleta o produto
            string query = "DELETE FROM Produtos WHERE IDProduto = @produtoID";

            try
            {
                // conecta ao DB
                using (SqlConnection cn = new SqlConnection(Conn.StrCon))
                {
                    // comando p executar query
                    SqlCommand comando = new SqlCommand(query, cn);

                    // adiciona o parametro a query
                    comando.Parameters.AddWithValue("@produtoID", produtoID);


                    cn.Open();

                    // executar a query
                    int rowsAffected = comando.ExecuteNonQuery();

                    // conferir se funcionou o deletar
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Produto removido.");
                    }
                    else
                    {
                        Console.WriteLine("Produto não encontrado.");
                    }
                }
            }

            catch (SqlException sqlEx)
            {
                Console.WriteLine("Erro ao conectar DB:\n\n " + sqlEx.Message);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Falha ao conectar\n\n" + ex.Message);
            }
        }
    }
}


