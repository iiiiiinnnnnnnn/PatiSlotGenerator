using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatiSlotGenerator
{
    public partial class ProbabilityBarControl : UserControl
    {
        public class ProbabilityItem
        {
            public string Name;
            public double Percentage; // 0〜100
            public Color Color;
        }

        public List<ProbabilityItem> Items = new List<ProbabilityItem>();
        private int draggingIndex = -1;   // どの境界を掴んでるか
        private const int GripWidth = 6;  // 境界の判定幅
        private ContextMenuStrip menu;
        private int clickedIndex = -1;

        private int GetBoundaryIndex(int mouseX)
        {
            double currentX = 0;

            for (int i = 0; i < Items.Count - 1; i++)
            {
                currentX += Width * (Items[i].Percentage / 100.0);

                if (Math.Abs(mouseX - currentX) <= GripWidth)
                    return i;
            }

            return -1;
        }

        private int GetItemIndex(int mouseX)
        {
            double currentX = 0;

            for (int i = 0; i < Items.Count; i++)
            {
                double w = Width * (Items[i].Percentage / 100.0);

                if (mouseX >= currentX && mouseX <= currentX + w)
                    return i;

                currentX += w;
            }

            return -1;
        }

        private void OnAdd(object sender, EventArgs e)
        {
            using (var dlg = new ProbabilityEditForm())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Items.Add(new ProbabilityItem
                    {
                        Name = dlg.ItemName,
                        Percentage = dlg.Percentage,
                        Color = dlg.SelectedColor
                    });

                    Normalize();
                    Invalidate();
                }
            }
        }

        private void OnEdit(object sender, EventArgs e)
        {
            if (clickedIndex < 0) return;

            var item = Items[clickedIndex];

            using (var dlg = new ProbabilityEditForm())
            {
                dlg.SetData(item.Name, item.Percentage, item.Color);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    item.Name = dlg.ItemName;
                    item.Percentage = dlg.Percentage;
                    item.Color = dlg.SelectedColor;

                    Normalize();
                    Invalidate();
                }
            }
        }

        private void OnDelete(object sender, EventArgs e)
        {
            if (clickedIndex < 0) return;

            Items.RemoveAt(clickedIndex);
            Normalize();
            Invalidate();
        }

        private void OnMoveLeft(object sender, EventArgs e)
        {
            if (clickedIndex <= 0) return;

            var temp = Items[clickedIndex];
            Items[clickedIndex] = Items[clickedIndex - 1];
            Items[clickedIndex - 1] = temp;

            clickedIndex--;
            Invalidate();
        }

        private void OnMoveRight(object sender, EventArgs e)
        {
            if (clickedIndex < 0 || clickedIndex >= Items.Count - 1) return;

            var temp = Items[clickedIndex];
            Items[clickedIndex] = Items[clickedIndex + 1];
            Items[clickedIndex + 1] = temp;

            clickedIndex++;
            Invalidate();
        }

        private void Normalize()
        {
            double total = Items.Sum(i => i.Percentage);
            if (total == 0) return;

            foreach (var item in Items)
                item.Percentage = item.Percentage / total * 100.0;
        }

        public ProbabilityBarControl()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw,
                          true);

            this.UpdateStyles();
            this.BackColor = Color.White;

            menu = new ContextMenuStrip();

            menu.Items.Add("追加", null, OnAdd);
            menu.Items.Add("編集", null, OnEdit);
            menu.Items.Add("削除", null, OnDelete);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("左に移動", null, OnMoveLeft);
            menu.Items.Add("右に移動", null, OnMoveRight);

            this.ContextMenuStrip = menu;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            base.OnPaint(e);

            if (Items.Count == 0) return;

            double totalWidth = Width;
            double currentX = 0;

            foreach (var item in Items)
            {
                double w = totalWidth * (item.Percentage / 100.0);

                Rectangle rect = new Rectangle(
                    (int)currentX,
                    0,
                    (int)w,
                    Height);

                // 塗る
                using (Brush b = new SolidBrush(item.Color))
                    e.Graphics.FillRectangle(b, rect);

                e.Graphics.DrawRectangle(Pens.Black, rect);

                // 🔥 ここから文字描画
                string text = $"{item.Name}\n{item.Percentage:F1}%";

                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    using (Brush textBrush = new SolidBrush(Color.Black))
                    {
                        e.Graphics.DrawString(
                            text,
                            this.Font,
                            textBrush,
                            rect,
                            sf);
                    }
                }

                currentX += w;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Right)
            {
                clickedIndex = GetItemIndex(e.X);
            }
            else if (e.Button == MouseButtons.Left)
            {
                draggingIndex = GetBoundaryIndex(e.X);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            draggingIndex = -1;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (draggingIndex < 0) return;

            double totalWidth = Width;

            // 現在の境界の実Xを計算
            double currentX = 0;
            for (int i = 0; i <= draggingIndex; i++)
                currentX += totalWidth * (Items[i].Percentage / 100.0);

            double deltaX = e.X - currentX;
            double deltaPercent = (deltaX / totalWidth) * 100.0;

            var left = Items[draggingIndex];
            var right = Items[draggingIndex + 1];

            double newLeft = left.Percentage + deltaPercent;
            double newRight = right.Percentage - deltaPercent;

            // 0%未満禁止
            if (newLeft < 0 || newRight < 0) return;

            left.Percentage = newLeft;
            right.Percentage = newRight;

            Invalidate();
        }
    }
}
