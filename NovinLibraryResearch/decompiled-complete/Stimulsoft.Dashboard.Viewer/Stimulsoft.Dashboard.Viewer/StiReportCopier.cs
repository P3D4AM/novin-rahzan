using System.IO;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Viewer;

public class StiReportCopier
{
	public static StiReport Copy(StiReport report)
	{
		StiReport reportCopy;
		if (!StiOptions.Engine.FullTrust || report.CalculationMode == StiCalculationMode.Interpretation || StiOptions.Engine.ForceInterpretationMode || report.NeedsCompiling)
		{
			reportCopy = new StiReport();
			MemoryStream memoryStream = new MemoryStream();
			report.Save(memoryStream);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			reportCopy.Load(memoryStream);
			memoryStream.Close();
		}
		else
		{
			reportCopy = StiActivator.CreateObject(report.GetType()) as StiReport;
		}
		reportCopy.Pages.ToList().ForEach(delegate(StiPage p)
		{
			p.Report = reportCopy;
		});
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			StiVariable stiVariable2 = reportCopy.Dictionary.Variables[variable.Name];
			if (stiVariable2 != null)
			{
				stiVariable2.DialogInfo.Keys = variable.DialogInfo.Keys?.Clone() as string[];
				stiVariable2.DialogInfo.Values = variable.DialogInfo.Values?.Clone() as string[];
				stiVariable2.DialogInfo.CheckedStates = variable.DialogInfo.CheckedStates?.Clone() as bool[];
			}
			FieldInfo field = report.GetType().GetField(variable.Name);
			if (field != null)
			{
				reportCopy[variable.Name] = field.GetValue(report);
			}
		}
		if (report.Variables != null && report.Variables.Count > 0)
		{
			foreach (string key in report.Variables.Keys)
			{
				PropertyInfo property = report.GetType().GetProperty(key);
				if (property != null && !property.CanWrite)
				{
					reportCopy[key] = report.Variables[key];
				}
				else
				{
					reportCopy[key] = report[key];
				}
			}
		}
		reportCopy.Dictionary.Variables.ToList().ForEach(delegate(StiVariable v)
		{
			v.RequestFromUser = false;
		});
		reportCopy.RegData(report.DataStore);
		reportCopy.RegBusinessObject(report.BusinessObjectsStore);
		reportCopy.Dictionary.Resources.Clear();
		reportCopy.Dictionary.Resources.AddRange(report.Dictionary.Resources);
		return reportCopy;
	}
}
