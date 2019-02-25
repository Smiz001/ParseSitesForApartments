namespace ParseSitesForApartments.UI
{
  partial class ConnectionForm
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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.tbServerName = new System.Windows.Forms.TextBox();
      this.tbDataBase = new System.Windows.Forms.TextBox();
      this.tbUser = new System.Windows.Forms.TextBox();
      this.tbPassword = new System.Windows.Forms.TextBox();
      this.cbSqlAut = new System.Windows.Forms.CheckBox();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(10, 10);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(47, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Сервер:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(10, 35);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(75, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "База данных:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(20, 85);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(83, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Пользователь:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(20, 110);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(48, 13);
      this.label4.TabIndex = 3;
      this.label4.Text = "Пароль:";
      // 
      // tbServerName
      // 
      this.tbServerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbServerName.Location = new System.Drawing.Point(119, 10);
      this.tbServerName.Name = "tbServerName";
      this.tbServerName.Size = new System.Drawing.Size(174, 20);
      this.tbServerName.TabIndex = 4;
      // 
      // tbDataBase
      // 
      this.tbDataBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbDataBase.Location = new System.Drawing.Point(119, 35);
      this.tbDataBase.Name = "tbDataBase";
      this.tbDataBase.Size = new System.Drawing.Size(174, 20);
      this.tbDataBase.TabIndex = 5;
      // 
      // tbUser
      // 
      this.tbUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbUser.Enabled = false;
      this.tbUser.Location = new System.Drawing.Point(116, 85);
      this.tbUser.Name = "tbUser";
      this.tbUser.Size = new System.Drawing.Size(174, 20);
      this.tbUser.TabIndex = 6;
      // 
      // tbPassword
      // 
      this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tbPassword.Enabled = false;
      this.tbPassword.Location = new System.Drawing.Point(116, 110);
      this.tbPassword.Name = "tbPassword";
      this.tbPassword.Size = new System.Drawing.Size(174, 20);
      this.tbPassword.TabIndex = 7;
      this.tbPassword.UseSystemPasswordChar = true;
      // 
      // cbSqlAut
      // 
      this.cbSqlAut.AutoSize = true;
      this.cbSqlAut.Location = new System.Drawing.Point(10, 60);
      this.cbSqlAut.Name = "cbSqlAut";
      this.cbSqlAut.Size = new System.Drawing.Size(150, 17);
      this.cbSqlAut.TabIndex = 8;
      this.cbSqlAut.Text = "SQL Server Авторизация";
      this.cbSqlAut.UseVisualStyleBackColor = true;
      this.cbSqlAut.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button1.Location = new System.Drawing.Point(134, 155);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 9;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.button2.Location = new System.Drawing.Point(215, 155);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 10;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // ConnectionForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(308, 190);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.cbSqlAut);
      this.Controls.Add(this.tbPassword);
      this.Controls.Add(this.tbUser);
      this.Controls.Add(this.tbDataBase);
      this.Controls.Add(this.tbServerName);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Name = "ConnectionForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Подключение к базе";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox tbServerName;
    private System.Windows.Forms.TextBox tbDataBase;
    private System.Windows.Forms.TextBox tbUser;
    private System.Windows.Forms.TextBox tbPassword;
    private System.Windows.Forms.CheckBox cbSqlAut;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
  }
}