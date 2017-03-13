﻿using System.Collections.Generic;

namespace Sitecore.Hypermedia.Model
{
    public class WorkflowModel
    {
        public string Name { get; set; }

        public ICollection<LinkModel> Links { get; set; }

        public ICollection<WorkflowStateModel> States { get; set; }
    }
}