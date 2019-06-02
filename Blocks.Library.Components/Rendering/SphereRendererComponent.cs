using System.Windows.Media.Media3D;
using Blocks.ECS;
using BulletSharp;
using HelixToolkit.Wpf;

namespace Blocks.Library.Components.Rendering
{
    public class SphereRendererComponent : RendererComponent
    {
        public override string Name => "Sphere Renderer";
        
        public Property<float> Radius { get; set; }
        
        public Property<int> ThetaDiv { get; set; }

        public Property<int> PhiDiv { get; set; }

        public SphereRendererComponent()
        {
            Radius = new Property<float>("Radius"){Value = 1};
            ThetaDiv = new Property<int>("Theta Div"){Value = 32};
            PhiDiv = new Property<int>("Phi Div"){Value = 32};
        }

        public override MeshGeometry3D BuildMesh()
        {
            var mb = new MeshBuilder();
            mb.AddSphere(new Point3D(), Radius.Value, ThetaDiv.Value, PhiDiv.Value);
            return mb.ToMesh();
        }

        public override CollisionShape BuildCollisionShape()
        {
            return new SphereShape(Radius.Value);
        }

        public override ComponentBase Clone()
        {
            var clone = new SphereRendererComponent();
            clone.Initialize();
            clone.Radius.Value = Radius.Value;
            clone.ThetaDiv.Value = ThetaDiv.Value;
            clone.PhiDiv.Value = PhiDiv.Value;
            return clone;
        }
    }
}