using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Blocks.Hierarchy;

namespace Blocks.ECS
{
    public abstract class System<TEntity> : IDisposable
        where TEntity : Entity, IHierarchyNode<TEntity>
    {
        protected SystemConfig Config { get; private set; } = new SystemConfig();

        public TEntity RootNode { get; private set; }

        public virtual void Initialize(TEntity rootNode)
        {
            SetupSystem(Config);
            RootNode = rootNode;
            AddNode(rootNode);
        }

        public virtual void Dispose()
        {
            RemoveNode(RootNode);
        }

        protected virtual void AddNode(TEntity node)
        {
            node.Traverse(TraverseSubscribeChildrenChangedAction);
            node.Traverse(TraverseSubscribeComponentChangedAction);
            node.Traverse(TraverseSubscribeComponentsChangedAction);
            node.Traverse(TraverseAfterAddedAction);
        }

        protected virtual void RemoveNode(TEntity node)
        {
            node.Traverse(TraverseBeforeRemovedAction);
            node.Traverse(TraverseUnsubscribeComponentChangedAction);
            node.Traverse(TraverseUnsubscribeComponentsChangedAction);
            node.Traverse(TraverseUnsubscribeChildrenChangedAction);
        }

        protected abstract void SetupSystem(SystemConfig config);

        private void TraverseSubscribeChildrenChangedAction(TEntity node)
        {
            node.ChildAdded += ChildAddedHandler;
            node.ChildRemoved += ChildRemovedHandler;
        }

        private void TraverseUnsubscribeChildrenChangedAction(TEntity node)
        {
            node.ChildAdded -= ChildAddedHandler;
            node.ChildRemoved -= ChildRemovedHandler;
        }

        private void TraverseSubscribeComponentsChangedAction(TEntity entity)
        {
            entity.ComponentsCollectionChanged += ComponentsChangedHandler;
        }

        private void TraverseUnsubscribeComponentsChangedAction(TEntity entity)
        {
            entity.ComponentsCollectionChanged -= ComponentsChangedHandler;
        }

        protected virtual void TraverseAfterAddedAction(TEntity entity)
        {

        }

        protected virtual void TraverseBeforeRemovedAction(TEntity entity)
        {

        }

        private void TraverseSubscribeComponentChangedAction(TEntity entity)
        {
            entity.ComponentChanged += EntityComponentChanged;
        }

        private void TraverseUnsubscribeComponentChangedAction(TEntity entity)
        {
            entity.ComponentChanged -= EntityComponentChanged;
        }

        protected virtual void EntityComponentChanged(object sender, ComponentPropertyChangedEventArgs e)
        {
        }

        private void ComponentsChangedHandler(object sender, EventArgs e)
        {
            if (!(sender is TEntity entity))
                return;

            RemoveNode(entity);
            AddNode(entity);
        }

        protected virtual void ChildRemovedHandler(TEntity child)
        {
            RemoveNode(child);
        }

        protected virtual void ChildAddedHandler(TEntity child)
        {
            AddNode(child);
        }
    }

    public abstract class System<TEntity, TEntitySystemInfo> : System<TEntity>
        where TEntity : Entity, IHierarchyNode<TEntity>
    {
        protected readonly IDictionary<TEntity, TEntitySystemInfo> EntityInfo =
            new ConcurrentDictionary<TEntity, TEntitySystemInfo>();

        protected abstract TEntitySystemInfo GenerateEntityInfo(TEntity entity);

        private void TraverseAddEntityToSystemAction(TEntity entity)
        {
            if (!Config.SupportsEntity(entity))
                return;

            EntityInfo.Add(entity, GenerateEntityInfo(entity));
        }

        private void TraverseRemoveEntityFromSystemAction(TEntity entity)
        {
            EntityInfo.Remove(entity);
        }

        protected override void AddNode(TEntity node)
        {
            node.Traverse(TraverseAddEntityToSystemAction);
            base.AddNode(node);
        }

        protected override void RemoveNode(TEntity node)
        {
            base.RemoveNode(node);
            node.Traverse(TraverseRemoveEntityFromSystemAction);
        }
    }
}