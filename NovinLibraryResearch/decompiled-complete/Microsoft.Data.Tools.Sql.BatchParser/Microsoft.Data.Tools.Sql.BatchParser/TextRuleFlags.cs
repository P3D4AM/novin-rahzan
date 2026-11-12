using System;

namespace Microsoft.Data.Tools.Sql.BatchParser;

[Flags]
internal enum TextRuleFlags
{
	ReportWhitespace = 1,
	RecognizeDoubleQuotedString = 2,
	RecognizeSingleQuotedString = 4,
	RecognizeLineComment = 8,
	RecognizeBlockComment = 0x10,
	RecognizeBrace = 0x20
}
