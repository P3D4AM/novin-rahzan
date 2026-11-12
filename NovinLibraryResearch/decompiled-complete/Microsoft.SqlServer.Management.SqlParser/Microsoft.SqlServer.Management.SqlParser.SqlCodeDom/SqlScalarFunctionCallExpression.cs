using System.Collections.Generic;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlScalarFunctionCallExpression : SqlScalarExpression
{
	private SqlScalarExpressionCollection arguments;

	public override IScalar BoundScalar => BoundFunction;

	public abstract IScalarFunction BoundFunction { get; }

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.Function;

	internal override bool IsConstantExpression => false;

	public SqlScalarExpressionCollection Arguments => arguments;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (arguments == null)
			{
				yield break;
			}
			foreach (SqlScalarExpression argument in arguments)
			{
				yield return argument;
			}
		}
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		bool flag = BoundFunction is IBuiltInFunction builtInFunction && builtInFunction.IsDatePartFunction;
		if (sqlCodeObject != null)
		{
			if (flag && arguments != null && sqlCodeObject == arguments[0])
			{
				completionContext = CompletionContext.Datepart;
			}
			result = sqlCodeObject.GetCompletionList(caretLocation, completionContext);
		}
		else if ((arguments == null || arguments.Count == 0) && IsCaretBetweenEmptyParenthesis(caretLocation))
		{
			if (flag)
			{
				completionContext = CompletionContext.Datepart;
			}
			result = RetrieveCompletionListItems(completionContext);
		}
		return result;
	}

	internal override MethodHelpText GetMyMethodHelpText(LexLocation caretLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		MethodHelpText result = null;
		IScalarFunction boundFunction = BoundFunction;
		if (boundFunction != null)
		{
			bool shouldShowParentheses = true;
			bool isVarArgs = false;
			SqlBuiltinScalarFunctionCallExpression sqlBuiltinScalarFunctionCallExpression = this as SqlBuiltinScalarFunctionCallExpression;
			if (sqlBuiltinScalarFunctionCallExpression != null)
			{
				isVarArgs = sqlBuiltinScalarFunctionCallExpression.boundFunction.IsVarArg;
			}
			string methodDescription = null;
			IMetadataOrderedCollection<IParameter> parameters = boundFunction.Parameters;
			if (sqlBuiltinScalarFunctionCallExpression.BoundFunction.Name.Equals("TRIM"))
			{
				methodDescription = "Syntax: TRIM ([characters FROM] expression)";
				parameters = Collection<IParameter>.ConvertArray(new IParameter[1] { boundFunction.Parameters[1] }, CollationInfo.OrdinalIgnoreCase);
			}
			string methodType = BuildParameterHelpMethodTypeString(boundFunction.DataType);
			result = BuildMethodHelpText(boundFunction, parameters, methodDescription, methodType, shouldShowParentheses, isVarArgs, displayInfoProvider);
		}
		return result;
	}

	internal SqlScalarFunctionCallExpression()
	{
	}

	internal SqlScalarFunctionCallExpression(SqlScalarExpression argument)
	{
		arguments = new SqlScalarExpressionCollection(argument);
	}

	internal SqlScalarFunctionCallExpression(IEnumerable<SqlScalarExpression> arguments)
	{
		if (arguments != null)
		{
			this.arguments = new SqlScalarExpressionCollection(arguments);
		}
	}
}
