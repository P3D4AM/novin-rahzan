using System;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal class ReusableStatementsCollection
{
	private readonly ParserStack<SqlStatement> statementStack;

	private readonly ParseContext parseContext;

	private readonly IParserTestSink parserTestSink;

	public SqlStatement Current
	{
		get
		{
			if (!statementStack.IsEmpty())
			{
				return statementStack.Top();
			}
			return null;
		}
	}

	public ReusableStatementsCollection(SqlScript prevScript, ParseContext parseContext, IParserTestSink parserTestSink)
	{
		statementStack = new ParserStack<SqlStatement>();
		this.parseContext = parseContext;
		this.parserTestSink = parserTestSink;
		foreach (SqlBatch batch in prevScript.Batches)
		{
			foreach (SqlStatement statement in batch.Statements)
			{
				AddReusableStatements(statement);
			}
		}
		Array.Reverse((Array)statementStack.array, 0, statementStack.top);
	}

	private void AddReusableStatements(SqlCodeObject stmtLevelObj)
	{
		SqlStatement asStatement = stmtLevelObj.AsStatement;
		bool flag = false;
		if (asStatement != null && asStatement.ReuseData != null)
		{
			asStatement.DetachFromTree();
			if (parseContext.IsStatementReusable(asStatement))
			{
				flag = true;
			}
		}
		if (flag)
		{
			statementStack.Push(asStatement);
			return;
		}
		if (parserTestSink != null && asStatement != null)
		{
			parserTestSink.StatementEvent(asStatement, StatementEventType.NotReusable);
		}
		foreach (SqlCodeObject child in stmtLevelObj.Children)
		{
			if (child.IsStatementLevelObject)
			{
				AddReusableStatements(child);
			}
		}
	}

	public void RemoveCurrent()
	{
		statementStack.Pop();
	}

	public void SplitCurrent()
	{
		SqlStatement current = Current;
		RemoveCurrent();
		int top = statementStack.top;
		foreach (SqlCodeObject child in current.Children)
		{
			if (child.IsStatementLevelObject)
			{
				AddReusableStatements(child);
			}
		}
		Array.Reverse((Array)statementStack.array, top, statementStack.top - top);
	}
}
