namespace Stimulsoft.Dashboard.Components.ComboBox;

public class StiComboBoxItem
{
	public bool Checked { get; set; } = true;


	public string Label { get; set; }

	public object Value { get; set; }

	public override string ToString()
	{
		if (Label == null)
		{
			return Value?.ToString();
		}
		return Label;
	}

	public StiComboBoxItem(string label)
		: this(label, label)
	{
	}

	public StiComboBoxItem(string label, object value)
	{
		Label = label;
		Value = value;
	}
}
