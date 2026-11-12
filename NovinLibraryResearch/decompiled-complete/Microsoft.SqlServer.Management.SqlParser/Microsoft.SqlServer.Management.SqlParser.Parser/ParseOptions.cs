using System;
using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

public class ParseOptions
{
	private const string DefaultBatchSeparator = "GO";

	private const bool DefalutIsQuotedIdentifierSet = true;

	private const bool DefaultIsErrorRecoveryEnabled = true;

	private const DatabaseCompatibilityLevel DefaultCompatibilityLevel = DatabaseCompatibilityLevel.Current;

	private const TransactSqlVersion DefaultTransactSqlVersion = TransactSqlVersion.Current;

	public string BatchSeparator { get; set; }

	public DatabaseCompatibilityLevel CompatibilityLevel { get; set; }

	public bool IsQuotedIdentifierSet { get; set; }

	public bool IsErrorRecoveryEnabled { get; set; }

	public TransactSqlVersion TransactSqlVersion { get; set; }

	public ParseOptions()
		: this("GO", isQuotedIdentifierSet: true, DatabaseCompatibilityLevel.Current, TransactSqlVersion.Current)
	{
	}

	public ParseOptions(string batchSeparator)
		: this(batchSeparator, isQuotedIdentifierSet: true, DatabaseCompatibilityLevel.Current, TransactSqlVersion.Current)
	{
	}

	public ParseOptions(bool isQuotedIdentifierSet)
		: this("GO", isQuotedIdentifierSet, DatabaseCompatibilityLevel.Current, TransactSqlVersion.Current)
	{
	}

	internal ParseOptions(ParseOptions rhs)
		: this(rhs.BatchSeparator, rhs.IsQuotedIdentifierSet, rhs.CompatibilityLevel, rhs.TransactSqlVersion)
	{
		IsErrorRecoveryEnabled = rhs.IsErrorRecoveryEnabled;
	}

	public ParseOptions(string batchSeparator, bool isQuotedIdentifierSet)
		: this(batchSeparator, isQuotedIdentifierSet, DatabaseCompatibilityLevel.Current)
	{
	}

	public ParseOptions(string batchSeparator, bool isQuotedIdentifierSet, DatabaseCompatibilityLevel compatibilityLevel)
		: this(batchSeparator, isQuotedIdentifierSet, compatibilityLevel, TransactSqlVersion.Current)
	{
	}

	public ParseOptions(string batchSeparator, bool isQuotedIdentifierSet, DatabaseCompatibilityLevel compatibilityLevel, TransactSqlVersion transactSqlVersion)
	{
		if (batchSeparator == null)
		{
			throw new ArgumentNullException("batchSeparator");
		}
		BatchSeparator = batchSeparator;
		IsQuotedIdentifierSet = isQuotedIdentifierSet;
		IsErrorRecoveryEnabled = true;
		CompatibilityLevel = compatibilityLevel;
		TransactSqlVersion = transactSqlVersion;
	}

	public bool Equals(ParseOptions other)
	{
		if (other == this)
		{
			return true;
		}
		if (other == null)
		{
			return false;
		}
		if (IsQuotedIdentifierSet == other.IsQuotedIdentifierSet && IsErrorRecoveryEnabled == other.IsErrorRecoveryEnabled && CompatibilityLevel == other.CompatibilityLevel && BatchSeparator == other.BatchSeparator)
		{
			return TransactSqlVersion == other.TransactSqlVersion;
		}
		return false;
	}
}
