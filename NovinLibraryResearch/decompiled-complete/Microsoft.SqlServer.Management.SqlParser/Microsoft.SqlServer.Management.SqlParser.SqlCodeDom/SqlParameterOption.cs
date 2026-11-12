using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlParameterOption : SqlCodeObject
{
	private sealed class OutputOption : SqlParameterOption
	{
		public OutputOption()
			: base(SqlParameterOptionType.Output)
		{
		}

		public override string ToString()
		{
			return "OUTPUT";
		}
	}

	private sealed class ReadOnlyOption : SqlParameterOption
	{
		public ReadOnlyOption()
			: base(SqlParameterOptionType.ReadOnly)
		{
		}

		public override string ToString()
		{
			return "READONLY";
		}
	}

	private sealed class InvalidOption : SqlParameterOption
	{
		private readonly string name;

		public InvalidOption(string name)
			: base(SqlParameterOptionType.Invalid)
		{
			this.name = name;
		}

		public override string ToString()
		{
			return name;
		}
	}

	public readonly SqlParameterOptionType Type;

	internal override bool IsVisitable => false;

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

	private SqlParameterOption(SqlParameterOptionType type)
	{
		Type = type;
	}

	public static SqlParameterOption Output()
	{
		return new OutputOption();
	}

	public static SqlParameterOption ReadOnly()
	{
		return new ReadOnlyOption();
	}

	internal static SqlParameterOption Create(string name)
	{
		switch (StringTokenLookup.Find(name))
		{
		case StringToken.Out:
		case StringToken.Output:
			return Output();
		case StringToken.ReadOnly:
			return ReadOnly();
		default:
			return new InvalidOption(name);
		}
	}
}
