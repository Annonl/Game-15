using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Игра_в_15
{
    public class ListComparer : IEqualityComparer<List<int>>
    {
        public bool Equals(List<int> x, List<int> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if ((x == null) || (y == null))
            {
                return false;
            }

            return x.SequenceEqual(y);
        }

        public int GetHashCode(List<int> obj)
        {
            return obj.Aggregate(17, (res, item) => unchecked(res * 23 + item.GetHashCode()));
        }
    }
}
