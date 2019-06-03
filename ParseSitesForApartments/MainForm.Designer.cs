namespace ParseSitesForApartments
{
  partial class MainForm
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
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.tspmFile = new System.Windows.Forms.ToolStripMenuItem();
      this.AnalysisResaltToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tspmExit = new System.Windows.Forms.ToolStripMenuItem();
      this.дополениеБазыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmUpdateTypeBuilding = new System.Windows.Forms.ToolStripMenuItem();
      this.lblChooseParse = new System.Windows.Forms.Label();
      this.cbChooseParse = new System.Windows.Forms.ComboBox();
      this.lblTypeRoom = new System.Windows.Forms.Label();
      this.cbTypeRoom = new System.Windows.Forms.ComboBox();
      this.btnExecute = new System.Windows.Forms.Button();
      this.tbSelectedPath = new System.Windows.Forms.TextBox();
      this.lblSavePath = new System.Windows.Forms.Label();
      this.btnSavePath = new System.Windows.Forms.Button();
      this.sfdParseFile = new System.Windows.Forms.SaveFileDialog();
      this.lblTypeSell = new System.Windows.Forms.Label();
      this.cbTypeSell = new System.Windows.Forms.ComboBox();
      this.загрузитьДанныеПоСреднейСтоимостиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspmFile,
            this.дополениеБазыToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(443, 24);
      this.menuStrip1.TabIndex = 25;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // tspmFile
      // 
      this.tspmFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AnalysisResaltToolStripMenuItem,
            this.tspmExit});
      this.tspmFile.Name = "tspmFile";
      this.tspmFile.Size = new System.Drawing.Size(48, 20);
      this.tspmFile.Text = "Файл";
      // 
      // AnalysisResaltToolStripMenuItem
      // 
      this.AnalysisResaltToolStripMenuItem.Name = "AnalysisResaltToolStripMenuItem";
      this.AnalysisResaltToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
      this.AnalysisResaltToolStripMenuItem.Text = "Анализ результата";
      this.AnalysisResaltToolStripMenuItem.Click += new System.EventHandler(this.AnalysisResaltToolStripMenuItem_Click);
      // 
      // tspmExit
      // 
      this.tspmExit.Name = "tspmExit";
      this.tspmExit.Size = new System.Drawing.Size(176, 22);
      this.tspmExit.Text = "Выход";
      this.tspmExit.Click += new System.EventHandler(this.tspmExit_Click);
      // 
      // дополениеБазыToolStripMenuItem
      // 
      this.дополениеБазыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmUpdateTypeBuilding,
            this.загрузитьДанныеПоСреднейСтоимостиToolStripMenuItem});
      this.дополениеБазыToolStripMenuItem.Name = "дополениеБазыToolStripMenuItem";
      this.дополениеБазыToolStripMenuItem.Size = new System.Drawing.Size(111, 20);
      this.дополениеБазыToolStripMenuItem.Text = "Дополение базы";
      // 
      // tsmUpdateTypeBuilding
      // 
      this.tsmUpdateTypeBuilding.Name = "tsmUpdateTypeBuilding";
      this.tsmUpdateTypeBuilding.Size = new System.Drawing.Size(299, 22);
      this.tsmUpdateTypeBuilding.Text = "Обновить тип домов";
      this.tsmUpdateTypeBuilding.Click += new System.EventHandler(this.tsmUpdateTypeBuilding_Click);
      // 
      // lblChooseParse
      // 
      this.lblChooseParse.AutoSize = true;
      this.lblChooseParse.Location = new System.Drawing.Point(12, 51);
      this.lblChooseParse.Name = "lblChooseParse";
      this.lblChooseParse.Size = new System.Drawing.Size(70, 13);
      this.lblChooseParse.TabIndex = 26;
      this.lblChooseParse.Text = "Что парсить";
      // 
      // cbChooseParse
      // 
      this.cbChooseParse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbChooseParse.Items.AddRange(new object[] {
            "Все сайты",
            "ELMS",
            "BN",
            "BKN",
            "Avito"});
      this.cbChooseParse.Location = new System.Drawing.Point(12, 67);
      this.cbChooseParse.Name = "cbChooseParse";
      this.cbChooseParse.Size = new System.Drawing.Size(121, 21);
      this.cbChooseParse.TabIndex = 27;
      // 
      // lblTypeRoom
      // 
      this.lblTypeRoom.AutoSize = true;
      this.lblTypeRoom.Location = new System.Drawing.Point(147, 51);
      this.lblTypeRoom.Name = "lblTypeRoom";
      this.lblTypeRoom.Size = new System.Drawing.Size(81, 13);
      this.lblTypeRoom.TabIndex = 28;
      this.lblTypeRoom.Text = "Кол-во комнат";
      // 
      // cbTypeRoom
      // 
      this.cbTypeRoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbTypeRoom.FormattingEnabled = true;
      this.cbTypeRoom.Items.AddRange(new object[] {
            "Все",
            "Студии",
            "1 ком.",
            "2 ком.",
            "3 ком.",
            "4 ком.",
            "Более 4 ком."});
      this.cbTypeRoom.Location = new System.Drawing.Point(150, 67);
      this.cbTypeRoom.Name = "cbTypeRoom";
      this.cbTypeRoom.Size = new System.Drawing.Size(121, 21);
      this.cbTypeRoom.TabIndex = 29;
      // 
      // btnExecute
      // 
      this.btnExecute.Enabled = false;
      this.btnExecute.Location = new System.Drawing.Point(12, 158);
      this.btnExecute.Name = "btnExecute";
      this.btnExecute.Size = new System.Drawing.Size(94, 23);
      this.btnExecute.TabIndex = 30;
      this.btnExecute.Text = "Выполнить";
      this.btnExecute.UseVisualStyleBackColor = true;
      this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
      // 
      // tbSelectedPath
      // 
      this.tbSelectedPath.Location = new System.Drawing.Point(12, 119);
      this.tbSelectedPath.Name = "tbSelectedPath";
      this.tbSelectedPath.ReadOnly = true;
      this.tbSelectedPath.Size = new System.Drawing.Size(223, 20);
      this.tbSelectedPath.TabIndex = 31;
      this.tbSelectedPath.TextChanged += new System.EventHandler(this.tpSelectedPath_TextChanged);
      // 
      // lblSavePath
      // 
      this.lblSavePath.AutoSize = true;
      this.lblSavePath.Location = new System.Drawing.Point(12, 103);
      this.lblSavePath.Name = "lblSavePath";
      this.lblSavePath.Size = new System.Drawing.Size(128, 13);
      this.lblSavePath.TabIndex = 32;
      this.lblSavePath.Text = "Путь сохранения файла";
      // 
      // btnSavePath
      // 
      this.btnSavePath.Location = new System.Drawing.Point(241, 117);
      this.btnSavePath.Name = "btnSavePath";
      this.btnSavePath.Size = new System.Drawing.Size(30, 23);
      this.btnSavePath.TabIndex = 33;
      this.btnSavePath.Text = "...";
      this.btnSavePath.UseVisualStyleBackColor = true;
      this.btnSavePath.Click += new System.EventHandler(this.btnSavePath_Click);
      // 
      // lblTypeSell
      // 
      this.lblTypeSell.AutoSize = true;
      this.lblTypeSell.Location = new System.Drawing.Point(281, 51);
      this.lblTypeSell.Name = "lblTypeSell";
      this.lblTypeSell.Size = new System.Drawing.Size(107, 13);
      this.lblTypeSell.TabIndex = 34;
      this.lblTypeSell.Text = "Продажа или Сдать";
      // 
      // cbTypeSell
      // 
      this.cbTypeSell.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbTypeSell.FormattingEnabled = true;
      this.cbTypeSell.Items.AddRange(new object[] {
            "Продажа",
            "Сдать"});
      this.cbTypeSell.Location = new System.Drawing.Point(284, 67);
      this.cbTypeSell.Name = "cbTypeSell";
      this.cbTypeSell.Size = new System.Drawing.Size(121, 21);
      this.cbTypeSell.TabIndex = 35;
      // 
      // загрузитьДанныеПоСреднейСтоимостиToolStripMenuItem
      // 
      this.загрузитьДанныеПоСреднейСтоимостиToolStripMenuItem.Name = "загрузитьДанныеПоСреднейСтоимостиToolStripMenuItem";
      this.загрузитьДанныеПоСреднейСтоимостиToolStripMenuItem.Size = new System.Drawing.Size(299, 22);
      this.загрузитьДанныеПоСреднейСтоимостиToolStripMenuItem.Text = "Загрузить данные по средней стоимости";
      this.загрузитьДанныеПоСреднейСтоимостиToolStripMenuItem.Click += new System.EventHandler(this.ЗагрузитьДанныеПоСреднейСтоимостиToolStripMenuItem_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(443, 217);
      this.Controls.Add(this.cbTypeSell);
      this.Controls.Add(this.lblTypeSell);
      this.Controls.Add(this.btnSavePath);
      this.Controls.Add(this.lblSavePath);
      this.Controls.Add(this.tbSelectedPath);
      this.Controls.Add(this.btnExecute);
      this.Controls.Add(this.cbTypeRoom);
      this.Controls.Add(this.lblTypeRoom);
      this.Controls.Add(this.cbChooseParse);
      this.Controls.Add(this.lblChooseParse);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Парсер квартир";
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem tspmFile;
    private System.Windows.Forms.ToolStripMenuItem tspmExit;
    private System.Windows.Forms.Label lblChooseParse;
    private System.Windows.Forms.ComboBox cbChooseParse;
    private System.Windows.Forms.Label lblTypeRoom;
    private System.Windows.Forms.ComboBox cbTypeRoom;
    private System.Windows.Forms.Button btnExecute;
    private System.Windows.Forms.TextBox tbSelectedPath;
    private System.Windows.Forms.Label lblSavePath;
    private System.Windows.Forms.Button btnSavePath;
    private System.Windows.Forms.SaveFileDialog sfdParseFile;
    private System.Windows.Forms.Label lblTypeSell;
    private System.Windows.Forms.ComboBox cbTypeSell;
    private System.Windows.Forms.ToolStripMenuItem AnalysisResaltToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem дополениеБазыToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem tsmUpdateTypeBuilding;
    private System.Windows.Forms.ToolStripMenuItem загрузитьДанныеПоСреднейСтоимостиToolStripMenuItem;
  }
}

