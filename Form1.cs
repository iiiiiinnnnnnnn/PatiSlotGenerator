using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static PatiSlotGenerator.ProbabilityBarControl;

namespace PatiSlotGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private System.Windows.Forms.Timer simTimer;
        private int gameCounter = 0;
        private Random random = new Random();

        private void Form1_Load(object sender, EventArgs e)
        {
            btnReset_Click(null, null);

            simTimer = new System.Windows.Forms.Timer();
            simTimer.Interval = 50; // 好きな速度に
            simTimer.Tick += SimTimer_Tick;

            gamecount.Text = "ゲーム数: 0";
        }

        private ProbabilityItem GetRandomResult()
        {
            double total = probabilityBar.Items.Sum(x => x.Percentage);
            double rand = random.NextDouble() * total;

            double current = 0;

            foreach (var item in probabilityBar.Items)
            {
                current += item.Percentage;
                if (rand <= current)
                    return item;
            }

            return probabilityBar.Items[0];
        }

        private void AppendLog(string text, Color backColor)
        {
            logtext.SelectionStart = logtext.TextLength;
            logtext.SelectionLength = 0;

            logtext.SelectionBackColor = backColor;
            logtext.SelectionColor = Color.Black; // 文字は黒固定

            logtext.AppendText(text + Environment.NewLine);

            logtext.SelectionBackColor = logtext.BackColor;
            logtext.ScrollToCaret();

            if (logtext.TextLength >= logtext.MaxLength - 100)
            {
                logtext.Clear();
            }
        }

        private void SimTimer_Tick(object sender, EventArgs e)
        {
            if (probabilityBar.Items.Count == 0)
                return;

            var result = GetRandomResult();

            gameCounter++;
            gamecount.Text = $"ゲーム数: {gameCounter}";

            AppendLog($"{gameCounter}G : {result.Name}", result.Color);
        }

        private void tablenamechanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Trim();
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            if (probabilityBar.Items.Count == 0)
            {
                MessageBox.Show("項目がありません");
                return;
            }

            double total = probabilityBar.Items.Sum(x => x.Percentage);
            if (Math.Abs(total - 100f) > 0.01f)
            {
                MessageBox.Show("確率合計は100%にしてください");
                return;
            }

            var sb = new StringBuilder();

            string tableName = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(tableName))
                tableName = "GachaTable";

            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using UdonSharp;");
            sb.AppendLine("using VRC.SDKBase;");
            sb.AppendLine("using VRC.Udon;");
            sb.AppendLine();

            // ===== ENUM =====
            sb.AppendLine($"    public enum {tableName}__ResultType");
            sb.AppendLine("    {");

            foreach (var item in probabilityBar.Items)
                sb.AppendLine($"        {item.Name},");

            sb.AppendLine("    }");
            sb.AppendLine();

            sb.AppendLine($"public class {tableName} : UdonSharpBehaviour");
            sb.AppendLine("{");

            // ===== 配列 =====
            sb.AppendLine($"    private {tableName}__ResultType[] results = new {tableName}__ResultType[]");
            sb.AppendLine("    {");

            foreach (var item in probabilityBar.Items)
                sb.AppendLine($"        {tableName}__ResultType.{item.Name},");

            sb.AppendLine("    };");
            sb.AppendLine();

            sb.AppendLine("    private float[] weights = new float[]");
            sb.AppendLine("    {");

            foreach (var item in probabilityBar.Items)
                sb.AppendLine($"        {item.Percentage}f,");

            sb.AppendLine("    };");
            sb.AppendLine();

            // ===== 抽選関数 =====
            sb.AppendLine($"    public {tableName}__ResultType GetRandom()");
            sb.AppendLine("    {");
            sb.AppendLine("        float total = 0f;");
            sb.AppendLine("        for (int i = 0; i < weights.Length; i++)");
            sb.AppendLine("            total += weights[i];");
            sb.AppendLine();
            sb.AppendLine("        float rand = Random.Range(0f, total);");
            sb.AppendLine("        float current = 0f;");
            sb.AppendLine();
            sb.AppendLine("        for (int i = 0; i < weights.Length; i++)");
            sb.AppendLine("        {");
            sb.AppendLine("            current += weights[i];");
            sb.AppendLine("            if (rand <= current)");
            sb.AppendLine("                return results[i];");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        return results[0];");
            sb.AppendLine("    }");
            sb.AppendLine();

            // ===== 色保存コメント =====
            sb.AppendLine("    // ===== Editor Color Data =====");
            sb.AppendLine("    // COLOR_DATA:");

            foreach (var item in probabilityBar.Items)
            {
                sb.AppendLine(
                    $"    // {item.Name}={item.Color.R},{item.Color.G},{item.Color.B}");
            }

            sb.AppendLine("}");

            Clipboard.SetText(sb.ToString());
            MessageBox.Show("Udon用テーブルクラスをコピーしました");
        }

        private List<float> ExtractWeights(string script)
        {
            var result = new List<float>();
            var lines = script.Split('\n');

            bool inWeights = false;

            foreach (var raw in lines)
            {
                var line = raw.Trim();

                if (line.StartsWith("private float[] weights"))
                {
                    inWeights = true;
                    continue;
                }

                if (inWeights && line.StartsWith("};"))
                    break;

                if (inWeights && line.EndsWith("f,"))
                {
                    string val = line.Replace("f,", "").Trim();
                    if (float.TryParse(val, out float parsed))
                        result.Add(parsed);
                }
            }

            return result;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string code = Clipboard.GetText();
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("スクリプトが空です");
                return;
            }

            probabilityBar.Items.Clear();

            var weights = ExtractWeights(code);
            int weightIndex = 0;

            var lines = code.Split('\n');
            bool colorSection = false;

            foreach (var raw in lines)
            {
                var line = raw.Trim();

                if (line.Contains("COLOR_DATA:"))
                {
                    colorSection = true;
                    continue;
                }

                if (!colorSection)
                    continue;

                if (!line.StartsWith("//"))
                    continue;

                line = line.Replace("//", "").Trim();

                if (!line.Contains("="))
                    continue;

                var split = line.Split('=');
                string name = split[0].Trim();

                var rgb = split[1].Split(',');

                if (rgb.Length != 3)
                    continue;

                int r = int.Parse(rgb[0]);
                int g = int.Parse(rgb[1]);
                int b = int.Parse(rgb[2]);

                float percent = 0f;
                if (weightIndex < weights.Count)
                    percent = weights[weightIndex];

                probabilityBar.Items.Add(new ProbabilityItem
                {
                    Name = name,
                    Percentage = percent,
                    Color = Color.FromArgb(r, g, b)
                });

                weightIndex++;
            }

            probabilityBar.Refresh();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "Lose",
                Percentage = 68.976f,
                Color = Color.Gray
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "Replay",
                Percentage = 13.7f,
                Color = Color.LightBlue
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "Bell",
                Percentage = 13.7f,
                Color = Color.Gold
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "Cherry",
                Percentage = 3.03f,
                Color = Color.Red
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "BIG",
                Percentage = 0.366f,
                Color = Color.Magenta
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "REG",
                Percentage = 0.228f,
                Color = Color.Blue
            });

            probabilityBar.Invalidate();
        }

        private void btnsimulate_Click(object sender, EventArgs e)
        {
            if (probabilityBar.Items.Count == 0)
            {
                MessageBox.Show("項目がありません");
                return;
            }

            simTimer.Start();
        }

        private void btnstop_Click(object sender, EventArgs e)
        {
            simTimer.Stop();
        }

        private void btnend_Click(object sender, EventArgs e)
        {
            simTimer.Stop();
            gameCounter = 0;
            gamecount.Text = "ゲーム数: 0";
            logtext.Clear();
        }
    }
}
