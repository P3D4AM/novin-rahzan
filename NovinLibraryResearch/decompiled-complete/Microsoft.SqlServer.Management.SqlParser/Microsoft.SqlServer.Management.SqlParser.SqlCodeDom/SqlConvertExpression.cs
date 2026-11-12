using System;
using System.Collections.Generic;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlConvertExpression : SqlCastExpression
{
	internal override bool IsVisitable => true;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.DataType;
			foreach (SqlScalarExpression argument in base.Arguments)
			{
				yield return argument;
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

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject == base.DataType)
		{
			result = base.DataType.GetCompletionList(caretLocation, CompletionContext.SystemDataTypes);
		}
		else if (sqlCodeObject != null)
		{
			result = sqlCodeObject.GetCompletionList(caretLocation, completionContext);
		}
		return result;
	}

	internal override MethodNameAndParamLocations GetMyMethodNameAndParams(LexLocation caretLocation)
	{
		MethodNameAndParamLocations result = null;
		if (BoundFunction != null)
		{
			TokenManager tokenManager = TokenManager;
			MethodNameAndParamLocations methodNameAndParamLocations = new MethodNameAndParamLocations(BoundObject.Name)
			{
				NameStartLocation = base.StartLocation,
				NameEndLocation = base.EndLocation
			};
			int nameEndPosition = base.Position.startTokenNumber + 1;
			methodNameAndParamLocations.ParamSeperatorLocations = new List<Location>();
			List<SqlCodeObject> list = GetSqlCodeObjectList(base.Arguments);
			if (list == null)
			{
				list = new List<SqlCodeObject>();
			}
			list.Insert(0, base.DataType);
			BuildMethodNameAndParams(methodNameAndParamLocations, nameEndPosition, list);
			Location location = tokenManager.GetLocation(caretLocation.startTokenNumber);
			if (methodNameAndParamLocations.IsLocationInArgumentList(location))
			{
				result = methodNameAndParamLocations;
			}
		}
		return result;
	}

	internal SqlConvertExpression(string name, SqlDataTypeSpecification dataType, SqlScalarExpression argument1, SqlScalarExpression argument2)
		: base(name, dataType, argument1, argument2)
	{
	}
}
