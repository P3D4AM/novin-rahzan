using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlLoginOption : SqlCodeObject
{
	public abstract class EmptyOption : SqlLoginOption
	{
		internal EmptyOption(SqlLoginOptionType type)
			: base(type)
		{
		}

		public override string ToString()
		{
			return TypeToString();
		}
	}

	public abstract class OnOffOption : SqlLoginOption
	{
		private readonly SqlOnOffValue value;

		public SqlOnOffValue Value => value;

		internal OnOffOption(SqlLoginOptionType type, SqlOnOffValue value)
			: base(type)
		{
			this.value = value;
		}

		internal static string OnOffToString(SqlOnOffValue value)
		{
			return value switch
			{
				SqlOnOffValue.Off => "OFF", 
				SqlOnOffValue.On => "ON", 
				_ => value.ToString(), 
			};
		}

		public override string ToString()
		{
			return TypeToString() + " = " + OnOffToString(value);
		}
	}

	public abstract class ObjectOption<V> : SqlLoginOption where V : SqlCodeObject
	{
		private readonly bool printEqualMark;

		private readonly V value;

		public V Value => value;

		public override IEnumerable<SqlCodeObject> Children
		{
			get
			{
				yield return value;
			}
		}

		internal ObjectOption(SqlLoginOptionType type, V value, bool printEqualMark = true)
			: base(type)
		{
			this.value = value;
			this.printEqualMark = printEqualMark;
		}

		public override string ToString()
		{
			if (!printEqualMark)
			{
				return TypeToString() + " " + value.ToString();
			}
			return TypeToString() + " = " + value.ToString();
		}
	}

	public abstract class InvalidOption : SqlLoginOption
	{
		private readonly string typeName;

		internal string TypeName => typeName;

		internal InvalidOption(SqlLoginOptionType type, string typeName)
			: base(type)
		{
			this.typeName = typeName;
		}

		internal override string TypeToString()
		{
			return typeName;
		}

		internal abstract void ReportError();
	}

	public sealed class InvalidEmptyOption : InvalidOption
	{
		internal InvalidEmptyOption(SqlLoginOptionType type, string typeName)
			: base(type, typeName)
		{
		}

		internal override void ReportError()
		{
			ReportParseError(SR.OptionNotRecognized(TypeToString()));
		}
	}

	public sealed class InvalidValueOption : InvalidOption
	{
		private readonly string value;

		internal InvalidValueOption(SqlLoginOptionType type, string typeName, string value)
			: base(type, typeName)
		{
			this.value = value;
		}

		internal override void ReportError()
		{
			switch (base.Type)
			{
			case SqlLoginOptionType.Unrecognized:
				ReportParseError(SR.OptionNotRecognized(TypeToString()));
				break;
			case SqlLoginOptionType.InvalidValue:
				ReportParseError(SR.IncorrectOptionValue(TypeToString(), value));
				break;
			}
		}
	}

	public sealed class InvalidObjectOption : InvalidOption
	{
		private readonly SqlCodeObject value;

		internal InvalidObjectOption(SqlLoginOptionType type, string typeName, SqlCodeObject value)
			: base(type, typeName)
		{
			this.value = value;
		}

		internal override void ReportError()
		{
			switch (base.Type)
			{
			case SqlLoginOptionType.Unrecognized:
				ReportParseError(SR.OptionNotRecognized(TypeToString()));
				break;
			case SqlLoginOptionType.InvalidValue:
				value.ReportParseError(SR.IncorrectOptionValue(TypeToString(), value.ToString()));
				break;
			}
		}
	}

	public sealed class HashedOption : EmptyOption
	{
		public HashedOption()
			: base(SqlLoginOptionType.Hashed)
		{
		}

		internal override string TypeToString()
		{
			return SqlLoginOptionContext.GetOptionString(base.Type);
		}
	}

	public sealed class MustChangeOption : EmptyOption
	{
		public MustChangeOption()
			: base(SqlLoginOptionType.MustChange)
		{
		}

		internal override string TypeToString()
		{
			return SqlLoginOptionContext.GetOptionString(base.Type);
		}
	}

	public sealed class SidOption : ObjectOption<SqlLiteralExpression>
	{
		public SidOption(SqlLiteralExpression value)
			: base(SqlLoginOptionType.Sid, value, printEqualMark: true)
		{
		}

		internal override string TypeToString()
		{
			return SqlLoginOptionContext.GetOptionString(base.Type);
		}
	}

	public sealed class DefaultDatabaseOption : ObjectOption<SqlIdentifier>
	{
		public DefaultDatabaseOption(SqlIdentifier value)
			: base(SqlLoginOptionType.DefaultDatabase, value, printEqualMark: true)
		{
		}

		internal override string TypeToString()
		{
			return SqlLoginOptionContext.GetOptionString(base.Type);
		}
	}

	public sealed class DefaultLanguageOption : ObjectOption<SqlIdentifier>
	{
		public DefaultLanguageOption(SqlIdentifier value)
			: base(SqlLoginOptionType.DefaultLanguage, value, printEqualMark: true)
		{
		}

		internal override string TypeToString()
		{
			return SqlLoginOptionContext.GetOptionString(base.Type);
		}
	}

	public sealed class CredentialOption : ObjectOption<SqlIdentifier>
	{
		public SqlModificationType ModificationType { get; private set; }

		public CredentialOption(SqlIdentifier value, SqlModificationType modificationValue)
			: base(SqlLoginOptionType.Credential, value, modificationValue == SqlModificationType.Set)
		{
			ModificationType = modificationValue;
		}

		internal override string TypeToString()
		{
			return SqlLoginOptionContext.GetOptionString(base.Type);
		}
	}

	public sealed class PasswordOption : SqlLoginOption
	{
		public SqlLoginPassword Password { get; private set; }

		public SqlLoginPassword OldPassword { get; private set; }

		public PasswordOption(SqlLoginPassword password, SqlLoginPassword oldPassword)
			: base(SqlLoginOptionType.Password)
		{
			Password = password;
			OldPassword = oldPassword;
		}

		internal override string TypeToString()
		{
			return SqlLoginOptionContext.GetOptionString(base.Type);
		}
	}

	public sealed class NameOption : ObjectOption<SqlIdentifier>
	{
		public NameOption(SqlIdentifier value)
			: base(SqlLoginOptionType.Name, value, printEqualMark: true)
		{
		}

		internal override string TypeToString()
		{
			return SqlLoginOptionContext.GetOptionString(base.Type);
		}
	}

	public sealed class NoCredentialOption : EmptyOption
	{
		public NoCredentialOption()
			: base(SqlLoginOptionType.NoCredential)
		{
		}

		internal override string TypeToString()
		{
			return SqlLoginOptionContext.GetOptionString(base.Type);
		}
	}

	public sealed class CheckExpirationOption : OnOffOption
	{
		public CheckExpirationOption(SqlOnOffValue value)
			: base(SqlLoginOptionType.CheckExpiration, value)
		{
		}

		internal override string TypeToString()
		{
			return SqlLoginOptionContext.GetOptionString(base.Type);
		}
	}

	public sealed class CheckPolicyOption : OnOffOption
	{
		public CheckPolicyOption(SqlOnOffValue value)
			: base(SqlLoginOptionType.CheckPolicy, value)
		{
		}

		internal override string TypeToString()
		{
			return SqlLoginOptionContext.GetOptionString(base.Type);
		}
	}

	private readonly SqlLoginOptionType type;

	internal override bool IsVisitable => false;

	public SqlLoginOptionType Type => type;

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		return default(T);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		return default(T);
	}

	internal SqlLoginOption(SqlLoginOptionType type)
	{
		this.type = type;
	}

	internal abstract string TypeToString();

	internal static SqlLoginOption Create(string name, SqlLoginOptionContext context)
	{
		SqlLoginOptionType optionType = context.GetOptionType(name);
		return optionType switch
		{
			SqlLoginOptionType.Hashed => new HashedOption(), 
			SqlLoginOptionType.MustChange => new MustChangeOption(), 
			_ => CreateInvalidOption(name, optionType), 
		};
	}

	internal static SqlLoginOption Create(string name, SqlIdentifier value, SqlLoginOptionContext context)
	{
		SqlLoginOptionType optionType = context.GetOptionType(name);
		return optionType switch
		{
			SqlLoginOptionType.DefaultDatabase => new DefaultDatabaseOption(value), 
			SqlLoginOptionType.DefaultLanguage => new DefaultLanguageOption(value), 
			SqlLoginOptionType.Credential => new CredentialOption(value, SqlModificationType.Set), 
			SqlLoginOptionType.Name => new NameOption(value), 
			_ => CreateInvalidOption(name, optionType, value), 
		};
	}

	internal static SqlLoginOption Create(string name, SqlOnOffValue value, SqlLoginOptionContext context)
	{
		SqlLoginOptionType optionType = context.GetOptionType(name);
		return optionType switch
		{
			SqlLoginOptionType.CheckExpiration => new CheckExpirationOption(value), 
			SqlLoginOptionType.CheckPolicy => new CheckPolicyOption(value), 
			_ => CreateInvalidOption(name, optionType, OnOffOption.OnOffToString(value)), 
		};
	}

	internal static SqlLoginOption Create(string name, SqlLiteralExpression value, SqlLoginOptionContext context)
	{
		SqlLoginOptionType optionType = context.GetOptionType(name);
		if (optionType == SqlLoginOptionType.Sid)
		{
			return new SidOption(value);
		}
		return CreateInvalidOption(name, optionType, value);
	}

	internal static SqlLoginOption Create(SqlModificationType modificationType, string name, SqlIdentifier value, SqlLoginOptionContext context)
	{
		SqlLoginOptionType optionType = context.GetOptionType(name);
		if (optionType == SqlLoginOptionType.Credential)
		{
			return new CredentialOption(value, modificationType);
		}
		return CreateInvalidOption(name, optionType, value);
	}

	internal static SqlLoginOption Create(SqlLoginOptionType loginOptionTypeType)
	{
		return new NoCredentialOption();
	}

	internal static SqlLoginOption Create(SqlLoginPassword password, SqlLoginPassword oldPassword)
	{
		return new PasswordOption(password, oldPassword);
	}

	private static SqlLoginOption CreateInvalidOption(string name, SqlLoginOptionType type)
	{
		return new InvalidEmptyOption(SqlLoginOptionType.Unrecognized, name);
	}

	private static SqlLoginOption CreateInvalidOption(string name, SqlLoginOptionType type, SqlCodeObject value)
	{
		SqlLoginOptionType sqlLoginOptionType = ((type == SqlLoginOptionType.Invalid) ? SqlLoginOptionType.Unrecognized : SqlLoginOptionType.InvalidValue);
		return new InvalidObjectOption(sqlLoginOptionType, name, value);
	}

	private static SqlLoginOption CreateInvalidOption(string name, SqlLoginOptionType type, string value)
	{
		SqlLoginOptionType sqlLoginOptionType = ((type == SqlLoginOptionType.Invalid) ? SqlLoginOptionType.Unrecognized : SqlLoginOptionType.InvalidValue);
		return new InvalidValueOption(sqlLoginOptionType, name, value);
	}

	internal OnOffOption AsOnOffOption()
	{
		return this as OnOffOption;
	}

	internal PasswordOption AsPasswordOption()
	{
		return this as PasswordOption;
	}

	internal InvalidOption AsInvalidOption()
	{
		return this as InvalidOption;
	}

	internal ObjectOption<T> AsObjectOption<T>() where T : SqlCodeObject
	{
		return this as ObjectOption<T>;
	}
}
