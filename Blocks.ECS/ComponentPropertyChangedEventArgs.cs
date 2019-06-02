using System;

namespace Blocks.ECS
{
    public class ComponentPropertyChangedEventArgs : EventArgs
    {
        public ComponentBase Component { get; }

        public ComponentPropertyChangedEventArgs(ComponentBase component)
        {
            Component = component;
        }
    }
}