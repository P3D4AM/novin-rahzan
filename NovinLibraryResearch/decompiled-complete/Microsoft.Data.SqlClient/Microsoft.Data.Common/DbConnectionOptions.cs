using System;
using System.Collections;
using System.Globalization;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Data.Common;

internal class DbConnectionOptions
{
	private static class KEY
	{
		internal const string Integrated_Security = "integrated security";

		internal const string Password = "password";

		internal const string Persist_Security_Info = "persist security info";

		internal const string User_ID = "user id";
	}

	private static class SYNONYM
	{
		internal const string Pwd = "pwd";

		internal const string UID = "uid";
	}

	private enum ParserState
	{
		NothingYet = 1,
		Key,
		KeyEqual,
		KeyEnd,
		UnquotedValue,
		DoubleQuoteValue,
		DoubleQuoteValueQuote,
		SingleQuoteValue,
		SingleQuoteValueQuote,
		BraceQuoteValue,
		BraceQuoteValueQuote,
		QuotedValueEnd,
		NullTermination
	}

	private const string ConnectionStringValidKeyPattern = "^(?![;\\s])[^\\p{Cc}]+(?<!\\s)$";

	private const string ConnectionStringValidValuePattern = "^[^\0]*$";

	private const string ConnectionStringQuoteValuePattern = "^[^\"'=;\\s\\p{Cc}]*$";

	private const string ConnectionStringQuoteOdbcValuePattern = "^\\{([^\\}\0]|\\}\\})*\\}$";

	internal const string DataDirectory = "|datadirectory|";

	private static readonly Regex ConnectionStringValidKeyRegex = new Regex("^(?![;\\s])[^\\p{Cc}]+(?<!\\s)$", RegexOptions.Compiled);

	private static readonly Regex ConnectionStringValidValueRegex = new Regex("^[^\0]*$", RegexOptions.Compiled);

	private static readonly Regex ConnectionStringQuoteValueRegex = new Regex("^[^\"'=;\\s\\p{Cc}]*$", RegexOptions.Compiled);

