using System;
using System.Data;

namespace Stimulsoft.Base;

public class StiJsonConnector : StiFileDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.JsonDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.JsonDataSource;

	public override string Name => "JSON";

	public override bool IsAvailable => true;

	public override StiFileType FileType => StiFileType.Json;

	public override DataSet GetDataSet(StiFileDataOptions options)
	{
		if (!(options is StiJsonOptions stiJsonOptions))
		{
			throw new NotSupportedException("Only StiJsonOptions accepted!");
		}
		if (stiJsonOptions.Content == null || stiJsonOptions.Content.Length == 0)
		{
			return null;
		}
		if (StiBaseOptions.DefaultJsonConverterVersion != StiJsonConverterVersion.ConverterV2)
		{
			return StiJsonToDataSetConverter.GetDataSet(stiJsonOptions.Content);
		}
		return StiJsonToDataSetConverterV2.GetDataSet(stiJsonOptions.Content);
	}

	public static StiJsonConnector Get()
	{
		return new StiJsonConnector();
	}

	private StiJsonConnector()
	{
	}
}
