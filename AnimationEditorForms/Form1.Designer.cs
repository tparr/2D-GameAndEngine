namespace AnimationEditorForms
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.OpenAnimationButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.NewAnimationButton = new System.Windows.Forms.Button();
            this.animationPanel = new System.Windows.Forms.Panel();
            this.selectedClassLabel = new System.Windows.Forms.Label();
            this.ChangeClassButton = new System.Windows.Forms.Button();
            this.frameSelectionPanel = new System.Windows.Forms.Panel();
            this.timingPanel = new System.Windows.Forms.Panel();
            this.AddAnimationButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1354, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xml";
            this.openFileDialog1.Filter = "Animation|*.xml";
            // 
            // OpenAnimationButton
            // 
            this.OpenAnimationButton.Location = new System.Drawing.Point(184, 3);
            this.OpenAnimationButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OpenAnimationButton.Name = "OpenAnimationButton";
            this.OpenAnimationButton.Size = new System.Drawing.Size(174, 35);
            this.OpenAnimationButton.TabIndex = 1;
            this.OpenAnimationButton.Text = "Open Animation File";
            this.OpenAnimationButton.UseVisualStyleBackColor = true;
            this.OpenAnimationButton.Click += new System.EventHandler(this.OpenAnimationButtonClicked);
            // 
            // SaveButton
            // 
            this.SaveButton.Enabled = false;
            this.SaveButton.Location = new System.Drawing.Point(368, 3);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(158, 35);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save Animation File";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButtonClicked);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xml";
            this.saveFileDialog1.Filter = "Xml files|*.xml";
            // 
            // NewAnimationButton
            // 
            this.NewAnimationButton.Location = new System.Drawing.Point(18, 3);
            this.NewAnimationButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NewAnimationButton.Name = "NewAnimationButton";
            this.NewAnimationButton.Size = new System.Drawing.Size(158, 35);
            this.NewAnimationButton.TabIndex = 8;
            this.NewAnimationButton.Text = "New Animation File";
            this.NewAnimationButton.UseVisualStyleBackColor = true;
            this.NewAnimationButton.Click += new System.EventHandler(this.NewButtonClicked);
            // 
            // animationPanel
            // 
            this.animationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.animationPanel.AutoScroll = true;
            this.animationPanel.AutoSize = true;
            this.animationPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.animationPanel.Location = new System.Drawing.Point(18, 80);
            this.animationPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.animationPanel.Name = "animationPanel";
            this.animationPanel.Size = new System.Drawing.Size(324, 612);
            this.animationPanel.TabIndex = 17;
            // 
            // selectedClassLabel
            // 
            this.selectedClassLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.selectedClassLabel.AutoSize = true;
            this.selectedClassLabel.Location = new System.Drawing.Point(662, 49);
            this.selectedClassLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.selectedClassLabel.Name = "selectedClassLabel";
            this.selectedClassLabel.Size = new System.Drawing.Size(0, 20);
            this.selectedClassLabel.TabIndex = 0;
            // 
            // ChangeClassButton
            // 
            this.ChangeClassButton.Enabled = false;
            this.ChangeClassButton.Location = new System.Drawing.Point(18, 42);
            this.ChangeClassButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ChangeClassButton.Name = "ChangeClassButton";
            this.ChangeClassButton.Size = new System.Drawing.Size(158, 35);
            this.ChangeClassButton.TabIndex = 18;
            this.ChangeClassButton.Text = "Change Class";
            this.ChangeClassButton.UseVisualStyleBackColor = true;
            this.ChangeClassButton.Visible = false;
            this.ChangeClassButton.Click += new System.EventHandler(this.ChangeAnimationButton_Click);
            // 
            // frameSelectionPanel
            // 
            this.frameSelectionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.frameSelectionPanel.AutoScroll = true;
            this.frameSelectionPanel.AutoSize = true;
            this.frameSelectionPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.frameSelectionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.frameSelectionPanel.Location = new System.Drawing.Point(351, 80);
            this.frameSelectionPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.frameSelectionPanel.Name = "frameSelectionPanel";
            this.frameSelectionPanel.Size = new System.Drawing.Size(683, 611);
            this.frameSelectionPanel.TabIndex = 18;
            // 
            // timingPanel
            // 
            this.timingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timingPanel.AutoScroll = true;
            this.timingPanel.AutoSize = true;
            this.timingPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.timingPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.timingPanel.Location = new System.Drawing.Point(1044, 80);
            this.timingPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.timingPanel.Name = "timingPanel";
            this.timingPanel.Size = new System.Drawing.Size(418, 611);
            this.timingPanel.TabIndex = 18;
            // 
            // AddAnimationButton
            // 
            this.AddAnimationButton.Enabled = false;
            this.AddAnimationButton.Location = new System.Drawing.Point(184, 42);
            this.AddAnimationButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AddAnimationButton.Name = "AddAnimationButton";
            this.AddAnimationButton.Size = new System.Drawing.Size(158, 35);
            this.AddAnimationButton.TabIndex = 19;
            this.AddAnimationButton.Text = "Add Animation";
            this.AddAnimationButton.UseVisualStyleBackColor = true;
            this.AddAnimationButton.Visible = false;
            this.AddAnimationButton.Click += new System.EventHandler(this.AddAnimationButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 711);
            this.Controls.Add(this.AddAnimationButton);
            this.Controls.Add(this.timingPanel);
            this.Controls.Add(this.frameSelectionPanel);
            this.Controls.Add(this.ChangeClassButton);
            this.Controls.Add(this.selectedClassLabel);
            this.Controls.Add(this.animationPanel);
            this.Controls.Add(this.NewAnimationButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.OpenAnimationButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Animation Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button OpenAnimationButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button NewAnimationButton;
        private System.Windows.Forms.Panel animationPanel;
        private System.Windows.Forms.Label selectedClassLabel;
        private System.Windows.Forms.Button ChangeClassButton;
        private System.Windows.Forms.Panel frameSelectionPanel;
        private System.Windows.Forms.Panel timingPanel;
        private System.Windows.Forms.Button AddAnimationButton;
    }
}
