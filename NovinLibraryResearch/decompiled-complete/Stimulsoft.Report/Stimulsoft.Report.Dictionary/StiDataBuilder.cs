using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;

namespace Stimulsoft.Report.Dictionary;

public class StiDataBuilder
{
	private static string stepScaleName = StiScale.StepName;

	private Hashtable usedRelations;

	private Hashtable usedDataSources;

	private Hashtable usedColumns;

	private int level;

	private StiTreeView treeView;

	private static ImageList images;

	private static Image imageLocked;

	private static Image imageCondition;

	private static Image imageInteractions;

	private static Image imageFilter;

	private static Image imageSortAsc;

	private static Image imageSortDesc;

	private static Image imageCollapsed;

	private static Image imageExpanded;

	public bool ShowParameters { get; set; }

	public static ImageList Images
	{
		get
		{
			if (images == null)
			{
				FillImages();
			}
			return images;
		}
	}

	public static Image ImageLocked => imageLocked ?? (imageLocked = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Locked" + stepScaleName + ".gif", false));

	public static Image ImageCondition => imageCondition ?? (imageCondition = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.ConditionIcon" + stepScaleName + ".png", false));

	public static Image ImageInteractions => imageInteractions ?? (imageInteractions = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Interactions" + stepScaleName + ".png"));

	public static Image ImageFilter => imageFilter ?? (imageFilter = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.FilterIcon" + stepScaleName + ".png"));

	public static Image ImageSortAsc => imageSortAsc ?? (imageSortAsc = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Images.SortAsc" + stepScaleName + ".png"));

	public static Image ImageSortDesc => imageSortDesc ?? (imageSortDesc = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Images.SortDesc" + stepScaleName + ".png"));

	public static Image ImageCollapsed => imageCollapsed ?? (imageCollapsed = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Images.Collapsed" + stepScaleName + ".png"));

	public static Image ImageExpanded => imageExpanded ?? (imageExpanded = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Images.Expanded" + stepScaleName + ".png"));

	private static StiReport GetReport(object nodeTag)
	{
		if (nodeTag is StiDataColumn)
		{
			return ((StiDataColumn)nodeTag).DataSource?.Dictionary?.Report;
		}
		if (nodeTag is StiDataSource)
		{
			return ((StiDataSource)nodeTag).Dictionary?.Report;
		}
		if (nodeTag is StiBusinessObject)
		{
			return ((StiBusinessObject)nodeTag).Dictionary?.Report;
		}
		if (nodeTag is StiDataRelation)
		{
			return ((StiDataRelation)nodeTag).Dictionary?.Report;
		}
		return null;
	}

	public static string GetParameterPathFromNode(TreeNode node)
	{
		StiDataParameter stiDataParameter = node.Tag as StiDataParameter;
		StiDataSource dataSource = stiDataParameter.DataSource;
		StiReport report = dataSource.Dictionary.Report;
		string text = StiNameValidator.CorrectName(dataSource.Name, report);
		return text + ".Parameters[\"" + stiDataParameter.Name + "\"].ParameterValue";
	}

	public static string GetColumnPathFromNode(TreeNode node)
	{
		return GetColumnPathFromNode(node, useRelationName: false);
	}

	public static string GetColumnPathFromNode(TreeNode node, bool useRelationName, bool isDataExpression = false)
	{
		string text = string.Empty;
		if (node.Tag is StiDataColumn)
		{
			text = ((StiDataColumn)node.Tag).Name;
		}
		if (node.Tag is StiDataSource)
		{
			text = ((StiDataSource)node.Tag).Name;
		}
		if (node.Tag is StiBusinessObject)
		{
			text = ((StiBusinessObject)node.Tag).Name;
		}
		if (!isDataExpression)
		{
			text = StiNameValidator.CorrectName(text, GetReport(node.Tag));
		}
		while (true)
		{
			node = node.Parent;
			if (node == null)
			{
				break;
			}
			if (node.Tag is StiDataRelation stiDataRelation)
			{
				string text2 = (useRelationName ? stiDataRelation.NameInSource : stiDataRelation.Name);
				if (!isDataExpression)
				{
					text2 = StiNameValidator.CorrectName(text2, GetReport(node.Tag));
				}
				text = text2 + "." + text;
			}
			if (node.Tag is StiDataSource stiDataSource)
			{
				string text3 = ((!isDataExpression) ? StiNameValidator.CorrectName(stiDataSource.Name, GetReport(stiDataSource)) : stiDataSource.Name);
				text = text3 + "." + text;
				break;
			}
			if (node.Tag is StiBusinessObject stiBusinessObject)
			{
				string text4 = ((!isDataExpression) ? StiNameValidator.CorrectBusinessObjectName(stiBusinessObject.GetCorrectFullName(), GetReport(stiBusinessObject)) : stiBusinessObject.GetCorrectFullName());
				text = text4 + "." + text;
				break;
			}
		}
		if (!isDataExpression)
		{
			return text;
		}
		return Funcs.ToExpression(text);
	}

	public static string GetDataBandColumnPathFromNode(TreeNode node, bool useRelationName)
	{
		string str = string.Empty;
		if (node.Tag is StiDataColumn)
		{
			str = ((StiDataColumn)node.Tag).Name;
		}
		if (node.Tag is StiDataBand)
		{
			str = ((StiDataBand)node.Tag).Name;
		}
		str = StiNameValidator.CorrectName(str, GetReport(node.Tag));
		StiDataBand stiDataBand;
		do
		{
			node = node.Parent;
			if (node.Tag is StiDataRelation stiDataRelation)
			{
				string str2 = (useRelationName ? stiDataRelation.NameInSource : stiDataRelation.Name);
				str2 = StiNameValidator.CorrectName(str2, GetReport(stiDataRelation));
				str = str2 + "." + str;
			}
			stiDataBand = node.Tag as StiDataBand;
		}
		while (stiDataBand == null);
		string text = StiNameValidator.CorrectName(stiDataBand.Name, GetReport(stiDataBand));
		return text + "." + str;
	}

	public static string GetPropertyInfoPathFromNode(TreeNode node)
	{
		string text = StiNameValidator.CorrectName(((PropertyInfo)node.Tag).Name, GetReport(node.Tag));
		StiDataSource stiDataSource;
		while (true)
		{
			node = node.Parent;
			if (node.Tag is StiDataColumn stiDataColumn)
			{
				string text2 = StiNameValidator.CorrectName(stiDataColumn.Name, GetReport(stiDataColumn));
				text = text2 + "." + text;
			}
			PropertyInfo propertyInfo = node.Tag as PropertyInfo;
			if (propertyInfo != null)
			{
				string text3 = StiNameValidator.CorrectName(propertyInfo.Name, GetReport(propertyInfo));
				text = text3 + "." + text;
			}
			stiDataSource = node.Tag as StiDataSource;
			if (stiDataSource != null)
			{
				break;
			}
			if (node.Tag is StiDataRelation stiDataRelation)
			{
				string text4 = StiNameValidator.CorrectName(stiDataRelation.Name, GetReport(stiDataRelation));
				text = text4 + "." + text;
			}
		}
		string text5 = StiNameValidator.CorrectName(stiDataSource.Name, GetReport(stiDataSource));
		return text5 + "." + text;
	}

	public static string GetColumnPathFromNodeWithAlias(TreeNode node)
	{
		string text = StiNameValidator.CorrectName(((StiDataColumn)node.Tag).Name, GetReport(node.Tag));
		while (true)
		{
			node = node.Parent;
			if (node == null)
			{
				break;
			}
			if (node.Tag is StiDataRelation stiDataRelation)
			{
				string text2 = StiNameValidator.CorrectName(stiDataRelation.Name, GetReport(stiDataRelation));
				text = text2 + "." + text;
			}
			if (node.Tag is StiDataSource stiDataSource)
			{
				string text3 = StiNameValidator.CorrectName(stiDataSource.Name, GetReport(stiDataSource));
				text = text3 + "." + text;
				break;
			}
			if (node.Tag is StiBusinessObject stiBusinessObject)
			{
				string text4 = StiNameValidator.CorrectBusinessObjectName(stiBusinessObject.GetCorrectFullName(), GetReport(stiBusinessObject));
				text = text4 + "." + text;
				break;
			}
		}
		return text;
	}

	public static string GetColumnPathFromNodeWithAliasWithoutReplace(TreeNode node)
	{
		string text = ((StiDataColumn)node.Tag).Name;
		while (true)
		{
			node = node.Parent;
			if (node == null)
			{
				break;
			}
			if (node.Tag is StiDataRelation stiDataRelation)
			{
				text = stiDataRelation.Name + "." + text;
			}
			if (node.Tag is StiDataSource stiDataSource)
			{
				text = stiDataSource.Name + "." + text;
				break;
			}
		}
		return text;
	}

	public static StiDataColumn GetColumnFromPath(string path, StiDictionary dictionary)
	{
		string[] array = path.Split(new char[1] { '.' });
		string text = array[0];
		foreach (StiDataSource dataSource in dictionary.DataSources)
		{
			if (StiNameValidator.CorrectName(dataSource.Name, dictionary.Report) == text && path.Length > dataSource.Name.Length + 1)
			{
				string path2 = path.Substring(dataSource.Name.Length + 1);
				return GetColumnFromPath(path2, dataSource);
			}
		}
		return null;
	}

	public static StiDataColumn GetColumnFromPath(string path, StiDataSource dataSource)
	{
		string[] array = path.Split(new char[1] { '.' });
		int num = 0;
		string text = array[num];
		while (true)
		{
			foreach (StiDataColumn column in dataSource.Columns)
			{
				if (StiNameValidator.CorrectName(column.Name, dataSource.Dictionary?.Report) == text && num == array.Length - 1)
				{
					return column;
				}
			}
			StiDataRelationsCollection parentRelations = dataSource.GetParentRelations();
			foreach (StiDataRelation item in parentRelations)
			{
				if (StiNameValidator.CorrectName(item.Name, dataSource.Dictionary?.Report) == text)
				{
					string path2 = path.Substring(item.Name.Length + 1);
					return GetColumnFromPath(path2, item.ParentSource);
				}
			}
			if (num == array.Length - 1)
			{
				break;
			}
			num++;
			text = text + "." + array[num];
		}
		return null;
	}

	internal static string GetImageKeyFromType(Type type)
	{
		if (type == typeof(bool) || type == typeof(bool?))
		{
			return "Bool";
		}
		if (type == typeof(char) || type == typeof(char?))
		{
			return "Char";
		}
		if (type == typeof(DateTime) || type == typeof(TimeSpan) || type == typeof(DateTime?) || type == typeof(TimeSpan?))
		{
			return "DateTime";
		}
		if (type == typeof(decimal) || type == typeof(decimal?))
		{
			return "Decimal";
		}
		if (type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int?) || type == typeof(uint?) || type == typeof(long?) || type == typeof(ulong?) || type == typeof(byte?) || type == typeof(sbyte?) || type == typeof(short?) || type == typeof(ushort?))
		{
			return "Int";
		}
		if (type == typeof(float) || type == typeof(double) || type == typeof(float?) || type == typeof(double?))
		{
			return "Float";
		}
		if (type == typeof(Image) || type == typeof(Bitmap))
		{
			return "Image";
		}
		if (type == typeof(CharRange))
		{
			return "RangeChar";
		}
		if (type == typeof(DateTimeRange) || type == typeof(TimeSpanRange))
		{
			return "RangeDateTime";
		}
		if (type == typeof(DecimalRange))
		{
			return "RangeDecimal";
		}
		if (type == typeof(IntRange) || type == typeof(LongRange) || type == typeof(ByteRange) || type == typeof(ShortRange))
		{
			return "RangeInt";
		}
		if (type == typeof(FloatRange) || type == typeof(DoubleRange))
		{
			return "RangeFloat";
		}
		if (type == typeof(StringRange) || type == typeof(GuidRange))
		{
			return "RangeString";
		}
		if (type == typeof(BoolList))
		{
			return "ListBool";
		}
		if (type == typeof(StringList) || type == typeof(GuidList))
		{
			return "ListString";
		}
		if (type == typeof(CharList))
		{
			return "ListChar";
		}
		if (type == typeof(DateTimeList) || type == typeof(TimeSpanList))
		{
			return "ListDateTime";
		}
		if (type == typeof(DecimalList))
		{
			return "ListDecimal";
		}
		if (type == typeof(IntList) || type == typeof(LongList) || type == typeof(ByteList) || type == typeof(ShortList))
		{
			return "ListInt";
		}
		if (type == typeof(FloatList) || type == typeof(DoubleList))
		{
			return "ListFloat";
		}
		if (type != null && type.IsArray)
		{
			return "Binary";
		}
		return "String";
	}

	private static string GetImageKeyFromType(StiResourceType type)
	{
		return type.ToString();
	}

	public static string GetImageKeyFromColumn(StiDataColumn column)
	{
		string imageKeyFromType = GetImageKeyFromType(column?.Type);
		if (column is StiCalcDataColumn)
		{
			if (column.DataSource != null)
			{
				if (!column.DataSource.Inherited)
				{
					return "CalcColumn" + imageKeyFromType;
				}
				return "LockedCalcColumn" + imageKeyFromType;
			}
			if (!column.BusinessObject.Inherited)
			{
				return "CalcColumn" + imageKeyFromType;
			}
			return "LockedCalcColumn" + imageKeyFromType;
		}
		if (column.DataSource != null)
		{
			if (!column.DataSource.Inherited)
			{
				return "DataColumn" + imageKeyFromType;
			}
			return "LockedDataColumn" + imageKeyFromType;
		}
		if (column.BusinessObject != null)
		{
			if (!column.BusinessObject.Inherited)
			{
				return "DataColumn" + imageKeyFromType;
			}
			return "LockedDataColumn" + imageKeyFromType;
		}
		return string.Empty;
	}

	public static string GetImageKeyFromVariable(StiVariable variable)
	{
		string imageKeyFromType = GetImageKeyFromType(variable?.Type);
		if (!variable.Inherited)
		{
			return "Variable" + imageKeyFromType;
		}
		return "LockedVariable" + imageKeyFromType;
	}

	public static string GetImageKeyFromResource(StiResource resource)
	{
		return "Resource" + GetImageKeyFromType(resource.Type);
	}

	private bool IsSkipColumn(StiDataColumn column)
	{
		if (column.DataSource is StiBusinessObjectSource && (column.Name.StartsWith("_ID", StringComparison.InvariantCulture) || column.Name.StartsWith("_parentID", StringComparison.InvariantCulture) || column.Name.StartsWith("_Current", StringComparison.InvariantCulture)))
		{
			return true;
		}
		return false;
	}

	private static void FillImages()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		images = new ImageList
		{
			ColorDepth = (ColorDepth)24,
			ImageSize = new Size(StiScale.I16, StiScale.I16)
		};
		string text = "Office2013.Close" + stepScaleName + ".png";
		images.Images.Add("Close.bmp", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.DataSource" + stepScaleName + ".png";
		images.Images.Add("DataSource", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		images.Images.Add("DataTransformation", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.DataTransformation" + stepScaleName + ".png"));
		string text2 = "Office2013.";
		images.Images.Add("DataColumn", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "DataColumn" + stepScaleName + ".png"));
		images.Images.Add("DataColumnBinary", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "DataColumnBinary" + stepScaleName + ".png"));
		images.Images.Add("DataColumnBool", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "DataColumnBool" + stepScaleName + ".png"));
		images.Images.Add("DataColumnChar", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "DataColumnChar" + stepScaleName + ".png"));
		images.Images.Add("DataColumnDateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "DataColumnDateTime" + stepScaleName + ".png"));
		images.Images.Add("DataColumnDecimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "DataColumnDecimal" + stepScaleName + ".png"));
		images.Images.Add("DataColumnFloat", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "DataColumnFloat" + stepScaleName + ".png"));
		images.Images.Add("DataColumnImage", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "DataColumnImage" + stepScaleName + ".png"));
		images.Images.Add("DataColumnInt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "DataColumnInt" + stepScaleName + ".png"));
		images.Images.Add("DataColumnString", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "DataColumnString" + stepScaleName + ".png"));
		images.Images.Add("CalcColumn", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "CalcColumn" + stepScaleName + ".png"));
		images.Images.Add("CalcColumnBinary", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "CalcColumnBinary" + stepScaleName + ".png"));
		images.Images.Add("CalcColumnBool", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "CalcColumnBool" + stepScaleName + ".png"));
		images.Images.Add("CalcColumnChar", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "CalcColumnChar" + stepScaleName + ".png"));
		images.Images.Add("CalcColumnDateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "CalcColumnDateTime" + stepScaleName + ".png"));
		images.Images.Add("CalcColumnDecimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "CalcColumnDecimal" + stepScaleName + ".png"));
		images.Images.Add("CalcColumnFloat", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "CalcColumnFloat" + stepScaleName + ".png"));
		images.Images.Add("CalcColumnImage", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "CalcColumnImage" + stepScaleName + ".png"));
		images.Images.Add("CalcColumnInt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "CalcColumnInt" + stepScaleName + ".png"));
		images.Images.Add("CalcColumnString", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "CalcColumnString" + stepScaleName + ".png"));
		images.Images.Add("LockedDataColumn", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedDataColumn" + stepScaleName + ".png"));
		images.Images.Add("LockedDataColumnBinary", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedDataColumnBinary" + stepScaleName + ".png"));
		images.Images.Add("LockedDataColumnBool", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedDataColumnBool" + stepScaleName + ".png"));
		images.Images.Add("LockedDataColumnChar", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedDataColumnChar" + stepScaleName + ".png"));
		images.Images.Add("LockedDataColumnDateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedDataColumnDateTime" + stepScaleName + ".png"));
		images.Images.Add("LockedDataColumnDecimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedDataColumnDecimal" + stepScaleName + ".png"));
		images.Images.Add("LockedDataColumnFloat", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedDataColumnFloat" + stepScaleName + ".png"));
		images.Images.Add("LockedDataColumnImage", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedDataColumnImage" + stepScaleName + ".png"));
		images.Images.Add("LockedDataColumnInt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedDataColumnInt" + stepScaleName + ".png"));
		images.Images.Add("LockedDataColumnString", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedDataColumnString" + stepScaleName + ".png"));
		images.Images.Add("LockedCalcColumn", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedCalcColumn" + stepScaleName + ".png"));
		images.Images.Add("LockedCalcColumnBinary", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedCalcColumnBinary" + stepScaleName + ".png"));
		images.Images.Add("LockedCalcColumnBool", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedCalcColumnBool" + stepScaleName + ".png"));
		images.Images.Add("LockedCalcColumnChar", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedCalcColumnChar" + stepScaleName + ".png"));
		images.Images.Add("LockedCalcColumnDateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedCalcColumnDateTime" + stepScaleName + ".png"));
		images.Images.Add("LockedCalcColumnDecimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedCalcColumnDecimal" + stepScaleName + ".png"));
		images.Images.Add("LockedCalcColumnFloat", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedCalcColumnFloat" + stepScaleName + ".png"));
		images.Images.Add("LockedCalcColumnImage", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedCalcColumnImage" + stepScaleName + ".png"));
		images.Images.Add("LockedCalcColumnInt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedCalcColumnInt" + stepScaleName + ".png"));
		images.Images.Add("LockedCalcColumnString", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedCalcColumnString" + stepScaleName + ".png"));
		images.Images.Add("Total", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Total" + stepScaleName + ".png"));
		images.Images.Add("Totals", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Totals" + stepScaleName + ".png"));
		images.Images.Add("LockedTotal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.LockedTotal" + stepScaleName + ".png"));
		images.Images.Add("Variable", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "Variable" + stepScaleName + ".png"));
		images.Images.Add("Variables", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "Variables" + stepScaleName + ".png"));
		images.Images.Add("VariableBinary", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableBinary" + stepScaleName + ".png"));
		images.Images.Add("VariableBool", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableBool" + stepScaleName + ".png"));
		images.Images.Add("VariableChar", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableChar" + stepScaleName + ".png"));
		images.Images.Add("VariableDateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableDateTime" + stepScaleName + ".png"));
		images.Images.Add("VariableDecimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableDecimal" + stepScaleName + ".png"));
		images.Images.Add("VariableFloat", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableFloat" + stepScaleName + ".png"));
		images.Images.Add("VariableImage", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableImage" + stepScaleName + ".png"));
		images.Images.Add("VariableInt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableInt" + stepScaleName + ".png"));
		images.Images.Add("VariableString", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableString" + stepScaleName + ".png"));
		images.Images.Add("Resource", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "Resource" + stepScaleName + ".png"));
		images.Images.Add("Resources", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "Resources" + stepScaleName + ".png"));
		images.Images.Add("ResourceCsv", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceCsv" + stepScaleName + ".png"));
		images.Images.Add("ResourceDbf", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceDbf" + stepScaleName + ".png"));
		images.Images.Add("ResourceExcel", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceExcel" + stepScaleName + ".png"));
		images.Images.Add("ResourceImage", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceImage" + stepScaleName + ".png"));
		images.Images.Add("ResourceJson", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceJson" + stepScaleName + ".png"));
		images.Images.Add("ResourceReport", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceReport" + stepScaleName + ".png"));
		images.Images.Add("ResourceReportSnapshot", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceReportSnapshot" + stepScaleName + ".png"));
		images.Images.Add("ResourceRtf", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceRtf" + stepScaleName + ".png"));
		images.Images.Add("ResourceTxt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceTxt" + stepScaleName + ".png"));
		images.Images.Add("ResourceXml", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceXml" + stepScaleName + ".png"));
		images.Images.Add("ResourceXsd", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceXsd" + stepScaleName + ".png"));
		images.Images.Add("ResourcePdf", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourcePdf" + stepScaleName + ".png"));
		images.Images.Add("ResourceWord", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceWord" + stepScaleName + ".png"));
		images.Images.Add("ResourceMap", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceMap" + stepScaleName + ".png"));
		images.Images.Add("ResourceGis", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceGis" + stepScaleName + ".png"));
		images.Images.Add("ResourceFontEot", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceFontEot" + stepScaleName + ".png"));
		images.Images.Add("ResourceFontOtf", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceFontOtf" + stepScaleName + ".png"));
		images.Images.Add("ResourceFontTtc", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceFontTtc" + stepScaleName + ".png"));
		images.Images.Add("ResourceFontTtf", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceFontTtf" + stepScaleName + ".png"));
		images.Images.Add("ResourceFontWoff", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "ResourceFontWoff" + stepScaleName + ".png"));
		images.Images.Add("VariableRangeChar", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableRangeChar" + stepScaleName + ".png"));
		images.Images.Add("VariableRangeDateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableRangeDateTime" + stepScaleName + ".png"));
		images.Images.Add("VariableRangeDecimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableRangeDecimal" + stepScaleName + ".png"));
		images.Images.Add("VariableRangeFloat", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableRangeFloat" + stepScaleName + ".png"));
		images.Images.Add("VariableRangeInt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableRangeInt" + stepScaleName + ".png"));
		images.Images.Add("VariableRangeString", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableRangeString" + stepScaleName + ".png"));
		images.Images.Add("VariableListImage", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableListImage" + stepScaleName + ".png"));
		images.Images.Add("VariableListBool", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableListBool" + stepScaleName + ".png"));
		images.Images.Add("VariableListChar", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableListChar" + stepScaleName + ".png"));
		images.Images.Add("VariableListDateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableListDateTime" + stepScaleName + ".png"));
		images.Images.Add("VariableListDecimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableListDecimal" + stepScaleName + ".png"));
		images.Images.Add("VariableListFloat", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableListFloat" + stepScaleName + ".png"));
		images.Images.Add("VariableListInt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableListInt" + stepScaleName + ".png"));
		images.Images.Add("VariableListString", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "VariableListString" + stepScaleName + ".png"));
		images.Images.Add("LockedVariable", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariable" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableBinary", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableBinary" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableBool", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableBool" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableChar", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableChar" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableDateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableDateTime" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableDecimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableDecimal" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableFloat", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableFloat" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableImage", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableImage" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableInt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableInt" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableString", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableString" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableRangeChar", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableRangeChar" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableRangeDateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableRangeDateTime" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableRangeDecimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableRangeDecimal" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableRangeFloat", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableRangeFloat" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableRangeInt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableRangeInt" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableRangeString", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableRangeString" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableListImage", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableListImage" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableListBool", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableListBool" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableListChar", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableListChar" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableListDateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableListDateTime" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableListDecimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableListDecimal" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableListFloat", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableListFloat" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableListInt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableListInt" + stepScaleName + ".png"));
		images.Images.Add("LockedVariableListString", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "LockedVariableListString" + stepScaleName + ".png"));
		images.Images.Add("SystemVariable", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariable" + stepScaleName + ".png"));
		images.Images.Add("SystemVariables", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariables" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableColumn", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableColumn" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableGroupLine", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableGroupLine" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableIsFirstPage", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableIsFirstPage" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableIsFirstPageThrough", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableIsFirstPageThrough" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableIsLastPage", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableIsLastPage" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableIsLastPageThrough", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableIsLastPageThrough" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableLine", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableLine" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableLineABC", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableLineABC" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableLineThrough", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableLineThrough" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableLineRoman", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableLineRoman" + stepScaleName + ".png"));
		images.Images.Add("SystemVariablePageNofM", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariablePageNofM" + stepScaleName + ".png"));
		images.Images.Add("SystemVariablePageNofMThrough", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariablePageNofMThrough" + stepScaleName + ".png"));
		images.Images.Add("SystemVariablePageNumber", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariablePageNumber" + stepScaleName + ".png"));
		images.Images.Add("SystemVariablePageNumberThrough", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariablePageNumberThrough" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableReportAlias", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableReportAlias" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableReportAuthor", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableReportAuthor" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableReportChanged", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableReportChanged" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableReportCreated", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableReportCreated" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableReportDescription", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableReportDescription" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableReportName", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableReportName" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableTime" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableToday", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableToday" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableTotalPageCount", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableTotalPageCount" + stepScaleName + ".png"));
		images.Images.Add("SystemVariableTotalPageCountThrough", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariableTotalPageCountThrough" + stepScaleName + ".png"));
		text = "Office2013.DataRelation" + stepScaleName + ".png";
		images.Images.Add("DataRelation", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.DataRelationActive" + stepScaleName + ".png";
		images.Images.Add("DataRelationActive", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.Parameter" + stepScaleName + ".png";
		images.Images.Add("Parameter", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.Function" + stepScaleName + ".png";
		images.Images.Add("Function", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp." + text));
		images.Images.Add("AggregateFunction", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.AggregateFunction.bmp"));
		text = "Office2013.Category" + stepScaleName + ".png";
		images.Images.Add("Category", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.Database" + stepScaleName + ".png";
		images.Images.Add("Database", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		images.Images.Add("Format", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.TextFormats.Format" + stepScaleName + ".png"));
		images.Images.Add("FormatGeneral", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.TextFormats.FormatGeneral" + stepScaleName + ".png"));
		images.Images.Add("FormatBoolean", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.TextFormats.FormatBoolean" + stepScaleName + ".png"));
		images.Images.Add("FormatCurrency", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.TextFormats.FormatCurrency" + stepScaleName + ".png"));
		images.Images.Add("FormatPercentage", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.TextFormats.FormatPercentage" + stepScaleName + ".png"));
		images.Images.Add("FormatNumber", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.TextFormats.FormatNumber" + stepScaleName + ".png"));
		images.Images.Add("FormatDate", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.TextFormats.FormatDate" + stepScaleName + ".png"));
		images.Images.Add("FormatTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.TextFormats.FormatTime" + stepScaleName + ".png"));
		text = "Office2013.LockedCategory" + stepScaleName + ".png";
		images.Images.Add("LockedCategory", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.LockedDataSource" + stepScaleName + ".png";
		images.Images.Add("LockedDataSource", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		images.Images.Add("LockedDataTransformation", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.LockedDataTransformation" + stepScaleName + ".png"));
		text = "Office2013.LockedRelation" + stepScaleName + ".png";
		images.Images.Add("LockedDataRelation", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.LockedRelationActive" + stepScaleName + ".png";
		images.Images.Add("LockedDataRelationActive", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.LockedParameter" + stepScaleName + ".png";
		images.Images.Add("LockedParameter", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.LockedDatabase" + stepScaleName + ".png";
		images.Images.Add("LockedDatabase", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.UndefinedDataSource" + stepScaleName + ".png";
		images.Images.Add("UndefinedDataSource", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.UndefinedDatabase" + stepScaleName + ".png";
		images.Images.Add("UndefinedDatabase", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.Function" + stepScaleName + ".png";
		images.Images.Add("Function", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.DataStore" + stepScaleName + ".png";
		images.Images.Add("DataStore", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.DatabaseFail" + stepScaleName + ".png";
		images.Images.Add("DatabaseFail", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		images.Images.Add("DatabaseTransformation", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.DatabaseTransformation" + stepScaleName + ".png"));
		text = "Office2013.HtmlTag" + stepScaleName + ".png";
		images.Images.Add("HtmlTag", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		text = "Office2013.BusinessObject" + stepScaleName + ".png";
		images.Images.Add("BusinessObject", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text));
		images.Images.Add("LockedBusinessObject", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.LockedBusinessObject" + stepScaleName + ".png"));
		images.Images.Add("SystemVariablePageCopyNumber", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary." + text2 + "SystemVariablePageNumber" + stepScaleName + ".png"));
	}

	public void Build(TreeNodeCollection nodes, StiDictionary dictionary, bool dictionaryDesign, bool includeCalcColumn)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		level = 0;
		StiDataSourcesCollection dataSources = dictionary.DataSources;
		StiBusinessObjectsCollection businessObjects = dictionary.BusinessObjects;
		foreach (StiDataSource item in dataSources)
		{
			if (dictionary.Restrictions.IsAllowShow(item.Name, StiDataType.Variable))
			{
				TreeNodeEx dataNode = new TreeNodeEx();
				BuildDataSource(item, ref dataNode, includeCalcColumn);
				nodes.Add((TreeNode)(object)dataNode);
			}
		}
		foreach (StiBusinessObject item2 in businessObjects)
		{
			if (dictionary.Restrictions.IsAllowShow(item2.Name, StiDataType.Variable))
			{
				TreeNode val = new TreeNode();
				bool? flag = item2.Dictionary?.Report?.Designer?.UseAliases;
				val.Text = item2.ToString(StiOptions.Dictionary.ShowOnlyAliasForBusinessObject && flag.GetValueOrDefault(false));
				string imageKey = (val.SelectedImageKey = "BusinessObject");
				val.ImageKey = imageKey;
				val.Tag = item2;
				BuildBusinessObject(item2, val, includeColumns: true, includeCalcColumn);
				nodes.Add(val);
			}
		}
	}

	public void BuildDataSource(StiDataSource dataSource, ref TreeNodeEx dataNode, bool includeCalcColumn)
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		if (dataSource != null)
		{
			bool? flag = dataSource.Dictionary?.Report?.Designer?.UseAliases;
			((TreeNode)dataNode).Text = dataSource.ToString(StiOptions.Dictionary.ShowOnlyAliasForDataSource && flag.GetValueOrDefault(false));
			TreeNodeEx obj = dataNode;
			string imageKey = (((TreeNode)dataNode).SelectedImageKey = "DataSource");
			((TreeNode)obj).ImageKey = imageKey;
			((TreeNode)dataNode).Tag = dataSource;
			if (usedDataSources != null && usedDataSources[dataSource] != null && treeView != null)
			{
				treeView.SetBold((TreeNode)(object)dataNode, isBold: true);
			}
			if (CheckDataSource(dataSource, includeCalcColumn))
			{
				((TreeNode)dataNode).Nodes.Add(new TreeNode("Loading"));
			}
		}
		else
		{
			dataNode = null;
		}
	}

	private void ExpandedDataSource(StiDataSource dataSource, TreeNodeEx parentNode, bool includeCalcColumn)
	{
		StiDataSourcesCollection datas = new StiDataSourcesCollection(null);
		parentNode.FillFromNodeEx(ref datas, ref level);
		ExpandedDataSource(dataSource, (TreeNode)(object)parentNode, datas, includeCalcColumn);
	}

	private void ExpandedDataSource(StiDataSource dataSource, TreeNode parentNode, StiDataSourcesCollection datas, bool includeCalcColumn)
	{
		level++;
		try
		{
			if (StiOptions.Designer.MaxLevelOfDictionaryObjects <= 0 || StiOptions.Designer.MaxLevelOfDictionaryObjects >= level)
			{
				if (!datas.Contains(dataSource))
				{
					datas.Add(dataSource);
					BuildRelations(dataSource.GetParentRelations(), parentNode, datas, includeCalcColumn);
					datas.Remove(dataSource);
				}
				BuildColumns(dataSource, dataSource.Columns, parentNode, includeCalcColumn);
				if (dataSource is StiSqlSource stiSqlSource && stiSqlSource.Parameters.Count > 0)
				{
					BuildParameters(stiSqlSource, parentNode);
				}
			}
		}
		finally
		{
			level--;
		}
	}

	private void BuildRelations(StiDataRelationsCollection relations, TreeNode parentNode, StiDataSourcesCollection datas, bool includeCalcColumn)
	{
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		if (relations.Count <= 0)
		{
			return;
		}
		foreach (StiDataRelation relation in relations)
		{
			if (relation.Dictionary == null || relation.Dictionary.Restrictions.IsAllowShow(relation.Name, StiDataType.DataRelation))
			{
				bool? flag = relation.Dictionary?.Report?.Designer?.UseAliases;
				string text = relation.ToString(StiOptions.Dictionary.ShowOnlyAliasForDataRelation && flag.GetValueOrDefault(false));
				TreeNodeEx treeNodeEx = new TreeNodeEx(text);
				treeNodeEx.FillNodeEx(datas, level);
				((TreeNode)treeNodeEx).Tag = relation;
				string imageKey = (((TreeNode)treeNodeEx).SelectedImageKey = (relation.Inherited ? "LockedDataRelation" : "DataRelation"));
				((TreeNode)treeNodeEx).ImageKey = imageKey;
				if (relation.Active)
				{
					imageKey = (((TreeNode)treeNodeEx).SelectedImageKey = ((TreeNode)treeNodeEx).SelectedImageKey + "Active");
					((TreeNode)treeNodeEx).ImageKey = imageKey;
				}
				if (usedRelations != null && usedRelations[relation] != null && treeView != null)
				{
					treeView.SetBold((TreeNode)(object)treeNodeEx, isBold: true);
				}
				if (relation.ParentSource != null && CheckDataSource(relation.ParentSource, includeCalcColumn))
				{
					((TreeNode)treeNodeEx).Nodes.Add(new TreeNode("Loading"));
				}
				parentNode.Nodes.Add((TreeNode)(object)treeNodeEx);
			}
		}
	}

	private void ExpandedRelations(TreeNodeEx parentNode, bool includeCalcColumn)
	{
		StiDataRelation stiDataRelation = ((TreeNode)parentNode).Tag as StiDataRelation;
		StiDataSource parentSource = stiDataRelation.ParentSource;
		if (parentSource != null)
		{
			StiDataSourcesCollection datas = new StiDataSourcesCollection(null);
			parentNode.FillFromNodeEx(ref datas, ref level);
			ExpandedDataSource(stiDataRelation.ParentSource, (TreeNode)(object)parentNode, datas, includeCalcColumn);
		}
	}

	public void BuildBusinessObject(StiBusinessObject businessObject, ref TreeNodeEx dataNode, bool includeCalcColumn)
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		if (businessObject != null)
		{
			bool? flag = businessObject.Dictionary?.Report?.Designer?.UseAliases;
			((TreeNode)dataNode).Text = businessObject.ToString(StiOptions.Dictionary.ShowOnlyAliasForBusinessObject && flag.GetValueOrDefault(false));
			TreeNodeEx obj = dataNode;
			string imageKey = (((TreeNode)dataNode).SelectedImageKey = "BusinessObject");
			((TreeNode)obj).ImageKey = imageKey;
			((TreeNode)dataNode).Tag = businessObject;
			if (usedDataSources != null && usedDataSources[businessObject] != null && treeView != null)
			{
				treeView.SetBold((TreeNode)(object)dataNode, isBold: true);
			}
			if (CheckBusinessObject(businessObject, includeCalcColumn, includeCalcColumns: true))
			{
				((TreeNode)dataNode).Nodes.Add(new TreeNode("Loading"));
			}
		}
		else
		{
			dataNode = null;
		}
	}

	public void BuildBusinessObjects(StiBusinessObjectsCollection objects, TreeNode parentNode, bool includeColumns, bool includeCalcColumns)
	{
		BuildBusinessObjects(objects, parentNode, includeColumns, includeCalcColumns, showCategories: true);
	}

	private void BuildBusinessObjects(StiBusinessObjectsCollection objects, TreeNode parentNode, bool includeColumns, bool includeCalcColumns, bool showCategories)
	{
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		foreach (StiBusinessObject @object in objects)
		{
			if (@object.Dictionary != null && !@object.Dictionary.Restrictions.IsAllowShow(@object.Name, StiDataType.BusinessObject))
			{
				continue;
			}
			TreeNodeEx treeNodeEx = new TreeNodeEx(@object.ToString());
			string imageKey = (((TreeNode)treeNodeEx).SelectedImageKey = (@object.Inherited ? "LockedBusinessObject" : "BusinessObject"));
			((TreeNode)treeNodeEx).ImageKey = imageKey;
			((TreeNode)treeNodeEx).Tag = @object;
			if (showCategories)
			{
				TreeNode val = null;
				if (!string.IsNullOrEmpty(@object.Category))
				{
					foreach (TreeNode node in parentNode.Nodes)
					{
						TreeNode val2 = node;
						if (val2.Text == @object.Category)
						{
							val = val2;
							break;
						}
					}
					if (val == null)
					{
						val = new TreeNode(@object.Category)
						{
							Tag = new StiBusinessObjectCategory(@object.Category),
							ImageKey = "Category",
							SelectedImageKey = "Category"
						};
						parentNode.Nodes.Add(val);
					}
					val.Nodes.Add((TreeNode)(object)treeNodeEx);
				}
				else
				{
					parentNode.Nodes.Add((TreeNode)(object)treeNodeEx);
				}
			}
			else
			{
				parentNode.Nodes.Add((TreeNode)(object)treeNodeEx);
			}
			if (CheckBusinessObject(@object, includeColumns, includeCalcColumns))
			{
				((TreeNode)treeNodeEx).Nodes.Add(new TreeNode("Loading"));
			}
		}
	}

	private void ExpandedBusinessObject(StiBusinessObject businessObject, TreeNode parentNode, bool includeColumns, bool includeCalcColumns)
	{
		BuildBusinessObject(businessObject, parentNode, includeColumns, includeCalcColumns);
	}

	public void BuildBusinessObject(StiBusinessObject businessObject, TreeNode parentNode, bool includeColumns, bool includeCalcColumns)
	{
		BuildBusinessObjects(businessObject.BusinessObjects, parentNode, includeColumns, includeCalcColumns, showCategories: false);
		if (includeColumns)
		{
			BuildColumns(businessObject, businessObject.Columns, parentNode, includeCalcColumns);
		}
	}

	private void BuildColumns(StiDataSource dataSource, StiDataColumnsCollection columns, TreeNode parentNode, bool includeCalcColumn)
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		if (dataSource == null || dataSource.Dictionary == null)
		{
			return;
		}
		StiDictionary dictionary = dataSource.Dictionary;
		foreach (StiDataColumn column in columns)
		{
			if (!IsSkipColumn(column) && (column.DataSource == null || dictionary.Restrictions.IsAllowShow(column.DataSource.Name + "." + column.Name, StiDataType.DataColumn)) && (!(column is StiCalcDataColumn) || includeCalcColumn))
			{
				bool? flag = dictionary?.Report?.Designer?.UseAliases;
				string text = column.ToString(StiOptions.Dictionary.ShowOnlyAliasForDataColumn && flag.GetValueOrDefault(false));
				string imageKeyFromColumn = GetImageKeyFromColumn(column);
				TreeNode val = new TreeNode
				{
					Text = text,
					ImageKey = imageKeyFromColumn,
					SelectedImageKey = imageKeyFromColumn,
					Tag = column
				};
				BuildType(column.Type, imageKeyFromColumn, val);
				parentNode.Nodes.Add(val);
				if (usedColumns != null && usedColumns[column] != null && treeView != null)
				{
					treeView.SetBold(val, isBold: true);
				}
			}
		}
	}

	private void BuildColumns(StiBusinessObject businessObject, StiDataColumnsCollection columns, TreeNode parentNode, bool includeCalcColumn)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (businessObject == null || businessObject.Dictionary == null)
		{
			return;
		}
		foreach (StiDataColumn column in columns)
		{
			if (!IsSkipColumn(column) && (column.BusinessObject == null || businessObject.Dictionary.Restrictions.IsAllowShow(column.BusinessObject.Name + "." + column.Name, StiDataType.DataColumn)) && (!(column is StiCalcDataColumn) || includeCalcColumn))
			{
				string text = column.ToString();
				string imageKeyFromColumn = GetImageKeyFromColumn(column);
				TreeNode val = new TreeNode(text);
				string imageKey = (val.SelectedImageKey = imageKeyFromColumn);
				val.ImageKey = imageKey;
				val.Tag = column;
				Type type = column.Type;
				BuildType(type, imageKeyFromColumn, val);
				parentNode.Nodes.Add(val);
				if (usedColumns != null && usedColumns[column] != null && treeView != null)
				{
					treeView.SetBold(val, isBold: true);
				}
			}
		}
	}

	private void BuildParameters(StiSqlSource dataSource, TreeNode parentNode)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		if (!ShowParameters)
		{
			return;
		}
		TreeNode val = new TreeNode(StiLocalization.Get("PropertyMain", "Parameters"));
		string imageKey = (val.SelectedImageKey = "Parameter");
		val.ImageKey = imageKey;
		if (dataSource != null && dataSource.Dictionary != null)
		{
			StiDataParametersCollection parameters = dataSource.Parameters;
			foreach (StiDataParameter item in parameters)
			{
				if (item.DataSource != null)
				{
					string text2 = item.ToString();
					TreeNode val2 = new TreeNode(text2);
					imageKey = (val2.SelectedImageKey = (dataSource.Inherited ? "LockedParameter" : "Parameter"));
					val2.ImageKey = imageKey;
					val2.Tag = item;
					val.Nodes.Add(val2);
				}
			}
		}
		parentNode.Nodes.Add(val);
	}

	private void BuildType(Type propertyType, string imageKey, TreeNode parentNode)
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		int num = 5;
		if (StiTypeFinder.FindInterface(propertyType, typeof(IComponent)))
		{
			num = 2;
		}
		level++;
		if (level < num && propertyType != typeof(Image) && propertyType != typeof(string) && (propertyType.IsClass || propertyType.IsInterface) && !StiTypeFinder.FindInterface(propertyType, typeof(IEnumerable)))
		{
			if (StiOptions.Dictionary.BusinessObjects.AllowUseProperties)
			{
				PropertyInfo[] properties = propertyType.GetProperties();
				PropertyInfo[] array = properties;
				foreach (PropertyInfo propertyInfo in array)
				{
					object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(BrowsableAttribute), inherit: true);
					if (customAttributes == null || customAttributes.Length == 0 || ((BrowsableAttribute)customAttributes[0]).Browsable || StiOptions.Dictionary.BusinessObjects.PropertiesProcessingType != StiPropertiesProcessingType.Browsable)
					{
						TreeNode val = new TreeNode(propertyInfo.Name);
						string imageKey2 = (val.SelectedImageKey = imageKey);
						val.ImageKey = imageKey2;
						val.Tag = propertyInfo;
						BuildType(propertyInfo.PropertyType, imageKey, val);
						parentNode.Nodes.Add(val);
					}
				}
			}
			if (StiOptions.Dictionary.BusinessObjects.AllowUseFields)
			{
				FieldInfo[] fields = propertyType.GetFields();
				FieldInfo[] array2 = fields;
				foreach (FieldInfo fieldInfo in array2)
				{
					object[] customAttributes2 = fieldInfo.GetCustomAttributes(typeof(BrowsableAttribute), inherit: true);
					if (customAttributes2 == null || customAttributes2.Length == 0 || ((BrowsableAttribute)customAttributes2[0]).Browsable || StiOptions.Dictionary.BusinessObjects.PropertiesProcessingType != StiPropertiesProcessingType.Browsable)
					{
						TreeNode val2 = new TreeNode(fieldInfo.Name);
						string imageKey2 = (val2.SelectedImageKey = imageKey);
						val2.ImageKey = imageKey2;
						val2.Tag = fieldInfo;
						BuildType(fieldInfo.FieldType, imageKey, val2);
						parentNode.Nodes.Add(val2);
					}
				}
			}
		}
		level--;
	}

	private bool CheckDataSource(StiDataSource dataSource, bool includeCalcColumn)
	{
		bool flag = false;
		flag = CheckRelations(dataSource);
		if (!flag)
		{
			flag = CheckColumns(dataSource, includeCalcColumn);
		}
		if (!flag && dataSource is StiSqlSource stiSqlSource && stiSqlSource.Parameters.Count > 0)
		{
			flag = CheckParameters(stiSqlSource);
		}
		return flag;
	}

	public bool CheckBusinessObject(StiBusinessObject businessObject, bool includeColumns, bool includeCalcColumns)
	{
		bool flag = false;
		flag = CheckBusinessObjects(businessObject.BusinessObjects);
		if (!flag && includeColumns)
		{
			flag = CheckColumns(businessObject, includeCalcColumns);
		}
		return flag;
	}

	private bool CheckBusinessObjects(StiBusinessObjectsCollection objects)
	{
		foreach (StiBusinessObject @object in objects)
		{
			if (@object.Dictionary == null || @object.Dictionary.Restrictions.IsAllowShow(@object.Name, StiDataType.BusinessObject))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckColumns(StiBusinessObject businessObject, bool includeCalcColumn)
	{
		if (businessObject == null || businessObject.Dictionary == null)
		{
			return false;
		}
		StiDictionary dictionary = businessObject.Dictionary;
		StiDataColumnsCollection columns = businessObject.Columns;
		foreach (StiDataColumn item in columns)
		{
			if (!IsSkipColumn(item) && (item.DataSource == null || dictionary.Restrictions.IsAllowShow(item.BusinessObject.Name + "." + item.Name, StiDataType.DataColumn)) && (!(item is StiCalcDataColumn) || includeCalcColumn))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckRelations(StiDataSource dataSource)
	{
		StiDictionary dictionary = dataSource.Dictionary;
		if (dictionary != null)
		{
			foreach (StiDataRelation relation in dictionary.Relations)
			{
				if (relation.ChildSource == dataSource && dictionary.Restrictions.IsAllowShow(relation.Name, StiDataType.DataRelation))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool CheckColumns(StiDataSource dataSource, bool includeCalcColumn)
	{
		if (dataSource == null || dataSource.Dictionary == null)
		{
			return false;
		}
		StiDictionary dictionary = dataSource.Dictionary;
		StiDataColumnsCollection columns = dataSource.Columns;
		foreach (StiDataColumn item in columns)
		{
			if (!IsSkipColumn(item) && (item.DataSource == null || dictionary.Restrictions.IsAllowShow(item.DataSource.Name + "." + item.Name, StiDataType.DataColumn)) && (!(item is StiCalcDataColumn) || includeCalcColumn))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckParameters(StiSqlSource dataSource)
	{
		if (!ShowParameters || dataSource == null || dataSource.Dictionary == null)
		{
			return false;
		}
		StiDataParametersCollection parameters = dataSource.Parameters;
		foreach (StiDataParameter item in parameters)
		{
			if (item.DataSource != null)
			{
				return true;
			}
		}
		return false;
	}

	public void ExpandedNode(TreeNodeEx node, bool includeCalcColumn)
	{
		object tag = ((TreeNode)node).Tag;
		ExpandedNode(node, includeCalcColumn, tag);
	}

	public void ExpandedNode(TreeNodeEx node, bool includeColumn, bool includeCalcColumn)
	{
		object tag = ((TreeNode)node).Tag;
		ExpandedNode(node, includeColumn, includeCalcColumn, tag);
	}

	public void ExpandedNode(TreeNodeEx node, bool includeCalcColumn, object tag)
	{
		ExpandedNode(node, includeColumn: true, includeCalcColumn, tag);
	}

	public void ExpandedNode(TreeNodeEx node, bool includeColumn, bool includeCalcColumn, object tag)
	{
		if (tag != null && ((TreeNode)node).Nodes.Count == 1 && ((TreeNode)node).Nodes[0].Text == "Loading")
		{
			((TreeNode)node).Nodes.Clear();
			if (tag is StiDataSource)
			{
				ExpandedDataSource(tag as StiDataSource, node, includeCalcColumn);
			}
			else if (tag is StiDataRelation)
			{
				ExpandedRelations(node, includeCalcColumn);
			}
			else if (tag is StiBusinessObject)
			{
				ExpandedBusinessObject(tag as StiBusinessObject, (TreeNode)(object)node, includeColumn, includeCalcColumn);
			}
		}
	}

	public void CollapsedNode(TreeNode node)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		if (!(node is TreeNodeEx))
		{
			return;
		}
		object tag = node.Tag;
		if (tag != null && node.Nodes.Count == 1 && node.Nodes[0].Text == "Loading")
		{
			node.Nodes.Clear();
			if (tag is StiDataSource || tag is StiDataRelation || tag is StiBusinessObject)
			{
				TreeNode val = new TreeNode("Loading");
				node.Nodes.Add(val);
			}
		}
	}

	public StiDataBuilder()
		: this(null)
	{
	}

	public StiDataBuilder(StiTreeView treeView)
		: this(null, null, null, treeView)
	{
	}

	public StiDataBuilder(Hashtable usedRelations, Hashtable usedDataSources, Hashtable usedColumns, StiTreeView treeView)
	{
		this.usedRelations = usedRelations;
		this.usedDataSources = usedDataSources;
		this.usedColumns = usedColumns;
		this.treeView = treeView;
	}
}
