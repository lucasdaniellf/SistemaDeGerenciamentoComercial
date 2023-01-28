namespace Core.Commands
{
    public interface ICommandHandler<in TRequest, TResponse> where TRequest : ICommandRequest
    {
        public Task<TResponse> Handle(TRequest command, CancellationToken token);
    }
}
