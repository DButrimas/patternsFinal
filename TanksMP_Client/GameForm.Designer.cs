namespace TanksMP_Client
{
    partial class GameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.upBtn = new System.Windows.Forms.Button();
            this.downBtn = new System.Windows.Forms.Button();
            this.rightBtn = new System.Windows.Forms.Button();
            this.leftBtn = new System.Windows.Forms.Button();
            this.fireBtn = new System.Windows.Forms.Button();
            this.joinBtn = new System.Windows.Forms.Button();
            this.leaveBtn = new System.Windows.Forms.Button();
            this.gameLog = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.testBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // upBtn
            // 
            this.upBtn.Location = new System.Drawing.Point(587, 253);
            this.upBtn.Name = "upBtn";
            this.upBtn.Size = new System.Drawing.Size(50, 50);
            this.upBtn.TabIndex = 1;
            this.upBtn.Text = "UP";
            this.upBtn.UseVisualStyleBackColor = true;
            this.upBtn.Click += new System.EventHandler(this.upBtn_Click);
            // 
            // downBtn
            // 
            this.downBtn.Location = new System.Drawing.Point(587, 309);
            this.downBtn.Name = "downBtn";
            this.downBtn.Size = new System.Drawing.Size(50, 50);
            this.downBtn.TabIndex = 2;
            this.downBtn.Text = "DOWN";
            this.downBtn.UseVisualStyleBackColor = true;
            this.downBtn.Click += new System.EventHandler(this.downBtn_Click);
            // 
            // rightBtn
            // 
            this.rightBtn.Location = new System.Drawing.Point(643, 309);
            this.rightBtn.Name = "rightBtn";
            this.rightBtn.Size = new System.Drawing.Size(50, 50);
            this.rightBtn.TabIndex = 3;
            this.rightBtn.Text = "RIGHT";
            this.rightBtn.UseVisualStyleBackColor = true;
            this.rightBtn.Click += new System.EventHandler(this.rightBtn_Click);
            // 
            // leftBtn
            // 
            this.leftBtn.Location = new System.Drawing.Point(528, 309);
            this.leftBtn.Name = "leftBtn";
            this.leftBtn.Size = new System.Drawing.Size(50, 50);
            this.leftBtn.TabIndex = 4;
            this.leftBtn.Text = "LEFT";
            this.leftBtn.UseVisualStyleBackColor = true;
            this.leftBtn.Click += new System.EventHandler(this.leftBtn_Click);
            // 
            // fireBtn
            // 
            this.fireBtn.Location = new System.Drawing.Point(528, 366);
            this.fireBtn.Name = "fireBtn";
            this.fireBtn.Size = new System.Drawing.Size(165, 39);
            this.fireBtn.TabIndex = 5;
            this.fireBtn.Text = "FIRE!";
            this.fireBtn.UseVisualStyleBackColor = true;
            this.fireBtn.Click += new System.EventHandler(this.fireBtn_Click);
            // 
            // joinBtn
            // 
            this.joinBtn.Location = new System.Drawing.Point(439, 179);
            this.joinBtn.Name = "joinBtn";
            this.joinBtn.Size = new System.Drawing.Size(146, 49);
            this.joinBtn.TabIndex = 6;
            this.joinBtn.Text = "JOIN GAME";
            this.joinBtn.UseVisualStyleBackColor = true;
            this.joinBtn.Click += new System.EventHandler(this.joinBtn_Click);
            // 
            // leaveBtn
            // 
            this.leaveBtn.Location = new System.Drawing.Point(642, 179);
            this.leaveBtn.Name = "leaveBtn";
            this.leaveBtn.Size = new System.Drawing.Size(146, 49);
            this.leaveBtn.TabIndex = 7;
            this.leaveBtn.Text = "LEAVE GAME";
            this.leaveBtn.UseVisualStyleBackColor = true;
            this.leaveBtn.Click += new System.EventHandler(this.leaveBtn_Click);
            // 
            // gameLog
            // 
            this.gameLog.Location = new System.Drawing.Point(439, 12);
            this.gameLog.Name = "gameLog";
            this.gameLog.Size = new System.Drawing.Size(349, 122);
            this.gameLog.TabIndex = 8;
            this.gameLog.Text = "";
            this.gameLog.TextChanged += new System.EventHandler(this.gameLog_TextChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(13, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(420, 400);
            this.panel2.TabIndex = 9;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(439, 140);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(349, 20);
            this.textBox1.TabIndex = 10;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(727, 140);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 20);
            this.button1.TabIndex = 11;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // testBox1
            // 
            this.testBox1.Location = new System.Drawing.Point(794, 12);
            this.testBox1.Name = "testBox1";
            this.testBox1.Size = new System.Drawing.Size(294, 122);
            this.testBox1.TabIndex = 12;
            this.testBox1.Text = "";
            this.testBox1.TextChanged += new System.EventHandler(this.testBox1_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(948, 392);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Interpreter";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(818, 280);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "newPlayer";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(818, 366);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "restoreMemento";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(818, 321);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 16;
            this.button5.Text = "changePosX";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 427);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.testBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.gameLog);
            this.Controls.Add(this.leaveBtn);
            this.Controls.Add(this.joinBtn);
            this.Controls.Add(this.fireBtn);
            this.Controls.Add(this.leftBtn);
            this.Controls.Add(this.rightBtn);
            this.Controls.Add(this.downBtn);
            this.Controls.Add(this.upBtn);
            this.Name = "GameForm";
            this.Text = "GameForm";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button upBtn;
        private System.Windows.Forms.Button downBtn;
        private System.Windows.Forms.Button rightBtn;
        private System.Windows.Forms.Button leftBtn;
        private System.Windows.Forms.Button fireBtn;
        private System.Windows.Forms.Button joinBtn;
        private System.Windows.Forms.Button leaveBtn;
        private System.Windows.Forms.RichTextBox gameLog;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox testBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}