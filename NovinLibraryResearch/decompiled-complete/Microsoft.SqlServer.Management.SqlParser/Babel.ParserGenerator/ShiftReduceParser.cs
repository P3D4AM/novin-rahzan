using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.SqlServer.Management.SqlParser;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Babel.ParserGenerator;

internal abstract class ShiftReduceParser
{
	private enum ErrorRecoveryMode
	{
		Normal,
		RequireErrorObject,
		Discarding
	}

	private enum ErrorRecoveryActionType
	{
		None,
		DefaultReduce,
		Reduce,
		Shift,
		ShiftError,
		Discard,
		PopState
	}

	private static class ErrorRecoveryUtils
	{
		public static bool RecoverFromError(ShiftReduceParser parser)
		{
			if (!parser.scanner.IsTokenRead)
			{
				parser.scanner.ReadToken();
			}
			if (parser.ParserTrace)
			{
				Console.Error.WriteLine("searching for an error recovery state, current state stack: {0}", parser.state_stack.ToString());
				Console.Error.WriteLine("Next token is {0}", TerminalToString(parser.scanner.Token));
			}
			ErrorRecoveryStep errorRecoveryStep = FindErrorRecoveryPath(parser);
			if (errorRecoveryStep == null)
			{
				if (parser.ParserTrace)
				{
					Console.Error.WriteLine("No error recovery path.");
				}
				return false;
			}
			while (errorRecoveryStep != null)
			{
				switch (errorRecoveryStep.ActionType)
				{
				case ErrorRecoveryActionType.PopState:
					parser.PopState();
					break;
				case ErrorRecoveryActionType.DefaultReduce:
				case ErrorRecoveryActionType.Reduce:
					parser.Reduce(-errorRecoveryStep.Action);
					break;
				case ErrorRecoveryActionType.Shift:
					if (IsMissingToken(errorRecoveryStep.Token))
					{
						parser.ShiftMissingToken(errorRecoveryStep.Token);
						break;
					}
					if (errorRecoveryStep.Token == 129)
					{
						parser.ShiftNotEmptyToken();
						break;
					}
					return false;
				case ErrorRecoveryActionType.ShiftError:
				{
					for (int i = 0; i < errorRecoveryStep.DiscardCount; i++)
					{
						if (parser.ParserTrace)
						{
							Console.Error.WriteLine("Error: Discarding {0}", TerminalToString(parser.scanner.Token));
						}
						parser.DiscardingInvalidToken();
					}
					parser.ShiftErrorToken(errorRecoveryStep.Token);
					break;
				}
				case ErrorRecoveryActionType.Discard:
					return false;
				case ErrorRecoveryActionType.None:
					return false;
				default:
					return false;
				}
				errorRecoveryStep = errorRecoveryStep.Next;
			}
			return true;
		}

		private static ErrorRecoveryStep FindErrorRecoveryPath(ShiftReduceParser parser)
		{
			PriorityQueue<ErrorRecoveryState> priorityQueue = new PriorityQueue<ErrorRecoveryState>(ErrorRecoveryState.ShortestIsGreater);
			ErrorRecoveryState errorRecoveryState = new ErrorRecoveryState(parser);
			LexLocation errorLocation = null;
			while (errorRecoveryState.StateStack.Count > 0)
			{
				priorityQueue.Enqueue(errorRecoveryState);
				errorRecoveryState = errorRecoveryState.PopState(ref errorLocation, parser.location_stack);
			}
			ErrorRecoveryStep errorRecoveryStep = null;
			while (priorityQueue.Count > 0 && errorRecoveryStep == null)
			{
				ErrorRecoveryState errorRecoveryState2 = priorityQueue.Dequeue();
				if (errorRecoveryState2.ActionCount > ErrorRecoveryMaxActionCount && !errorRecoveryState2.AllowUnlimitedPath)
				{
					if (parser.ParserTrace)
					{
						Console.Error.WriteLine("Error recovery search apth is too long, aborting.");
					}
					continue;
				}
				ErrorRecoveryState errorRecoveryState3 = null;
				if (errorRecoveryState2.SearchMode != ErrorRecoveryMode.RequireErrorObject)
				{
					errorRecoveryState3 = errorRecoveryState2.SimulateShiftToken();
					if (errorRecoveryState3 != null)
					{
						errorRecoveryStep = errorRecoveryState3.GetErrorRecoverySteps();
						break;
					}
				}
				if (errorRecoveryState2.SearchMode != ErrorRecoveryMode.Discarding || errorRecoveryState2.IsAtEOF)
				{
					if (errorRecoveryState2.DiscardsCount <= 1)
					{
						errorRecoveryState3 = errorRecoveryState2.SimulateNextToken(131);
						if (errorRecoveryState3 != null)
						{
							if (errorRecoveryState2.SearchMode == ErrorRecoveryMode.Discarding)
							{
								errorRecoveryState3.RecoveryCost += 10;
							}
							priorityQueue.Enqueue(errorRecoveryState3);
						}
					}
					errorRecoveryState3 = errorRecoveryState2.SimulateNextToken(127);
					if (errorRecoveryState3 != null)
					{
						if (errorRecoveryState2.SearchMode == ErrorRecoveryMode.Discarding)
						{
							errorRecoveryState3.RecoveryCost += 10;
						}
						priorityQueue.Enqueue(errorRecoveryState3);
					}
					errorRecoveryState3 = errorRecoveryState2.SimulateNextToken(132);
					if (errorRecoveryState3 != null)
					{
						if (errorRecoveryState2.SearchMode == ErrorRecoveryMode.Discarding)
						{
							errorRecoveryState3.RecoveryCost += 10;
						}
						priorityQueue.Enqueue(errorRecoveryState3);
					}
				}
				if (errorRecoveryState2.SearchMode != ErrorRecoveryMode.RequireErrorObject)
				{
					State state = errorRecoveryState2.CurrentState();
					if (state.parser_table == null || 17 <= state.parser_table.Count)
					{
						for (int i = 959; i < 976; i++)
						{
							errorRecoveryState3 = errorRecoveryState2.SimulateNextToken(i);
							if (errorRecoveryState3 != null)
							{
								priorityQueue.Enqueue(errorRecoveryState3);
							}
						}
					}
					else
					{
						foreach (int key in state.parser_table.Keys)
						{
							if (IsMissingToken(key))
							{
								errorRecoveryState3 = errorRecoveryState2.SimulateNextToken(key);
								if (errorRecoveryState3 != null)
								{
									priorityQueue.Enqueue(errorRecoveryState3);
								}
							}
						}
					}
				}
				if (errorRecoveryState2.SearchMode != ErrorRecoveryMode.Discarding)
				{
					continue;
				}
				bool flag = false;
				if (errorRecoveryState2.DiscardsCount > 0 || errorRecoveryState2.IsAtEOF)
				{
					errorRecoveryState3 = errorRecoveryState2.SimulateNextToken(129);
					if (errorRecoveryState3 != null)
					{
						if (errorRecoveryState2.DiscardsCount == 0)
						{
							errorRecoveryState3.RecoveryCost += 10;
						}
						priorityQueue.Enqueue(errorRecoveryState3);
						flag = true;
					}
				}
				if (!flag && !errorRecoveryState2.IsAtEOF && errorRecoveryState2.DiscardsRemaining > 0)
				{
					priorityQueue.Enqueue(errorRecoveryState2.Discard());
				}
			}
			return errorRecoveryStep;
		}
	}

