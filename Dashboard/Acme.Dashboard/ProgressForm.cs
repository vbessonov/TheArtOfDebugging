using System;
using System.Windows.Forms;

namespace Acme.Dashboard
{
    public partial class ProgressForm : Form
    {
        public string Label
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        public ProgressForm(string label)
        {
            if (string.IsNullOrEmpty(label))
            {
                throw new ArgumentException("Label must be non-empty");
            }

            InitializeComponent();

            Label = label;
        }
    }
}
