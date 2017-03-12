using System;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.Services
{
    public interface IItemWorkflowService
    {
        ItemModel GetItem(Guid itemId);

        void Update(ItemModel model);

        bool CanExecuteWorkflowCommand(Guid itemId, string commandId);

        void ExecuteWorkflowCommand(Guid itemId, string commandId);
    }
}