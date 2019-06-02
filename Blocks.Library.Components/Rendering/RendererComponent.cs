using System.Linq;
using System.Windows.Media.Media3D;
using Blocks.ECS;
using BulletSharp;
using BulletSharp.Math;

namespace Blocks.Library.Components.Rendering
{
    public abstract class RendererComponent : ComponentBase
    {
        public abstract MeshGeometry3D BuildMesh();

        public virtual CollisionShape BuildCollisionShape()
        {
            var mesh = BuildMesh();
            return new ConvexTriangleMeshShape(new TriangleIndexVertexArray(mesh.TriangleIndices,
                mesh.Positions.Select(p => new Vector3((float) p.X, (float) p.Y, (float) p.Z)).ToList()));
        }
    }
}