using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Blocks.ECS.Annotations;

namespace Blocks.ECS
{
    public abstract class ComponentBase : INotifyPropertyChanged, IDisposable
    {
        private bool _isChanging;
        private bool _changeMade;

        private IDictionary<Property, string> _properties;

        public abstract string Name { get; }

        public IEnumerable<Property> ComponentProperties => _properties.Keys;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Initialize()
        {
            InitializeComponentAttributes();
        }

        public void BeginChange()
        {
            _isChanging = true;
            _changeMade = false;
        }

        public void EndChange()
        {
            _isChanging = false;
            if (_changeMade)
            {
                _changeMade = false;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitializeComponentAttributes()
        {
            _properties = new Dictionary<Property, string>();
            foreach (var propertyInfo in GetType().GetProperties())
            {
                if (!propertyInfo.PropertyType.IsGenericType)
                    continue;

                if (!(propertyInfo.GetValue(this) is Property propertyValue))
                    continue;

                _properties.Add(propertyValue, propertyInfo.Name);

                propertyValue.ValueChanged += ComponentPropertyValueChangedHandler;
            }
        }

        private void ComponentPropertyValueChangedHandler(object sender, EventArgs eventArgs)
        {
            if (!(sender is Property senderProperty))
                return;
            _changeMade = true;
            if (_isChanging)
                return;
            OnPropertyChanged(_properties[senderProperty]);
        }

        public void Dispose()
        {
            if (_properties == null)
                return;

            foreach (var property in _properties.Keys)
            {
                property.ValueChanged -= ComponentPropertyValueChangedHandler;
            }
        }

        public abstract ComponentBase Clone();
    }
}
