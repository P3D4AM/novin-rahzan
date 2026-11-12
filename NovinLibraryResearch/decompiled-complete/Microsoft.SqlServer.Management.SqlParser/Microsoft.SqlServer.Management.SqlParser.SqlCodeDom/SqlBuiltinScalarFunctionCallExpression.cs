using System;
using System.Collections.Generic;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlBuiltinScalarFunctionCallExpression : SqlScalarFunctionCallExpression
{
	internal IBuiltInFunction boundFunction;

	private readonly string functionName;

	private readonly bool isStar;

	internal override bool IsVisitable => true;

	internal override bool IsValidFilterExpressionConstant => false;

	public override IScalarFunction BoundFunction => boundFunction;

	public string FunctionName => functionName;

	public bool IsStar => isStar;

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

	internal override void Bind(IBindingContext bindingContext)
	{
		boundFunction = bindingContext.Batch.FindBuiltInFunction(functionName);
		if (boundFunction != null)
		{
			base.BindingState = BindingState.BindSucceeded;
			if (isStar)
			{
				return;
			}
			int num = ((base.Arguments != null) ? base.Arguments.Count : 0);
			int minNumberOfArgs = boundFunction.MinNumberOfArgs;
			int maxNumberOfArgs = boundFunction.MaxNumberOfArgs;
			if (boundFunction.IsVarArg)
			{
				if (num < minNumberOfArgs)
				{
					ReportBindError(SR.FunctionRequiresAtLeastNumberOfArguments(functionName, minNumberOfArgs));
				}
			}
			else if (num < minNumberOfArgs || num > maxNumberOfArgs)
			{
				if (minNumberOfArgs == maxNumberOfArgs)
				{
					if (minNumberOfArgs == 0)
					{
						ReportBindError(SR.FunctionRequiresZeroArguments(functionName));
					}
					else
					{
						ReportBindError(SR.FunctionRequiresNumberOfArguments(functionName, minNumberOfArgs));
					}
				}
				else
				{
					ReportBindError(SR.FunctionRequiresRangeOfAruments(functionName, minNumberOfArgs, maxNumberOfArgs));
				}
			}
			if (!boundFunction.IsDatePartFunction || num <= 0)
			{
				foreach (SqlCodeObject child in Children)
				{
					child.Bind(bindingContext);
					base.BindingState |= child.BindingState;
				}
				return;
			}
			if (base.Arguments[0] is SqlScalarRefExpression sqlScalarRefExpression)
			{
				IDatePart datePart = null;
				if (sqlScalarRefExpression.MultipartIdentifier.Count == 1)
				{
					datePart = bindingContext.Batch.FindDatePart(sqlScalarRefExpression.MultipartIdentifier[0].Value);
				}
				if (datePart != null)
				{
					bindingContext.Batch.SetBoundObject(sqlScalarRefExpression.MultipartIdentifier[0], datePart);
				}
				else
				{
					sqlScalarRefExpression.ReportBindError(SR.NotRecognizedDatePartOption(sqlScalarRefExpression.ToString(), boundFunction.Name), ErrorFlags.UnresolvedReference);
				}
			}
			else
			{
				base.Arguments[0].Bind(bindingContext);
				if (!base.Arguments[0].HasBindingErrors)
				{
					base.Arguments[0].ReportBindError(SR.InvalidParameterOne(boundFunction.Name));
				}
			}
			for (int i = 1; i < base.Arguments.Count; i++)
			{
				base.Arguments[i].Bind(bindingContext);
			}
		}
		else
		{
			ReportBindError(SR.NotRecognizedFunctionName(functionName), ErrorFlags.UnresolvedReference);
			base.BindingState = BindingState.BindFailed;
		}
	}

	internal override void Unbind()
	{
		boundFunction = null;
		base.Unbind();
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref cursorLocation);
		if (sqlCodeObject != null)
		{
			return sqlCodeObject.GetQuickInfoText(cursorLocation, displayInfoProvider);
		}
		if (IsStar && TokenManager.IsSingleCharacterToken(TokenManager.GetId(cursorLocation.startTokenNumber)))
		{
			return null;
		}
		return (boundFunction != null) ? displayInfoProvider.GetDescription(boundFunction) : null;
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
			List<SqlCodeObject> sqlCodeObjectList = GetSqlCodeObjectList(base.Arguments);
			BuildMethodNameAndParams(methodNameAndParamLocations, nameEndPosition, sqlCodeObjectList);
			Location location = tokenManager.GetLocation(caretLocation.startTokenNumber);
			if (methodNameAndParamLocations.IsLocationInArgumentList(location))
			{
				result = methodNameAndParamLocations;
			}
		}
		return result;
	}

	internal static SqlBuiltinScalarFunctionCallExpression CreateStar(string functionName)
	{
		SqlAggregateFunctionCallExpression sqlAggregateFunctionCallExpression = SqlAggregateFunctionCallExpression.Create(functionName);
		if (sqlAggregateFunctionCallExpression != null)
		{
			return sqlAggregateFunctionCallExpression;
		}
		StringComparer invariantCultureIgnoreCase = StringComparer.InvariantCultureIgnoreCase;
		if (invariantCultureIgnoreCase.Equals(functionName, "CHECKSUM") || invariantCultureIgnoreCase.Equals(functionName, "BINARY_CHECKSUM"))
		{
			return new SqlBuiltinScalarFunctionCallExpression(functionName, isStar: true);
		}
		return null;
	}

	internal static SqlBuiltinScalarFunctionCallExpression Create(string functionName, IEnumerable<SqlScalarExpression> expression)
	{
		SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression = SqlAggregateFunctionCallExpression.Create(functionName, SqlSetQuantifier.None, expression);
		if (sqlBuiltinScalarFunctionCallExpression == null)
		{
			sqlBuiltinScalarFunctionCallExpression = new SqlBuiltinScalarFunctionCallExpression(functionName, expression);
		}
		return sqlBuiltinScalarFunctionCallExpression;
	}

	internal SqlBuiltinScalarFunctionCallExpression(string functionName, SqlScalarExpression argument)
		: base(argument)
	{
		this.functionName = functionName;
	}

	internal SqlBuiltinScalarFunctionCallExpression(string functionName, IEnumerable<SqlScalarExpression> arguments)
		: base(arguments)
	{
		this.functionName = functionName;
	}

	internal SqlBuiltinScalarFunctionCallExpression(string functionName, bool isStar)
	{
		this.functionName = functionName;
		this.isStar = isStar;
	}

	internal SqlBuiltinScalarFunctionCallExpression(string functionName)
		: this(functionName, isStar: false)
	{
	}
}
