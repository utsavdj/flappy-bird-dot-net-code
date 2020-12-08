namespace FlappyBird
{
  partial class Game
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
	  this._flappyBirdTimer = new System.Windows.Forms.Timer(this.components);
	  this._pictureBox = new System.Windows.Forms.PictureBox();
	  ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
	  this.SuspendLayout();
	  // 
	  // _flappyBirdTimer
	  // 
	  this._flappyBirdTimer.Enabled = true;
	  this._flappyBirdTimer.Interval = 17;
	  this._flappyBirdTimer.Tick += new System.EventHandler(this.FlappyBirdTimerTick);
	  // 
	  // _flappyBirdPictureBox
	  // 
	  this._pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
	  this._pictureBox.BackColor = System.Drawing.Color.SkyBlue;
	  this._pictureBox.Location = new System.Drawing.Point(0, 0);
	  this._pictureBox.Name = "_flappyBirdPictureBox";
	  this._pictureBox.Size = new System.Drawing.Size(534, 601);
	  this._pictureBox.TabIndex = 0;
	  this._pictureBox.TabStop = false;
	  this._pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBoxPaint);
	  // 
	  // Game
	  // 
	  this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
	  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	  this.ClientSize = new System.Drawing.Size(534, 601);
	  this.Controls.Add(this._pictureBox);
	  this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
	  this.MaximizeBox = false;
	  this.Name = "Game";
	  this.Text = "Flappy Bird";
	  this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameKeyDown);
	  this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameKeyUp);
	  ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
	  this.ResumeLayout(false);

	}

		#endregion

		private System.Windows.Forms.PictureBox _pictureBox;
		private System.Windows.Forms.Timer _flappyBirdTimer;
	}
}

