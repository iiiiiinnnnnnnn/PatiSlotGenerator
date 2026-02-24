using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static PatiSlotGenerator.ProbabilityBarControl;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

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

        Dictionary<string, Image> map = new();

        private void AddReelSlot()
        {
            map.Clear();
            panel1.Controls.Clear();

            for (int i = 0; i < 10; i++)
            {
                PictureBox btn1 = new PictureBox();
                btn1.Text = "パレット" + (i + 1);
                btn1.Width = 120;
                btn1.Height = 120;
                btn1.ForeColor = Color.Black;
                btn1.BackColor = Color.White;
                btn1.Margin = new Padding(5);
                btn1.SizeMode = PictureBoxSizeMode.StretchImage;
                btn1.BorderStyle = BorderStyle.FixedSingle;

                btn1.Click += (s, e) =>
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "PNG files (*.png)|*.png";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        btn1.Image = Image.FromFile(ofd.FileName);
                        map[btn1.Text] = btn1.Image;
                    }
                };

                panel1.Controls.Add(btn1);
            }

            CreateReels();
        }

        List<PictureBox[]> reels = new();

        private void CreateReels()
        {
            int reelCount = 3;
            int rows = 21;
            int size = 100;
            int height = size - ((int)numericUpDown1.Value);

            panelReels.Controls.Clear();
            reels.Clear();

            for (int r = 0; r < reelCount; r++)
            {
                PictureBox[] reel = new PictureBox[rows];

                for (int i = 0; i < rows; i++)
                {
                    PictureBox pic = new PictureBox();
                    pic.Width = size;
                    pic.Height = height;

                    pic.Location = new Point(r * (size + 10), i * (height + 5));
                    pic.BorderStyle = BorderStyle.None;
                    pic.BackColor = Color.LightGray;
                    pic.Tag = ""; // 選ばれた役名を保持
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.ContextMenuStrip = CreateReelMenu(pic);

                    panelReels.Controls.Add(pic);
                    reel[i] = pic;
                }

                reels.Add(reel);
            }

            panelReels.AutoScroll = true;
        }

        private void ExportReels(bool combine)
        {
            int reelCount = 3;
            int rows = 21;
            int width = 100;
            int height = 100 - ((int)numericUpDown1.Value);

            if (combine)
            {
                // 横に3リール連結
                Bitmap bmp = new Bitmap(width * reelCount, rows * height);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);

                    for (int r = 0; r < reelCount; r++)
                    {
                        for (int i = 0; i < rows; i++)
                        {
                            var pic = reels[r][i];

                            if (pic.Image != null)
                            {
                                g.DrawImage(
                                    pic.Image,
                                    r * width,          // 横位置
                                    i * height,          // 縦位置
                                    width,
                                    height);
                            }
                        }
                    }
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PNG files (*.png)|*.png";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if (fbd.ShowDialog() != DialogResult.OK)
                    return;

                for (int r = 0; r < reelCount; r++)
                {
                    Bitmap bmp = new Bitmap(width, rows * height);

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.Transparent);

                        for (int i = 0; i < rows; i++)
                        {
                            var pic = reels[r][i];

                            if (pic.Image != null)
                            {
                                g.DrawImage(pic.Image, 0, i * height, width, height);
                            }
                        }
                    }

                    string path = Path.Combine(fbd.SelectedPath, $"Reel_{r + 1}.png");
                    bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                }

                MessageBox.Show("3リールを書き出しました");
            }
        }

        private void exportOne_Click(object sender, EventArgs e)
        {
            ExportReels(true);  // 1枚にまとめる
        }

        private void exportThree_Click(object sender, EventArgs e)
        {
            ExportReels(false); // 3枚に分ける
        }

        private ContextMenuStrip CreateReelMenu(PictureBox target)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            for (int i = 0; i < 10; i++)
            {
                var name = "パレット" + (i + 1);

                ToolStripMenuItem mi = new ToolStripMenuItem(name);

                mi.Click += (s, e) =>
                {
                    if (!map.ContainsKey(name))
                    {
                        MessageBox.Show("この役の画像が登録されていません");
                        return;
                    }

                    target.Image = map[name];
                    target.Tag = name;
                };

                menu.Items.Add(mi);
            }

            return menu;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddReelSlot();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnReset_Click(null, null);

            simTimer = new System.Windows.Forms.Timer();
            simTimer.Interval = 50; // 好きな速度に
            simTimer.Tick += SimTimer_Tick;

            gamecount.Text = "ゲーム数: 0";

            AddReelSlot();
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
            probabilityBar.Items.Clear();

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
                Name = "Melon",
                Percentage = 2.03f,
                Color = Color.GreenYellow
            });

            probabilityBar.Items.Add(new ProbabilityItem
            {
                Name = "Cherry",
                Percentage = 1.00f,
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
                Color = Color.AliceBlue
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
