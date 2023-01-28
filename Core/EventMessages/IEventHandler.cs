namespace Core.EventMessages
{
    public interface IEventHandler<in Request> where Request : EventRequest
    {
        public Task Handle(Request request, CancellationToken token);
    }
}
