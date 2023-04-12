using System;
using System.Linq;
using System.Reflection;

namespace TTX.Library.InstancingHelpers;

public static partial class CopyHelper
{
    public static TTarget CopyPropertiesTo<TSource, TTarget>(this TSource source, TTarget target)
    {
        if (source is null || target is null)
            throw new NullReferenceException();

        foreach (var prop in typeof(TSource).GetProperties().Where(x => x.CanRead))
        {
            if (typeof(TTarget).GetProperty(prop.Name) is PropertyInfo targetProp &&
                targetProp.CanWrite &&
                prop.PropertyType == targetProp.PropertyType)
            {
                targetProp.SetValue(target, prop.GetValue(source));
            }
        }

        return target;
    }
}