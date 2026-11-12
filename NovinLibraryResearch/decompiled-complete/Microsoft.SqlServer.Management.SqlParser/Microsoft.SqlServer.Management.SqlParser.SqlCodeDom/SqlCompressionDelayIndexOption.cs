using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCompressionDelayIndexOption : SqlIndexOption
{
	private readonly int compressionDelay;

	internal override bool IsVisitable => true;

	internal sealed override string Phrase => "COMPRESSION_DELAY";

	public int CompressionDelay => compressionDelay;

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

	internal SqlCompressionDelayIndexOption(SqlLiteralExpression compressionDelay)
		: base(SqlIndexOptionType.CompressionDelay)
	{
		if (compressionDelay != null)
		{
			this.compressionDelay = compressionDelay.AsInt32().Value;
		}
	}
}
