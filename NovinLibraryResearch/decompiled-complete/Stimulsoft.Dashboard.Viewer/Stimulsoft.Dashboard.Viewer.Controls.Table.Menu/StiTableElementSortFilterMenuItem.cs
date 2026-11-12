using System;
using System.Globalization;
using System.Windows.Forms;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Viewer.Controls.Table.Menu;

public class StiTableElementSortFilterMenuItem
{
	public object Item { get; set; }

	public Type Type { get; set; }

	public ToolStripMenuItem MenuItem { get; set; }

	public string DisplayString
	{
		get
		{
			if (Item == DBNull.Value || Item == null)
			{
				return Loc.Get("Dashboard", "Nulls");
			}
			if (string.IsNullOrWhiteSpace(Item.ToString()))
			{
				return Loc.Get("Dashboard", "Blanks");
			}
			if (Type.IsEnum && Enum.IsDefined(Type, Item))
			{
				return Enum.GetName(Type, Item);
			}
			if (Item is DateTime)
			{
				return ((DateTime)Item).ToShortDateString();
			}
			bool flag = false;
			string text = Item.ToString();
			int num = text.IndexOf(Environment.NewLine);
			if (num != -1)
			{
				text = text.Substring(0, num);
				flag = true;
			}
			if (text.Length > 100)
			{
				text = text.Substring(0, 100);
				flag = true;
			}
			if (flag)
			{
				text += "...";
			}
			return text;
		}
	}

	public string FilterString
	{
		get
		{
			if (Item == DBNull.Value || Item == null || Type == null)
			{
				return null;
			}
			if (string.IsNullOrWhiteSpace(Item.ToString()))
			{
				return string.Empty;
			}
			if (Type.IsEnum && Enum.IsDefined(Type, Item))
			{
				return Enum.GetName(Type, Item);
			}
			if (Type.IsNumericType())
			{
				return Item.ToString().Replace(",", ".");
			}
			if (Item is DateTime)
			{
				return ((DateTime)Item).ToString("MM'/'dd'/'yyyy", CultureInfo.InvariantCulture);
			}
			return Item.ToString();
		}
	}

	public override string ToString()
	{
		return DisplayString;
	}

	public StiTableElementSortFilterMenuItem()
	{
	}

	public StiTableElementSortFilterMenuItem(object item, Type type)
		: this()
	{
		Item = item;
		Type = type;
	}
}
