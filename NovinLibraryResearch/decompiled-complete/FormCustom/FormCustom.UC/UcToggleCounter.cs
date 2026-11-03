using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser;

namespace FormCustom.UC;

public class UcToggleCounter : UserControl
{
	private IContainer components;

	private PictureBox pictureBox1;

	private Label lblCounter;

	public int Count
	{
		get
		{
			return ((Control)lblCounter).Text.ParseToInt();
		}
		set
		{
			((Control)lblCounter).Text = value.ToString();
		}
	}

	public UcToggleCounter()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		pictureBox1 = new PictureBox();
		lblCounter = new Label();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)pictureBox1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)pictureBox1).Controls.Add((Control)(object)lblCounter);
		((Control)pictureBox1).Dock = (DockStyle)5;
		pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.Red3;
		((Control)pictureBox1).Location = new Point(0, 0);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(35, 31);
		pictureBox1.SizeMode = (PictureBoxSizeMode)4;
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		((Control)lblCounter).BackColor = Color.Transparent;
		((Control)lblCounter).Dock = (DockStyle)5;
		((Control)lblCounter).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblCounter).ForeColor = Color.Black;
		((Control)lblCounter).Location = new Point(0, 0);
		((Control)lblCounter).Name = "lblCounter";
		((Control)lblCounter).Size = new Size(35, 31);
		((Control)lblCounter).TabIndex = 1;
		((Control)lblCounter).Text = "8";
		lblCounter.TextAlign = (ContentAlignment)32;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(249, 250, 255);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcToggleCounter";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(35, 31);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)pictureBox1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
