using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcButtonsNew : UserControl
{
	public delegate void OnClickEventHandler(object sender, EventArgs e);

	private FlatCardType _theme = FlatCardType.Light;

	private IContainer components;

	private Label label1;

	private PanelEx pnlHeader;

	public string DisplayText
	{
		get
		{
			return ((Control)label1).Text;
		}
		set
		{
			((Control)label1).Text = value;
		}
	}

	public Font TitleFont
	{
		get
		{
			return ((Control)label1).Font;
		}
		set
		{
			((Control)label1).Font = value;
		}
	}

	public FlatCardType Theme
	{
		get
		{
			return _theme;
		}
		set
		{
			_theme = value;
			switch (_theme)
			{
			case FlatCardType.Light:
				pnlHeader.Style.BackColor1.Color = Color.FromArgb(241, 245, 254);
				pnlHeader.Style.BackColor2.Color = Color.FromArgb(241, 245, 254);
				pnlHeader.Style.BorderColor.Color = Color.FromArgb(241, 245, 254);
				((Control)label1).ForeColor = Color.FromArgb(65, 81, 220);
				break;
			case FlatCardType.Dark:
				pnlHeader.Style.BackColor1.Color = Color.FromArgb(65, 81, 220);
				pnlHeader.Style.BackColor2.Color = Color.FromArgb(65, 81, 220);
				pnlHeader.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
				((Control)label1).ForeColor = Color.FromArgb(241, 245, 254);
				break;
			}
		}
	}

	public bool Enabled
	{
		get
		{
			return ((Control)label1).Enabled;
		}
		set
		{
			try
			{
				((Control)label1).Enabled = value;
				if (!value)
				{
					pnlHeader.Style.BackColor1.Color = Color.Silver;
					pnlHeader.Style.BackColor2.Color = Color.Silver;
					pnlHeader.Style.BorderColor.Color = Color.Silver;
				}
				else
				{
					Theme = _theme;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Enabled", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcButtonsNew.cs", 57);
			}
		}
	}

	public event OnClickEventHandler OnClick;

	public UcButtonsNew()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	public void PerformClick()
	{
		this.OnClick?.Invoke(this, new EventArgs());
	}

	private void label1_Click(object sender, EventArgs e)
	{
		this.OnClick?.Invoke(this, new EventArgs());
	}

	private void label1_MouseEnter(object sender, EventArgs e)
	{
		((Control)label1).ForeColor = Color.Red;
	}

	private void label1_MouseLeave(object sender, EventArgs e)
	{
		try
		{
			switch (_theme)
			{
			case FlatCardType.Light:
				((Control)label1).ForeColor = Color.FromArgb(65, 81, 220);
				break;
			case FlatCardType.Dark:
				((Control)label1).ForeColor = Color.FromArgb(241, 245, 254);
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "label1_MouseLeave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcButtonsNew.cs", 79);
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
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		label1 = new Label();
		pnlHeader = new PanelEx();
		((Control)pnlHeader).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)label1).Cursor = Cursors.Hand;
		((Control)label1).Dock = (DockStyle)5;
		((Control)label1).Font = new Font("PeydaWeb Black", 9.749999f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)label1).Location = new Point(0, 0);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(200, 39);
		((Control)label1).TabIndex = 0;
		label1.TextAlign = (ContentAlignment)32;
		((Control)label1).Click += label1_Click;
		((Control)label1).MouseEnter += label1_MouseEnter;
		((Control)label1).MouseLeave += label1_MouseLeave;
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)label1);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(200, 39);
		pnlHeader.Style.BackColor1.Color = Color.FromArgb(241, 245, 254);
		pnlHeader.Style.BackColor2.Color = Color.FromArgb(241, 245, 254);
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.FromArgb(241, 245, 254);
		pnlHeader.Style.BorderWidth = 0;
		pnlHeader.Style.CornerDiameter = 16;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 131;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcButtonsNew";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(200, 39);
		((Control)pnlHeader).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
