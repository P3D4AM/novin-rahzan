using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlErrorCodeObject : SqlCodeObject, ISqlErrorObject
{
	private readonly List<SqlCodeObject> children;

	internal override bool IsVisitable => false;

	public override IEnumerable<SqlCodeObject> Children => children.AsReadOnly();

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

	internal SqlErrorCodeObject(IEnumerable<SqlCodeObject> children)
	{
		this.children = new List<SqlCodeObject>();
		if (children == null)
		{
			return;
		}
		foreach (SqlCodeObject child in children)
		{
			if (child.IsVisitable)
			{
				this.children.Add(child);
			}
		}
	}
}
