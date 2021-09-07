using System.Collections.Generic;
using TeamAlignment.Core.Model.Database;

namespace TeamAlignment.Core.Model.Comparers
{
    public class TeamMemberComparer : IEqualityComparer<Employee>
    {
        public bool Equals(Employee x, Employee y)
        {
            if (x.Id.Equals(y.Id))
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(Employee obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}