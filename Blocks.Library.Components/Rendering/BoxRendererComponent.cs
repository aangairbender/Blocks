using System.Windows.Media.Media3D;
using Blocks.ECS;
using BulletSharp;
using HelixToolkit.Wpf;

namespace Blocks.Library.Components.Rendering
{
    public class BoxRendererComponent : RendererComponent
    {
        public override string Name => "Box Renderer";

        public Property<float> XLength { get; } = new Property<float>("X Length"){ Value = 1 };

        public Property<float> YLength { get; } = new Property<float>("Y Length") { Value = 1 };

        public Property<float> ZLength { get; } = new Property<float>("Z Length") { Value = 1 };

        public override MeshGeometry3D BuildMesh()
        {
            var mb = new MeshBuilder();
            mb.AddBox(new Point3D(), XLength.Value, YLength.Value, ZLength.Value);
            return mb.ToMesh();
        }

        public override CollisionShape BuildCollisionShape()
        {
            return new BoxShape(XLength.Value / 2, YLength.Value / 2, ZLength.Value / 2);
        }

        public override ComponentBase Clone()
        {
            var clone = new BoxRendererComponent();
            clone.Initialize();
            clone.XLength.Value = XLength.Value;
            clone.YLength.Value = YLength.Value;
            clone.ZLength.Value = ZLength.Value;
            return clone;
        }
    }
}