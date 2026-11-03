using System;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.SaveLoad;

public class StiJsonPageSLService : StiPageSLService
{
	public override Type ServiceType => typeof(StiPageSLService);

	public override void Save(StiPage page, Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		string name = page.Name;
		page.IgnoreNamingRule = true;
		page.Name = page.Name + ":Unit:" + page.Unit.GetType();
		page.IgnoreNamingRule = false;
		StiReport report = page.Report;
		page.ReportUnit = page.Unit;
		page.Report = null;
		string text = ((!(page is IStiForm)) ? "StiPage" : "StiForm");
		val.Serialize((object)page, stream, text);
		page.Name = name;
		page.Report = report;
		page.ReportUnit = null;
	}

	public override void Load(StiPage page, Stream stream)
	{
		StiReport report = page.Report;
		string name = page.Name;
		page.IgnoreNamingRule = true;
		report.IsSerializing = true;
		string text;
		using (StreamReader streamReader = new StreamReader(stream))
		{
			text = streamReader.ReadToEnd();
		}
		page.LoadFromJsonInternal(text);
		report.IsSerializing = false;
		if (page.Name.IndexOf(":Unit", StringComparison.InvariantCulture) != -1)
		{
			StiUnit stiUnit = null;
			if (page.Name.IndexOf("Centimeters", StringComparison.InvariantCulture) != -1)
			{
				stiUnit = new StiCentimetersUnit();
			}
			if (page.Name.IndexOf("HundredthsOfInch", StringComparison.InvariantCulture) != -1)
			{
				stiUnit = new StiHundredthsOfInchUnit();
			}
			if (page.Name.IndexOf("Inches", StringComparison.InvariantCulture) != -1)
			{
				stiUnit = new StiInchesUnit();
			}
			if (page.Name.IndexOf("Millimeters", StringComparison.InvariantCulture) != -1)
			{
				stiUnit = new StiMillimetersUnit();
			}
			if (stiUnit != report.Unit)
			{
				page.Convert(stiUnit, report.Unit);
			}
		}
		page.Name = name;
		page.IgnoreNamingRule = false;
		page.Report = report;
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