	private static readonly Regex ConnectionStringQuoteOdbcValueRegex = new Regex("^\\{([^\\}\0]|\\}\\})*\\}$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

	private readonly string _usersConnectionString;

	private readonly Hashtable _parsetable;

	internal readonly NameValuePair KeyChain;

	internal readonly bool HasPasswordKeyword;

	internal readonly bool HasUserIdKeyword;

	internal readonly bool UseOdbcRules;

	private PermissionSet _permissionset;

	internal bool HasBlankPassword
	{
		get
		{
			if (!ConvertValueToIntegratedSecurity())
			{
				if (_parsetable.ContainsKey("password"))
				{
					return ADP.IsEmpty((string)_parsetable["password"]);
				}
				if (_parsetable.ContainsKey("pwd"))
				{
					return ADP.IsEmpty((string)_parsetable["pwd"]);
				}
				if (!_parsetable.ContainsKey("user id") || ADP.IsEmpty((string)_parsetable["user id"]))
				{
					if (_parsetable.ContainsKey("uid"))
					{
						return !ADP.IsEmpty((string)_parsetable["uid"]);
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	internal bool HasPersistablePassword
	{
		get
		{
			if (HasPasswordKeyword)
			{
				return ConvertValueToBoolean("persist security info", defaultValue: false);
			}
			return true;
		}
	}

	public bool IsEmpty => KeyChain == null;

	internal Hashtable Parsetable => _parsetable;

	public ICollection Keys => _parsetable.Keys;

	public string this[string keyword] => (string)_parsetable[keyword];

	public DbConnectionOptions(string connectionString)
		: this(connectionString, null, useOdbcRules: false)
	{
	}

	public DbConnectionOptions(string connectionString, Hashtable synonyms, bool useOdbcRules)
	{
		UseOdbcRules = useOdbcRules;
		_parsetable = new Hashtable();
		_usersConnectionString = ((connectionString != null) ? connectionString : "");
		if (0 < _usersConnectionString.Length)
		{
			KeyChain = ParseInternal(_parsetable, _usersConnectionString, buildChain: true, synonyms, UseOdbcRules);
			HasPasswordKeyword = _parsetable.ContainsKey("password") || _parsetable.ContainsKey("pwd");
			HasUserIdKeyword = _parsetable.ContainsKey("user id") || _parsetable.ContainsKey("uid");
		}
	}

	protected DbConnectionOptions(DbConnectionOptions connectionOptions)
	{
		_usersConnectionString = connectionOptions._usersConnectionString;
		HasPasswordKeyword = connectionOptions.HasPasswordKeyword;
		HasUserIdKeyword = connectionOptions.HasUserIdKeyword;
		UseOdbcRules = connectionOptions.UseOdbcRules;
		_parsetable = connectionOptions._parsetable;
		KeyChain = connectionOptions.KeyChain;
	}

	public string UsersConnectionString(bool hidePassword)
	{
		return UsersConnectionString(hidePassword, forceHidePassword: false);
	}

	private string UsersConnectionString(bool hidePassword, bool forceHidePassword)
	{
		string constr = _usersConnectionString;
		if (HasPasswordKeyword && (forceHidePassword || (hidePassword && !HasPersistablePassword)))
		{
			ReplacePasswordPwd(out constr, fakePassword: false);
		}
		if (constr == null)
		{
			return "";
		}
		return constr;
	}

	internal string UsersConnectionStringForTrace()
	{
		return UsersConnectionString(hidePassword: true, forceHidePassword: true);
	}

	internal static void AppendKeyValuePairBuilder(StringBuilder builder, string keyName, string keyValue, bool useOdbcRules)
	{
		ADP.CheckArgumentNull(builder, "builder");
		ADP.CheckArgumentLength(keyName, "keyName");
		if (keyName == null || !ConnectionStringValidKeyRegex.IsMatch(keyName))
		{
			throw ADP.InvalidKeyname(keyName);
		}
		if (keyValue != null && !IsValueValidInternal(keyValue))
		{
			throw ADP.InvalidValue(keyName);
		}
		if (0 < builder.Length && ';' != builder[builder.Length - 1])
		{
			builder.Append(";");
		}
		if (useOdbcRules)
		{
			builder.Append(keyName);
		}
		else
		{
			builder.Append(keyName.Replace("=", "=="));
		}
		builder.Append("=");
		if (keyValue == null)
		{
			return;
		}
		if (useOdbcRules)
		{
			if (0 < keyValue.Length && ('{' == keyValue[0] || 0 <= keyValue.IndexOf(';') || string.Compare("Driver", keyName, StringComparison.OrdinalIgnoreCase) == 0) && !ConnectionStringQuoteOdbcValueRegex.IsMatch(keyValue))
			{
				builder.Append('{').Append(keyValue.Replace("}", "}}")).Append('}');
			}
			else
			{
				builder.Append(keyValue);
			}
		}
		else if (ConnectionStringQuoteValueRegex.IsMatch(keyValue))
		{
			builder.Append(keyValue);
		}
		else if (-1 != keyValue.IndexOf('"') && -1 == keyValue.IndexOf('\''))
		{
			builder.Append('\'');
			builder.Append(keyValue);
			builder.Append('\'');
		}
		else
		{
			builder.Append('"');
			builder.Append(keyValue.Replace("\"", "\"\""));
			builder.Append('"');
		}
	}

	public bool ConvertValueToBoolean(string keyName, bool defaultValue)
	{
		object obj = _parsetable[keyName];
		if (obj == null)
		{
			return defaultValue;
		}
		return ConvertValueToBooleanInternal(keyName, (string)obj);
	}

	internal static bool ConvertValueToBooleanInternal(string keyName, string stringValue)
	{
		if (CompareInsensitiveInvariant(stringValue, "true") || CompareInsensitiveInvariant(stringValue, "yes"))
		{
			return true;
		}
		if (CompareInsensitiveInvariant(stringValue, "false") || CompareInsensitiveInvariant(stringValue, "no"))
		{
			return false;
		}
		string strvalue = stringValue.Trim();
		if (CompareInsensitiveInvariant(strvalue, "true") || CompareInsensitiveInvariant(strvalue, "yes"))
		{
			return true;
		}
		if (CompareInsensitiveInvariant(strvalue, "false") || CompareInsensitiveInvariant(strvalue, "no"))
		{
			return false;
		}
		throw ADP.InvalidConnectionOptionValue(keyName);
	}

	public bool ConvertValueToIntegratedSecurity()
	{
		object obj = _parsetable["integrated security"];
		if (obj == null)
		{
			return false;
		}
		return ConvertValueToIntegratedSecurityInternal((string)obj);
	}

	internal bool ConvertValueToIntegratedSecurityInternal(string stringValue)
	{
		if (CompareInsensitiveInvariant(stringValue, "sspi") || CompareInsensitiveInvariant(stringValue, "true") || CompareInsensitiveInvariant(stringValue, "yes"))
		{
			return true;
		}
		if (CompareInsensitiveInvariant(stringValue, "false") || CompareInsensitiveInvariant(stringValue, "no"))
		{
			return false;
		}
		string strvalue = stringValue.Trim();
		if (CompareInsensitiveInvariant(strvalue, "sspi") || CompareInsensitiveInvariant(strvalue, "true") || CompareInsensitiveInvariant(strvalue, "yes"))
		{
			return true;
		}
		if (CompareInsensitiveInvariant(strvalue, "false") || CompareInsensitiveInvariant(strvalue, "no"))
		{
			return false;
		}
		throw ADP.InvalidConnectionOptionValue("integrated security");
	}

	public int ConvertValueToInt32(string keyName, int defaultValue)
	{
		object obj = _parsetable[keyName];
		if (obj == null)
		{
			return defaultValue;
		}
		return ConvertToInt32Internal(keyName, (string)obj);
	}

	internal static int ConvertToInt32Internal(string keyname, string stringValue)
	{
		try
		{
			return int.Parse(stringValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
		}
		catch (FormatException inner)
		{
			throw ADP.InvalidConnectionOptionValue(keyname, inner);
		}
		catch (OverflowException inner2)
		{
			throw ADP.InvalidConnectionOptionValue(keyname, inner2);
		}
	}

	public string ConvertValueToString(string keyName, string defaultValue)
	{
		string text = (string)_parsetable[keyName];
		if (text == null)
		{
			return defaultValue;
		}
		return text;
	}

	private static bool CompareInsensitiveInvariant(string strvalue, string strconst)
	{
		return StringComparer.OrdinalIgnoreCase.Compare(strvalue, strconst) == 0;
	}

	public bool ContainsKey(string keyword)
	{
		return _parsetable.ContainsKey(keyword);
	}

	protected internal virtual PermissionSet CreatePermissionSet()
	{
		return null;
	}

	internal void DemandPermission()
	{
		if (_permissionset == null)
		{
			_permissionset = CreatePermissionSet();
		}
		_permissionset.Demand();
	}

	protected internal virtual string Expand()
	{
		return _usersConnectionString;
	}

	internal static string ExpandDataDirectory(string keyword, string value, ref string datadir)
	{
		string text = null;
		if (value != null && value.StartsWith("|datadirectory|", StringComparison.OrdinalIgnoreCase))
		{
			string text2 = datadir;
			if (text2 == null)
			{
				object data = AppDomain.CurrentDomain.GetData("DataDirectory");
				text2 = data as string;
				if (data != null && text2 == null)
				{
					throw ADP.InvalidDataDirectory();
				}
				if (ADP.IsEmpty(text2))
				{
					text2 = AppDomain.CurrentDomain.BaseDirectory;
				}
				if (text2 == null)
				{
					text2 = "";
				}
				datadir = text2;
			}
			int length = "|datadirectory|".Length;
			bool flag = 0 < text2.Length && text2[text2.Length - 1] == '\\';
			bool flag2 = length < value.Length && value[length] == '\\';
			text = ((!flag && !flag2) ? (text2 + "\\" + value.Substring(length)) : ((!(flag && flag2)) ? (text2 + value.Substring(length)) : (text2 + value.Substring(length + 1))));
			if (!ADP.GetFullPath(text).StartsWith(text2, StringComparison.Ordinal))
			{
				throw ADP.InvalidConnectionOptionValue(keyword);
			}
		}
		return text;
	}

	internal string ExpandDataDirectories(ref string filename, ref int position)
	{
		string text = null;
		StringBuilder stringBuilder = new StringBuilder(_usersConnectionString.Length);
		string datadir = null;
		int num = 0;
		bool flag = false;
		for (NameValuePair nameValuePair = KeyChain; nameValuePair != null; nameValuePair = nameValuePair.Next)
		{
			text = nameValuePair.Value;
			if (UseOdbcRules)
			{
				switch (nameValuePair.Name)
				{
				default:
					text = ExpandDataDirectory(nameValuePair.Name, text, ref datadir);
					break;
				case "driver":
				case "pwd":
				case "uid":
					break;
				}
			}
			else
			{
				switch (nameValuePair.Name)
				{
				default:
					text = ExpandDataDirectory(nameValuePair.Name, text, ref datadir);
					break;
				case "provider":
				case "data provider":
				case "remote provider":
				case "extended properties":
				case "user id":
				case "password":
				case "uid":
				case "pwd":
					break;
				}
			}
			if (text == null)
			{
				text = nameValuePair.Value;
			}
			if (UseOdbcRules || "file name" != nameValuePair.Name)
			{
				if (text != nameValuePair.Value)
				{
					flag = true;
					AppendKeyValuePairBuilder(stringBuilder, nameValuePair.Name, text, UseOdbcRules);
					stringBuilder.Append(';');
				}
				else
				{
					stringBuilder.Append(_usersConnectionString, num, nameValuePair.Length);
				}
			}
			else
			{
				flag = true;
				filename = text;
				position = stringBuilder.Length;
			}
			num += nameValuePair.Length;
		}
		if (flag)
		{
			return stringBuilder.ToString();
		}
		return null;
	}

	internal string ExpandKeyword(string keyword, string replacementValue)
	{
		bool flag = false;
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder(_usersConnectionString.Length);
		for (NameValuePair nameValuePair = KeyChain; nameValuePair != null; nameValuePair = nameValuePair.Next)
		{
			if (nameValuePair.Name == keyword && nameValuePair.Value == this[keyword])
			{
				AppendKeyValuePairBuilder(stringBuilder, nameValuePair.Name, replacementValue, UseOdbcRules);
				stringBuilder.Append(';');
				flag = true;
			}
			else
			{
				stringBuilder.Append(_usersConnectionString, num, nameValuePair.Length);
			}
			num += nameValuePair.Length;
		}
		if (!flag)
		{
			AppendKeyValuePairBuilder(stringBuilder, keyword, replacementValue, UseOdbcRules);
		}
		return stringBuilder.ToString();
	}

	private static string GetKeyName(StringBuilder buffer)
	{
		int num = buffer.Length;
		while (0 < num && char.IsWhiteSpace(buffer[num - 1]))
		{
			num--;
		}
		return buffer.ToString(0, num).ToLower(CultureInfo.InvariantCulture);
	}

	private static string GetKeyValue(StringBuilder buffer, bool trimWhitespace)
	{
		int num = buffer.Length;
		int i = 0;
		if (trimWhitespace)
		{
			for (; i < num && char.IsWhiteSpace(buffer[i]); i++)
			{
			}
			while (0 < num && char.IsWhiteSpace(buffer[num - 1]))
			{
				num--;
			}
		}
		return buffer.ToString(i, num - i);
	}

	internal static int GetKeyValuePair(string connectionString, int currentPosition, StringBuilder buffer, bool useOdbcRules, out string keyname, out string keyvalue)
	{
		int index = currentPosition;
		buffer.Length = 0;
		keyname = null;
		keyvalue = null;
		char c = '\0';
		ParserState parserState = ParserState.NothingYet;
		for (int length = connectionString.Length; currentPosition < length; currentPosition++)
		{
			c = connectionString[currentPosition];
			switch (parserState)
			{
			case ParserState.NothingYet:
				if (';' == c || char.IsWhiteSpace(c))
				{
					continue;
				}
				if (c == '\0')
				{
					parserState = ParserState.NullTermination;
					continue;
				}
				if (char.IsControl(c))
				{
					throw ADP.ConnectionStringSyntax(index);
				}
				index = currentPosition;
				if ('=' != c)
				{
					parserState = ParserState.Key;
					goto IL_0248;
				}
				parserState = ParserState.KeyEqual;
				continue;
			case ParserState.Key:
				if ('=' == c)
				{
					parserState = ParserState.KeyEqual;
					continue;
				}
				if (!char.IsWhiteSpace(c) && char.IsControl(c))
				{
					throw ADP.ConnectionStringSyntax(index);
				}
				goto IL_0248;
			case ParserState.KeyEqual:
				if (!useOdbcRules && '=' == c)
				{
					parserState = ParserState.Key;
					goto IL_0248;
				}
				keyname = GetKeyName(buffer);
				if (ADP.IsEmpty(keyname))
				{
					throw ADP.ConnectionStringSyntax(index);
				}
				buffer.Length = 0;
				parserState = ParserState.KeyEnd;
				goto case ParserState.KeyEnd;
			case ParserState.KeyEnd:
				if (char.IsWhiteSpace(c))
				{
					continue;
				}
				if (useOdbcRules)
				{
					if ('{' == c)
					{
						parserState = ParserState.BraceQuoteValue;
						goto IL_0248;
					}
				}
				else
				{
					if ('\'' == c)
					{
						parserState = ParserState.SingleQuoteValue;
						continue;
					}
					if ('"' == c)
					{
						parserState = ParserState.DoubleQuoteValue;
						continue;
					}
				}
				if (';' == c || c == '\0')
				{
					break;
				}
				if (char.IsControl(c))
				{
					throw ADP.ConnectionStringSyntax(index);
				}
				parserState = ParserState.UnquotedValue;
				goto IL_0248;
			case ParserState.UnquotedValue:
				if (!char.IsWhiteSpace(c) && (char.IsControl(c) || ';' == c))
				{
					break;
				}
				goto IL_0248;
			case ParserState.DoubleQuoteValue:
				if ('"' == c)
				{
					parserState = ParserState.DoubleQuoteValueQuote;
					continue;
				}
				if (c == '\0')
				{
					throw ADP.ConnectionStringSyntax(index);
				}
				goto IL_0248;
			case ParserState.DoubleQuoteValueQuote:
				if ('"' == c)
				{
					parserState = ParserState.DoubleQuoteValue;
					goto IL_0248;
				}
				keyvalue = GetKeyValue(buffer, trimWhitespace: false);
				parserState = ParserState.QuotedValueEnd;
				goto case ParserState.QuotedValueEnd;
			case ParserState.SingleQuoteValue:
				if ('\'' == c)
				{
					parserState = ParserState.SingleQuoteValueQuote;
					continue;
				}
				if (c == '\0')
				{
					throw ADP.ConnectionStringSyntax(index);
				}
				goto IL_0248;
			case ParserState.SingleQuoteValueQuote:
				if ('\'' == c)
				{
					parserState = ParserState.SingleQuoteValue;
					goto IL_0248;
				}
				keyvalue = GetKeyValue(buffer, trimWhitespace: false);
				parserState = ParserState.QuotedValueEnd;
				goto case ParserState.QuotedValueEnd;
			case ParserState.BraceQuoteValue:
				if ('}' == c)
				{
					parserState = ParserState.BraceQuoteValueQuote;
				}
				else if (c == '\0')
				{
					throw ADP.ConnectionStringSyntax(index);
				}
				goto IL_0248;
			case ParserState.BraceQuoteValueQuote:
				if ('}' == c)
				{
					parserState = ParserState.BraceQuoteValue;
					goto IL_0248;
				}
				keyvalue = GetKeyValue(buffer, trimWhitespace: false);
				parserState = ParserState.QuotedValueEnd;
				goto case ParserState.QuotedValueEnd;
			case ParserState.QuotedValueEnd:
				if (char.IsWhiteSpace(c))
				{
					continue;
				}
				if (';' != c)
				{
					if (c == '\0')
					{
						parserState = ParserState.NullTermination;
						continue;
					}
					throw ADP.ConnectionStringSyntax(index);
				}
				break;
			case ParserState.NullTermination:
				if (c == '\0' || char.IsWhiteSpace(c))
				{
					continue;
				}
				throw ADP.ConnectionStringSyntax(currentPosition);
			default:
				{
					throw ADP.InternalError(ADP.InternalErrorCode.InvalidParserState1);
				}
				IL_0248:
				buffer.Append(c);
				continue;
			}
			break;
		}
		switch (parserState)
		{
		case ParserState.Key:
		case ParserState.DoubleQuoteValue:
		case ParserState.SingleQuoteValue:
		case ParserState.BraceQuoteValue:
			throw ADP.ConnectionStringSyntax(index);
		case ParserState.KeyEqual:
			keyname = GetKeyName(buffer);
			if (ADP.IsEmpty(keyname))
			{
				throw ADP.ConnectionStringSyntax(index);
			}
			break;
		case ParserState.UnquotedValue:
		{
			keyvalue = GetKeyValue(buffer, trimWhitespace: true);
			char c2 = keyvalue[keyvalue.Length - 1];
			if (!useOdbcRules && ('\'' == c2 || '"' == c2))
			{
				throw ADP.ConnectionStringSyntax(index);
			}
			break;
		}
		case ParserState.DoubleQuoteValueQuote:
		case ParserState.SingleQuoteValueQuote:
		case ParserState.BraceQuoteValueQuote:
		case ParserState.QuotedValueEnd:
			keyvalue = GetKeyValue(buffer, trimWhitespace: false);
			break;
		default:
			throw ADP.InternalError(ADP.InternalErrorCode.InvalidParserState2);
		case ParserState.NothingYet:
		case ParserState.KeyEnd:
		case ParserState.NullTermination:
			break;
		}
		if (';' == c && currentPosition < connectionString.Length)
		{
			currentPosition++;
		}
		return currentPosition;
	}

	private static bool IsValueValidInternal(string keyvalue)
	{
		if (keyvalue != null)
		{
			return -1 == keyvalue.IndexOf('\0');
		}
		return true;
	}

	private static bool IsKeyNameValid(string keyname)
	{
		if (keyname != null)
		{
			if (0 < keyname.Length && ';' != keyname[0] && !char.IsWhiteSpace(keyname[0]))
			{
				return -1 == keyname.IndexOf('\0');
			}
			return false;
		}
		return false;
	}

	private static NameValuePair ParseInternal(Hashtable parsetable, string connectionString, bool buildChain, Hashtable synonyms, bool firstKey)
	{
		StringBuilder buffer = new StringBuilder();
		NameValuePair nameValuePair = null;
		NameValuePair result = null;
		int num = 0;
		int length = connectionString.Length;
		while (num < length)
		{
			int num2 = num;
			num = GetKeyValuePair(connectionString, num2, buffer, firstKey, out var keyname, out var keyvalue);
			if (ADP.IsEmpty(keyname))
			{
				break;
			}
			string text = ((synonyms != null) ? ((string)synonyms[keyname]) : keyname);
			if (!IsKeyNameValid(text))
			{
				throw ADP.KeywordNotSupported(keyname);
			}
			if (!firstKey || !parsetable.Contains(text))
			{
				parsetable[text] = keyvalue;
			}
			if (nameValuePair != null)
			{
				NameValuePair nameValuePair3 = (nameValuePair.Next = new NameValuePair(text, keyvalue, num - num2));
				nameValuePair = nameValuePair3;
			}
			else if (buildChain)
			{
				result = (nameValuePair = new NameValuePair(text, keyvalue, num - num2));
			}
		}
		return result;
	}

	internal NameValuePair ReplacePasswordPwd(out string constr, bool fakePassword)
	{
		bool flag = false;
		int num = 0;
		NameValuePair result = null;
		NameValuePair nameValuePair = null;
		NameValuePair nameValuePair2 = null;
		StringBuilder stringBuilder = new StringBuilder(_usersConnectionString.Length);
		for (NameValuePair nameValuePair3 = KeyChain; nameValuePair3 != null; nameValuePair3 = nameValuePair3.Next)
		{
			if ("password" != nameValuePair3.Name && "pwd" != nameValuePair3.Name)
			{
				stringBuilder.Append(_usersConnectionString, num, nameValuePair3.Length);
				if (fakePassword)
				{
					nameValuePair2 = new NameValuePair(nameValuePair3.Name, nameValuePair3.Value, nameValuePair3.Length);
				}
			}
			else if (fakePassword)
			{
				stringBuilder.Append(nameValuePair3.Name).Append("=*;");
				nameValuePair2 = new NameValuePair(nameValuePair3.Name, "*", nameValuePair3.Name.Length + "=*;".Length);
				flag = true;
			}
			else
			{
				flag = true;
			}
			if (fakePassword)
			{
				if (nameValuePair != null)
				{
					NameValuePair nameValuePair5 = (nameValuePair.Next = nameValuePair2);
					nameValuePair = nameValuePair5;
				}
				else
				{
					nameValuePair = (result = nameValuePair2);
				}
			}
			num += nameValuePair3.Length;
		}
		constr = stringBuilder.ToString();
		return result;
	}

	internal static void ValidateKeyValuePair(string keyword, string value)
	{
		if (keyword == null || !ConnectionStringValidKeyRegex.IsMatch(keyword))
		{
			throw ADP.InvalidKeyname(keyword);
		}
		if (value != null && !ConnectionStringValidValueRegex.IsMatch(value))
		{
			throw ADP.InvalidValue(keyword);
		}
	}
}
