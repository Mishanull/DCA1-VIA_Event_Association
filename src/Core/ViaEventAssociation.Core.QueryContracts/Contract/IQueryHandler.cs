namespace ViaEventAssociation.Core.QueryContracts.Contract;

public interface IQueryHandler<in TQuery, TAnswer> where TQuery : IQuery<TAnswer>
{
    public Task<TAnswer> HandleAsync(TQuery query);
}