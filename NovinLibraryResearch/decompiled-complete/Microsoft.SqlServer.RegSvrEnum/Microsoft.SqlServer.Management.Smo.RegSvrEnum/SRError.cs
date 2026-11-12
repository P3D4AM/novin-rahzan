using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

[CompilerGenerated]
internal class SRError
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(SRError));

		private static CultureInfo _culture = null;

		public const string NotSaveRegisteredServers = "NotSaveRegisteredServers";

		public const string RecursiveMove = "RecursiveMove";

		public const string ConnectionInfoTypeIncorrect = "ConnectionInfoTypeIncorrect";

		public const string ServerTypeNotSpecified = "ServerTypeNotSpecified";

		public const string ServerTypeFilterInvalid = "ServerTypeFilterInvalid";

		public const string ServerTypeMustBeGuid = "ServerTypeMustBeGuid";

		public const string InvalidParentResultType = "InvalidParentResultType";

		public const string ArrayDimensionInvalid = "ArrayDimensionInvalid";

		public const string ArraySizeError = "ArraySizeError";

		public const string PathNotSupported = "PathNotSupported";

		public const string DirectoryNotFound = "DirectoryNotFound";

		public const string AccessDenied = "AccessDenied";

		public const string ErrCannotHaveSiblingsWithSameName = "ErrCannotHaveSiblingsWithSameName";

		public const string ErrCannotMoveToSelf = "ErrCannotMoveToSelf";

		public const string ErrUnableToExport = "ErrUnableToExport";

		public const string SqlServerTypeNotFound = "SqlServerTypeNotFound";

		public const string PathFilterInvalid = "PathFilterInvalid";

		public const string RegSvrDatafileInvalid = "RegSvrDatafileInvalid";

		public const string UniqueRegisteredNamePattern = "UniqueRegisteredNamePattern";

		public const string CryptProtectDataWin32Error = "CryptProtectDataWin32Error";

		public const string CryptUnprotectDataWin32Error = "CryptUnprotectDataWin32Error";

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

		public static string GetString(string key, object arg0)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0);
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

	public static string NotSaveRegisteredServers => Keys.GetString("NotSaveRegisteredServers");

	public static string RecursiveMove => Keys.GetString("RecursiveMove");

	public static string ConnectionInfoTypeIncorrect => Keys.GetString("ConnectionInfoTypeIncorrect");

	public static string ServerTypeNotSpecified => Keys.GetString("ServerTypeNotSpecified");

	public static string ServerTypeFilterInvalid => Keys.GetString("ServerTypeFilterInvalid");

	public static string ServerTypeMustBeGuid => Keys.GetString("ServerTypeMustBeGuid");

	public static string InvalidParentResultType => Keys.GetString("InvalidParentResultType");

	public static string ArrayDimensionInvalid => Keys.GetString("ArrayDimensionInvalid");

	public static string ArraySizeError => Keys.GetString("ArraySizeError");

	public static string ErrCannotHaveSiblingsWithSameName => Keys.GetString("ErrCannotHaveSiblingsWithSameName");

	public static string ErrCannotMoveToSelf => Keys.GetString("ErrCannotMoveToSelf");

	protected SRError()
	{
	}

	public static string PathNotSupported(string path)
	{
		return Keys.GetString("PathNotSupported", path);
	}

	public static string DirectoryNotFound(string directory)
	{
		return Keys.GetString("DirectoryNotFound", directory);
	}

	public static string AccessDenied(string path)
	{
		return Keys.GetString("AccessDenied", path);
	}

	public static string ErrUnableToExport(string fileName)
	{
		return Keys.GetString("ErrUnableToExport", fileName);
	}

	public static string SqlServerTypeNotFound(string typeGuid)
	{
		return Keys.GetString("SqlServerTypeNotFound", typeGuid);
	}

	public static string PathFilterInvalid(string pathNode)
	{
		return Keys.GetString("PathFilterInvalid", pathNode);
	}

	public static string RegSvrDatafileInvalid(string filePath)
	{
		return Keys.GetString("RegSvrDatafileInvalid", filePath);
	}

	public static string UniqueRegisteredNamePattern(int copyNum, string oldName)
	{
		return Keys.GetString("UniqueRegisteredNamePattern", copyNum, oldName);
	}

	public static string CryptProtectDataWin32Error(int err)
	{
		return Keys.GetString("CryptProtectDataWin32Error", err);
	}

	public static string CryptUnprotectDataWin32Error(int err)
	{
		return Keys.GetString("CryptUnprotectDataWin32Error", err);
	}
}
