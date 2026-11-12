using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

[Serializable]
public sealed class SqlClientPermission : DBDataPermission
{
	private static class XmlStr
	{
		internal const string _class = "class";

		internal const string _IPermission = "IPermission";

		internal const string _Permission = "Permission";

		internal const string _Unrestricted = "Unrestricted";

		internal const string _AllowBlankPassword = "AllowBlankPassword";

		internal const string _true = "true";

		internal const string _Version = "version";

		internal const string _VersionNumber = "1";

		internal const string _add = "add";

		internal const string _ConnectionString = "ConnectionString";

		internal const string _KeyRestrictions = "KeyRestrictions";

		internal const string _KeyRestrictionBehavior = "KeyRestrictionBehavior";
	}

	private NameValuePermission _keyvaluetree = NameValuePermission.Default;

	private ArrayList _keyvalues;

	private bool _IsUnrestricted
	{
		get
		{
			return ((DBDataPermission)this).IsUnrestricted();
		}
		set
		{
			FieldInfo field = ((object)this).GetType().BaseType.GetField("_isUnrestricted", BindingFlags.Instance | BindingFlags.NonPublic);
			field.SetValue(this, value);
		}
	}

	[Obsolete("SqlClientPermission() has been deprecated.  Use the SqlClientPermission(PermissionState.None) constructor.  http://go.microsoft.com/fwlink/?linkid=14202", true)]
	public SqlClientPermission()
		: this(PermissionState.None)
	{
	}

	public SqlClientPermission(PermissionState state)
		: base(state)
	{
	}

	[Obsolete("SqlClientPermission(PermissionState state, Boolean allowBlankPassword) has been deprecated.  Use the SqlClientPermission(PermissionState.None) constructor.  http://go.microsoft.com/fwlink/?linkid=14202", true)]
	public SqlClientPermission(PermissionState state, bool allowBlankPassword)
		: this(state)
	{
		((DBDataPermission)this).AllowBlankPassword = allowBlankPassword;
	}

	private SqlClientPermission(SqlClientPermission permission)
		: base((DBDataPermission)(object)permission)
	{
		CopyFrom(permission);
	}

	internal SqlClientPermission(SqlClientPermissionAttribute permissionAttribute)
		: base((DBDataPermissionAttribute)(object)permissionAttribute)
	{
	}

	internal SqlClientPermission(SqlConnectionString constr)
		: base(PermissionState.None)
	{
		if (constr != null)
		{
			((DBDataPermission)this).AllowBlankPassword = constr.HasBlankPassword;
			AddPermissionEntry(new DBConnectionString(constr));
		}
		if (constr == null || constr.IsEmpty)
		{
			((DBDataPermission)this).Add(ADP.StrEmpty, ADP.StrEmpty, KeyRestrictionBehavior.AllowOnly);
		}
	}

	public override void Add(string connectionString, string restrictions, KeyRestrictionBehavior behavior)
	{
		DBConnectionString entry = new DBConnectionString(connectionString, restrictions, behavior, SqlConnectionString.GetParseSynonyms(), useOdbcRules: false);
		AddPermissionEntry(entry);
	}

	public override IPermission Copy()
	{
		return (IPermission)(object)new SqlClientPermission(this);
	}

	internal void AddPermissionEntry(DBConnectionString entry)
	{
		if (_keyvaluetree == null)
		{
			_keyvaluetree = new NameValuePermission();
		}
		if (_keyvalues == null)
		{
			_keyvalues = new ArrayList();
		}
		NameValuePermission.AddEntry(_keyvaluetree, _keyvalues, entry);
		_IsUnrestricted = false;
	}

	private void CopyFrom(SqlClientPermission permission)
	{
		if (!_IsUnrestricted && permission._keyvalues != null)
		{
			_keyvalues = (ArrayList)permission._keyvalues.Clone();
			if (permission._keyvaluetree != null)
			{
				_keyvaluetree = permission._keyvaluetree.CopyNameValue();
			}
		}
	}

