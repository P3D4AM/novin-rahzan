using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcDialogActivity : UserControl
{
	private IContainer components;

	private PanelEx pnlHeader;

	private PictureBox pic;

	private Label lblTitle;

	private Label lblShortCut;

	public Image Icon
	{
		get
		{
			return pic.Image;
		}
		set
		{
			pic.Image = value;
		}
	}

	public string ShortCut
	{
		get
		{
			return ((Control)lblShortCut).Text;
		}
		set
		{
			((Control)lblShortCut).Text = value;
		}
	}

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

	public short Tag { get; set; }

	public event Func<short, Task> OnClick;

	public void PerformClick()
	{
		this.OnClick?.Invoke(Tag);
	}

	public UcDialogActivity()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void lblTitle_MouseEnter(object sender, EventArgs e)
	{
		try
		{
			((Control)lblTitle).ForeColor = Color.Red;
			((Control)lblShortCut).ForeColor = Color.Red;
			pnlHeader.Style.BorderColor.Color = Color.Red;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblTitle_MouseEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcDialogActivity.cs", 43);
		}
	}

	private void lblTitle_MouseLeave(object sender, EventArgs e)
	{
		try
		{
			((Control)lblShortCut).ForeColor = Color.FromArgb(65, 81, 220);
			((Control)lblTitle).ForeColor = Color.FromArgb(65, 81, 220);
			pnlHeader.Style.BorderColor.Color = Color.FromArgb(210, 221, 251);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblTitle_MouseLeave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcDialogActivity.cs", 53);
		}
	}

	private void lblTitle_Click(object sender, EventArgs e)
	{
		this.OnClick?.Invoke(Tag);
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Expected O, but got Unknown
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		pnlHeader = new PanelEx();
		lblShortCut = new Label();
		lblTitle = new Label();
		pic = new PictureBox();
		((Control)pnlHeader).SuspendLayout();
		((ISupportInitialize)pic).BeginInit();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)lblShortCut);
		((Control)pnlHeader).Controls.Add((Control)(object)lblTitle);
		((Control)pnlHeader).Controls.Add((Control)(object)pic);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(376, 57);
		pnlHeader.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		pnlHeader.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.FromArgb(210, 221, 251);
		pnlHeader.Style.BorderWidth = 2;
		pnlHeader.Style.CornerDiameter = 5;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 131;
		((Control)pnlHeader).Enter += lblTitle_MouseEnter;
		((Control)pnlHeader).Leave += lblTitle_MouseLeave;
		((Control)lblShortCut).Cursor = Cursors.Hand;
		((Control)lblShortCut).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblShortCut).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblShortCut).Location = new Point(3, 5);
		((Control)lblShortCut).Name = "lblShortCut";
		((Control)lblShortCut).Size = new Size(47, 46);
		((Control)lblShortCut).TabIndex = 150;
		((Control)lblShortCut).Text = "-";
		lblShortCut.TextAlign = (ContentAlignment)64;
		((Control)lblShortCut).Click += lblTitle_Click;
		((Control)lblShortCut).MouseEnter += lblTitle_MouseEnter;
		((Control)lblShortCut).MouseLeave += lblTitle_MouseLeave;
		((Control)lblTitle).Anchor = (AnchorStyles)13;
		((Control)lblTitle).Cursor = Cursors.Hand;
		((Control)lblTitle).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblTitle).Location = new Point(49, 5);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(243, 46);
		((Control)lblTitle).TabIndex = 149;
		((Control)lblTitle).Text = "---";
		lblTitle.TextAlign = (ContentAlignment)16;
		((Control)lblTitle).Click += lblTitle_Click;
		((Control)lblTitle).MouseEnter += lblTitle_MouseEnter;
		((Control)lblTitle).MouseLeave += lblTitle_MouseLeave;
		((Control)pic).Anchor = (AnchorStyles)9;
		((Control)pic).Cursor = Cursors.Hand;
		((Control)pic).Location = new Point(298, 5);
		((Control)pic).Name = "pic";
		((Control)pic).Size = new Size(59, 46);
		pic.SizeMode = (PictureBoxSizeMode)4;
		pic.TabIndex = 148;
		pic.TabStop = false;
		((Control)pic).Click += lblTitle_Click;
		((Control)pic).MouseEnter += lblTitle_MouseEnter;
		((Control)pic).MouseLeave += lblTitle_MouseLeave;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcDialogActivity";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(376, 57);
		((Control)this).Enter += lblTitle_MouseEnter;
		((Control)this).Leave += lblTitle_MouseLeave;
		((Control)pnlHeader).ResumeLayout(false);
		((ISupportInitialize)pic).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
