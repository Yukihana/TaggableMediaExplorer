using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TTX.Library.Comparers;

public class StringMemberComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T, string> _transform;
    private readonly StringComparison _comparisonType;

    public StringMemberComparer(Func<T, string> transform, StringComparison comparisonType)
    {
        _transform = transform;
        _comparisonType = comparisonType;
    }

    public bool Equals(T? x, T? y)
    {
        if (x is null && y is null)
            return true;

        if (x is null || y is null)
            return false;

        return _transform(x).Equals(_transform(y), _comparisonType);
    }

    public int GetHashCode([DisallowNull] T obj)
        => _transform(obj).GetHashCode();
}