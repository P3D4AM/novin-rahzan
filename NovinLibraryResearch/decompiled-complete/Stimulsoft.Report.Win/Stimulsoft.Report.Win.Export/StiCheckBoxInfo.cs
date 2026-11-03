using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Win.Export;

[ToolboxItem(false)]
public class StiCheckBoxInfo : CheckBox
{
	private Image imageInfo;

	private bool isDrawInfoImage;

	private bool isShowToolTip;

	private Rectangle rectangleImageInfo;

	private SuperTooltip superToolTip;

	private SuperTooltipInfo info;

	private Timer timer;

	private bool isMouseEnter;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Image ImageInfo
	{
		get
		{
			if (imageInfo == null)
			{
				imageInfo = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Information" + StiScale.StepName + ".png");
			}
			return imageInfo;
		}
	}

	public StiCheckBoxInfo()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		if (StiOptions.Appearance.ShowFormsHints)
		{
			timer = new Timer();
			((Control)this).Paint += new PaintEventHandler(ThisOnPaint);
			((Control)this).MouseEnter += ThisOnMouseEnter;
			((Control)this).MouseLeave += ThisOnMouseLeave;
			((Control)this).MouseMove += new MouseEventHandler(ThisOnMouseMove);
		}
	}

	protected override void Dispose(bool disposing)
	{
		((ButtonBase)this).Dispose(disposing);
		if (superToolTip != null)
		{
			((Component)(object)superToolTip).Dispose();
		}
	}

	private void ThisOnPaint(object sender, PaintEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).AutoSize && ImageInfo != null)
		{
			((Control)this).Padding = new Padding(0, 0, ImageInfo.Width, 0);
		}
		if (isDrawInfoImage && ImageInfo != null && StiOptions.Appearance.ShowFormsHints)
		{
			rectangleImageInfo = new Rectangle(TextRenderer.MeasureText(((Control)this).Text, ((Control)this).Font).Width + StiScale.XXI(14.0), (int)((e.Graphics.ClipBounds.Height - (float)ImageInfo.Height) / 2f - (float)StiScale.I1), ImageInfo.Width, ImageInfo.Height);
			e.Graphics.DrawImage(ImageInfo, rectangleImageInfo);
		}
	}

	private void ThisOnMouseMove(object sender, MouseEventArgs e)
	{
		if (superToolTip != null)
		{
			if (rectangleImageInfo.Contains(e.Location) && !isShowToolTip)
			{
				isShowToolTip = true;
				superToolTip.ShowTooltip((object)this, info);
			}
			if (!rectangleImageInfo.Contains(e.Location))
			{
				superToolTip.HideTooltip();
				isShowToolTip = false;
			}
		}
	}

	private void ThisOnMouseLeave(object sender, EventArgs e)
	{
		((Component)(object)timer).Dispose();
		isMouseEnter = false;
		isDrawInfoImage = false;
		((Control)this).Invalidate();
		if (superToolTip != null)
		{
			superToolTip.HideTooltip();
		}
	}

	private void ThisOnMouseEnter(object sender, EventArgs e)
	{
		isMouseEnter = true;
		timer.Interval = 400;
		timer.Tick += timer_Tick;
		timer.Start();
	}

	private void timer_Tick(object sender, EventArgs e)
	{
		if (isMouseEnter)
		{
			isDrawInfoImage = true;
			((Control)this).Invalidate();
		}
		((Component)(object)timer).Dispose();
	}

	public void SetToolTip(string text)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
		eTooltipColor val = (eTooltipColor)19;
		superToolTip = new SuperTooltip();
		info = new SuperTooltipInfo(string.Empty, string.Empty, text, (Image)null, (Image)null, val);
		info.HeaderVisible = false;
		info.FooterVisible = true;
		SizeF sizeF = superToolTip.MeasureBodyText(globalMeasureGraphics, text, StiScale.XXI(300.0));
		info.CustomSize = new Size((int)sizeF.Width + StiScale.XXI(40.0), (int)sizeF.Height + StiScale.YYI(8.0));
	}
}
