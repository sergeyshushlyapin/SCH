using System;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Hypermedia.Services
{
    public class ItemService : IItemService
    {
        private readonly Database _database;

        public ItemService(Database database)
        {
            _database = database;
        }

        public IEnumerable<Item> GetContentItems()
        {
            return _database
                .GetItem(ItemIDs.ContentRoot)
                .Axes
                .GetDescendants();
        }

        public Item GetItem(Guid itemId)
        {
            return _database.GetItem(new ID(itemId));
        }
    }
}