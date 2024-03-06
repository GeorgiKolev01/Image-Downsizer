namespace ResizeAnImage
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonImageSelect = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonSaveFunctionalWay = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSaveThreadsWay = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.calculationTimeTxt = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(150, 70);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(500, 27);
            this.textBox1.TabIndex = 0;
            // 
            // buttonImageSelect
            // 
            this.buttonImageSelect.Location = new System.Drawing.Point(335, 114);
            this.buttonImageSelect.Name = "buttonImageSelect";
            this.buttonImageSelect.Size = new System.Drawing.Size(130, 29);
            this.buttonImageSelect.TabIndex = 1;
            this.buttonImageSelect.Text = "Select Image";
            this.buttonImageSelect.UseVisualStyleBackColor = true;
            this.buttonImageSelect.Click += new System.EventHandler(this.buttonImageSelect_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(200, 160);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 200);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // buttonSaveFunctionalWay
            // 
            this.buttonSaveFunctionalWay.Enabled = false;
            this.buttonSaveFunctionalWay.Location = new System.Drawing.Point(401, 366);
            this.buttonSaveFunctionalWay.Name = "buttonSaveFunctionalWay";
            this.buttonSaveFunctionalWay.Size = new System.Drawing.Size(195, 29);
            this.buttonSaveFunctionalWay.TabIndex = 3;
            this.buttonSaveFunctionalWay.Text = "Save Image by Functions";
            this.buttonSaveFunctionalWay.UseVisualStyleBackColor = true;
            this.buttonSaveFunctionalWay.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(295, 383);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 27);
            this.textBox2.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Image path:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 386);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Minify image %:";
            // 
            // buttonSaveThreadsWay
            // 
            this.buttonSaveThreadsWay.Enabled = false;
            this.buttonSaveThreadsWay.Location = new System.Drawing.Point(401, 401);
            this.buttonSaveThreadsWay.Name = "buttonSaveThreadsWay";
            this.buttonSaveThreadsWay.Size = new System.Drawing.Size(195, 29);
            this.buttonSaveThreadsWay.TabIndex = 7;
            this.buttonSaveThreadsWay.Text = "Save Image by Threads";
            this.buttonSaveThreadsWay.UseVisualStyleBackColor = true;
            this.buttonSaveThreadsWay.Click += new System.EventHandler(this.buttonSaveThreadsWay_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(205, 448);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(190, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Time for Whole Calculation";
            // 
            // calculationTimeTxt
            // 
            this.calculationTimeTxt.Enabled = false;
            this.calculationTimeTxt.Location = new System.Drawing.Point(401, 445);
            this.calculationTimeTxt.Name = "calculationTimeTxt";
            this.calculationTimeTxt.Size = new System.Drawing.Size(195, 27);
            this.calculationTimeTxt.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 482);
            this.Controls.Add(this.calculationTimeTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonSaveThreadsWay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.buttonSaveFunctionalWay);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonImageSelect);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Image Resizer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBox1;
        private Button buttonImageSelect;
        private PictureBox pictureBox1;
        private Button buttonSaveFunctionalWay;
        private TextBox textBox2;
        private Label label1;
        private Label label2;
        private Button buttonSaveThreadsWay;
        private Label label3;
        private TextBox calculationTimeTxt;
    }
}