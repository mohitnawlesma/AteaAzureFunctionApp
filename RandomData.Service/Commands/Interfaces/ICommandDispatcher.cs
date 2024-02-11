using RandomData.Domain.SharedInterfaces;

namespace RandomData.Service.Commands.Interfaces
{
    public interface ICommandDispatcher
    {
        Task<bool> DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
    }
}
