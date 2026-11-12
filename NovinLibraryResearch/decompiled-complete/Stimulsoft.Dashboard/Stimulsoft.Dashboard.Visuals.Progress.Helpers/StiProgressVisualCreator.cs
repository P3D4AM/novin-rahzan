using System.Collections.Generic;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Visuals.Progress.Helpers;

public static class StiProgressVisualCreator
{
	public static StiProgressVisual CreateProgressVisual(StiProgressElement element, List<StiProgressIteration> iterations)
	{
		return element.Mode switch
		{
			StiProgressElementMode.DataBars => new StiDataBarsProgressVisual(element, iterations), 
			StiProgressElementMode.Circle => new StiCircleProgressVisual(element, iterations), 
			StiProgressElementMode.Pie => new StiPieProgressVisual(element, iterations), 
			_ => throw new StiTypeNotRecognizedException(element.Mode), 
		};
	}
}
