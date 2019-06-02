using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Blocks.Hierarchy
{
    public interface IHierarchyNode<T>
        where T : IHierarchyNode<T>
    {
        T Parent { get; }
        IReadOnlyCollection<T> Children { get; }

        event Action<T> ChildAdded;
        event Action<T> ChildRemoved;

        void AddChild(T child);
        void RemoveChild(T child);
    }
}