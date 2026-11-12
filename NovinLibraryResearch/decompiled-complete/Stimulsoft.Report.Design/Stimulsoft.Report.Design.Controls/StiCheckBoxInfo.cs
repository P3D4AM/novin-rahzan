using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design.Ribbon;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiCheckBoxInfo : StiCheckBoxUX
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
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			if (imageInfo == null)
			{
				string name = ((StiScale.Factor > 1.0) ? "Information_x2" : "Information");
				ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonBackStageUserControl));
				imageInfo = (Image)componentResourceManager.GetObject(name);
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
			superToolTip.Dispose();
		}
	}

	private void ThisOnPaint(object sender, PaintEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		double factor = StiScale.Factor;
		if (((Control)this).AutoSize && ImageInfo != null)
		{
			((Control)this).Padding = new Padding(0, 0, (int)((double)ImageInfo.Width * factor), 0);
		}
		if (isDrawInfoImage && ImageInfo != null && StiOptions.Appearance.ShowFormsHints)
		{
			rectangleImageInfo = new Rectangle((int)((double)TextRenderer.MeasureText(((Control)this).Text, ((Control)this).Font).Width + 14.0 * factor), (int)((double)((e.Graphics.ClipBounds.Height - (float)ImageInfo.Height) / 2f) - 1.0 * factor), ImageInfo.Width, ImageInfo.Height);
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
				superToolTip.ShowTooltip(this, info);
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
		timer.Tick += Timer_Tick;
		timer.Start();
	}

	private void Timer_Tick(object sender, EventArgs e)
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
		Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
		eTooltipColor color = eTooltipColor.StiOffice2013;
		superToolTip = new SuperTooltip();
		info = new SuperTooltipInfo(string.Empty, string.Empty, text, null, null, color);
		info.HeaderVisible = false;
		info.FooterVisible = true;
		SizeF sizeF = superToolTip.MeasureBodyText(globalMeasureGraphics, text, (int)(300.0 * StiScale.X));
		info.CustomSize = new Size((int)sizeF.Width + (int)(40.0 * StiScale.X), (int)sizeF.Height + (int)(8.0 * StiScale.Y));
	}
}
