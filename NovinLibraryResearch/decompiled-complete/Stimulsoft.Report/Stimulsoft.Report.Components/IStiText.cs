using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Components;

public interface IStiText
{
	StiExpression Text { get; set; }

	string TextValue { get; set; }

	StiPenStyle LinesOfUnderline { get; set; }

	bool HideZeros { get; set; }

	StiProcessingDuplicatesType ProcessingDuplicates { get; set; }

	bool OnlyText { get; set; }

	int MaxNumberOfLines { get; set; }

	void SetText(StiGetValue getValue);

	string GetTextInternal();

	void SetTextInternal(string value);
}
