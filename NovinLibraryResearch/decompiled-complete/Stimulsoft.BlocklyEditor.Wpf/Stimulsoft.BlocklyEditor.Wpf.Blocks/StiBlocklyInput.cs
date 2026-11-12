using System.Collections.Generic;

namespace Stimulsoft.BlocklyEditor.Wpf.Blocks;

public abstract class StiBlocklyInput
{
	public List<StiBlocklyField> Fields { get; set; } = new List<StiBlocklyField>();


	public abstract string GetDefinitions();
}
