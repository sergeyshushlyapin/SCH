using System;

namespace Sitecore.Hypermedia.Model
{
    public class ItemWorkflowModel : IEquatable<ItemWorkflowModel>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string StateId { get; set; }

        public string StateName { get; set; }

        public bool FinalState { get; set; }

        public bool Equals(ItemWorkflowModel other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Equals(Id, other.Id)
                   && Equals(Name, other.Name)
                   && Equals(StateId, other.StateId)
                   && Equals(StateName, other.StateName)
                   && Equals(FinalState, other.FinalState);
        }

        public override int GetHashCode()
        {
            var hashCode = Id.GetHashCode() ^ FinalState.GetHashCode() ^ 317;
            if (Name != null)
                hashCode ^= Name.GetHashCode();

            if (StateId != null)
                hashCode ^= StateId.GetHashCode();

            if (StateName != null)
                hashCode ^= StateName.GetHashCode();


            return hashCode;
        }
    }
}