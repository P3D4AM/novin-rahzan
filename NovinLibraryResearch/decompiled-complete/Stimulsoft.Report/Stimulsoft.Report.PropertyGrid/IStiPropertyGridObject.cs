using Stimulsoft.Base;

namespace Stimulsoft.Report.PropertyGrid;

public interface IStiPropertyGridObject
{
	StiComponentId ComponentId { get; }

	string PropName { get; }

	StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level);

	StiEventCollection GetEvents(IStiPropertyGrid propertyGrid);
}
