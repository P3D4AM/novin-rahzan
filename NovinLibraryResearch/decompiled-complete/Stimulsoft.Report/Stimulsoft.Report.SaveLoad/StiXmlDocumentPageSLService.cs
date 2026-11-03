using System;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.SaveLoad;

public class StiXmlDocumentPageSLService : StiDocumentPageSLService
{
	public override Type ServiceType => typeof(StiDocumentPageSLService);

	public override void Save(StiPage page, Stream stream)
	{
		StiReport report = page.Report;
		StiReport stiReport = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
		try
		{
			stiReport.Unit = page.Unit;
			stiReport.RenderedPages.Clear();
			stiReport.RenderedPages.Add(page);
			page.Report = stiReport;
			stiReport.SaveDocument(stream);
		}
		finally
		{
			page.Report = report;
			stiReport.Dispose();
		}
	}

	public override void Load(StiPage page, Stream stream)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		StiReport report = page.Report;
		StiReport stiReport = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
		stiReport.LoadDocument(stream);
		if (stiReport.RenderedPages.Count == 0)
		{
			return;
		}
		if (stiReport.Unit != page.Unit)
		{
			stiReport.Convert(stiReport.Unit, page.Unit);
		}
		StiPage stiPage = stiReport.RenderedPages[0];
		stiPage.Report = null;
		StiComponentsCollection components = stiPage.Components;
		stiPage.Components = new StiComponentsCollection();
		MemoryStream memoryStream = new MemoryStream();
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter(allowLoadOptimization: true));
		val.CheckSerializable = true;
		val.Serialize((object)stiPage, (Stream)memoryStream, "temp");
		memoryStream.Seek(0L, SeekOrigin.Begin);
		val.Deserialize((object)page, (Stream)memoryStream, "temp");
		memoryStream.Close();
		memoryStream.Dispose();
		page.Report = report;
		page.Components.AddRange(components);
		components = page.GetComponents();
		foreach (StiComponent item in components)
		{
			item.Page = page;
			item.Report = report;
		}
	}

	public override StiSLActions GetAction()
	{
		return StiSLActions.Save | StiSLActions.Load;
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "PageFiles");
	}
}
