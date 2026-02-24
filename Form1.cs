using System.ComponentModel;
using static PatiSlotGenerator.ProbabilityBarControl;

namespace PatiSlotGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private ProbabilityBarControl probabilityBar;

        private void Form1_Load(object sender, EventArgs e)
        {
            probabilityBar = new ProbabilityBarControl();
            probabilityBar.Dock = DockStyle.Top;
            probabilityBar.Height = 60;

            probabilityBar.Items.Clear();

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "ハズレ",
                Percentage = 45,
                Color = Color.Gray
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "リプレイ",
                Percentage = 18,
                Color = Color.LightBlue
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "ベル",
                Percentage = 25,
                Color = Color.Gold
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "チェリー",
                Percentage = 7,
                Color = Color.Red
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "スイカ",
                Percentage = 4,
                Color = Color.Green
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "ボーナス",
                Percentage = 1,
                Color = Color.Magenta
            });

            probabilityBar.Invalidate();

            this.Controls.Add(probabilityBar);
        }
    }
}
