using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDataCompressionIndexOption : SqlIndexOption
{
	private readonly SqlDataCompressionType compressionType;

	private readonly SqlCompressionPartitionRangeCollection partitions;

	internal override bool IsVisitable => true;

	internal sealed override string Phrase => "DATA_COMPRESSION";

	public SqlDataCompressionType CompressionType => compressionType;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (partitions == null)
			{
				yield break;
			}
			foreach (SqlCompressionPartitionRange partition in partitions)
			{
				yield return partition;
			}
		}
	}

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

	internal SqlDataCompressionIndexOption(SqlDataCompressionType compressionType, IEnumerable<SqlCompressionPartitionRange> partitions)
		: base(SqlIndexOptionType.DataCompression)
	{
		if (partitions != null)
		{
			this.partitions = new SqlCompressionPartitionRangeCollection(partitions);
		}
		this.compressionType = compressionType;
	}
}
