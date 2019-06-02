using System.Windows.Media;
using System.Windows.Media.Media3D;
using Blocks.Library.Components;
using Blocks.Library.Components.Rendering;
using HelixToolkit.Wpf;
using Matrix = BulletSharp.Math.Matrix;

namespace Blocks.Systems.Rendering
{
    internal static class RenderingHelper
    {
        public static Matrix3D ToMatrix3D(this Matrix matrix)
        {
            return new Matrix3D(matrix.M11, matrix.M12, matrix.M13, matrix.M14, matrix.M21,
                matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34, matrix.M41, matrix.M42, matrix.M43, matrix.M44);
        }

        public static GeometryModel3D BuildModel(this RendererComponent renderer)
        {
            return new GeometryModel3D
            {
                Geometry = renderer.BuildMesh(),
                Material = MaterialHelper.CreateMaterial(Colors.Red),
                BackMaterial = MaterialHelper.CreateMaterial(Colors.Red)
            };
        }
    }
}