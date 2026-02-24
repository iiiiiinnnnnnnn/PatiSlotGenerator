using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PatiSlotGenerator
{
    public partial class ProbabilityEditForm : Form
    {
        public string ItemName => textBox1.Text;
        public double Percentage => (double)numericUpDown1.Value;
        public Color SelectedColor { get; private set; }

        public ProbabilityEditForm()
        {
            InitializeComponent();
            pictureBox1.BackColor = SelectedColor;
        }

        public void SetData(string name, double percent, Color color)
        {
            textBox1.Text = name;
            numericUpDown1.Value = (decimal)percent;
            SelectedColor = color;
            pictureBox1.BackColor = color;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    SelectedColor = dlg.Color;
                    pictureBox1.BackColor = SelectedColor;
                }
            }
        }

        private void changed(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Trim();
        }
    }
}
