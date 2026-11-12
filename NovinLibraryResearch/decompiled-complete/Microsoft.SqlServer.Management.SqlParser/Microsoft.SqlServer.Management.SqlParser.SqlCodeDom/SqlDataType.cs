using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDataType : SqlCodeObject
{
	internal readonly DataTypeSpec TypeSpec;

	internal readonly DataTypeErrors TypeErrors;

	private readonly SqlObjectIdentifier objectIdentifier;

	private readonly bool national;

	private readonly bool varying;

	internal override bool IsVisitable => true;

	public SqlObjectIdentifier ObjectIdentifier => objectIdentifier;

	public bool National => national;

	public bool Varying => varying;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return objectIdentifier;
		}
	}

	internal bool IsBinary => IsOfType(Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Binary);

	internal bool IsChar => IsOfType(Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Char);

	internal bool IsNChar => IsOfType(Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.NChar);

	internal bool IsUniqueidentifier => IsOfType(Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.UniqueIdentifier);

	internal bool IsVarBinary => IsOfType(Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.VarBinary);

	internal bool IsXml => IsOfType(Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType.Xml);

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

	internal string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider, IDataType boundType)
	{
		string result = null;
		if (BindUtils.DataType.AsUserDefinedType(boundType) == null && objectIdentifier.IsMultiPartName)
		{
			if (objectIdentifier.FindChildContainingLocation(ref cursorLocation) == objectIdentifier.ObjectName)
			{
				result = displayInfoProvider.GetDescription(boundType);
			}
		}
		else
		{
			result = objectIdentifier.GetQuickInfoText(cursorLocation, displayInfoProvider, boundType);
		}
		return result;
	}

	internal static SqlDataType Double(SqlObjectIdentifier objectIdentifier)
	{
		return new SqlDataType(objectIdentifier, national: false, varying: false);
	}

	internal SqlDataType(SqlObjectIdentifier objectIdentifier)
		: this(objectIdentifier, national: false, varying: false)
	{
	}

	internal SqlDataType(SqlObjectIdentifier objectIdentifier, bool national, bool varying)
	{
		this.objectIdentifier = objectIdentifier;
		this.national = national;
		this.varying = varying;
		TypeSpec = DataTypeLookup.Find(this.objectIdentifier.ObjectName.Value, national, varying, out TypeErrors);
	}

	public DataTypeSpec GetTypeSpec()
	{
		return TypeSpec;
	}

	private bool IsOfType(Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType sqlDataType)
	{
		if (TypeSpec == null)
		{
			return false;
		}
		return TypeSpec.SqlDataType == sqlDataType;
	}
}
