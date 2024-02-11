using Microsoft.Extensions.DependencyInjection;
using RandomData.Domain.SharedInterfaces;
using RandomData.Service.Commands.Interfaces;

namespace RandomData.Service.Commands;

internal sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task<bool> DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        return await handler.HandleAsync(command);
    }
}

