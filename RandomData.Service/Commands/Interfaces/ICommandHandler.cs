using RandomData.Domain.SharedInterfaces;

namespace RandomData.Service.Commands.Interfaces
{
    public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
    {
        Task<bool> HandleAsync(TCommand command);
    }

}
