namespace Stimulsoft.BlocklyEditor.Wpf.Blocks;

public class StiBlocklyValueInput : StiBlocklyInput
{
	public string Name { get; set; }

	public override string GetDefinitions()
	{
		string text = string.Empty;
		foreach (StiBlocklyField field in base.Fields)
		{
			text += field.GetDefinitions();
		}
		return "this.appendValueInput(\"" + Name + "\").setAlign(Blockly.ALIGN_RIGHT)" + text + ";";
	}
}
