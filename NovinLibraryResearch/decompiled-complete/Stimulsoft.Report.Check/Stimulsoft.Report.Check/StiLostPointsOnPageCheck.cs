using System.Collections.Generic;
using System.Text;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiLostPointsOnPageCheck : StiPageCheck
{
	private string lostPointsNames = string.Empty;

	public override bool PreviewVisible => false;

	private StiPage Page => base.Element as StiPage;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckPage", "StiLostPointsOnPageCheckShort"), ElementName, LostPointsNames);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckPage", "StiLostPointsOnPageCheckLong"), ElementName, LostPointsNames);

	public string LostPointsNames
	{
		get
		{
			return lostPointsNames;
		}
		set
		{
			lostPointsNames = value;
		}
	}

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	public static List<StiPointPrimitive> GetLostPointsOnPage(StiPage page)
	{
		StiComponentsCollection components = page.GetComponents();
		Dictionary<StiPointPrimitive, StiPointPrimitive> dictionary = new Dictionary<StiPointPrimitive, StiPointPrimitive>();
		List<StiCrossLinePrimitive> list = new List<StiCrossLinePrimitive>();
		foreach (StiComponent item in components)
		{
			if (item is StiPointPrimitive)
			{
				dictionary.Add(item as StiPointPrimitive, item as StiPointPrimitive);
			}
			else if (item is StiCrossLinePrimitive)
			{
				list.Add(item as StiCrossLinePrimitive);
			}
		}
		foreach (StiCrossLinePrimitive item2 in list)
		{
			StiStartPointPrimitive startPoint = item2.GetStartPoint(item2.Page);
			StiEndPointPrimitive endPoint = item2.GetEndPoint(item2.Page);
			if (startPoint != null && dictionary.ContainsKey(startPoint))
			{
				dictionary.Remove(startPoint);
			}
			if (endPoint != null && dictionary.ContainsKey(endPoint))
			{
				dictionary.Remove(endPoint);
			}
		}
		List<StiPointPrimitive> list2 = new List<StiPointPrimitive>();
		foreach (StiPointPrimitive value in dictionary.Values)
		{
			list2.Add(value);
		}
		return list2;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		StiPage page = obj as StiPage;
		List<StiPointPrimitive> lostPointsOnPage = GetLostPointsOnPage(page);
		if (lostPointsOnPage != null && lostPointsOnPage.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (StiPointPrimitive item in lostPointsOnPage)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder = stringBuilder.Append(", ");
				}
				stringBuilder = stringBuilder.Append(item.Name);
			}
			StiLostPointsOnPageCheck stiLostPointsOnPageCheck = new StiLostPointsOnPageCheck();
			stiLostPointsOnPageCheck.Element = obj;
			stiLostPointsOnPageCheck.lostPointsNames = stringBuilder.ToString();
			stiLostPointsOnPageCheck.Actions.Add(new StiDeleteLostPointsAction());
			return stiLostPointsOnPageCheck;
		}
		return null;
	}
}
