using System;
using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlModuleOption : SqlCodeObject
{
	private sealed class InvalidOption : SqlModuleOption
	{
		private readonly string name;

		internal override bool IsVisitable => true;

		internal override string DisplayName => name;

		public override void Accept(ISqlCodeObjectVisitor visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			visitor.Visit(this);
		}

		public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			return visitor.Visit(this);
		}

		public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			visitor.Visit(this, context);
		}

		public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			return visitor.Visit(this, context);
		}

		public InvalidOption(string name)
			: base(SqlModuleOptionType.None)
		{
			this.name = name;
		}
	}

	private readonly SqlModuleOptionType type;

	public SqlModuleOptionType Type => type;

	internal virtual SqlModuleOptionMask TypeMask => GetTypeMask(type);

	internal virtual SqlExecuteAsClause ExecuteAsClause => null;

	internal abstract string DisplayName { get; }

	internal SqlModuleOption(SqlModuleOptionType type)
	{
		this.type = type;
	}

	internal static SqlModuleOptionMask GetTypeMask(SqlModuleOptionType type)
	{
		if (type == SqlModuleOptionType.None)
		{
			return SqlModuleOptionMask.Invalid;
		}
		return (SqlModuleOptionMask)(1 << (int)(ushort)type);
	}

	internal static SqlModuleOption Create(string name)
	{
		return StringTokenLookup.Find(name) switch
		{
			StringToken.Encryption => CreateEncryptionOption(), 
			StringToken.Recompile => CreateRecompileOption(), 
			StringToken.SchemaBinding => CreateSchemaBindingOption(), 
			StringToken.ViewMetadata => CreateViewMetadataOption(), 
			StringToken.NativeCompilation => CreateNativeCompilationOption(), 
			_ => new InvalidOption(name), 
		};
	}

	internal static SqlModuleEncryptionOption CreateEncryptionOption()
	{
		return new SqlModuleEncryptionOption();
	}

	internal static SqlModuleRecompileOption CreateRecompileOption()
	{
		return new SqlModuleRecompileOption();
	}

	internal static SqlModuleSchemaBindingOption CreateSchemaBindingOption()
	{
		return new SqlModuleSchemaBindingOption();
	}

	internal static SqlModuleViewMetadataOption CreateViewMetadataOption()
	{
		return new SqlModuleViewMetadataOption();
	}

	internal static SqlModuleReturnsNullOnNullInputOption CreateReturnsNullOnNullInputOption()
	{
		return new SqlModuleReturnsNullOnNullInputOption();
	}

	internal static SqlModuleCalledOnNullInputOption CreateCalledOnNullInputOption()
	{
		return new SqlModuleCalledOnNullInputOption();
	}

	internal static SqlModuleExecuteAsOption ExecuteAs(SqlExecuteAsClause executeAsClause)
	{
		return new SqlModuleExecuteAsOption(executeAsClause);
	}

	internal static SqlModuleNativeCompilationOption CreateNativeCompilationOption()
	{
		return new SqlModuleNativeCompilationOption();
	}
}
