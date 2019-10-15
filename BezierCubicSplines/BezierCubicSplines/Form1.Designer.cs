namespace BezierCubicSplines
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ClearBtn = new System.Windows.Forms.Button();
            this.PointslistBox = new System.Windows.Forms.ListBox();
            this.DeletePntBtn = new System.Windows.Forms.Button();
            this.AddPointBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(736, 615);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(773, 563);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(164, 64);
            this.ClearBtn.TabIndex = 1;
            this.ClearBtn.Text = "Clear";
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // PointslistBox
            // 
            this.PointslistBox.FormattingEnabled = true;
            this.PointslistBox.ItemHeight = 16;
            this.PointslistBox.Location = new System.Drawing.Point(773, 12);
            this.PointslistBox.Name = "PointslistBox";
            this.PointslistBox.Size = new System.Drawing.Size(164, 420);
            this.PointslistBox.TabIndex = 2;
            // 
            // DeletePntBtn
            // 
            this.DeletePntBtn.Location = new System.Drawing.Point(773, 506);
            this.DeletePntBtn.Name = "DeletePntBtn";
            this.DeletePntBtn.Size = new System.Drawing.Size(164, 41);
            this.DeletePntBtn.TabIndex = 3;
            this.DeletePntBtn.Text = "Delete Point";
            this.DeletePntBtn.UseVisualStyleBackColor = true;
            // 
            // AddPointBtn
            // 
            this.AddPointBtn.Location = new System.Drawing.Point(773, 453);
            this.AddPointBtn.Name = "AddPointBtn";
            this.AddPointBtn.Size = new System.Drawing.Size(164, 41);
            this.AddPointBtn.TabIndex = 4;
            this.AddPointBtn.Text = "Add Point";
            this.AddPointBtn.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 655);
            this.Controls.Add(this.AddPointBtn);
            this.Controls.Add(this.DeletePntBtn);
            this.Controls.Add(this.PointslistBox);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "BezierCurve";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ClearBtn;
        private System.Windows.Forms.ListBox PointslistBox;
        private System.Windows.Forms.Button DeletePntBtn;
        private System.Windows.Forms.Button AddPointBtn;
    }
}

