using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcColorPicker : UserControl
{
	private string _hex;

	private IContainer components;

	private UcButtons btnColorSearch;

	internal Label lblColorTemplate;

	internal Label lblTitle;

	private Panel panel1;

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

	public string HexColor
	{
		get
		{
			return _hex;
		}
		set
		{
			_hex = value;
			((Control)lblColorTemplate).BackColor = _hex.RGBConverter();
		}
	}

	public Color Color => HexColor.RGBConverter();

	public Font Font
	{
		get
		{
			return ((Control)lblTitle).Font;
		}
		set
		{
			((Control)lblTitle).Font = value;
		}
	}

	public Color ForeColor
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

	public UcColorPicker()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void btnColorSearch_OnClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		try
		{
			ColorDialog val = new ColorDialog
			{
				AnyColor = true
			};
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				HexColor = val.Color.HexConverter();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnColorSearch_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcColorPicker.cs", 41);
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		btnColorSearch = new UcButtons();
		lblColorTemplate = new Label();
		lblTitle = new Label();
		panel1 = new Panel();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btnColorSearch).Anchor = (AnchorStyles)7;
		((Control)btnColorSearch).BackColor = Color.Transparent;
		btnColorSearch.CornerType = EnButtonCornerType.Square;
		btnColorSearch.DisplayText = "...";
		((Control)btnColorSearch).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnColorSearch.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnColorSearch.IsShowMoreDetails = true;
		((Control)btnColorSearch).Location = new Point(4, 5);
		((Control)btnColorSearch).Margin = new Padding(4, 5, 4, 5);
		((Control)btnColorSearch).Name = "btnColorSearch";
		((Control)btnColorSearch).RightToLeft = (RightToLeft)1;
		((Control)btnColorSearch).Size = new Size(35, 30);
		((Control)btnColorSearch).TabIndex = 156;
		btnColorSearch.TitleFont = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnColorSearch.Type = EnButtonType.Normal;
		btnColorSearch.OnClick += btnColorSearch_OnClick;
		((Control)lblColorTemplate).BackColor = Color.Transparent;
		lblColorTemplate.BorderStyle = (BorderStyle)1;
		((Control)lblColorTemplate).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblColorTemplate).Location = new Point(45, 10);
		((Control)lblColorTemplate).Name = "lblColorTemplate";
		((Control)lblColorTemplate).Size = new Size(23, 20);
		((Control)lblColorTemplate).TabIndex = 154;
		lblColorTemplate.TextAlign = (ContentAlignment)16;
		((Control)lblTitle).Anchor = (AnchorStyles)13;
		((Control)lblTitle).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).Location = new Point(74, 8);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(169, 27);
		((Control)lblTitle).TabIndex = 155;
		((Control)lblTitle).Text = "رنگ زمینه:";
		lblTitle.TextAlign = (ContentAlignment)16;
		((Control)panel1).Controls.Add((Control)(object)btnColorSearch);
		((Control)panel1).Controls.Add((Control)(object)lblTitle);
		((Control)panel1).Controls.Add((Control)(object)lblColorTemplate);
		((Control)panel1).Dock = (DockStyle)5;
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(243, 39);
		((Control)panel1).TabIndex = 157;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)panel1);
		Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcColorPicker";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(243, 39);
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
