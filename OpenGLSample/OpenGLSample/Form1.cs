using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Threading;

namespace OpenGLSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Vector2 FP_MouseStart;
        Vector2 WP_CameraPosition = new Vector2(0, 0);
        public float Scale_FtoW = 1;


        public List<Point> Triangle_List = new List<Point>(6);
        public List<Point> Ractangle_List = new List<Point>(8);

        public Point Line1Point = new Point(250, 0);
        public Point Line2Point = new Point(350, 0);
        public Point Line3Point = new Point(350, 50);
        private Bitmap img;
        private int Objectidx = 10;

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if(0 == Triangle_List.Count)
            {

            Triangle_List.Add(new Point(0, 0));
            Triangle_List.Add(new Point(100, 0));
            Triangle_List.Add(new Point(100, 50));

            Triangle_List.Add(new Point(20, 30));
            Triangle_List.Add(new Point(120, 30));
            Triangle_List.Add(new Point(120, 80));

            }
//             if(0 == Triangle_List.Count)
//             {
//                    Triangle_List.Add(new Point(250, 0));
//                    Triangle_List.Add(new Point(350, 0));
//                    Triangle_List.Add(new Point(350, 50));
// //                 Triangle_List.Add(new Point(0, 0));
// //                 Triangle_List.Add(new Point(100, 0));
// //                 Triangle_List.Add(new Point(100, 50));
//             }
            rePaint();
        }

        public void rePaint()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.InitNames();
            for (int i = 0; i < (Triangle_List.Count / 3); i++ )
            {
                GL.PushName(Objectidx++);
                GL.Color3(Color.Yellow);
                GL.Begin(BeginMode.Triangles);
                GL.Vertex2(Triangle_List[(3 * i) + 0].X, Triangle_List[(3 * i) + 0].Y);
                GL.Vertex2(Triangle_List[(3 * i) + 1].X, Triangle_List[(3 * i) + 1].Y);
                GL.Vertex2(Triangle_List[(3 * i) + 2].X, Triangle_List[(3 * i) + 2].Y);
                GL.End();
                GL.PopName();
            }
            for (int i = 0; i < (Ractangle_List.Count / 4); i++)
            {
                GL.Color3(Color.Red);
                GL.Begin(BeginMode.Polygon);
                GL.Vertex2(Ractangle_List[(4 * i) + 0].X, Ractangle_List[(4 * i) + 0].Y);
                GL.Vertex2(Ractangle_List[(4 * i) + 1].X, Ractangle_List[(4 * i) + 1].Y);
                GL.Vertex2(Ractangle_List[(4 * i) + 2].X, Ractangle_List[(4 * i) + 2].Y);
                GL.Vertex2(Ractangle_List[(4 * i) + 3].X, Ractangle_List[(4 * i) + 3].Y);
                GL.End();
            }
            UpdateImage();
            OpenTK.Graphics.GraphicsContext.CurrentContext.SwapBuffers();
        }

        private void UpdateImage()
        {
            img = new Bitmap(glControl1.Width, glControl1.Height);
            System.Drawing.Imaging.BitmapData data = img.LockBits(glControl1.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            GL.ReadPixels(0, 0, glControl1.Width, glControl1.Height, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            img.UnlockBits(data);
            //img.RotateFlip(RotateFlipType.RotateNoneFlipY);

            img.Save(@"d:\img.bmp");
        }
        public Color getColor(Point pt)
        {
            return img.GetPixel(pt.X,pt.Y);
        }
        public Color getColor(int X, int Y)
        {
            Point XY = new Point(X, Y);
            return getColor(XY);
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Blue);
            UpdateProjection();
        }
        //변경 public
        public Vector2 TransForm_FtoW( Vector2 F )
        {
            return F / Scale_FtoW;
        }

        private Vector2 TransForm_WtoF(Vector2 W)
        {
            return W * Scale_FtoW;
        }
        //변경 public
        public void Pan_Move(Vector2 VS ,Vector2 VE)
        {
            Vector2 T_FES = VE - VS;
            Vector2 w = TransForm_FtoW(T_FES);
            WP_CameraPosition -= w;
            UpdateProjection();
        }
        public void UpdateProjection()
        {
            GL.ClearColor(Color.Blue);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
            GL.Ortho(0, glControl1.Width / Scale_FtoW, 0, glControl1.Height / Scale_FtoW, -100, 100); // Bottom-left corner pixel has coordinate (0, 0)      안원근법
            {
                Vector3 V3Eye = new Vector3(WP_CameraPosition);
                V3Eye.Z = 100;
                Vector3 V3Target = new Vector3(WP_CameraPosition);
                V3Target.Z = -1;
                Vector3 V3Up = Vector3.UnitY;

                Matrix4 m4 = Matrix4.LookAt(V3Eye, V3Target, V3Up);

                GL.MultMatrix(ref m4);
            }
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            Redraw();
        }

        public void View_reset()
        {
            Scale_FtoW = 1;
            WP_CameraPosition = new Vector2(0, 0);
        }
        public int Mode_Select(Vector2 MousePoint)
        {
            int[] SelectBuffer = new int[64];
            GL.SelectBuffer(64, SelectBuffer);
            GL.RenderMode(RenderingMode.Select);

            // 확대 (scale 계산)
            Move_PixelZoom(MousePoint);
            rePaint();
            int hits = GL.RenderMode(RenderingMode.Render);
            Console.WriteLine("Hit: " + hits);
            // 원복 (추후 구현)


            return hits;
        }


        private void Redraw()
        {
            glControl1.Invalidate();
        }

        private bool isMouseDown = false;
        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            return;
            Console.WriteLine("1");
            isMouseDown = true;
            FP_MouseStart = new Vector2(e.X , glControl1.Height - e.Y);
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            return;
            Console.WriteLine("2");
            isMouseDown = false;
            Vector2 FE = new Vector2(e.X,glControl1.Height - e.Y);
            Pan_Move(FP_MouseStart,FE);
        }
  

        private void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            Vector2 FE = new Vector2(0, 0);
            FP_MouseStart = new Vector2(e.X, glControl1.Height - e.Y);

            Pan_Move(FP_MouseStart, FE);
            if (e.Delta > 0)
            {
                if (Scale_FtoW > 5) Scale_FtoW = 5;
                Scale_FtoW +=0.1f;
            }
            else
            {
                Scale_FtoW -=0.1f;
                if (Scale_FtoW < 0.1) Scale_FtoW = 0.1f;
            }
            Pan_Move(FE, FP_MouseStart);
        }

        public bool Move_PixelZoom(Vector2 MousePoint)
        {
            Vector2 FE = new Vector2(0, 0);
            Vector2 FMousePosition = new Vector2(MousePoint.X, glControl1.Height - MousePoint.Y);

            Pan_Move(FMousePosition, FE);
            
            Scale_FtoW = (glControl1.Width / 10f);

            FMousePosition = new Vector2(glControl1.Width / 2, glControl1.Height / 2);
            Pan_Move(FE, FMousePosition);

            return true;
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if(isMouseDown)
            {
                Vector2 FE = new Vector2(e.X, glControl1.Height - e.Y);
                Pan_Move(FP_MouseStart, FE);
                FP_MouseStart = new Vector2(e.X, glControl1.Height - e.Y);
            }
        }

        private void glControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Triangle_List.Clear();

            Triangle_List.Add(new Point(0, 0));
            Triangle_List.Add(new Point(100, 0));
            Triangle_List.Add(new Point(100, 50));

            Triangle_List.Add(new Point(20, 30));
            Triangle_List.Add(new Point(120, 30));
            Triangle_List.Add(new Point(120, 80));

            UpdateProjection();
            rePaint();

            Mode_Select(new Vector2(e.X, e.Y));
        }

        public bool CompareColor(Color MColor, Color CColor)
        {
            if ((MColor.R == CColor.R) && (MColor.G == CColor.G) && (MColor.B == CColor.B))
            {
                return true;
            }
            return false;
        }
        Point clickp = new Point(0, 0);
        private void glControl1_MouseClick(object sender, MouseEventArgs e)
        {
            clickp.X = e.X;
            clickp.Y = e.Y;
        }
    }
}
