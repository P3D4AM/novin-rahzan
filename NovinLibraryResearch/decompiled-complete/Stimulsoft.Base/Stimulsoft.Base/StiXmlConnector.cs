using System;
using System.Data;

namespace Stimulsoft.Base;

public class StiXmlConnector : StiFileDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.XmlDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.XmlDataSource;

	public override string Name => "XML";

	public override bool IsAvailable => true;

	public override StiFileType FileType => StiFileType.Xml;

	public override DataSet GetDataSet(StiFileDataOptions options)
	{
		if (!(options is StiXmlOptions stiXmlOptions))
		{
			throw new NotSupportedException("Only StiXmlOptions accepted!");
		}
		if (stiXmlOptions.Content == null || stiXmlOptions.Content.Length == 0)
		{
			return null;
		}
		if (stiXmlOptions.IsAdoNet)
		{
			DataSet dataSet = new DataSet
			{
				EnforceConstraints = false
			};
			dataSet.Read(stiXmlOptions.Schema, stiXmlOptions.Content);
			return dataSet;
		}
		if (StiBaseOptions.DefaultJsonConverterVersion != StiJsonConverterVersion.ConverterV2)
		{
			return StiJsonToDataSetConverter.GetDataSetFromXml(stiXmlOptions.Content);
		}
		return StiJsonToDataSetConverterV2.GetDataSetFromXml(stiXmlOptions.Content);
	}

	public static StiXmlConnector Get()
	{
		return new StiXmlConnector();
	}

	private StiXmlConnector()
	{
	}
}
