using System;
using System.Collections.Generic;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCastExpression : SqlBuiltinScalarFunctionCallExpression
{
	private SqlDataTypeSpecification dataType;

	internal override bool IsVisitable => true;

	internal sealed override bool IsValidFilterExpressionConstant => base.Arguments[0].IsValidFilterExpressionConstant;

	public SqlDataTypeSpecification DataType => dataType;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Arguments[0];
			yield return DataType;
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

	internal sealed override void Bind(IBindingContext bindingContext)
	{
		foreach (SqlCodeObject child in Children)
		{
			child.Bind(bindingContext);
			base.BindingState |= child.BindingState;
		}
		switch (StringTokenLookup.Find(base.FunctionName))
		{
		case StringToken.Cast:
		case StringToken.Parse:
		case StringToken.Convert:
		case StringToken.TryCast:
		case StringToken.TryParse:
		case StringToken.TryConvert:
		{
			IDataType boundType = dataType.BoundType;
			ISystemDataType systemDataType = ((boundType != null) ? BindUtils.DataType.AsSystemDataType(boundType) : null);
			if (systemDataType == null)
			{
				DataType.ReportBindError(SR.TypeIsNotSystemType(DataType.ToString()));
				boundFunction = bindingContext.Batch.FindBuiltInFunction(base.FunctionName);
				base.BindingState = BindingState.BindSucceeded;
			}
			else
			{
				boundFunction = bindingContext.Batch.FindMultiTypeBuiltInFunction(base.FunctionName, systemDataType);
			}
			base.BindingState = BindingState.BindSucceeded;
			break;
		}
		default:
			ReportBindError(SR.NotRecognizedFunctionName(base.FunctionName));
			base.BindingState = BindingState.BindFailed;
			break;
		}
	}

	internal override MethodNameAndParamLocations GetMyMethodNameAndParams(LexLocation caretLocation)
	{
		return null;
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> metadataCollection = null;
		if (FindChildContainingLocation(ref caretLocation) == DataType)
		{
			return DataType.GetCompletionList(caretLocation, CompletionContext.SystemDataTypes);
		}
		return base.GetCompletionList(caretLocation, completionContext);
	}

	internal SqlCastExpression(string name, SqlDataTypeSpecification dataType, SqlScalarExpression argument)
		: this(name, dataType, argument, null)
	{
	}

	internal SqlCastExpression(string name, SqlDataTypeSpecification dataType, SqlScalarExpression argument1, SqlScalarExpression argument2)
		: base(name, Enumerate(argument1, argument2))
	{
		this.dataType = dataType;
	}

	private static IEnumerable<SqlScalarExpression> Enumerate(SqlScalarExpression arg1, SqlScalarExpression arg2)
	{
		if (arg1 != null)
		{
			yield return arg1;
		}
		if (arg2 != null)
		{
			yield return arg2;
		}
	}
}
