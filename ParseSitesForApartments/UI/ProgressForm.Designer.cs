namespace ParseSitesForApartments.UI
{
  partial class ProgressForm
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
      this.pbDownloadInfo = new System.Windows.Forms.ProgressBar();
      this.button1 = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.lbCountFlat = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // pbDownloadInfo
      // 
      this.pbDownloadInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pbDownloadInfo.Location = new System.Drawing.Point(12, 36);
      this.pbDownloadInfo.Name = "pbDownloadInfo";
      this.pbDownloadInfo.Size = new System.Drawing.Size(395, 23);
      this.pbDownloadInfo.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
      this.pbDownloadInfo.TabIndex = 0;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(181, 69);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "Остановить";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(115, 12);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(121, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Обработано квартир - ";
      // 
      // lbCountFlat
      // 
      this.lbCountFlat.AutoSize = true;
      this.lbCountFlat.Location = new System.Drawing.Point(239, 13);
      this.lbCountFlat.Name = "lbCountFlat";
      this.lbCountFlat.Size = new System.Drawing.Size(0, 13);
      this.lbCountFlat.TabIndex = 3;
      // 
      // ProgressForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(419, 104);
      this.Controls.Add(this.lbCountFlat);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.pbDownloadInfo);
      this.MaximizeBox = false;
      this.Name = "ProgressForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Результат обработки";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ProgressBar pbDownloadInfo;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lbCountFlat;
  }
}