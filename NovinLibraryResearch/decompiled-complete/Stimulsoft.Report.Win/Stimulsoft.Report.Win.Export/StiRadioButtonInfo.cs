using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Win.Export;

[ToolboxItem(false)]
public class StiRadioButtonInfo : RadioButton
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
	public bool ShowToolTip { get; set; } = true;


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

	public StiRadioButtonInfo()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
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
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		double factor = StiScale.Factor;
		if (((Control)this).AutoSize && ImageInfo != null)
		{
			((Control)this).Padding = new Padding(0, 0, (int)((double)ImageInfo.Width * factor), 0);
		}
		if (isDrawInfoImage && ImageInfo != null && ShowToolTip)
		{
			rectangleImageInfo = new Rectangle((int)((double)((SizeF)TextRenderer.MeasureText(((Control)this).Text, ((Control)this).Font)).Width + 14.0 * factor), (int)(e.Graphics.ClipBounds.Height - (float)ImageInfo.Height) / 2, ImageInfo.Width, ImageInfo.Height);
			e.Graphics.DrawImage(ImageInfo, rectangleImageInfo);
		}
	}

	private void ThisOnMouseMove(object sender, MouseEventArgs e)
	{
		if (superToolTip != null && ShowToolTip)
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
		if (ShowToolTip)
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
	}

	private void ThisOnMouseEnter(object sender, EventArgs e)
	{
		if (ShowToolTip)
		{
			isMouseEnter = true;
			timer.Interval = 400;
			timer.Tick += timer_Tick;
			timer.Start();
		}
	}

	private void timer_Tick(object sender, EventArgs e)
	{
		if (ShowToolTip)
		{
			if (isMouseEnter)
			{
				isDrawInfoImage = true;
				((Control)this).Invalidate();
			}
			((Component)(object)timer).Dispose();
		}
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
		SizeF sizeF = superToolTip.MeasureBodyText(globalMeasureGraphics, text, (int)(300.0 * StiScale.X));
		info.CustomSize = new Size((int)sizeF.Width + (int)(40.0 * StiScale.X), (int)sizeF.Height + (int)(8.0 * StiScale.Y));
	}
}
