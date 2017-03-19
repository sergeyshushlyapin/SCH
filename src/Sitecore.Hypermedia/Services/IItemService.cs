using System;
using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Sitecore.Hypermedia.Services
{
    public interface IItemService
    {
        IEnumerable<Item> GetContentItems();

        Item GetItem(Guid itemId);
    }
}