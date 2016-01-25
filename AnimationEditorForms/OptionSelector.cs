using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnimationEditorForms
{
    public partial class OptionSelector : Form
    {
        public string selectedOption;
        public event EventHandler onOptionChosen;
        public OptionSelector(List<string> options, string confirmMessage, bool topLevel = true)
        {
            InitializeComponent();
            this.AcceptButton.Text = confirmMessage;
            this.TopLevel = topLevel;
            if (topLevel == false)
            {
                this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                this.MinimizeBox = false;
                this.MaximizeBox = false;
                this.ShowIcon = false;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.FormBorderStyle = FormBorderStyle.None;
                
            }
            foreach (string optionName in options)
            {
                RadioButton button = new RadioButton();
                button.Text = optionName;
                flowLayoutPanel1.Controls.Add(button);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var checkedButton = flowLayoutPanel1.Controls.OfType<RadioButton>().Where(x => x.Checked).FirstOrDefault();
            if (checkedButton != null)
            {
                selectedOption = checkedButton.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                if (onOptionChosen != null)
                    onOptionChosen(checkedButton.Text, null);
            }
        }
    }
}