	private class ErrorRecoveryStep
	{
		public ErrorRecoveryActionType ActionType;

		public int Token;

		public int Action;

		public ErrorRecoveryStep Next;

		public int DiscardCount;

		private ErrorRecoveryStep(ErrorRecoveryActionType actionType, int token, int action)
		{
			ActionType = actionType;
			Token = token;
			Action = action;
		}

		public static ErrorRecoveryStep DefaultReduce(int action)
		{
			return new ErrorRecoveryStep(ErrorRecoveryActionType.DefaultReduce, 0, action);
		}

		public static ErrorRecoveryStep ReduceOnToken(int token, int action)
		{
			return new ErrorRecoveryStep(ErrorRecoveryActionType.Reduce, token, action);
		}

		public static ErrorRecoveryStep ShiftOnToken(int token, int action)
		{
			return new ErrorRecoveryStep(ErrorRecoveryActionType.Shift, token, action);
		}

		public ErrorRecoveryStep(ErrorRecoveryStep value)
		{
			Token = value.Token;
			Action = value.Action;
			ActionType = value.ActionType;
			Next = value.Next;
		}

		internal static ErrorRecoveryStep Discard(int token)
		{
			return new ErrorRecoveryStep(ErrorRecoveryActionType.Discard, token, 0);
		}

		internal static ErrorRecoveryStep PopState()
		{
			return new ErrorRecoveryStep(ErrorRecoveryActionType.PopState, 0, 0);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("ActionType: {0},", ActionType);
			if (ActionType == ErrorRecoveryActionType.ShiftError)
			{
				stringBuilder.AppendFormat("DiscardCount: {0},", DiscardCount);
			}
			if (Token != 0)
			{
				stringBuilder.AppendFormat("Token: {0},", (Tokens)Token);
			}
			if (Next != null)
			{
				stringBuilder.Append(Next.ToString());
			}
			return stringBuilder.ToString();
		}
	}

	private class ErrorRecoveryState
	{
		private class ShortestIsGreaterComparer : IComparer<ErrorRecoveryState>
		{
			public int Compare(ErrorRecoveryState x, ErrorRecoveryState y)
			{
				if (x == y)
				{
					return 0;
				}
				if (x == null)
				{
					return 1;
				}
				if (y == null)
				{
					return -1;
				}
				return y.RecoveryCost - x.RecoveryCost;
			}
		}

		public static readonly IComparer<ErrorRecoveryState> ShortestIsGreater = new ShortestIsGreaterComparer();

		private readonly State[] states;

		private readonly Rule[] rules;

		private readonly ParserConfig parserConfig;

		public readonly ErrorRecoveryStep History;

		public readonly List<State> StateStack;

		public ErrorRecoveryMode SearchMode;

		public readonly StateOnlyParser StateOnlyParser;

		public int ActionCount;

		public int RecoveryCost;

		public bool AllowUnlimitedPath;

		public int DiscardsRemaining;

		public int DiscardsCount;

		public bool IsAtEOF => StateOnlyParser.scanner.Token == 128;

		internal ErrorRecoveryState(ShiftReduceParser baseParser)
		{
			rules = baseParser.rules;
			states = baseParser.states;
			parserConfig = baseParser.parserConfig;
			History = null;
			ActionCount = 0;
			RecoveryCost = 0;
			StateStack = new List<State>(baseParser.state_stack.top);
			SearchMode = ErrorRecoveryMode.Normal;
			StateOnlyParser = baseParser.CreateStateOnlyParser();
			DiscardsRemaining = 0;
			DiscardsCount = 0;
			AllowUnlimitedPath = false;
			for (int i = 0; i < baseParser.state_stack.top; i++)
			{
				StateStack.Add(baseParser.state_stack.array[i]);
			}
		}

