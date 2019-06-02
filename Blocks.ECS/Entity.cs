using System;
using System.Collections.Generic;
using System.Linq;

namespace Blocks.ECS
{
    public abstract class Entity
    {
        private readonly ICollection<ComponentBase> _components = new List<ComponentBase>();

        public IReadOnlyCollection<ComponentBase> Components => (IReadOnlyCollection<ComponentBase>) _components;

        public event EventHandler ComponentsCollectionChanged;

        public event EventHandler<ComponentPropertyChangedEventArgs> ComponentChanged;

        public void AddComponent<T>() where T : ComponentBase, new()
        {
            var component = new T();
            AddComponent(component);
        }

        public void AddComponent(ComponentBase component)
        {
            component.Initialize();

            component.PropertyChanged += ComponentPropertyChangedHandler;

            _components.Add(component);
            ComponentsCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ComponentPropertyChangedHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ComponentChanged?.Invoke(this, new ComponentPropertyChangedEventArgs(sender as ComponentBase));
        }

        public void RemoveComponent<T>() where T : ComponentBase
        {
            var component = GetComponent<T>();
            if (component == null)
                return;

            component.PropertyChanged -= ComponentPropertyChangedHandler;

            _components.Remove(component);
            ComponentsCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public T GetComponent<T>() where T : ComponentBase
        {
            return _components.OfType<T>().FirstOrDefault();
        }

        public ComponentBase GetComponent(Type componentType)
        {
            return _components.FirstOrDefault(componentType.IsInstanceOfType);
        }
    }
}