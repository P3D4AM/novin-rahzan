using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.BlocklyEditor.Wpf.Helper;

public static class StiVariableHelper
{
	public static List<string> LoadVariables(StiReport report)
	{
		return (from v in report.Dictionary.Variables.ToList()
			select v.Name).ToList();
	}
}
