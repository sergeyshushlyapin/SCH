using System;
using Sitecore.Data;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.Services
{
    public class ItemWorkflowService : IItemWorkflowService
    {
        private readonly Database _database;

        public ItemWorkflowService(Database database)
        {
            _database = database;
        }

        public ItemModel GetItem(Guid itemId)
        {
            var item = _database.GetItem(new ID(itemId));
            if (item == null)
                return null;

            var model = new ItemModel
            {
                Id = item.ID.Guid,
                Name = item.Name,
                Title = item["Title"]
            };

            var workflow = item.State?.GetWorkflow();
            if (workflow != null)
            {
                var state = workflow.GetState(item);
                model.Workflow = new WorkflowModel
                {
                    Id = workflow.WorkflowID,
                    Name = workflow.Appearance.DisplayName,
                    CurrentStateId = state.StateID,
                    CurrentStateName = state.DisplayName,
                    FinalState = state.FinalState
                };
            }

            return model;
        }
    }
}