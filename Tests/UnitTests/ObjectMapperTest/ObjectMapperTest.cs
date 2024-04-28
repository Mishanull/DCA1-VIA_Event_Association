using Moq;

using ViaEventsAssociation.Core.Tools.ObjectMapper;

namespace UnitTests.ObjectMapperTest;

public class ConcreteObjectMapper : ObjectMapper
{
    public ConcreteObjectMapper(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}

public class ObjectMapperTests
{
    private readonly ConcreteObjectMapper _mapper;
    private readonly Mock<IServiceProvider> _serviceProviderMock;

    public ObjectMapperTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _mapper = new ConcreteObjectMapper(_serviceProviderMock.Object);
    }

    [Fact]
    public void Map_UsesConfiguredMappingConfig_WhenAvailable()
    {
        // Arrange
        var input = new SourceClass { Id = 1, Name = "John Doe" };
        var expectedOutput = new DestinationClass { Id = 1, FullName = "John Doe" };
        var mappingConfigMock = new Mock<IMappingConfig<SourceClass, DestinationClass>>();
        mappingConfigMock.Setup(m => m.Map(input)).Returns(expectedOutput);

        _serviceProviderMock.Setup(sp => sp.GetService(typeof(IMappingConfig<SourceClass, DestinationClass>)))
            .Returns(mappingConfigMock.Object);

        // Act
        var result = _mapper.Map<DestinationClass>(input);

        // Assert
        Assert.Equal(expectedOutput, result);
        Assert.Equal(expectedOutput.FullName, result.FullName);
        Assert.Equal(expectedOutput.Id, result.Id);
        mappingConfigMock.Verify(m => m.Map(input), Times.Once);
    }
}

public class DestinationClass {
    public int Id { get; set; }
    public string FullName { get; set; }

    public override bool Equals(object obj) {
        if (obj is DestinationClass other) {
            return Id == other.Id && FullName == other.FullName;
        }
        return false;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Id, FullName);
    }
}

public class SourceClass {
    public int Id { get; set; }
    public string Name { get; set; }
}

