namespace AnimationEditorForms
{
    partial class FrameSelection
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.setButton = new System.Windows.Forms.Button();
            this.ColliderRadioButton = new System.Windows.Forms.RadioButton();
            this.AnimationRadioButton = new System.Windows.Forms.RadioButton();
            this.AnimationNameLabel = new System.Windows.Forms.Label();
            this.frameNumberLabel = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.AddFramebutton = new System.Windows.Forms.Button();
            this.DeleteFrameButton = new System.Windows.Forms.Button();
            this.imageBoxEx1 = new Cyotek.Windows.Forms.Demo.ImageBoxEx();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.setButton);
            this.panel1.Controls.Add(this.ColliderRadioButton);
            this.panel1.Controls.Add(this.AnimationRadioButton);
            this.panel1.Location = new System.Drawing.Point(0, 244);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(468, 40);
            this.panel1.TabIndex = 1;
            // 
            // setButton
            // 
            this.setButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.setButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.setButton.Location = new System.Drawing.Point(197, 3);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(265, 32);
            this.setButton.TabIndex = 3;
            this.setButton.Text = "Set";
            this.setButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // ColliderRadioButton
            // 
            this.ColliderRadioButton.AutoSize = true;
            this.ColliderRadioButton.Location = new System.Drawing.Point(132, 3);
            this.ColliderRadioButton.Name = "ColliderRadioButton";
            this.ColliderRadioButton.Size = new System.Drawing.Size(59, 17);
            this.ColliderRadioButton.TabIndex = 2;
            this.ColliderRadioButton.Text = "Collider";
            this.ColliderRadioButton.UseVisualStyleBackColor = true;
            this.ColliderRadioButton.CheckedChanged += new System.EventHandler(this.ColliderRadioButtonChecked);
            // 
            // AnimationRadioButton
            // 
            this.AnimationRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimationRadioButton.AutoSize = true;
            this.AnimationRadioButton.Checked = true;
            this.AnimationRadioButton.Location = new System.Drawing.Point(3, 3);
            this.AnimationRadioButton.Name = "AnimationRadioButton";
            this.AnimationRadioButton.Size = new System.Drawing.Size(123, 17);
            this.AnimationRadioButton.TabIndex = 1;
            this.AnimationRadioButton.TabStop = true;
            this.AnimationRadioButton.Text = "Animation Rectangle";
            this.AnimationRadioButton.UseVisualStyleBackColor = true;
            this.AnimationRadioButton.CheckedChanged += new System.EventHandler(this.AnimationRadioButtonChecked);
            // 
            // AnimationNameLabel
            // 
            this.AnimationNameLabel.AutoSize = true;
            this.AnimationNameLabel.Location = new System.Drawing.Point(6, 9);
            this.AnimationNameLabel.Name = "AnimationNameLabel";
            this.AnimationNameLabel.Size = new System.Drawing.Size(59, 13);
            this.AnimationNameLabel.TabIndex = 2;
            this.AnimationNameLabel.Tag = "";
            this.AnimationNameLabel.Text = "Animation: ";
            // 
            // frameNumberLabel
            // 
            this.frameNumberLabel.AutoSize = true;
            this.frameNumberLabel.Location = new System.Drawing.Point(164, 9);
            this.frameNumberLabel.Name = "frameNumberLabel";
            this.frameNumberLabel.Size = new System.Drawing.Size(82, 13);
            this.frameNumberLabel.TabIndex = 3;
            this.frameNumberLabel.Text = "Frame Number: ";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(9, 25);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(438, 45);
            this.trackBar1.TabIndex = 7;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // AddFramebutton
            // 
            this.AddFramebutton.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddFramebutton.Location = new System.Drawing.Point(384, 0);
            this.AddFramebutton.Name = "AddFramebutton";
            this.AddFramebutton.Size = new System.Drawing.Size(78, 23);
            this.AddFramebutton.TabIndex = 8;
            this.AddFramebutton.Text = "Add Frame";
            this.AddFramebutton.UseVisualStyleBackColor = true;
            this.AddFramebutton.Click += new System.EventHandler(this.AddFramebutton_Click);
            // 
            // DeleteFrameButton
            // 
            this.DeleteFrameButton.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteFrameButton.Location = new System.Drawing.Point(293, 0);
            this.DeleteFrameButton.Name = "DeleteFrameButton";
            this.DeleteFrameButton.Size = new System.Drawing.Size(94, 23);
            this.DeleteFrameButton.TabIndex = 9;
            this.DeleteFrameButton.Text = "Delete Frame";
            this.DeleteFrameButton.UseVisualStyleBackColor = true;
            this.DeleteFrameButton.Click += new System.EventHandler(this.DeleteFrameButton_Click);
            // 
            // imageBoxEx1
            // 
            this.imageBoxEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBoxEx1.GridCellSize = 10;
            this.imageBoxEx1.Location = new System.Drawing.Point(9, 73);
            this.imageBoxEx1.Margin = new System.Windows.Forms.Padding(0);
            this.imageBoxEx1.Name = "imageBoxEx1";
            this.imageBoxEx1.PixelGridThreshold = 3;
            this.imageBoxEx1.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.Rectangle;
            this.imageBoxEx1.Size = new System.Drawing.Size(450, 165);
            this.imageBoxEx1.TabIndex = 5;
            // 
            // FrameSelection
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.Controls.Add(this.DeleteFrameButton);
            this.Controls.Add(this.AddFramebutton);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.imageBoxEx1);
            this.Controls.Add(this.frameNumberLabel);
            this.Controls.Add(this.AnimationNameLabel);
            this.Controls.Add(this.panel1);
            this.Name = "FrameSelection";
            this.Size = new System.Drawing.Size(468, 341);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton ColliderRadioButton;
        private System.Windows.Forms.RadioButton AnimationRadioButton;
        private System.Windows.Forms.Label AnimationNameLabel;
        private System.Windows.Forms.Label frameNumberLabel;
        private System.Windows.Forms.Button setButton;
        private Cyotek.Windows.Forms.Demo.ImageBoxEx imageBoxEx1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button AddFramebutton;
        private System.Windows.Forms.Button DeleteFrameButton;
    }
}