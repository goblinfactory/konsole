using System;
using System.Collections.Generic;
using System.Linq;

namespace Konsole.Internal
{
    internal static class LinqExtensions
    {
        ///// <summary>
        ///// decorate a collection of items.
        ///// </summary>
        ///// <param name="decorate">a function taking an item, bool first, and bool last</param>
        //public static (T, TDecoration)[] Decorate<T, TDecoration>(this IEnumerable<T> src,
        //     Func<T, (bool isFirst, bool isLast), TDecoration> decorate)
        //{
        //    var arr = src.ToArray();
        //    return arr.Decorate(decorate);
        //}

        ///// <summary>
        ///// decorate an array of items.
        ///// </summary>
        ///// <param name="decorate">a function taking an item, bool first, and bool last</param>
        ///// <remarks>closures that capture and modify local variables will not be threadsafe.</remarks>
        //public static (T, TDecoration)[] Decorate<T,TDecoration>(this T[] src, 
        //    Func<T, bool, bool, TDecoration> decorate) 
        //{
        //    int cnt = src?.Length ?? 0;
        //    var results = new (T, TDecoration)[cnt];
        //    if (cnt == 0) return results;
            
        //    int i = 0;
        //    foreach (var item in src)
        //    {
        //        i++;
        //        bool first = (i == 1);
        //        bool last = (i == cnt);
        //        var decoration = decorate(item, first, last);
        //        results[i-1] = (item, decoration);
        //    }
        //    return results;
        //}

        public static T[] SelectWithFirstLast<T>(this T[] src, Func<T, bool, bool, T> selector)
        {
            int cnt = src?.Length ?? 0;
            var results = new T[cnt];
            if (cnt == 0) return results;

            int i = 0;
            foreach (var item in src)
            {
                i++;
                bool first = (i == 1);
                bool last = (i == cnt);
                var newitem = selector(item, first, last);
                results[i - 1] = newitem;
            }
            return results;
        }

    }
}
