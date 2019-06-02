using Blocks.ECS;

namespace Blocks.Library.Components.Physics
{
    public class RigidBodyComponent : ComponentBase
    {
        public override string Name => "Rigid body";

        public Property<float> Mass { get; } = new Property<float>("Mass") {Value = 1};

        public Property<bool> IsKinematic { get; } = new Property<bool>("Is Kinematic") {Value = false};

        public Property<float> Friction { get; } = new Property<float>("Friction") {Value = 0.2f};

        public Property<float> Restitution { get; } = new Property<float>("Restitution") {Value = 0.5f};

        public override ComponentBase Clone()
        {
            var clone = new RigidBodyComponent();
            clone.Initialize();
            clone.Mass.Value = Mass.Value;
            clone.IsKinematic.Value = IsKinematic.Value;
            clone.Friction.Value = Friction.Value;
            clone.Restitution.Value = Restitution.Value;
            return clone;
        }
    }
}