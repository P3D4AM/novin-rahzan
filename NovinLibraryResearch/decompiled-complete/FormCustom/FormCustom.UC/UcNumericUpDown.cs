using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcNumericUpDown : UserControl
{
	private IContainer components;

	private PictureBox picUp;

	private PictureBox picDown;

	private Label lblQuantity;

	public decimal Quantity
	{
		get
		{
			return ((Control)lblQuantity).Text.ParseToDecimal();
		}
		set
		{
			((Control)lblQuantity).Text = value.ToString();
		}
	}

	public event Action OnValueChanged;

	public UcNumericUpDown()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void picUp_Click(object sender, EventArgs e)
	{
		try
		{
			Quantity += 1m;
			this.OnValueChanged?.Invoke();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picUp_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcNumericUpDown.cs", 27);
		}
	}

	private void picDown_Click(object sender, EventArgs e)
	{
		try
		{
			if (!(Quantity <= 0m))
			{
				Quantity -= 1m;
				this.OnValueChanged?.Invoke();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picDown_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcNumericUpDown.cs", 37);
		}
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		picUp = new PictureBox();
		picDown = new PictureBox();
		lblQuantity = new Label();
		((ISupportInitialize)picUp).BeginInit();
		((ISupportInitialize)picDown).BeginInit();
		((Control)this).SuspendLayout();
		((Control)picUp).Anchor = (AnchorStyles)11;
		((Control)picUp).Cursor = Cursors.Hand;
		picUp.Image = (Image)(object)FormCustom.Properties.Resources.up;
		((Control)picUp).Location = new Point(239, 3);
		((Control)picUp).Name = "picUp";
		((Control)picUp).Size = new Size(96, 37);
		picUp.SizeMode = (PictureBoxSizeMode)4;
		picUp.TabIndex = 0;
		picUp.TabStop = false;
		((Control)picUp).Click += picUp_Click;
		((Control)picDown).Anchor = (AnchorStyles)7;
		((Control)picDown).Cursor = Cursors.Hand;
		picDown.Image = (Image)(object)FormCustom.Properties.Resources.down;
		((Control)picDown).Location = new Point(3, 3);
		((Control)picDown).Name = "picDown";
		((Control)picDown).Size = new Size(96, 37);
		picDown.SizeMode = (PictureBoxSizeMode)4;
		picDown.TabIndex = 0;
		picDown.TabStop = false;
		((Control)picDown).Click += picDown_Click;
		((Control)lblQuantity).Anchor = (AnchorStyles)15;
		((Control)lblQuantity).BackColor = Color.Transparent;
		((Control)lblQuantity).Cursor = Cursors.Default;
		((Control)lblQuantity).Font = new Font("B Yekan", 40f);
		((Control)lblQuantity).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblQuantity).Location = new Point(103, -22);
		((Control)lblQuantity).Margin = new Padding(0);
		((Control)lblQuantity).Name = "lblQuantity";
		((Control)lblQuantity).Size = new Size(133, 67);
		((Control)lblQuantity).TabIndex = 13;
		((Control)lblQuantity).Text = "0";
		lblQuantity.TextAlign = (ContentAlignment)32;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)lblQuantity);
		((Control)this).Controls.Add((Control)(object)picDown);
		((Control)this).Controls.Add((Control)(object)picUp);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcNumericUpDown";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(338, 42);
		((ISupportInitialize)picUp).EndInit();
		((ISupportInitialize)picDown).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
