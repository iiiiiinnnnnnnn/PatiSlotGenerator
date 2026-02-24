namespace PatiSlotGenerator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pictureBox1 = new PictureBox();
            probabilityBar = new ProbabilityBarControl();
            label1 = new Label();
            btnbuild = new Button();
            btnload = new Button();
            textBox1 = new TextBox();
            label2 = new Label();
            btnReset = new Button();
            btnsimulate = new Button();
            btnstop = new Button();
            gamecount = new Label();
            btnend = new Button();
            logtext = new RichTextBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            label4 = new Label();
            numericUpDown1 = new NumericUpDown();
            exportOne = new Button();
            exportThree = new Button();
            label3 = new Label();
            panelReels = new Panel();
            panel1 = new FlowLayoutPanel();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.logo;
            pictureBox1.Location = new Point(15, 16);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(377, 72);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // probabilityBar
            // 
            probabilityBar.BackColor = Color.White;
            probabilityBar.Location = new Point(15, 94);
            probabilityBar.Name = "probabilityBar";
            probabilityBar.Size = new Size(776, 94);
            probabilityBar.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 202);
            label1.Name = "label1";
            label1.Size = new Size(402, 135);
            label1.TabIndex = 4;
            label1.Text = resources.GetString("label1.Text");
            // 
            // btnbuild
            // 
            btnbuild.Font = new Font("Yu Gothic UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnbuild.Location = new Point(636, 202);
            btnbuild.Name = "btnbuild";
            btnbuild.Size = new Size(155, 136);
            btnbuild.TabIndex = 6;
            btnbuild.Text = "UdonC#\r\n出力";
            btnbuild.UseVisualStyleBackColor = true;
            btnbuild.Click += btnBuild_Click;
            // 
            // btnload
            // 
            btnload.Font = new Font("Yu Gothic UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnload.Location = new Point(475, 202);
            btnload.Name = "btnload";
            btnload.Size = new Size(155, 136);
            btnload.TabIndex = 7;
            btnload.Text = "UdonC#\r\n読込";
            btnload.UseVisualStyleBackColor = true;
            btnload.Click += btnLoad_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 128);
            textBox1.Location = new Point(407, 40);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(384, 39);
            textBox1.TabIndex = 8;
            textBox1.Text = "SlotTable_DefaultA";
            textBox1.TextChanged += tablenamechanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(407, 22);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 9;
            label2.Text = "テーブル名";
            // 
            // btnReset
            // 
            btnReset.Location = new Point(423, 202);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(46, 135);
            btnReset.TabIndex = 11;
            btnReset.Text = "テーブル初期化";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnsimulate
            // 
            btnsimulate.Location = new Point(797, 40);
            btnsimulate.Name = "btnsimulate";
            btnsimulate.Size = new Size(72, 29);
            btnsimulate.TabIndex = 12;
            btnsimulate.Text = "シミュレート";
            btnsimulate.UseVisualStyleBackColor = true;
            btnsimulate.Click += btnsimulate_Click;
            // 
            // btnstop
            // 
            btnstop.Location = new Point(875, 40);
            btnstop.Name = "btnstop";
            btnstop.Size = new Size(28, 29);
            btnstop.TabIndex = 13;
            btnstop.Text = "停";
            btnstop.UseVisualStyleBackColor = true;
            btnstop.Click += btnstop_Click;
            // 
            // gamecount
            // 
            gamecount.AutoSize = true;
            gamecount.Location = new Point(797, 72);
            gamecount.Name = "gamecount";
            gamecount.Size = new Size(58, 15);
            gamecount.TabIndex = 15;
            gamecount.Text = "ゲーム数: 0";
            // 
            // btnend
            // 
            btnend.Location = new Point(901, 40);
            btnend.Name = "btnend";
            btnend.Size = new Size(28, 29);
            btnend.TabIndex = 16;
            btnend.Text = "終";
            btnend.UseVisualStyleBackColor = true;
            btnend.Click += btnend_Click;
            // 
            // logtext
            // 
            logtext.Location = new Point(797, 94);
            logtext.Name = "logtext";
            logtext.ReadOnly = true;
            logtext.ScrollBars = RichTextBoxScrollBars.Vertical;
            logtext.Size = new Size(132, 243);
            logtext.TabIndex = 17;
            logtext.Text = "";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(954, 383);
            tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(pictureBox1);
            tabPage1.Controls.Add(logtext);
            tabPage1.Controls.Add(probabilityBar);
            tabPage1.Controls.Add(btnend);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(gamecount);
            tabPage1.Controls.Add(btnbuild);
            tabPage1.Controls.Add(btnstop);
            tabPage1.Controls.Add(btnload);
            tabPage1.Controls.Add(btnsimulate);
            tabPage1.Controls.Add(textBox1);
            tabPage1.Controls.Add(btnReset);
            tabPage1.Controls.Add(label2);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(946, 355);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "排出確率";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(numericUpDown1);
            tabPage2.Controls.Add(exportOne);
            tabPage2.Controls.Add(exportThree);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(panelReels);
            tabPage2.Controls.Add(panel1);
            tabPage2.Controls.Add(button1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(946, 355);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "リール配列";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(662, 141);
            label4.Name = "label4";
            label4.Size = new Size(244, 30);
            label4.TabIndex = 10;
            label4.Text = "＊縦比率補正(変えた後リセット押す必要あるので\r\n　最初に調整してください)";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(662, 174);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(278, 23);
            numericUpDown1.TabIndex = 9;
            numericUpDown1.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // exportOne
            // 
            exportOne.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            exportOne.Location = new Point(662, 235);
            exportOne.Name = "exportOne";
            exportOne.Size = new Size(136, 114);
            exportOne.TabIndex = 8;
            exportOne.Text = "3つのリールを1つの画像として書出";
            exportOne.UseVisualStyleBackColor = true;
            exportOne.Click += exportOne_Click;
            // 
            // exportThree
            // 
            exportThree.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            exportThree.Location = new Point(804, 235);
            exportThree.Name = "exportThree";
            exportThree.Size = new Size(136, 114);
            exportThree.TabIndex = 7;
            exportThree.Text = "3つのリールを3つの画像として書出";
            exportThree.UseVisualStyleBackColor = true;
            exportThree.Click += exportThree_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(662, 9);
            label3.Name = "label3";
            label3.Size = new Size(280, 90);
            label3.TabIndex = 4;
            label3.Text = "1. 左のパレットに役物の画像を登録してください\r\n　パレットは10個あります(10個あれば十分だよね★)\r\n\r\n2. リール配列を右クリックしそれぞれ役物を設定してください\r\n\r\n3. 下のボタンでリール配列画像を書き出してください";
            // 
            // panelReels
            // 
            panelReels.Location = new Point(311, 9);
            panelReels.Name = "panelReels";
            panelReels.Size = new Size(345, 340);
            panelReels.TabIndex = 3;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Location = new Point(6, 52);
            panel1.Name = "panel1";
            panel1.Size = new Size(299, 297);
            panel1.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new Point(6, 9);
            button1.Name = "button1";
            button1.Size = new Size(299, 37);
            button1.TabIndex = 1;
            button1.Text = "リセット";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(978, 401);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "パチスロジェネレーター";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private PictureBox pictureBox1;
        private ProbabilityBarControl probabilityBar;
        private Label label1;
        private Button btnbuild;
        private Button btnload;
        private TextBox textBox1;
        private Label label2;
        private Button btnReset;
        private Button btnsimulate;
        private Button btnstop;
        private Label gamecount;
        private Button btnend;
        private RichTextBox logtext;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button button1;
        private FlowLayoutPanel panel1;
        private Panel panelReels;
        private Label label3;
        private Button exportOne;
        private Button exportThree;
        private Label label4;
        private NumericUpDown numericUpDown1;
    }
}
