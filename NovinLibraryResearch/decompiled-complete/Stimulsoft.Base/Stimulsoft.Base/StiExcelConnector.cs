using System;
using System.Data;
using Stimulsoft.Base.Excel;

namespace Stimulsoft.Base;

public class StiExcelConnector : StiFileDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.ExcelDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.ExcelDataSource;

	public override string Name => "Excel";

	public override bool IsAvailable => StiAssemblyFinder.GetAssembly("LibExcel.dll") != null;

	public override StiFileType FileType => StiFileType.Excel;

	public override DataSet GetDataSet(StiFileDataOptions options)
	{
		if (!(options is StiExcelOptions stiExcelOptions))
		{
			throw new NotSupportedException("Only StiExcelOptions accepted!");
		}
		if (stiExcelOptions.Content == null || stiExcelOptions.Content.Length == 0)
		{
			return null;
		}
		return StiExcelHelper.GetDataSetFromExcelDocument(stiExcelOptions.Content, stiExcelOptions.FirstRowIsHeader);
	}

	public static StiExcelConnector Get()
	{
		return new StiExcelConnector();
	}

	private StiExcelConnector()
	{
	}
}
