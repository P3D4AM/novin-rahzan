using System;

namespace Stimulsoft.Report.Dashboard;

public sealed class StiElementChangedArgs : EventArgs
{
	public StiElementMeterAction Action { get; set; }

	public string OldName { get; set; }

	public string NewName { get; set; }

	public static StiElementChangedArgs CreateEmptyArgs()
	{
		return new StiElementChangedArgs();
	}

	public static StiElementChangedArgs CreateRenamingArgs(string oldName, string newName)
	{
		return new StiElementChangedArgs
		{
			Action = StiElementMeterAction.Rename,
			OldName = oldName,
			NewName = newName
		};
	}

	public static StiElementChangedArgs CreateDeletingArgs(string name)
	{
		return new StiElementChangedArgs
		{
			Action = StiElementMeterAction.Delete,
			OldName = name
		};
	}

	public static StiElementChangedArgs CreateClearingAllArgs()
	{
		return new StiElementChangedArgs
		{
			Action = StiElementMeterAction.ClearAll
		};
	}
}
