namespace LTA_PGS_20
{
    partial class Form_new_schedule
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
            this.textBox_user_name = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_user_name
            // 
            this.textBox_user_name.Location = new System.Drawing.Point(248, 44);
            this.textBox_user_name.Name = "textBox_user_name";
            this.textBox_user_name.Size = new System.Drawing.Size(150, 20);
            this.textBox_user_name.TabIndex = 88;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(101, 47);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(86, 13);
            this.label24.TabIndex = 87;
            this.label24.Text = "Schedule Name:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(117, 104);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 89;
            this.button1.Text = "Confirm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(282, 104);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 90;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form_new_schedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 166);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_user_name);
            this.Controls.Add(this.label24);
            this.Name = "Form_new_schedule";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_new_schedule";
            this.Load += new System.EventHandler(this.Form_new_schedule_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_user_name;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}