using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace Stimulsoft.Base.Drawing;

public static class StiBrushes
{
	public static Brush Content { get; } = (Brush)new SolidBrush(StiColors.Content);


	public static Brush ContentDark { get; } = (Brush)new SolidBrush(StiColors.ContentDark);


	public static Brush Focus { get; } = (Brush)new SolidBrush(StiColors.Focus);


	public static Brush Selected { get; } = (Brush)new SolidBrush(StiColors.Selected);


	public static Brush SelectedText { get; } = (Brush)new SolidBrush(StiColors.SelectedText);


	public static Brush Window { get; } = (Brush)new SolidBrush(SystemColors.Window);


	public static HatchStyle[] HatchStyles { get; }

	public static HatchStyle HatchStyleFromName(string name)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		HatchStyle[] hatchStyles = HatchStyles;
		for (int i = 0; i < hatchStyles.Length; i++)
		{
			HatchStyle result = hatchStyles[i];
			if (((object)(HatchStyle)(ref result)).ToString() == name)
			{
				return result;
			}
		}
		return (HatchStyle)3;
	}

	public static Brush GetControlBrush(Rectangle rectangle, float angle)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		return (Brush)new LinearGradientBrush(rectangle, StiColors.ControlStart, StiColors.ControlEnd, angle);
	}

	public static Brush GetControlDarkBrush(Rectangle rectangle, float angle)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		return (Brush)new LinearGradientBrush(rectangle, StiColors.ControlStartDark, StiColors.ControlEndDark, angle);
	}

	public static Brush GetControlLightBrush(Rectangle rectangle, float angle)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		return (Brush)new LinearGradientBrush(rectangle, StiColors.ControlStartLight, StiColors.ControlEndLight, angle);
	}

	public static Brush GetActiveCaptionBrush(Rectangle rectangle, float angle)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		return (Brush)new LinearGradientBrush(rectangle, StiColors.ActiveCaptionStart, StiColors.ActiveCaptionEnd, angle);
	}

	public static Brush GetActiveCaptionLightBrush(Rectangle rectangle, float angle)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		return (Brush)new LinearGradientBrush(rectangle, StiColorUtils.Light(StiColors.ActiveCaptionStart, 20), StiColorUtils.Light(StiColors.ActiveCaptionEnd, 20), angle);
	}

	static StiBrushes()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		HatchStyle[] array = new HatchStyle[52];
		RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		HatchStyles = (HatchStyle[])(object)array;
	}
}
