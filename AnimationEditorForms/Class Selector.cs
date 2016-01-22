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
    public partial class Class_Selector : Form
    {
        List<string> classes;
        public string selectedClass;
        public Class_Selector(List<string> classOptions)
        {
            InitializeComponent();
            classes = classOptions.ToList();
            foreach (string className in classOptions)
            {
                RadioButton button = new RadioButton();
                button.Text = className;
                flowLayoutPanel1.Controls.Add(button);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var checkedButton = flowLayoutPanel1.Controls.OfType<RadioButton>().Where(x => x.Checked).FirstOrDefault();
            if (checkedButton != null)
                selectedClass = checkedButton.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
