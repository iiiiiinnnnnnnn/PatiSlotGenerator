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
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.logo;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(377, 72);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // probabilityBar
            // 
            probabilityBar.BackColor = Color.White;
            probabilityBar.Location = new Point(12, 90);
            probabilityBar.Name = "probabilityBar";
            probabilityBar.Size = new Size(776, 94);
            probabilityBar.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 198);
            label1.Name = "label1";
            label1.Size = new Size(402, 135);
            label1.TabIndex = 4;
            label1.Text = resources.GetString("label1.Text");
            // 
            // btnbuild
            // 
            btnbuild.Font = new Font("Yu Gothic UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 128);
            btnbuild.Location = new Point(633, 198);
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
            btnload.Location = new Point(461, 198);
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
            textBox1.Location = new Point(395, 36);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(393, 39);
            textBox1.TabIndex = 8;
            textBox1.Text = "SlotTable_DefaultA";
            textBox1.TextChanged += tablenamechanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(395, 18);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 9;
            label2.Text = "テーブル名";
            // 
            // btnReset
            // 
            btnReset.Location = new Point(400, 198);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(49, 45);
            btnReset.TabIndex = 11;
            btnReset.Text = "リセット";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 346);
            Controls.Add(btnReset);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(btnload);
            Controls.Add(btnbuild);
            Controls.Add(label1);
            Controls.Add(probabilityBar);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "パチスロジェネレーター";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
    }
}
