namespace Stimulsoft.Report.Design.Styles;

internal sealed class StiStyleDesignerChangeName
{
	public string OldName { get; set; }

	public string NewName { get; set; }

	public StiStyleDesignerChangeName(string oldName, string newName)
	{
		OldName = oldName;
		NewName = newName;
	}
}
