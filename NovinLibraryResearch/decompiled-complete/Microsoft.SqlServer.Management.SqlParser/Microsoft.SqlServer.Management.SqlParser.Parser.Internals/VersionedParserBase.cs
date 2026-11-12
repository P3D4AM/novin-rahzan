using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Diagnostics;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal abstract class VersionedParserBase : ShiftReduceParser
{
	protected SqlBatch root;

	public VersionedParserBase(ParserConfig parserConfig)
		: base(parserConfig)
	{
	}

	private static ParserScanner CreateParserScanner(ErrorHandler errorHandler, string sql, ParseOptions options, out ParseOptions resultOptions)
	{
		return ParserScanner.CreateParserScanner(Scanner.ScanSource(errorHandler, sql, options, out resultOptions), options.TransactSqlVersion);
	}

	public SqlScript Parse(bool isIncremental, string sql, SqlScript prevScript, ParseOptions options, out ParseOptions resultOptions)
	{
		try
		{
			TraceHelper.TraceContext.TraceActivityStart("Parse T-SQL");
			TraceHelper.TraceContext.TraceInformation("Incremental:{0}, T-SQL Input Size: {1} characters.", isIncremental, sql.Length);
			TraceHelper.TraceContext.TraceActivityStart("Lexer");
			GeneralErrorHandler generalErrorHandler = new GeneralErrorHandler();
			ParserScanner parserScanner = CreateParserScanner(generalErrorHandler, sql, options, out resultOptions);
			TraceHelper.TraceContext.TraceActivityEnd("Lexer");
			TraceHelper.TraceContext.TraceInformation("Lexer Ended - Token Count: {0}.", parserScanner.FirstLevelTokenManager.TokenList.Count);
			ParseContext parseContext = (ShiftReduceParser.currentParseContext = new ParseContext(parserScanner, options, isIncremental));
			if (prevScript != null && prevScript.IsBound)
			{
				prevScript.Unbind();
			}
			parseContext.BeginParse(generalErrorHandler, this, prevScript);
			parseContext.SetSideEffectMode(isIncremental);
			List<SqlBatch> list = new List<SqlBatch>();
			SqlScript sqlScript;
			if (parseContext.Diff != null && parseContext.Diff.IsEmpty)
			{
				sqlScript = prevScript;
			}
			else
			{
				ReusableStatementsCollection reusableStatementsCollection = null;
				if (parseContext.Diff != null)
				{
					reusableStatementsCollection = new ReusableStatementsCollection(prevScript, parseContext, parserTestSink);
					TraceHelper.TraceContext.TraceInformation("Using token diff: {0}", parseContext.Diff);
				}
				TokenManager tokenManager = parseContext.TokenManager;
				while (parserScanner.MoveToNextBatch())
				{
					root = null;
					Parse(isIncremental, parserScanner, parseContext, reusableStatementsCollection);
					if (root == null)
					{
						root = new SqlBatch();
					}
					root.Position = tokenManager.GetBatchRange(parserScanner.CurrentBatchNumber);
					list.Add(root);
				}
				parseContext.SetSideEffectMode(sideEffectMode: false);
				sqlScript = new SqlScript(list, isIncremental);
				sqlScript.Position = new LexLocation(0, tokenManager.Count);
				sqlScript.SetParentPointers();
				parseContext.PostParseErrorChecking();
				generalErrorHandler.CommitErrors();
				ErrorCollection errorCollection = generalErrorHandler.GetErrorCollection(parseContext.TokenManager);
				sqlScript.UpdateParseErrors(errorCollection);
			}
			parseContext.EndParse();
			return sqlScript;
		}
		catch (Exception ex)
		{
			TraceHelper.TraceContext.TraceCatch(ex);
			throw new SqlParserInternalParserError(ex);
		}
		finally
		{
			base.parseContext = null;
			ShiftReduceParser.currentParseContext = null;
			TraceHelper.TraceContext.TraceActivityEnd("Parse T-SQL");
		}
	}

	protected bool ExpectKeyword(StringToken keyword, LexLocation lexLocation)
	{
		if (GetStringToken(lexLocation) != keyword)
		{
			SyntaxError(lexLocation);
			return false;
		}
		return true;
	}

	protected bool ExpectKeywords(IEnumerable<StringToken> keywords, LexLocation lexLocation)
	{
		foreach (StringToken keyword in keywords)
		{
			if (GetStringToken(lexLocation) == keyword)
			{
				return true;
			}
		}
		SyntaxError(lexLocation);
		return false;
	}

	protected bool IsMaxAbsoluteIntegerValue(string value)
	{
		return StringComparer.Ordinal.Equals(value, "2147483648");
	}

	protected static void SyntaxError(LexLocation lexLocation)
	{
		ShiftReduceParser.currentParseContext.ReportSyntaxError(lexLocation);
	}

	protected static void SyntaxError(string tokenString, LexLocation lexLocation)
	{
		ShiftReduceParser.currentParseContext.ReportSyntaxError(tokenString, lexLocation);
	}

	protected static void ReportError(string errorMessage, LexLocation lexLocation)
	{
		ShiftReduceParser.currentParseContext.ErrorHandler.AddError(errorMessage, ErrorType.ParseError, ErrorFlags.None, lexLocation);
	}
}
