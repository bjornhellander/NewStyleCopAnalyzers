// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Lightup
{
    internal delegate bool TryGetValueAccessor<T, TKey, TValue>(T instance, TKey key, out TValue value);
}
