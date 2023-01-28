using Clientes.Domain.Infrastructure;
using Clientes.Domain.Model;
using Clientes.Query.DTO;
using Core.Infrastructure;

namespace Clientes.Query
{
    public class ClienteQueryService
    {
        private readonly IClienteRepository _repository;
        private readonly IUnitOfWork<Cliente> _unitOfWork;
        public ClienteQueryService(IClienteRepository repository, IUnitOfWork<Cliente> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<IEnumerable<ClienteQueryDto>> BuscarClientes(CancellationToken token)
        {
            _unitOfWork.Begin();
            return from c in await _repository.BuscarClientes(token) select MapQueryDto(c);
        }
        public async Task<IEnumerable<ClienteQueryDto>> BuscarClientePorNome(string nome, CancellationToken token)
        {
            _unitOfWork.Begin();
            return from c in await _repository.BuscarClientePorNome(nome, token) select MapQueryDto(c);
        }
        public async Task<IEnumerable<ClienteQueryDto>> BuscarClientePorCPF(string cpf, CancellationToken token)
        {
            _unitOfWork.Begin();
            return from c in await _repository.BuscarClientePorCPF(cpf, token) select MapQueryDto(c);

        }
        public async Task<IEnumerable<ClienteQueryDto>> BuscarClientePorId(string Id, CancellationToken token)
        {
            _unitOfWork.Begin();
            return from c in await _repository.BuscarClientePorId(Id, token) select MapQueryDto(c);

        }

        private ClienteQueryDto MapQueryDto(Cliente cliente)
        {
            return new ClienteQueryDto(cliente.Id, cliente.Cpf.Numero, cliente.Nome, cliente.EstaAtivo);
        }
    }
}
