﻿namespace GPS地图.示例
{
    partial class F主窗口
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.uTab1 = new Utility.WindowsForm.UTab();
            this.SuspendLayout();
            // 
            // uTab1
            // 
            this.uTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uTab1.Location = new System.Drawing.Point(0, 0);
            this.uTab1.Name = "uTab1";
            this.uTab1.Size = new System.Drawing.Size(1087, 739);
            this.uTab1.TabIndex = 0;
            this.uTab1.标题宽度 = 120;
            // 
            // F主窗口
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uTab1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "F主窗口";
            this.Size = new System.Drawing.Size(1087, 739);
            this.ResumeLayout(false);

        }

        #endregion

        private Utility.WindowsForm.UTab uTab1;
    }
}
