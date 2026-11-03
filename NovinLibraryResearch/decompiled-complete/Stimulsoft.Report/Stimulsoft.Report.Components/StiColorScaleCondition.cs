using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components;

[RefreshProperties(RefreshProperties.All)]
[TypeConverter(typeof(StiConditionConverter))]
public class StiColorScaleCondition : StiBaseCondition, IStiIndicatorCondition
{
	private float? minimum;

	private float? maximum;

	[StiSerializable]
	[DefaultValue(StiColorScaleType.Color2)]
	public StiColorScaleType ScaleType { get; set; }

	[StiSerializable]
	public Color MinimumColor { get; set; } = Color.Red;


	[StiSerializable]
	public Color MidColor { get; set; } = Color.Yellow;


	[StiSerializable]
	public Color MaximumColor { get; set; } = Color.Green;


	[StiSerializable]
	[DefaultValue(StiMinimumType.Auto)]
	public StiMinimumType MinimumType { get; set; }

	[StiSerializable]
	[DefaultValue(0f)]
	public float MinimumValue { get; set; }

	[StiSerializable]
	[DefaultValue(StiMidType.Auto)]
	public StiMidType MidType { get; set; }

	[StiSerializable]
	[DefaultValue(50f)]
	public float MidValue { get; set; } = 50f;


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
		JsonExtensions.AddPropertyEnum(val, "ScaleType", (Enum)ScaleType, (Enum)StiColorScaleType.Color2);
		JsonExtensions.AddPropertyColor(val, "MinimumColor", MinimumColor, Color.Red);
		JsonExtensions.AddPropertyColor(val, "MidColor", MidColor, Color.Yellow);
		JsonExtensions.AddPropertyColor(val, "MaximumColor", MaximumColor, Color.Green);
		JsonExtensions.AddPropertyEnum(val, "MinimumType", (Enum)MinimumType, (Enum)StiMinimumType.Auto);
		JsonExtensions.AddPropertyFloat(val, "MinimumValue", MinimumValue, 0f);
		JsonExtensions.AddPropertyEnum(val, "MidType", (Enum)MidType, (Enum)StiMidType.Auto);
		JsonExtensions.AddPropertyFloat(val, "MidValue", MidValue, 50f);
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
			case "ScaleType":
				ScaleType = StiJsonReportObjectHelper.DeserializeEnum<StiColorScaleType>(item);
				break;
			case "MinimumColor":
				MinimumColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "MidColor":
				MidColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "MaximumColor":
				MaximumColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "MinimumType":
				MinimumType = StiJsonReportObjectHelper.DeserializeEnum<StiMinimumType>(item);
				break;
			case "MinimumValue":
				MinimumValue = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "MidType":
				MidType = StiJsonReportObjectHelper.DeserializeEnum<StiMidType>(item);
				break;
			case "MidValue":
				MidValue = StiJsonReportObjectHelper.DeserializeFloat(item);
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
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Expected O, but got Unknown
		if (string.IsNullOrEmpty(Column))
		{
			component.Report.WriteToReportRenderingMessages("Column of Color Scale Condition of '" + component.Name + "' component is not specified!");
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
		float midValue = MidValue;
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
		float num8 = (num6 - num5) / 2f;
		if (MidType == StiMidType.Value)
		{
			num8 = MidValue;
		}
		else if (MidType == StiMidType.Percent)
		{
			num8 = num5 + num7 * midValue / 100f;
		}
		if (num8 < num5)
		{
			num8 = num5;
		}
		if (num8 > num6)
		{
			num8 = num6;
		}
		object dataFromDataColumn3 = StiDataColumn.GetDataFromDataColumn(component.Report.Dictionary, Column);
		float num9 = 0f;
		try
		{
			num9 = (float)StiReport.ChangeType(dataFromDataColumn3, typeof(float));
		}
		catch
		{
		}
		if (num9 > num6)
		{
			num9 = num6;
		}
		if (num9 < num5)
		{
			num9 = num5;
		}
		float num10 = 0f;
		Color color;
		Color color2;
		if (ScaleType == StiColorScaleType.Color2)
		{
			num10 = (num9 - num5) / (num6 - num5);
			color = MinimumColor;
			color2 = MaximumColor;
		}
		else if (num9 > num8)
		{
			num10 = (num9 - num8) / (num6 - num8);
			color = MidColor;
			color2 = MaximumColor;
		}
		else
		{
			num10 = num9 / (num8 - num5);
			color = MinimumColor;
			color2 = MidColor;
		}
		float val = (float)(color2.A - color.A) * num10 + (float)(int)color.A;
		float val2 = (float)(color2.R - color.R) * num10 + (float)(int)color.R;
		float val3 = (float)(color2.G - color.G) * num10 + (float)(int)color.G;
		float val4 = (float)(color2.B - color.B) * num10 + (float)(int)color.B;
		val = Math.Min(val, 255f);
		val2 = Math.Min(val2, 255f);
		val3 = Math.Min(val3, 255f);
		val4 = Math.Min(val4, 255f);
		component.Brush = (StiBrush)new StiSolidBrush(Color.FromArgb((byte)val, (byte)val2, (byte)val3, (byte)val4));
		return null;
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
		if (!(obj is StiColorScaleCondition stiColorScaleCondition))
		{
			return false;
		}
		if (Column.Equals(stiColorScaleCondition.Column) && ScaleType.Equals(stiColorScaleCondition.ScaleType) && MinimumColor.Equals((object?)stiColorScaleCondition.MinimumColor) && MidColor.Equals((object?)stiColorScaleCondition.MidColor) && MaximumColor.Equals((object?)stiColorScaleCondition.MaximumColor) && MinimumType.Equals(stiColorScaleCondition.MinimumType) && MinimumValue.Equals(stiColorScaleCondition.MinimumValue) && MidType.Equals(stiColorScaleCondition.MidType) && MidValue.Equals(stiColorScaleCondition.MidValue) && MaximumType.Equals(stiColorScaleCondition.MaximumType))
		{
			return MaximumValue.Equals(stiColorScaleCondition.MaximumValue);
		}
		return false;
	}

	public StiColorScaleCondition()
	{
	}

	public StiColorScaleCondition(string column, StiColorScaleType scaleType, Color minimumColor, Color midColor, Color maximumColor, StiMinimumType minimumType, float minimumValue, StiMidType midType, float midValue, StiMaximumType maximumType, float maximumValue)
	{
		Column = column;
		ScaleType = scaleType;
		MinimumColor = minimumColor;
		MidColor = midColor;
		MaximumColor = maximumColor;
		MinimumType = minimumType;
		MinimumValue = minimumValue;
		MidType = midType;
		MidValue = midValue;
		MaximumType = maximumType;
		MaximumValue = maximumValue;
	}
}
