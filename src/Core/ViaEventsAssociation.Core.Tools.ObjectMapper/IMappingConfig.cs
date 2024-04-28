namespace ViaEventsAssociation.Core.Tools.ObjectMapper;

public interface IMappingConfig<TInput, TOutput>
    where TOutput : class
    where TInput : class
{
    public TOutput Map(TInput input);
}