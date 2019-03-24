namespace CoreUI
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
      this.cmbTypeBuild = new System.Windows.Forms.ComboBox();
      this.label6 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.lblOtklonenie = new System.Windows.Forms.Label();
      this.nudFoot = new System.Windows.Forms.NumericUpDown();
      this.nudCar = new System.Windows.Forms.NumericUpDown();
      this.label8 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudFoot)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudCar)).BeginInit();
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
      this.cbMetro.SelectedIndexChanged += new System.EventHandler(this.cbMetro_SelectedIndexChanged);
      // 
      // cbCountRoom
      // 
      this.cbCountRoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbCountRoom.FormattingEnabled = true;
      this.cbCountRoom.Items.AddRange(new object[] {
            "Все комнаты",
            "Студия",
            "1 км.",
            "2 км.",
            "3 км.",
            "4 км.",
            "Более 4 км."});
      this.cbCountRoom.Location = new System.Drawing.Point(176, 80);
      this.cbCountRoom.Name = "cbCountRoom";
      this.cbCountRoom.Size = new System.Drawing.Size(110, 21);
      this.cbCountRoom.TabIndex = 2;
      this.cbCountRoom.SelectedIndexChanged += new System.EventHandler(this.cbCountRoom_SelectedIndexChanged);
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
      this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnLoad.Location = new System.Drawing.Point(674, 131);
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
      this.dataGridView1.Location = new System.Drawing.Point(15, 159);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.ReadOnly = true;
      this.dataGridView1.Size = new System.Drawing.Size(734, 377);
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
      this.label4.Location = new System.Drawing.Point(438, 26);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(121, 13);
      this.label4.TabIndex = 9;
      this.label4.Text = "Средняя цена за кв.м:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(438, 49);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(115, 13);
      this.label5.TabIndex = 10;
      this.label5.Text = "Кол-во предложений:";
      // 
      // lbAveragePriceForSquare
      // 
      this.lbAveragePriceForSquare.AutoSize = true;
      this.lbAveragePriceForSquare.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbAveragePriceForSquare.Location = new System.Drawing.Point(586, 26);
      this.lbAveragePriceForSquare.Name = "lbAveragePriceForSquare";
      this.lbAveragePriceForSquare.Size = new System.Drawing.Size(0, 13);
      this.lbAveragePriceForSquare.TabIndex = 11;
      // 
      // lbCountFlat
      // 
      this.lbCountFlat.AutoSize = true;
      this.lbCountFlat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbCountFlat.Location = new System.Drawing.Point(586, 49);
      this.lbCountFlat.Name = "lbCountFlat";
      this.lbCountFlat.Size = new System.Drawing.Size(0, 13);
      this.lbCountFlat.TabIndex = 12;
      // 
      // cmbTypeBuild
      // 
      this.cmbTypeBuild.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbTypeBuild.FormattingEnabled = true;
      this.cmbTypeBuild.Location = new System.Drawing.Point(16, 132);
      this.cmbTypeBuild.Name = "cmbTypeBuild";
      this.cmbTypeBuild.Size = new System.Drawing.Size(154, 21);
      this.cmbTypeBuild.TabIndex = 13;
      this.cmbTypeBuild.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(16, 113);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(55, 13);
      this.label6.TabIndex = 14;
      this.label6.Text = "Тип дома";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(438, 75);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(136, 13);
      this.label7.TabIndex = 15;
      this.label7.Text = "Сред. квадр. отклонение:";
      // 
      // lblOtklonenie
      // 
      this.lblOtklonenie.AutoSize = true;
      this.lblOtklonenie.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOtklonenie.Location = new System.Drawing.Point(586, 75);
      this.lblOtklonenie.Name = "lblOtklonenie";
      this.lblOtklonenie.Size = new System.Drawing.Size(0, 13);
      this.lblOtklonenie.TabIndex = 16;
      // 
      // nudFoot
      // 
      this.nudFoot.Location = new System.Drawing.Point(176, 133);
      this.nudFoot.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.nudFoot.Name = "nudFoot";
      this.nudFoot.Size = new System.Drawing.Size(120, 20);
      this.nudFoot.TabIndex = 17;
      this.nudFoot.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.nudFoot.ValueChanged += new System.EventHandler(this.nudFoot_ValueChanged);
      // 
      // nudCar
      // 
      this.nudCar.Location = new System.Drawing.Point(302, 133);
      this.nudCar.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.nudCar.Name = "nudCar";
      this.nudCar.Size = new System.Drawing.Size(120, 20);
      this.nudCar.TabIndex = 18;
      this.nudCar.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.nudCar.ValueChanged += new System.EventHandler(this.nudCar_ValueChanged);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(173, 113);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(97, 13);
      this.label8.TabIndex = 19;
      this.label8.Text = "Пешком не более";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(299, 113);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(95, 13);
      this.label9.TabIndex = 20;
      this.label9.Text = "На авто не более";
      // 
      // AnalysisResultForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(761, 548);
      this.Controls.Add(this.label9);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.nudCar);
      this.Controls.Add(this.nudFoot);
      this.Controls.Add(this.lblOtklonenie);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.cmbTypeBuild);
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
      ((System.ComponentModel.ISupportInitialize)(this.nudFoot)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudCar)).EndInit();
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
    private System.Windows.Forms.ComboBox cmbTypeBuild;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label lblOtklonenie;
    private System.Windows.Forms.NumericUpDown nudFoot;
    private System.Windows.Forms.NumericUpDown nudCar;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
  }
}