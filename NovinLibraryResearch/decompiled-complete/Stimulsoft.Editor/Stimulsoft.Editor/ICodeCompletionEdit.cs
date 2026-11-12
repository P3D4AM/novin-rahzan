namespace Stimulsoft.Editor;

public interface ICodeCompletionEdit : ICodeCompletionWindow, IControl
{
	string EditField { get; set; }

	string EditPath { get; set; }

	string EditText { get; set; }
}
