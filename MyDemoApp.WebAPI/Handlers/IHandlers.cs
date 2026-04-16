namespace MyDemoApp.WebAPI.Handlers
{
    public interface ICommandHandler<TCommand>
    {
        Task Handle(TCommand command);
    }

    public interface IQueryHandler<TQuery, TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}
