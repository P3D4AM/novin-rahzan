using System.Linq;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.App;

public static class StiAppCreator
{
	public static IStiScreen CreateScreen(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiScreen);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Report = report;
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiScreen;
	}
}
