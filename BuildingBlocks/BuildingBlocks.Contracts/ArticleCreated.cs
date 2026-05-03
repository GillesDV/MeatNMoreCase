using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Contracts
{
    public record ArticleCreated(
        Guid ArticleId,
        string Description,
        string Unit
    );
}
