using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Properties;

namespace FormCustom.UC;

public class UcService : UserControl
{
	private IContainer components;

	private PanelEx pnlHeader;

	private PictureBox picBox;

	private Label lblTitle;

	private Label label1;

	private Line label2;

	public UcService()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
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
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Expected O, but got Unknown
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Expected O, but got Unknown
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Expected O, but got Unknown
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		pnlHeader = new PanelEx();
		picBox = new PictureBox();
		label1 = new Label();
		lblTitle = new Label();
		label2 = new Line();
		((Control)pnlHeader).SuspendLayout();
		((ISupportInitialize)picBox).BeginInit();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)picBox);
		((Control)pnlHeader).Controls.Add((Control)(object)label1);
		((Control)pnlHeader).Controls.Add((Control)(object)lblTitle);
		((Control)pnlHeader).Controls.Add((Control)(object)label2);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(143, 104);
		pnlHeader.Style.BackColor1.Color = Color.White;
		pnlHeader.Style.BackColor2.Color = Color.White;
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.White;
		pnlHeader.Style.BorderWidth = 0;
		pnlHeader.Style.CornerDiameter = 10;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 147;
		picBox.Image = (Image)(object)FormCustom.Properties.Resources.Services;
		((Control)picBox).Location = new Point(3, 9);
		((Control)picBox).Name = "picBox";
		((Control)picBox).Size = new Size(29, 26);
		picBox.SizeMode = (PictureBoxSizeMode)1;
		picBox.TabIndex = 4;
		picBox.TabStop = false;
		((Control)label1).Anchor = (AnchorStyles)13;
		((Control)label1).Cursor = Cursors.Hand;
		((Control)label1).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.FromArgb(177, 92, 255);
		((Control)label1).Location = new Point(0, 56);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(143, 32);
		((Control)label1).TabIndex = 3;
		((Control)label1).Text = "---";
		label1.TextAlign = (ContentAlignment)32;
		((Control)lblTitle).Anchor = (AnchorStyles)9;
		((Control)lblTitle).AutoSize = true;
		((Control)lblTitle).Cursor = Cursors.Hand;
		((Control)lblTitle).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.FromArgb(177, 92, 255);
		((Control)lblTitle).Location = new Point(39, 11);
		((Control)lblTitle).Margin = new Padding(4, 0, 4, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(102, 24);
		((Control)lblTitle).TabIndex = 3;
		((Control)lblTitle).Text = "[خدمات دریافتی]";
		lblTitle.TextAlign = (ContentAlignment)64;
		((Control)label2).Anchor = (AnchorStyles)13;
		((Control)label2).AutoSize = true;
		((Control)label2).Cursor = Cursors.Default;
		((Control)label2).Font = new Font("B Yekan", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label2).ForeColor = Color.DarkGray;
		((Control)label2).Location = new Point(31, 81);
		((Control)label2).Margin = new Padding(4, 0, 4, 0);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(75, 16);
		((Control)label2).TabIndex = 3;
		((Control)label2).Text = "خدمات دریافتی";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcService";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(143, 104);
		((Control)pnlHeader).ResumeLayout(false);
		((Control)pnlHeader).PerformLayout();
		((ISupportInitialize)picBox).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
