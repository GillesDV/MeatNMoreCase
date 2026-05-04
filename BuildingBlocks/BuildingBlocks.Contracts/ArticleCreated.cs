using NServiceBus;

namespace BuildingBlocks.Contracts
{
    public record ArticleCreated(
        int ArticleId
    ) : IEvent;
}
