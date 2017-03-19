using System;

namespace Sitecore.Hypermedia.Model
{
    public class SimpleItemModel
    {
        public Guid Id { get; set; }

        public string Language { get; set; }

        public int Version { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public SimpleWorkflowModel Workflow { get; set; }
    }
}