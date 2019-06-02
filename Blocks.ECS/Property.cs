using System;

namespace Blocks.ECS
{
    public class Property<T> : Property
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (_value != null && _value.Equals(value))
                    return;

                _value = value;
                InvokeValueChanged();
            }
        }

        public Property(string name) : base(name)
        {
        }
    }

    public abstract class Property
    {
        public string Name { get; }

        public event EventHandler ValueChanged;

        protected Property(string name)
        {
            Name = name;
        }

        protected void InvokeValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}