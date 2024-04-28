namespace ObjectMapper;

public interface IMapper
{
    TOutput Map<TOutput>(object input) where TOutput : class;
}