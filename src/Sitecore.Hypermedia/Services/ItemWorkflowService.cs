using System;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Hypermedia.Services
{
    public class ItemWorkflowService : IItemWorkflowService
    {
        private readonly Database _database;

        public ItemWorkflowService(Database database)
        {
            _database = database;
        }

        public Item GetItem(Guid itemId)
        {
            return _database.GetItem(new ID(itemId));
        }
    }
}