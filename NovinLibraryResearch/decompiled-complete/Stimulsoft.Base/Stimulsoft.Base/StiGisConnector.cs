using System;
using System.Data;

namespace Stimulsoft.Base;

public class StiGisConnector : StiFileDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.GisDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.GisDataSource;

	public override string Name => "GIS";

	public override bool IsAvailable => true;

	public override StiFileType FileType => StiFileType.Gis;

	public override DataSet GetDataSet(StiFileDataOptions options)
	{
		if (!(options is StiGisOptions stiGisOptions))
		{
			throw new NotSupportedException("Only StiGisOptions accepted!");
		}
		if (stiGisOptions.Content == null || stiGisOptions.Content.Length == 0)
		{
			return null;
		}
		return StiGisToDataSetConverter.GetDataSetFromWkt(stiGisOptions.Content, stiGisOptions.Separator);
	}

	public static StiGisConnector Get()
	{
		return new StiGisConnector();
	}

	private StiGisConnector()
	{
	}
}
