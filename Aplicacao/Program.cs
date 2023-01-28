using AplicacaoGerenciamentoLoja.HostedServices.Consumers.Cliente;
using AplicacaoGerenciamentoLoja.HostedServices.Consumers.Produto;
using AplicacaoGerenciamentoLoja.HostedServices.Consumers.Venda;
using AplicacaoGerenciamentoLoja.Middlewares;
using Clientes.Domain;
using Clientes.Domain.Commands;
using Clientes.Domain.Infrastructure;
using Clientes.Domain.Model;
using Clientes.Infrastructure;
using Clientes.Query;
using Core.Infrastructure;
using Core.MessageBroker;
using Microsoft.AspNetCore.Connections;
using Produtos.Domain;
using Produtos.Domain.Commands;
using Produtos.Domain.Model;
using Produtos.Domain.Repository;
using Produtos.Infrastructure;
using Produtos.Query;
using StackExchange.Redis;
using Vendas.Domain;
using Vendas.Domain.Commands;
using Vendas.Domain.Events;
using Vendas.Domain.Model;
using Vendas.Domain.Repository;
using Vendas.Infrastructure;
using Vendas.Query;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//Clientes
builder.Services.AddScoped<IDbContext<Cliente>, ClienteDbContext>( _ => new ClienteDbContext(builder.Configuration.GetConnectionString("ClientesConnectionString")));
builder.Services.AddScoped<IDbContext<Produto>, ProdutoDbContext>(_ => new ProdutoDbContext(builder.Configuration.GetConnectionString("ProdutosConnectionString")));
builder.Services.AddScoped<IDbContext<Venda>, VendaDbContext>(_ => new VendaDbContext(builder.Configuration.GetConnectionString("VendasConnectionString")));

builder.Services.AddScoped<ClienteCommandHandler>();
builder.Services.AddScoped<ClienteQueryService>();
builder.Services.Configure<ClienteDomainSettings>(builder.Configuration.GetSection("Queues").GetSection("ClienteDomainSettings"));

builder.Services.AddScoped<ProdutoCommandHandler>();
builder.Services.AddScoped<ProdutoQueryService>();
builder.Services.Configure<ProdutoDomainSettings>(builder.Configuration.GetSection("Queues").GetSection("ProdutoDomainSettings"));

builder.Services.AddScoped<VendaCommandHandler>();
builder.Services.AddScoped<VendaQueryService>();
builder.Services.AddScoped<VendaEventHandler>();
builder.Services.Configure<VendaDomainSettings>(builder.Configuration.GetSection("Queues").GetSection("VendaDomainSettings"));

builder.Services.AddScoped<IUnitOfWork<Cliente>, UnitOfWork<Cliente>>();
builder.Services.AddScoped<IUnitOfWork<Produto>, UnitOfWork<Produto>>();
builder.Services.AddScoped<IUnitOfWork<Venda>, UnitOfWork<Venda>>();


builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();


builder.Services.AddScoped<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(builder.Configuration.GetSection("Redis")["RedisConnection"]));
builder.Services.AddScoped<IMessageBrokerPublisher, RedisPublisher>();
builder.Services.AddScoped<IMessageBrokerSubscriber, RedisSubscriber>();


builder.Services.AddHostedService<ClienteAtualizadoConsumer>();
builder.Services.AddHostedService<ProdutoAtualizadoConsumer>();
builder.Services.AddHostedService<VendaAprovadaConsumer>();
builder.Services.AddHostedService<VendaConfirmadaConsumer>();
builder.Services.AddHostedService<VendaReprovadaConsumer>();
builder.Services.AddHostedService<VendaCanceladaConsumer>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Configuração necessária devido ao enum Status estar presente em 3 projetos distintos, causando conflito no Swagger
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.useExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
