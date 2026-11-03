using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public abstract class StiAxisCoreXF : ICloneable, IStiApplyStyle
{
	private bool isMouseOverDecreaseButton;

	private bool isMouseOverIncreaseButton;

	private bool isMouseOverTrackBar;

	public static float DefaultScrollBarSize = 12f;

	public static float DefaultScrollBarSmallFactor = 0.01f;

	public static float DefaultScrollBarFirstRecallTime = 0.3f;

	public static float DefaultScrollBarOtherRecallTime = 0.05f;

	private IStiAxis axis;

	[Browsable(false)]
	public bool IsMouseOverDecreaseButton
	{
		get
		{
			return isMouseOverDecreaseButton;
		}
		set
		{
			isMouseOverDecreaseButton = value;
		}
	}

	[Browsable(false)]
	public bool IsMouseOverIncreaseButton
	{
		get
		{
			return isMouseOverIncreaseButton;
		}
		set
		{
			isMouseOverIncreaseButton = value;
		}
	}

	[Browsable(false)]
	public bool IsMouseOverTrackBar
	{
		get
		{
			return isMouseOverTrackBar;
		}
		set
		{
			isMouseOverTrackBar = value;
		}
	}

	internal float TicksMaxLength
	{
		get
		{
			if (Axis.Visible)
			{
				float val = ((!Axis.Ticks.Visible) ? 0f : ((Axis.Ticks.LengthUnderLabels > 0f) ? Axis.Ticks.LengthUnderLabels : Axis.Ticks.Length));
				float val2 = (Axis.Ticks.MinorVisible ? Axis.Ticks.MinorLength : 0f);
				return Math.Max(val, val2);
			}
			return 0f;
		}
	}

	internal float ArrowWidth => 3f;

	internal float ArrowHeight
	{
		get
		{
			if (this is StiXAxisCoreXF)
			{
				if (Axis.Area.ReverseHor)
				{
					return -5f;
				}
				return 5f;
			}
			if (this is StiYAxisCoreXF)
			{
				if (Axis.Area.ReverseVert)
				{
					return -5f;
				}
				return 5f;
			}
			return 0f;
		}
	}

	public IStiAxis Axis
	{
		get
		{
			return axis;
		}
		set
		{
			axis = value;
		}
	}

	public StiAxisInfoXF Info
	{
		get
		{
			return Axis.Info;
		}
		set
		{
			Axis.Info = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual void ApplyStyle(IStiChartStyle style)
	{
		if (Axis.AllowApplyStyle)
		{
			Axis.LineColor = style.Core.AxisLineColor;
			Axis.Labels.Core.ApplyStyle(style);
			Axis.Title.Core.ApplyStyle(style);
		}
	}

	public virtual bool GetStartFromZero()
	{
		if (Axis.Area.AxisCore.ValuesCount == 1)
		{
			return true;
		}
		if (Axis != null && Axis.Range != null && !Axis.Range.Auto)
		{
			return true;
		}
		if (Axis == null)
		{
			return true;
		}
		return Axis.StartFromZero;
	}

	public abstract StiCellGeom Render(StiContext context, RectangleF rect);

	public abstract StiCellGeom RenderView(StiContext context, RectangleF rect);

	protected internal void CalculateStripPositions(float topPosition, float bottomPosition)
	{
		bottomPosition -= topPosition;
		topPosition = 0f;
		if (Axis.Info.StripLines == null || Axis.Info.StripLines.Count < 2)
		{
			Axis.Info.StripPositions = new float[0];
			return;
		}
		bool flag = false;
		if (Axis.Area.Chart.Series.Count > 0 && Axis.Area.Chart.Series[0] is StiScatterSeries)
		{
			flag = true;
		}
		if (Axis.LogarithmicScale && flag)
		{
			int count = Axis.Info.StripLines.Count;
			Axis.Info.StripPositions = new float[Axis.Info.StripLines.Count];
			Axis.Info.StripPositions[0] = topPosition;
			Axis.Info.StripPositions[Axis.Info.StripPositions.Length - 1] = bottomPosition;
			double value = Axis.Info.StripLines[0].Value;
			double value2 = Axis.Info.StripLines[count - 1].Value;
			double num = (double)bottomPosition / (Math.Log10(value2) - Math.Log10(value));
			for (int i = 1; i < Axis.Info.StripPositions.Length - 1; i++)
			{
				double value3 = Axis.Info.StripLines[i].Value;
				double num2 = Math.Abs(Math.Log10(Math.Abs(Axis.Info.StripLines[i].Value)) * num - Math.Log10(Math.Abs(Axis.Info.StripLines[i - 1].Value)) * num);
				Axis.Info.StripPositions[i] = (float)((double)topPosition + num2);
				topPosition = Axis.Info.StripPositions[i];
			}
		}
		else
		{
			Axis.Info.StripPositions = new float[Axis.Info.StripLines.Count];
			Axis.Info.StripPositions[0] = topPosition;
			Axis.Info.StripPositions[Axis.Info.StripPositions.Length - 1] = bottomPosition;
			for (int j = 1; j < Axis.Info.StripPositions.Length - 1; j++)
			{
				Axis.Info.StripPositions[j] = topPosition + (float)j * Axis.Info.Step;
			}
		}
	}

	protected internal float GetTicksMaxLength(StiContext context)
	{
		return Axis.Core.TicksMaxLength * context.Options.Zoom;
	}

	protected internal float GetArrowHeight(StiContext context)
	{
		return Axis.Core.ArrowHeight * 4f * context.Options.Zoom;
	}

	protected internal float GetLabelsSpaceAxis(StiContext context)
	{
		return 5f * context.Options.Zoom;
	}

	protected internal float GetLabelsTwoLinesDestination(StiContext context)
	{
		return Axis.Labels.Font.SizeInPoints * 2f * context.Options.Zoom;
	}

	protected internal StiFontGeom GetFontGeom(StiContext context)
	{
		return StiFontGeom.ChangeFontSize(Axis.Labels.Font, Axis.Labels.Font.Size * context.Options.Zoom);
	}

	protected internal StiHorAlignment GetTextAlignment()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (Axis is IStiYAxis)
		{
			IStiYAxis stiYAxis = Axis as IStiYAxis;
			if (Axis.Labels.Placement == StiLabelsPlacement.TwoLines)
			{
				if (((StiYAxisCoreXF)stiYAxis.Core).IsLeftSide)
				{
					return (StiHorAlignment)3;
				}
				if (((StiYAxisCoreXF)stiYAxis.Core).IsRightSide)
				{
					return (StiHorAlignment)1;
				}
			}
		}
		return Axis.Labels.TextAlignment;
	}

	protected internal StiStringFormatGeom GetStringFormatGeom(StiContext context, bool wordWrap)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
		genericStringFormat.Trimming = (StringTrimming)0;
		if (!wordWrap)
		{
			genericStringFormat.FormatFlags = (StringFormatFlags)6144;
		}
		if (Axis.Labels.Width > 0f)
		{
			StiHorAlignment textAlignment = GetTextAlignment();
			if ((int)textAlignment == 1)
			{
				genericStringFormat.Alignment = (StringAlignment)0;
			}
			else if ((int)textAlignment == 3)
			{
				genericStringFormat.Alignment = (StringAlignment)2;
			}
			else if ((int)textAlignment == 2)
			{
				genericStringFormat.Alignment = (StringAlignment)1;
			}
		}
		return genericStringFormat;
	}

	protected SizeF GetAxisTitleSize(StiContext context)
	{
		if (string.IsNullOrEmpty(Axis.Title.Text))
		{
			return Size.Empty;
		}
		StiFontGeom font = StiFontGeom.ChangeFontSize(Axis.Title.Font, Axis.Title.Font.Size * context.Options.Zoom);
		SizeF result = context.MeasureString(Axis.Title.Text, font);
		switch (Axis.Title.Direction)
		{
		case StiDirection.LeftToRight:
		case StiDirection.RightToLeft:
			return result;
		case StiDirection.TopToBottom:
		case StiDirection.BottomToTop:
			return new SizeF(result.Height, result.Width);
		default:
			return Size.Empty;
		}
	}

	protected internal float GetAngleTitle()
	{
		float result = 0f;
		switch (Axis.Title.Direction)
		{
		case StiDirection.LeftToRight:
			result = 0f;
			break;
		case StiDirection.RightToLeft:
			result = 180f;
			break;
		case StiDirection.BottomToTop:
			result = -90f;
			break;
		case StiDirection.TopToBottom:
			result = 90f;
			break;
		}
		return result;
	}

	protected internal float GetCorrectionFontSize(RectangleF axisRect, RectangleF titleRect, float currentFontSize)
	{
		switch (Axis.Title.Direction)
		{
		case StiDirection.LeftToRight:
		case StiDirection.RightToLeft:
			if (axisRect.Width < titleRect.Width)
			{
				currentFontSize = axisRect.Width / titleRect.Width * currentFontSize;
			}
			break;
		case StiDirection.TopToBottom:
		case StiDirection.BottomToTop:
			if (axisRect.Height < titleRect.Height)
			{
				currentFontSize = axisRect.Height / titleRect.Height * currentFontSize;
			}
			break;
		}
		return currentFontSize;
	}

	protected internal bool CheckUseMaxWidth(RectangleF axisRect, RectangleF titleRect, out float maxWidth)
	{
		maxWidth = 0f;
		switch (Axis.Title.Direction)
		{
		case StiDirection.LeftToRight:
		case StiDirection.RightToLeft:
			if (axisRect.Width < titleRect.Width)
			{
				maxWidth = axisRect.Width;
				return true;
			}
			break;
		case StiDirection.TopToBottom:
		case StiDirection.BottomToTop:
			if (axisRect.Height < titleRect.Height)
			{
				maxWidth = axisRect.Width;
				return true;
			}
			break;
		}
		return false;
	}

	public StiAxisCoreXF(IStiAxis axis)
	{
		this.axis = axis;
	}
}
