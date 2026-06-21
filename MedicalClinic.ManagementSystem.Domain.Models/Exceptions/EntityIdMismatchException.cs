namespace MedicalClinic.ManagementSystem.Domain.Models.Exceptions;

public sealed class EntityIdMismatchException : BadRequestException
{
    public EntityIdMismatchException(string entityName, Guid routeId, Guid bodyId)
        : base($"{entityName} id mismatch. Route id '{routeId}' does not match body id '{bodyId}'.")
    {
    }
}
