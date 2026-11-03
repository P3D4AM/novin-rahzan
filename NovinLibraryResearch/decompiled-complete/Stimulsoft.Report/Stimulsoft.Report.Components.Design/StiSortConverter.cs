using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components.Design;

public class StiSortConverter : TypeConverter
{
	public string SortToString(string[] sorts, StiDataSource dataSource)
	{
		if (dataSource == null || sorts == null || sorts.Length == 0)
		{
			return "[" + Loc.Get("FormBand", "NoSort") + "]";
		}
		sorts = (string[])sorts.Clone();
		string text = Loc.Get("FormBand", "SortBy") + " ";
		int num = 0;
		while (num < sorts.Length)
		{
			StiDataSource stiDataSource = dataSource;
			int num2 = num;
			if (sorts[num] == "DESC")
			{
				text = text + sorts[num] + " ";
			}
			num++;
			while (num < sorts.Length - 1 && sorts[num + 1] != "ASC" && sorts[num + 1] != "DESC")
			{
				StiDataRelationsCollection parentRelations = stiDataSource.GetParentRelations();
				foreach (StiDataRelation item in parentRelations)
				{
					if (!(item.NameInSource != sorts[num]))
					{
						sorts[num] = item.Alias;
						stiDataSource = item.ParentSource;
						break;
					}
				}
				text += sorts[num++];
				if (num - num2 > 0)
				{
					text += ".";
				}
			}
			text += sorts[num++];
			if (num < sorts.Length - 1)
			{
				text += ", ";
			}
		}
		return text;
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			if (!(context.Instance is Array))
			{
				return SortToString((string[])value, ((IStiDataSource)context.Instance).DataSource);
			}
			foreach (IStiDataSource item in (Array)context.Instance)
			{
				SortToString((string[])value, item.DataSource);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
