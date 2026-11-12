using System;
using System.Collections.Generic;
using System.Text;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlObjectIdentifier : SqlMultipartIdentifier
{
	private sealed class OnePartObjectIdentifier : SqlObjectIdentifier
	{
		private readonly SqlIdentifier objectName;

		public override SqlIdentifier ServerName => SqlIdentifier.Null;

		public override SqlIdentifier DatabaseName => SqlIdentifier.Null;

		public override SqlIdentifier SchemaName => SqlIdentifier.Null;

		public override SqlIdentifier ObjectName => objectName;

		public override bool IsMultiPartName => false;

		public override IEnumerable<SqlCodeObject> Children
		{
			get
			{
				yield return objectName;
			}
		}

		public override int Count => 1;

		public override SqlIdentifier this[int index]
		{
			get
			{
				if (index == 0)
				{
					return objectName;
				}
				throw new IndexOutOfRangeException();
			}
		}

		internal OnePartObjectIdentifier(SqlIdentifier objectName)
		{
			this.objectName = objectName;
		}

		public override string ToString()
		{
			return objectName.ToString();
		}

		public override IEnumerator<SqlIdentifier> GetEnumerator()
		{
			yield return objectName;
		}
	}

	private sealed class TwoPartObjectIdentifier : SqlObjectIdentifier
	{
		private readonly SqlIdentifier schemaName;

		private readonly SqlIdentifier objectName;

		public override SqlIdentifier ServerName => SqlIdentifier.Null;

		public override SqlIdentifier DatabaseName => SqlIdentifier.Null;

		public override SqlIdentifier SchemaName => schemaName;

		public override SqlIdentifier ObjectName => objectName;

		public override bool IsMultiPartName => true;

		public override IEnumerable<SqlCodeObject> Children
		{
			get
			{
				yield return schemaName;
				yield return objectName;
			}
		}

		public override int Count => 2;

		public override SqlIdentifier this[int index] => index switch
		{
			0 => schemaName, 
			1 => objectName, 
			_ => throw new IndexOutOfRangeException(), 
		};

		internal TwoPartObjectIdentifier(SqlIdentifier schemaName, SqlIdentifier objectName)
		{
			this.schemaName = schemaName;
			this.objectName = objectName;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(schemaName);
			stringBuilder.Append('.');
			stringBuilder.Append(objectName);
			return stringBuilder.ToString();
		}

		public override IEnumerator<SqlIdentifier> GetEnumerator()
		{
			yield return schemaName;
			yield return objectName;
		}
	}

	private sealed class ThreePartObjectIdentifier : SqlObjectIdentifier
	{
		private readonly SqlIdentifier databaseName;

		private readonly SqlIdentifier schemaName;

		private readonly SqlIdentifier objectName;

		public override SqlIdentifier ServerName => SqlIdentifier.Null;

		public override SqlIdentifier DatabaseName => databaseName;

		public override SqlIdentifier SchemaName => schemaName;

		public override SqlIdentifier ObjectName => objectName;

		public override bool IsMultiPartName => true;

		public override IEnumerable<SqlCodeObject> Children
		{
			get
			{
				yield return databaseName;
				yield return schemaName;
				yield return objectName;
			}
		}

		public override int Count => 3;

		public override SqlIdentifier this[int index] => index switch
		{
			0 => databaseName, 
			1 => schemaName, 
			2 => objectName, 
			_ => throw new IndexOutOfRangeException(), 
		};

		internal ThreePartObjectIdentifier(SqlIdentifier databaseName, SqlIdentifier schemaName, SqlIdentifier objectName)
		{
			this.databaseName = databaseName;
			this.schemaName = schemaName;
			this.objectName = objectName;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(databaseName);
			stringBuilder.Append('.');
			stringBuilder.Append(schemaName);
			stringBuilder.Append('.');
			stringBuilder.Append(objectName);
			return stringBuilder.ToString();
		}

		public override IEnumerator<SqlIdentifier> GetEnumerator()
		{
			yield return databaseName;
			yield return schemaName;
			yield return objectName;
		}
	}

	private sealed class FourPartObjectIdentifier : SqlObjectIdentifier
	{
		private readonly SqlIdentifier serverName;

		private readonly SqlIdentifier databaseName;

		private readonly SqlIdentifier schemaName;

		private readonly SqlIdentifier objectName;

		public override SqlIdentifier ServerName => serverName;

		public override SqlIdentifier DatabaseName => databaseName;

		public override SqlIdentifier SchemaName => schemaName;

		public override SqlIdentifier ObjectName => objectName;

		public override bool IsMultiPartName => true;

		public override IEnumerable<SqlCodeObject> Children
		{
			get
			{
				yield return serverName;
				yield return databaseName;
				yield return schemaName;
				yield return objectName;
			}
		}

		public override int Count => 4;

		public override SqlIdentifier this[int index] => index switch
		{
			0 => serverName, 
			1 => databaseName, 
			2 => schemaName, 
			3 => objectName, 
			_ => throw new IndexOutOfRangeException(), 
		};

		internal FourPartObjectIdentifier(SqlIdentifier serverName, SqlIdentifier databaseName, SqlIdentifier schemaName, SqlIdentifier objectName)
		{
			this.serverName = serverName;
			this.databaseName = databaseName;
			this.schemaName = schemaName;
			this.objectName = objectName;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(serverName);
			stringBuilder.Append('.');
			stringBuilder.Append(databaseName);
			stringBuilder.Append('.');
			stringBuilder.Append(schemaName);
			stringBuilder.Append('.');
			stringBuilder.Append(objectName);
			return stringBuilder.ToString();
		}

		public override IEnumerator<SqlIdentifier> GetEnumerator()
		{
			yield return serverName;
			yield return databaseName;
			yield return schemaName;
			yield return objectName;
		}
	}

	private sealed class MultiPartObjectIdentifier : SqlObjectIdentifier
	{
		private readonly SqlIdentifier[] identifiers;

		public override SqlIdentifier ServerName => identifiers[identifiers.Length - 4];

		public override SqlIdentifier DatabaseName => identifiers[identifiers.Length - 3];

		public override SqlIdentifier SchemaName => identifiers[identifiers.Length - 2];

		public override SqlIdentifier ObjectName => identifiers[identifiers.Length - 1];

		public override bool IsMultiPartName => true;

		public override IEnumerable<SqlCodeObject> Children => identifiers;

		public override int Count => identifiers.Length;

		public override SqlIdentifier this[int index] => identifiers[index];

		public MultiPartObjectIdentifier(List<SqlIdentifier> ids)
		{
			identifiers = ids.ToArray();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < identifiers.Length - 1; i++)
			{
				stringBuilder.Append(identifiers[i]);
				stringBuilder.Append('.');
			}
			stringBuilder.Append(identifiers[identifiers.Length - 1]);
			return stringBuilder.ToString();
		}

		public override IEnumerator<SqlIdentifier> GetEnumerator()
		{
			for (int i = 0; i < identifiers.Length; i++)
			{
				yield return identifiers[i];
			}
		}
	}

	internal override bool IsVisitable => true;

	public abstract SqlIdentifier ServerName { get; }

	public abstract SqlIdentifier DatabaseName { get; }

	public abstract SqlIdentifier SchemaName { get; }

	public abstract SqlIdentifier ObjectName { get; }

	public abstract bool IsMultiPartName { get; }

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

	internal string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider, IMetadataObject boundObject)
	{
		if (IsMultiPartName)
		{
			IDatabaseObject databaseObject = boundObject as IDatabaseObject;
			SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref cursorLocation);
			if (sqlCodeObject == DatabaseName)
			{
				boundObject = databaseObject.Parent.Parent;
			}
			else if (sqlCodeObject == SchemaName)
			{
				boundObject = databaseObject.Parent;
			}
			else if (sqlCodeObject != ObjectName)
			{
				boundObject = null;
			}
		}
		if (boundObject == null)
		{
			return null;
		}
		return displayInfoProvider.GetDescription(boundObject);
	}

	internal static SqlObjectIdentifier Create(SqlIdentifier objectName)
	{
		return new OnePartObjectIdentifier(objectName);
	}

	internal static SqlObjectIdentifier Create(SqlIdentifier schemaName, SqlIdentifier objectName)
	{
		return new TwoPartObjectIdentifier(schemaName, objectName);
	}

	internal static SqlObjectIdentifier Create(SqlIdentifier databaseName, SqlIdentifier schemaName, SqlIdentifier objectName)
	{
		return new ThreePartObjectIdentifier(databaseName, schemaName, objectName);
	}

	internal static SqlObjectIdentifier Create(List<SqlIdentifier> ids)
	{
		return ids.Count switch
		{
			1 => new OnePartObjectIdentifier(ids[0]), 
			2 => new TwoPartObjectIdentifier(ids[0], ids[1]), 
			3 => new ThreePartObjectIdentifier(ids[0], ids[1], ids[2]), 
			4 => new FourPartObjectIdentifier(ids[0], ids[1], ids[2], ids[3]), 
			_ => new MultiPartObjectIdentifier(ids), 
		};
	}
}
