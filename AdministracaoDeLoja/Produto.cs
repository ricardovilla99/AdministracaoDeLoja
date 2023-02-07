using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministracaoDeLoja
{
    internal class Produto
    {


        public string Nome { get; set; }
        public string Quantidade { get; set; }
        public string Preco { get; set; }
        public string IDProduto { get; set; }

        // Construtor
        public Produto(string Nome, string Quantidade, string Preco, string IDProduto)
        {
            Nome = Nome;
            Quantidade = Quantidade;
            Preco = Preco;
            IDProduto = IDProduto;
        }
    }


}

