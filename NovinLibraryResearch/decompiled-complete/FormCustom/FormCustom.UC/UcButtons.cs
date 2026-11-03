using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcButtons : UserControl
{
	public delegate void OnClickEventHandler(object sender, EventArgs e);

	private EnButtonType _type;

	private EnButtonCornerType _cornerType;

	private bool _isShowMoreDetails;

	private IContainer components;

	private PanelEx pnlHeader;

	private Label label1;

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

	public EnButtonType Type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
			if (!IsShowMoreDetails)
			{
				if (_type == EnButtonType.Normal)
				{
					pnlHeader.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
					pnlHeader.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
					pnlHeader.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
					((Control)label1).ForeColor = Color.FromArgb(65, 81, 220);
				}
				else if (_type == EnButtonType.Bold)
				{
					pnlHeader.Style.BackColor1.Color = Color.FromArgb(65, 81, 220);
					pnlHeader.Style.BackColor2.Color = Color.FromArgb(65, 81, 220);
					((Control)label1).ForeColor = Color.White;
				}
			}
		}
	}

	public EnButtonCornerType CornerType
	{
		get
		{
			return _cornerType;
		}
		set
		{
			_cornerType = value;
			if (_cornerType == EnButtonCornerType.Square)
			{
				pnlHeader.Style.CornerType = eCornerType.Square;
			}
			else if (_cornerType == EnButtonCornerType.Round)
			{
				pnlHeader.Style.CornerType = eCornerType.Rounded;
			}
		}
	}

	public bool IsShowMoreDetails
	{
		get
		{
			return _isShowMoreDetails;
		}
		set
		{
			_isShowMoreDetails = value;
			if (_isShowMoreDetails)
			{
				CornerType = EnButtonCornerType.Square;
				pnlHeader.Style.BackColor1.Color = Color.White;
				pnlHeader.Style.BackColor2.Color = Color.White;
				pnlHeader.Style.BorderColor.Color = Color.DimGray;
				((Control)label1).ForeColor = Color.Black;
				((Control)label1).Text = "...";
			}
			else
			{
				CornerType = EnButtonCornerType.Round;
				pnlHeader.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
				pnlHeader.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
				pnlHeader.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
				((Control)label1).ForeColor = Color.FromArgb(65, 81, 220);
			}
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

	public Guid Guid { get; set; }

	public int Id { get; set; }

	public Color ForeColor
	{
		get
		{
			return pnlHeader.Style.BackColor1.Color;
		}
		set
		{
			if (Type != EnButtonType.Bold)
			{
				pnlHeader.Style.BackColor1.Color = value;
				pnlHeader.Style.BackColor2.Color = value;
			}
		}
	}

	public event OnClickEventHandler OnClick;

	public void PerformClick()
	{
		this.OnClick?.Invoke(this, new EventArgs());
	}

	public UcButtons()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void label1_Click(object sender, EventArgs e)
	{
		this.OnClick?.Invoke(this, new EventArgs());
	}

	private void label1_MouseLeave(object sender, EventArgs e)
	{
		try
		{
			if (Type != EnButtonType.Bold && !IsShowMoreDetails)
			{
				((Control)label1).ForeColor = Color.FromArgb(65, 81, 220);
				pnlHeader.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "label1_MouseLeave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcButtons.cs", 111);
		}
	}

	private void UcButtons_Enter(object sender, EventArgs e)
	{
		label1_MouseEnter(sender, e);
	}

	private void UcButtons_Leave(object sender, EventArgs e)
	{
		label1_MouseLeave(sender, e);
	}

	private void label1_MouseEnter(object sender, EventArgs e)
	{
		try
		{
			if (Type != EnButtonType.Bold && !IsShowMoreDetails)
			{
				((Control)label1).ForeColor = Color.Red;
				pnlHeader.Style.BorderColor.Color = Color.Red;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "label1_MouseEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcButtons.cs", 123);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		pnlHeader = new PanelEx();
		label1 = new Label();
		((Control)pnlHeader).SuspendLayout();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)label1);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(135, 30);
		pnlHeader.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		pnlHeader.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		pnlHeader.Style.BorderWidth = 2;
		pnlHeader.Style.CornerDiameter = 10;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 127;
		((Control)label1).Cursor = Cursors.Hand;
		((Control)label1).Dock = (DockStyle)5;
		((Control)label1).Font = new Font("Anjoman Regular", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)label1).Location = new Point(0, 0);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(135, 30);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "[تصاویر]";
		label1.TextAlign = (ContentAlignment)32;
		((Control)label1).Click += label1_Click;
		((Control)label1).MouseEnter += label1_MouseEnter;
		((Control)label1).MouseLeave += label1_MouseLeave;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcButtons";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(135, 30);
		((Control)this).Enter += UcButtons_Enter;
		((Control)this).Leave += UcButtons_Leave;
		((Control)pnlHeader).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
