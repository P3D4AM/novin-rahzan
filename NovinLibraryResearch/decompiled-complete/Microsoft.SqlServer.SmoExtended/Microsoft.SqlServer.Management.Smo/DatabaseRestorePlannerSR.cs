using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.Smo;

[CompilerGenerated]
internal class DatabaseRestorePlannerSR
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(DatabaseRestorePlannerSR));

		private static CultureInfo _culture = null;

		public const string SelectingBackupSets = "SelectingBackupSets";

		public const string IdentifyingMediaSets = "IdentifyingMediaSets";

		public const string ReadingBackupSetHeader = "ReadingBackupSetHeader";

		public const string IncompatibleObject = "IncompatibleObject";

		public static CultureInfo Culture
		{
			get
			{
				return _culture;
			}
			set
			{
				_culture = value;
			}
		}

		private Keys()
		{
		}

		public static string GetString(string key)
		{
			return resourceManager.GetString(key, _culture);
		}

		public static string GetString(string key, object arg0, object arg1)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0, arg1);
		}
	}

	public static CultureInfo Culture
	{
		get
		{
			return Keys.Culture;
		}
		set
		{
			Keys.Culture = value;
		}
	}

	public static string SelectingBackupSets => Keys.GetString("SelectingBackupSets");

	public static string IdentifyingMediaSets => Keys.GetString("IdentifyingMediaSets");

	public static string ReadingBackupSetHeader => Keys.GetString("ReadingBackupSetHeader");

	protected DatabaseRestorePlannerSR()
	{
	}

	public static string IncompatibleObject(string type, string name)
	{
		return Keys.GetString("IncompatibleObject", type, name);
	}
}
