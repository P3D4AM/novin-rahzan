namespace Stimulsoft.Dashboard.Design.Editors.PivotTable;

internal class StiPivotMeterComboBoxItem
{
	public string Text { get; set; }

	public string Key { get; set; }

	public override string ToString()
	{
		return Text;
	}

	public StiPivotMeterComboBoxItem(string text, string key)
	{
		Text = text;
		Key = key;
	}
}
