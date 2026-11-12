using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Data.Tools.Sql.BatchParser;

[CompilerGenerated]
internal class SR
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(SR));

		private static CultureInfo _culture = null;

		public const string BatchParser_CircularReference = "BatchParser_CircularReference";

		public const string BatchParser_CommentNotTerminated = "BatchParser_CommentNotTerminated";

		public const string BatchParser_IncorrectSyntax = "BatchParser_IncorrectSyntax";

		public const string BatchParser_StringNotTerminated = "BatchParser_StringNotTerminated";

		public const string BatchParser_VariableNotDefined = "BatchParser_VariableNotDefined";

		public const string EE_ExecutionError_CommandNotSupported = "EE_ExecutionError_CommandNotSupported";

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

	public static string BatchParser_CircularReference => Keys.GetString("BatchParser_CircularReference");

	public static string BatchParser_CommentNotTerminated => Keys.GetString("BatchParser_CommentNotTerminated");

	public static string BatchParser_IncorrectSyntax => Keys.GetString("BatchParser_IncorrectSyntax");

	public static string BatchParser_StringNotTerminated => Keys.GetString("BatchParser_StringNotTerminated");

	public static string BatchParser_VariableNotDefined => Keys.GetString("BatchParser_VariableNotDefined");

	public static string EE_ExecutionError_CommandNotSupported => Keys.GetString("EE_ExecutionError_CommandNotSupported");

	protected SR()
	{
	}
}
