using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Wizards;

[StiToolbox(false)]
public class StiReportTemplatesService : StiService
{
	public override string ServiceName => "StiReportTemplatesService";

	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryDesigner");

	public sealed override Type ServiceType => typeof(StiReportTemplatesService);

	[StiServiceParam]
	[DefaultValue("Templates")]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public virtual string ReportTemplatesPath
	{
		get
		{
			return StiDesigner.DefaultTemplatesPath;
		}
		set
		{
			StiDesigner.DefaultTemplatesPath = value;
		}
	}

	public static StiReportTemplatesService GetService()
	{
		return StiDesignerOptions.ReportTemplates.FirstOrDefault();
	}

	public static string GetDefaultTemplatesConfigPath()
	{
		string defaultTemplatesPath = StiDesigner.DefaultTemplatesPath;
		if (string.IsNullOrEmpty(defaultTemplatesPath))
		{
			return StiOptions.Configuration.ApplicationDirectory + "\\Templates\\";
		}
		return defaultTemplatesPath;
	}

	public string[] GetReportsNames()
	{
		string defaultTemplatesConfigPath = GetDefaultTemplatesConfigPath();
		if (Directory.Exists(defaultTemplatesConfigPath))
		{
			List<string> list = new List<string>();
			string[] files = Directory.GetFiles(defaultTemplatesConfigPath);
			foreach (string text in files)
			{
				if (File.Exists(text) && (text.EndsWith(".mrt") || text.EndsWith(".mrz") || text.EndsWith(".mrx")))
				{
					list.Add(Path.GetFileNameWithoutExtension(text));
				}
			}
			return list.ToArray();
		}
		return null;
	}

	public virtual StiReport GetReportTemplate(string reportName)
	{
		string text = GetDefaultTemplatesConfigPath() + reportName;
		string path = text + ".mrt";
		string path2 = text + ".mrz";
		if (File.Exists(path) || File.Exists(path2))
		{
			StiReport report = StiReport.GetReport();
			if (File.Exists(path))
			{
				report.Load(path);
			}
			if (File.Exists(path2))
			{
				report.Load(path2);
			}
			return report;
		}
		return null;
	}
}
