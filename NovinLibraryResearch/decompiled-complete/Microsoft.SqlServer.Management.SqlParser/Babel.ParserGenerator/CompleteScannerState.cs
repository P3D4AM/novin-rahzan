using System.Globalization;

namespace Babel.ParserGenerator;

internal struct CompleteScannerState
{
	public static readonly CompleteScannerState InvalidState = new CompleteScannerState(-1, -1, -1, isTokenRead: false);

	public readonly int InitialScannerState;

	public readonly int FinalScannerState;

	public readonly int TokenIndex;

	public readonly bool IsTokenRead;

	public bool IsValid => InitialScannerState != -1;

	public CompleteScannerState(int initialScannerState, int finalScannerState, int tokenIndex, bool isTokenRead)
	{
		InitialScannerState = initialScannerState;
		FinalScannerState = finalScannerState;
		TokenIndex = tokenIndex;
		IsTokenRead = isTokenRead;
	}

	public CompleteScannerState(SimpleScannerState simpleScannerState, int finalScannerState, bool isTokenRead)
		: this(simpleScannerState.ScannerState, finalScannerState, simpleScannerState.TokenIndex, isTokenRead)
	{
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}:{1}/{2}{3}", TokenIndex, InitialScannerState, FinalScannerState, IsTokenRead ? "/Read" : "");
	}
}
