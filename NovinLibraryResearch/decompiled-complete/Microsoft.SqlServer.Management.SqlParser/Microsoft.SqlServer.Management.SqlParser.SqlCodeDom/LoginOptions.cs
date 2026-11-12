using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class LoginOptions
{
	private sealed class SqlCodeObjectPosComparer : IComparer<SqlCodeObject>
	{
		public int Compare(SqlCodeObject x, SqlCodeObject y)
		{
			return x.Position.CompareTo(y.Position);
		}
	}

	private static readonly SqlCodeObjectPosComparer sqlCodeObjectPosComparer = new SqlCodeObjectPosComparer();

	private SqlCodeObject[] optionObjs;

	private bool isOptionObjsCached;

	public SqlLiteralExpression Sid { get; private set; }

	public SqlIdentifier DefaultDatabase { get; private set; }

	public SqlIdentifier DefaultLanguage { get; private set; }

	public SqlIdentifier Credential { get; private set; }

	public SqlOnOffValue CheckExpiration { get; private set; }

	public SqlOnOffValue CheckPolicy { get; private set; }

	public SqlIdentifier Name { get; private set; }

	public SqlLoginPassword Password { get; private set; }

	public SqlLoginPassword OldPassword { get; private set; }

	public bool NoCredential { get; private set; }

	public IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (!isOptionObjsCached)
			{
				int num = ((Sid != null) ? 1 : 0) + ((DefaultDatabase != null) ? 1 : 0) + ((DefaultLanguage != null) ? 1 : 0) + ((Credential != null) ? 1 : 0) + ((Name != null) ? 1 : 0) + ((Password != null) ? 1 : 0) + ((OldPassword != null) ? 1 : 0);
				if (num > 0)
				{
					optionObjs = new SqlCodeObject[num];
					int num2 = 0;
					if (Sid != null)
					{
						optionObjs[num2++] = Sid;
					}
					if (DefaultDatabase != null)
					{
						optionObjs[num2++] = DefaultDatabase;
					}
					if (DefaultLanguage != null)
					{
						optionObjs[num2++] = DefaultLanguage;
					}
					if (Credential != null)
					{
						optionObjs[num2++] = Credential;
					}
					if (Name != null)
					{
						optionObjs[num2++] = Name;
					}
					if (Password != null)
					{
						optionObjs[num2++] = Password;
					}
					if (OldPassword != null)
					{
						optionObjs[num2] = OldPassword;
					}
					Array.Sort(optionObjs, sqlCodeObjectPosComparer);
				}
				isOptionObjsCached = true;
			}
			if (optionObjs != null)
			{
				for (int i = 0; i < optionObjs.Length; i++)
				{
					yield return optionObjs[i];
				}
			}
		}
	}

	public LoginOptions(List<SqlLoginOption> options)
	{
		if (options == null)
		{
			return;
		}
		SqlLoginOptionType sqlLoginOptionType = SqlLoginOptionType.None;
		foreach (SqlLoginOption option in options)
		{
			SqlLoginOptionType type = option.Type;
			if ((sqlLoginOptionType & type) == type)
			{
				string message = SR.OptionSpecifiedMultipleTimes(option.TypeToString());
				option.ReportParseError(message);
				continue;
			}
			if ((type & SqlLoginOptionType.Invalid) == SqlLoginOptionType.Invalid)
			{
				option.AsInvalidOption().ReportError();
				continue;
			}
			switch (type)
			{
			case SqlLoginOptionType.Sid:
				Sid = option.AsObjectOption<SqlLiteralExpression>().Value;
				break;
			case SqlLoginOptionType.Credential:
				Credential = option.AsObjectOption<SqlIdentifier>().Value;
				break;
			case SqlLoginOptionType.DefaultDatabase:
				DefaultDatabase = option.AsObjectOption<SqlIdentifier>().Value;
				break;
			case SqlLoginOptionType.DefaultLanguage:
				DefaultLanguage = option.AsObjectOption<SqlIdentifier>().Value;
				break;
			case SqlLoginOptionType.CheckExpiration:
				CheckExpiration = option.AsOnOffOption().Value;
				break;
			case SqlLoginOptionType.CheckPolicy:
				CheckPolicy = option.AsOnOffOption().Value;
				break;
			case SqlLoginOptionType.Name:
				Name = option.AsObjectOption<SqlIdentifier>().Value;
				break;
			case SqlLoginOptionType.Password:
			{
				SqlLoginOption.PasswordOption passwordOption = option.AsPasswordOption();
				Password = passwordOption.Password;
				OldPassword = passwordOption.OldPassword;
				break;
			}
			case SqlLoginOptionType.NoCredential:
				NoCredential = true;
				break;
			}
			sqlLoginOptionType |= type;
		}
	}
}
