using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace FormCustom.UC;

public class UcGroupBox : UserControl
{
	private IContainer components;

	private PanelEx pnlContent;

	private PanelEx pnlHeader;

	private LabelX lblTitle;

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

	public bool IsShowHeader
	{
		get
		{
			return ((Control)pnlHeader).Visible;
		}
		set
		{
			((Control)pnlHeader).Visible = value;
		}
	}

	public Color BackGroundColor => pnlContent.Style.BackColor1.Color;

	public Font DisplayFont
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

	public int HeadHeight
	{
		get
		{
			return ((Control)pnlHeader).Height;
		}
		set
		{
			((Control)pnlHeader).Height = value;
		}
	}

	public UcGroupBox()
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
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Expected O, but got Unknown
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		pnlContent = new PanelEx();
		pnlHeader = new PanelEx();
		lblTitle = new LabelX();
		((Control)pnlContent).SuspendLayout();
		((Control)pnlHeader).SuspendLayout();
		((Control)this).SuspendLayout();
		pnlContent.CanvasColor = Color.FromArgb(127, 119, 103);
		((Control)pnlContent).Controls.Add((Control)(object)pnlHeader);
		pnlContent.DisabledBackColor = Color.Empty;
		((Control)pnlContent).Dock = (DockStyle)5;
		((Control)pnlContent).Location = new Point(0, 0);
		((Control)pnlContent).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlContent).Name = "pnlContent";
		((Control)pnlContent).Size = new Size(282, 103);
		pnlContent.Style.BackColor1.Color = Color.FromArgb(249, 250, 255);
		pnlContent.Style.BackColor2.Color = Color.FromArgb(249, 250, 255);
		pnlContent.Style.Border = eBorderType.SingleLine;
		pnlContent.Style.BorderColor.Color = Color.FromArgb(148, 160, 232);
		pnlContent.Style.CornerDiameter = 6;
		pnlContent.Style.CornerType = eCornerType.Rounded;
		((Control)pnlContent).TabIndex = 4;
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)lblTitle);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)1;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(282, 40);
		pnlHeader.Style.BackColor1.Color = Color.FromArgb(65, 81, 220);
		pnlHeader.Style.BackColor2.Color = Color.FromArgb(65, 81, 220);
		pnlHeader.Style.BorderColor.Color = Color.FromArgb(148, 160, 232);
		pnlHeader.Style.BorderWidth = 2;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 8;
		((Control)lblTitle).BackColor = Color.FromArgb(65, 81, 220);
		lblTitle.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)lblTitle).Cursor = Cursors.Default;
		((Control)lblTitle).Dock = (DockStyle)5;
		((Control)lblTitle).Font = new Font("PeydaWeb Black", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblTitle).ForeColor = Color.FromArgb(235, 237, 255);
		((Control)lblTitle).Location = new Point(0, 0);
		((Control)lblTitle).Margin = new Padding(4);
		((Control)lblTitle).Name = "lblTitle";
		lblTitle.Padding = new Padding(10);
		lblTitle.PaddingRight = 15;
		((Control)lblTitle).Size = new Size(282, 40);
		lblTitle.Style = eDotNetBarStyle.Office2003;
		((Control)lblTitle).TabIndex = 2;
		((Control)lblTitle).Text = "[عنوان]";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlContent);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcGroupBox";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(282, 103);
		((Control)pnlContent).ResumeLayout(false);
		((Control)pnlHeader).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
