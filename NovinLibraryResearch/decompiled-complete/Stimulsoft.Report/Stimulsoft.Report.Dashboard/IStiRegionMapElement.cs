using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Dashboard;

public interface IStiRegionMapElement : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiUserFilters, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters
{
	string MapIdent { get; set; }

	StiMapSource DataFrom { get; set; }

	string MapData { get; set; }

	StiMapType MapType { get; set; }

	bool ShowValue { get; set; }

	bool ColorEach { get; set; }

	bool ShortValue { get; set; }

	bool ShowBubble { get; set; }

	StiDisplayNameType ShowName { get; set; }

	List<StiMapData> GetMapData();

	void CreateNextMeter(IStiAppDataCell cell);

	void AddKeyMeter(IStiAppDataCell cell);

	void AddKeyMeter(IStiMeter meter);

	IStiMeter GetKeyMeter();

	void RemoveKeyMeter();

	void CreateNewKeyMeter();

	void AddNameMeter(IStiAppDataCell cell);

	void AddNameMeter(IStiMeter meter);

	IStiMeter GetNameMeter();

	void RemoveNameMeter();

	void CreateNewNameMeter();

	void AddValueMeter(IStiAppDataCell cell);

	void AddValueMeter(IStiMeter meter);

	IStiMeter GetValueMeter();

	void RemoveValueMeter();

	void CreateNewValueMeter();

	void AddGroupMeter(IStiAppDataCell cell);

	void AddGroupMeter(IStiMeter meter);

	IStiMeter GetGroupMeter();

	void RemoveGroupMeter();

	void CreateNewGroupMeter();

	void AddColorMeter(IStiAppDataCell cell);

	void AddColorMeter(IStiMeter meter);

	IStiMeter GetColorMeter();

	void RemoveColorMeter();

	void CreateNewColorMeter();
}
