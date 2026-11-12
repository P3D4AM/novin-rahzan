namespace Stimulsoft.Dashboard.Components.ListBox;

public class StiListBoxItem
{
	public string Label { get; set; }

	public object Value { get; set; }

	public bool Checked { get; set; }

	public override string ToString()
	{
		string? text = Label;
		if (text == null)
		{
			object value = Value;
			if (value == null)
			{
				return null;
			}
			text = value.ToString();
		}
		return text;
	}

	public StiListBoxItem(string label)
		: this(label, label)
	{
	}

	public StiListBoxItem(string label, object value)
	{
		Label = label;
		Value = value;
	}
}
