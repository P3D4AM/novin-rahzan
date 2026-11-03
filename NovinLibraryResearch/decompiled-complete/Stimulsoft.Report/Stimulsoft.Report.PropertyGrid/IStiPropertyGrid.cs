using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.PropertyGrid;

public interface IStiPropertyGrid
{
	bool IsLocalizePropertyGrid { get; set; }

	IStiDesignerBase DesignerControl { get; }

	IStiPropertiesHelper PropertiesHelper { get; }

	void OnChangeValue(string propertyName, object oldValue, object newValue);

	void UpdateReport(bool updatePage, bool updateOnlyComponents, bool updatePropertyGrid, bool updateSelection, bool allowAnimation = false);
}
