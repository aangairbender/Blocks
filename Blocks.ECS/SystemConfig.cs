using System;
using System.Collections.Generic;
using System.Linq;

namespace Blocks.ECS
{
    public class SystemConfig
    {
        private readonly ICollection<Type> _requiredComponents = new List<Type>();

        public void RequiresComponent<T>() where T : ComponentBase
        {
            _requiredComponents.Add(typeof(T));
        }

        public bool SupportsEntity(Entity entity)
        {
            return _requiredComponents
                .Select(entity.GetComponent)
                .All(component => component != null);
        }
    }
}