namespace ViaEventsAssociation.Core.Application.AppEntry.Exceptions;

public class ServiceNotFoundException(string message) : SystemException(message);