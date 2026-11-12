using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Design.Components;

[ToolboxItem(false)]
public class StiWatermarkControl : UserControl
{
	private Image imageTemplate;

	private string textValue;

	private double textAngle;

	private StiBrush textBrush;

	private bool textBehind;

	private Image imageValue;

	private int imageTransparency;

	private string imageAlignment;

	private bool imageStretch;

	private bool imageTiling;

	private bool imageBehind;

	private bool watermarkEnable;

	private IContainer components;

	public string TextValue
	{
		get
		{
			return textValue;
		}
		set
		{
			textValue = value;
			((Control)this).Invalidate();
		}
	}

	public double TextAngle
	{
		get
		{
			return textAngle;
		}
		set
		{
			textAngle = 360.0 - value;
			((Control)this).Invalidate();
		}
	}

	public StiBrush TextBrush
	{
		get
		{
			return textBrush;
		}
		set
		{
			textBrush = value;
			((Control)this).Invalidate();
		}
	}

	public bool TextBehind
	{
		get
		{
			return textBehind;
		}
		set
		{
			textBehind = value;
			((Control)this).Invalidate();
		}
	}

	public Image Image
	{
		get
		{
			return imageValue;
		}
		set
		{
			imageValue = value;
			((Control)this).Invalidate();
		}
	}

	public int ImageTransparency
	{
		get
		{
			return imageTransparency;
		}
		set
		{
			imageTransparency = value;
			((Control)this).Invalidate();
		}
	}

	public string ImageAlignment
	{
		get
		{
			return imageAlignment;
		}
		set
		{
			imageAlignment = value;
			((Control)this).Invalidate();
		}
	}

	public bool ImageStretch
	{
		get
		{
			return imageStretch;
		}
		set
		{
			imageStretch = value;
			((Control)this).Invalidate();
		}
	}

	public bool ImageTiling
	{
		get
		{
			return imageTiling;
		}
		set
		{
			imageTiling = value;
			((Control)this).Invalidate();
		}
	}

	public bool ImageBehind
	{
		get
		{
			return imageBehind;
		}
		set
		{
			imageBehind = value;
			((Control)this).Invalidate();
		}
	}

	public bool WatermarkEnable
	{
		get
		{
			return watermarkEnable;
		}
		set
		{
			watermarkEnable = value;
			((Control)this).Invalidate();
		}
	}

