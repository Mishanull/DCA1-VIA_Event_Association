namespace ViaEventsAssociation.Core.Tools.ObjectMapper;

public interface IMapper
{
    TOutput Map<TOutput>(object input) where TOutput : class;
}