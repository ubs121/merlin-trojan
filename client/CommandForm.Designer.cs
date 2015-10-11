namespace client {
    partial class CommandForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.textReceiveBox = new System.Windows.Forms.TextBox();
            this.textMsg = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.textMerlinHost = new System.Windows.Forms.TextBox();
            this.menuCmdInsert = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.playInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.moveInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.getFileInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.showHideInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain.SuspendLayout();
            this.menuCmdInsert.SuspendLayout();
            this.SuspendLayout();
            // 
            // textReceiveBox
            // 
            this.textReceiveBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textReceiveBox.Location = new System.Drawing.Point(12, 68);
            this.textReceiveBox.Multiline = true;
            this.textReceiveBox.Name = "textReceiveBox";
            this.textReceiveBox.Size = new System.Drawing.Size(584, 286);
            this.textReceiveBox.TabIndex = 0;
            // 
            // textMsg
            // 
            this.textMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textMsg.Location = new System.Drawing.Point(12, 361);
            this.textMsg.Name = "textMsg";
            this.textMsg.Size = new System.Drawing.Size(485, 20);
            this.textMsg.TabIndex = 1;
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Location = new System.Drawing.Point(504, 359);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(608, 24);
            this.menuMain.TabIndex = 3;
            this.menuMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Merlin Host:";
            // 
            // textMerlinHost
            // 
            this.textMerlinHost.Location = new System.Drawing.Point(108, 37);
            this.textMerlinHost.Name = "textMerlinHost";
            this.textMerlinHost.Size = new System.Drawing.Size(124, 20);
            this.textMerlinHost.TabIndex = 5;
            this.textMerlinHost.Text = "192.168.0.208";
            // 
            // menuCmdInsert
            // 
            this.menuCmdInsert.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playInsert,
            this.moveInsert,
            this.showHideInsert,
            this.getFileInsert});
            this.menuCmdInsert.Name = "menuCmdInsert";
            this.menuCmdInsert.Size = new System.Drawing.Size(153, 114);
            // 
            // playInsert
            // 
            this.playInsert.Name = "playInsert";
            this.playInsert.Size = new System.Drawing.Size(152, 22);
            this.playInsert.Text = "&Play";
            this.playInsert.Click += new System.EventHandler(this.playInsert_Click);
            // 
            // moveInsert
            // 
            this.moveInsert.Name = "moveInsert";
            this.moveInsert.Size = new System.Drawing.Size(152, 22);
            this.moveInsert.Text = "&Move";
            // 
            // getFileInsert
            // 
            this.getFileInsert.Name = "getFileInsert";
            this.getFileInsert.Size = new System.Drawing.Size(152, 22);
            this.getFileInsert.Text = "Get &File";
            // 
            // showHideInsert
            // 
            this.showHideInsert.Name = "showHideInsert";
            this.showHideInsert.Size = new System.Drawing.Size(152, 22);
            this.showHideInsert.Text = "Show/Hide";
            // 
            // CommandForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 393);
            this.Controls.Add(this.textMerlinHost);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textMsg);
            this.Controls.Add(this.textReceiveBox);
            this.Controls.Add(this.menuMain);
            this.MainMenuStrip = this.menuMain;
            this.Name = "CommandForm";
            this.Text = "Merlin Client";
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.menuCmdInsert.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textReceiveBox;
        private System.Windows.Forms.TextBox textMsg;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textMerlinHost;
        private System.Windows.Forms.ContextMenuStrip menuCmdInsert;
        private System.Windows.Forms.ToolStripMenuItem playInsert;
        private System.Windows.Forms.ToolStripMenuItem moveInsert;
        private System.Windows.Forms.ToolStripMenuItem showHideInsert;
        private System.Windows.Forms.ToolStripMenuItem getFileInsert;
    }
}

