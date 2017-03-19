namespace Sitecore.Hypermedia.Model
{
    public class SimpleWorkflowModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string StateId { get; set; }

        public string StateName { get; set; }

        public bool FinalState { get; set; }
    }
}