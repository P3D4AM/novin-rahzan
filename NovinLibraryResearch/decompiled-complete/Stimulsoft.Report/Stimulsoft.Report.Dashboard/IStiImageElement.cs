using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dashboard;

public interface IStiImageElement : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiGroupElement, IStiCrossFiltering
{
	byte[] Image { get; set; }

	string ImageHyperlink { get; set; }

	StiFontIcons? Icon { get; set; }

	Color IconColor { get; set; }

	bool AspectRatio { get; set; }

	void CopyAllImageProperties(IStiImageElement element);
}
