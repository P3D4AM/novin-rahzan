using System;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.SaveLoad;

public class StiXmlReportSLService : StiReportSLService
{
	public override Type ServiceType => typeof(StiReportSLService);

	public override void Save(StiReport report, Stream stream)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		report.IsSerializing = true;
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		try
		{
			val.Serializing += base.OnSaving;
			val.Serialize((object)report, stream, "StiReport");
		}
		finally
		{
			report.IsSerializing = false;
			val.Serializing -= base.OnSaving;
		}
	}

	public override void Load(StiReport report, Stream stream)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		report.Clear(generateNewScript: false);
		report.IsSerializing = true;
		StiDataCollection dataStore = report.DataStore;
		StiReportUnitType reportUnit = report.ReportUnit;
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		try
		{
			val.Deserializing += base.OnLoading;
			val.Deserialize((object)report, stream, "StiReport");
			for (int i = 0; i < report.Pages.Count; i++)
			{
				if (!(report.Pages[i] is IStiForm))
				{
					continue;
				}
				StiPage stiPage = report.Pages[i];
				int num = 0;
				while (num < stiPage.Components.Count)
				{
					StiComponent stiComponent = stiPage.Components[num];
					if (stiComponent == null)
					{
						stiPage.Components.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
			}
			for (int j = 0; j < report.Pages.Count; j++)
			{
				if (report.Pages[j] is IStiForm)
				{
					continue;
				}
				foreach (StiComponent component in report.Pages[j].GetComponents())
				{
					if (component.Page != null || component.Parent == null)
					{
						continue;
					}
					for (StiComponent parent = component.Parent; parent != null; parent = parent.Parent)
					{
						if (parent.Page != null)
						{
							component.Page = parent.Page;
							break;
						}
					}
				}
			}
			if (reportUnit != report.ReportUnit)
			{
				foreach (StiPage page in report.Pages)
				{
					if (!(page is IStiForm))
					{
						page.DockToContainer();
					}
				}
			}
			report.DataStore.Clear();
			report.DataStore.RegData(dataStore);
		}
		finally
		{
			val.Deserializing -= base.OnLoading;
			report.IsSerializing = false;
		}
	}

	public override StiSLActions GetAction()
	{
		return StiSLActions.Save | StiSLActions.Load;
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "ReportFiles");
	}
}
