namespace HookTest
{
	partial class Form1
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.textBoxLog = new System.Windows.Forms.TextBox();
			this.buttonLog = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxMouse = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxKeyboard = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxHandleCount = new System.Windows.Forms.TextBox();
			this.textBoxLogSize = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// textBoxLog
			// 
			this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxLog.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.textBoxLog.Location = new System.Drawing.Point(12, 51);
			this.textBoxLog.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxLog.MaxLength = 1024;
			this.textBoxLog.Multiline = true;
			this.textBoxLog.Name = "textBoxLog";
			this.textBoxLog.ReadOnly = true;
			this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxLog.Size = new System.Drawing.Size(873, 237);
			this.textBoxLog.TabIndex = 0;
			// 
			// buttonLog
			// 
			this.buttonLog.Location = new System.Drawing.Point(12, 12);
			this.buttonLog.Name = "buttonLog";
			this.buttonLog.Size = new System.Drawing.Size(145, 29);
			this.buttonLog.TabIndex = 3;
			this.buttonLog.Text = "記録開始";
			this.buttonLog.UseVisualStyleBackColor = true;
			this.buttonLog.Click += new System.EventHandler(this.buttonLog_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(172, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 15);
			this.label1.TabIndex = 4;
			this.label1.Text = "マウス";
			// 
			// textBoxMouse
			// 
			this.textBoxMouse.Location = new System.Drawing.Point(218, 16);
			this.textBoxMouse.Name = "textBoxMouse";
			this.textBoxMouse.ReadOnly = true;
			this.textBoxMouse.Size = new System.Drawing.Size(98, 22);
			this.textBoxMouse.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(334, 19);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 15);
			this.label2.TabIndex = 6;
			this.label2.Text = "キーボード";
			// 
			// textBoxKeyboard
			// 
			this.textBoxKeyboard.Location = new System.Drawing.Point(406, 16);
			this.textBoxKeyboard.Name = "textBoxKeyboard";
			this.textBoxKeyboard.ReadOnly = true;
			this.textBoxKeyboard.Size = new System.Drawing.Size(100, 22);
			this.textBoxKeyboard.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(529, 19);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(69, 15);
			this.label3.TabIndex = 8;
			this.label3.Text = "ハンドル数";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(704, 18);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(65, 15);
			this.label4.TabIndex = 9;
			this.label4.Text = "ログサイズ";
			// 
			// textBoxHandleCount
			// 
			this.textBoxHandleCount.Location = new System.Drawing.Point(604, 16);
			this.textBoxHandleCount.Name = "textBoxHandleCount";
			this.textBoxHandleCount.ReadOnly = true;
			this.textBoxHandleCount.Size = new System.Drawing.Size(78, 22);
			this.textBoxHandleCount.TabIndex = 10;
			// 
			// textBoxLogSize
			// 
			this.textBoxLogSize.Location = new System.Drawing.Point(775, 15);
			this.textBoxLogSize.Name = "textBoxLogSize";
			this.textBoxLogSize.ReadOnly = true;
			this.textBoxLogSize.Size = new System.Drawing.Size(100, 22);
			this.textBoxLogSize.TabIndex = 11;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(898, 304);
			this.Controls.Add(this.textBoxLogSize);
			this.Controls.Add(this.textBoxHandleCount);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBoxKeyboard);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBoxMouse);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonLog);
			this.Controls.Add(this.textBoxLog);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "Form1";
			this.Text = "PC作業記録";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxLog;
		private System.Windows.Forms.Button buttonLog;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxMouse;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxKeyboard;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxHandleCount;
		private System.Windows.Forms.TextBox textBoxLogSize;
	}
}

