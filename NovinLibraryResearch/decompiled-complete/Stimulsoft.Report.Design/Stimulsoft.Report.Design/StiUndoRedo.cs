using System.Collections.Generic;
using System.IO;
using System.Text;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design;

public class StiUndoRedo
{
	public string Name { get; set; }

	public object Value { get; set; }

	public List<StiResource> Resources { get; set; }

	public int CurrentPage { get; set; }

	public int PageIndex { get; set; } = -1;


	public StiPageType PageType { get; set; }

	public static StiUndoRedo CreateURFromObject(string name, object obj, int currentPage)
	{
		List<StiResource> resources = null;
		StiReport stiReport = null;
		if (obj is StiPage && ((StiPage)obj).Report != null)
		{
			stiReport = ((StiPage)obj).Report;
			((StiPage)obj).Report = null;
		}
		StiSerializing stiSerializing = new StiSerializing(new StiReportObjectStringConverter());
		StringBuilder stringBuilder = new StringBuilder();
		using (StringWriter stringWriter = new StringWriter(stringBuilder))
		{
			if (obj is StiReport stiReport2)
			{
				StiResourcesCollection resources2 = stiReport2.Dictionary.Resources;
				stiReport2.Dictionary.Resources = new StiResourcesCollection();
				stiSerializing.Serialize(obj, stringWriter, "StiUndoRedo", StiSerializeTypes.SerializeToAll);
				stiReport2.Dictionary.Resources = resources2;
				if (resources2.Count > 0)
				{
					resources = resources2.CloneAsList();
				}
			}
			else
			{
				stiSerializing.Serialize(obj, stringWriter, "StiUndoRedo", StiSerializeTypes.SerializeToAll);
			}
			stringWriter.Close();
		}
		if (stiReport != null)
		{
			((StiPage)obj).Report = stiReport;
		}
		return new StiUndoRedo(name, stringBuilder.ToString(), currentPage, resources);
	}

	public static object CreateObjectFromUR(StiUndoRedo ur, object obj)
	{
		using (StringReader stringReader = new StringReader(ur.Value as string))
		{
			StiSerializing stiSerializing = new StiSerializing(new StiReportObjectStringConverter());
			stiSerializing.Deserialize(obj, stringReader, "StiUndoRedo");
			stringReader.Close();
			if (obj is StiReport stiReport)
			{
				foreach (StiPage page in stiReport.Pages)
				{
					if (page.IsPage)
					{
						page.ReportUnit = stiReport.Unit;
					}
				}
				if (ur.Resources != null)
				{
					stiReport.Dictionary.Resources.Clear();
					ur.Resources.ForEach(stiReport.Dictionary.Resources.Add);
				}
			}
		}
		return obj;
	}

	public StiUndoRedo(string name, object value, int currentPage, List<StiResource> resources = null)
	{
		Name = name;
		Value = value;
		CurrentPage = currentPage;
		Resources = resources;
	}
}
