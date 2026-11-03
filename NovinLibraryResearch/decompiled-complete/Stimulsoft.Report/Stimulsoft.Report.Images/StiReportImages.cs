using System.Collections;
using System.Drawing;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Images;

public static class StiReportImages
{
	public static class Actions
	{
		public static Image DropDown(bool allowCache = true)
		{
			return GetImage("Actions.DropDown", (StiImageSize)0, allowCache);
		}

		public static Image DropDownArrowMouseOver(bool allowCache = true)
		{
			return GetImage("Actions.DropDownArrowMouseOver", (StiImageSize)0, allowCache);
		}

		public static Image DropDownFilter(bool allowCache = true)
		{
			return GetImage("Actions.DropDownFilter", (StiImageSize)0, allowCache);
		}

		public static Image DropDownFilterMouseOver(bool allowCache = true)
		{
			return GetImage("Actions.DropDownFilterMouseOver", (StiImageSize)0, allowCache);
		}

		public static Image DropDownArrowMouseOverWhite(bool allowCache = true)
		{
			return GetImage("Actions.DropDownArrowMouseOver-white", (StiImageSize)0, allowCache);
		}

		public static Image DropDownFilterWhite(bool allowCache = true)
		{
			return GetImage("Actions.DropDownFilter-white", (StiImageSize)0, allowCache);
		}

		public static Image DropDownFilterMouseOverWhite(bool allowCache = true)
		{
			return GetImage("Actions.DropDownFilterMouseOver-white", (StiImageSize)0, allowCache);
		}

		public static Image SortArrowAsc(bool allowCache = true)
		{
			return GetImage("Actions.SortArrowAsc", (StiImageSize)0, allowCache);
		}

		public static Image SortArrowAscMouseOver(bool allowCache = true)
		{
			return GetImage("Actions.SortArrowAscMouseOver", (StiImageSize)0, allowCache);
		}

		public static Image SortArrowDesc(bool allowCache = true)
		{
			return GetImage("Actions.SortArrowDesc", (StiImageSize)0, allowCache);
		}

		public static Image SortArrowDescMouseOver(bool allowCache = true)
		{
			return GetImage("Actions.SortArrowDescMouseOver", (StiImageSize)0, allowCache);
		}

		public static Image SortArrowAscWhite(bool allowCache = true)
		{
			return GetImage("Actions.SortArrowAsc-white", (StiImageSize)0, allowCache);
		}

		public static Image SortArrowAscMouseOverWhite(bool allowCache = true)
		{
			return GetImage("Actions.SortArrowAscMouseOver-white", (StiImageSize)0, allowCache);
		}

		public static Image SortArrowAscMouseOverDisable(bool allowCache = true)
		{
			return GetImage("Actions.SortArrowAscMouseOver-disable", (StiImageSize)0, allowCache);
		}

		public static Image SortArrowDescWhite(bool allowCache = true)
		{
			return GetImage("Actions.SortArrowDesc-white", (StiImageSize)0, allowCache);
		}

		public static Image SortArrowDescMouseOverWhite(bool allowCache = true)
		{
			return GetImage("Actions.SortArrowDescMouseOver-white", (StiImageSize)0, allowCache);
		}

		public static Image SortAsc(bool allowCache = true)
		{
			return GetImage("Actions.SortAsc", (StiImageSize)0, allowCache);
		}

		public static Image SortDesc(bool allowCache = true)
		{
			return GetImage("Actions.SortDesc", (StiImageSize)0, allowCache);
		}
	}

	public static class Dictionary
	{
		public static Image CalcColumn(bool allowCache = true)
		{
			return GetDictionaryImage("CalcColumn", (StiImageSize)0, allowCache);
		}

		public static Image CalcColumnBool(bool allowCache = true)
		{
			return GetDictionaryImage("CalcColumnBool", (StiImageSize)0, allowCache);
		}

		public static Image CalcColumnChar(bool allowCache = true)
		{
			return GetDictionaryImage("CalcColumnChar", (StiImageSize)0, allowCache);
		}

		public static Image CalcColumnDateTime(bool allowCache = true)
		{
			return GetDictionaryImage("CalcColumnDateTime", (StiImageSize)0, allowCache);
		}

		public static Image CalcColumnInt(bool allowCache = true)
		{
			return GetDictionaryImage("CalcColumnInt", (StiImageSize)0, allowCache);
		}

		public static Image CalcColumnFloat(bool allowCache = true)
		{
			return GetDictionaryImage("CalcColumnFloat", (StiImageSize)0, allowCache);
		}

		public static Image CalcColumnImage(bool allowCache = true)
		{
			return GetDictionaryImage("CalcColumnImage", (StiImageSize)0, allowCache);
		}

		public static Image CalcColumnBinary(bool allowCache = true)
		{
			return GetDictionaryImage("CalcColumnBinary", (StiImageSize)0, allowCache);
		}

		public static Image CalcColumnDecimal(bool allowCache = true)
		{
			return GetDictionaryImage("CalcColumnDecimal", (StiImageSize)0, allowCache);
		}

		public static Image Category(bool allowCache = true)
		{
			return GetDictionaryImage("Category", (StiImageSize)0, allowCache);
		}

