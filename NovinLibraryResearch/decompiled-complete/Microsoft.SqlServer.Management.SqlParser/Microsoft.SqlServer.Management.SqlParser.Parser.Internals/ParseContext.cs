using System.Collections.Generic;
using System.Globalization;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class ParseContext : SqlCodeObject
{
	private abstract class SideEffect
	{
		internal abstract void Fix(ParseContext parserContext, TokenUpdateDiff diff);

		internal abstract void Execute(ParseContext parserContext);
	}

	private sealed class ParsePhaseContext
	{
		private class ErrorSideEffect : SideEffect
		{
			private readonly ErrorRecord error;

			public ErrorSideEffect(ErrorRecord error)
			{
				this.error = error;
			}

			internal override void Fix(ParseContext parseContext, TokenUpdateDiff diff)
			{
				error.FixError(diff);
			}

			internal override void Execute(ParseContext parseContext)
			{
				parseContext.parsePhase.baseErrorHandler.AddError(error);
			}

			public override string ToString()
			{
				return error.ToString();
			}
		}

		private class LastStatementSideEffect : SideEffect
		{
			private SqlStatement statement;

			public LastStatementSideEffect(SqlStatement statement)
			{
				this.statement = statement;
			}

			internal override void Fix(ParseContext parseContext, TokenUpdateDiff diff)
			{
			}

			internal override void Execute(ParseContext parseContext)
			{
				parseContext.parsePhase.lastStatements.Add(statement);
			}

			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "Last statement: {0}", statement.Position);
			}
		}

		private struct StackItem
		{
			public SideEffectSpan sideEffectSpan;

			public int parserStackSize;

			public StackItem(SideEffectSpan sideEffectSpan, int parserStackSize)
			{
				this.sideEffectSpan = sideEffectSpan;
				this.parserStackSize = parserStackSize;
			}
		}

		private class SideEffectErrorHandler : ErrorHandler
		{
			private readonly ParsePhaseContext parseDriver;

			public SideEffectErrorHandler(ParsePhaseContext parseDriver)
			{
				this.parseDriver = parseDriver;
			}

			public override void AddError(ErrorRecord error)
			{
				SideEffect sideEffect = new ErrorSideEffect(error);
				if (parseDriver.sideEffectMode)
				{
					parseDriver.ExecuteAndRegisterSideEffect(sideEffect);
				}
				else
				{
					parseDriver.baseErrorHandler.AddError(error);
				}
			}
		}

		private readonly ParseContext parseContext;

		private readonly ShiftReduceParser currentParser;

		private readonly List<SqlStatement> lastStatements;

		private readonly ErrorHandler baseErrorHandler;

		private readonly ErrorHandler sideEffectErrorHandler;

		private readonly SqlScript prevScript;

		private readonly ParseContext prevContext;

		private readonly TokenUpdateDiff diff;

		private readonly ParserStack<StackItem> spanStack;

		private bool sideEffectMode;

		public ParseContext PrevContext => prevContext;

		public TokenUpdateDiff Diff => diff;

		public ErrorHandler ErrorHandler
		{
			get
			{
				if (!sideEffectMode)
				{
					return baseErrorHandler;
				}
				return sideEffectErrorHandler;
			}
		}

		public ParsePhaseContext(ParseContext parseContext, ErrorHandler errorHandler, ShiftReduceParser currentParser, SqlScript prevScript)
		{
			this.parseContext = parseContext;
			this.currentParser = currentParser;
			this.prevScript = prevScript;
			baseErrorHandler = errorHandler;
			prevContext = ((this.prevScript != null) ? this.prevScript.ParseContext : null);
			lastStatements = new List<SqlStatement>();
			if (PrevContext != null && this.parseContext.parseOptions.Equals(PrevContext.parseOptions))
			{
				diff = TokenManager.CalculateTokenUpdateDiff(PrevContext.firstLevelTokenManager, this.parseContext.firstLevelTokenManager);
			}
			if (this.parseContext.enableSideEffectsTrace)
			{
				sideEffectErrorHandler = new SideEffectErrorHandler(this);
				spanStack = new ParserStack<StackItem>();
			}
		}

		public void SetSideEffectMode(bool sideEffectMode)
		{
			this.sideEffectMode = sideEffectMode;
		}

		public void AddLastStatement(SqlStatement statement)
		{
			if (sideEffectMode)
			{
				SideEffect sideEffect = new LastStatementSideEffect(statement);
				ExecuteAndRegisterSideEffect(sideEffect);
			}
			else
			{
				lastStatements.Add(statement);
			}
		}

		public void PostParseErrorChecking()
		{
			ValidateLastStatements();
		}

		private void ValidateLastStatements()
		{
			foreach (SqlStatement lastStatement in lastStatements)
			{
				if (!(lastStatement.Parent is SqlBatch sqlBatch) || sqlBatch.Statements.Count != 1)
				{
					string statementPhrase = lastStatement.StatementPhrase;
					ErrorHandler.AddError(SR.MustBeOnlyStatementInBatch(statementPhrase), ErrorType.ParseError, ErrorFlags.None, lastStatement);
				}
			}
		}

		public bool IsStatementReusable(SqlStatement sqlStatement)
		{
			if (!IsStatementPositionBeforeDiff(sqlStatement))
			{
				return IsStatementPositionAfterDiff(sqlStatement);
			}
			return true;
		}

		public void ReuseStatement(SqlStatement sqlStatement)
		{
			ReuseSideEffects(sqlStatement);
			if (IsStatementPositionAfterDiff(sqlStatement))
			{
				sqlStatement.ReuseData.LocalPositionOffset += diff.NewEndIndex - diff.OldEndIndex;
			}
			sqlStatement.SetParentParseContext(parseContext);
		}

		private bool IsStatementPositionBeforeDiff(SqlStatement sqlStatement)
		{
			SqlStatementReuseData reuseData = sqlStatement.ReuseData;
			if (reuseData.AfterScannerStateRelative.TokenIndex + reuseData.LocalPositionOffset <= diff.StartIndex)
			{
				return reuseData.LookAheadTokenIndexRelative + reuseData.LocalPositionOffset < diff.StartIndex;
			}
			return false;
		}

		private bool IsStatementPositionAfterDiff(SqlStatement sqlStatement)
		{
			SqlStatementReuseData reuseData = sqlStatement.ReuseData;
			return reuseData.BeforeScannerStateRelative.TokenIndex + reuseData.LocalPositionOffset >= diff.OldEndIndex;
		}

		public SideEffectSpan RegisterReduction()
		{
			int stackSize = currentParser.StackSize;
			SideEffectSpan sideEffectSpan = null;
			int num = -1;
			while (!spanStack.IsEmpty() && spanStack.Top().parserStackSize >= stackSize)
			{
				SideEffectSpan sideEffectSpan2 = spanStack.Pop().sideEffectSpan;
				if (sideEffectSpan == null)
				{
					sideEffectSpan = sideEffectSpan2;
					num = sideEffectSpan.BeginIndex;
				}
				else
				{
					num = sideEffectSpan2.BeginIndex;
				}
			}
			if (sideEffectSpan != null)
			{
				if (num != sideEffectSpan.BeginIndex)
				{
					sideEffectSpan = new SideEffectSpan(num, sideEffectSpan.EndIndex);
				}
				spanStack.Push(new StackItem(sideEffectSpan, stackSize));
			}
			return sideEffectSpan;
		}

		public void BeginNewBatchParsing()
		{
			spanStack.Clear();
		}

		public void ReuseSideEffects(SqlStatement sqlStatement)
		{
			SqlStatementReuseData reuseData = sqlStatement.ReuseData;
			if (reuseData.SideEffectSpan != null)
			{
				int num = parseContext.sideEffects.Count - reuseData.SideEffectSpan.BeginIndex;
				for (int i = reuseData.SideEffectSpan.BeginIndex; i < reuseData.SideEffectSpan.EndIndex; i++)
				{
					SideEffect sideEffect = PrevContext.sideEffects[i];
					FixSideEffect(sideEffect);
					parseContext.sideEffects.Add(sideEffect);
					ExecuteSideEffect(sideEffect);
				}
				if (num != 0)
				{
					FixReusedSideEffectSpan(sqlStatement, num);
				}
				int stackSize = currentParser.StackSize;
				spanStack.Push(new StackItem(reuseData.SideEffectSpan, stackSize));
			}
		}

		private void ExecuteAndRegisterSideEffect(SideEffect sideEffect)
		{
			if (sideEffectMode)
			{
				parseContext.sideEffects.Add(sideEffect);
				int stackSize = currentParser.StackSize;
				spanStack.Push(new StackItem(new SideEffectSpan(parseContext.sideEffects.Count - 1), stackSize));
			}
			ExecuteSideEffect(sideEffect);
		}

		private void ExecuteSideEffect(SideEffect sideEffect)
		{
			sideEffect.Execute(parseContext);
		}

		private void FixSideEffect(SideEffect sideEffect)
		{
			sideEffect.Fix(parseContext, diff);
		}

		private void FixReusedSideEffectSpan(SqlCodeObject stmtLevelObj, int delta)
		{
			SqlStatement asStatement = stmtLevelObj.AsStatement;
			SqlStatementReuseData sqlStatementReuseData = asStatement?.ReuseData;
			if (asStatement != null && sqlStatementReuseData != null && sqlStatementReuseData.SideEffectSpan != null)
			{
				sqlStatementReuseData.SideEffectSpan = sqlStatementReuseData.SideEffectSpan.ShiftBy(delta);
			}
			if (asStatement != null && (sqlStatementReuseData == null || sqlStatementReuseData.SideEffectSpan == null))
			{
				return;
			}
			foreach (SqlCodeObject child in stmtLevelObj.Children)
			{
				if (child.IsStatementLevelObject)
				{
					FixReusedSideEffectSpan(child, delta);
				}
			}
		}
	}

	private sealed class BindPhaseContext
	{
		private readonly ParseContext parseContext;

		private readonly ErrorHandler errorHandler;

		public ErrorHandler ErrorHandler => errorHandler;

		public BindPhaseContext(ParseContext parseContext, ErrorHandler errorHandler)
		{
			this.parseContext = parseContext;
			this.errorHandler = errorHandler;
		}
	}

	private readonly ParseOptions parseOptions;

	private readonly TokenManager firstLevelTokenManager;

	private readonly TokenManager tokenManager;

	private readonly bool enableSideEffectsTrace;

	private readonly List<SideEffect> sideEffects;

	private ParseContextState currentState;

	private ParsePhaseContext parsePhase;

	private BindPhaseContext bindPhase;

	internal bool IsErrorRecoveryEnabled => parseOptions.IsErrorRecoveryEnabled;

	internal override bool IsVisitable => false;

	public override SqlStatement Statement => null;

	public override TokenManager TokenManager => tokenManager;

	public ParseContextState CurrentState => currentState;

	public TokenUpdateDiff Diff => parsePhase.Diff;

	public ParseContext PrevContext => parsePhase.PrevContext;

	public ErrorHandler ErrorHandler => currentState switch
	{
		ParseContextState.Parsing => parsePhase.ErrorHandler, 
		ParseContextState.Binding => bindPhase.ErrorHandler, 
		_ => null, 
	};

	public ParseContext(ParserScanner scanner, ParseOptions parseOptions, bool enableSideEffectsTrace)
	{
		this.parseOptions = parseOptions;
		firstLevelTokenManager = scanner.FirstLevelTokenManager;
		tokenManager = scanner.CreateTokenManager();
		this.enableSideEffectsTrace = enableSideEffectsTrace;
		if (enableSideEffectsTrace)
		{
			sideEffects = new List<SideEffect>();
		}
		currentState = ParseContextState.Initial;
	}

	public void BeginParse(ErrorHandler errorHandler, ShiftReduceParser currentParser, SqlScript prevScript)
	{
		currentState = ParseContextState.Parsing;
		parsePhase = new ParsePhaseContext(this, errorHandler, currentParser, prevScript);
	}

	public void EndParse()
	{
		currentState = ParseContextState.ParseCompleted;
		parsePhase = null;
	}

	public void BeginBind(ErrorHandler errorHandler)
	{
		currentState = ParseContextState.Binding;
		bindPhase = new BindPhaseContext(this, errorHandler);
	}

	public void EndBind()
	{
		currentState = ParseContextState.BindCompleted;
		bindPhase = null;
	}

	internal void ResetBind()
	{
		currentState = ParseContextState.ParseCompleted;
	}

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		return default(T);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		return default(T);
	}

	public void SetSideEffectMode(bool sideEffectMode)
	{
		parsePhase.SetSideEffectMode(sideEffectMode);
	}

	public void AddLastStatement(SqlStatement statement)
	{
		parsePhase.AddLastStatement(statement);
	}

	public void PostParseErrorChecking()
	{
		parsePhase.PostParseErrorChecking();
	}

	public SideEffectSpan RegisterReduction()
	{
		return parsePhase.RegisterReduction();
	}

	public void BeginNewBatchParsing()
	{
		parsePhase.BeginNewBatchParsing();
	}

	public bool IsStatementReusable(SqlStatement sqlStatement)
	{
		return parsePhase.IsStatementReusable(sqlStatement);
	}

	public void ReuseStatement(SqlStatement sqlStatement)
	{
		parsePhase.ReuseStatement(sqlStatement);
	}
}
