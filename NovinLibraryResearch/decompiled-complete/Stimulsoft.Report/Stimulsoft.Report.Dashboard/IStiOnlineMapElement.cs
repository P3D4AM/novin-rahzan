using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Gis;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dashboard;

public interface IStiOnlineMapElement : IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiManuallyEnteredData
{
	StiOnlineMapLocationType LocationType { get; set; }

	StiOnlineMapCulture Culture { get; set; }

	Color LocationColor { get; set; }

	StiOnlineMapLocationColorType LocationColorType { get; set; }

	StiOnlineMapHeatmapColorGradientType HeatmapColorGradientType { get; set; }

	StiOnlineMapValueViewMode ValueViewMode { get; set; }

	StiFontIcons Icon { get; set; }

	Color IconColor { get; set; }

	byte[] CustomIcon { get; set; }

	List<IStiHeatmapGradientRange> HeatmapGradientRanges { get; set; }

	int HeatmapRadius { get; set; }

	StiGeoMapProviderType Provider { get; set; }

	void CreateNextMeter(IStiAppDataCell cell);

	void AddLatitudeMeter(IStiAppDataCell cell);

	void AddLatitudeMeter(IStiMeter meter);

	IStiMeter GetLatitudeMeter();

	void RemoveLatitudeMeter();

	void CreateNewLatitudeMeter();

	void AddLongitudeMeter(IStiAppDataCell cell);

	void AddLongitudeMeter(IStiMeter meter);

	IStiMeter GetLongitudeMeter();

	void RemoveLongitudeMeter();

	void CreateNewLongitudeMeter();

	void AddLocationMeter(IStiAppDataCell cell);

	void AddLocationMeter(IStiMeter meter);

	IStiMeter GetLocationMeter();

	void RemoveLocationMeter();

	void CreateNewLocationMeter();

	void AddLocationColorMeter(IStiAppDataCell cell);

	void AddLocationColorMeter(IStiMeter meter);

	IStiMeter GetLocationColorMeter();

	void RemoveLocationColorMeter();

	void CreateNewLocationColorMeter();

	void AddLocationValueMeter(IStiAppDataCell cell);

	void AddLocationValueMeter(IStiMeter meter);

	IStiMeter GetLocationValueMeter();

	void RemoveLocationValueMeter();

	void CreateNewLocationValueMeter();

	void AddLocationArgumentMeter(IStiAppDataCell cell);

	void AddLocationArgumentMeter(IStiMeter meter);

	IStiMeter GetLocationArgumentMeter();

	void RemoveLocationArgumentMeter();

	void CreateNewLocationArgumentMeter();

	void AddGisMeter(IStiAppDataCell cell);

	void AddGisMeter(IStiMeter meter);

	IStiMeter GetGisMeter();

	void RemoveGisMeter();

	void CreateNewGisMeter();

	void AddGisColorMeter(IStiAppDataCell cell);

	void AddGisColorMeter(IStiMeter meter);

	IStiMeter GetGisColorMeter();

	void RemoveGisColorMeter();

	void CreateNewGisColorMeter();

	void AddLineSizeMeter(IStiAppDataCell cell);

	void AddLineSizeMeter(IStiMeter meter);

	IStiMeter GetLineSizeMeter();

	void RemoveLineSizeMeter();

	void CreateNewLineSizeMeter();

	void AddDescriptionMeter(IStiAppDataCell cell);

	void AddDescriptionMeter(IStiMeter meter);

	IStiMeter GetDescriptionMeter();

	void RemoveDescriptionMeter();

	void CreateNewDescriptionMeter();

	int GetUniqueCode();
}
