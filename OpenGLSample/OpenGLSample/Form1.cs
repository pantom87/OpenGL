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

namespace OpenGLSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Vector2 StartV2;
        Vector2 DefaultV2 = new Vector2(0, 0);
        Vector2 CurMousePositionV2;

        private float ScrollValue = 1;

        Point Line1Point = new Point(10, 20);
        Point Line2Point = new Point(100, 20);
        Point Line3Point = new Point(100, 50);


        private void glControl1_Paint(object sender, PaintEventArgs e)
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

            glControl1.SwapBuffers();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.SkyBlue);
            SetupViewport();
        }

        private void SetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }

        private void ZoomMode()
        {
            Oper_Zoom();
            Move_Origin();
        }

        private void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            CurMousePositionV2.X = e.X;
            CurMousePositionV2.Y = glControl1.Height - e.Y;
            Move_Center();
            if(e.Delta > 0)
            {
                ScrollValue += 0.1f;
            }
            else
            {
                ScrollValue -= 0.1f;
            }
            ZoomMode();
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            StartV2 = new Vector2(e.X, -e.Y);
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            CurMousePositionV2 = new Vector2(e.X - StartV2.X, -e.Y - StartV2.Y);
            DefaultV2 -= CurMousePositionV2;
            Move_Origin();
        }

        private void Oper_Zoom()
        {
            Vector2 v2 = new Vector2(0, 0);
            GL.ClearColor(Color.SkyBlue);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(0, 500 / ScrollValue, 0, 500 / ScrollValue, -100, 100); // Bottom-left corner pixel has coordinate (0, 0)      안원근법
            {
                Vector3 V3Eye = new Vector3(v2);
                V3Eye.Z = 100;
                Vector3 V3Target = new Vector3(v2);
                V3Target.Z = -1;
                Vector3 V3Up = Vector3.UnitY;

                Matrix4 m4 = Matrix4.LookAt(V3Eye, V3Target, V3Up);

                GL.MultMatrix(ref m4);
            }

            GL.Viewport(0, 0, glControl1.Width, glControl1.Height); // Use all of the glControl painting area
        }

        private void Move_Origin()
        {
            Line1Point.X += Convert.ToInt32(CurMousePositionV2.X / ScrollValue);
            Line1Point.Y += Convert.ToInt32(CurMousePositionV2.Y / ScrollValue);
            Line2Point.X += Convert.ToInt32(CurMousePositionV2.X / ScrollValue);
            Line2Point.Y += Convert.ToInt32(CurMousePositionV2.Y / ScrollValue);
            Line3Point.X += Convert.ToInt32(CurMousePositionV2.X / ScrollValue);
            Line3Point.Y += Convert.ToInt32(CurMousePositionV2.Y / ScrollValue);

            glControl1.Invalidate();
        }

        private void Move_Center()
        {
            Line1Point.X -= Convert.ToInt32(CurMousePositionV2.X / ScrollValue);
            Line1Point.Y -= Convert.ToInt32(CurMousePositionV2.Y / ScrollValue);
            Line2Point.X -= Convert.ToInt32(CurMousePositionV2.X / ScrollValue);
            Line2Point.Y -= Convert.ToInt32(CurMousePositionV2.Y / ScrollValue);
            Line3Point.X -= Convert.ToInt32(CurMousePositionV2.X / ScrollValue);
            Line3Point.Y -= Convert.ToInt32(CurMousePositionV2.Y / ScrollValue);
        }

    }
}
