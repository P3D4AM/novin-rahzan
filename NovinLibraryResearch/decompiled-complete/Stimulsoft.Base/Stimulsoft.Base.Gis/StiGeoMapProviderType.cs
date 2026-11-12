using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base.Gis;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiGeoMapProviderType
{
	OpenStreetMap,
	OpenCycleMap,
	OpenCycleMapLandscape,
	OpenCycleMapTransport,
	Wikimapia,
	Bing,
	BingSatellite,
	BingHybrid,
	BingOS,
	Google,
	GoogleSatellite,
	GoogleTerrain,
	GoogleChina,
	GoogleChinaSatellite,
	GoogleChinaTerrain,
	YandexMap,
	YandexSatelliteMap,
	Czech,
	CzechSatellite,
	CzechTurist,
	CzechTuristWinter,
	CzechGeographic,
	ArcGISStreetMapWorld2D
}
