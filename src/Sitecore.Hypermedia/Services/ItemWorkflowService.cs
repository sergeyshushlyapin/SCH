using System;
using System.Linq;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Hypermedia.Model;
using Sitecore.SecurityModel;

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
                Language = item.Language.Name,
                Version = item.Version.Number,
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
                    StateId = state.StateID,
                    StateName = state.DisplayName,
                    FinalState = state.FinalState
                };
            }

            return model;
        }

        public void Update(ItemModel model)
        {
            // TODO: Remove SecurityDisabler
            using (new SecurityDisabler())
            {
                var item = this._database.GetItem(new ID(model.Id));
                Assert.IsNotNull(item, $"Item {model.Id} not found.");

                using (new EditContext(item))
                {
                    item["Title"] = model.Title;
                }
            }
        }

        public bool CanExecuteWorkflowCommand(
            Guid itemId, string commandId)
        {
            // TODO: Remove SecurityDisabler
            using (new SecurityDisabler())
            {
                var item = _database.GetItem(new ID(itemId));
                var workflow = item?.State?.GetWorkflow();
                if (workflow == null)
                    return false;

                return workflow.GetCommands(item)
                    .Any(c => c.CommandID.Equals(
                        commandId, StringComparison.OrdinalIgnoreCase));
            }
        }

        public void ExecuteWorkflowCommand(Guid itemId, string commandId)
        {
            var item = _database.GetItem(new ID(itemId));
            var workflow = item.State.GetWorkflow();
            workflow.Execute(
                commandId, item, new StringDictionary(), false);
        }
    }
}