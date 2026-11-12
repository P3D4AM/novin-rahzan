using System;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlIdentifier : SqlCodeObject
{
	private sealed class NullSqlIdentifier : SqlIdentifier
	{
		public static readonly SqlIdentifier Instance = new NullSqlIdentifier();

		private NullSqlIdentifier()
		{
		}
	}

	private sealed class MissingSqlIdentifier : SqlIdentifier
	{
		internal override bool IsMissing => true;

		internal MissingSqlIdentifier()
			: base("")
		{
		}
	}

	private readonly string value;

	internal override bool IsVisitable => true;

	public string Value => value;

	internal bool IsEmpty => string.IsNullOrEmpty(value);

	internal virtual bool IsMissing => false;

	internal bool IsTempName
	{
		get
		{
			if (value != null)
			{
				return value.StartsWith("#", StringComparison.Ordinal);
			}
			return false;
		}
	}

	internal static SqlIdentifier Null => NullSqlIdentifier.Instance;

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

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return RetrieveCompletionListItems(completionContext);
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		BatchBindingContext batch = BindingContext.Batch;
		if (batch != null)
		{
			IMetadataObject boundObject = batch.GetBoundObject(this);
			return (boundObject != null) ? displayInfoProvider.GetDescription(boundObject) : null;
		}
		return null;
	}

	private SqlIdentifier()
	{
	}

	internal SqlIdentifier(string value)
	{
		this.value = EscapeSequence.UnescapeIdentifier(value);
		if (this.value.Length == 0 && value.Length != 0)
		{
			ReportParseError(SR.ObjectNameIsMissingOrEmpty);
		}
	}

	public override string ToString()
	{
		return value;
	}

	internal static bool IsNullOrEmpty(SqlIdentifier identifier)
	{
		return identifier?.IsEmpty ?? true;
	}

	internal static bool IsNull(SqlIdentifier identifier)
	{
		if (identifier == null)
		{
			return true;
		}
		return identifier.value == null;
	}

	internal static string RetrieveValue(SqlIdentifier identifier)
	{
		return identifier?.value;
	}

	internal static SqlIdentifier Missing()
	{
		return new MissingSqlIdentifier();
	}
}
