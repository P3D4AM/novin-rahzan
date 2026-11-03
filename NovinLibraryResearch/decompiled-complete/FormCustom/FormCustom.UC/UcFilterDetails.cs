using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Properties;

namespace FormCustom.UC;

public class UcFilterDetails : UserControl
{
	public delegate void OnClickEventHandler(object sender, EventArgs e);

	private bool _autoSize;

	private IContainer components;

	private PictureBox pic;

	private Label label1;

	private PanelEx pnlHeader;

	public Guid Guid { get; set; }

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

	public bool AutoSize
	{
		get
		{
			return _autoSize;
		}
		set
		{
			_autoSize = value;
			if (_autoSize)
			{
				((Control)this).Width = TextRenderer.MeasureText(DisplayText, ((Control)label1).Font).Width + 40;
			}
		}
	}

	public bool ShowDeleteImage
	{
		get
		{
			return ((Control)pic).Visible;
		}
		set
		{
			((Control)pic).Visible = value;
		}
	}

	public event OnClickEventHandler OnClick;

	public UcFilterDetails()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
		Guid = Guid.NewGuid();
	}

	private void pic_Click(object sender, EventArgs e)
	{
		this.OnClick?.Invoke(this, new EventArgs());
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		label1 = new Label();
		pnlHeader = new PanelEx();
		pic = new PictureBox();
		((Control)pnlHeader).SuspendLayout();
		((ISupportInitialize)pic).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label1).Cursor = Cursors.Default;
		((Control)label1).Dock = (DockStyle)5;
		((Control)label1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.Black;
		((Control)label1).Location = new Point(0, 0);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Padding = new Padding(0, 0, 5, 0);
		((Control)label1).Size = new Size(96, 30);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "[تصاویر]";
		label1.TextAlign = (ContentAlignment)16;
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)pic);
		((Control)pnlHeader).Controls.Add((Control)(object)label1);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(96, 30);
		pnlHeader.Style.BackColor1.Color = Color.White;
		pnlHeader.Style.BackColor2.Color = Color.White;
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.DimGray;
		pnlHeader.Style.BorderWidth = 2;
		pnlHeader.Style.CornerDiameter = 10;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 131;
		((Control)pic).Cursor = Cursors.Hand;
		pic.Image = (Image)(object)FormCustom.Properties.Resources.close;
		((Control)pic).Location = new Point(6, 2);
		((Control)pic).Name = "pic";
		((Control)pic).Size = new Size(22, 24);
		pic.SizeMode = (PictureBoxSizeMode)4;
		pic.TabIndex = 2;
		pic.TabStop = false;
		((Control)pic).Click += pic_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Cursor = Cursors.Hand;
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcFilterDetails";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(96, 30);
		((Control)pnlHeader).ResumeLayout(false);
		((ISupportInitialize)pic).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