		private ErrorRecoveryState(ErrorRecoveryStep step, ErrorRecoveryState parent)
		{
			rules = parent.rules;
			states = parent.states;
			parserConfig = parent.parserConfig;
			StateStack = new List<State>(parent.StateStack);
			History = step;
			History.Next = parent.History;
			ActionCount = parent.ActionCount;
			RecoveryCost = parent.RecoveryCost;
			SearchMode = parent.SearchMode;
			StateOnlyParser = parent.StateOnlyParser.Create();
			DiscardsRemaining = parent.DiscardsRemaining;
			DiscardsCount = parent.DiscardsCount;
			AllowUnlimitedPath = parent.AllowUnlimitedPath;
		}

		public ErrorRecoveryStep GetErrorRecoverySteps()
		{
			ErrorRecoveryStep errorRecoveryStep = History;
			while (errorRecoveryStep != null && IsParserStep(errorRecoveryStep))
			{
				errorRecoveryStep = errorRecoveryStep.Next;
			}
			ErrorRecoveryStep errorRecoveryStep2 = null;
			int num = 0;
			while (errorRecoveryStep != null)
			{
				ErrorRecoveryStep errorRecoveryStep3 = new ErrorRecoveryStep(errorRecoveryStep);
				if (errorRecoveryStep.ActionType == ErrorRecoveryActionType.Discard)
				{
					num++;
					errorRecoveryStep = errorRecoveryStep.Next;
					continue;
				}
				errorRecoveryStep3.Next = errorRecoveryStep2;
				errorRecoveryStep2 = errorRecoveryStep3;
				if (errorRecoveryStep3.ActionType == ErrorRecoveryActionType.ShiftError)
				{
					errorRecoveryStep3.DiscardCount = num;
					num = 0;
				}
				errorRecoveryStep = errorRecoveryStep.Next;
			}
			return errorRecoveryStep2;
		}

		private ErrorRecoveryState DefaultReduce()
		{
			ErrorRecoveryState errorRecoveryState = new ErrorRecoveryState(ErrorRecoveryStep.DefaultReduce(CurrentState().defaultAction), this);
			errorRecoveryState.ReduceInternal(-CurrentState().defaultAction);
			return errorRecoveryState;
		}

		private ErrorRecoveryState Reduce(int token, int action)
		{
			ErrorRecoveryState errorRecoveryState = new ErrorRecoveryState(ErrorRecoveryStep.ReduceOnToken(token, action), this);
			errorRecoveryState.ReduceInternal(-action);
			return errorRecoveryState;
		}

		private void ReduceInternal(int ruleNumber)
		{
			Rule rule = rules[ruleNumber];
			StateOnlyParser.DoAction(ruleNumber);
			StateStack.RemoveRange(StateStack.Count - rule.rhs.Length, rule.rhs.Length);
			State state = CurrentState();
			if (state.Goto.TryGetValue(rule.lhs, out var value))
			{
				state = states[value];
			}
			StateStack.Add(state);
		}

		private ErrorRecoveryState Shift(int token, int action)
		{
			ErrorRecoveryState errorRecoveryState = new ErrorRecoveryState(ErrorRecoveryStep.ShiftOnToken(token, action), this);
			State item = states[action];
			errorRecoveryState.StateStack.Add(item);
			if (IsErrorToken(token))
			{
				errorRecoveryState.SearchMode = ErrorRecoveryMode.Discarding;
				errorRecoveryState.History.ActionType = ErrorRecoveryActionType.ShiftError;
				switch (token)
				{
				case 127:
					errorRecoveryState.DiscardsRemaining = 500000 - errorRecoveryState.DiscardsCount;
					errorRecoveryState.AllowUnlimitedPath = false;
					break;
				case 132:
					errorRecoveryState.DiscardsRemaining = 500000 - errorRecoveryState.DiscardsCount;
					errorRecoveryState.AllowUnlimitedPath = AllowUnlimitedRecoveryPath;
					break;
				case 131:
					errorRecoveryState.DiscardsRemaining = 1 - errorRecoveryState.DiscardsCount;
					errorRecoveryState.AllowUnlimitedPath = false;
					break;
				}
				if (errorRecoveryState.DiscardsRemaining < 0)
				{
					errorRecoveryState.DiscardsRemaining = 0;
				}
			}
			else if (token == 129)
			{
				if (errorRecoveryState.DiscardsCount > 0)
				{
					errorRecoveryState.DiscardsCount--;
				}
			}
			else
			{
				errorRecoveryState.AllowUnlimitedPath = false;
				if (errorRecoveryState.SearchMode == ErrorRecoveryMode.Discarding)
				{
					errorRecoveryState.SearchMode = ErrorRecoveryMode.Normal;
					errorRecoveryState.AllowUnlimitedPath = false;
					errorRecoveryState.DiscardsCount = 0;
					errorRecoveryState.DiscardsRemaining = 0;
				}
			}
			return errorRecoveryState;
		}

