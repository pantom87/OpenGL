﻿namespace OpenGLSample
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.glc_Main = new OpenTK.GLControl();
            this.SuspendLayout();
            // 
            // glc_Main
            // 
            this.glc_Main.BackColor = System.Drawing.Color.Black;
            this.glc_Main.Location = new System.Drawing.Point(122, 64);
            this.glc_Main.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.glc_Main.Name = "glc_Main";
            this.glc_Main.Size = new System.Drawing.Size(500, 500);
            this.glc_Main.TabIndex = 0;
            this.glc_Main.VSync = false;
            this.glc_Main.Load += new System.EventHandler(this.glc_Main_Load);
            this.glc_Main.Paint += new System.Windows.Forms.PaintEventHandler(this.glc_Main_Paint);
            this.glc_Main.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.glControl_Main_MouseDoubleClick);
            this.glc_Main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl_Main_MouseDown);
            this.glc_Main.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl_Main_MouseMove);
            this.glc_Main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl_Main_MouseUp);
            this.glc_Main.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glControl_Main_MouseWheel);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 630);
            this.Controls.Add(this.glc_Main);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        public OpenTK.GLControl glc_Main;
    }
}

