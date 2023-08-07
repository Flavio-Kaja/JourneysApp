using JourneyService.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace JourneyService.NotificationHandlers
{
    /// <summary>
    /// When a new domain event is publiched, clear all journey data
    /// </summary>
    /// <seealso cref="MediatR.INotificationHandler&lt;JourneyService.Domain.DomainEvent&gt;" />
    public class ClearJourneyCacheHandler : INotificationHandler<DomainEvent>
    {
        private readonly IMemoryCache _cache;

        public ClearJourneyCacheHandler(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task Handle(DomainEvent notification, CancellationToken cancellationToken)
        {
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_cache) as dynamic;

            List<object> keysToRemove = new();

            foreach (var cacheItem in cacheEntriesCollection)
            {
                var cacheItemValue = ((ICacheEntry)cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null));
                if (cacheItemValue.Key.ToString().StartsWith("journey"))
                {
                    keysToRemove.Add(cacheItemValue.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }

            return Task.CompletedTask;
        }
    }

}