		internal ErrorRecoveryState Discard()
		{
			ErrorRecoveryState errorRecoveryState = new ErrorRecoveryState(ErrorRecoveryStep.Discard(StateOnlyParser.scanner.Token), this);
			errorRecoveryState.StateOnlyParser.scanner.NextToken();
			errorRecoveryState.StateOnlyParser.scanner.ReadToken();
			errorRecoveryState.ActionCount++;
			errorRecoveryState.RecoveryCost++;
			errorRecoveryState.DiscardsCount++;
			errorRecoveryState.DiscardsRemaining--;
			return errorRecoveryState;
		}

		internal ErrorRecoveryState PopState(ref LexLocation errorLocation, ParserStack<LexLocation> locationStack)
		{
			ErrorRecoveryState errorRecoveryState = new ErrorRecoveryState(ErrorRecoveryStep.PopState(), this);
			int num = errorRecoveryState.StateStack.Count - 1;
			errorLocation = ((errorLocation != null) ? errorLocation.Merge(locationStack.array[num]) : locationStack.array[num]);
			errorRecoveryState.StateStack.RemoveAt(num);
			int num2 = (errorRecoveryState.DiscardsCount = ((errorLocation != null) ? (errorLocation.endTokenNumber - errorLocation.startTokenNumber) : 0));
			errorRecoveryState.RecoveryCost = 50 * num2;
			if (num2 > 0)
			{
				errorRecoveryState.SearchMode = ErrorRecoveryMode.RequireErrorObject;
			}
			return errorRecoveryState;
		}

		internal State CurrentState()
		{
			return StateStack[StateStack.Count - 1];
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Current State: {0},", CurrentState().num);
			stringBuilder.AppendFormat("Current SearchMode: {0},", SearchMode);
			stringBuilder.AppendFormat("History: {0}", (History == null) ? "null" : History.ToString());
			return stringBuilder.ToString();
		}

		internal ErrorRecoveryState SimulateNextToken(int nextToken)
		{
			ErrorRecoveryState errorRecoveryState = this;
			bool flag = false;
			while (!flag && errorRecoveryState != null)
			{
				State state = errorRecoveryState.CurrentState();
				if (state.defaultAction != 0)
				{
					errorRecoveryState = errorRecoveryState.DefaultReduce();
					continue;
				}
				if (!state.ExpectsToken(nextToken, out var action))
				{
					errorRecoveryState = null;
					break;
				}
				if (action > 0)
				{
					flag = true;
					errorRecoveryState = errorRecoveryState.Shift(nextToken, action);
				}
				else
				{
					errorRecoveryState = errorRecoveryState.Reduce(nextToken, action);
				}
			}
			if (flag)
			{
				while (errorRecoveryState.CurrentState().defaultAction != 0)
				{
					errorRecoveryState = errorRecoveryState.DefaultReduce();
				}
			}
			if (errorRecoveryState != null)
			{
				errorRecoveryState.ActionCount++;
				errorRecoveryState.RecoveryCost++;
			}
			return errorRecoveryState;
		}

		internal ErrorRecoveryState SimulateShiftToken()
		{
			ErrorRecoveryState errorRecoveryState = this;
			bool flag = false;
			while (!flag && errorRecoveryState != null)
			{
				State state = errorRecoveryState.CurrentState();
				if (state.defaultAction != 0)
				{
					errorRecoveryState = errorRecoveryState.DefaultReduce();
					continue;
				}
				GetNextAction(parserConfig, StateOnlyParser.scanner, state, out var action);
				if (action > 0)
				{
					flag = true;
					errorRecoveryState = errorRecoveryState.Shift(StateOnlyParser.scanner.Token, action);
					continue;
				}
				if (action < 0)
				{
					errorRecoveryState = errorRecoveryState.Reduce(StateOnlyParser.scanner.Token, action);
					continue;
				}
				errorRecoveryState = null;
				break;
			}
			if (flag)
			{
				while (errorRecoveryState.CurrentState().defaultAction != 0)
				{
					errorRecoveryState = errorRecoveryState.DefaultReduce();
				}
			}
			if (errorRecoveryState != null)
			{
				errorRecoveryState.ActionCount++;
				errorRecoveryState.RecoveryCost++;
			}
			return errorRecoveryState;
		}

		private bool IsParserStep(ErrorRecoveryStep step)
		{
			switch (step.ActionType)
			{
			case ErrorRecoveryActionType.Shift:
				if (!IsMissingToken(step.Token))
				{
					return step.Token != 129;
				}
				return false;
			case ErrorRecoveryActionType.DefaultReduce:
			case ErrorRecoveryActionType.Reduce:
				return true;
			default:
				return false;
			}
		}
	}

	public bool ParserTrace;

	public bool[] CoveredStates;

	public IParserTestSink parserTestSink;

	protected ParserScanner scanner;

	protected ParseContext parseContext;

	protected ReusableStatementsCollection reusableStmts;

	protected bool isIncremental;

	protected ParseNode yyval;

	protected LexLocation yyloc;

	private State current_state;

	private bool inErrorRecovery;

	private bool recovering;

	private int tokensSinceLastError;

	private ParserStack<State> state_stack;

	protected ParserStack<ParseNode> value_stack;

	protected ParserStack<LexLocation> location_stack;

	private ParserStack<SimpleScannerState> scanner_stack;

	protected bool isDumpLoadStatement;

	protected List<ParseNode> errValueList;

	protected LexLocation errorLocation;

	protected string[] nonTerminals;

	protected State[] states;

	protected Rule[] rules;

	private ParserConfig parserConfig;

