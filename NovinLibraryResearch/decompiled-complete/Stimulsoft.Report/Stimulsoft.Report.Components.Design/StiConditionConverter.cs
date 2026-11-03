using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Components.Design;

public class StiConditionConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			InstanceDescriptor instanceDescriptor = ConvertMultiConditionToInstanceDescriptor(value);
			if (instanceDescriptor != null)
			{
				return instanceDescriptor;
			}
			instanceDescriptor = ConvertExpressionConditionToInstanceDescriptor(value);
			if (instanceDescriptor != null)
			{
				return instanceDescriptor;
			}
			instanceDescriptor = ConvertConditionToInstanceDescriptor(value);
			if (instanceDescriptor != null)
			{
				return instanceDescriptor;
			}
			instanceDescriptor = ConvertDataBarConditionToInstanceDescriptor(value);
			if (instanceDescriptor != null)
			{
				return instanceDescriptor;
			}
			instanceDescriptor = ConvertColorScaleConditionToInstanceDescriptor(value);
			if (instanceDescriptor != null)
			{
				return instanceDescriptor;
			}
			instanceDescriptor = ConvertIconSetConditionToInstanceDescriptor(value);
			if (instanceDescriptor != null)
			{
				return instanceDescriptor;
			}
		}
		else if (destinationType == typeof(string))
		{
			string text = ConvertMultiConditionToString(value);
			if (text != null)
			{
				return text;
			}
			text = ConvertExpressionConditionToString(value);
			if (text != null)
			{
				return text;
			}
			text = ConvertConditionToString(value);
			if (text != null)
			{
				return text;
			}
			text = ConvertDataBarConditionToString(value);
			if (text != null)
			{
				return text;
			}
			text = ConvertColorScaleToString(value);
			if (text != null)
			{
				return text;
			}
			text = ConvertIconSetConditionToString(value);
			if (text != null)
			{
				return text;
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	private string ConvertConditionToString(object value)
	{
		if (!(value is StiCondition stiCondition))
		{
			return null;
		}
		if (string.IsNullOrEmpty(stiCondition.Style) && stiCondition.BorderSides == StiConditionBorderSides.NotAssigned)
		{
			return string.Format("{0}{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", stiCondition.BreakIfTrue ? "BreakIfTrue," : string.Empty, XmlConvert.EncodeName(stiCondition.Column), stiCondition.Condition, XmlConvert.EncodeName(stiCondition.Value1), XmlConvert.EncodeName(stiCondition.Value2), stiCondition.DataType, StiReportObjectStringConverter.ConvertColorToString(stiCondition.TextColor), StiReportObjectStringConverter.ConvertColorToString(stiCondition.BackColor), XmlConvert.EncodeName(StiReportObjectStringConverter.ConvertFontToString(stiCondition.Font)), stiCondition.Enabled, stiCondition.CanAssignExpression, XmlConvert.EncodeName(stiCondition.AssignExpression));
		}
		if (stiCondition.Permissions == StiConditionPermissions.All)
		{
			return string.Format("{0}{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", stiCondition.BreakIfTrue ? "BreakIfTrue," : string.Empty, XmlConvert.EncodeName(stiCondition.Column), stiCondition.Condition, XmlConvert.EncodeName(stiCondition.Value1), XmlConvert.EncodeName(stiCondition.Value2), stiCondition.DataType, StiReportObjectStringConverter.ConvertColorToString(stiCondition.TextColor), StiReportObjectStringConverter.ConvertColorToString(stiCondition.BackColor), XmlConvert.EncodeName(StiReportObjectStringConverter.ConvertFontToString(stiCondition.Font)), stiCondition.Enabled, stiCondition.CanAssignExpression, XmlConvert.EncodeName(stiCondition.AssignExpression), XmlConvert.EncodeName(stiCondition.Style), XmlConvert.EncodeName(stiCondition.BorderSides.ToString()));
		}
		return string.Format("{0}{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", stiCondition.BreakIfTrue ? "BreakIfTrue," : string.Empty, XmlConvert.EncodeName(stiCondition.Column), stiCondition.Condition, XmlConvert.EncodeName(stiCondition.Value1), XmlConvert.EncodeName(stiCondition.Value2), stiCondition.DataType, StiReportObjectStringConverter.ConvertColorToString(stiCondition.TextColor), StiReportObjectStringConverter.ConvertColorToString(stiCondition.BackColor), XmlConvert.EncodeName(StiReportObjectStringConverter.ConvertFontToString(stiCondition.Font)), stiCondition.Enabled, stiCondition.CanAssignExpression, XmlConvert.EncodeName(stiCondition.AssignExpression), XmlConvert.EncodeName(stiCondition.Style), XmlConvert.EncodeName(stiCondition.BorderSides.ToString()), XmlConvert.EncodeName(stiCondition.Permissions.ToString()));
	}

	private string ConvertExpressionConditionToString(object value)
	{
		if (!(value is StiCondition { Item: StiFilterItem.Expression } stiCondition))
		{
			return null;
		}
		if (string.IsNullOrEmpty(stiCondition.Style) && stiCondition.BorderSides == StiConditionBorderSides.NotAssigned && stiCondition.Permissions == StiConditionPermissions.All)
		{
			return string.Format("{0}{1},{2},{3},{4},{5},{6}{7}", stiCondition.BreakIfTrue ? "BreakIfTrue," : string.Empty, XmlConvert.EncodeName(stiCondition.Expression), StiReportObjectStringConverter.ConvertColorToString(stiCondition.TextColor), StiReportObjectStringConverter.ConvertColorToString(stiCondition.BackColor), XmlConvert.EncodeName(StiReportObjectStringConverter.ConvertFontToString(stiCondition.Font)), stiCondition.Enabled, stiCondition.CanAssignExpression, XmlConvert.EncodeName(stiCondition.AssignExpression));
		}
		if (stiCondition.Permissions == StiConditionPermissions.All)
		{
			return string.Format("{0}{1},{2},{3},{4},{5},{6},{7},{8},{9}", stiCondition.BreakIfTrue ? "BreakIfTrue," : string.Empty, XmlConvert.EncodeName(stiCondition.Expression), StiReportObjectStringConverter.ConvertColorToString(stiCondition.TextColor), StiReportObjectStringConverter.ConvertColorToString(stiCondition.BackColor), XmlConvert.EncodeName(StiReportObjectStringConverter.ConvertFontToString(stiCondition.Font)), stiCondition.Enabled, stiCondition.CanAssignExpression, XmlConvert.EncodeName(stiCondition.AssignExpression), XmlConvert.EncodeName(stiCondition.Style), XmlConvert.EncodeName(stiCondition.BorderSides.ToString()));
		}
		return string.Format("{0}{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", stiCondition.BreakIfTrue ? "BreakIfTrue," : string.Empty, XmlConvert.EncodeName(stiCondition.Expression), StiReportObjectStringConverter.ConvertColorToString(stiCondition.TextColor), StiReportObjectStringConverter.ConvertColorToString(stiCondition.BackColor), XmlConvert.EncodeName(StiReportObjectStringConverter.ConvertFontToString(stiCondition.Font)), stiCondition.Enabled, stiCondition.CanAssignExpression, XmlConvert.EncodeName(stiCondition.AssignExpression), XmlConvert.EncodeName(stiCondition.Style), XmlConvert.EncodeName(stiCondition.BorderSides.ToString()), XmlConvert.EncodeName(stiCondition.Permissions.ToString()));
	}

	private string ConvertMultiConditionToString(object value)
	{
		if (!(value is StiMultiCondition stiMultiCondition))
		{
			return null;
		}
		return $"Multi{StiObjectStateSaver.WriteObjectStateToString((object)new StiMultiConditionContainer
		{
			BackColor = stiMultiCondition.BackColor,
			Enabled = stiMultiCondition.Enabled,
			FilterMode = stiMultiCondition.FilterMode,
			Filters = stiMultiCondition.Filters,
			Font = stiMultiCondition.Font,
			TextColor = stiMultiCondition.TextColor,
			CanAssignExpression = stiMultiCondition.CanAssignExpression,
			AssignExpression = stiMultiCondition.AssignExpression,
			Style = stiMultiCondition.Style,
			BorderSides = stiMultiCondition.BorderSides,
			Permissions = stiMultiCondition.Permissions,
			BreakIfTrue = stiMultiCondition.BreakIfTrue
		})}";
	}

	private string ConvertDataBarConditionToString(object value)
	{
		if (!(value is StiDataBarCondition stiDataBarCondition))
		{
			return null;
		}
		return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", "DataBar", XmlConvert.EncodeName(stiDataBarCondition.Column), XmlConvert.EncodeName(stiDataBarCondition.BrushType.ToString()), StiReportObjectStringConverter.ConvertColorToString(stiDataBarCondition.PositiveColor), StiReportObjectStringConverter.ConvertColorToString(stiDataBarCondition.NegativeColor), stiDataBarCondition.ShowBorder, StiReportObjectStringConverter.ConvertColorToString(stiDataBarCondition.PositiveBorderColor), StiReportObjectStringConverter.ConvertColorToString(stiDataBarCondition.NegativeBorderColor), XmlConvert.EncodeName(stiDataBarCondition.Direction.ToString()), XmlConvert.EncodeName(stiDataBarCondition.MinimumType.ToString()), stiDataBarCondition.MinimumValue, XmlConvert.EncodeName(stiDataBarCondition.MaximumType.ToString()), stiDataBarCondition.MaximumValue);
	}

	private string ConvertColorScaleToString(object value)
	{
		if (!(value is StiColorScaleCondition stiColorScaleCondition))
		{
			return null;
		}
		return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", "ColorScale", XmlConvert.EncodeName(stiColorScaleCondition.Column), XmlConvert.EncodeName(stiColorScaleCondition.ScaleType.ToString()), StiReportObjectStringConverter.ConvertColorToString(stiColorScaleCondition.MinimumColor), StiReportObjectStringConverter.ConvertColorToString(stiColorScaleCondition.MidColor), StiReportObjectStringConverter.ConvertColorToString(stiColorScaleCondition.MaximumColor), XmlConvert.EncodeName(stiColorScaleCondition.MinimumType.ToString()), stiColorScaleCondition.MinimumValue, XmlConvert.EncodeName(stiColorScaleCondition.MidType.ToString()), stiColorScaleCondition.MidValue, XmlConvert.EncodeName(stiColorScaleCondition.MaximumType.ToString()), stiColorScaleCondition.MaximumValue);
	}

	private string ConvertIconSetConditionToString(object value)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		if (!(value is StiIconSetCondition stiIconSetCondition))
		{
			return null;
		}
		object[] obj = new object[9]
		{
			"IconSet",
			XmlConvert.EncodeName(stiIconSetCondition.Column),
			XmlConvert.EncodeName(stiIconSetCondition.IconSet.ToString()),
			null,
			null,
			null,
			null,
			null,
			null
		};
		ContentAlignment contentAlignment = stiIconSetCondition.ContentAlignment;
		obj[3] = XmlConvert.EncodeName(((object)(ContentAlignment)(ref contentAlignment)).ToString());
		obj[4] = XmlConvert.EncodeName(ConvertIconSetItemToString(stiIconSetCondition.IconSetItem1));
		obj[5] = XmlConvert.EncodeName(ConvertIconSetItemToString(stiIconSetCondition.IconSetItem2));
		obj[6] = XmlConvert.EncodeName(ConvertIconSetItemToString(stiIconSetCondition.IconSetItem3));
		obj[7] = XmlConvert.EncodeName(ConvertIconSetItemToString(stiIconSetCondition.IconSetItem4));
		obj[8] = XmlConvert.EncodeName(ConvertIconSetItemToString(stiIconSetCondition.IconSetItem5));
		return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", obj);
	}

	private InstanceDescriptor ConvertMultiConditionToInstanceDescriptor(object value)
	{
		if (!(value is StiMultiCondition stiMultiCondition))
		{
			return null;
		}
		ConstructorInfo constructor = typeof(StiMultiCondition).GetConstructor(new Type[9]
		{
			typeof(Color),
			typeof(Color),
			typeof(Font),
			typeof(bool),
			typeof(StiFilterMode),
			typeof(StiFilter[]),
			typeof(string),
			typeof(StiConditionBorderSides),
			typeof(StiConditionPermissions)
		});
		if (constructor == null)
		{
			return null;
		}
		return CreateNewInstanceDescriptor(constructor, new object[9]
		{
			stiMultiCondition.TextColor,
			stiMultiCondition.BackColor,
			stiMultiCondition.Font,
			stiMultiCondition.Enabled,
			stiMultiCondition.FilterMode,
			stiMultiCondition.Filters.ToList().ToArray(),
			stiMultiCondition.Style,
			stiMultiCondition.BorderSides,
			stiMultiCondition.Permissions
		});
	}

	private InstanceDescriptor ConvertExpressionConditionToInstanceDescriptor(object value)
	{
		if (!(value is StiCondition { Item: StiFilterItem.Expression } stiCondition))
		{
			return null;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(string));
		list.Add(typeof(Color));
		list.Add(typeof(Color));
		list.Add(typeof(Font));
		list.Add(typeof(bool));
		list.Add(typeof(string));
		list.Add(typeof(StiConditionBorderSides));
		list.Add(typeof(StiConditionPermissions));
		List<Type> list2 = list;
		if (stiCondition.BreakIfTrue)
		{
			list2.Add(typeof(bool));
		}
		ConstructorInfo constructor = typeof(StiCondition).GetConstructor(list2.ToArray());
		if (constructor == null)
		{
			return null;
		}
		List<object> list3 = new List<object> { stiCondition.Expression, stiCondition.TextColor, stiCondition.BackColor, stiCondition.Font, stiCondition.Enabled, stiCondition.Style, stiCondition.BorderSides, stiCondition.Permissions };
		if (stiCondition.BreakIfTrue)
		{
			list3.Add(stiCondition.BreakIfTrue);
		}
		return CreateNewInstanceDescriptor(constructor, list3.ToArray());
	}

	private InstanceDescriptor ConvertConditionToInstanceDescriptor(object value)
	{
		if (!(value is StiCondition stiCondition))
		{
			return null;
		}
		List<Type> list = new List<Type>();
		list.Add(typeof(string));
		list.Add(typeof(StiFilterCondition));
		list.Add(typeof(string));
		list.Add(typeof(string));
		list.Add(typeof(StiFilterDataType));
		list.Add(typeof(Color));
		list.Add(typeof(Color));
		list.Add(typeof(Font));
		list.Add(typeof(bool));
		list.Add(typeof(bool));
		list.Add(typeof(string));
		list.Add(typeof(string));
		list.Add(typeof(StiConditionBorderSides));
		list.Add(typeof(StiConditionPermissions));
		List<Type> list2 = list;
		if (stiCondition.BreakIfTrue)
		{
			list2.Add(typeof(bool));
		}
		ConstructorInfo constructor = typeof(StiFilter).GetConstructor(list2.ToArray());
		if (constructor == null)
		{
			return null;
		}
		List<object> list3 = new List<object>
		{
			stiCondition.Column, stiCondition.Condition, stiCondition.Value1, stiCondition.Value2, stiCondition.DataType, stiCondition.TextColor, stiCondition.BackColor, stiCondition.Font, stiCondition.Enabled, stiCondition.CanAssignExpression,
			stiCondition.AssignExpression, stiCondition.Style, stiCondition.BorderSides, stiCondition.Permissions
		};
		if (stiCondition.BreakIfTrue)
		{
			list3.Add(stiCondition.BreakIfTrue);
		}
		return CreateNewInstanceDescriptor(constructor, list3.ToArray());
	}

	private InstanceDescriptor ConvertDataBarConditionToInstanceDescriptor(object value)
	{
		if (!(value is StiDataBarCondition stiDataBarCondition))
		{
			return null;
		}
		ConstructorInfo constructor = typeof(StiDataBarCondition).GetConstructor(new Type[12]
		{
			typeof(string),
			typeof(StiBrushType),
			typeof(Color),
			typeof(Color),
			typeof(bool),
			typeof(Color),
			typeof(Color),
			typeof(StiDataBarDirection),
			typeof(StiMinimumType),
			typeof(float),
			typeof(StiMaximumType),
			typeof(float)
		});
		if (constructor == null)
		{
			return null;
		}
		return CreateNewInstanceDescriptor(constructor, new object[12]
		{
			stiDataBarCondition.Column, stiDataBarCondition.BrushType, stiDataBarCondition.PositiveColor, stiDataBarCondition.NegativeColor, stiDataBarCondition.ShowBorder, stiDataBarCondition.PositiveBorderColor, stiDataBarCondition.NegativeBorderColor, stiDataBarCondition.Direction, stiDataBarCondition.MinimumType, stiDataBarCondition.MinimumValue,
			stiDataBarCondition.MaximumType, stiDataBarCondition.MaximumValue
		});
	}

	private InstanceDescriptor ConvertColorScaleConditionToInstanceDescriptor(object value)
	{
		if (!(value is StiColorScaleCondition stiColorScaleCondition))
		{
			return null;
		}
		ConstructorInfo constructor = typeof(StiColorScaleCondition).GetConstructor(new Type[11]
		{
			typeof(string),
			typeof(StiColorScaleType),
			typeof(Color),
			typeof(Color),
			typeof(Color),
			typeof(StiMinimumType),
			typeof(float),
			typeof(StiMidType),
			typeof(float),
			typeof(StiMaximumType),
			typeof(float)
		});
		if (constructor == null)
		{
			return null;
		}
		return CreateNewInstanceDescriptor(constructor, new object[11]
		{
			stiColorScaleCondition.Column, stiColorScaleCondition.ScaleType, stiColorScaleCondition.MinimumColor, stiColorScaleCondition.MidColor, stiColorScaleCondition.MaximumColor, stiColorScaleCondition.MinimumType, stiColorScaleCondition.MinimumValue, stiColorScaleCondition.MidType, stiColorScaleCondition.MidValue, stiColorScaleCondition.MaximumType,
			stiColorScaleCondition.MaximumValue
		});
	}

	private InstanceDescriptor ConvertIconSetConditionToInstanceDescriptor(object value)
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		if (!(value is StiIconSetCondition stiIconSetCondition))
		{
			return null;
		}
		ConstructorInfo constructor = typeof(StiIconSetCondition).GetConstructor(new Type[8]
		{
			typeof(string),
			typeof(StiIconSet),
			typeof(ContentAlignment),
			typeof(StiIconSetItem),
			typeof(StiIconSetItem),
			typeof(StiIconSetItem),
			typeof(StiIconSetItem),
			typeof(StiIconSetItem)
		});
		if (constructor == null)
		{
			return null;
		}
		return CreateNewInstanceDescriptor(constructor, new object[8] { stiIconSetCondition.Column, stiIconSetCondition.IconSet, stiIconSetCondition.ContentAlignment, stiIconSetCondition.IconSetItem1, stiIconSetCondition.IconSetItem2, stiIconSetCondition.IconSetItem3, stiIconSetCondition.IconSetItem4, stiIconSetCondition.IconSetItem5 });
	}

	private string ConvertIconSetItemToString(StiIconSetItem item)
	{
		if (item != null)
		{
			return new StiIconSetItemConverter().ConvertToInvariantString(item);
		}
		return "null";
	}

	private StiIconSetItem ConvertIconSetItemFromString(string str)
	{
		if (!(str == "null"))
		{
			return new StiIconSetItemConverter().ConvertFromInvariantString(str) as StiIconSetItem;
		}
		return null;
	}

	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, sourceType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	private bool IsBorderSides(string str)
	{
		str = XmlConvert.DecodeName(str);
		if (str == "False" || str == "True")
		{
			return false;
		}
		if (!str.Contains("Left") && !str.Contains("Top") && !str.Contains("Bottom") && !str.Contains("Right") && !str.Contains("All") && !str.Contains("None"))
		{
			return str.Contains("NotAssigned");
		}
		return true;
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		if (value is string)
		{
			string text = value as string;
			bool breakIfTrue = false;
			if (text.StartsWith("BreakIfTrue,"))
			{
				breakIfTrue = true;
				text = text.Replace("BreakIfTrue,", string.Empty);
			}
			char[] separator = new char[1] { ',' };
			string[] array = text.Split(separator);
			if (array[0] == "DataBar")
			{
				return new StiDataBarCondition(XmlConvert.DecodeName(array[1]), (StiBrushType)Enum.Parse(typeof(StiBrushType), XmlConvert.DecodeName(array[2])), StiReportObjectStringConverter.ConvertStringToColor(array[3]), StiReportObjectStringConverter.ConvertStringToColor(array[4]), array[5].ToLower(CultureInfo.InvariantCulture) == "true", StiReportObjectStringConverter.ConvertStringToColor(array[6]), StiReportObjectStringConverter.ConvertStringToColor(array[7]), (StiDataBarDirection)Enum.Parse(typeof(StiDataBarDirection), XmlConvert.DecodeName(array[8])), (StiMinimumType)Enum.Parse(typeof(StiMinimumType), XmlConvert.DecodeName(array[9])), float.Parse(array[10]), (StiMaximumType)Enum.Parse(typeof(StiMaximumType), XmlConvert.DecodeName(array[11])), float.Parse(array[12]));
			}
			if (array[0] == "ColorScale")
			{
				return new StiColorScaleCondition(XmlConvert.DecodeName(array[1]), (StiColorScaleType)Enum.Parse(typeof(StiColorScaleType), XmlConvert.DecodeName(array[2])), StiReportObjectStringConverter.ConvertStringToColor(array[3]), StiReportObjectStringConverter.ConvertStringToColor(array[4]), StiReportObjectStringConverter.ConvertStringToColor(array[5]), (StiMinimumType)Enum.Parse(typeof(StiMinimumType), XmlConvert.DecodeName(array[6])), float.Parse(array[7]), (StiMidType)Enum.Parse(typeof(StiMidType), XmlConvert.DecodeName(array[8])), float.Parse(array[9]), (StiMaximumType)Enum.Parse(typeof(StiMaximumType), XmlConvert.DecodeName(array[10])), float.Parse(array[11]));
			}
			if (array[0] == "IconSet")
			{
				return new StiIconSetCondition(XmlConvert.DecodeName(array[1]), (StiIconSet)Enum.Parse(typeof(StiIconSet), XmlConvert.DecodeName(array[2])), (ContentAlignment)Enum.Parse(typeof(ContentAlignment), XmlConvert.DecodeName(array[3])), ConvertIconSetItemFromString(XmlConvert.DecodeName(array[4])), ConvertIconSetItemFromString(XmlConvert.DecodeName(array[5])), ConvertIconSetItemFromString(XmlConvert.DecodeName(array[6])), ConvertIconSetItemFromString(XmlConvert.DecodeName(array[7])), ConvertIconSetItemFromString(XmlConvert.DecodeName(array[8])));
			}
			if (text.StartsWith("Multi<", StringComparison.InvariantCulture))
			{
				StiMultiConditionContainer stiMultiConditionContainer = new StiMultiConditionContainer();
				StiMultiCondition stiMultiCondition = new StiMultiCondition();
				StiObjectStateSaver.ReadObjectStateFromString((object)stiMultiConditionContainer, text.Substring(5));
				stiMultiCondition.BackColor = stiMultiConditionContainer.BackColor;
				stiMultiCondition.Enabled = stiMultiConditionContainer.Enabled;
				stiMultiCondition.FilterMode = stiMultiConditionContainer.FilterMode;
				stiMultiCondition.Filters = stiMultiConditionContainer.Filters;
				stiMultiCondition.Font = stiMultiConditionContainer.Font;
				stiMultiCondition.TextColor = stiMultiConditionContainer.TextColor;
				stiMultiCondition.CanAssignExpression = stiMultiConditionContainer.CanAssignExpression;
				stiMultiCondition.AssignExpression = stiMultiConditionContainer.AssignExpression;
				stiMultiCondition.Style = stiMultiConditionContainer.Style;
				stiMultiCondition.Permissions = stiMultiConditionContainer.Permissions;
				stiMultiCondition.BorderSides = stiMultiConditionContainer.BorderSides;
				stiMultiCondition.BreakIfTrue = stiMultiConditionContainer.BreakIfTrue;
				return stiMultiCondition;
			}
			if (array.Length == 5)
			{
				return new StiCondition(XmlConvert.DecodeName(array[0]), StiReportObjectStringConverter.ConvertStringToColor(array[1]), StiReportObjectStringConverter.ConvertStringToColor(array[2]), StiReportObjectStringConverter.ConvertStringToFont(XmlConvert.DecodeName(array[3])), array[4].ToLower(CultureInfo.InvariantCulture) == "true");
			}
			if (array.Length == 6)
			{
				return new StiCondition(XmlConvert.DecodeName(array[0]), StiReportObjectStringConverter.ConvertStringToColor(array[1]), StiReportObjectStringConverter.ConvertStringToColor(array[2]), StiReportObjectStringConverter.ConvertStringToFont(XmlConvert.DecodeName(array[3])), array[4].ToLower(CultureInfo.InvariantCulture) == "true", array[5].ToLower(CultureInfo.InvariantCulture) == "true", string.Empty);
			}
			if (array.Length == 7)
			{
				return new StiCondition(XmlConvert.DecodeName(array[0]), StiReportObjectStringConverter.ConvertStringToColor(array[1]), StiReportObjectStringConverter.ConvertStringToColor(array[2]), StiReportObjectStringConverter.ConvertStringToFont(XmlConvert.DecodeName(array[3])), array[4].ToLower(CultureInfo.InvariantCulture) == "true", array[5].ToLower(CultureInfo.InvariantCulture) == "true", XmlConvert.DecodeName(array[6]));
			}
			if (array.Length == 9 && IsBorderSides(array[8]))
			{
				return new StiCondition(XmlConvert.DecodeName(array[0]), StiReportObjectStringConverter.ConvertStringToColor(array[1]), StiReportObjectStringConverter.ConvertStringToColor(array[2]), StiReportObjectStringConverter.ConvertStringToFont(XmlConvert.DecodeName(array[3])), array[4].ToLower(CultureInfo.InvariantCulture) == "true", array[5].ToLower(CultureInfo.InvariantCulture) == "true", XmlConvert.DecodeName(array[6]), XmlConvert.DecodeName(array[7]), (StiConditionBorderSides)Enum.Parse(typeof(StiConditionBorderSides), XmlConvert.DecodeName(array[8])))
				{
					BreakIfTrue = breakIfTrue
				};
			}
			if (array.Length == 10 && IsBorderSides(array[8]))
			{
				return new StiCondition(XmlConvert.DecodeName(array[0]), StiReportObjectStringConverter.ConvertStringToColor(array[1]), StiReportObjectStringConverter.ConvertStringToColor(array[2]), StiReportObjectStringConverter.ConvertStringToFont(XmlConvert.DecodeName(array[3])), array[4].ToLower(CultureInfo.InvariantCulture) == "true", array[5].ToLower(CultureInfo.InvariantCulture) == "true", XmlConvert.DecodeName(array[6]), XmlConvert.DecodeName(array[7]), (StiConditionBorderSides)Enum.Parse(typeof(StiConditionBorderSides), XmlConvert.DecodeName(array[8])), (StiConditionPermissions)Enum.Parse(typeof(StiConditionPermissions), XmlConvert.DecodeName(array[9])))
				{
					BreakIfTrue = breakIfTrue
				};
			}
			string text2 = array[1];
			if (text2 == "GreaterThen")
			{
				text2 = "GreaterThan";
			}
			if (text2 == "GreaterThenOrEqualTo")
			{
				text2 = "GreaterThanOrEqualTo";
			}
			if (text2 == "LessThen")
			{
				text2 = "LessThan";
			}
			if (text2 == "LessThenOrEqualTo")
			{
				text2 = "LessThanOrEqualTo";
			}
			if (array.Length == 9)
			{
				return new StiCondition(XmlConvert.DecodeName(array[0]), (StiFilterCondition)Enum.Parse(typeof(StiFilterCondition), text2), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), (StiFilterDataType)Enum.Parse(typeof(StiFilterDataType), array[4]), StiReportObjectStringConverter.ConvertStringToColor(array[5]), StiReportObjectStringConverter.ConvertStringToColor(array[6]), StiReportObjectStringConverter.ConvertStringToFont(XmlConvert.DecodeName(array[7])), array[8].ToLower(CultureInfo.InvariantCulture) == "true")
				{
					BreakIfTrue = breakIfTrue
				};
			}
			if (array.Length == 11)
			{
				return new StiCondition(XmlConvert.DecodeName(array[0]), (StiFilterCondition)Enum.Parse(typeof(StiFilterCondition), text2), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), (StiFilterDataType)Enum.Parse(typeof(StiFilterDataType), array[4]), StiReportObjectStringConverter.ConvertStringToColor(array[5]), StiReportObjectStringConverter.ConvertStringToColor(array[6]), StiReportObjectStringConverter.ConvertStringToFont(XmlConvert.DecodeName(array[7])), array[8].ToLower(CultureInfo.InvariantCulture) == "true", array[9].ToLower(CultureInfo.InvariantCulture) == "true", XmlConvert.DecodeName(array[10]))
				{
					BreakIfTrue = breakIfTrue
				};
			}
			if (array.Length == 13)
			{
				return new StiCondition(XmlConvert.DecodeName(array[0]), (StiFilterCondition)Enum.Parse(typeof(StiFilterCondition), text2), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), (StiFilterDataType)Enum.Parse(typeof(StiFilterDataType), array[4]), StiReportObjectStringConverter.ConvertStringToColor(array[5]), StiReportObjectStringConverter.ConvertStringToColor(array[6]), StiReportObjectStringConverter.ConvertStringToFont(XmlConvert.DecodeName(array[7])), array[8].ToLower(CultureInfo.InvariantCulture) == "true", array[9].ToLower(CultureInfo.InvariantCulture) == "true", XmlConvert.DecodeName(array[10]), XmlConvert.DecodeName(array[11]), (StiConditionBorderSides)Enum.Parse(typeof(StiConditionBorderSides), XmlConvert.DecodeName(array[12])))
				{
					BreakIfTrue = breakIfTrue
				};
			}
			if (array.Length == 14)
			{
				return new StiCondition(XmlConvert.DecodeName(array[0]), (StiFilterCondition)Enum.Parse(typeof(StiFilterCondition), text2), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), (StiFilterDataType)Enum.Parse(typeof(StiFilterDataType), array[4]), StiReportObjectStringConverter.ConvertStringToColor(array[5]), StiReportObjectStringConverter.ConvertStringToColor(array[6]), StiReportObjectStringConverter.ConvertStringToFont(XmlConvert.DecodeName(array[7])), array[8].ToLower(CultureInfo.InvariantCulture) == "true", array[9].ToLower(CultureInfo.InvariantCulture) == "true", XmlConvert.DecodeName(array[10]), XmlConvert.DecodeName(array[11]), (StiConditionBorderSides)Enum.Parse(typeof(StiConditionBorderSides), XmlConvert.DecodeName(array[12])), (StiConditionPermissions)Enum.Parse(typeof(StiConditionPermissions), XmlConvert.DecodeName(array[13])))
				{
					BreakIfTrue = breakIfTrue
				};
			}
		}
		return base.ConvertFrom(context, culture, value);
	}

	private InstanceDescriptor CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
