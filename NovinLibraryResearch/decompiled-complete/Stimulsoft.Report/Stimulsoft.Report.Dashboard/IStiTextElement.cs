using Stimulsoft.Base;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Report.Dashboard;

public interface IStiTextElement : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiGroupElement, IStiCrossFiltering
{
	string Text { get; set; }

	string GetSimpleText();
}