	private const int eofToken = 128;

	private const int notEmptyToken = 129;

	private const int BeginMissingToken = 959;

	private const int EndMissingToken = 976;

	private const int MissingTokenCount = 17;

	[ThreadStatic]
	protected static ParseContext currentParseContext;

	private static readonly string[] prefixesToIgnore;

	private const int MaxDiscards = 500000;

	private static readonly int ErrorRecoveryMaxActionCount;

	private static readonly bool AllowUnlimitedRecoveryPath;

	private const int ErrorRecoveryNotEmptyTokenAtEofWeight = 10;

	private const int ErrorRecoveryErrorInsideOfErrorWeight = 10;

	private const int ErrorRecoveryPopStateTokenWeight = 50;

	private const int ErrorRecoveryDiscardTokenWeight = 1;

	public static ParseContext CurrentParseContext => currentParseContext;

	public int StackSize => state_stack.top;

	public ShiftReduceParser(ParserConfig parserConfig)
	{
		nonTerminals = parserConfig.NonTerminals;
		states = parserConfig.States;
		rules = parserConfig.Rules;
		this.parserConfig = parserConfig;
		state_stack = new ParserStack<State>();
		value_stack = new ParserStack<ParseNode>();
		location_stack = new ParserStack<LexLocation>();
		scanner_stack = new ParserStack<SimpleScannerState>();
		errValueList = new List<ParseNode>();
		Reset();
	}

	protected abstract StateOnlyParser CreateStateOnlyParser();

	private void Reset()
	{
		yyval.value = null;
		yyloc = null;
		current_state = null;
		inErrorRecovery = false;
		recovering = false;
		tokensSinceLastError = 0;
		state_stack.Clear();
		value_stack.Clear();
		location_stack.Clear();
		scanner_stack.Clear();
		isDumpLoadStatement = false;
		errValueList.Clear();
		errorLocation = null;
		scanner = null;
		parseContext = null;
		reusableStmts = null;
		isIncremental = false;
	}

	private static void GetNextAction(ParserConfig parserConfig, SimpleParserScanner scanner, State current_state, out int action)
	{
		action = 0;
		if (current_state.parser_table != null)
		{
			if (current_state.parser_table.TryGetValue(scanner.Token, out var value))
			{
				action = value;
			}
			if (action <= 0 || scanner.Token == 206 || scanner.Token == 233)
			{
				TokenTransformContext transformContext = parserConfig.GetTransformContext(current_state, scanner.Token);
				int num = 0;
				if (transformContext != 0)
				{
					num = scanner.yytransform(scanner.Token, transformContext);
				}
				if (num != 0 && current_state.parser_table.TryGetValue(scanner.Token, out value))
				{
					action = value;
				}
			}
		}
		if (action == 0)
		{
			action = current_state.defaultAction;
		}
	}

	public bool Parse(bool isIncremental, ParserScanner inputScanner, ParseContext parseContext, ReusableStatementsCollection reusableStmts)
	{
		try
		{
			scanner = inputScanner;
			this.parseContext = parseContext;
			this.isIncremental = isIncremental;
			if (this.isIncremental)
			{
				this.reusableStmts = reusableStmts;
				this.parseContext.BeginNewBatchParsing();
			}
			return DoParse();
		}
		finally
		{
			Reset();
		}
	}

	private bool DoParse()
	{
		if (ParserTrace)
		{
			CoveredStates = new bool[states.Length];
		}
		current_state = states[0];
		state_stack.Push(current_state);
		value_stack.Push(yyval);
		location_stack.Push(yyloc);
		if (isIncremental)
		{
			scanner_stack.Push(GetSimpleScannerState());
		}
		while (true)
		{
			if (ParserTrace)
			{
				Console.Error.WriteLine("Entering state {0} ", current_state.num);
				CoveredStates[current_state.num] = true;
			}
			if (current_state.parser_table != null)
			{
				if (!scanner.IsTokenRead)
				{
					if (ParserTrace)
					{
						Console.Error.Write("Reading a token: ");
					}
					scanner.ReadToken();
				}
				if (ParserTrace)
				{
					Console.Error.WriteLine("Next token is {0}", TerminalToString(scanner.Token));
				}
			}
			GetNextAction(parserConfig, scanner, current_state, out var action);
			if (action > 0)
			{
				SqlStatement sqlStatement = null;
				if (isIncremental && !recovering && reusableStmts != null)
				{
					SqlStatement current;
					while ((current = reusableStmts.Current) != null)
					{
						SimpleScannerState beforeScannerState = current.BeforeScannerState;
						if (beforeScannerState.IsAfter(parseContext.Diff, scanner.SimpleScannerState))
						{
							break;
						}
						if (beforeScannerState.Equals(parseContext.Diff, scanner.SimpleScannerState))
						{
							if (current.BeforeParserState.Equals(current_state))
							{
								sqlStatement = current;
								reusableStmts.RemoveCurrent();
								break;
							}
							if (parserTestSink != null)
							{
								parserTestSink.StatementEvent(current, StatementEventType.RejectedByState);
							}
						}
						else if (parserTestSink != null)
						{
							parserTestSink.StatementEvent(current, StatementEventType.RejectedByPosition);
						}
						reusableStmts.SplitCurrent();
					}
				}
				if (sqlStatement != null)
				{
					ShiftStatement(sqlStatement);
				}
				else
				{
					ShiftToken(action);
				}
			}
			else if (action < 0)
			{
				Reduce(-action);
				if (action == -1)
				{
					return true;
				}
			}
			else if (!ErrorRecovery())
			{
				break;
			}
		}
		return false;
	}

