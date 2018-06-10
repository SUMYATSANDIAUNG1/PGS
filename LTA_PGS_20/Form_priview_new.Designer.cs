namespace LTA_PGS_20
{
    partial class Form_priview_new
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox80 = new System.Windows.Forms.GroupBox();
            this.richTextBox5 = new System.Windows.Forms.RichTextBox();
            this.richTextBox6 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.timer_toggle = new System.Windows.Forms.Timer(this.components);
            this.groupBox80.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox80
            // 
            this.groupBox80.Controls.Add(this.richTextBox5);
            this.groupBox80.Controls.Add(this.richTextBox6);
            this.groupBox80.Controls.Add(this.richTextBox3);
            this.groupBox80.Controls.Add(this.richTextBox4);
            this.groupBox80.Controls.Add(this.richTextBox2);
            this.groupBox80.Controls.Add(this.richTextBox1);
            this.groupBox80.Location = new System.Drawing.Point(12, 12);
            this.groupBox80.Name = "groupBox80";
            this.groupBox80.Size = new System.Drawing.Size(387, 335);
            this.groupBox80.TabIndex = 6;
            this.groupBox80.TabStop = false;
            this.groupBox80.Text = "Priview";
            // 
            // richTextBox5
            // 
            this.richTextBox5.BackColor = System.Drawing.SystemColors.WindowText;
            this.richTextBox5.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Bold);
            this.richTextBox5.ForeColor = System.Drawing.Color.Green;
            this.richTextBox5.Location = new System.Drawing.Point(6, 219);
            this.richTextBox5.Name = "richTextBox5";
            this.richTextBox5.ReadOnly = true;
            this.richTextBox5.Size = new System.Drawing.Size(368, 50);
            this.richTextBox5.TabIndex = 22;
            this.richTextBox5.Text = "";
            // 
            // richTextBox6
            // 
            this.richTextBox6.BackColor = System.Drawing.SystemColors.WindowText;
            this.richTextBox6.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Bold);
            this.richTextBox6.ForeColor = System.Drawing.Color.Green;
            this.richTextBox6.Location = new System.Drawing.Point(6, 269);
            this.richTextBox6.Name = "richTextBox6";
            this.richTextBox6.ReadOnly = true;
            this.richTextBox6.Size = new System.Drawing.Size(368, 50);
            this.richTextBox6.TabIndex = 21;
            this.richTextBox6.Text = "";
            // 
            // richTextBox3
            // 
            this.richTextBox3.BackColor = System.Drawing.SystemColors.WindowText;
            this.richTextBox3.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Bold);
            this.richTextBox3.ForeColor = System.Drawing.Color.Green;
            this.richTextBox3.Location = new System.Drawing.Point(6, 119);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.Size = new System.Drawing.Size(368, 50);
            this.richTextBox3.TabIndex = 20;
            this.richTextBox3.Text = "";
            // 
            // richTextBox4
            // 
            this.richTextBox4.BackColor = System.Drawing.SystemColors.WindowText;
            this.richTextBox4.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Bold);
            this.richTextBox4.ForeColor = System.Drawing.Color.Green;
            this.richTextBox4.Location = new System.Drawing.Point(7, 169);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.ReadOnly = true;
            this.richTextBox4.Size = new System.Drawing.Size(367, 50);
            this.richTextBox4.TabIndex = 19;
            this.richTextBox4.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.SystemColors.WindowText;
            this.richTextBox2.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Bold);
            this.richTextBox2.ForeColor = System.Drawing.Color.Green;
            this.richTextBox2.Location = new System.Drawing.Point(6, 69);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(368, 50);
            this.richTextBox2.TabIndex = 18;
            this.richTextBox2.Text = "";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.WindowText;
            this.richTextBox1.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Green;
            this.richTextBox1.Location = new System.Drawing.Point(6, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(368, 50);
            this.richTextBox1.TabIndex = 17;
            this.richTextBox1.Text = "";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(324, 361);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Exit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // timer_toggle
            // 
            this.timer_toggle.Interval = 1000;
            this.timer_toggle.Tick += new System.EventHandler(this.timer_toggle_Tick);
            // 
            // Form_priview_new
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 396);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox80);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(422, 430);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(422, 430);
            this.Name = "Form_priview_new";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form Priview";
            this.Load += new System.EventHandler(this.Form_priview_new_Load);
            this.groupBox80.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox80;
        private System.Windows.Forms.RichTextBox richTextBox5;
        private System.Windows.Forms.RichTextBox richTextBox6;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Timer timer_toggle;
    }
}