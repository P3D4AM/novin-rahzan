using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlDmlSpecification : SqlCodeObject
{
	private DmlSpecificationBindingContext dmlBindingContext;

	private readonly SqlTopSpecification topSpecification;

	private readonly SqlTableExpression target;

	private readonly SqlOutputIntoClause outputIntoClause;

	private readonly SqlOutputClause outputClause;

	internal sealed override IBindingContext BindingContext => dmlBindingContext;

	public SqlTopSpecification TopSpecification => topSpecification;

	public SqlTableExpression Target => target;

	public SqlOutputIntoClause OutputIntoClause => outputIntoClause;

	public SqlOutputClause OutputClause => outputClause;

	internal void BindDmlSpecification(DmlSpecificationBindingContext bindingContext)
	{
		dmlBindingContext = bindingContext;
		target.BindDmlTarget(bindingContext);
		base.BindingState |= Target.BindingState;
		if (TopSpecification != null)
		{
			TopSpecification.Bind(bindingContext.Batch);
			base.BindingState |= TopSpecification.BindingState;
		}
		if (OutputIntoClause != null)
		{
			LocalBindingContext bindingContext2 = bindingContext.CreateOutputClauseBindingContext();
			OutputIntoClause.Bind(bindingContext2);
			base.BindingState |= OutputIntoClause.BindingState;
		}
		if (OutputClause != null)
		{
			LocalBindingContext bindingContext3 = bindingContext.CreateOutputClauseBindingContext();
			OutputClause.Bind(bindingContext3);
			base.BindingState |= OutputClause.BindingState;
		}
	}

	internal sealed override void Unbind()
	{
		base.Unbind();
		dmlBindingContext = null;
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject != null)
		{
			completionContext = ((sqlCodeObject == Target) ? CompletionContext.SelectTabular : ((sqlCodeObject != TopSpecification) ? FindCompletionContextForCodeObject(sqlCodeObject) : CompletionContext.Scalar));
			result = sqlCodeObject.GetCompletionList(caretLocation, completionContext);
		}
		return result;
	}

	internal virtual CompletionContext FindCompletionContextForCodeObject(SqlCodeObject codeObject)
	{
		return CompletionContext.None;
	}

	internal SqlDmlSpecification(SqlTopSpecification topSpecification, DmlTarget dmlTarget, List<SqlOutputClause> outputClauses)
	{
		this.topSpecification = topSpecification;
		target = dmlTarget.TableExpression;
		outputClause = null;
		outputIntoClause = null;
		if (outputClauses != null)
		{
			if (outputClauses.Count == 1)
			{
				outputClause = outputClauses[0];
				return;
			}
			outputIntoClause = outputClauses[0].AsSqlOutputIntoClause;
			outputClause = outputClauses[1];
		}
	}
}
