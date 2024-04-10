namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.EntityM_Trial;

public class EntityM
{
    public MId Id { get; }
    public EntityM(MId id) => Id = id;
    private EntityM(){}
}