	public override IPermission Intersect(IPermission target)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		if (target == null)
		{
			return null;
		}
		if (target.GetType() != ((object)this).GetType())
		{
			throw ADP.PermissionTypeMismatch();
		}
		if (((DBDataPermission)this).IsUnrestricted())
		{
			return target.Copy();
		}
		DBDataPermission val = (DBDataPermission)target;
		if (val.IsUnrestricted())
		{
			return ((CodeAccessPermission)this).Copy();
		}
		SqlClientPermission sqlClientPermission = (SqlClientPermission)(object)((CodeAccessPermission)val).Copy();
		SqlClientPermission sqlClientPermission2 = sqlClientPermission;
		((DBDataPermission)sqlClientPermission2).AllowBlankPassword = ((DBDataPermission)sqlClientPermission2).AllowBlankPassword & ((DBDataPermission)this).AllowBlankPassword;
		if (_keyvalues != null && sqlClientPermission._keyvalues != null)
		{
			sqlClientPermission._keyvalues.Clear();
			sqlClientPermission._keyvaluetree.Intersect(sqlClientPermission._keyvalues, _keyvaluetree);
		}
		else
		{
			sqlClientPermission._keyvalues = null;
			sqlClientPermission._keyvaluetree = null;
		}
		if (sqlClientPermission.IsEmpty())
		{
			sqlClientPermission = null;
		}
		return (IPermission)(object)sqlClientPermission;
	}

	private bool IsEmpty()
	{
		ArrayList keyvalues = _keyvalues;
		return !((DBDataPermission)this).IsUnrestricted() && !((DBDataPermission)this).AllowBlankPassword && (keyvalues == null || keyvalues.Count == 0);
	}

	public override bool IsSubsetOf(IPermission target)
	{
		if (target == null)
		{
			return IsEmpty();
		}
		if (target.GetType() != ((object)this).GetType())
		{
			throw ADP.PermissionTypeMismatch();
		}
		SqlClientPermission sqlClientPermission = target as SqlClientPermission;
		bool flag = ((DBDataPermission)sqlClientPermission).IsUnrestricted();
		if (!flag && !((DBDataPermission)this).IsUnrestricted() && (!((DBDataPermission)this).AllowBlankPassword || ((DBDataPermission)sqlClientPermission).AllowBlankPassword) && (_keyvalues == null || sqlClientPermission._keyvaluetree != null))
		{
			flag = true;
			if (_keyvalues != null)
			{
				foreach (DBConnectionString keyvalue in _keyvalues)
				{
					if (!sqlClientPermission._keyvaluetree.CheckValueForKeyPermit(keyvalue))
					{
						flag = false;
						break;
					}
				}
			}
		}
		return flag;
	}

	public override IPermission Union(IPermission target)
	{
		if (target == null)
		{
			return ((CodeAccessPermission)this).Copy();
		}
		if (target.GetType() != ((object)this).GetType())
		{
			throw ADP.PermissionTypeMismatch();
		}
		if (((DBDataPermission)this).IsUnrestricted())
		{
			return ((CodeAccessPermission)this).Copy();
		}
		SqlClientPermission sqlClientPermission = (SqlClientPermission)(object)target.Copy();
		if (!((DBDataPermission)sqlClientPermission).IsUnrestricted())
		{
			((DBDataPermission)sqlClientPermission).AllowBlankPassword = ((DBDataPermission)sqlClientPermission).AllowBlankPassword | ((DBDataPermission)this).AllowBlankPassword;
			if (_keyvalues != null)
			{
				foreach (DBConnectionString keyvalue in _keyvalues)
				{
					sqlClientPermission.AddPermissionEntry(keyvalue);
				}
			}
		}
		if (!sqlClientPermission.IsEmpty())
		{
			return (IPermission)(object)sqlClientPermission;
		}
		return null;
	}

	private string DecodeXmlValue(string value)
	{
		if (value != null && 0 < value.Length)
		{
			value = value.Replace("&quot;", "\"");
			value = value.Replace("&apos;", "'");
			value = value.Replace("&lt;", "<");
			value = value.Replace("&gt;", ">");
			value = value.Replace("&amp;", "&");
		}
		return value;
	}

	private string EncodeXmlValue(string value)
	{
		if (value != null && 0 < value.Length)
		{
			value = value.Replace('\0', ' ');
			value = value.Trim();
			value = value.Replace("&", "&amp;");
			value = value.Replace(">", "&gt;");
			value = value.Replace("<", "&lt;");
			value = value.Replace("'", "&apos;");
			value = value.Replace("\"", "&quot;");
		}
		return value;
	}

	public override void FromXml(SecurityElement securityElement)
	{
		if (securityElement == null)
		{
			throw ADP.ArgumentNull("securityElement");
		}
		string tag = securityElement.Tag;
		if (!tag.Equals("Permission") && !tag.Equals("IPermission"))
		{
			throw ADP.NotAPermissionElement();
		}
		string text = securityElement.Attribute("version");
		if (text != null && !text.Equals("1"))
		{
			throw ADP.InvalidXMLBadVersion();
		}
		string text2 = securityElement.Attribute("Unrestricted");
		_IsUnrestricted = text2 != null && bool.Parse(text2);
		((DBDataPermission)this).Clear();
		if (!_IsUnrestricted)
		{
			string text3 = securityElement.Attribute("AllowBlankPassword");
			((DBDataPermission)this).AllowBlankPassword = text3 != null && bool.Parse(text3);
			ArrayList children = securityElement.Children;
			if (children == null)
			{
				return;
			}
			{
				foreach (SecurityElement item in children)
				{
					tag = item.Tag;
					if ("add" == tag || (tag != null && "add" == tag.ToLower(CultureInfo.InvariantCulture)))
					{
						string value = item.Attribute("ConnectionString");
						string value2 = item.Attribute("KeyRestrictions");
						string text4 = item.Attribute("KeyRestrictionBehavior");
						KeyRestrictionBehavior keyRestrictionBehavior = KeyRestrictionBehavior.AllowOnly;
						if (text4 != null)
						{
							keyRestrictionBehavior = (KeyRestrictionBehavior)Enum.Parse(typeof(KeyRestrictionBehavior), text4, ignoreCase: true);
						}
						value = DecodeXmlValue(value);
						value2 = DecodeXmlValue(value2);
						((DBDataPermission)this).Add(value, value2, keyRestrictionBehavior);
					}
				}
				return;
			}
		}
		((DBDataPermission)this).AllowBlankPassword = false;
	}

	public override SecurityElement ToXml()
	{
		Type type = ((object)this).GetType();
		SecurityElement securityElement = new SecurityElement("IPermission");
		securityElement.AddAttribute("class", type.AssemblyQualifiedName.Replace('"', '\''));
		securityElement.AddAttribute("version", "1");
		if (((DBDataPermission)this).IsUnrestricted())
		{
			securityElement.AddAttribute("Unrestricted", "true");
		}
		else
		{
			securityElement.AddAttribute("AllowBlankPassword", ((DBDataPermission)this).AllowBlankPassword.ToString(CultureInfo.InvariantCulture));
			if (_keyvalues != null)
			{
				foreach (DBConnectionString keyvalue in _keyvalues)
				{
					SecurityElement securityElement2 = new SecurityElement("add");
					string connectionString = keyvalue.ConnectionString;
					connectionString = EncodeXmlValue(connectionString);
					if (!ADP.IsEmpty(connectionString))
					{
						securityElement2.AddAttribute("ConnectionString", connectionString);
					}
					connectionString = keyvalue.Restrictions;
					connectionString = EncodeXmlValue(connectionString);
					if (connectionString == null)
					{
						connectionString = ADP.StrEmpty;
					}
					securityElement2.AddAttribute("KeyRestrictions", connectionString);
					connectionString = keyvalue.Behavior.ToString();
					securityElement2.AddAttribute("KeyRestrictionBehavior", connectionString);
					securityElement.AddChild(securityElement2);
				}
			}
		}
		return securityElement;
	}
}
