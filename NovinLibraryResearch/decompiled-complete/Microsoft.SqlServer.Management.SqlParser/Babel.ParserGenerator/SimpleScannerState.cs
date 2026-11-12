using System.Globalization;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Babel.ParserGenerator;

internal struct SimpleScannerState
{
	public static readonly SimpleScannerState InvalidState = new SimpleScannerState(-1, -1);

	public readonly int ScannerState;

	public readonly int TokenIndex;

	public bool IsValid => ScannerState != -1;

	public SimpleScannerState(int scannerState, int tokenIndex)
	{
		ScannerState = scannerState;
		TokenIndex = tokenIndex;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", TokenIndex, ScannerState);
	}

	public bool Equals(TokenUpdateDiff diff, SimpleScannerState other)
	{
		if (diff.FixTokenIndex(TokenIndex) == other.TokenIndex)
		{
			return ScannerState == other.ScannerState;
		}
		return false;
	}

	public bool IsBefore(TokenUpdateDiff diff, SimpleScannerState other)
	{
		return diff.FixTokenIndex(TokenIndex) < other.TokenIndex;
	}

	public bool IsAfter(TokenUpdateDiff diff, SimpleScannerState other)
	{
		return diff.FixTokenIndex(TokenIndex) > other.TokenIndex;
	}
}
