namespace ViaEventAssociation.Core.Domain.Contracts;

public interface IEmailCheck
{
    public bool DoesEmailExist(string email);
}