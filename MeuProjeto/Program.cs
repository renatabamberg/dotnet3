using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaEstoque
{
    class Program
    {
        public record Produto(int Codigo, string Nome, int Quantidade, double PrecoUnitario);

        static List<Produto> estoque = new List<Produto>();

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Bem-vindo ao Sistema de Gerenciamento de Estoque!");

                // Cadastro de produtos
                CadastrarProduto(1, "Produto A", 50, 10.0);
                CadastrarProduto(2, "Produto B", 30, 15.0);

                // Consulta de produtos
                ConsultarProduto(1);

                // Atualização de estoque
                AtualizarEstoque(1, 20, TipoMovimentacao.Entrada);
                AtualizarEstoque(2, 40, TipoMovimentacao.Saida);

                // Relatórios
                GerarRelatorioEstoqueAbaixoLimite(40);
                GerarRelatorioValorEntreLimites(5, 20);
                GerarRelatorioValorTotalEstoque();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro: " + ex.Message);
            }
        }

        public enum TipoMovimentacao
        {
            Entrada,
            Saida
        }

        // Cadastro de produtos
        static void CadastrarProduto(int codigo, string nome, int quantidade, double preco)
        {
            if (estoque.Any(p => p.Codigo == codigo))
            {
                throw new Exception("Produto já cadastrado.");
            }

            Produto novoProduto = new Produto(codigo, nome, quantidade, preco);
            estoque.Add(novoProduto);
            Console.WriteLine("Produto cadastrado com sucesso!");
        }

        // Consulta de produtos
        static void ConsultarProduto(int codigo)
        {
            Produto produto = estoque.FirstOrDefault(p => p.Codigo == codigo);
            if (produto == null)
            {
                throw new Exception("Produto não encontrado.");
            }

            Console.WriteLine($"Produto encontrado: {produto.Nome}, Quantidade: {produto.Quantidade}, Preço: {produto.PrecoUnitario}");
        }

        // Atualização de estoque
        static void AtualizarEstoque(int codigo, int quantidade, TipoMovimentacao tipoMovimentacao)
        {
            Produto produto = estoque.FirstOrDefault(p => p.Codigo == codigo);
            if (produto == null)
            {
                throw new Exception("Produto não encontrado para atualização de estoque.");
            }

            if (tipoMovimentacao == TipoMovimentacao.Entrada)
            {
                produto = produto with { Quantidade = produto.Quantidade + quantidade };
                Console.WriteLine("Estoque atualizado com sucesso (entrada)!");
            }
            else if (tipoMovimentacao == TipoMovimentacao.Saida)
            {
                if (produto.Quantidade < quantidade)
                {
                    throw new Exception("Quantidade insuficiente em estoque para a saída.");
                }

                produto = produto with { Quantidade = produto.Quantidade - quantidade };
                Console.WriteLine("Estoque atualizado com sucesso (saída)!");
            }

            estoque[estoque.FindIndex(p => p.Codigo == codigo)] = produto;
        }

        // Relatórios
        static void GerarRelatorioEstoqueAbaixoLimite(int limite)
        {
            var produtosAbaixoLimite = estoque.Where(p => p.Quantidade < limite);
            Console.WriteLine("\nProdutos com quantidade em estoque abaixo do limite:");
            foreach (var produto in produtosAbaixoLimite)
            {
                Console.WriteLine($"Nome: {produto.Nome}, Quantidade: {produto.Quantidade}");
            }
        }

        static void GerarRelatorioValorEntreLimites(double minimo, double maximo)
        {
            var produtosEntreLimites = estoque.Where(p => p.PrecoUnitario >= minimo && p.PrecoUnitario <= maximo);
            Console.WriteLine("\nProdutos com valor entre os limites informados:");
            foreach (var produto in produtosEntreLimites)
            {
                Console.WriteLine($"Nome: {produto.Nome}, Preço: {produto.PrecoUnitario}");
            }
        }

        static void GerarRelatorioValorTotalEstoque()
        {
            double valorTotalEstoque = estoque.Sum(p => p.Quantidade * p.PrecoUnitario);
            Console.WriteLine($"\nValor total do estoque: {valorTotalEstoque}");

            Console.WriteLine("\nValor total de cada produto de acordo com seu estoque:");
            foreach (var produto in estoque)
            {
                double valorProduto = produto.Quantidade * produto.PrecoUnitario;
                Console.WriteLine($"Nome: {produto.Nome}, Valor Total: {valorProduto}");
            }
        }
    }
}
