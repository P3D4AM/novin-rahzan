using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace Microsoft.Data.Common;

internal static class DbConnectionStringBuilderUtil
{
	private const string PoolBlockingPeriodAutoString = "Auto";

	private const string PoolBlockingPeriodAlwaysBlockString = "AlwaysBlock";

	private const string PoolBlockingPeriodNeverBlockString = "NeverBlock";

	private const string ApplicationIntentReadWriteString = "ReadWrite";

	private const string ApplicationIntentReadOnlyString = "ReadOnly";

	private const string SqlPasswordString = "Sql Password";

	private const string ActiveDirectoryPasswordString = "Active Directory Password";

	private const string ActiveDirectoryIntegratedString = "Active Directory Integrated";

	private const string ActiveDirectoryInteractiveString = "Active Directory Interactive";

	private const string ActiveDirectoryServicePrincipalString = "Active Directory Service Principal";

	private const string ActiveDirectoryDeviceCodeFlowString = "Active Directory Device Code Flow";

	internal const string ActiveDirectoryManagedIdentityString = "Active Directory Managed Identity";

	internal const string ActiveDirectoryMSIString = "Active Directory MSI";

	internal const string ActiveDirectoryDefaultString = "Active Directory Default";

	private const string ColumnEncryptionSettingEnabledString = "Enabled";

	private const string ColumnEncryptionSettingDisabledString = "Disabled";

	private const string AttestationProtocolHGS = "HGS";

	private const string AttestationProtocolAAS = "AAS";

	private static readonly Dictionary<string, SqlConnectionIPAddressPreference> s_preferenceNames;

