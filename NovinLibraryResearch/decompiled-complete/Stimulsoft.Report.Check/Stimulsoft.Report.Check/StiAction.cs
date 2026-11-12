namespace Stimulsoft.Report.Check;

public abstract class StiAction
{
	public abstract string Name { get; }

	public abstract string Description { get; }

	public virtual void Invoke(StiReport report, object element, string elementName)
	{
		if (report.Designer != null)
		{
			report.Designer.UndoRedoSave(Description);
		}
	}
}
