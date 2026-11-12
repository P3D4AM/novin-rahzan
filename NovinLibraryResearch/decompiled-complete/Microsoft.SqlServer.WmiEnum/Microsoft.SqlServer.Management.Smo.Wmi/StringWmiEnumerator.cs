using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

[CompilerGenerated]
internal class StringWmiEnumerator
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(StringWmiEnumerator));

		private static CultureInfo _culture = null;

		public const string MachineNameMustBeSpecified = "MachineNameMustBeSpecified";

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

	public static string MachineNameMustBeSpecified => Keys.GetString("MachineNameMustBeSpecified");

	protected StringWmiEnumerator()
	{
	}
}
