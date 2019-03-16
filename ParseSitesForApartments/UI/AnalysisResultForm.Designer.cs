namespace ParseSitesForApartments.UI
{
  partial class AnalysisResultForm
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
      this.cbMetro = new System.Windows.Forms.ComboBox();
      this.cbCountRoom = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btnLoad = new System.Windows.Forms.Button();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.tbPathToFile = new System.Windows.Forms.TextBox();
      this.btnSelectFile = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.lbAveragePriceForSquare = new System.Windows.Forms.Label();
      this.lbCountFlat = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 59);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(39, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Метро";
      // 
      // cbMetro
      // 
      this.cbMetro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbMetro.FormattingEnabled = true;
      this.cbMetro.Location = new System.Drawing.Point(15, 80);
      this.cbMetro.Name = "cbMetro";
      this.cbMetro.Size = new System.Drawing.Size(155, 21);
      this.cbMetro.TabIndex = 1;
      // 
      // cbCountRoom
      // 
      this.cbCountRoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbCountRoom.FormattingEnabled = true;
      this.cbCountRoom.Items.AddRange(new object[] {
            "Студии",
            "1 ком.",
            "2 ком.",
            "3 ком.",
            "4 ком.",
            "Более 4 ком."});
      this.cbCountRoom.Location = new System.Drawing.Point(176, 80);
      this.cbCountRoom.Name = "cbCountRoom";
      this.cbCountRoom.Size = new System.Drawing.Size(164, 21);
      this.cbCountRoom.TabIndex = 2;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(173, 59);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(81, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Кол-во комнат";
      // 
      // btnLoad
      // 
      this.btnLoad.Location = new System.Drawing.Point(346, 80);
      this.btnLoad.Name = "btnLoad";
      this.btnLoad.Size = new System.Drawing.Size(75, 21);
      this.btnLoad.TabIndex = 4;
      this.btnLoad.Text = "Загрузить";
      this.btnLoad.UseVisualStyleBackColor = true;
      this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
      // 
      // dataGridView1
      // 
      this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Default;
      this.dataGridView1.Location = new System.Drawing.Point(15, 107);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.ReadOnly = true;
      this.dataGridView1.Size = new System.Drawing.Size(734, 361);
      this.dataGridView1.TabIndex = 5;
      this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
      // 
      // tbPathToFile
      // 
      this.tbPathToFile.Location = new System.Drawing.Point(12, 28);
      this.tbPathToFile.Name = "tbPathToFile";
      this.tbPathToFile.ReadOnly = true;
      this.tbPathToFile.Size = new System.Drawing.Size(242, 20);
      this.tbPathToFile.TabIndex = 6;
      // 
      // btnSelectFile
      // 
      this.btnSelectFile.Location = new System.Drawing.Point(260, 26);
      this.btnSelectFile.Name = "btnSelectFile";
      this.btnSelectFile.Size = new System.Drawing.Size(26, 23);
      this.btnSelectFile.TabIndex = 7;
      this.btnSelectFile.Text = "...";
      this.btnSelectFile.UseVisualStyleBackColor = true;
      this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 9);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(75, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "Выбор файла";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(481, 26);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(121, 13);
      this.label4.TabIndex = 9;
      this.label4.Text = "Средняя цена за кв.м:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(481, 49);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(115, 13);
      this.label5.TabIndex = 10;
      this.label5.Text = "Кол-во предложений:";
      // 
      // lbAveragePriceForSquare
      // 
      this.lbAveragePriceForSquare.AutoSize = true;
      this.lbAveragePriceForSquare.Location = new System.Drawing.Point(608, 26);
      this.lbAveragePriceForSquare.Name = "lbAveragePriceForSquare";
      this.lbAveragePriceForSquare.Size = new System.Drawing.Size(35, 13);
      this.lbAveragePriceForSquare.TabIndex = 11;
      this.lbAveragePriceForSquare.Text = "label6";
      // 
      // lbCountFlat
      // 
      this.lbCountFlat.AutoSize = true;
      this.lbCountFlat.Location = new System.Drawing.Point(608, 49);
      this.lbCountFlat.Name = "lbCountFlat";
      this.lbCountFlat.Size = new System.Drawing.Size(0, 13);
      this.lbCountFlat.TabIndex = 12;
      // 
      // AnalysisResultForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(761, 480);
      this.Controls.Add(this.lbCountFlat);
      this.Controls.Add(this.lbAveragePriceForSquare);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.btnSelectFile);
      this.Controls.Add(this.tbPathToFile);
      this.Controls.Add(this.dataGridView1);
      this.Controls.Add(this.btnLoad);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.cbCountRoom);
      this.Controls.Add(this.cbMetro);
      this.Controls.Add(this.label1);
      this.Name = "AnalysisResultForm";
      this.Text = "Анализ квартир";
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbMetro;
    private System.Windows.Forms.ComboBox cbCountRoom;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnLoad;
    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.TextBox tbPathToFile;
    private System.Windows.Forms.Button btnSelectFile;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label lbAveragePriceForSquare;
    private System.Windows.Forms.Label lbCountFlat;
  }
}