using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Controls;

public static class StiReportCopyHelper
{
	public static StiReport Copy(StiReport report)
	{
		bool isModified = report.IsModified;
		MemoryStream memoryStream = new MemoryStream();
		report.Save(memoryStream);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		report.IsModified = isModified;
		StiReport stiReport = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
		stiReport.Load(memoryStream);
		stiReport.RegData(report.DataStore);
		stiReport.RegBusinessObject(report.BusinessObjectsStore);
		stiReport.RenderedPages.Add(new StiPage(stiReport));
		return stiReport;
	}
}
