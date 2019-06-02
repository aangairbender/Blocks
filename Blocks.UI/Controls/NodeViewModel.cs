using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Blocks.Core;
using Blocks.Core.Annotations;
using PropertyTools;

namespace Blocks.UI.Controls
{
    public class NodeViewModel : IDragSource, IDropTarget, INotifyPropertyChanged
    {
        private readonly Block _block;
        private ObservableCollection<NodeViewModel> _children;

        public NodeViewModel Parent { get; private set; }

        public bool HasItems => _block.Children.Count > 0;

        public bool CanDrop(IDragSource node, DropPosition mode, DragDropEffect effect)
        {
            var cvm2 = this;
            while (cvm2 != null)
            {
                if (cvm2 == node)
                    return false;
                cvm2 = cvm2.Parent;
            }
            return node is NodeViewModel && (mode == DropPosition.Add || this.Parent != null);
        }

        public void Drop(IEnumerable<IDragSource> nodes, DropPosition mode, DragDropEffect effect, DragDropKeyStates initialKeyStates)
        {
            foreach (var node in nodes)
            {
                this.Drop(node, mode, effect == DragDropEffect.Copy);
            }
        }

        public Block Node => _block;

        public void Drop(IDragSource node, DropPosition mode, bool copy)
        {
            var cvm = node as NodeViewModel;
            if (copy) cvm = new NodeViewModel(cvm.Node, cvm.Parent);
            switch (mode)
            {
                case DropPosition.Add:
                    this.Children.Add(cvm);
                    cvm.Parent = this;
                    cvm.Parent.Node.AddChild(cvm.Node);
                    this.IsExpanded = true;
                    break;
                case DropPosition.InsertBefore:
                    int index = this.Parent.Children.IndexOf(this);
                    Parent.Children.Insert(index, cvm);
                    cvm.Parent = this.Parent;
                    break;
                case DropPosition.InsertAfter:
                    int index2 = this.Parent.Children.IndexOf(this);
                    Parent.Children.Insert(index2 + 1, cvm);
                    cvm.Parent = this.Parent;
                    break;
            }
        }

        public bool IsDraggable => Parent != null;

        public void Detach()
        {
            Parent.Node.RemoveChild(Node);
            this.Parent.Children.Remove(this);
            this.Parent = null;
        }

        public ObservableCollection<NodeViewModel> Children
        {
            get
            {
                this.LoadChildren();
                return _children;
            }
        }

        private void LoadChildren()
        {
            if (_children == null)
            {
                _children = new ObservableCollection<NodeViewModel>();
                foreach (var child in _block.Children)
                {
                    _children.Add(new NodeViewModel(child, this));
                }
            }
        }

        public string Name
        {
            get => _block.Name;
            set
            {
                _block.Name = value;
                OnPropertyChanged();
            }
        }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public int Level { get; set; }

        private bool _isEditing;

        public bool IsEditing
        {
            get
            {
                return _isEditing;
            }
            set
            {
                _isEditing = value;
                OnPropertyChanged();
            }
        }

        public NodeViewModel(Block block, NodeViewModel parent)
        {
            _block = block;
            Parent = parent;
            IsExpanded = true;
        }

        public override string ToString()
        {
            return Name;
        }

        public NodeViewModel AddChild(Block newChild, bool isEditing = false)
        {
            _block.AddChild(newChild);
            var vm = new NodeViewModel(newChild, this) {IsEditing = isEditing};
            Children.Add(vm);
            return vm;
        }

        public void ExpandParents()
        {
            if (this.Parent != null)
            {
                this.Parent.ExpandParents();
                this.Parent.IsExpanded = true;
            }
        }

        public void ExpandAll()
        {
            this.IsExpanded = true;
            foreach (var child in this.Children)
            {
                child.ExpandAll();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}