using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

[CompilerGenerated]
internal class ExceptionTemplates
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(ExceptionTemplates));

		private static CultureInfo _culture = null;

		public const string NoSuchCollection = "NoSuchCollection";

		public const string UnsupportedKey = "UnsupportedKey";

		public const string UnsupportedConnectionType = "UnsupportedConnectionType";

		public const string SqlExecutionFailure = "SqlExecutionFailure";

		public const string FileAlreadyExists = "FileAlreadyExists";

		public const string FileNotExist = "FileNotExist";

		public const string ChangeNameForExistingSession = "ChangeNameForExistingSession";

		public const string ChangePropertyForExistingEvent = "ChangePropertyForExistingEvent";

		public const string NewSessionMustContainEvent = "NewSessionMustContainEvent";

		public const string InvalidParameter = "InvalidParameter";

		public const string ParentNull = "ParentNull";

		public const string NameNullEmpty = "NameNullEmpty";

		public const string CannotSetNameForExistingObject = "CannotSetNameForExistingObject";

		public const string Expression2NotNull = "Expression2NotNull";

		public const string InvalidProperty = "InvalidProperty";

		public const string PackageNotExist = "PackageNotExist";

		public const string ObjectNotExist = "ObjectNotExist";

		public const string CannotAddActionToExistingEvent = "CannotAddActionToExistingEvent";

		public const string CannotChangeFieldForExistingEvent = "CannotChangeFieldForExistingEvent";

		public const string CannotChangeFieldForExistingTarget = "CannotChangeFieldForExistingTarget";

		public const string CannotSetEventInfoForExistingEvent = "CannotSetEventInfoForExistingEvent";

		public const string CannotSetTargetInfoForExistingTarget = "CannotSetTargetInfoForExistingTarget";

		public const string CannotSetActionInfoForExistingAction = "CannotSetActionInfoForExistingAction";

		public const string SaveTemplateForExistingSession = "SaveTemplateForExistingSession";

		public const string PackageNameNotUnique = "PackageNameNotUnique";

		public const string ObjectNameNotUnique = "ObjectNameNotUnique";

		public const string UnknownType = "UnknownType";

		public const string NoBlockingEventNotAllowedInNoEventLossSession = "NoBlockingEventNotAllowedInNoEventLossSession";

		public const string InvalidState = "InvalidState";

		public const string AlterValidationFailure = "AlterValidationFailure";

		public const string CannotReadTargetData = "CannotReadTargetData";

		public const string ActionNameNotUnique = "ActionNameNotUnique";

		public const string InvalidConnectionType = "InvalidConnectionType";

		public const string InvalidVersion = "InvalidVersion";

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

	public static string FileAlreadyExists => Keys.GetString("FileAlreadyExists");

	public static string FileNotExist => Keys.GetString("FileNotExist");

	public static string ChangeNameForExistingSession => Keys.GetString("ChangeNameForExistingSession");

	public static string NewSessionMustContainEvent => Keys.GetString("NewSessionMustContainEvent");

	public static string ParentNull => Keys.GetString("ParentNull");

	public static string NameNullEmpty => Keys.GetString("NameNullEmpty");

	public static string CannotSetNameForExistingObject => Keys.GetString("CannotSetNameForExistingObject");

	public static string Expression2NotNull => Keys.GetString("Expression2NotNull");

	public static string InvalidProperty => Keys.GetString("InvalidProperty");

	public static string CannotAddActionToExistingEvent => Keys.GetString("CannotAddActionToExistingEvent");

	public static string CannotChangeFieldForExistingEvent => Keys.GetString("CannotChangeFieldForExistingEvent");

	public static string CannotChangeFieldForExistingTarget => Keys.GetString("CannotChangeFieldForExistingTarget");

	public static string CannotSetEventInfoForExistingEvent => Keys.GetString("CannotSetEventInfoForExistingEvent");

	public static string CannotSetTargetInfoForExistingTarget => Keys.GetString("CannotSetTargetInfoForExistingTarget");

	public static string CannotSetActionInfoForExistingAction => Keys.GetString("CannotSetActionInfoForExistingAction");

	public static string SaveTemplateForExistingSession => Keys.GetString("SaveTemplateForExistingSession");

	public static string UnknownType => Keys.GetString("UnknownType");

	public static string AlterValidationFailure => Keys.GetString("AlterValidationFailure");

	public static string CannotReadTargetData => Keys.GetString("CannotReadTargetData");

	public static string InvalidConnectionType => Keys.GetString("InvalidConnectionType");

	protected ExceptionTemplates()
	{
	}

	public static string NoSuchCollection(string name)
	{
		return Keys.GetString("NoSuchCollection", name);
	}

	public static string UnsupportedKey(string key)
	{
		return Keys.GetString("UnsupportedKey", key);
	}

	public static string UnsupportedConnectionType(string type)
	{
		return Keys.GetString("UnsupportedConnectionType", type);
	}

	public static string SqlExecutionFailure(string sql)
	{
		return Keys.GetString("SqlExecutionFailure", sql);
	}

	public static string ChangePropertyForExistingEvent(string propertyName)
	{
		return Keys.GetString("ChangePropertyForExistingEvent", propertyName);
	}

	public static string InvalidParameter(string param)
	{
		return Keys.GetString("InvalidParameter", param);
	}

	public static string PackageNotExist(string name)
	{
		return Keys.GetString("PackageNotExist", name);
	}

	public static string ObjectNotExist(string name)
	{
		return Keys.GetString("ObjectNotExist", name);
	}

	public static string PackageNameNotUnique(string name)
	{
		return Keys.GetString("PackageNameNotUnique", name);
	}

	public static string ObjectNameNotUnique(string name)
	{
		return Keys.GetString("ObjectNameNotUnique", name);
	}

	public static string NoBlockingEventNotAllowedInNoEventLossSession(string sessionName, string eventName)
	{
		return Keys.GetString("NoBlockingEventNotAllowedInNoEventLossSession", sessionName, eventName);
	}

	public static string InvalidState(SfcObjectState current_state, SfcObjectState required_state)
	{
		return Keys.GetString("InvalidState", current_state, required_state);
	}

	public static string ActionNameNotUnique(string name)
	{
		return Keys.GetString("ActionNameNotUnique", name);
	}

	public static string InvalidVersion(string version)
	{
		return Keys.GetString("InvalidVersion", version);
	}
}
