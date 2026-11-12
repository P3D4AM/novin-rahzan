namespace Stimulsoft.BlocklyEditor.Wpf.Blocks;

public class StiBlocklyField
{
	public string Name { get; set; }

	public string GetDefinitions()
	{
		return ".appendField(\"" + Name + "\")";
	}
}
