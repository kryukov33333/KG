using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Media.Media3D;

namespace Axes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // The camera.
        private PerspectiveCamera TheCamera = null;

        // The camera controller.
        private SphericalCameraController CameraController = null;
            ModelVisual3D visual3d = new ModelVisual3D();
            Model3DGroup group = new Model3DGroup();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Define WPF objects.

            visual3d.Content = group;
            mainViewport.Children.Add(visual3d);

            // Define the camera, lights, and model.
            DefineCamera(mainViewport);
            DefineLights(group);
            DefineModel(group);
            DefineObj(group);
        }

        Point3D[] mty;
        // Define the camera.
        private void DefineCamera(Viewport3D viewport)
        {
            TheCamera = new PerspectiveCamera();
            TheCamera.FieldOfView = 60;
            CameraController = new SphericalCameraController
                (TheCamera, viewport, this, mainGrid, mainGrid);
        }

        // Define the lights.
        private void DefineLights(Model3DGroup group)
        {
            group.Children.Add(new AmbientLight(Color.FromArgb(255, 64, 64, 64)));

            group.Children.Add(new DirectionalLight(Colors.LightGray, new Vector3D(1, -3, -2)));
            group.Children.Add(new DirectionalLight(Colors.LightGray, new Vector3D(-1, 3, 2)));
        }

        // Define the model.
        double massh = 1;




        private void DefineObj(Model3DGroup group)
        {
            MeshGeometry3D mesh = MakeCubeMesh(0.05, 0.6, 0, massh);
            mesh.ApplyTransformation(new ScaleTransform3D(1, 0.1, 0.1));
            mesh.ApplyTransformation(new TranslateTransform3D(1, 1.6, 1));
            Material material = new DiffuseMaterial(Brushes.Purple);
            GeometryModel3D model = new GeometryModel3D(mesh, material);
            group.Children.Add(model);

            // Z axis cube.
            MeshGeometry3D zsmesh = MakeCubeMesh(0.2, 0.2, 0.2, massh);
            Transform3DGroup zsgroup = new Transform3DGroup();
            zsmesh.ApplyTransformation(new ScaleTransform3D(0.1, 1, 0.1));
            zsmesh.ApplyTransformation(new TranslateTransform3D(1, 1, 1));
            zsmesh.ApplyTransformation(zsgroup);
            Material zsmaterial = new DiffuseMaterial(Brushes.Black);
            GeometryModel3D zsmodel = new GeometryModel3D(zsmesh, zsmaterial);
            group.Children.Add(zsmodel);
            mty = zsmesh.Positions.ToArray();

            MeshGeometry3D imesh = MakeCubeMesh(0, -6, 0, massh);
            imesh.ApplyTransformation(new ScaleTransform3D(1, 0.1, 0.1));
            imesh.ApplyTransformation(new TranslateTransform3D(1, 1.3, 1));
            Material imaterial = new DiffuseMaterial(Brushes.Green);
            GeometryModel3D imodel = new GeometryModel3D(imesh, imaterial);
            group.Children.Add(imodel);

        }
        private void DefineModel(Model3DGroup group)
        {
            // Origin cube.
            //MeshGeometry3D mesh = MakeCubeMesh(0, 0, 0, 0.5);
            //Material material = new DiffuseMaterial(Brushes.Yellow);
            //GeometryModel3D model = new GeometryModel3D(mesh, material);
            //group.Children.Add(model);

            
            

            const double thickness = 0.01;
            const double length = 3;

            // X axis.
            MeshGeometry3D xmesh = MakeCubeMesh(0, 0, 0, 1);
            xmesh.ApplyTransformation(new ScaleTransform3D(length, thickness, thickness));
            xmesh.ApplyTransformation(new TranslateTransform3D(length / 2, 0, 0));
            Material xmaterial = new DiffuseMaterial(Brushes.Black);
            GeometryModel3D xmodel = new GeometryModel3D(xmesh, xmaterial);
            group.Children.Add(xmodel);

            // Y axis cube.
            MeshGeometry3D ymesh = MakeCubeMesh(0, 0, 0, 1);
            ymesh.ApplyTransformation(new ScaleTransform3D(thickness, length, thickness));
            ymesh.ApplyTransformation(new TranslateTransform3D(0, length / 2, 0));
            Material ymaterial = new DiffuseMaterial(Brushes.Black);
            GeometryModel3D ymodel = new GeometryModel3D(ymesh, ymaterial);
            group.Children.Add(ymodel);

            // Z axis cube.
            MeshGeometry3D zmesh = MakeCubeMesh(0, 0, 0, 1);
            Transform3DGroup zgroup = new Transform3DGroup();
            zgroup.Children.Add(new ScaleTransform3D(thickness, thickness, length));
            zgroup.Children.Add(new TranslateTransform3D(0, 0, length / 2));
            zmesh.ApplyTransformation(zgroup);
            Material zmaterial = new DiffuseMaterial(Brushes.Black);
            GeometryModel3D zmodel = new GeometryModel3D(zmesh, zmaterial);
            group.Children.Add(zmodel);
        }

        // Make a mesh containing a cube centered at this point.
        private MeshGeometry3D MakeCubeMesh(double x, double y, double z, double width)
        {
            // Create the geometry.
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Define the positions.
            width /= 2;
            Point3D[] points =
            {
                new Point3D(x - width, y - width, z - width),
                new Point3D(x + width, y - width, z - width),
                new Point3D(x + width, y - width, z + width),
                new Point3D(x - width, y - width, z + width),
                new Point3D(x - width, y - width, z + width),
                new Point3D(x + width, y - width, z + width),
                new Point3D(x + width, y + width, z + width),
                new Point3D(x - width, y + width, z + width),
                new Point3D(x + width, y - width, z + width),
                new Point3D(x + width, y - width, z - width),
                new Point3D(x + width, y + width, z - width),
                new Point3D(x + width, y + width, z + width),
                new Point3D(x + width, y + width, z + width),
                new Point3D(x + width, y + width, z - width),
                new Point3D(x - width, y + width, z - width),
                new Point3D(x - width, y + width, z + width),
                new Point3D(x - width, y - width, z + width),
                new Point3D(x - width, y + width, z + width),
                new Point3D(x - width, y + width, z - width),
                new Point3D(x - width, y - width, z - width),
                new Point3D(x - width, y - width, z - width),
                new Point3D(x - width, y + width, z - width),
                new Point3D(x + width, y + width, z - width),
                new Point3D(x + width, y - width, z - width),
            };
            foreach (Point3D point in points) mesh.Positions.Add(point);

            // Define the triangles.
            Tuple<int, int, int>[] triangles =
            {
                 new Tuple<int, int, int>(0, 1, 2),
                 new Tuple<int, int, int>(2, 3, 0),
                 new Tuple<int, int, int>(4, 5, 6),
                 new Tuple<int, int, int>(6, 7, 4),
                 new Tuple<int, int, int>(8, 9, 10),
                 new Tuple<int, int, int>(10, 11, 8),
                 new Tuple<int, int, int>(12, 13, 14),
                 new Tuple<int, int, int>(14, 15, 12),
                 new Tuple<int, int, int>(16, 17, 18),
                 new Tuple<int, int, int>(18, 19, 16),
                 new Tuple<int, int, int>(20, 21, 22),
                 new Tuple<int, int, int>(22, 23, 20),
            };
            foreach (Tuple<int, int, int> tuple in triangles)
            {
                mesh.TriangleIndices.Add(tuple.Item1);
                mesh.TriangleIndices.Add(tuple.Item2);
                mesh.TriangleIndices.Add(tuple.Item3);
            }

            return mesh;
        }




        private void MSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            this.massh = slider.Value;
            DefineObj(group);
            
        }

        private void BtTransp_Click(object sender, RoutedEventArgs e)
        {
            string ss = "";
            foreach(var s in mty)
            {
                ss += s + " ";
            }
            MessageBox.Show(ss);
        }
    }
}
