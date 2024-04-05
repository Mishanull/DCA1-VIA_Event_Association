using ViaEventAssociation.Core.Domain.Contracts;

namespace UnitTests.FakeServices;

public class FakeEmailCheck : IEmailCheck
{
    public bool DoesEmailExist(string email)
    {
        return false;
    }
}