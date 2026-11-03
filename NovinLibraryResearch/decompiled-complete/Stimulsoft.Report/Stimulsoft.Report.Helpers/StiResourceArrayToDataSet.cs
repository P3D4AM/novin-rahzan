using System;
using System.Data;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Helpers;

public static class StiResourceArrayToDataSet
{
	public static DataSet Get(StiResourceType resourceType, byte[] array)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		return resourceType switch
		{
			StiResourceType.Excel => ((StiFileDataConnector)StiExcelConnector.Get()).GetDataSet((StiFileDataOptions)new StiExcelOptions(array, false)), 
			StiResourceType.Csv => ((StiFileDataConnector)StiCsvConnector.Get()).GetDataSet((StiFileDataOptions)new StiCsvOptions(array, 0, ";")), 
			StiResourceType.Dbf => ((StiFileDataConnector)StiDBaseConnector.Get()).GetDataSet((StiFileDataOptions)new StiDBaseOptions(array, 0)), 
			StiResourceType.Xml => ((StiFileDataConnector)StiXmlConnector.Get()).GetDataSet((StiFileDataOptions)new StiXmlOptions(array)), 
			StiResourceType.Json => ((StiFileDataConnector)StiJsonConnector.Get()).GetDataSet((StiFileDataOptions)new StiJsonOptions(array)), 
			_ => throw new NotSupportedException($"'{resourceType}' resource type is not supported!"), 
		};
	}
}
