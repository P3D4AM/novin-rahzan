using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlLargeDataStorageInformation : SqlCodeObject
{
	private readonly SqlStorageSpecification textAndImageStorageSpecification;

	private readonly SqlStorageSpecification fileStreamStorageSpecification;

	internal override bool IsVisitable => true;

	public SqlStorageSpecification TextAndImageStorageSpecification => textAndImageStorageSpecification;

	public SqlStorageSpecification FileStreamStorageSpecification => fileStreamStorageSpecification;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (textAndImageStorageSpecification != null)
			{
				yield return textAndImageStorageSpecification;
			}
			if (fileStreamStorageSpecification != null)
			{
				yield return fileStreamStorageSpecification;
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

	internal SqlLargeDataStorageInformation(SqlStorageSpecification textAndImageStorageSpecification, SqlStorageSpecification fileStreamStorageSpecification)
	{
		this.textAndImageStorageSpecification = textAndImageStorageSpecification;
		this.fileStreamStorageSpecification = fileStreamStorageSpecification;
	}
}
