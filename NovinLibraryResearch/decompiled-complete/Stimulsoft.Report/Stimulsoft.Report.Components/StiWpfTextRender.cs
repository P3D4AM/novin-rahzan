using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components;

public static class StiWpfTextRender
{
	public static SizeD MeasureString(double width, StiText textBox)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		SizeD result = StiWpfPainter.MeasureComponent(width, textBox);
		if (textBox.Angle == 90f || textBox.Angle == 180f)
		{
			((SizeD)(ref result)).Width = ((SizeD)(ref result)).Width * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorWpf;
		}
		else
		{
			((SizeD)(ref result)).Height = ((SizeD)(ref result)).Height * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorWpf;
		}
		return result;
	}

	public static SizeD MeasureRtfString(double width, StiRichText textBox)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return StiWpfPainter.MeasureComponent(width, textBox);
	}

	public static string BreakText(RectangleD rect, ref string text, StiText textComp)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		Type type = Type.GetType("Stimulsoft.Report.Wpf.StiWpfBreakTextHelper, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type == null)
		{
			throw new Exception("Assembly 'Stimulsoft.Report.Wpf' is not found");
		}
		IStiWpfBreakTextHelper stiWpfBreakTextHelper = StiActivator.CreateObject(type, new object[0]) as IStiWpfBreakTextHelper;
		return stiWpfBreakTextHelper.BreakText(rect, ref text, textComp);
	}
}
