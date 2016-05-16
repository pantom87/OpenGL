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

        Vector2 FS;
        Vector2 W_C = new Vector2(0, 0);
        private float ScaleVelue_FtoW = 1;

        Point Line1Point = new Point(0, 0);
        Point Line2Point = new Point(100, 0);
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
            UpdateProjection();
        }

        private Vector2 TransForm_FtoW( Vector2 F )
        {
            return F / ScaleVelue_FtoW;
        }

        private Vector2 TransForm_WtoF(Vector2 W)
        {
            return W * ScaleVelue_FtoW;
        }

        private void Pan_Move(Vector2 VS ,Vector2 VE)
        {
            Vector2 T_FES = VE - VS;
            Vector2 w = TransForm_FtoW(T_FES);
            W_C -= w;
            UpdateProjection();
        }
        private void UpdateProjection()
        {
            GL.ClearColor(Color.SkyBlue);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
            GL.Ortho(0, glControl1.Width / ScaleVelue_FtoW, 0, glControl1.Height / ScaleVelue_FtoW, -100, 100); // Bottom-left corner pixel has coordinate (0, 0)      안원근법
            {
                Vector3 V3Eye = new Vector3(W_C);
                V3Eye.Z = 100;
                Vector3 V3Target = new Vector3(W_C);
                V3Target.Z = -1;
                Vector3 V3Up = Vector3.UnitY;

                Matrix4 m4 = Matrix4.LookAt(V3Eye, V3Target, V3Up);

                GL.MultMatrix(ref m4);
            }
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            glControl1.Invalidate();
        }


        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            FS = new Vector2(e.X , -e.Y);
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            Vector2 FE = new Vector2(e.X,-e.Y);
            Pan_Move(FS,FE);
        }
  

        private void glControl1_MousWheel(object sender, MouseEventArgs e)
        {
            Vector2 FE = new Vector2(0,-glControl1.Height);
            FS = new Vector2(e.X, -e.Y);

            Pan_Move(FS, FE);
            if (e.Delta > 0)
            {
                if (ScaleVelue_FtoW > 5) ScaleVelue_FtoW = 5;
                ScaleVelue_FtoW +=0.1f;
            }
            else
            {
                ScaleVelue_FtoW -=0.1f;
                if (ScaleVelue_FtoW < 0.1) ScaleVelue_FtoW = 0.1f;
            }
            Pan_Move(FE, FS);
        }
    }
}
