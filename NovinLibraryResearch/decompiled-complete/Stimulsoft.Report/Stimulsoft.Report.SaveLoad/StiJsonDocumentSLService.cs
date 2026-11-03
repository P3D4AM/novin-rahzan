using System;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.SaveLoad;

public class StiJsonDocumentSLService : StiDocumentSLService
{
	public override Type ServiceType => typeof(StiJsonDocumentSLService);

	public override bool MultiplePages => false;

	public override void Save(StiReport report, Stream stream)
	{
		try
		{
			string value = report.SaveToJsonInternal((StiJsonSaveMode)1);
			StreamWriter streamWriter = new StreamWriter(stream);
			streamWriter.Write(value);
			streamWriter.Flush();
		}
		finally
		{
			report.IsSerializing = false;
		}
	}

	public override void Load(StiReport report, Stream stream)
	{
		try
		{
			StreamReader streamReader = new StreamReader(stream);
			string text = streamReader.ReadToEnd();
			report.LoadFromJsonInternal(text);
			report.IsRendered = true;
			report.NeedsCompiling = false;
			report.IsDocument = true;
		}
		finally
		{
			foreach (StiPage renderedPage in report.RenderedPages)
			{
				renderedPage.Report = report;
				StiComponentsCollection components = renderedPage.GetComponents();
				foreach (StiComponent item in components)
				{
					item.Page = renderedPage;
				}
			}
		}
	}

	public override StiSLActions GetAction()
	{
		return StiSLActions.Save | StiSLActions.Load;
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "JsonDocumentFiles");
	}
}