	private static bool IsErrorToken(int value)
	{
		if (value == 127 || (uint)(value - 131) <= 1u)
		{
			return true;
		}
		return false;
	}

	private SimpleScannerState GetSimpleScannerState()
	{
		if (inErrorRecovery)
		{
			return SimpleScannerState.InvalidState;
		}
		return scanner.SimpleScannerState;
	}

	private CompleteScannerState GetCompleteScannerState()
	{
		if (inErrorRecovery)
		{
			return CompleteScannerState.InvalidState;
		}
		return scanner.CompleteScannerState;
	}

	protected void ShiftStatement(SqlStatement stmt)
	{
		parseContext.ReuseStatement(stmt);
		if (ParserTrace)
		{
			Console.Error.WriteLine("Shifting stmt {0}, from: {1} {2} to: {3} {4}", stmt.GetType(), stmt.BeforeParserState.num, stmt.BeforeScannerState, stmt.AfterParserState.num, stmt.AfterScannerState);
		}
		current_state = stmt.AfterParserState;
		value_stack.Push(new ParseNode(stmt));
		state_stack.Push(current_state);
		location_stack.Push(stmt.YYLoc);
		scanner_stack.Push(scanner.SimpleScannerState);
		ModifiedMinimalList<TokenData> prevTokenList = parseContext.PrevContext.TokenManager.TokenList as ModifiedMinimalList<TokenData>;
		scanner.ForwardScannerState(stmt.AfterScannerState, prevTokenList, parseContext.Diff);
		if (parserTestSink != null)
		{
			parserTestSink.StatementEvent(stmt, StatementEventType.Reused);
		}
	}

	protected void ShiftToken(int state_nr)
	{
		Shift(state_nr, scanner.Token, default(ParseNode), scanner.TokenLocation);
		if (scanner.Token != 128)
		{
			scanner.NextToken();
		}
	}

	protected void Shift(int state_nr, int token, ParseNode value, LexLocation loc)
	{
		if (ParserTrace)
		{
			Console.Error.Write("Shifting token {0}, ", TerminalToString(token));
		}
		current_state = states[state_nr];
		value_stack.Push(value);
		state_stack.Push(current_state);
		location_stack.Push(loc);
		if (recovering)
		{
			if (!IsErrorToken(scanner.Token))
			{
				tokensSinceLastError++;
			}
			if (tokensSinceLastError > 0)
			{
				recovering = false;
			}
		}
		if (isIncremental)
		{
			scanner_stack.Push(GetSimpleScannerState());
		}
	}

	protected void Reduce(int rule_nr)
	{
		if (ParserTrace)
		{
			DisplayRule(rule_nr);
		}
		Rule rule = rules[rule_nr];
		switch (rule.rhs.Length)
		{
		case 0:
			yyval = default(ParseNode);
			yyloc = null;
			break;
		case 1:
			yyval = value_stack.Top();
			yyloc = location_stack.Top();
			break;
		case 2:
			yyval = default(ParseNode);
			yyloc = LexLocation.Merge(location_stack.array[location_stack.top - 2], location_stack.array[location_stack.top - 1]);
			break;
		default:
			yyval = default(ParseNode);
			yyloc = MergeLocationRange(location_stack.top - rule.rhs.Length, location_stack.top - 1);
			break;
		}
		DoAction(rule_nr);
		SqlStatement sqlStatement = ((isIncremental && !inErrorRecovery) ? (yyval.value as SqlStatement) : null);
		State beforeParserState = null;
		SimpleScannerState beforeScannerStateRelative = default(SimpleScannerState);
		SideEffectSpan sideEffectSpan = null;
		state_stack.Pop(rule.rhs.Length);
		value_stack.Pop(rule.rhs.Length);
		location_stack.Pop(rule.rhs.Length);
		if (isIncremental)
		{
			scanner_stack.Pop(rule.rhs.Length);
			if (sqlStatement != null)
			{
				beforeParserState = state_stack.Top();
				beforeScannerStateRelative = scanner_stack.Top();
			}
			sideEffectSpan = parseContext.RegisterReduction();
		}
		if (ParserTrace)
		{
			DisplayStack();
		}
		current_state = state_stack.Top();
		if (current_state.Goto.TryGetValue(rule.lhs, out var value))
		{
			current_state = states[value];
		}
		state_stack.Push(current_state);
		value_stack.Push(yyval);
		location_stack.Push(yyloc);
		if (!isIncremental)
		{
			return;
		}
		scanner_stack.Push(GetSimpleScannerState());
		if (sqlStatement != null)
		{
			State afterParserState = state_stack.Top();
			CompleteScannerState completeScannerState = GetCompleteScannerState();
			if (beforeScannerStateRelative.IsValid && completeScannerState.IsValid && yyloc != null)
			{
				sqlStatement.ReuseData = new SqlStatementReuseData(beforeParserState, afterParserState, beforeScannerStateRelative, completeScannerState, scanner.LookaheadTokenIndex, yyloc);
				sqlStatement.ReuseData.SideEffectSpan = sideEffectSpan;
			}
		}
	}

