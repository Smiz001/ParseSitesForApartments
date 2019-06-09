namespace Core.UI
{
  partial class MainChartsForm
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
      this.mainChart = new LiveCharts.WinForms.CartesianChart();
      this.label1 = new System.Windows.Forms.Label();
      this.cmbTypeRoom = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.cmbStartDate = new System.Windows.Forms.ComboBox();
      this.cmbEndDate = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.btnDownload = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // mainChart
      // 
      this.mainChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.mainChart.Location = new System.Drawing.Point(12, 39);
      this.mainChart.Name = "mainChart";
      this.mainChart.Size = new System.Drawing.Size(860, 496);
      this.mainChart.TabIndex = 0;
      this.mainChart.Text = "cartesianChart1";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(15, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(73, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Тип квартир:";
      // 
      // cmbTypeRoom
      // 
      this.cmbTypeRoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbTypeRoom.FormattingEnabled = true;
      this.cmbTypeRoom.Location = new System.Drawing.Point(91, 12);
      this.cmbTypeRoom.Name = "cmbTypeRoom";
      this.cmbTypeRoom.Size = new System.Drawing.Size(149, 21);
      this.cmbTypeRoom.TabIndex = 2;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(260, 15);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(74, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Дата начала:";
      // 
      // cmbStartDate
      // 
      this.cmbStartDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbStartDate.FormattingEnabled = true;
      this.cmbStartDate.Location = new System.Drawing.Point(340, 12);
      this.cmbStartDate.Name = "cmbStartDate";
      this.cmbStartDate.Size = new System.Drawing.Size(121, 21);
      this.cmbStartDate.TabIndex = 4;
      // 
      // cmbEndDate
      // 
      this.cmbEndDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbEndDate.FormattingEnabled = true;
      this.cmbEndDate.Location = new System.Drawing.Point(565, 12);
      this.cmbEndDate.Name = "cmbEndDate";
      this.cmbEndDate.Size = new System.Drawing.Size(121, 21);
      this.cmbEndDate.TabIndex = 5;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(467, 15);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(92, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Дата окончания:";
      // 
      // btnDownload
      // 
      this.btnDownload.Location = new System.Drawing.Point(692, 10);
      this.btnDownload.Name = "btnDownload";
      this.btnDownload.Size = new System.Drawing.Size(75, 23);
      this.btnDownload.TabIndex = 7;
      this.btnDownload.Text = "Загрузить";
      this.btnDownload.UseVisualStyleBackColor = true;
      this.btnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
      // 
      // MainChartsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(884, 547);
      this.Controls.Add(this.btnDownload);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.cmbEndDate);
      this.Controls.Add(this.cmbStartDate);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.cmbTypeRoom);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.mainChart);
      this.Name = "MainChartsForm";
      this.Text = "График зависимости цен";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private LiveCharts.WinForms.CartesianChart mainChart;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cmbTypeRoom;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cmbStartDate;
    private System.Windows.Forms.ComboBox cmbEndDate;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btnDownload;
  }
}