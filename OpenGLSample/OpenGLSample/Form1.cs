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

        public Point Line1Point = new Point(250, 0);
        public Point Line2Point = new Point(350, 0);
        public Point Line3Point = new Point(350, 50);

        private Bitmap img; 

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            rePaint();
        }

        public void rePaint()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color3(Color.Yellow);
            GL.Begin(BeginMode.Triangles);
            GL.Vertex2(Line1Point.X, Line1Point.Y);
            GL.Vertex2(Line2Point.X, Line2Point.Y);
            GL.Vertex2(Line3Point.X, Line3Point.Y);
            GL.End();
            UpdateImage();
            //glControl1.SwapBuffers();
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

        private void Redraw()
        {
            glControl1.Invalidate();
        }

        private bool isMouseDown = false;
        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            FP_MouseStart = new Vector2(e.X , glControl1.Height - e.Y);
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            Vector2 FE = new Vector2(e.X,glControl1.Height - e.Y);
            Pan_Move(FP_MouseStart,FE);
        }
  

        private void glControl1_MousWheel(object sender, MouseEventArgs e)
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

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if(isMouseDown)
            {
                Vector2 FE = new Vector2(e.X, glControl1.Height - e.Y);
                Pan_Move(FP_MouseStart, FE);
                FP_MouseStart = new Vector2(e.X, glControl1.Height - e.Y);
            }
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
