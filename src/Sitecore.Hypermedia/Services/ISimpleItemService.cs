using System;
using System.Collections.Generic;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.Services
{
    public interface ISimpleItemService
    {
        IEnumerable<SimpleItemModel> GetContentItems();

        SimpleItemModel GetItem(Guid itemId);

        void Update(SimpleItemModel model);

        bool CanExecuteWorkflowCommand(Guid itemId, string commandId);

        void ExecuteWorkflowCommand(Guid itemId, string commandId);
    }
}