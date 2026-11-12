using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlConditionalStatement : SqlStatement
{
	private SqlBooleanExpression condition;

	private SqlStatement trueStatement;

	internal sealed override bool IsConditionalStatement => true;

	public SqlBooleanExpression Condition => condition;

	public SqlStatement TrueStatement => trueStatement;

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject == condition)
		{
			completionContext = CompletionContext.Scalar;
		}
		return sqlCodeObject?.GetCompletionList(caretLocation, completionContext);
	}

	internal SqlConditionalStatement(SqlBooleanExpression condition, SqlStatement trueStatement)
	{
		this.condition = condition;
		this.trueStatement = trueStatement;
	}
}