	internal static bool ConvertToBoolean(object value)
	{
		if (value is string text)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(text, "true") || StringComparer.OrdinalIgnoreCase.Equals(text, "yes"))
			{
				return true;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(text, "false") || StringComparer.OrdinalIgnoreCase.Equals(text, "no"))
			{
				return false;
			}
			string x = text.Trim();
			if (StringComparer.OrdinalIgnoreCase.Equals(x, "true") || StringComparer.OrdinalIgnoreCase.Equals(x, "yes"))
			{
				return true;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(x, "false") || StringComparer.OrdinalIgnoreCase.Equals(x, "no"))
			{
				return false;
			}
			return bool.Parse(text);
		}
		try
		{
			return ((IConvertible)value).ToBoolean(CultureInfo.InvariantCulture);
		}
		catch (InvalidCastException innerException)
		{
			throw ADP.ConvertFailed(value.GetType(), typeof(bool), innerException);
		}
	}

	internal static bool ConvertToIntegratedSecurity(object value)
	{
		if (value is string text)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(text, "sspi") || StringComparer.OrdinalIgnoreCase.Equals(text, "true") || StringComparer.OrdinalIgnoreCase.Equals(text, "yes"))
			{
				return true;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(text, "false") || StringComparer.OrdinalIgnoreCase.Equals(text, "no"))
			{
				return false;
			}
			string x = text.Trim();
			if (StringComparer.OrdinalIgnoreCase.Equals(x, "sspi") || StringComparer.OrdinalIgnoreCase.Equals(x, "true") || StringComparer.OrdinalIgnoreCase.Equals(x, "yes"))
			{
				return true;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(x, "false") || StringComparer.OrdinalIgnoreCase.Equals(x, "no"))
			{
				return false;
			}
			return bool.Parse(text);
		}
		try
		{
			return ((IConvertible)value).ToBoolean(CultureInfo.InvariantCulture);
		}
		catch (InvalidCastException innerException)
		{
			throw ADP.ConvertFailed(value.GetType(), typeof(bool), innerException);
		}
	}

	internal static int ConvertToInt32(object value)
	{
		try
		{
			return ((IConvertible)value).ToInt32(CultureInfo.InvariantCulture);
		}
		catch (InvalidCastException innerException)
		{
			throw ADP.ConvertFailed(value.GetType(), typeof(int), innerException);
		}
	}

	internal static string ConvertToString(object value)
	{
		try
		{
			return ((IConvertible)value).ToString(CultureInfo.InvariantCulture);
		}
		catch (InvalidCastException innerException)
		{
			throw ADP.ConvertFailed(value.GetType(), typeof(string), innerException);
		}
	}

	internal static bool TryConvertToPoolBlockingPeriod(string value, out PoolBlockingPeriod result)
	{
		if (StringComparer.OrdinalIgnoreCase.Equals(value, "Auto"))
		{
			result = PoolBlockingPeriod.Auto;
			return true;
		}
		if (StringComparer.OrdinalIgnoreCase.Equals(value, "AlwaysBlock"))
		{
			result = PoolBlockingPeriod.AlwaysBlock;
			return true;
		}
		if (StringComparer.OrdinalIgnoreCase.Equals(value, "NeverBlock"))
		{
			result = PoolBlockingPeriod.NeverBlock;
			return true;
		}
		result = PoolBlockingPeriod.Auto;
		return false;
	}

	internal static bool IsValidPoolBlockingPeriodValue(PoolBlockingPeriod value)
	{
		if (value != 0 && value != PoolBlockingPeriod.AlwaysBlock)
		{
			return value == PoolBlockingPeriod.NeverBlock;
		}
		return true;
	}

	internal static string PoolBlockingPeriodToString(PoolBlockingPeriod value)
	{
		return value switch
		{
			PoolBlockingPeriod.AlwaysBlock => "AlwaysBlock", 
			PoolBlockingPeriod.NeverBlock => "NeverBlock", 
			_ => "Auto", 
		};
	}

	internal static PoolBlockingPeriod ConvertToPoolBlockingPeriod(string keyword, object value)
	{
		if (value is string text)
		{
			if (TryConvertToPoolBlockingPeriod(text, out var result))
			{
				return result;
			}
			string value2 = text.Trim();
			if (TryConvertToPoolBlockingPeriod(value2, out result))
			{
				return result;
			}
			throw ADP.InvalidConnectionOptionValue(keyword);
		}
		PoolBlockingPeriod poolBlockingPeriod;
		if (value is PoolBlockingPeriod)
		{
			poolBlockingPeriod = (PoolBlockingPeriod)value;
		}
		else
		{
			if (value.GetType().IsEnum)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(PoolBlockingPeriod), null);
			}
			try
			{
				poolBlockingPeriod = (PoolBlockingPeriod)Enum.ToObject(typeof(PoolBlockingPeriod), value);
			}
			catch (ArgumentException innerException)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(PoolBlockingPeriod), innerException);
			}
		}
		if (IsValidPoolBlockingPeriodValue(poolBlockingPeriod))
		{
			return poolBlockingPeriod;
		}
		throw ADP.InvalidEnumerationValue(typeof(ApplicationIntent), (int)poolBlockingPeriod);
	}

	internal static bool TryConvertToApplicationIntent(string value, out ApplicationIntent result)
	{
		if (StringComparer.OrdinalIgnoreCase.Equals(value, "ReadOnly"))
		{
			result = ApplicationIntent.ReadOnly;
			return true;
		}
		if (StringComparer.OrdinalIgnoreCase.Equals(value, "ReadWrite"))
		{
			result = ApplicationIntent.ReadWrite;
			return true;
		}
		result = ApplicationIntent.ReadWrite;
		return false;
	}

	internal static bool IsValidApplicationIntentValue(ApplicationIntent value)
	{
		if (value != ApplicationIntent.ReadOnly)
		{
			return value == ApplicationIntent.ReadWrite;
		}
		return true;
	}

	internal static string ApplicationIntentToString(ApplicationIntent value)
	{
		if (value == ApplicationIntent.ReadOnly)
		{
			return "ReadOnly";
		}
		return "ReadWrite";
	}

	internal static ApplicationIntent ConvertToApplicationIntent(string keyword, object value)
	{
		if (value is string text)
		{
			if (TryConvertToApplicationIntent(text, out var result))
			{
				return result;
			}
			string value2 = text.Trim();
			if (TryConvertToApplicationIntent(value2, out result))
			{
				return result;
			}
			throw ADP.InvalidConnectionOptionValue(keyword);
		}
		ApplicationIntent applicationIntent;
		if (value is ApplicationIntent)
		{
			applicationIntent = (ApplicationIntent)value;
		}
		else
		{
			if (value.GetType().IsEnum)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(ApplicationIntent), null);
			}
			try
			{
				applicationIntent = (ApplicationIntent)Enum.ToObject(typeof(ApplicationIntent), value);
			}
			catch (ArgumentException innerException)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(ApplicationIntent), innerException);
			}
		}
		if (IsValidApplicationIntentValue(applicationIntent))
		{
			return applicationIntent;
		}
		throw ADP.InvalidEnumerationValue(typeof(ApplicationIntent), (int)applicationIntent);
	}

	internal static bool TryConvertToAuthenticationType(string value, out SqlAuthenticationMethod result)
	{
		bool result2 = false;
		if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "Sql Password") || StringComparer.InvariantCultureIgnoreCase.Equals(value, Convert.ToString(SqlAuthenticationMethod.SqlPassword, CultureInfo.InvariantCulture)))
		{
			result = SqlAuthenticationMethod.SqlPassword;
			result2 = true;
		}
		else if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "Active Directory Password") || StringComparer.InvariantCultureIgnoreCase.Equals(value, Convert.ToString(SqlAuthenticationMethod.ActiveDirectoryPassword, CultureInfo.InvariantCulture)))
		{
			result = SqlAuthenticationMethod.ActiveDirectoryPassword;
			result2 = true;
		}
		else if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "Active Directory Integrated") || StringComparer.InvariantCultureIgnoreCase.Equals(value, Convert.ToString(SqlAuthenticationMethod.ActiveDirectoryIntegrated, CultureInfo.InvariantCulture)))
		{
			result = SqlAuthenticationMethod.ActiveDirectoryIntegrated;
			result2 = true;
		}
		else if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "Active Directory Interactive") || StringComparer.InvariantCultureIgnoreCase.Equals(value, Convert.ToString(SqlAuthenticationMethod.ActiveDirectoryInteractive, CultureInfo.InvariantCulture)))
		{
			result = SqlAuthenticationMethod.ActiveDirectoryInteractive;
			result2 = true;
		}
		else if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "Active Directory Service Principal") || StringComparer.InvariantCultureIgnoreCase.Equals(value, Convert.ToString(SqlAuthenticationMethod.ActiveDirectoryServicePrincipal, CultureInfo.InvariantCulture)))
		{
			result = SqlAuthenticationMethod.ActiveDirectoryServicePrincipal;
			result2 = true;
		}
		else if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "Active Directory Device Code Flow") || StringComparer.InvariantCultureIgnoreCase.Equals(value, Convert.ToString(SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow, CultureInfo.InvariantCulture)))
		{
			result = SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow;
			result2 = true;
		}
		else if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "Active Directory Managed Identity") || StringComparer.InvariantCultureIgnoreCase.Equals(value, Convert.ToString(SqlAuthenticationMethod.ActiveDirectoryManagedIdentity, CultureInfo.InvariantCulture)))
		{
			result = SqlAuthenticationMethod.ActiveDirectoryManagedIdentity;
			result2 = true;
		}
		else if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "Active Directory MSI") || StringComparer.InvariantCultureIgnoreCase.Equals(value, Convert.ToString(SqlAuthenticationMethod.ActiveDirectoryMSI, CultureInfo.InvariantCulture)))
		{
			result = SqlAuthenticationMethod.ActiveDirectoryMSI;
			result2 = true;
		}
		else if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "Active Directory Default") || StringComparer.InvariantCultureIgnoreCase.Equals(value, Convert.ToString(SqlAuthenticationMethod.ActiveDirectoryDefault, CultureInfo.InvariantCulture)))
		{
			result = SqlAuthenticationMethod.ActiveDirectoryDefault;
			result2 = true;
		}
		else
		{
			result = DbConnectionStringDefaults.Authentication;
		}
		return result2;
	}

	internal static bool TryConvertToColumnEncryptionSetting(string value, out SqlConnectionColumnEncryptionSetting result)
	{
		bool result2 = false;
		if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "Enabled"))
		{
			result = SqlConnectionColumnEncryptionSetting.Enabled;
			result2 = true;
		}
		else if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "Disabled"))
		{
			result = SqlConnectionColumnEncryptionSetting.Disabled;
			result2 = true;
		}
		else
		{
			result = DbConnectionStringDefaults.ColumnEncryptionSetting;
		}
		return result2;
	}

	internal static bool IsValidColumnEncryptionSetting(SqlConnectionColumnEncryptionSetting value)
	{
		if (value != SqlConnectionColumnEncryptionSetting.Enabled)
		{
			return value == SqlConnectionColumnEncryptionSetting.Disabled;
		}
		return true;
	}

	internal static string ColumnEncryptionSettingToString(SqlConnectionColumnEncryptionSetting value)
	{
		return value switch
		{
			SqlConnectionColumnEncryptionSetting.Enabled => "Enabled", 
			SqlConnectionColumnEncryptionSetting.Disabled => "Disabled", 
			_ => null, 
		};
	}

	internal static bool IsValidAuthenticationTypeValue(SqlAuthenticationMethod value)
	{
		if (value != SqlAuthenticationMethod.SqlPassword && value != SqlAuthenticationMethod.ActiveDirectoryPassword && value != SqlAuthenticationMethod.ActiveDirectoryIntegrated && value != SqlAuthenticationMethod.ActiveDirectoryInteractive && value != SqlAuthenticationMethod.ActiveDirectoryServicePrincipal && value != SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow && value != SqlAuthenticationMethod.ActiveDirectoryManagedIdentity && value != SqlAuthenticationMethod.ActiveDirectoryMSI && value != SqlAuthenticationMethod.ActiveDirectoryDefault)
		{
			return value == SqlAuthenticationMethod.NotSpecified;
		}
		return true;
	}

	internal static string AuthenticationTypeToString(SqlAuthenticationMethod value)
	{
		return value switch
		{
			SqlAuthenticationMethod.SqlPassword => "Sql Password", 
			SqlAuthenticationMethod.ActiveDirectoryPassword => "Active Directory Password", 
			SqlAuthenticationMethod.ActiveDirectoryIntegrated => "Active Directory Integrated", 
			SqlAuthenticationMethod.ActiveDirectoryInteractive => "Active Directory Interactive", 
			SqlAuthenticationMethod.ActiveDirectoryServicePrincipal => "Active Directory Service Principal", 
			SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow => "Active Directory Device Code Flow", 
			SqlAuthenticationMethod.ActiveDirectoryManagedIdentity => "Active Directory Managed Identity", 
			SqlAuthenticationMethod.ActiveDirectoryMSI => "Active Directory MSI", 
			SqlAuthenticationMethod.ActiveDirectoryDefault => "Active Directory Default", 
			_ => null, 
		};
	}

	internal static SqlAuthenticationMethod ConvertToAuthenticationType(string keyword, object value)
	{
		if (value == null)
		{
			return DbConnectionStringDefaults.Authentication;
		}
		if (value is string text)
		{
			if (TryConvertToAuthenticationType(text, out var result))
			{
				return result;
			}
			string value2 = text.Trim();
			if (TryConvertToAuthenticationType(value2, out result))
			{
				return result;
			}
			throw ADP.InvalidConnectionOptionValue(keyword);
		}
		SqlAuthenticationMethod sqlAuthenticationMethod;
		if (value is SqlAuthenticationMethod)
		{
			sqlAuthenticationMethod = (SqlAuthenticationMethod)value;
		}
		else
		{
			if (value.GetType().IsEnum)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(SqlAuthenticationMethod), null);
			}
			try
			{
				sqlAuthenticationMethod = (SqlAuthenticationMethod)Enum.ToObject(typeof(SqlAuthenticationMethod), value);
			}
			catch (ArgumentException innerException)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(SqlAuthenticationMethod), innerException);
			}
		}
		if (IsValidAuthenticationTypeValue(sqlAuthenticationMethod))
		{
			return sqlAuthenticationMethod;
		}
		throw ADP.InvalidEnumerationValue(typeof(SqlAuthenticationMethod), (int)sqlAuthenticationMethod);
	}

	internal static SqlConnectionColumnEncryptionSetting ConvertToColumnEncryptionSetting(string keyword, object value)
	{
		if (value == null)
		{
			return DbConnectionStringDefaults.ColumnEncryptionSetting;
		}
		if (value is string text)
		{
			if (TryConvertToColumnEncryptionSetting(text, out var result))
			{
				return result;
			}
			string value2 = text.Trim();
			if (TryConvertToColumnEncryptionSetting(value2, out result))
			{
				return result;
			}
			throw ADP.InvalidConnectionOptionValue(keyword);
		}
		SqlConnectionColumnEncryptionSetting sqlConnectionColumnEncryptionSetting;
		if (value is SqlConnectionColumnEncryptionSetting)
		{
			sqlConnectionColumnEncryptionSetting = (SqlConnectionColumnEncryptionSetting)value;
		}
		else
		{
			if (value.GetType().IsEnum)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(SqlConnectionColumnEncryptionSetting), null);
			}
			try
			{
				sqlConnectionColumnEncryptionSetting = (SqlConnectionColumnEncryptionSetting)Enum.ToObject(typeof(SqlConnectionColumnEncryptionSetting), value);
			}
			catch (ArgumentException innerException)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(SqlConnectionColumnEncryptionSetting), innerException);
			}
		}
		if (IsValidColumnEncryptionSetting(sqlConnectionColumnEncryptionSetting))
		{
			return sqlConnectionColumnEncryptionSetting;
		}
		throw ADP.InvalidEnumerationValue(typeof(SqlConnectionColumnEncryptionSetting), (int)sqlConnectionColumnEncryptionSetting);
	}

	internal static bool TryConvertToAttestationProtocol(string value, out SqlConnectionAttestationProtocol result)
	{
		if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "HGS"))
		{
			result = SqlConnectionAttestationProtocol.HGS;
			return true;
		}
		if (StringComparer.InvariantCultureIgnoreCase.Equals(value, "AAS"))
		{
			result = SqlConnectionAttestationProtocol.AAS;
			return true;
		}
		result = SqlConnectionAttestationProtocol.NotSpecified;
		return false;
	}

	internal static bool IsValidAttestationProtocol(SqlConnectionAttestationProtocol value)
	{
		if (value != 0 && value != SqlConnectionAttestationProtocol.HGS)
		{
			return value == SqlConnectionAttestationProtocol.AAS;
		}
		return true;
	}

	internal static string AttestationProtocolToString(SqlConnectionAttestationProtocol value)
	{
		return value switch
		{
			SqlConnectionAttestationProtocol.HGS => "HGS", 
			SqlConnectionAttestationProtocol.AAS => "AAS", 
			_ => null, 
		};
	}

	internal static SqlConnectionAttestationProtocol ConvertToAttestationProtocol(string keyword, object value)
	{
		if (value == null)
		{
			return SqlConnectionAttestationProtocol.NotSpecified;
		}
		if (value is string text)
		{
			string value2 = text.Trim();
			if (TryConvertToAttestationProtocol(value2, out var result))
			{
				return result;
			}
			throw ADP.InvalidConnectionOptionValue(keyword);
		}
		SqlConnectionAttestationProtocol sqlConnectionAttestationProtocol;
		if (value is SqlConnectionAttestationProtocol)
		{
			sqlConnectionAttestationProtocol = (SqlConnectionAttestationProtocol)value;
		}
		else
		{
			if (value.GetType().IsEnum)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(SqlConnectionAttestationProtocol), null);
			}
			try
			{
				sqlConnectionAttestationProtocol = (SqlConnectionAttestationProtocol)Enum.ToObject(typeof(SqlConnectionAttestationProtocol), value);
			}
			catch (ArgumentException innerException)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(SqlConnectionAttestationProtocol), innerException);
			}
		}
		if (IsValidAttestationProtocol(sqlConnectionAttestationProtocol))
		{
			return sqlConnectionAttestationProtocol;
		}
		throw ADP.InvalidEnumerationValue(typeof(SqlConnectionAttestationProtocol), (int)sqlConnectionAttestationProtocol);
	}

	static DbConnectionStringBuilderUtil()
	{
		s_preferenceNames = new Dictionary<string, SqlConnectionIPAddressPreference>(StringComparer.InvariantCultureIgnoreCase);
		foreach (SqlConnectionIPAddressPreference value in Enum.GetValues(typeof(SqlConnectionIPAddressPreference)))
		{
			s_preferenceNames.Add(value.ToString(), value);
		}
	}

	internal static bool TryConvertToIPAddressPreference(string value, out SqlConnectionIPAddressPreference result)
	{
		if (!s_preferenceNames.TryGetValue(value, out result))
		{
			result = SqlConnectionIPAddressPreference.IPv4First;
			return false;
		}
		return true;
	}

	internal static bool IsValidIPAddressPreference(SqlConnectionIPAddressPreference value)
	{
		if (value != 0 && value != SqlConnectionIPAddressPreference.IPv6First)
		{
			return value == SqlConnectionIPAddressPreference.UsePlatformDefault;
		}
		return true;
	}

	internal static string IPAddressPreferenceToString(SqlConnectionIPAddressPreference value)
	{
		return Enum.GetName(typeof(SqlConnectionIPAddressPreference), value);
	}

	internal static SqlConnectionIPAddressPreference ConvertToIPAddressPreference(string keyword, object value)
	{
		if (value == null)
		{
			return SqlConnectionIPAddressPreference.IPv4First;
		}
		if (value is string text)
		{
			string value2 = text.Trim();
			if (TryConvertToIPAddressPreference(value2, out var result))
			{
				return result;
			}
			throw ADP.InvalidConnectionOptionValue(keyword);
		}
		SqlConnectionIPAddressPreference sqlConnectionIPAddressPreference2;
		if (value is SqlConnectionIPAddressPreference sqlConnectionIPAddressPreference)
		{
			sqlConnectionIPAddressPreference2 = sqlConnectionIPAddressPreference;
		}
		else
		{
			if (value.GetType().IsEnum)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(SqlConnectionIPAddressPreference), null);
			}
			try
			{
				sqlConnectionIPAddressPreference2 = (SqlConnectionIPAddressPreference)Enum.ToObject(typeof(SqlConnectionIPAddressPreference), value);
			}
			catch (ArgumentException innerException)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(SqlConnectionIPAddressPreference), innerException);
			}
		}
		if (IsValidIPAddressPreference(sqlConnectionIPAddressPreference2))
		{
			return sqlConnectionIPAddressPreference2;
		}
		throw ADP.InvalidEnumerationValue(typeof(SqlConnectionIPAddressPreference), (int)sqlConnectionIPAddressPreference2);
	}

	internal static bool IsValidCertificateValue(string value)
	{
		if (!string.IsNullOrEmpty(value) && !value.StartsWith("subject:", StringComparison.OrdinalIgnoreCase))
		{
			return value.StartsWith("sha1:", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}
}
