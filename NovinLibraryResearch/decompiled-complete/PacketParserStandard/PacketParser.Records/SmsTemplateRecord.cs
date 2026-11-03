namespace PacketParser.Records;

public record SmsTemplateRecord
{
	public string Text { get; private set; }

	public bool Enabled { get; private set; }

	public SmsTemplateRecord(string text, bool enabled = true)
	{
		Text = text;
		Enabled = enabled;
	}
}
