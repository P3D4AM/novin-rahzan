using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components;

[RefreshProperties(RefreshProperties.All)]
[TypeConverter(typeof(StiConditionConverter))]
public class StiIconSetCondition : StiBaseCondition, IStiIndicatorCondition
{
	private float? minimum;

	private float? maximum;

	[StiSerializable]
	[DefaultValue(StiIconSet.None)]
	public StiIconSet IconSet { get; set; }

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public ContentAlignment ContentAlignment { get; set; } = (ContentAlignment)16;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiIconSetItem IconSetItem1 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiIconSetItem IconSetItem2 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiIconSetItem IconSetItem3 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiIconSetItem IconSetItem4 { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiIconSetItem IconSetItem5 { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "IconSet", (Enum)IconSet, (Enum)StiIconSet.None);
		JsonExtensions.AddPropertyEnum(val, "ContentAlignment", (Enum)(object)ContentAlignment, (Enum)(object)(ContentAlignment)16);
		if (IconSetItem1 != null)
		{
			JsonExtensions.AddPropertyJObject(val, "IconSetItem1", IconSetItem1.SaveToJsonObject(mode));
		}
		if (IconSetItem2 != null)
		{
			JsonExtensions.AddPropertyJObject(val, "IconSetItem2", IconSetItem2.SaveToJsonObject(mode));
		}
		if (IconSetItem3 != null)
		{
			JsonExtensions.AddPropertyJObject(val, "IconSetItem3", IconSetItem3.SaveToJsonObject(mode));
		}
		if (IconSetItem4 != null)
		{
			JsonExtensions.AddPropertyJObject(val, "IconSetItem4", IconSetItem4.SaveToJsonObject(mode));
		}
		if (IconSetItem5 != null)
		{
			JsonExtensions.AddPropertyJObject(val, "IconSetItem5", IconSetItem5.SaveToJsonObject(mode));
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "IconSet":
				IconSet = StiJsonReportObjectHelper.DeserializeEnum<StiIconSet>(item);
				break;
			case "ContentAlignment":
				ContentAlignment = StiJsonReportObjectHelper.DeserializeEnum<ContentAlignment>(item);
				break;
			case "IconSetItem1":
			{
				StiIconSetItem stiIconSetItem5 = new StiIconSetItem();
				stiIconSetItem5.LoadFromJsonObject((JObject)item.Value);
				IconSetItem1 = stiIconSetItem5;
				break;
			}
			case "IconSetItem2":
			{
				StiIconSetItem stiIconSetItem4 = new StiIconSetItem();
				stiIconSetItem4.LoadFromJsonObject((JObject)item.Value);
				IconSetItem2 = stiIconSetItem4;
				break;
			}
			case "IconSetItem3":
			{
				StiIconSetItem stiIconSetItem3 = new StiIconSetItem();
				stiIconSetItem3.LoadFromJsonObject((JObject)item.Value);
				IconSetItem3 = stiIconSetItem3;
				break;
			}
			case "IconSetItem4":
			{
				StiIconSetItem stiIconSetItem2 = new StiIconSetItem();
				stiIconSetItem2.LoadFromJsonObject((JObject)item.Value);
				IconSetItem4 = stiIconSetItem2;
				break;
			}
			case "IconSetItem5":
			{
				StiIconSetItem stiIconSetItem = new StiIconSetItem();
				stiIconSetItem.LoadFromJsonObject((JObject)item.Value);
				IconSetItem5 = stiIconSetItem;
				break;
			}
			}
		}
	}

	public StiIndicator CreateIndicator(StiText component)
	{
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(Column))
		{
			component.Report.WriteToReportRenderingMessages("Column of Icon Set Condition of '" + component.Name + "' component is not specified!");
			return null;
		}
		if (!minimum.HasValue || !maximum.HasValue)
		{
			minimum = 0f;
			maximum = 0f;
			StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(component.Report.Dictionary, Column);
			if (dataSourceFromDataColumn != null)
			{
				dataSourceFromDataColumn.SaveState("Indicator");
				if (dataSourceFromDataColumn.DetailRows != null && dataSourceFromDataColumn.DataTable != null)
				{
					dataSourceFromDataColumn.DetailRows = null;
				}
				dataSourceFromDataColumn.First();
				int num = 0;
				while (!dataSourceFromDataColumn.IsEof)
				{
					object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(component.Report.Dictionary, Column);
					float num2 = 0f;
					try
					{
						num2 = (float)StiReport.ChangeType(dataFromDataColumn, typeof(float));
					}
					catch
					{
					}
					if (num == 0)
					{
						minimum = num2;
						maximum = num2;
					}
					else
					{
						minimum = Math.Min(minimum.GetValueOrDefault(), num2);
						maximum = Math.Max(maximum.GetValueOrDefault(), num2);
					}
					num++;
					dataSourceFromDataColumn.Next();
				}
				dataSourceFromDataColumn.RestoreState("Indicator");
			}
			if (dataSourceFromDataColumn == null)
			{
				StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(component.Report.Dictionary, Column);
				if (businessObjectFromDataColumn == null)
				{
					component.Report.WriteToReportRenderingMessages("Column of Data Bar Condition of '" + component.Name + "' component is not found!");
					return null;
				}
				businessObjectFromDataColumn.SaveState("Indicator");
				businessObjectFromDataColumn.CreateEnumerator();
				int num3 = 0;
				while (!businessObjectFromDataColumn.IsEof)
				{
					object dataFromDataColumn2 = StiDataColumn.GetDataFromDataColumn(component.Report.Dictionary, Column);
					float num4 = 0f;
					try
					{
						num4 = (float)StiReport.ChangeType(dataFromDataColumn2, typeof(float));
					}
					catch
					{
					}
					if (num3 == 0)
					{
						minimum = num4;
						maximum = num4;
					}
					else
					{
						minimum = Math.Min(minimum.GetValueOrDefault(), num4);
						maximum = Math.Max(maximum.GetValueOrDefault(), num4);
					}
					num3++;
					businessObjectFromDataColumn.Next();
				}
				businessObjectFromDataColumn.RestoreState("Indicator");
			}
		}
		float valueOrDefault = minimum.GetValueOrDefault();
		float valueOrDefault2 = maximum.GetValueOrDefault();
		float num5 = valueOrDefault2 - valueOrDefault;
		object dataFromDataColumn3 = StiDataColumn.GetDataFromDataColumn(component.Report.Dictionary, Column);
		float num6 = 0f;
		try
		{
			num6 = (float)StiReport.ChangeType(dataFromDataColumn3, typeof(float));
		}
		catch
		{
		}
		if (num6 > valueOrDefault2)
		{
			num6 = valueOrDefault2;
		}
		if (num6 < valueOrDefault)
		{
			num6 = valueOrDefault;
		}
		float floatCurrentValuePercent = (num6 - valueOrDefault) / num5 * 100f;
		StiIcon stiIcon = StiIcon.None;
		if (IconSetItem4 == null && IconSetItem5 == null)
		{
			if (InRange(IconSetItem1, num6, floatCurrentValuePercent))
			{
				stiIcon = IconSetItem1.Icon;
			}
			else if (InRange(IconSetItem2, num6, floatCurrentValuePercent))
			{
				stiIcon = IconSetItem2.Icon;
			}
			else if (IconSetItem3 != null)
			{
				stiIcon = IconSetItem3.Icon;
			}
		}
		else if (IconSetItem5 == null)
		{
			if (InRange(IconSetItem1, num6, floatCurrentValuePercent))
			{
				stiIcon = IconSetItem1.Icon;
			}
			else if (InRange(IconSetItem2, num6, floatCurrentValuePercent))
			{
				stiIcon = IconSetItem2.Icon;
			}
			else if (InRange(IconSetItem3, num6, floatCurrentValuePercent))
			{
				stiIcon = IconSetItem3.Icon;
			}
			else if (IconSetItem4 != null)
			{
				stiIcon = IconSetItem4.Icon;
			}
		}
		else if (InRange(IconSetItem1, num6, floatCurrentValuePercent))
		{
			stiIcon = IconSetItem1.Icon;
		}
		else if (InRange(IconSetItem2, num6, floatCurrentValuePercent))
		{
			stiIcon = IconSetItem2.Icon;
		}
		else if (InRange(IconSetItem3, num6, floatCurrentValuePercent))
		{
			stiIcon = IconSetItem3.Icon;
		}
		else if (InRange(IconSetItem4, num6, floatCurrentValuePercent))
		{
			stiIcon = IconSetItem4.Icon;
		}
		else if (IconSetItem5 != null)
		{
			stiIcon = IconSetItem5.Icon;
		}
		if (stiIcon == StiIcon.None)
		{
			return null;
		}
		return new StiIconSetIndicator
		{
			Icon = stiIcon,
			Alignment = ContentAlignment
		};
	}

	private bool InRange(StiIconSetItem item, float floatCurrentValue, float floatCurrentValuePercent)
	{
		if (item == null)
		{
			return false;
		}
		if (item.ValueType == StiIconSetValueType.Percent)
		{
			if (item.Operation == StiIconSetOperation.MoreThan)
			{
				if (floatCurrentValuePercent > item.Value)
				{
					return true;
				}
			}
			else if (floatCurrentValuePercent >= item.Value)
			{
				return true;
			}
		}
		else if (item.Operation == StiIconSetOperation.MoreThan)
		{
			if (floatCurrentValue > item.Value)
			{
				return true;
			}
		}
		else if (floatCurrentValue >= item.Value)
		{
			return true;
		}
		return false;
	}

	public void Reset()
	{
		minimum = null;
		maximum = null;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		if (!(obj is StiIconSetCondition stiIconSetCondition))
		{
			return false;
		}
		if (IconSet.Equals(stiIconSetCondition.IconSet))
		{
			ContentAlignment contentAlignment = ContentAlignment;
			return ((object)(ContentAlignment)(ref contentAlignment)).Equals((object?)stiIconSetCondition.ContentAlignment);
		}
		return false;
	}

	public StiIconSetCondition()
	{
	}//IL_0003: Unknown result type (might be due to invalid IL or missing references)


	public StiIconSetCondition(string column, StiIconSet iconSet, ContentAlignment contentAlignment, StiIconSetItem item1, StiIconSetItem item2, StiIconSetItem item3, StiIconSetItem item4, StiIconSetItem item5)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		Column = column;
		IconSet = iconSet;
		ContentAlignment = contentAlignment;
		IconSetItem1 = item1;
		IconSetItem2 = item2;
		IconSetItem3 = item3;
		IconSetItem4 = item4;
		IconSetItem5 = item5;
	}
}
