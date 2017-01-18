using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            AcceptButton.Text = confirmMessage;
            TopLevel = topLevel;
            if (topLevel == false)
            {
                AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                MinimizeBox = false;
                MaximizeBox = false;
                ShowIcon = false;
                StartPosition = FormStartPosition.CenterScreen;
                FormBorderStyle = FormBorderStyle.None;
                
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
                DialogResult = System.Windows.Forms.DialogResult.OK;

                if (onOptionChosen != null)
                    onOptionChosen(checkedButton.Text, null);
            }
        }
    }
}
