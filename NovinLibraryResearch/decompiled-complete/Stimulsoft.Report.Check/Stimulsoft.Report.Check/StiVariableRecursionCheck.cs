using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiVariableRecursionCheck : StiVariableCheck
{
	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckVariable", "StiVariableRecursionCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckVariable", "StiVariableRecursionCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Error;

	private bool CheckForRecursion(StiVariable variable, StiReport report)
	{
		Hashtable variablesHash = new Hashtable();
		return CheckForRecursion2(variable.Name, report, variablesHash);
	}

	private bool CheckForRecursion2(string name, StiReport report, Hashtable variablesHash)
	{
		try
		{
			if (variablesHash.ContainsKey(name))
			{
				return true;
			}
			StiVariable stiVariable = report.Dictionary.Variables[name];
			if (stiVariable == null || stiVariable.InitBy != StiVariableInitBy.Expression)
			{
				return false;
			}
			if (stiVariable.Type == typeof(DateTimeRange) && stiVariable.Value != null && stiVariable.Value.Contains("<<|>>"))
			{
				return false;
			}
			bool storeToPrint = false;
			StiText component = new StiText
			{
				Name = stiVariable.Name,
				Page = report.Pages[0]
			};
			object obj = StiParser.ParseTextValue("{" + stiVariable.Value + "}", component, ref storeToPrint, executeIfStoreToPrint: false, returnAsmList: true);
			if (obj is List<StiParser.StiAsmCommand> list)
			{
				foreach (StiParser.StiAsmCommand item in list)
				{
					if (item.Type == StiParser.StiAsmCommandType.PushVariable)
					{
						string text = (string)item.Parameter1;
						if (stiVariable.Name.Equals(text))
						{
							return true;
						}
						variablesHash[name] = null;
						if (CheckForRecursion2(text, report, variablesHash))
						{
							return true;
						}
						variablesHash.Remove(name);
					}
				}
			}
		}
		catch
		{
		}
		return false;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiVariable { InitBy: StiVariableInitBy.Expression } stiVariable && CheckForRecursion(stiVariable, report))
		{
			StiVariableRecursionCheck stiVariableRecursionCheck = new StiVariableRecursionCheck();
			stiVariableRecursionCheck.Element = obj;
			stiVariableRecursionCheck.Actions.Add(new StiEditPropertyAction());
			return stiVariableRecursionCheck;
		}
		return null;
	}
}
