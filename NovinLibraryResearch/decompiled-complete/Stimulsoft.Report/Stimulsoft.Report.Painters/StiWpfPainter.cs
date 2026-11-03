using System;
using System.Collections;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Painters;

public abstract class StiWpfPainter : StiPainter
{
	public static SizeD MeasureComponent(double width, StiComponent comp)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (StiPainter.GetPainter(((object)comp).GetType(), (StiGuiMode)1) is StiWpfPainter stiWpfPainter)
		{
			return stiWpfPainter.Measure(width, comp);
		}
		throw new Exception($"StiWpfPainter for component '{comp}' is not found!");
	}

	public virtual SizeD Measure(double width, StiComponent comp)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return SizeD.Empty;
	}

	protected Hashtable GetHashtableForViewWpfPainter(StiReport report)
	{
		if (report.HashViewWpfPainter == null)
		{
			report.HashViewWpfPainter = new Hashtable();
		}
		return report.HashViewWpfPainter;
	}
}
