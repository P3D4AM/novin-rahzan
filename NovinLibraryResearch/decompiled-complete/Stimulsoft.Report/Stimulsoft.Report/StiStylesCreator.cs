using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

public class StiStylesCreator
{
	private StiReport report;

	private List<StiBaseStyle> hashAllStyles;

	public bool ShowReportTitles { get; set; } = true;


	public bool ShowReportSummaries { get; set; } = true;


	public bool ShowPageHeaders { get; set; } = true;


	public bool ShowPageFooters { get; set; } = true;


	public bool ShowGroupHeaders { get; set; } = true;


	public bool ShowGroupFooters { get; set; } = true;


	public bool ShowHeaders { get; set; } = true;


	public bool ShowDatas { get; set; } = true;


	public bool ShowFooters { get; set; } = true;


	public bool ShowBorders { get; set; }

	private float ColorFactor
	{
		get
		{
			if (NestedFactor == StiNestedFactor.High)
			{
				return 1.5f;
			}
			if (NestedFactor == StiNestedFactor.Normal)
			{
				return 1f;
			}
			return 0.5f;
		}
	}

	public int MaxNestedLevel { get; set; } = 3;


	public StiNestedFactor NestedFactor { get; set; } = StiNestedFactor.Normal;


	public List<StiBaseStyle> CreateStyles(string collectionName, Color baseColor)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Expected O, but got Unknown
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Expected O, but got Unknown
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Expected O, but got Unknown
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Expected O, but got Unknown
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Expected O, but got Unknown
		//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Expected O, but got Unknown
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Expected O, but got Unknown
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_093c: Expected O, but got Unknown
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Expected O, but got Unknown
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Expected O, but got Unknown
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Expected O, but got Unknown
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Expected O, but got Unknown
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Expected O, but got Unknown
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_0757: Expected O, but got Unknown
		//IL_098c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Expected O, but got Unknown
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Expected O, but got Unknown
		if (baseColor.IsEmpty || baseColor == Color.FromArgb(0, 0, 0, 0))
		{
			baseColor = Color.White;
		}
		float num = (float)(baseColor.R + baseColor.G + baseColor.B) / 3f;
		Color color = StiColorUtils.Dark(baseColor, (byte)(100f * ColorFactor));
		StiBorder val = new StiBorder((StiBorderSides)0, color, 1.0, (StiPenStyle)0);
		StiStyleComponentType value = StiStyleComponentType.Text | StiStyleComponentType.Primitive | StiStyleComponentType.Image | StiStyleComponentType.CheckBox;
		List<StiBaseStyle> list = new List<StiBaseStyle>();
		if (ShowReportTitles)
		{
			Font font = new Font("Arial", 14f, (FontStyle)1);
			Color color2 = ((num > 150f) ? StiColorUtils.Dark(baseColor, (byte)200) : baseColor);
			StiSolidBrush headerBrush = new StiSolidBrush(Color.Transparent);
			StiSolidBrush headerForeBrush = new StiSolidBrush(color2);
			string text = StiFunctionsStrings.ToProperCase(Loc.Get("Components", "StiReportTitleBand")).Replace(" ", "_");
			string name = collectionName + "_" + text + "1";
			CreateStyles(name, ShowBorders, font, (StiBrush)(object)headerBrush, (StiBrush)(object)headerForeBrush, val, value, StiStyleComponentPlacement.ReportTitle, 1, StiStyleConditionOperation.EqualTo, list);
			font = new Font("Arial", 10f, (FontStyle)1);
			string name2 = collectionName + "_" + text + "2";
			CreateStyles(name2, ShowBorders, font, (StiBrush)(object)headerBrush, (StiBrush)(object)headerForeBrush, val, value, StiStyleComponentPlacement.ReportTitle, 2, StiStyleConditionOperation.GreaterThanOrEqualTo, list);
		}
		if (ShowReportSummaries)
		{
			Font font2 = new Font("Arial", 12f, (FontStyle)1);
			Color color3 = ((num > 150f) ? StiColorUtils.Dark(baseColor, (byte)200) : baseColor);
			StiSolidBrush headerBrush2 = new StiSolidBrush(Color.Transparent);
			StiSolidBrush headerForeBrush2 = new StiSolidBrush(color3);
			string text2 = StiFunctionsStrings.ToProperCase(Loc.Get("Components", "StiReportSummaryBand")).Replace(" ", "_");
			string name3 = collectionName + "_" + text2 + "1";
			CreateStyles(name3, ShowBorders, font2, (StiBrush)(object)headerBrush2, (StiBrush)(object)headerForeBrush2, val, value, StiStyleComponentPlacement.ReportSummary, 1, StiStyleConditionOperation.EqualTo, list);
			font2 = new Font("Arial", 10f, (FontStyle)1);
			string name4 = collectionName + "_" + text2 + "2";
			CreateStyles(name4, ShowBorders, font2, (StiBrush)(object)headerBrush2, (StiBrush)(object)headerForeBrush2, val, value, StiStyleComponentPlacement.ReportSummary, 2, StiStyleConditionOperation.GreaterThanOrEqualTo, list);
		}
		if (ShowPageHeaders)
		{
			Font font3 = new Font("Arial", 10f);
			Color color4 = ((num > 150f) ? StiColorUtils.Dark(baseColor, (byte)200) : baseColor);
			StiSolidBrush headerBrush3 = new StiSolidBrush(Color.Transparent);
			StiSolidBrush headerForeBrush3 = new StiSolidBrush(color4);
			string text3 = StiFunctionsStrings.ToProperCase(Loc.Get("Components", "StiPageHeaderBand")).Replace(" ", "_");
			text3 = collectionName + "_" + text3;
			CreateStyles(text3, ShowBorders, font3, (StiBrush)(object)headerBrush3, (StiBrush)(object)headerForeBrush3, val, value, StiStyleComponentPlacement.PageHeader, null, null, list);
		}
		if (ShowPageFooters)
		{
			Font font4 = new Font("Arial", 10f);
			Color color5 = ((num > 150f) ? StiColorUtils.Dark(baseColor, (byte)200) : baseColor);
			StiSolidBrush headerBrush4 = new StiSolidBrush(Color.Transparent);
			StiSolidBrush headerForeBrush4 = new StiSolidBrush(color5);
			string text4 = StiFunctionsStrings.ToProperCase(Loc.Get("Components", "StiPageFooterBand")).Replace(" ", "_");
			text4 = collectionName + "_" + text4;
			CreateStyles(text4, ShowBorders, font4, (StiBrush)(object)headerBrush4, (StiBrush)(object)headerForeBrush4, val, value, StiStyleComponentPlacement.PageFooter, null, null, list);
		}
		if (ShowGroupHeaders)
		{
			float num2 = 4f / (float)MaxNestedLevel;
			Color color6 = ((num > 150f) ? StiColorUtils.Dark(baseColor, (byte)150) : baseColor);
			StiSolidBrush headerForeBrush5 = new StiSolidBrush(color6);
			for (int i = 1; i <= MaxNestedLevel; i++)
			{
				int num3 = (int)(num2 * (float)(i - 1));
				Font font5 = new Font("Arial", (float)(13 - num3), (FontStyle)1);
				StiSolidBrush headerBrush5 = new StiSolidBrush(Color.Transparent);
				StiStyleConditionOperation value2 = ((i == MaxNestedLevel) ? StiStyleConditionOperation.GreaterThanOrEqualTo : StiStyleConditionOperation.EqualTo);
				string arg = StiFunctionsStrings.ToProperCase(Loc.Get("Components", "StiGroupHeaderBand")).Replace(" ", "_");
				arg = $"{collectionName}_{arg}{i}";
				CreateStyles(arg, ShowBorders, font5, (StiBrush)(object)headerBrush5, (StiBrush)(object)headerForeBrush5, val, value, StiStyleComponentPlacement.GroupHeader, i, value2, list);
			}
		}
		if (ShowGroupFooters)
		{
			float num4 = 4f / (float)MaxNestedLevel;
			Color color7 = ((num > 150f) ? StiColorUtils.Dark(baseColor, (byte)150) : baseColor);
			StiSolidBrush headerForeBrush6 = new StiSolidBrush(color7);
			for (int j = 1; j <= MaxNestedLevel; j++)
			{
				int num5 = (int)(num4 * (float)(j - 1));
				Font font6 = new Font("Arial", (float)(13 - num5), (FontStyle)1);
				StiSolidBrush headerBrush6 = new StiSolidBrush(Color.Transparent);
				StiStyleConditionOperation value3 = ((j == 0) ? StiStyleConditionOperation.GreaterThanOrEqualTo : StiStyleConditionOperation.EqualTo);
				string arg2 = StiFunctionsStrings.ToProperCase(Loc.Get("Components", "StiGroupFooterBand")).Replace(" ", "_");
				arg2 = $"{collectionName}_{arg2}{j}";
				CreateStyles(arg2, ShowBorders, font6, (StiBrush)(object)headerBrush6, (StiBrush)(object)headerForeBrush6, val, value, StiStyleComponentPlacement.GroupFooter, j, value3, list);
			}
		}
		if (ShowHeaders)
		{
			int num6 = 100 / MaxNestedLevel;
			Font font7 = new Font("Arial", 8f, (FontStyle)1);
			Color color8 = StiColorUtils.Dark(baseColor, (byte)200);
			StiSolidBrush headerForeBrush7 = new StiSolidBrush(color8);
			val.Topmost = true;
			for (int k = 1; k <= MaxNestedLevel; k++)
			{
				int num7 = (int)((float)num6 * (float)(k - 1) * ColorFactor);
				if (num7 > 255)
				{
					num7 = 255;
				}
				StiSolidBrush headerBrush7 = new StiSolidBrush(StiColorUtils.Light(baseColor, (byte)num7));
				StiStyleConditionOperation value4 = ((k == MaxNestedLevel) ? StiStyleConditionOperation.GreaterThanOrEqualTo : StiStyleConditionOperation.EqualTo);
				string arg3 = StiFunctionsStrings.ToProperCase(Loc.Get("Components", "StiHeaderBand")).Replace(" ", "_");
				arg3 = $"{collectionName}_{arg3}{k}";
				CreateStyles(arg3, ShowBorders, font7, (StiBrush)(object)headerBrush7, (StiBrush)(object)headerForeBrush7, val, value, StiStyleComponentPlacement.Header, k, value4, list);
			}
		}
		if (ShowDatas)
		{
			int num8 = 60 / MaxNestedLevel;
			Font font8 = new Font("Arial", 8f);
			Color color9 = StiColorUtils.Dark(baseColor, (byte)200);
			StiSolidBrush headerForeBrush8 = new StiSolidBrush(color9);
			for (int l = 1; l <= MaxNestedLevel; l++)
			{
				int num9 = (int)((150f + (float)num8 * (float)(l - 1)) * ColorFactor);
				if (num9 > 255)
				{
					num9 = 255;
				}
				StiSolidBrush headerBrush8 = new StiSolidBrush(StiColorUtils.Light(baseColor, (byte)num9));
				StiSolidBrush headerBrush9 = new StiSolidBrush(StiColorUtils.Light(baseColor, (byte)((double)(float)num9 * 0.5)));
				StiStyleConditionOperation value5 = ((l == MaxNestedLevel) ? StiStyleConditionOperation.GreaterThanOrEqualTo : StiStyleConditionOperation.EqualTo);
				string arg4 = StiFunctionsStrings.ToProperCase(Loc.Get("Components", "StiDataBand")).Replace(" ", "_");
				arg4 = $"{collectionName}_{arg4}{l}";
				CreateStyles(arg4, ShowBorders, font8, (StiBrush)new StiSolidBrush(Color.Transparent), (StiBrush)(object)headerForeBrush8, val, value, StiStyleComponentPlacement.Data, l, value5, list);
				arg4 = StiFunctionsStrings.ToProperCase(Loc.Get("Components", "StiDataBand")).Replace(" ", "_");
				string text5 = Loc.Get("PropertyMain", "OddStyle");
				arg4 = $"{collectionName}_{arg4}{l}_{text5}";
				CreateStyles(arg4, ShowBorders, font8, (StiBrush)(object)headerBrush8, (StiBrush)(object)headerForeBrush8, val, null, StiStyleComponentPlacement.DataOddStyle, l, value5, list);
				arg4 = StiFunctionsStrings.ToProperCase(Loc.Get("Components", "StiDataBand")).Replace(" ", "_");
				string text6 = Loc.Get("PropertyMain", "EvenStyle");
				arg4 = $"{collectionName}_{arg4}{l}_{text6}";
				CreateStyles(arg4, ShowBorders, font8, (StiBrush)(object)headerBrush9, (StiBrush)(object)headerForeBrush8, val, null, StiStyleComponentPlacement.DataEvenStyle, l, value5, list);
			}
		}
		if (ShowFooters)
		{
			int num10 = 100 / MaxNestedLevel;
			Font font9 = new Font("Arial", 8f);
			Color color10 = StiColorUtils.Dark(baseColor, (byte)200);
			StiSolidBrush headerForeBrush9 = new StiSolidBrush(color10);
			for (int m = 1; m <= MaxNestedLevel; m++)
			{
				int num11 = (int)(((float)num10 * (float)(m - 1) + 20f) * ColorFactor);
				if (num11 > 255)
				{
					num11 = 255;
				}
				StiSolidBrush headerBrush10 = new StiSolidBrush(StiColorUtils.Light(baseColor, (byte)num11));
				StiStyleConditionOperation value6 = ((m == 0) ? StiStyleConditionOperation.GreaterThanOrEqualTo : StiStyleConditionOperation.EqualTo);
				string arg5 = StiFunctionsStrings.ToProperCase(Loc.Get("Components", "StiFooterBand")).Replace(" ", "_");
				arg5 = $"{collectionName}_{arg5}{m}";
				CreateStyles(arg5, ShowBorders, font9, (StiBrush)(object)headerBrush10, (StiBrush)(object)headerForeBrush9, val, value, StiStyleComponentPlacement.Footer, m, value6, list);
			}
		}
		foreach (StiBaseStyle item in list)
		{
			item.CollectionName = collectionName;
		}
		return list;
	}

	private void CreateStyles(string name, bool showBorders, Font font, StiBrush headerBrush1, StiBrush headerForeBrush, StiBorder border, StiStyleComponentType? simpleCompTypes, StiStyleComponentPlacement placement, int? placementNestedLevel, StiStyleConditionOperation? operation, List<StiBaseStyle> styles)
	{
		List<StiBaseStyle> list = CreateStyles(name, font, headerBrush1, headerForeBrush, border, placement, placementNestedLevel, operation, simpleCompTypes, showBorders);
		foreach (StiBaseStyle item in list)
		{
			styles.Add(item);
		}
	}

	private List<StiBaseStyle> CreateStyles(string name, Font font, StiBrush brush, StiBrush textBrush, StiBorder border, StiStyleComponentPlacement placement, int? placementNestedLevel, StiStyleConditionOperation? placementNestedLevelOperation, StiStyleComponentType? componentType, bool showBorders)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		List<StiBaseStyle> list = new List<StiBaseStyle>();
		border = new StiBorder((StiBorderSides)0, border.Color, border.Size, border.Style, border.DropShadow, border.ShadowSize, border.ShadowBrush, border.Topmost);
		StiStyle stiStyle = CreateStyle(name, font, brush, textBrush, border, placement, placementNestedLevel, placementNestedLevelOperation, componentType);
		if (placement == StiStyleComponentPlacement.ReportTitle || placement == StiStyleComponentPlacement.ReportSummary || placement == StiStyleComponentPlacement.PageFooter || placement == StiStyleComponentPlacement.PageHeader)
		{
			stiStyle.AllowUseBorderSidesFromLocation = false;
		}
		else
		{
			stiStyle.AllowUseBorderSidesFromLocation = ShowBorders;
		}
		list.Add(stiStyle);
		return list;
	}

	private StiStyle CreateStyle(string name, Font font, StiBrush brush, StiBrush textBrush, StiBorder border, StiStyleComponentPlacement placement, int? placementNestedLevel, StiStyleConditionOperation? placementNesterLevelOperation, StiStyleComponentType? componentType)
	{
		StiStyle stiStyle = new StiStyle(GetStyleName(name))
		{
			Font = font,
			Brush = brush,
			TextBrush = textBrush,
			Border = border
		};
		List<StiStyleConditionElement> list = new List<StiStyleConditionElement>();
		if (placementNesterLevelOperation.HasValue && placementNestedLevel.HasValue)
		{
			list.Add(new StiStyleConditionPlacementNestedLevelElement(placementNestedLevel.Value, placementNesterLevelOperation.Value));
		}
		if (componentType.HasValue)
		{
			list.Add(new StiStyleConditionComponentTypeElement(componentType.Value));
		}
		list.Add(new StiStyleConditionPlacementElement(placement));
		stiStyle.Conditions.Add(list.ToArray());
		return stiStyle;
	}

	private string GetStyleName(string baseName)
	{
		string name = baseName;
		int num = 2;
		while (true)
		{
			bool flag = false;
			if (report != null)
			{
				foreach (StiBaseStyle style in report.Styles)
				{
					if (style.Name == name)
					{
						flag = true;
						break;
					}
				}
			}
			else
			{
				flag = hashAllStyles.FirstOrDefault((StiBaseStyle x) => x.Name == name) != null;
			}
			if (!flag)
			{
				break;
			}
			name = $"{baseName}_{num}";
			num++;
		}
		return name;
	}

	public StiStylesCreator(StiReport report)
	{
		this.report = report;
	}

	public StiStylesCreator(List<StiBaseStyle> hashAllStyles)
	{
		this.hashAllStyles = hashAllStyles;
	}
}
