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
public class StiDataBarCondition : StiBaseCondition, IStiDataBarIndicator, IStiIndicatorCondition
{
	private float? minimum;

	private float? maximum;

	[StiSerializable]
	public StiBrushType BrushType { get; set; } = StiBrushType.Gradient;


	[StiSerializable]
	public Color PositiveColor { get; set; } = Color.Green;


	[StiSerializable]
	public Color NegativeColor { get; set; } = Color.Red;


	[StiSerializable]
	public Color PositiveBorderColor { get; set; } = Color.DarkGreen;


	[StiSerializable]
	public Color NegativeBorderColor { get; set; } = Color.DarkRed;


	[StiSerializable]
	[DefaultValue(false)]
	public bool ShowBorder { get; set; }

	[StiSerializable]
	[DefaultValue(StiDataBarDirection.Default)]
	public StiDataBarDirection Direction { get; set; }

	[StiSerializable]
	[DefaultValue(StiMinimumType.Auto)]
	public StiMinimumType MinimumType { get; set; }

	[StiSerializable]
	[DefaultValue(0f)]
	public float MinimumValue { get; set; }

	[StiSerializable]
	[DefaultValue(StiMaximumType.Auto)]
	public StiMaximumType MaximumType { get; set; }

	[StiSerializable]
	[DefaultValue(100f)]
	public float MaximumValue { get; set; } = 100f;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "BrushType", (Enum)BrushType, (Enum)StiBrushType.Gradient);
		JsonExtensions.AddPropertyColor(val, "PositiveColor", PositiveColor, Color.Green);
		JsonExtensions.AddPropertyColor(val, "NegativeColor", NegativeColor, Color.Red);
		JsonExtensions.AddPropertyColor(val, "PositiveBorderColor", PositiveBorderColor, Color.DarkGreen);
		JsonExtensions.AddPropertyColor(val, "NegativeBorderColor", NegativeBorderColor, Color.DarkRed);
		JsonExtensions.AddPropertyBool(val, "ShowBorder", ShowBorder, false, false);
		JsonExtensions.AddPropertyEnum(val, "Direction", (Enum)Direction, (Enum)StiDataBarDirection.Default);
		JsonExtensions.AddPropertyEnum(val, "MinimumType", (Enum)MinimumType, (Enum)StiMinimumType.Auto);
		JsonExtensions.AddPropertyFloat(val, "MinimumValue", MinimumValue, 0f);
		JsonExtensions.AddPropertyEnum(val, "MaximumType", (Enum)MaximumType, (Enum)StiMaximumType.Auto);
		JsonExtensions.AddPropertyFloat(val, "MaximumValue", MaximumValue, 100f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "BrushType":
				BrushType = StiJsonReportObjectHelper.DeserializeEnum<StiBrushType>(item);
				break;
			case "PositiveColor":
				PositiveColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "NegativeColor":
				NegativeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "PositiveBorderColor":
				PositiveBorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "NegativeBorderColor":
				NegativeBorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "ShowBorder":
				ShowBorder = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Direction":
				Direction = StiJsonReportObjectHelper.DeserializeEnum<StiDataBarDirection>(item);
				break;
			case "MinimumType":
				MinimumType = StiJsonReportObjectHelper.DeserializeEnum<StiMinimumType>(item);
				break;
			case "MinimumValue":
				MinimumValue = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "MaximumType":
				MaximumType = StiJsonReportObjectHelper.DeserializeEnum<StiMaximumType>(item);
				break;
			case "MaximumValue":
				MaximumValue = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	public StiIndicator CreateIndicator(StiText component)
	{
		if (string.IsNullOrEmpty(Column))
		{
			component.Report.WriteToReportRenderingMessages("Column of Data Bar Condition of '" + component.Name + "' component is not specified!");
			return null;
		}
		if ((MinimumType != StiMinimumType.Value || MaximumType != StiMaximumType.Value) && (!minimum.HasValue || !maximum.HasValue))
		{
			minimum = 0f;
			maximum = 0f;
			StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(component.Report.Dictionary, Column);
			if (dataSourceFromDataColumn != null)
			{
				dataSourceFromDataColumn.SaveState("Indicator");
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
			if (minimum > 0f && MinimumType != StiMinimumType.Minimum)
			{
				minimum = 0f;
			}
			if (maximum < 0f && MaximumType != StiMaximumType.Maximum)
			{
				maximum = 0f;
			}
		}
		float num5 = minimum.GetValueOrDefault();
		float num6 = maximum.GetValueOrDefault();
		float num7 = num6 - num5;
		float minimumValue = MinimumValue;
		minimumValue = Math.Min(minimumValue, 100f);
		minimumValue = Math.Max(minimumValue, 0f);
		float maximumValue = MaximumValue;
		maximumValue = Math.Min(maximumValue, 100f);
		maximumValue = Math.Max(maximumValue, 0f);
		if (minimumValue > maximumValue)
		{
			minimumValue = maximumValue;
		}
		if (MinimumType == StiMinimumType.Value)
		{
			num5 = MinimumValue;
		}
		else if (MinimumType == StiMinimumType.Percent)
		{
			num5 += num7 * minimumValue / 100f;
		}
		if (MaximumType == StiMaximumType.Value)
		{
			num6 = MaximumValue;
		}
		else if (MaximumType == StiMaximumType.Percent)
		{
			num6 = num5 + num7 * maximumValue / 100f;
		}
		object dataFromDataColumn3 = StiDataColumn.GetDataFromDataColumn(component.Report.Dictionary, Column);
		float num8 = 0f;
		try
		{
			num8 = (float)StiReport.ChangeType(dataFromDataColumn3, typeof(float));
		}
		catch
		{
		}
		if (num8 > num6)
		{
			num8 = num6;
		}
		if (num8 < num5)
		{
			num8 = num5;
		}
		if (num5 > 0f)
		{
			num8 -= num5;
			num6 -= num5;
			num5 = 0f;
		}
		if (num6 < 0f)
		{
			num8 -= num6;
			num5 -= num6;
			num6 = 0f;
		}
		return new StiDataBarIndicator
		{
			BrushType = BrushType,
			PositiveColor = PositiveColor,
			NegativeColor = NegativeColor,
			ShowBorder = ShowBorder,
			PositiveBorderColor = PositiveBorderColor,
			NegativeBorderColor = NegativeBorderColor,
			Direction = Direction,
			Minimum = num5,
			Maximum = num6,
			Value = num8
		};
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
		if (!(obj is StiDataBarCondition stiDataBarCondition))
		{
			return false;
		}
		if (Column.Equals(stiDataBarCondition.Column) && BrushType.Equals(stiDataBarCondition.BrushType) && PositiveColor.Equals((object?)stiDataBarCondition.PositiveColor) && NegativeColor.Equals((object?)stiDataBarCondition.NegativeColor) && ShowBorder.Equals(stiDataBarCondition.ShowBorder) && PositiveBorderColor.Equals((object?)stiDataBarCondition.PositiveBorderColor) && NegativeBorderColor.Equals((object?)stiDataBarCondition.NegativeBorderColor) && MinimumType.Equals(stiDataBarCondition.MinimumType) && MinimumValue.Equals(stiDataBarCondition.MinimumValue) && MaximumType.Equals(stiDataBarCondition.MaximumType))
		{
			return MaximumValue.Equals(stiDataBarCondition.MaximumValue);
		}
		return false;
	}

	public StiDataBarCondition()
	{
	}

	public StiDataBarCondition(string column, StiBrushType brushType, Color positiveColor, Color negativeColor, bool showBorder, Color positiveBorderColor, Color negativeBorderColor, StiDataBarDirection direction, StiMinimumType minimumType, float minimumValue, StiMaximumType maximumType, float maximumValue)
	{
		Column = column;
		BrushType = brushType;
		PositiveColor = positiveColor;
		NegativeColor = negativeColor;
		ShowBorder = showBorder;
		PositiveBorderColor = positiveBorderColor;
		NegativeBorderColor = negativeBorderColor;
		Direction = direction;
		MinimumType = minimumType;
		MinimumValue = minimumValue;
		MaximumType = maximumType;
		MaximumValue = maximumValue;
	}
}
