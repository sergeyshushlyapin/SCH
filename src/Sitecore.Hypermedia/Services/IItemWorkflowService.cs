using System;
using Sitecore.Data.Items;

namespace Sitecore.Hypermedia.Services
{
    public interface IItemWorkflowService
    {
        Item GetItem(Guid itemId);
    }
}