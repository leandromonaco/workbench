using TeamAlignment.Core.Model.Database;
using System.Collections.Generic;
using System.Linq;

namespace TeamAlignment.Core.Common.Extensions
{
    public static class ProductExtensions
    {
        public static List<Team> GetTeams(this Product product) 
        {
            return product.Teams.ToList();
        }
    }
}
