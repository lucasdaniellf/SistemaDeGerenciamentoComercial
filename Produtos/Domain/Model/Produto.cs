﻿using Core.Entity;
using static Produtos.Domain.Model.Status;

namespace Produtos.Domain.Model
{
    public class Produto : IAggregateRoot
    {
        public string Id { get; private set; } = null!;
        public string Descricao { get; private set; } = null!;
        public decimal Preco { get; private set; }
        public Estoque Estoque { get; private set; } = null!;
        public ProdutoStatus EstaAtivo { get; private set; } = ProdutoStatus.ATIVO;

        public void AdicionarACatalogoDeVenda()
        {
            EstaAtivo = ProdutoStatus.ATIVO;
        }

        public void RetirarDeCatalogoDeVenda()
        {
            EstaAtivo = ProdutoStatus.INATIVO;
        }

        public void AtualizarDescricao(string descricao)
        {
            if (string.IsNullOrEmpty(descricao))
            {
                throw new ProdutoException("Descricao inválido, não deve ser vazio");
            }
            Descricao = descricao;
        }

        public void AtualizarPreco(decimal preco)
        {
            if (preco <= 0)
            {
                throw new ProdutoException("Preço inválido, deve ser maior que 0");
            }
            Preco = preco;
        }


        public static Produto CadastrarProduto(string descricao, decimal preco)
        {
            Produto produto = new(descricao, preco)
            {
                Id = Guid.NewGuid().ToString(),
                Estoque = Estoque.CriarEstoque()
            };
            return produto;
        }



        internal Produto(string id, string descricao, decimal preco, long estaAtivo, string estoqueId, int quantidade, int estoqueMinimo, DateTime UltimaAlteracao) : this(descricao, preco)
        {
            Id = id;
            Estoque = new Estoque(estoqueId, quantidade, estoqueMinimo, UltimaAlteracao);
            EstaAtivo = AplicarStatusEmProduto(estaAtivo);
        }

        private Produto(string descricao, decimal preco)
        {
            AtualizarDescricao(descricao);
            AtualizarPreco(preco);
        }
    }
}
