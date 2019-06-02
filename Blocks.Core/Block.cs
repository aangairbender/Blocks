using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Blocks.Core.Annotations;
using Blocks.ECS;
using Blocks.Hierarchy;

namespace Blocks.Core
{
    public sealed class Block : Entity, IHierarchyNode<Block>, INotifyPropertyChanged
    {
        private readonly ICollection<Block> _children = new List<Block>();
        private string _name;

        public Block Parent { get; private set; }

        public IReadOnlyCollection<Block> Children => (IReadOnlyCollection<Block>)_children;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public event Action<Block> ChildAdded;
        public event Action<Block> ChildRemoved;

        public void AddChild(Block child)
        {
            child.Parent = this;
            _children.Add(child);
            ChildAdded?.Invoke(child);
        }

        public void RemoveChild(Block child)
        {
            if (!_children.Remove(child))
                return;
            ChildRemoved?.Invoke(child);
            child.Parent = null;
        }

        public Block(string name)
        {
            _name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Block Clone()
        {
            var clone = new Block(Name);
            foreach (var componentBase in Components)
            {
                clone.AddComponent(componentBase.Clone());
            }
            foreach (var child in _children)
            {
                clone.AddChild(child.Clone());
            }

            return clone;
        }
    }
}