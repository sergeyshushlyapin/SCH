using System;

namespace Sitecore.Hypermedia.Model
{
    public class WorkflowModel : IEquatable<WorkflowModel>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CurrentStateId { get; set; }

        public string CurrentStateName { get; set; }

        public bool FinalState { get; set; }

        public bool Equals(WorkflowModel other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Equals(Id, other.Id)
                   && Equals(Name, other.Name)
                   && Equals(CurrentStateId, other.CurrentStateId)
                   && Equals(CurrentStateName, other.CurrentStateName)
                   && Equals(FinalState, other.FinalState);
        }

        public override int GetHashCode()
        {
            var hashCode = Id.GetHashCode() ^ FinalState.GetHashCode() ^ 317;
            if (Name != null)
                hashCode ^= Name.GetHashCode();

            if (CurrentStateId != null)
                hashCode ^= CurrentStateId.GetHashCode();

            if (CurrentStateName != null)
                hashCode ^= CurrentStateName.GetHashCode();


            return hashCode;
        }
    }
}