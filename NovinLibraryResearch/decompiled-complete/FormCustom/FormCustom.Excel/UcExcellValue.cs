using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.Excel;

public class UcExcellValue : UserControl
{
	private IContainer components;

	internal Label lblTitle;

	internal TextBox txtValue;

	public string Title
	{
		get
		{
			return ((Control)lblTitle).Text;
		}
		set
		{
			((Control)lblTitle).Text = value;
		}
	}

	public int Value
	{
		get
		{
			return ((Control)txtValue).Text.ParseToInt();
		}
		set
		{
			if (value <= 0)
			{
				((Control)txtValue).Text = "";
			}
			else
			{
				((Control)txtValue).Text = value.ToString();
			}
		}
	}

	public Color TitleForeColor
	{
		get
		{
			return ((Control)lblTitle).ForeColor;
		}
		set
		{
			((Control)lblTitle).ForeColor = value;
		}
	}

	public Color ValueForeColor
	{
		get
		{
			return ((Control)txtValue).ForeColor;
		}
		set
		{
			((Control)txtValue).ForeColor = value;
		}
	}

	public Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			TextBox obj = txtValue;
			Color color2 = (((Control)lblTitle).BackColor = value);
			Color backColor = (((Control)obj).BackColor = color2);
			((Control)this).BackColor = backColor;
		}
	}

	public UcExcellValue()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void TXTCode_Enter(object sender, EventArgs e)
	{
		try
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val != null)
			{
				((Control)val).BackColor = Color.FromArgb(210, 221, 251);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTCode_Enter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Excel\\UcExcellValue.cs", 39);
		}
	}

	private void TXTCode_Leave(object sender, EventArgs e)
	{
		try
		{
			TextBox val = (TextBox)((sender is TextBox) ? sender : null);
			if (val != null)
			{
				((Control)val).BackColor = Color.White;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTCode_Leave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Excel\\UcExcellValue.cs", 48);
		}
	}

	private void txtValue_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				SendKeys.Send("{Tab}");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtValue_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Excel\\UcExcellValue.cs", 57);
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
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		lblTitle = new Label();
		txtValue = new TextBox();
		((Control)this).SuspendLayout();
		((Control)lblTitle).Anchor = (AnchorStyles)15;
		((Control)lblTitle).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)lblTitle).Location = new Point(42, 3);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(116, 21);
		((Control)lblTitle).TabIndex = 127;
		((Control)lblTitle).Text = "---";
		lblTitle.TextAlign = (ContentAlignment)16;
		((Control)txtValue).Anchor = (AnchorStyles)7;
		((Control)txtValue).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)txtValue).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtValue).Location = new Point(1, 1);
		((Control)txtValue).Name = "txtValue";
		((Control)txtValue).Size = new Size(36, 27);
		((Control)txtValue).TabIndex = 128;
		txtValue.TextAlign = (HorizontalAlignment)2;
		((Control)txtValue).Enter += TXTCode_Enter;
		((Control)txtValue).KeyDown += new KeyEventHandler(txtValue_KeyDown);
		((Control)txtValue).Leave += TXTCode_Leave;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		BackColor = SystemColors.Control;
		((Control)this).Controls.Add((Control)(object)txtValue);
		((Control)this).Controls.Add((Control)(object)lblTitle);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcExcellValue";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(156, 29);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
