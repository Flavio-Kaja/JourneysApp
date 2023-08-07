using FluentValidation;
using JourneyService.Domain.TransportationTypes.DomainEvents;
using JourneyService.Domain.TransportationTypes.Dtos;
using Sieve.Attributes;

namespace JourneyService.Domain.TransportationTypes;

/// <summary>
/// The transportation type entity
/// </summary>
/// <seealso cref="JourneyService.Domain.BaseEntity" />
public class TransportationType : BaseEntity
{

    /// <summary>Gets the type.</summary>
    /// <value>The type.</value>
    public string Type { get; private set; }


    /// <summary>Create a transportation type</summary>
    /// <param name="transportationTypeForCreation">The transportation type for creation.</param>
    public static TransportationType Create(PostTransportationTypeDto transportationTypeForCreation)
    {
        TransportationType newTransportationType = new()
        {
            Type = transportationTypeForCreation.Type
        };
        newTransportationType.QueueDomainEvent(new TransportationTypeCreated() { TransportationType = newTransportationType });

        return newTransportationType;
    }

    /// <summary>Updates the specified transportation </summary>
    /// <param name="transportationTypeForUpdate">The transportation type update data.</param>
    public TransportationType Update(PostTransportationTypeDto transportationTypeForUpdate)
    {
        Type = transportationTypeForUpdate.Type;

        QueueDomainEvent(new TransportationTypeUpdated() { Id = Id });
        return this;
    }

    protected TransportationType() { } // For EF + Mocking
}