	protected LexLocation MergeLocationRange(int startIndex, int endIndex)
	{
		while (startIndex < endIndex && location_stack.array[startIndex] == null)
		{
			startIndex++;
		}
		while (endIndex > startIndex && location_stack.array[endIndex] == null)
		{
			endIndex--;
		}
		return LexLocation.Merge(location_stack.array[startIndex], location_stack.array[endIndex]);
	}

	protected abstract void DoAction(int action_nr);

	public bool ErrorRecovery()
	{
		inErrorRecovery = true;
		if (!recovering)
		{
			ReportError();
		}
		if (!CurrentParseContext.IsErrorRecoveryEnabled || !ErrorRecoveryUtils.RecoverFromError(this))
		{
			return false;
		}
		recovering = true;
		tokensSinceLastError = 0;
		inErrorRecovery = false;
		return true;
	}

	internal static bool IsKeyword(int id)
	{
		if (id > 128)
		{
			return id != 139;
		}
		return false;
	}

	private static bool IsMissingToken(int token)
	{
		if (token >= 959)
		{
			return token < 976;
		}
		return false;
	}

	public void ReportError()
	{
		StringBuilder stringBuilder = new StringBuilder();
		string tokenNear = ((scanner.TokenLocation.startTokenNumber != scanner.FirstLevelTokenManager.TokenList.Count) ? scanner.TokenText : SR.EndOfFile);
		stringBuilder.Append(SR.IncorrectSyntaxNear(tokenNear));
		LexLocation tokenLocation = scanner.TokenLocation;
		if (current_state.parser_table.Count < 7)
		{
			List<string> list = new List<string>(current_state.parser_table.Count);
			foreach (int key in current_state.parser_table.Keys)
			{
				if ((key <= 959 || key >= 976) && !IsErrorToken(key) && key != 129)
				{
					string tokenName = TerminalToString(key);
					list.Add(GetTokenFriendlyName(tokenName));
				}
			}
			list.Sort(StringComparer.OrdinalIgnoreCase);
			for (int i = 0; i < list.Count; i++)
			{
				if (i == 0)
				{
					stringBuilder.Append(SR.Expecting);
				}
				else if (i < list.Count - 1)
				{
					stringBuilder.Append(SR.Comma);
				}
				else
				{
					stringBuilder.Append(SR.CommaOr);
				}
				stringBuilder.Append(list[i]);
			}
			stringBuilder.Append(SR.Period);
		}
		parseContext.ErrorHandler.AddError(stringBuilder.ToString(), ErrorType.ParseError, ErrorFlags.None, tokenLocation);
	}

	public void ShiftMissingToken(int missingToken)
	{
		ParseNode value = default(ParseNode);
		LexLocation currentInsertLocation = GetCurrentInsertLocation();
		if (ParserTrace)
		{
			Console.Error.WriteLine("Shifting missing token {0} in state {1} ", (Tokens)missingToken, current_state.num);
		}
		Shift(current_state.parser_table[missingToken], missingToken, value, currentInsertLocation);
		if (ParserTrace)
		{
			Console.Error.WriteLine("Entering state {0} ", current_state.num);
		}
	}

	public void ShiftErrorToken(int errToken)
	{
		SqlErrorCodeObject sqlErrorCodeObject = CreateErrorObject();
		ParseNode value = new ParseNode(sqlErrorCodeObject);
		LexLocation position = sqlErrorCodeObject.Position;
		if (ParserTrace)
		{
			Console.Error.WriteLine("Shifting error in state {0} ", current_state.num);
		}
		Shift(current_state.parser_table[errToken], errToken, value, position);
		if (ParserTrace)
		{
			Console.Error.WriteLine("Entering state {0} ", current_state.num);
		}
	}

	public void ShiftNotEmptyToken()
	{
		int num = 129;
		ParseNode value = default(ParseNode);
		LexLocation loc = location_stack.array[location_stack.top - 1];
		if (ParserTrace)
		{
			Console.Error.WriteLine("Shifting error in state {0}", current_state.num);
		}
		Shift(current_state.parser_table[num], num, value, loc);
		if (ParserTrace)
		{
			Console.Error.WriteLine("Entering state {0} ", current_state.num);
		}
	}

	private bool PopState()
	{
		if (ParserTrace)
		{
			Console.Error.WriteLine("Error: popping state {0}", state_stack.Top().num);
		}
		errValueList.Add(value_stack.Top());
		if (errorLocation == null)
		{
			errorLocation = location_stack.Top();
		}
		else
		{
			errorLocation = errorLocation.Merge(location_stack.Top());
		}
		state_stack.Pop();
		value_stack.Pop();
		location_stack.Pop();
		if (isIncremental)
		{
			scanner_stack.Pop();
		}
		if (ParserTrace)
		{
			DisplayStack();
		}
		if (state_stack.IsEmpty())
		{
			if (ParserTrace)
			{
				Console.Error.Write("Aborting: didn't find a state that accepts error token");
			}
			return false;
		}
		current_state = state_stack.Top();
		return true;
	}

	private void DiscardingInvalidToken()
	{
		if (errorLocation == null)
		{
			errorLocation = scanner.TokenLocation;
		}
		else
		{
			errorLocation = errorLocation.Merge(scanner.TokenLocation);
		}
		scanner.NextToken();
		scanner.ReadToken();
		if (ParserTrace)
		{
			Console.Error.Write("Reading a token: {0}", TerminalToString(scanner.Token));
		}
	}

