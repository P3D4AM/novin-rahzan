using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base;

public class StiObjectStateSaver
{
	public static string WriteObjectStateToString(object obj)
	{
		return WriteObjectStateToString(obj, new StiObjectStringConverter());
	}

	public static string WriteObjectStateToString(object obj, StiObjectStringConverter converter)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			StringBuilder stringBuilder = new StringBuilder();
			StiSerializing stiSerializing = new StiSerializing(converter);
			using (StringWriter textWriter = new StringWriter(stringBuilder))
			{
				stiSerializing.SortProperties = false;
				stiSerializing.CheckSerializable = true;
				stiSerializing.Serialize(obj, textWriter, "State", StiSerializeTypes.SerializeToAll);
			}
			return stringBuilder.ToString();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public static void ReadObjectStateFromString(object obj, string str)
	{
		ReadObjectStateFromString(obj, str, new StiObjectStringConverter());
	}

	public static void ReadObjectStateFromString(object obj, string str, StiObjectStringConverter converter)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			using StringReader textReader = new StringReader(str);
			StiSerializing stiSerializing = new StiSerializing(converter);
			stiSerializing.Deserialize(obj, textReader, "State");
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}
}
