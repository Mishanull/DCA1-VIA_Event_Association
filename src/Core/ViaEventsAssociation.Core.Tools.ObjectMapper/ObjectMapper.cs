using System.Text.Json;

namespace ViaEventsAssociation.Core.Tools.ObjectMapper;

public abstract class ObjectMapper (IServiceProvider serviceProvider) : IMapper
{
    public TOutput Map<TOutput>(object input) where TOutput : class
    {
        Type type = typeof(IMappingConfig<,>).MakeGenericType(input.GetType(), typeof(TOutput));
        dynamic mappingConfig = serviceProvider.GetService(type)!;
        if (mappingConfig != null)
        {
            return mappingConfig.Map((dynamic)input);
        }

        string toJson = JsonSerializer.Serialize(input);
        return JsonSerializer.Deserialize<TOutput>(toJson)!;
    }
}