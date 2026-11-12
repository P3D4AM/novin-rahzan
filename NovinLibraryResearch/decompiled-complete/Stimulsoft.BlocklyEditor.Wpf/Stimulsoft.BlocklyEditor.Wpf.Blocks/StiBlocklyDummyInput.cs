namespace Stimulsoft.BlocklyEditor.Wpf.Blocks;

public class StiBlocklyDummyInput : StiBlocklyInput
{
	public override string GetDefinitions()
	{
		string text = string.Empty;
		foreach (StiBlocklyField field in base.Fields)
		{
			text += field.GetDefinitions();
		}
		return "this.appendDummyInput()" + text + ";";
	}
}
