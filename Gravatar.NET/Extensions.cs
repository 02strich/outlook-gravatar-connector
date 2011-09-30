using System;
using System.Collections.Generic;
using System.Linq;
using Gravatar.NET.Data;

namespace Gravatar.NET {
    public static class Extensions {
        internal static void Do<T>(this IEnumerable<T> collection, Action<T> action) {
            foreach (var element in collection)
                action.Invoke(element);
        }

        public static GravatarParameter Get(this IEnumerable<GravatarParameter> collection, string name) {
            return collection.FirstOrDefault(par => par.Name == name);
        }
    }
}
