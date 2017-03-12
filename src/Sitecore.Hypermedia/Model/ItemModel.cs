using System;

namespace Sitecore.Hypermedia.Model
{
    public class ItemModel : IEquatable<ItemModel>
    {
        public Guid Id { get; set; }

        public string Language { get; set; }

        public int Version { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public WorkflowModel Workflow { get; set; }

        public bool Equals(ItemModel other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Equals(Id, other.Id)
                   && Equals(Language, other.Language)
                   && Equals(Version, other.Version)
                   && Equals(Name, other.Name)
                   && Equals(Title, other.Title)
                   && Equals(Workflow, other.Workflow);
        }

        public override int GetHashCode()
        {
            var hashCode = Id.GetHashCode() ^ 317;
            if (Language != null)
                hashCode ^= Language.GetHashCode();
            if (Version != null)
                hashCode ^= Version.GetHashCode();
            if (Name != null)
                hashCode ^= Name.GetHashCode();
            if (Title != null)
                hashCode ^= Title.GetHashCode();
            if (Workflow != null)
                hashCode ^= Workflow.GetHashCode();

            return hashCode;
        }
    }
}