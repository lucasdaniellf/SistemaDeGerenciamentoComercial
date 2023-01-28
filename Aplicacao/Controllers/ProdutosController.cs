﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Produtos.Domain;
using Produtos.Domain.Commands;
using Produtos.Domain.Commands.ProdutoEstoque;
using Produtos.Query;
using Produtos.Query.DTO;

namespace AplicacaoGerenciamentoLoja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoCommandHandler _handler;
        private readonly ProdutoQueryService _service;

        public ProdutosController(ProdutoQueryService service, ProdutoCommandHandler handler)
        {
            _service = service;
            _handler = handler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoQueryDto>>> BuscarProdutos(string? descricao, CancellationToken token)
        {
            IEnumerable<ProdutoQueryDto> Produtos;
            if (string.IsNullOrEmpty(descricao))
            {
                Produtos = await _service.BuscarProdutos(token);
            }
            else
            {
                Produtos = await _service.BuscarProdutoPorDescricao(descricao, token);
            }
            return Ok(Produtos);
        }

        [HttpGet("{Id}", Name = "BuscarProdutoPorId")]
        public async Task<ActionResult<IEnumerable<ProdutoQueryDto>>> BuscarProdutoPorId(string Id, CancellationToken token)
        {
            IEnumerable<ProdutoQueryDto> Produtos = await _service.BuscarProdutoPorId(Id, token);
            if (Produtos.Any())
            {
                return Ok(Produtos);
            }

            return NotFound();
        }

        //Check this
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ProdutoQueryDto>>> CadastrarProduto(CadastrarProdutoCommand command, CancellationToken token)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _handler.Handle(command, token);
                    if (success)
                    {
                        var produto = await _service.BuscarProdutoPorId(command.Id, token);
                        return CreatedAtAction(nameof(BuscarProdutoPorId), new { produto.First().Id }, produto.First());
                    }
                }
                catch (ProdutoException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult> AtualizarCadastroProduto(AtualizarCadastroProdutoCommand command, CancellationToken token)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool success = await _handler.Handle(command, token);
                    if (!success)
                    {
                        return NotFound();
                    }
                    return NoContent();
                }
                catch (ProdutoException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpPut("catalogo/adicionar")]
        public async Task<ActionResult> AdicionarProdutoACatalogo(AdicionarProdutoEmCatalogoCommand command, CancellationToken token)
        {
            try
            {
                bool success = await _handler.Handle(command, token);
                if (success)
                {
                    return NoContent();
                }
            }
            catch (ProdutoException ex)
            {
                return BadRequest(ex.Message);
            }
            return NotFound();
        }

        [HttpPut("catalogo/retirar")]
        public async Task<ActionResult> RetirarProdutoACatalogo(RetirarProdutoDeCatalogoCommand command, CancellationToken token)
        {
            try
            {
                bool success = await _handler.Handle(command, token);
                if (success)
                {
                    return NoContent();
                }
            }
            catch (ProdutoException ex)
            {
                return BadRequest(ex.Message);
            }
            return NotFound();
        }

        [HttpPut("estoque/repor")]
        public async Task<ActionResult> ReporEstoqueProduto(ReporEstoqueProdutoCommand command, CancellationToken token)
        {
            try
            {
                bool success = await _handler.Handle(command, token);
                if (success)
                {
                    return NoContent();
                }
            }
            catch (ProdutoException ex)
            {
                return BadRequest(ex.Message);
            }
            return NotFound();
        }


        [HttpPut("estoque/baixar")]
        public async Task<ActionResult> BaixarEstoqueProduto(BaixarEstoqueProdutoCommand command, CancellationToken token)
        {
            try
            {
                bool success = await _handler.Handle(command, token);
                if (success)
                {
                    return NoContent();
                }
            }
            catch (ProdutoException ex)
            {
                return BadRequest(ex.Message);
            }
            return NotFound();
        }
    }
}