	public StiWatermarkControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			imageTemplate = (Image)(object)StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Components.Images.BigImage.png");
		}
	}

	private void ThisPaint(object sender, PaintEventArgs e)
	{
		Build(e.Graphics);
	}

	private void Build(Graphics graphic)
	{
		if (imageBehind)
		{
			DrawImageTemplate(graphic);
		}
		if (textBehind)
		{
			DrawTextTemplate(graphic);
		}
		DrawGridTemplate(graphic);
		if (!imageBehind)
		{
			DrawImageTemplate(graphic);
		}
		if (!textBehind)
		{
			DrawTextTemplate(graphic);
		}
	}

	private void DrawGridTemplate(Graphics graphic)
	{
		for (int i = 1; i < 15; i++)
		{
			int num = StiScale.I10 * i;
			graphic.DrawLine(Pens.LightGray, num, StiScale.I10, num, StiScale.YYI(200.0));
		}
		for (int j = 1; j < 21; j++)
		{
			int num2 = StiScale.I10 * j;
			graphic.DrawLine(Pens.LightGray, StiScale.I10, num2, StiScale.XXI(140.0), num2);
		}
	}

	private void DrawTextTemplate(Graphics graphic)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		if (string.IsNullOrEmpty(TextValue) || !WatermarkEnable || textBrush == null)
		{
			return;
		}
		Font val = new Font("Arial", 16f, ((Control)this).Font.Style);
		try
		{
			SizeF size = graphic.MeasureString(TextValue, val);
			Brush brush = StiBrush.GetBrush(textBrush, new RectangleF(new Point(0, 0), size));
			try
			{
				PointF point = new PointF(((Control)this).Width / 2, ((Control)this).Height / 2);
				StringFormat stringFormat = GetStringFormat(new StiStringFormatGeom(StringFormat.GenericDefault));
				try
				{
					StiRotatedTextDrawing.DrawString(graphic, TextValue, val, brush, point, stringFormat, StiRotationMode.CenterCenter, (float)TextAngle, antialiasing: true);
				}
				finally
				{
					((IDisposable)stringFormat)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private StringFormat GetStringFormat(StiStringFormatGeom sfGeom)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		StringFormat val = (StringFormat)(sfGeom.IsGeneric ? ((object)/*isinst with value type is only supported in some contexts*/) : ((object)new StringFormat()));
		val.Alignment = sfGeom.Alignment;
		val.FormatFlags = sfGeom.FormatFlags;
		val.HotkeyPrefix = sfGeom.HotkeyPrefix;
		val.LineAlignment = sfGeom.LineAlignment;
		val.Trimming = sfGeom.Trimming;
		return val;
	}

	private void DrawImageTemplate(Graphics graphic)
	{
		if (!WatermarkEnable || Image == null)
		{
			return;
		}
		Image transparentedImage = imageTemplate;
		if (ImageTransparency != 0)
		{
			transparentedImage = (Image)(object)StiImageTransparenceHelper.GetTransparentedImage(transparentedImage, 1f - (float)ImageTransparency / 255f);
		}
		if (imageTiling)
		{
			int num = ((Control)this).Width / 2;
			int num2 = ((Control)this).Height / 3;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					graphic.DrawImage(transparentedImage, new Rectangle(i * num, j * num2, num, num2));
				}
			}
		}
		else if (imageStretch)
		{
			graphic.DrawImage(transparentedImage, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
		}
		else
		{
			int x = 0;
			int y = 0;
			if (ImageAlignment == StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomCenter"))
			{
				x = ((Control)this).Width / 2 - transparentedImage.Size.Width / 2;
				y = ((Control)this).Height - transparentedImage.Size.Height;
			}
			else if (ImageAlignment == StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomLeft"))
			{
				y = ((Control)this).Height - transparentedImage.Size.Height;
			}
			else if (ImageAlignment == StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomRight"))
			{
				x = ((Control)this).Width - transparentedImage.Size.Width;
				y = ((Control)this).Height - transparentedImage.Size.Height;
			}
			else if (ImageAlignment == StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleCenter"))
			{
				x = ((Control)this).Width / 2 - transparentedImage.Size.Width / 2;
				y = ((Control)this).Height / 2 - transparentedImage.Size.Height / 2;
			}
			else if (ImageAlignment == StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleLeft"))
			{
				y = ((Control)this).Height / 2 - transparentedImage.Size.Height / 2;
			}
			else if (ImageAlignment == StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleRight"))
			{
				x = ((Control)this).Width - transparentedImage.Size.Width;
				y = ((Control)this).Height / 2 - transparentedImage.Size.Height / 2;
			}
			else if (ImageAlignment == StiLocalization.Get("PropertyEnum", "ContentAlignmentTopCenter"))
			{
				x = ((Control)this).Width / 2 - transparentedImage.Size.Width / 2;
			}
			else if (!(ImageAlignment == StiLocalization.Get("PropertyEnum", "ContentAlignmentTopLeft")) && ImageAlignment == StiLocalization.Get("PropertyEnum", "ContentAlignmentTopRight"))
			{
				x = ((Control)this).Width - transparentedImage.Size.Width;
			}
			graphic.DrawImage(transparentedImage, new Rectangle(x, y, transparentedImage.Size.Width, transparentedImage.Size.Height));
		}
		if (ImageTransparency != 0)
		{
			transparentedImage.Dispose();
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
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = SystemColors.ButtonHighlight;
		((UserControl)this).BorderStyle = (BorderStyle)1;
		((Control)this).DoubleBuffered = true;
		((Control)this).Name = "StiWatermarkControl";
		((Control)this).Size = new Size(152, 212);
		((Control)this).Paint += new PaintEventHandler(ThisPaint);
		((Control)this).ResumeLayout(false);
	}
}
