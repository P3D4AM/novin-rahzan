namespace Stimulsoft.Report.PropertyGrid;

public abstract class StiPropertyObject
{
	protected string propertyName;

	public StiPropertyObjectId ObjectId { get; set; }

	public abstract IStiProperty GetProperty(IStiPropertyGrid progGrid, object[] obj);
}
