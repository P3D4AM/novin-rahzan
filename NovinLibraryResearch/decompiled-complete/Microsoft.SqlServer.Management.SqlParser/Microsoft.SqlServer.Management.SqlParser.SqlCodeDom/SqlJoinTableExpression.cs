using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlJoinTableExpression : SqlTableExpression
{
	private enum JoinExprElement : sbyte
	{
		None,
		Left,
		Right,
		OnCondition
	}

	private class JoinExprBindingContext : LocalBindingContext
	{
		private readonly LocalBindingContext m_baseBindingContext;

		private readonly TabularCollection m_leftTableExpressions;

		private readonly TabularCollection m_rightTableExpressions;

		private readonly bool m_isApplyOperator;

		private JoinExprElement m_currentExprElement;

		public override IMetadataCollection<ITabular> LocalTableExpressions
		{
			get
			{
				IMetadataCollection<ITabular> metadataCollection = m_baseBindingContext.LocalTableExpressions;
				switch (m_currentExprElement)
				{
				case JoinExprElement.Right:
					if (m_isApplyOperator)
					{
						metadataCollection = Collection<ITabular>.Merge(metadataCollection, m_leftTableExpressions);
					}
					break;
				case JoinExprElement.OnCondition:
					metadataCollection = Collection<ITabular>.Merge(metadataCollection, m_leftTableExpressions, m_rightTableExpressions);
					break;
				}
				return metadataCollection;
			}
		}

		public JoinExprBindingContext(LocalBindingContext baseLocalBindingContext, bool isApplyOperator)
			: base(baseLocalBindingContext)
		{
			m_baseBindingContext = baseLocalBindingContext;
			CollationInfo databaseCollation = base.DatabaseCollation;
			m_leftTableExpressions = new TabularCollection(8, databaseCollation);
			m_rightTableExpressions = new TabularCollection(8, databaseCollation);
			m_isApplyOperator = isApplyOperator;
		}

		public void SetCurrentJoinExprElement(JoinExprElement exprElement)
		{
			m_currentExprElement = exprElement;
		}

		public override ITabular AddLocalTableExpression(ITabular expression)
		{
			ITabular tabular = m_baseBindingContext.AddLocalTableExpression(expression);
			if (tabular == null)
			{
				switch (m_currentExprElement)
				{
				case JoinExprElement.Left:
					m_leftTableExpressions.Add(expression);
					break;
				case JoinExprElement.Right:
					m_rightTableExpressions.Add(expression);
					break;
				}
			}
			return tabular;
		}
	}

	private JoinExprBindingContext bindingContext;

	private readonly SqlJoinOperatorType joinOperator;

	private readonly SqlTableExpression left;

	private readonly SqlTableExpression right;

	internal override IBindingContext BindingContext => bindingContext;

	internal override TableExpressionName ExpressionName => default(TableExpressionName);

	public SqlJoinOperatorType JoinOperator => joinOperator;

	public SqlTableExpression Left => left;

	public SqlTableExpression Right => right;

	internal override void Bind(IBindingContext bindingContext)
	{
		bool isApplyOperator = joinOperator == SqlJoinOperatorType.CrossApply || joinOperator == SqlJoinOperatorType.OuterApply;
		JoinExprBindingContext joinExprBindingContext = (this.bindingContext = new JoinExprBindingContext(bindingContext.AsLocalBindingContext, isApplyOperator));
		foreach (SqlCodeObject child in Children)
		{
			if (child == left)
			{
				joinExprBindingContext.SetCurrentJoinExprElement(JoinExprElement.Left);
			}
			else if (child == right)
			{
				joinExprBindingContext.SetCurrentJoinExprElement(JoinExprElement.Right);
			}
			else
			{
				joinExprBindingContext.SetCurrentJoinExprElement(JoinExprElement.OnCondition);
			}
			child.Bind(joinExprBindingContext);
			base.BindingState |= child.BindingState;
		}
		joinExprBindingContext.SetCurrentJoinExprElement(JoinExprElement.None);
	}

	internal override SqlTableExpression FindSiblingTableExpressionBoundTo(SqlTableExpression tableExpression, ITabular boundTable)
	{
		SqlTableExpression sqlTableExpression = null;
		if (tableExpression == right)
		{
			sqlTableExpression = left.FindTableExpressionBoundTo(boundTable);
		}
		if (sqlTableExpression == null)
		{
			sqlTableExpression = base.Parent.FindSiblingTableExpressionBoundTo(this, boundTable);
		}
		return sqlTableExpression;
	}

	internal override SqlTableExpression FindTableExpressionBoundTo(ITabular boundTable)
	{
		SqlTableExpression sqlTableExpression = left.FindTableExpressionBoundTo(boundTable);
		if (sqlTableExpression == null)
		{
			sqlTableExpression = right.FindTableExpressionBoundTo(boundTable);
		}
		return sqlTableExpression;
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (joinOperator == SqlJoinOperatorType.CrossApply)
		{
			_ = 1;
		}
		else
			_ = joinOperator == SqlJoinOperatorType.OuterApply;
		if (sqlCodeObject == left)
		{
			bindingContext.SetCurrentJoinExprElement(JoinExprElement.Left);
			result = sqlCodeObject.GetCompletionList(caretLocation, completionContext);
		}
		else if (sqlCodeObject == right)
		{
			bindingContext.SetCurrentJoinExprElement(JoinExprElement.Right);
			result = sqlCodeObject.GetCompletionList(caretLocation, completionContext);
		}
		else if (sqlCodeObject != null)
		{
			bindingContext.SetCurrentJoinExprElement(JoinExprElement.OnCondition);
			result = sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.SelectScalar);
		}
		return result;
	}

	internal static bool IsUnqualifiedJoinOperator(SqlJoinOperatorType joinOperator)
	{
		if (joinOperator != 0 && joinOperator != SqlJoinOperatorType.CrossApply)
		{
			return joinOperator == SqlJoinOperatorType.OuterApply;
		}
		return true;
	}

	internal static bool IsQualifiedJoinOperator(SqlJoinOperatorType joinOperator)
	{
		return !IsUnqualifiedJoinOperator(joinOperator);
	}

	internal SqlJoinTableExpression(SqlTableExpression left, SqlJoinOperatorType joinOperator, SqlTableExpression right)
	{
		this.left = left;
		this.joinOperator = joinOperator;
		this.right = right;
	}
}
