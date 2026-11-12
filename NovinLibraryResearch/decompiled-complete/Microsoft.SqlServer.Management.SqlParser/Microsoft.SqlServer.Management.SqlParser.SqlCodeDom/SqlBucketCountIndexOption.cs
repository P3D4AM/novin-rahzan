using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlBucketCountIndexOption : SqlIndexOption
{
	private readonly int bucketCount;

	internal override bool IsVisitable => true;

	internal sealed override string Phrase => "BUCKET_COUNT";

	public int BucketCount => bucketCount;

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

	internal SqlBucketCountIndexOption(SqlLiteralExpression bucketCount)
		: base(SqlIndexOptionType.BucketCount)
	{
		if (bucketCount != null)
		{
			this.bucketCount = bucketCount.AsInt32().Value;
		}
	}
}
