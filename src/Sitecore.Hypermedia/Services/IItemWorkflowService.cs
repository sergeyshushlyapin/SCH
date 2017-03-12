using System;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.Services
{
    public interface IItemWorkflowService
    {
        ItemModel GetItem(Guid itemId);
    }
}