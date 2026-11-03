using System;
using System.IO;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.SaveLoad;

public class StiJsonReportSLService : StiReportSLService
{
	public override Type ServiceType => typeof(StiReportSLService);

	public override void Save(StiReport report, Stream stream)
	{
		report.IsSerializing = true;
		try
		{
			string s = report.SaveToJsonInternal((StiJsonSaveMode)0);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			stream.Write(bytes, 0, bytes.Length);
			stream.Flush();
		}
		finally
		{
			report.IsSerializing = false;
		}
	}

	public override void Load(StiReport report, Stream stream)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		report.Clear(generateNewScript: false);
		report.Dictionary.Clear();
		report.Dictionary.Databases.Clear();
		report.Dictionary.Resources.Clear();
		report.IsSerializing = true;
		StiDataCollection dataStore = report.DataStore;
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		try
		{
			string text = null;
			using (StreamReader streamReader = new StreamReader(stream))
			{
				text = streamReader.ReadToEnd();
			}
			report.LoadFromJsonInternal(text);
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
		return StiLocalization.Get("FileFilters", "JsonReportFiles");
	}
}
