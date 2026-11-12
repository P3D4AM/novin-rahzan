using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals105;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals110;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals120;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals130;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals140;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals150;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals160;
using Microsoft.SqlServer.Management.SqlParser.Parser.InternalsAzure;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

public static class Parser
{
	private static VersionedParserBase CreateVersionedParser(ParseOptions options)
	{
		switch (options.TransactSqlVersion)
		{
		case TransactSqlVersion.Version105:
			return new Microsoft.SqlServer.Management.SqlParser.Parser.Internals105.Parser();
		case TransactSqlVersion.Version110:
			return new Microsoft.SqlServer.Management.SqlParser.Parser.Internals110.Parser();
		case TransactSqlVersion.Version120:
			return new Microsoft.SqlServer.Management.SqlParser.Parser.Internals120.Parser();
		case TransactSqlVersion.Version130:
			return new Microsoft.SqlServer.Management.SqlParser.Parser.Internals130.Parser();
		case TransactSqlVersion.Version140:
			return new Microsoft.SqlServer.Management.SqlParser.Parser.Internals140.Parser();
		case TransactSqlVersion.Version150:
			return new Microsoft.SqlServer.Management.SqlParser.Parser.Internals150.Parser();
		case TransactSqlVersion.Current:
		case TransactSqlVersion.Version160:
			return new Microsoft.SqlServer.Management.SqlParser.Parser.Internals160.Parser();
		case TransactSqlVersion.Azure:
			return new Microsoft.SqlServer.Management.SqlParser.Parser.InternalsAzure.Parser();
		default:
			return new Microsoft.SqlServer.Management.SqlParser.Parser.Internals110.Parser();
		}
	}

	public static ParseResult Parse(string sql)
	{
		return Parse(sql, new ParseOptions());
	}

	public static ParseResult Parse(string sql, ParseOptions options)
	{
		ParseOptions resultOptions;
		return Parse(sql, options, out resultOptions);
	}

	public static ParseResult Parse(string sql, ParseOptions options, out ParseOptions resultOptions)
	{
		return Parse(isIncremental: false, sql, null, options, out resultOptions);
	}

	public static ParseResult IncrementalParse(string sql, ParseResult prevResult)
	{
		return IncrementalParse(sql, prevResult, new ParseOptions());
	}

	public static ParseResult IncrementalParse(string sql, ParseResult prevResult, ParseOptions options)
	{
		ParseOptions resultOptions;
		return IncrementalParse(sql, prevResult, options, out resultOptions);
	}

	public static ParseResult IncrementalParse(string sql, ParseResult prevResult, ParseOptions options, out ParseOptions resultOptions)
	{
		return Parse(isIncremental: true, sql, prevResult, options, out resultOptions);
	}

	internal static ParseResult Parse(bool isIncremental, string sql, ParseResult prevResult, ParseOptions options, out ParseOptions resultOptions)
	{
		if (sql == null)
		{
			throw new ArgumentNullException("sql");
		}
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		if (prevResult != null)
		{
			if (!prevResult.Script.IsIncrementalMode)
			{
				throw new ArgumentException("Must be a result of incremental parsing.", "prevResult");
			}
			if (!isIncremental)
			{
				throw new ArgumentException("Must be null for non-incremental parsing.", "prevResult");
			}
		}
		VersionedParserBase versionedParserBase = CreateVersionedParser(options);
		SqlScript prevScript = prevResult?.Script;
		return new ParseResult(versionedParserBase.Parse(isIncremental, sql, prevScript, options, out resultOptions));
	}

	internal static ParseResult Parse(bool isIncremental, string sql, ParseResult prevResult, ParseOptions options, out ParseOptions resultOptions, IParserTestSink parserTestSink)
	{
		if (sql == null)
		{
			throw new ArgumentNullException("sql");
		}
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		if (prevResult != null)
		{
			if (!prevResult.Script.IsIncrementalMode)
			{
				throw new ArgumentException("Must be a result of incremental parsing.", "prevResult");
			}
			if (!isIncremental)
			{
				throw new ArgumentException("Must be null for non-incremental parsing.", "prevResult");
			}
		}
		VersionedParserBase versionedParserBase = CreateVersionedParser(options);
		versionedParserBase.parserTestSink = parserTestSink;
		try
		{
			SqlScript prevScript = prevResult?.Script;
			return new ParseResult(versionedParserBase.Parse(isIncremental, sql, prevScript, options, out resultOptions));
		}
		finally
		{
			versionedParserBase.parserTestSink = null;
		}
	}

	[Conditional("DEBUG")]
	internal static void ParseStates(string sql, List<bool> states)
	{
		if (sql == null)
		{
			throw new ArgumentNullException("sql");
		}
		ParseOptions parseOptions = new ParseOptions();
		parseOptions.TransactSqlVersion = TransactSqlVersion.Current;
		VersionedParserBase versionedParserBase = CreateVersionedParser(parseOptions);
		versionedParserBase.ParserTrace = true;
		versionedParserBase.Parse(isIncremental: false, sql, null, parseOptions, out var _);
		states.AddRange(versionedParserBase.CoveredStates);
	}

	internal static TokenManager ScanSource(string sql, ParseOptions options)
	{
		if (sql == null)
		{
			throw new ArgumentNullException("sql");
		}
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		ParseOptions resultOptions;
		return Microsoft.SqlServer.Management.SqlParser.Parser.Internals.Scanner.ScanSource(new GeneralErrorHandler(), sql, options, out resultOptions);
	}
}