	private SqlErrorCodeObject CreateErrorObject()
	{
		List<SqlCodeObject> list = new List<SqlCodeObject>();
		for (int i = 0; i < errValueList.Count; i++)
		{
			object value = errValueList[i].value;
			if (value is SqlCodeObject item)
			{
				list.Add(item);
			}
			else
			{
				if (!(value is IEnumerable enumerable))
				{
					continue;
				}
				foreach (object item3 in enumerable)
				{
					if (item3 is SqlCodeObject item2)
					{
						list.Add(item2);
					}
				}
			}
		}
		list.Sort(SqlCodeObject.LexicalComparer);
		SqlErrorCodeObject sqlErrorCodeObject = new SqlErrorCodeObject(list);
		if (errorLocation == null)
		{
			errorLocation = GetCurrentInsertLocation();
		}
		sqlErrorCodeObject.Position = errorLocation;
		errValueList.Clear();
		errorLocation = null;
		return sqlErrorCodeObject;
	}

	private LexLocation GetCurrentInsertLocation()
	{
		LexLocation lexLocation = null;
		int num = location_stack.top - 1;
		while (lexLocation == null && num >= 0)
		{
			lexLocation = location_stack.array[num];
			num--;
		}
		int num2 = lexLocation?.endTokenNumber ?? ((errorLocation == null) ? scanner.TokenLocation.startTokenNumber : errorLocation.startTokenNumber);
		return new LexLocation(num2, num2);
	}

	protected void yyerrok()
	{
		recovering = false;
	}

	protected static T UpdatePos<T>(T codeObj, LexLocation pos) where T : SqlCodeObject
	{
		codeObj.Position = pos;
		return codeObj;
	}

	protected string GetText(LexLocation location)
	{
		return scanner.FirstLevelTokenManager.GetText(location.startTokenNumber, location.endTokenNumber);
	}

	protected StringToken GetStringToken(LexLocation location)
	{
		return scanner.FirstLevelTokenManager.GetStringToken(location.startTokenNumber);
	}

	private void DisplayStack()
	{
		Console.Error.Write("State now");
		for (int i = 0; i < state_stack.top; i++)
		{
			Console.Error.Write(" {0}", state_stack.array[i].num);
		}
		Console.Error.WriteLine();
	}

	private void DisplayRule(int rule_nr)
	{
		Console.Error.Write("Reducing stack by rule {0}, ", rule_nr);
		DisplayProduction(rules[rule_nr]);
	}

	private void DisplayProduction(Rule rule)
	{
		if (rule.rhs.Length == 0)
		{
			Console.Error.Write("/* empty */ ");
		}
		else
		{
			int[] rhs = rule.rhs;
			foreach (int symbol in rhs)
			{
				Console.Error.Write("{0} ", SymbolToString(symbol));
			}
		}
		Console.Error.WriteLine("-> {0}", SymbolToString(rule.lhs));
	}

	internal static string TerminalToString(int token)
	{
		if (token < 127)
		{
			return CharToString((char)token);
		}
		Tokens tokens = (Tokens)token;
		return tokens.ToString();
	}

	private string SymbolToString(int symbol)
	{
		if (symbol < 0)
		{
			return nonTerminals[-symbol];
		}
		return TerminalToString(symbol);
	}

	protected static string CharToString(char ch)
	{
		return ch switch
		{
			'\a' => string.Format(CultureInfo.CurrentCulture, "'\\a'"), 
			'\b' => string.Format(CultureInfo.CurrentCulture, "'\\b'"), 
			'\f' => string.Format(CultureInfo.CurrentCulture, "'\\f'"), 
			'\n' => string.Format(CultureInfo.CurrentCulture, "'\\n'"), 
			'\r' => string.Format(CultureInfo.CurrentCulture, "'\\r'"), 
			'\t' => string.Format(CultureInfo.CurrentCulture, "'\\t'"), 
			'\v' => string.Format(CultureInfo.CurrentCulture, "'\\v'"), 
			'\0' => string.Format(CultureInfo.CurrentCulture, "'\\0'"), 
			_ => string.Format(CultureInfo.CurrentCulture, "'{0}'", ch), 
		};
	}

	protected static string GetTokenFriendlyName(string tokenName)
	{
		int num = 0;
		string[] array = prefixesToIgnore;
		foreach (string text in array)
		{
			if (tokenName.StartsWith(text, StringComparison.Ordinal))
			{
				num = Math.Max(num, text.Length);
			}
		}
		if (num <= 0)
		{
			return tokenName;
		}
		return tokenName.Substring(num);
	}

	static ShiftReduceParser()
	{
		prefixesToIgnore = new string[12]
		{
			"TOKEN_", "TOKEN_c_", "TOKEN_s_", "TOKEN_s_AE_", "TOKEN_s_AW_", "TOKEN_s_AWEO_", "TOKEN_s_CDA_", "TOKEN_s_CES_", "TOKEN_s_CTW_", "TOKEN_s_DD_",
			"TOKEN_s_ET_", "TOKEN_s_ELFS_"
		};
		string environmentVariable = Environment.GetEnvironmentVariable("SqlParserErrorRecoveryLimit");
		AllowUnlimitedRecoveryPath = false;
		ErrorRecoveryMaxActionCount = 40;
		if (!string.IsNullOrEmpty(environmentVariable))
		{
			if (environmentVariable == "0")
			{
				AllowUnlimitedRecoveryPath = true;
				ErrorRecoveryMaxActionCount = 50;
			}
			else
			{
				int.TryParse(environmentVariable, out ErrorRecoveryMaxActionCount);
			}
		}
	}
}
