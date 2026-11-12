using System.Collections.Generic;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCalculatedColumnRecursionCheck : StiDataColumnCheck
{
	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckDataSource", "StiCalculatedColumnRecursionCheckShort"), ElementName);

	public override string LongMessage
	{
		get
		{
			StiDataSource stiDataSource = ((base.Element is StiCalcDataColumn stiCalcDataColumn) ? stiCalcDataColumn.DataSource : null);
			string arg = ((stiDataSource != null) ? stiDataSource.Name : string.Empty);
			return string.Format(StiLocalizationExt.Get("CheckDataSource", "StiCalculatedColumnRecursionCheckLong"), ElementName, arg);
		}
	}

	public override StiCheckStatus Status => StiCheckStatus.Error;

	private bool CheckForRecursion(StiCalcDataColumn column, StiReport report)
	{
		try
		{
			string text = column.DataSource.Name + "." + column.Name;
			bool storeToPrint = false;
			StiText component = new StiText
			{
				Name = column.Name,
				Page = report.Pages[0]
			};
			object obj = StiParser.ParseTextValue("{" + column.Expression + "}", component, ref storeToPrint, executeIfStoreToPrint: false, returnAsmList: true);
			if (obj is List<StiParser.StiAsmCommand> list)
			{
				foreach (StiParser.StiAsmCommand item in list)
				{
					if (item.Type == StiParser.StiAsmCommandType.PushDataSourceField && text.Equals(item.Parameter1))
					{
						return true;
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
		if (obj is StiCalcDataColumn column && CheckForRecursion(column, report))
		{
			StiCalculatedColumnRecursionCheck stiCalculatedColumnRecursionCheck = new StiCalculatedColumnRecursionCheck();
			stiCalculatedColumnRecursionCheck.Element = obj;
			stiCalculatedColumnRecursionCheck.Actions.Add(new StiEditPropertyAction());
			return stiCalculatedColumnRecursionCheck;
		}
		return null;
	}
}
