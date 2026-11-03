namespace Stimulsoft.Report;

public class StiDragDropFromDictionaryEventArgs
{
	public string Text { get; set; }

	public string TextLabel { get; set; }

	public bool Cancel { get; set; }

	public StiDragDropFromDictionaryEventArgs(string text, string textLabel, bool cancel)
	{
		Text = text;
		TextLabel = textLabel;
		Cancel = cancel;
	}
}
