using System.Windows.Media.Media3D;
using Blocks.ECS;
using Blocks.Utils;
using BulletSharp.Math;

namespace Blocks.Library.Components
{
    public class TransformComponent : ComponentBase
    {
        public override string Name => "Transform";
        public override ComponentBase Clone()
        {
            var clone = new TransformComponent();
            clone.Initialize();
            clone.Position.Value = Position.Value;
            clone.Rotation.Value = Rotation.Value;
            clone.Scale.Value = Scale.Value;
            return clone;
        }

        public Property<Vector3> Position { get; }
        
        public Property<Vector3> Rotation { get; }
        
        public Property<Vector3> Scale { get; }

        public TransformComponent()
        {
            Position = new Property<Vector3>("Position"){ Value = Vector3.Zero };
            Rotation = new Property<Vector3>("Rotation") { Value = Vector3.Zero };
            Scale = new Property<Vector3>("Scale") { Value = Vector3.One };
        }

        public Matrix BuildMatrix()
        {
            return Matrix.Scaling(Scale.Value.X, Scale.Value.Y, Scale.Value.Z) *
                   Matrix.RotationYawPitchRoll(Rotation.Value.Y, Rotation.Value.X, Rotation.Value.Z) *
                   Matrix.Translation(Position.Value);
        }
    }
}