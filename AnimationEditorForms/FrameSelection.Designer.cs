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
            this.panel2 = new System.Windows.Forms.Panel();
            this.AnimationTimingButton = new System.Windows.Forms.Button();
            this.finishSaveButton = new System.Windows.Forms.Button();
            this.setButton = new System.Windows.Forms.Button();
            this.ColliderRadioButton = new System.Windows.Forms.RadioButton();
            this.AnimationRadioButton = new System.Windows.Forms.RadioButton();
            this.AnimationNameLabel = new System.Windows.Forms.Label();
            this.frameNumberLabel = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.imageBoxEx1 = new Cyotek.Windows.Forms.ImageBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.setButton);
            this.panel1.Controls.Add(this.ColliderRadioButton);
            this.panel1.Controls.Add(this.AnimationRadioButton);
            this.panel1.Location = new System.Drawing.Point(0, 244);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(456, 97);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.AnimationTimingButton);
            this.panel2.Controls.Add(this.finishSaveButton);
            this.panel2.Location = new System.Drawing.Point(4, 64);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(452, 30);
            this.panel2.TabIndex = 4;
            // 
            // AnimationTimingButton
            // 
            this.AnimationTimingButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimationTimingButton.Location = new System.Drawing.Point(224, 0);
            this.AnimationTimingButton.Name = "AnimationTimingButton";
            this.AnimationTimingButton.Size = new System.Drawing.Size(225, 30);
            this.AnimationTimingButton.TabIndex = 1;
            this.AnimationTimingButton.Text = "Edit Animation Timings";
            this.AnimationTimingButton.UseVisualStyleBackColor = true;
            this.AnimationTimingButton.Click += new System.EventHandler(this.AnimationTimingButton_Click);
            // 
            // finishSaveButton
            // 
            this.finishSaveButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.finishSaveButton.Location = new System.Drawing.Point(0, 0);
            this.finishSaveButton.Name = "finishSaveButton";
            this.finishSaveButton.Size = new System.Drawing.Size(218, 30);
            this.finishSaveButton.TabIndex = 0;
            this.finishSaveButton.Text = "Finish && Save";
            this.finishSaveButton.UseVisualStyleBackColor = true;
            this.finishSaveButton.Click += new System.EventHandler(this.finishSaveButton_Click);
            // 
            // setButton
            // 
            this.setButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.setButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.setButton.Location = new System.Drawing.Point(4, 26);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(449, 32);
            this.setButton.TabIndex = 3;
            this.setButton.Text = "Set";
            this.setButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // ColliderRadioButton
            // 
            this.ColliderRadioButton.AutoSize = true;
            this.ColliderRadioButton.Location = new System.Drawing.Point(291, 3);
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
            this.AnimationRadioButton.Location = new System.Drawing.Point(16, 3);
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
            this.frameNumberLabel.Location = new System.Drawing.Point(195, 9);
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
            this.imageBoxEx1.Size = new System.Drawing.Size(438, 165);
            this.imageBoxEx1.TabIndex = 5;
            // 
            // FrameSelection
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.imageBoxEx1);
            this.Controls.Add(this.frameNumberLabel);
            this.Controls.Add(this.AnimationNameLabel);
            this.Controls.Add(this.panel1);
            this.Name = "FrameSelection";
            this.Size = new System.Drawing.Size(456, 341);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton ColliderRadioButton;
        private System.Windows.Forms.RadioButton AnimationRadioButton;
        private System.Windows.Forms.Button finishSaveButton;
        private System.Windows.Forms.Label AnimationNameLabel;
        private System.Windows.Forms.Label frameNumberLabel;
        private System.Windows.Forms.Button setButton;
        private Cyotek.Windows.Forms.ImageBox imageBoxEx1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button AnimationTimingButton;
        private System.Windows.Forms.TrackBar trackBar1;
    }
}