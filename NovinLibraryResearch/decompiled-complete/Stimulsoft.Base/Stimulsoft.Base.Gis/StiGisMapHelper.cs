using System;

namespace Stimulsoft.Base.Gis;

public static class StiGisMapHelper
{
	public static string CustomOpenStreetMapUrl { get; set; }

	internal static string YandexMapAPIKey { get; set; }

	public static string GetProviderName(StiGeoMapProviderType id)
	{
		return id switch
		{
			StiGeoMapProviderType.OpenStreetMap => "Open Street Map", 
			StiGeoMapProviderType.OpenCycleMap => "Open Cycle Map", 
			StiGeoMapProviderType.OpenCycleMapLandscape => "Open Cycle Map Landscape", 
			StiGeoMapProviderType.OpenCycleMapTransport => "Open Cycle Map Transport", 
			StiGeoMapProviderType.Wikimapia => "Wikimapia", 
			StiGeoMapProviderType.Bing => "Bing", 
			StiGeoMapProviderType.BingSatellite => "Bing Satellite", 
			StiGeoMapProviderType.BingHybrid => "Bing Hybrid", 
			StiGeoMapProviderType.BingOS => "Bing OS", 
			StiGeoMapProviderType.Google => "Google", 
			StiGeoMapProviderType.GoogleSatellite => "Google Satellite", 
			StiGeoMapProviderType.GoogleTerrain => "Google Terrain", 
			StiGeoMapProviderType.GoogleChina => "Google China", 
			StiGeoMapProviderType.GoogleChinaSatellite => "Google China Satellite", 
			StiGeoMapProviderType.GoogleChinaTerrain => "Google China Terrain", 
			StiGeoMapProviderType.YandexMap => "Yandex Map", 
			StiGeoMapProviderType.YandexSatelliteMap => "Yandex Satellite Map", 
			StiGeoMapProviderType.Czech => "Czech", 
			StiGeoMapProviderType.CzechSatellite => "Czech Satellite", 
			StiGeoMapProviderType.CzechTurist => "Czech Turist", 
			StiGeoMapProviderType.CzechTuristWinter => "Czech Turist Winter", 
			StiGeoMapProviderType.CzechGeographic => "Czech Geographic", 
			StiGeoMapProviderType.ArcGISStreetMapWorld2D => "Arc GIS StreetMap World 2D", 
			_ => throw new NotSupportedException($"Type '{id}' is not supported"), 
		};
	}
}