		public static Image Close(bool allowCache = true)
		{
			return GetDictionaryImage("Close", (StiImageSize)0, allowCache);
		}

		public static Image Database(bool allowCache = true)
		{
			return GetDictionaryImage("Database", (StiImageSize)0, allowCache);
		}

		public static Image BusinessObject(bool allowCache = true)
		{
			return GetDictionaryImage("BusinessObject", (StiImageSize)0, allowCache);
		}

		public static Image DataColumn(bool allowCache = true)
		{
			return GetDictionaryImage("DataColumn", (StiImageSize)0, allowCache);
		}

		public static Image DataColumnBool(bool allowCache = true)
		{
			return GetDictionaryImage("DataColumnBool", (StiImageSize)0, allowCache);
		}

		public static Image DataColumnChar(bool allowCache = true)
		{
			return GetDictionaryImage("DataColumnChar", (StiImageSize)0, allowCache);
		}

		public static Image DataColumnDateTime(bool allowCache = true)
		{
			return GetDictionaryImage("DataColumnDateTime", (StiImageSize)0, allowCache);
		}

		public static Image DataColumnDecimal(bool allowCache = true)
		{
			return GetDictionaryImage("DataColumnDecimal", (StiImageSize)0, allowCache);
		}

		public static Image DataColumnInt(bool allowCache = true)
		{
			return GetDictionaryImage("DataColumnInt", (StiImageSize)0, allowCache);
		}

		public static Image DataColumnFloat(bool allowCache = true)
		{
			return GetDictionaryImage("DataColumnFloat", (StiImageSize)0, allowCache);
		}

		public static Image DataColumnImage(bool allowCache = true)
		{
			return GetDictionaryImage("DataColumnImage", (StiImageSize)0, allowCache);
		}

		public static Image DataColumnBinary(bool allowCache = true)
		{
			return GetDictionaryImage("DataColumnBinary", (StiImageSize)0, allowCache);
		}

		public static Image DataParameter(bool allowCache = true)
		{
			return GetDictionaryImage("Parameter", (StiImageSize)0, allowCache);
		}

		public static Image DataRelation(bool allowCache = true)
		{
			return GetDictionaryImage("DataRelation", (StiImageSize)0, allowCache);
		}

		public static Image DataSource(bool allowCache = true)
		{
			return GetDictionaryImage("DataSource", (StiImageSize)0, allowCache);
		}

		public static Image Variable(bool allowCache = true)
		{
			return GetDictionaryImage("Variable", (StiImageSize)0, allowCache);
		}
	}

	public static class Styles
	{
		public static Image Cards(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("Styles.Cards", size, allowCache);
		}

		public static Image Chart(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("Styles.Chart", size, allowCache);
		}

		public static Image CheckBox(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("Styles.CheckBox", size, allowCache);
		}

		public static Image Image(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("Styles.Image", size, allowCache);
		}

		public static Image Map(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("Styles.Map", size, allowCache);
		}

		public static Image Progress(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("Styles.Progress", size, allowCache);
		}

		public static Image RichText(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("Styles.RichText", size, allowCache);
		}

		public static Image Style(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("Styles.Style", size, allowCache);
		}

		public static Image Text(StiImageSize size = 0, bool allowCache = true)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return GetImage("Styles.Text", size, allowCache);
		}
	}

	private const string RootPath = "Stimulsoft.Report.Images";

	private const string RootPathDictionary = "Stimulsoft.Report.Bmp.Dictionary.Office2013";

	private static Hashtable mainCache = new Hashtable();

	private static Hashtable dictionaryCache = new Hashtable();

	public static Image GetImage(string path, StiImageSize size = 0, bool allowCache = true)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		Bitmap val = (Bitmap)(allowCache ? /*isinst with value type is only supported in some contexts*/: null);
		if (val != null && !IsDisposedImage((Image)(object)val))
		{
			return (Image)(object)val;
		}
		Assembly assembly = typeof(StiReportImages).Assembly;
		Image image = StiScaledImagesHelper.GetImage(assembly, "Stimulsoft.Report.Images." + path, size);
		if (allowCache)
		{
			mainCache[path + ((object)(StiImageSize)(ref size)).ToString()] = image;
		}
		return image;
	}

	public static Image GetDictionaryImage(string path, StiImageSize size = 0, bool allowCache = true)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		Bitmap val = (Bitmap)(allowCache ? /*isinst with value type is only supported in some contexts*/: null);
		if (val != null && !IsDisposedImage((Image)(object)val))
		{
			return (Image)(object)val;
		}
		Assembly assembly = typeof(StiReportImages).Assembly;
		Image image = StiScaledImagesHelper.GetImage(assembly, "Stimulsoft.Report.Bmp.Dictionary.Office2013." + path, size);
		if (allowCache)
		{
			dictionaryCache[path + ((object)(StiImageSize)(ref size)).ToString()] = image;
		}
		return image;
	}

	private static bool IsDisposedImage(Image image)
	{
		try
		{
			return image.Width == 0;
		}
		catch
		{
			return true;
		}
	}
}
