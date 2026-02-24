using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using static PatiSlotGenerator.ProbabilityBarControl;

namespace PatiSlotGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnReset_Click(null, null);
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

            sb.AppendLine($"public class {tableName} : UdonSharpBehaviour");
            sb.AppendLine("{");

            // ===== ENUM =====
            sb.AppendLine("    public enum ResultType");
            sb.AppendLine("    {");

            foreach (var item in probabilityBar.Items)
                sb.AppendLine($"        {item.Name},");

            sb.AppendLine("    }");
            sb.AppendLine();

            // ===== 配列 =====
            sb.AppendLine("    [SerializeField] private ResultType[] results = new ResultType[]");
            sb.AppendLine("    {");

            foreach (var item in probabilityBar.Items)
                sb.AppendLine($"        ResultType.{item.Name},");

            sb.AppendLine("    };");
            sb.AppendLine();

            sb.AppendLine("    [SerializeField] private float[] weights = new float[]");
            sb.AppendLine("    {");

            foreach (var item in probabilityBar.Items)
                sb.AppendLine($"        {item.Percentage}f,");

            sb.AppendLine("    };");
            sb.AppendLine();

            // ===== 抽選関数 =====
            sb.AppendLine("    public ResultType GetRandom()");
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

        private float ExtractPercentFromScript(string script, string enumName)
        {
            var lines = script.Split('\n');

            bool inResults = false;
            int index = -1;
            int currentIndex = 0;

            foreach (var raw in lines)
            {
                var line = raw.Trim();

                if (line.StartsWith("[SerializeField] private ResultType[]"))
                {
                    inResults = true;
                    continue;
                }

                if (inResults && line.StartsWith("};"))
                    inResults = false;

                if (inResults && line.Contains("ResultType."))
                {
                    if (line.Contains(enumName))
                    {
                        index = currentIndex;
                        break;
                    }
                    currentIndex++;
                }
            }

            if (index == -1)
                return 0f;

            bool inWeights = false;
            int weightIndex = 0;

            foreach (var raw in lines)
            {
                var line = raw.Trim();

                if (line.StartsWith("[SerializeField] private float[] weights"))
                {
                    inWeights = true;
                    continue;
                }

                if (inWeights && line.StartsWith("};"))
                    break;

                if (inWeights && line.EndsWith("f,"))
                {
                    if (weightIndex == index)
                    {
                        string val = line.Replace("f,", "");
                        return float.Parse(val);
                    }
                    weightIndex++;
                }
            }

            return 0f;
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

                // 確率はweights配列から取る
                float percent = ExtractPercentFromScript(code, name);

                probabilityBar.Items.Add(new ProbabilityItem
                {
                    Name = name,
                    Percentage = percent,
                    Color = Color.FromArgb(r, g, b)
                });
            }

            probabilityBar.Refresh();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            probabilityBar.Items.Clear();

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "Lose",
                Percentage = 48,
                Color = Color.Gray
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "Replay",
                Percentage = 18,
                Color = Color.LightBlue
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "Bell",
                Percentage = 25,
                Color = Color.Gold
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "Cherry",
                Percentage = 5,
                Color = Color.Red
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "Melon",
                Percentage = 3.5f,
                Color = Color.Green
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "Bonus",
                Percentage = 0.5f,
                Color = Color.Magenta
            });

            probabilityBar.Invalidate();
        }
    }
}
