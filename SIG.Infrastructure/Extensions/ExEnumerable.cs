using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIG.Infrastructure.Extensions
{
    public static class ExEnumerable
    {
        public static IEnumerable<T> SelectDeep<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (T item in source)
            {
                yield return item;
                foreach (T subItem in SelectDeep(selector(item), selector))
                {
                    yield return subItem;
                }
            }
        }
    }
}
