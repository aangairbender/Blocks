using System;

namespace Blocks.Hierarchy
{
    public static class HierarchyHelper
    {
        public static void Traverse<T>(this T node, Action<T> action)
            where T : IHierarchyNode<T>
        {
            action?.Invoke(node);
            foreach (var childNode in node.Children)
            {
                childNode.Traverse(action);
            }
        }
    }
}