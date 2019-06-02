using System;
using System.Collections.Generic;
using System.Linq;
using Blocks.Hierarchy;

namespace Blocks.ECS
{
    public static class SystemManager<TEntity> where TEntity : Entity, IHierarchyNode<TEntity>
    {
        private static readonly ICollection<System<TEntity>> _registeredSystems = new List<System<TEntity>>();

        public static void RegisterSystem(System<TEntity> system)
        {
            _registeredSystems.Add(system);
        }

        public static void InitializeSystems(TEntity rootEntity)
        {
            foreach (var registeredSystem in _registeredSystems)
            {
                registeredSystem.Initialize(rootEntity);
            }
        }

        public static T GetSystem<T>() where T : System<TEntity>
        {
            return _registeredSystems.OfType<T>().FirstOrDefault();
        }

        public static void Dispose()
        {
            foreach (var registeredSystem in _registeredSystems)
            {
                registeredSystem.Dispose();
            }
        }
    }
}