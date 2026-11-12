using System;
using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiExpressionElementCheck : StiComponentCheck
{
	public override bool DefaultStateEnabled => true;

	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiExpressionElementCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiExpressionElementCheckLong"), Expression, ElementName, Message);

	public string Expression { get; set; }

	public string Message { get; set; }

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		try
		{
			if (!(base.Element is IStiElement stiElement))
			{
				return null;
			}
			IStiAppDictionary stiAppDictionary = stiElement?.GetApp().GetDictionary();
			List<IStiMeter> list = stiElement?.FetchAllMeters();
			if (list == null)
			{
				return null;
			}
			foreach (IStiMeter item in list)
			{
				if (!string.IsNullOrWhiteSpace(item.Expression))
				{
					try
					{
						StiExpressionHelper.Compile(item.Expression);
					}
					catch (Exception ex)
					{
						StiExpressionElementCheck stiExpressionElementCheck = new StiExpressionElementCheck
						{
							Element = obj,
							Message = ex.Message,
							Expression = item.Expression
						};
						stiExpressionElementCheck.Actions.Add(new StiEditPropertyAction());
						return stiExpressionElementCheck;
					}
				}
			}
		}
		finally
		{
			base.Element = null;
		}
		return null;
	}
}
