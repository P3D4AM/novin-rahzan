using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report;

public sealed class Totals
{
	private class StiFunctionCalcData
	{
		public bool IsCompilationMode = true;

		public List<StiParser.StiAsmCommand> AsmList;

		public List<StiParser.StiAsmCommand> AsmList2;

		public List<StiParser.StiAsmCommand> ConditionAsmList;

		public StiParser Parser;

		public MethodInfo Method;

		public MethodInfo Method2;

		public StiAggregateFunctionService Function;

		public StiSumDistinctDecimalFunctionService SumDistinctFunction;

		public StiReport Report;
	}

	public static MethodInfo GetMethod(StiReport report, string name)
	{
		return report.GetType().GetMethod(name);
	}

	private static object Calculate(StiAggregateFunctionService function, object data, StiReport report, string name, bool allLevels, bool onlyChilds, bool isPageTotal)
	{
		StiReport stiReport = report;
		if (stiReport == null)
		{
			object obj = data;
			if (data is StiParser.StiParserData stiParserData)
			{
				obj = stiParserData.Data;
			}
			if (obj is StiDataSource)
			{
				StiDictionary dictionary = (obj as StiDataSource).Dictionary;
				if (dictionary != null)
				{
					stiReport = dictionary.Report;
				}
			}
			if (obj is StiBand)
			{
				stiReport = (obj as StiBand).Report;
			}
		}
		bool flag = stiReport != null && stiReport.CacheTotals && data != null;
		object result = null;
		string storeId = null;
		if (flag && GetCachedValue(function, data, stiReport, name, allLevels, onlyChilds, isPageTotal, ref result, out storeId))
		{
			return result;
		}
		if (stiReport != null)
		{
			stiReport.CachedTotalsLocked = true;
		}
		result = Calculate1(function, data, report, name, allLevels, onlyChilds, isPageTotal);
		if (stiReport != null)
		{
			stiReport.CachedTotalsLocked = false;
		}
		if (flag)
		{
			StoreCachedValue(data, stiReport, storeId, result);
		}
		return result;
	}

	private static object Calculate1(StiAggregateFunctionService function, object data, StiReport report, string name, bool allLevels, bool onlyChilds, bool isPageTotal)
	{
		//IL_0ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bab: Invalid comparison between Unknown and I4
		//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfc: Invalid comparison between Unknown and I4
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_0958: Invalid comparison between Unknown and I4
		//IL_09b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Invalid comparison between Unknown and I4
		//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Invalid comparison between Unknown and I4
		//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7d: Invalid comparison between Unknown and I4
		//IL_0a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a52: Invalid comparison between Unknown and I4
		bool flag = true;
		if (report != null)
		{
			flag = report.CalculationMode == StiCalculationMode.Compilation;
		}
		StiFunctionCalcData stiFunctionCalcData = new StiFunctionCalcData();
		if (!flag)
		{
			if (data is StiParser.StiParserData stiParserData)
			{
				stiFunctionCalcData.AsmList = stiParserData.AsmList;
				stiFunctionCalcData.AsmList2 = stiParserData.AsmList2;
				stiFunctionCalcData.ConditionAsmList = stiParserData.ConditionAsmList;
				stiFunctionCalcData.Parser = stiParserData.Parser;
				data = stiParserData.Data;
			}
			else
			{
				flag = true;
			}
		}
		stiFunctionCalcData.IsCompilationMode = flag;
		stiFunctionCalcData.Report = report;
		stiFunctionCalcData.Function = function;
		string text = null;
		if (function is StiSumDistinctDecimalFunctionService sumDistinctFunction)
		{
			if (flag)
			{
				string[] array = name.Split(new char[1] { '~' });
				if (array.Length > 1)
				{
					name = array[0];
					text = array[1];
					stiFunctionCalcData.SumDistinctFunction = sumDistinctFunction;
				}
			}
			else if (stiFunctionCalcData.AsmList2 != null)
			{
				stiFunctionCalcData.SumDistinctFunction = sumDistinctFunction;
			}
		}
		if (flag)
		{
			if (name != null)
			{
				stiFunctionCalcData.Method = GetMethod(report, name);
				if (stiFunctionCalcData.Method == null)
				{
					return 0;
				}
			}
			if (text != null)
			{
				stiFunctionCalcData.Method2 = GetMethod(report, text);
				if (stiFunctionCalcData.Method2 == null)
				{
					return 0;
				}
			}
		}
		StiDataBand stiDataBand = data as StiDataBand;
		if (stiDataBand != null && isPageTotal)
		{
			if (report == null)
			{
				report = stiDataBand.Report;
			}
			function.Init();
			SaveState(stiDataBand);
			if (!stiDataBand.IsBusinessObjectEmpty)
			{
				stiDataBand.BusinessObject.CreateEnumerator();
				stiDataBand.BusinessObject.specTotalsCalculation = true;
			}
			if (report.Engine != null && report.Engine.IndexPageForPageTotal != -1)
			{
				int num = report.Engine.IndexPageForPageTotal;
				if (report.Engine.StartIndexPageForPageTotal != -1)
				{
					num = report.Engine.StartIndexPageForPageTotal;
				}
				string text2 = "Breaked_" + stiDataBand.Name;
				string text3 = "Continued_" + stiDataBand.Name;
				for (int i = num; i <= report.Engine.IndexPageForPageTotal; i++)
				{
					StiPage stiPage = report.RenderedPages[i];
					StiComponentsCollection components = stiPage.GetComponents();
					foreach (StiComponent item in components)
					{
						if (!(item is StiContainer stiContainer) || (!(stiDataBand.Name == stiContainer.Name) && !(text2 == stiContainer.Name) && !(text3 == stiContainer.Name)) || stiContainer.ContainerInfoV2.DataBandPosition == -1)
						{
							continue;
						}
						DataRow[] detailRows = null;
						object currentObject = null;
						if (!stiDataBand.IsDataSourceEmpty)
						{
							stiDataBand.Position = stiContainer.ContainerInfoV2.DataBandPosition;
							if (stiContainer.ContainerInfoV2.DataSourceRow != null)
							{
								stiDataBand.Position = 0;
								detailRows = stiDataBand.DataSource.DetailRows;
								stiDataBand.DataSource.DetailRows = new DataRow[1];
								stiDataBand.DataSource.DetailRows[0] = stiContainer.ContainerInfoV2.DataSourceRow;
							}
						}
						if (!stiDataBand.IsBusinessObjectEmpty && stiContainer.ContainerInfoV2.BusinessObjectCurrent != null)
						{
							currentObject = stiDataBand.BusinessObject.currentObject;
							stiDataBand.BusinessObject.currentObject = stiContainer.ContainerInfoV2.BusinessObjectCurrent;
						}
						CalcItem(stiFunctionCalcData);
						if (!stiDataBand.IsDataSourceEmpty && stiContainer.ContainerInfoV2.DataSourceRow != null)
						{
							stiDataBand.DataSource.DetailRows = detailRows;
						}
						if (!stiDataBand.IsBusinessObjectEmpty && stiContainer.ContainerInfoV2.BusinessObjectCurrent != null)
						{
							stiDataBand.BusinessObject.currentObject = currentObject;
						}
					}
				}
			}
			if (!stiDataBand.IsBusinessObjectEmpty)
			{
				stiDataBand.BusinessObject.specTotalsCalculation = false;
			}
			RestoreState(stiDataBand);
			return function.GetValue();
		}
		if (data is StiDataSource stiDataSource)
		{
			if (allLevels)
			{
				throw new Exception("You can't use AllLevels together with DataSource! Only HierarchicalBand can be used.");
			}
			function.Init();
			stiDataSource.SaveState("Totals");
			stiDataSource.First();
			stiDataSource.ResetData();
			stiDataSource.ResetDetailsRows();
			while (!stiDataSource.IsEof)
			{
				CalcItem(stiFunctionCalcData);
				stiDataSource.Next();
			}
			stiDataSource.RestoreState("Totals");
			return function.GetValue();
		}
		if (data is StiBusinessObject stiBusinessObject)
		{
			if (allLevels)
			{
				throw new Exception("You can't use AllLevels together with BusinessObject! Only HierarchicalBand can be used.");
			}
			function.Init();
			stiBusinessObject.SaveState("Totals");
			stiBusinessObject.CreateEnumerator();
			stiBusinessObject.specTotalsCalculation = true;
			while (!stiBusinessObject.IsEof)
			{
				CalcItem(stiFunctionCalcData);
				stiBusinessObject.Next();
			}
			stiBusinessObject.specTotalsCalculation = false;
			stiBusinessObject.RestoreState("Totals");
			return function.GetValue();
		}
		if (data is StiHierarchicalBand stiHierarchicalBand)
		{
			bool flag2 = false;
			if (!allLevels && onlyChilds)
			{
				flag2 = true;
				allLevels = true;
			}
			if (report == null)
			{
				report = stiHierarchicalBand.Report;
			}
			function.Init();
			if (!stiHierarchicalBand.IsDataSourceEmpty)
			{
				stiHierarchicalBand.SaveState("Totals");
				if (stiHierarchicalBand.HierarchicalBandInfoV2.FinalFooterCalculation)
				{
					stiHierarchicalBand.Position = 0;
				}
				int num2 = stiHierarchicalBand.DataSource.GetLevel();
				if (stiHierarchicalBand.HierarchicalBandInfoV2.SpecifiedLevel != -1)
				{
					num2 = stiHierarchicalBand.HierarchicalBandInfoV2.SpecifiedLevel;
				}
				if (stiHierarchicalBand.DataSource.Count > 0 && (stiHierarchicalBand.HierarchicalBandInfoV2.SpecifiedLevel == -1 || allLevels) && !onlyChilds)
				{
					CalcItem(stiFunctionCalcData);
				}
				int position = stiHierarchicalBand.Position;
				while (stiHierarchicalBand.Position > 0 && !onlyChilds)
				{
					stiHierarchicalBand.Position--;
					int level = stiHierarchicalBand.DataSource.GetLevel();
					if (level < num2)
					{
						break;
					}
					if (allLevels || level == num2)
					{
						CalcItem(stiFunctionCalcData);
					}
				}
				stiHierarchicalBand.Position = position;
				while (stiHierarchicalBand.Position < stiHierarchicalBand.Count - 1)
				{
					stiHierarchicalBand.Position++;
					int level2 = stiHierarchicalBand.DataSource.GetLevel();
					if (level2 < num2)
					{
						break;
					}
					if (onlyChilds)
					{
						if (flag2 && level2 - num2 > 1)
						{
							continue;
						}
						if (level2 == num2)
						{
							break;
						}
					}
					if (allLevels || level2 == num2)
					{
						CalcItem(stiFunctionCalcData);
					}
				}
				stiHierarchicalBand.Position = position;
				stiHierarchicalBand.RestoreState("Totals");
			}
			if (!stiHierarchicalBand.IsBusinessObjectEmpty)
			{
				int position2 = stiHierarchicalBand.Position;
				stiHierarchicalBand.SaveState("Totals");
				List<object> list = new List<object>();
				stiHierarchicalBand.SaveState("Totals2");
				stiHierarchicalBand.BusinessObject.CreateEnumerator();
				while (!stiHierarchicalBand.BusinessObject.IsEof)
				{
					list.Add(stiHierarchicalBand.BusinessObject.enumerator.Current);
					stiHierarchicalBand.BusinessObject.Next();
				}
				stiHierarchicalBand.RestoreState("Totals2");
				if (stiHierarchicalBand.HierarchicalBandInfoV2.FinalFooterCalculation)
				{
					stiHierarchicalBand.Position = 0;
				}
				stiHierarchicalBand.BusinessObject.currentObject = list[stiHierarchicalBand.Position];
				int num3 = stiHierarchicalBand.BusinessObject.GetLevel();
				if (stiHierarchicalBand.HierarchicalBandInfoV2.SpecifiedLevel != -1)
				{
					num3 = stiHierarchicalBand.HierarchicalBandInfoV2.SpecifiedLevel;
				}
				if (stiHierarchicalBand.BusinessObject.Count > 0 && (stiHierarchicalBand.HierarchicalBandInfoV2.SpecifiedLevel == -1 || allLevels) && !onlyChilds)
				{
					CalcItem(stiFunctionCalcData);
				}
				int position3 = stiHierarchicalBand.Position;
				while (stiHierarchicalBand.Position > 0 && !onlyChilds)
				{
					stiHierarchicalBand.Position--;
					stiHierarchicalBand.BusinessObject.currentObject = list[stiHierarchicalBand.Position];
					int level3 = stiHierarchicalBand.BusinessObject.GetLevel();
					if (level3 < num3)
					{
						break;
					}
					if (allLevels || level3 == num3)
					{
						CalcItem(stiFunctionCalcData);
					}
				}
				stiHierarchicalBand.Position = position3;
				while (stiHierarchicalBand.Position < stiHierarchicalBand.Count - 1)
				{
					stiHierarchicalBand.Position++;
					stiHierarchicalBand.BusinessObject.currentObject = list[stiHierarchicalBand.Position];
					int level4 = stiHierarchicalBand.BusinessObject.GetLevel();
					if (level4 < num3)
					{
						break;
					}
					if (onlyChilds)
					{
						if (flag2 && level4 - num3 > 1)
						{
							continue;
						}
						if (level4 == num3)
						{
							break;
						}
					}
					if (allLevels || level4 == num3)
					{
						CalcItem(stiFunctionCalcData);
					}
				}
				stiHierarchicalBand.Position = position3;
				stiHierarchicalBand.RestoreState("Totals");
			}
			return function.GetValue();
		}
		if (data is StiDataBand stiDataBand2)
		{
			if (allLevels)
			{
				throw new Exception("You can't use AllLevels together with DataBand! Only HierarchicalBand can be used.");
			}
			if (report == null)
			{
				report = stiDataBand2.Report;
			}
			function.Init();
			if (stiDataBand2.BusinessObject != null)
			{
				StiBusinessObject businessObject = stiDataBand2.BusinessObject;
				businessObject.SaveState("Totals");
				businessObject.CreateEnumerator();
				businessObject.specTotalsCalculation = true;
				while (!businessObject.IsEof)
				{
					CalcItem(stiFunctionCalcData);
					businessObject.Next();
				}
				businessObject.specTotalsCalculation = false;
				businessObject.RestoreState("Totals");
			}
			else
			{
				SaveState(stiDataBand2);
				StiDataHelper.SetData(stiDataBand2, reinit: false);
				stiDataBand2.First();
				while (!stiDataBand2.IsEof)
				{
					CalcItem(stiFunctionCalcData);
					stiDataBand2.Next();
				}
				RestoreState(stiDataBand2);
			}
			return function.GetValue();
		}
		if (data is StiGroupHeaderBand stiGroupHeaderBand)
		{
			if (allLevels)
			{
				throw new Exception("You can't use AllLevels together with StiGroupHeaderBand! Only HierarchicalBand can be used.");
			}
			if (report == null)
			{
				report = stiGroupHeaderBand.Report;
			}
			function.Init();
			StiDataBand dataBand = stiGroupHeaderBand.GetDataBand();
			if (dataBand == null)
			{
				return 0;
			}
			SaveState(dataBand);
			bool[] array2 = null;
			bool[] array3 = null;
			if ((int)report.EngineVersion == 2 && dataBand.DataBandInfoV2 != null)
			{
				if (dataBand.DataBandInfoV2.GroupHeaderResults != null)
				{
					array2 = dataBand.DataBandInfoV2.GroupHeaderResults.Clone() as bool[];
				}
				if (dataBand.DataBandInfoV2.GroupFooterResults != null)
				{
					array3 = dataBand.DataBandInfoV2.GroupFooterResults.Clone() as bool[];
				}
			}
			StiDataBandV1Builder stiDataBandV1Builder = null;
			StiDataBandV2Builder stiDataBandV2Builder = null;
			if ((int)report.EngineVersion == 1)
			{
				stiDataBandV1Builder = StiV1Builder.GetBuilder(typeof(StiDataBand)) as StiDataBandV1Builder;
			}
			else
			{
				stiDataBandV2Builder = StiV2Builder.GetBuilder(typeof(StiDataBand)) as StiDataBandV2Builder;
			}
			StiBusinessObject businessObject2 = dataBand.BusinessObject;
			bool flag3 = false;
			bool flag4 = false;
			if ((int)report.EngineVersion == 1)
			{
				flag3 = stiDataBandV1Builder.GetGroupHeaderResult(dataBand, stiGroupHeaderBand);
				flag4 = stiDataBandV1Builder.GetGroupFooterResult(dataBand, stiGroupHeaderBand);
			}
			else
			{
				StiDataBandV2Builder.PrepareGroupResults(dataBand);
				flag3 = stiDataBandV2Builder.GetGroupHeaderResult(dataBand, stiGroupHeaderBand);
				flag4 = stiDataBandV2Builder.GetGroupFooterResult(dataBand, stiGroupHeaderBand);
			}
			if (flag3 && flag4)
			{
				try
				{
					if (report != null && (int)report.EngineVersion == 1)
					{
						StiDataBandV1Builder.SetDetails(dataBand);
					}
					else
					{
						StiDataBandV2Builder.SetDetails(dataBand);
					}
					CalcItem(stiFunctionCalcData);
				}
				catch
				{
				}
			}
			else
			{
				if (businessObject2 != null)
				{
					int num4 = businessObject2.Position;
					businessObject2.SaveState("Totals");
					businessObject2.CreateEnumerator();
					businessObject2.specTotalsCalculation = true;
					List<bool> list2 = new List<bool>();
					int j;
					for (j = 0; j != num4; j++)
					{
						if (j == 0)
						{
							StiDataBandV2Builder.PrepareGroupResults(dataBand);
						}
						list2.Add(stiDataBandV2Builder.GetGroupHeaderResult(dataBand, stiGroupHeaderBand));
						dataBand.Next();
					}
					j--;
					if (!flag3)
					{
						Hashtable hashtable = new Hashtable();
						if (num4 > 0)
						{
							try
							{
								do
								{
									if (list2[j])
									{
										hashtable[j] = j;
										break;
									}
									hashtable[j] = j;
									j--;
								}
								while (j >= 0);
							}
							catch
							{
								num4 = 0;
							}
						}
						businessObject2.First();
						j = 0;
						while (j != num4)
						{
							if (hashtable[j] != null)
							{
								CalcItem(stiFunctionCalcData);
							}
							j++;
							businessObject2.Next();
						}
					}
				}
				else
				{
					while (true)
					{
						if ((int)report.EngineVersion == 1)
						{
							if (stiDataBandV1Builder.GetGroupHeaderResult(dataBand, stiGroupHeaderBand))
							{
								break;
							}
						}
						else if (stiDataBandV2Builder.GetGroupHeaderResult(dataBand, stiGroupHeaderBand))
						{
							break;
						}
						dataBand.Prior();
					}
				}
				while (true)
				{
					if ((int)report.EngineVersion == 1)
					{
						if (stiDataBandV1Builder.GetGroupFooterResult(dataBand, stiGroupHeaderBand))
						{
							break;
						}
					}
					else if (stiDataBandV2Builder.GetGroupFooterResult(dataBand, stiGroupHeaderBand))
					{
						break;
					}
					CalcItem(stiFunctionCalcData);
					if (businessObject2 != null)
					{
						if (businessObject2.IsEof)
						{
							break;
						}
						businessObject2.Next();
						StiDataBandV2Builder.PrepareGroupResults(dataBand);
					}
					else
					{
						dataBand.Next();
					}
				}
				if ((int)report.EngineVersion == 1)
				{
					flag3 = stiDataBandV1Builder.GetGroupHeaderResult(dataBand, stiGroupHeaderBand);
					flag4 = stiDataBandV1Builder.GetGroupFooterResult(dataBand, stiGroupHeaderBand);
				}
				else
				{
					flag3 = stiDataBandV2Builder.GetGroupHeaderResult(dataBand, stiGroupHeaderBand);
					flag4 = stiDataBandV2Builder.GetGroupFooterResult(dataBand, stiGroupHeaderBand);
				}
				if (!flag3 && flag4)
				{
					CalcItem(stiFunctionCalcData);
				}
			}
			if (businessObject2 != null)
			{
				businessObject2.specTotalsCalculation = false;
				businessObject2.RestoreState("Totals");
			}
			RestoreState(dataBand);
			if (array2 != null)
			{
				dataBand.DataBandInfoV2.GroupHeaderResults = array2;
			}
			if (array3 != null)
			{
				dataBand.DataBandInfoV2.GroupFooterResults = array3;
			}
			return function.GetValue();
		}
		return 0;
	}

	private static void CalcItem(StiFunctionCalcData calcData)
	{
		try
		{
			object value = null;
			if (calcData.IsCompilationMode)
			{
				if (calcData.Method != null)
				{
					StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
					calcData.Method.Invoke(calcData.Report, new object[2] { calcData.Report, stiValueEventArgs });
					value = stiValueEventArgs.Value;
				}
				if (calcData.SumDistinctFunction != null)
				{
					StiValueEventArgs stiValueEventArgs2 = new StiValueEventArgs();
					calcData.Method2.Invoke(calcData.Report, new object[2] { calcData.Report, stiValueEventArgs2 });
					object value2 = stiValueEventArgs2.Value;
					calcData.SumDistinctFunction.CalcItem(value, value2);
				}
				else
				{
					calcData.Function.CalcItem(value);
				}
			}
			else if (calcData.ConditionAsmList == null || Convert.ToBoolean(calcData.Parser.ExecuteAsm(calcData.ConditionAsmList)))
			{
				value = calcData.Parser.ExecuteAsm(calcData.AsmList);
				if (calcData.SumDistinctFunction != null)
				{
					object valueToSum = calcData.Parser.ExecuteAsm(calcData.AsmList2);
					calcData.SumDistinctFunction.CalcItem(value, valueToSum);
				}
				else
				{
					calcData.Function.CalcItem(value);
				}
			}
		}
		catch
		{
		}
	}

	private static object CalculateByCondition(StiAggregateFunctionService function, object data, StiReport report, string name, StiFilterCondition filterCondition, decimal value1, decimal value2, bool allLevels, bool onlyChilds)
	{
		bool flag = true;
		if (report != null)
		{
			flag = report.CalculationMode == StiCalculationMode.Compilation;
		}
		List<StiParser.StiAsmCommand> objectAsmList = null;
		StiParser stiParser = null;
		if (!flag)
		{
			if (data is StiParser.StiParserData stiParserData)
			{
				objectAsmList = stiParserData.AsmList;
				stiParser = stiParserData.Parser;
				data = stiParserData.Data;
			}
			else
			{
				flag = true;
			}
		}
		MethodInfo methodInfo = null;
		if (flag && name != null)
		{
			methodInfo = GetMethod(report, name);
			if (methodInfo == null)
			{
				return 0;
			}
		}
		if (data is StiHierarchicalBand stiHierarchicalBand)
		{
			bool flag2 = false;
			if (!allLevels && onlyChilds)
			{
				flag2 = true;
				allLevels = true;
			}
			if (report == null)
			{
				report = stiHierarchicalBand.Report;
			}
			function.Init();
			stiHierarchicalBand.SaveState("Totals");
			List<object> list = new List<object>();
			if (stiHierarchicalBand.BusinessObject != null)
			{
				stiHierarchicalBand.SaveState("Totals2");
				stiHierarchicalBand.BusinessObject.CreateEnumerator();
				while (!stiHierarchicalBand.BusinessObject.IsEof)
				{
					list.Add(stiHierarchicalBand.BusinessObject.enumerator.Current);
					stiHierarchicalBand.BusinessObject.Next();
				}
				stiHierarchicalBand.RestoreState("Totals2");
			}
			if (stiHierarchicalBand.HierarchicalBandInfoV2.FinalFooterCalculation)
			{
				stiHierarchicalBand.Position = 0;
			}
			if (stiHierarchicalBand.BusinessObject != null)
			{
				stiHierarchicalBand.BusinessObject.currentObject = list[stiHierarchicalBand.Position];
			}
			int num = ((stiHierarchicalBand.DataSource != null) ? stiHierarchicalBand.DataSource.GetLevel() : stiHierarchicalBand.BusinessObject.GetLevel());
			if (stiHierarchicalBand.HierarchicalBandInfoV2.SpecifiedLevel != -1)
			{
				num = stiHierarchicalBand.HierarchicalBandInfoV2.SpecifiedLevel;
			}
			if ((list.Count > 0 || stiHierarchicalBand.DataSource.Count > 0) && (stiHierarchicalBand.HierarchicalBandInfoV2.SpecifiedLevel == -1 || allLevels) && !onlyChilds)
			{
				try
				{
					object value3 = null;
					if (flag)
					{
						if (methodInfo != null)
						{
							StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
							methodInfo.Invoke(report, new object[2] { report, stiValueEventArgs });
							value3 = CompareValue(stiValueEventArgs.Value, filterCondition, value1, value2);
						}
					}
					else
					{
						value3 = CompareValue(stiParser.ExecuteAsm(objectAsmList), filterCondition, value1, value2);
					}
					function.CalcItem(value3);
				}
				catch
				{
				}
			}
			int position = stiHierarchicalBand.Position;
			while (stiHierarchicalBand.Position > 0 && !onlyChilds)
			{
				stiHierarchicalBand.Position--;
				if (stiHierarchicalBand.BusinessObject != null)
				{
					stiHierarchicalBand.BusinessObject.currentObject = list[stiHierarchicalBand.Position];
				}
				int num2 = ((stiHierarchicalBand.DataSource != null) ? stiHierarchicalBand.DataSource.GetLevel() : stiHierarchicalBand.BusinessObject.GetLevel());
				if (num2 < num)
				{
					break;
				}
				if (!allLevels && num2 != num)
				{
					continue;
				}
				try
				{
					object value4 = null;
					if (flag)
					{
						if (methodInfo != null)
						{
							StiValueEventArgs stiValueEventArgs2 = new StiValueEventArgs();
							methodInfo.Invoke(report, new object[2] { report, stiValueEventArgs2 });
							value4 = CompareValue(stiValueEventArgs2.Value, filterCondition, value1, value2);
						}
					}
					else
					{
						value4 = CompareValue(stiParser.ExecuteAsm(objectAsmList), filterCondition, value1, value2);
					}
					function.CalcItem(value4);
				}
				catch
				{
				}
			}
			stiHierarchicalBand.Position = position;
			while (stiHierarchicalBand.Position < stiHierarchicalBand.Count - 1)
			{
				stiHierarchicalBand.Position++;
				if (stiHierarchicalBand.BusinessObject != null)
				{
					stiHierarchicalBand.BusinessObject.currentObject = list[stiHierarchicalBand.Position];
				}
				int num3 = ((stiHierarchicalBand.DataSource != null) ? stiHierarchicalBand.DataSource.GetLevel() : stiHierarchicalBand.BusinessObject.GetLevel());
				if (num3 < num)
				{
					break;
				}
				if (onlyChilds)
				{
					if (flag2 && num3 - num > 1)
					{
						continue;
					}
					if (num3 == num)
					{
						break;
					}
				}
				if (!allLevels && num3 != num)
				{
					continue;
				}
				try
				{
					object value5 = null;
					if (flag)
					{
						if (methodInfo != null)
						{
							StiValueEventArgs stiValueEventArgs3 = new StiValueEventArgs();
							methodInfo.Invoke(report, new object[2] { report, stiValueEventArgs3 });
							value5 = CompareValue(stiValueEventArgs3.Value, filterCondition, value1, value2);
						}
					}
					else
					{
						value5 = CompareValue(stiParser.ExecuteAsm(objectAsmList), filterCondition, value1, value2);
					}
					function.CalcItem(value5);
				}
				catch
				{
				}
			}
			stiHierarchicalBand.Position = position;
			stiHierarchicalBand.RestoreState("Totals");
			return function.GetValue();
		}
		return 0;
	}

	private static decimal CalculateDecimalByCondition(StiAggregateFunctionService function, object data, StiReport report, string name, StiFilterCondition filterCondition, decimal value1, decimal value2, bool allLevels, bool onlyChilds)
	{
		object obj = CalculateByCondition(function, data, report, name, filterCondition, value1, value2, allLevels, onlyChilds);
		return StiObjectConverter.ConvertToDecimal(obj);
	}

	private static object CompareValue(object value, StiFilterCondition filterCondition, decimal value1, decimal value2)
	{
		decimal result = default(decimal);
		decimal.TryParse(value.ToString(), out result);
		switch (filterCondition)
		{
		case StiFilterCondition.Between:
			return (result >= value1 && result <= value2) ? result : 0m;
		case StiFilterCondition.EqualTo:
			return (result == value1) ? result : 0m;
		case StiFilterCondition.GreaterThan:
			return (result > value1) ? result : 0m;
		case StiFilterCondition.GreaterThanOrEqualTo:
			return (result >= value1) ? result : 0m;
		case StiFilterCondition.LessThan:
			return (result < value1) ? result : 0m;
		case StiFilterCondition.LessThanOrEqualTo:
			return (result <= value1) ? result : 0m;
		case StiFilterCondition.NotBetween:
			return (result < value1 || result > value2) ? result : 0m;
		case StiFilterCondition.NotEqualTo:
			return (result != value1) ? result : 0m;
		case StiFilterCondition.Containing:
		case StiFilterCondition.NotContaining:
		case StiFilterCondition.BeginningWith:
		case StiFilterCondition.EndingWith:
			return 0;
		default:
			return 0;
		}
	}

	private static decimal CalculateDecimal(StiAggregateFunctionService function, object data, StiReport report, string name, bool allLevels, bool onlyChilds)
	{
		object obj = Calculate(function, data, report, name, allLevels, onlyChilds, isPageTotal: false);
		return StiObjectConverter.ConvertToDecimal(obj);
	}

	private static decimal? CalculateDecimalNullable(StiAggregateFunctionService function, object data, StiReport report, string name, bool allLevels, bool onlyChilds)
	{
		object obj = Calculate(function, data, report, name, allLevels, onlyChilds, isPageTotal: false);
		if (obj == null)
		{
			return null;
		}
		return StiObjectConverter.ConvertToDecimal(obj);
	}

	private static double CalculateDouble(StiAggregateFunctionService function, object data, StiReport report, string name, bool allLevels, bool onlyChilds)
	{
		object obj = Calculate(function, data, report, name, allLevels, onlyChilds, isPageTotal: false);
		return StiObjectConverter.ConvertToDouble(obj);
	}

	private static long CalculateInt64(StiAggregateFunctionService function, object data, StiReport report, string name, bool allLevels, bool onlyChilds)
	{
		object obj = Calculate(function, data, report, name, allLevels, onlyChilds, isPageTotal: false);
		return StiObjectConverter.ConvertToInt64(obj);
	}

	private static object Calculate(StiAggregateFunctionService function, object data, StiReport report, string name)
	{
		return Calculate(function, data, report, name, allLevels: false, onlyChilds: false, isPageTotal: true);
	}

	private static decimal CalculateDecimal(StiAggregateFunctionService function, object data, StiReport report, string name)
	{
		return StiObjectConverter.ConvertToDecimal(Calculate(function, data, report, name, allLevels: false, onlyChilds: false, isPageTotal: true));
	}

	private static double CalculateDouble(StiAggregateFunctionService function, object data, StiReport report, string name)
	{
		return StiObjectConverter.ConvertToDouble(Calculate(function, data, report, name, allLevels: false, onlyChilds: false, isPageTotal: true));
	}

	private static long CalculateInt64(StiAggregateFunctionService function, object data, StiReport report, string name)
	{
		return StiObjectConverter.ConvertToInt64(Calculate(function, data, report, name, allLevels: false, onlyChilds: false, isPageTotal: true));
	}

	private static object CalculateRunning(StiAggregateFunctionService function, object data, StiReport report, string name)
	{
		try
		{
			if (report == null)
			{
				if (data is StiDataBand)
				{
					report = (data as StiDataBand).Report;
				}
				else if (data is StiParser.StiParserData && (data as StiParser.StiParserData).Data is StiDataBand stiDataBand)
				{
					report = stiDataBand.Report;
				}
			}
			report.Engine.StartIndexPageForPageTotal = 0;
			return Calculate(function, data, report, name, allLevels: false, onlyChilds: false, isPageTotal: true);
		}
		finally
		{
			report.Engine.StartIndexPageForPageTotal = -1;
		}
	}

	private static decimal CalculateDecimalRunning(StiAggregateFunctionService function, object data, StiReport report, string name)
	{
		try
		{
			if (report == null)
			{
				if (data is StiDataBand)
				{
					report = (data as StiDataBand).Report;
				}
				else if (data is StiParser.StiParserData && (data as StiParser.StiParserData).Data is StiDataBand stiDataBand)
				{
					report = stiDataBand.Report;
				}
			}
			report.Engine.StartIndexPageForPageTotal = 0;
			return StiObjectConverter.ConvertToDecimal(Calculate(function, data, report, name, allLevels: false, onlyChilds: false, isPageTotal: true));
		}
		finally
		{
			report.Engine.StartIndexPageForPageTotal = -1;
		}
	}

	private static double CalculateDoubleRunning(StiAggregateFunctionService function, object data, StiReport report, string name)
	{
		try
		{
			if (report == null)
			{
				if (data is StiDataBand)
				{
					report = (data as StiDataBand).Report;
				}
				else if (data is StiParser.StiParserData && (data as StiParser.StiParserData).Data is StiDataBand stiDataBand)
				{
					report = stiDataBand.Report;
				}
			}
			report.Engine.StartIndexPageForPageTotal = 0;
			return StiObjectConverter.ConvertToDouble(Calculate(function, data, report, name, allLevels: false, onlyChilds: false, isPageTotal: true));
		}
		finally
		{
			report.Engine.StartIndexPageForPageTotal = -1;
		}
	}

	private static long CalculateInt64Running(StiAggregateFunctionService function, object data, StiReport report, string name)
	{
		try
		{
			if (report == null)
			{
				if (data is StiDataBand)
				{
					report = (data as StiDataBand).Report;
				}
				else if (data is StiParser.StiParserData && (data as StiParser.StiParserData).Data is StiDataBand stiDataBand)
				{
					report = stiDataBand.Report;
				}
			}
			report.Engine.StartIndexPageForPageTotal = 0;
			return StiObjectConverter.ConvertToInt64(Calculate(function, data, report, name, allLevels: false, onlyChilds: false, isPageTotal: true));
		}
		finally
		{
			report.Engine.StartIndexPageForPageTotal = -1;
		}
	}

	public static decimal Sum(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiSumDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static decimal? SumNullable(object data, StiReport report, string name)
	{
		return CalculateDecimalNullable(new StiSumDecimalNullableFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static decimal SumDistinct(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiSumDistinctDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static decimal SumDistinct(object data, StiReport report, string name, string name2)
	{
		return CalculateDecimal(new StiSumDistinctDecimalFunctionService(), data, report, name + "~" + name2, allLevels: false, onlyChilds: false);
	}

	public static decimal cSum(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiSumDecimalFunctionService(), data, report, name);
	}

	public static decimal cSumRunning(object data, StiReport report, string name)
	{
		return CalculateDecimalRunning(new StiSumDecimalFunctionService(), data, report, name);
	}

	public static decimal SumAllLevels(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiSumDecimalFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static decimal SumAllLevelsByCondition(object data, StiReport report, string name, StiFilterCondition filterCondition, decimal value1, decimal value2)
	{
		return CalculateDecimalByCondition(new StiSumDecimalFunctionService(), data, report, name, filterCondition, value1, value2, allLevels: true, onlyChilds: false);
	}

	public static decimal SumAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiSumDecimalFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static decimal SumOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiSumDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static double SumD(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiSumDoubleFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static double cSumD(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiSumDoubleFunctionService(), data, report, name);
	}

	public static double cSumDRunning(object data, StiReport report, string name)
	{
		return CalculateDoubleRunning(new StiSumDoubleFunctionService(), data, report, name);
	}

	public static double SumDAllLevels(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiSumDoubleFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static double SumDAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiSumDoubleFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static double SumDOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiSumDoubleFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static long SumI(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiSumIntFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static long cSumI(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiSumIntFunctionService(), data, report, name);
	}

	public static long cSumIRunning(object data, StiReport report, string name)
	{
		return CalculateInt64Running(new StiSumIntFunctionService(), data, report, name);
	}

	public static long SumIAllLevels(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiSumIntFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static long SumIAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiSumIntFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static long SumIOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiSumIntFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static TimeSpan SumTime(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiSumTimeFunctionService(), data, report, name, allLevels: false, onlyChilds: false, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static TimeSpan cSumTime(object data, StiReport report, string name)
	{
		return (TimeSpan)Calculate(new StiSumTimeFunctionService(), data, report, name);
	}

	public static TimeSpan SumTimeAllLevels(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiSumTimeFunctionService(), data, report, name, allLevels: true, onlyChilds: false, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static TimeSpan SumTimeAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiSumTimeFunctionService(), data, report, name, allLevels: true, onlyChilds: true, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static TimeSpan SumTimeOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiSumTimeFunctionService(), data, report, name, allLevels: false, onlyChilds: true, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static decimal Avg(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiAvgDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static decimal cAvg(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiAvgDecimalFunctionService(), data, report, name);
	}

	public static decimal cAvgRunning(object data, StiReport report, string name)
	{
		return CalculateDecimalRunning(new StiAvgDecimalFunctionService(), data, report, name);
	}

	public static decimal AvgAllLevels(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiAvgDecimalFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static decimal AvgAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiAvgDecimalFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static decimal AvgOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiAvgDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static DateTime AvgDate(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiAvgDateFunctionService(), data, report, name, allLevels: false, onlyChilds: false, isPageTotal: false);
		return (DateTime)obj;
	}

	public static DateTime cAvgDate(object data, StiReport report, string name)
	{
		return (DateTime)Calculate(new StiAvgDateFunctionService(), data, report, name);
	}

	public static DateTime AvgDateAllLevels(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiAvgDateFunctionService(), data, report, name, allLevels: true, onlyChilds: false, isPageTotal: false);
		return (DateTime)obj;
	}

	public static DateTime AvgDateAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiAvgDateFunctionService(), data, report, name, allLevels: true, onlyChilds: true, isPageTotal: false);
		return (DateTime)obj;
	}

	public static DateTime AvgDateOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiAvgDateFunctionService(), data, report, name, allLevels: false, onlyChilds: true, isPageTotal: false);
		return (DateTime)obj;
	}

	public static TimeSpan AvgTime(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiAvgTimeFunctionService(), data, report, name, allLevels: false, onlyChilds: false, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static TimeSpan cAvgTime(object data, StiReport report, string name)
	{
		return (TimeSpan)Calculate(new StiAvgTimeFunctionService(), data, report, name);
	}

	public static TimeSpan AvgTimeAllLevels(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiAvgTimeFunctionService(), data, report, name, allLevels: true, onlyChilds: false, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static TimeSpan AvgTimeAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiAvgTimeFunctionService(), data, report, name, allLevels: true, onlyChilds: true, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static TimeSpan AvgTimeOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiAvgTimeFunctionService(), data, report, name, allLevels: false, onlyChilds: true, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static double AvgD(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiAvgDoubleFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static double cAvgD(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiAvgDoubleFunctionService(), data, report, name);
	}

	public static double cAvgDRunning(object data, StiReport report, string name)
	{
		return CalculateDoubleRunning(new StiAvgDoubleFunctionService(), data, report, name);
	}

	public static double AvgDAllLevels(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiAvgDoubleFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static double AvgDAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiAvgDoubleFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static double AvgDOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiAvgDoubleFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static long AvgI(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiAvgIntFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static long cAvgI(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiAvgIntFunctionService(), data, report, name);
	}

	public static long cAvgIRunning(object data, StiReport report, string name)
	{
		return CalculateInt64Running(new StiAvgIntFunctionService(), data, report, name);
	}

	public static long AvgIAllLevels(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiAvgIntFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static long AvgIAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiAvgIntFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static long AvgIOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiAvgIntFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static decimal Max(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMaxDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static decimal cMax(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMaxDecimalFunctionService(), data, report, name);
	}

	public static decimal cMaxRunning(object data, StiReport report, string name)
	{
		return CalculateDecimalRunning(new StiMaxDecimalFunctionService(), data, report, name);
	}

	public static decimal MaxAllLevels(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMaxDecimalFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static decimal MaxAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMaxDecimalFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static decimal MaxOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMaxDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static double MaxD(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMaxDoubleFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static double cMaxD(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMaxDoubleFunctionService(), data, report, name);
	}

	public static double cMaxDRunning(object data, StiReport report, string name)
	{
		return CalculateDoubleRunning(new StiMaxDoubleFunctionService(), data, report, name);
	}

	public static double MaxDAllLevels(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMaxDoubleFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static double MaxDAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMaxDoubleFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static double MaxDOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMaxDoubleFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static long MaxI(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMaxIntFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static long cMaxI(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMaxIntFunctionService(), data, report, name);
	}

	public static long cMaxIRunning(object data, StiReport report, string name)
	{
		return CalculateInt64Running(new StiMaxIntFunctionService(), data, report, name);
	}

	public static long MaxIAllLevels(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMaxIntFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static long MaxIAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMaxIntFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static long MaxIOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMaxIntFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static decimal Min(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMinDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static decimal cMin(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMinDecimalFunctionService(), data, report, name);
	}

	public static decimal cMinRunning(object data, StiReport report, string name)
	{
		return CalculateDecimalRunning(new StiMinDecimalFunctionService(), data, report, name);
	}

	public static decimal MinAllLevels(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMinDecimalFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static decimal MinAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMinDecimalFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static decimal MinOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMinDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static double MinD(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMinDoubleFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static double cMinD(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMinDoubleFunctionService(), data, report, name);
	}

	public static double cMinDRunning(object data, StiReport report, string name)
	{
		return CalculateDoubleRunning(new StiMinDoubleFunctionService(), data, report, name);
	}

	public static double MinDAllLevels(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMinDoubleFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static double MinDAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMinDoubleFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static double MinDOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMinDoubleFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static long MinI(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMinIntFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static long cMinI(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMinIntFunctionService(), data, report, name);
	}

	public static long cMinIRunning(object data, StiReport report, string name)
	{
		return CalculateInt64Running(new StiMinIntFunctionService(), data, report, name);
	}

	public static long MinIAllLevels(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMinIntFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static long MinIAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMinIntFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static long MinIOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMinIntFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static decimal Median(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMedianDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static decimal cMedian(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMedianDecimalFunctionService(), data, report, name);
	}

	public static decimal cMedianRunning(object data, StiReport report, string name)
	{
		return CalculateDecimalRunning(new StiMedianDecimalFunctionService(), data, report, name);
	}

	public static decimal MedianAllLevels(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMedianDecimalFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static decimal MedianAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMedianDecimalFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static decimal MedianOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiMedianDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static double MedianD(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMedianDoubleFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static double cMedianD(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMedianDoubleFunctionService(), data, report, name);
	}

	public static double cMedianDRunning(object data, StiReport report, string name)
	{
		return CalculateDoubleRunning(new StiMedianDoubleFunctionService(), data, report, name);
	}

	public static double MedianDAllLevels(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMedianDoubleFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static double MedianDAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMedianDoubleFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static double MedianDOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiMedianDoubleFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static long MedianI(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMedianIntFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static long cMedianI(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMedianIntFunctionService(), data, report, name);
	}

	public static long cMedianIRunning(object data, StiReport report, string name)
	{
		return CalculateInt64Running(new StiMedianIntFunctionService(), data, report, name);
	}

	public static long MedianIAllLevels(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMedianIntFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static long MedianIAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMedianIntFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static long MedianIOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiMedianIntFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static decimal Mode(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiModeDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static decimal cMode(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiModeDecimalFunctionService(), data, report, name);
	}

	public static decimal cModeRunning(object data, StiReport report, string name)
	{
		return CalculateDecimalRunning(new StiModeDecimalFunctionService(), data, report, name);
	}

	public static decimal ModeAllLevels(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiModeDecimalFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static decimal ModeAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiModeDecimalFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static decimal ModeOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDecimal(new StiModeDecimalFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static double ModeD(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiModeDoubleFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static double cModeD(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiModeDoubleFunctionService(), data, report, name);
	}

	public static double cModeDRunning(object data, StiReport report, string name)
	{
		return CalculateDoubleRunning(new StiModeDoubleFunctionService(), data, report, name);
	}

	public static double ModeDAllLevels(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiModeDoubleFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static double ModeDAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiModeDoubleFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static double ModeDOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateDouble(new StiModeDoubleFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static long ModeI(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiModeIntFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static long cModeI(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiModeIntFunctionService(), data, report, name);
	}

	public static long cModeIRunning(object data, StiReport report, string name)
	{
		return CalculateInt64Running(new StiModeIntFunctionService(), data, report, name);
	}

	public static long ModeIAllLevels(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiModeIntFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static long ModeIAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiModeIntFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static long ModeIOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiModeIntFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static object First(object data, StiReport report, string name)
	{
		return Calculate(new StiFirstFunctionService(), data, report, name, allLevels: false, onlyChilds: false, isPageTotal: false);
	}

	public static object cFirst(object data, StiReport report, string name)
	{
		return Calculate(new StiFirstFunctionService(), data, report, name);
	}

	public static object cFirstRunning(object data, StiReport report, string name)
	{
		return CalculateRunning(new StiFirstFunctionService(), data, report, name);
	}

	public static object FirstAllLevels(object data, StiReport report, string name)
	{
		return Calculate(new StiFirstFunctionService(), data, report, name, allLevels: true, onlyChilds: false, isPageTotal: false);
	}

	public static object FirstAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return Calculate(new StiFirstFunctionService(), data, report, name, allLevels: true, onlyChilds: true, isPageTotal: false);
	}

	public static object FirstOnlyChilds(object data, StiReport report, string name)
	{
		return Calculate(new StiFirstFunctionService(), data, report, name, allLevels: false, onlyChilds: true, isPageTotal: false);
	}

	public static object Last(object data, StiReport report, string name)
	{
		return Calculate(new StiLastFunctionService(), data, report, name, allLevels: false, onlyChilds: false, isPageTotal: false);
	}

	public static object cLast(object data, StiReport report, string name)
	{
		return Calculate(new StiLastFunctionService(), data, report, name);
	}

	public static object cLastRunning(object data, StiReport report, string name)
	{
		return CalculateRunning(new StiLastFunctionService(), data, report, name);
	}

	public static object LastAllLevels(object data, StiReport report, string name)
	{
		return Calculate(new StiLastFunctionService(), data, report, name, allLevels: true, onlyChilds: false, isPageTotal: false);
	}

	public static object LastAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return Calculate(new StiLastFunctionService(), data, report, name, allLevels: true, onlyChilds: true, isPageTotal: false);
	}

	public static object LastOnlyChilds(object data, StiReport report, string name)
	{
		return Calculate(new StiLastFunctionService(), data, report, name, allLevels: false, onlyChilds: true, isPageTotal: false);
	}

	public static long Count(object data)
	{
		return CalculateInt64(new StiCountFunctionService(), data, null, null, allLevels: false, onlyChilds: false);
	}

	public static long Count(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiCountFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static long cCount(object data)
	{
		return CalculateInt64(new StiCountFunctionService(), data, null, null);
	}

	public static long cCount(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiCountFunctionService(), data, report, name);
	}

	public static long cCountRunning(object data)
	{
		return CalculateInt64Running(new StiCountFunctionService(), data, null, null);
	}

	public static long cCountRunning(object data, StiReport report, string name)
	{
		return CalculateInt64Running(new StiCountFunctionService(), data, report, name);
	}

	public static long CountAllLevels(object data)
	{
		return CalculateInt64(new StiCountFunctionService(), data, null, null, allLevels: true, onlyChilds: false);
	}

	public static long CountAllLevelsOnlyChilds(object data)
	{
		return CalculateInt64(new StiCountFunctionService(), data, null, null, allLevels: true, onlyChilds: true);
	}

	public static long CountOnlyChilds(object data)
	{
		return CalculateInt64(new StiCountFunctionService(), data, null, null, allLevels: false, onlyChilds: true);
	}

	public static long CountDistinct(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiCountDistinctFunctionService(), data, report, name, allLevels: false, onlyChilds: false);
	}

	public static long cCountDistinct(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiCountDistinctFunctionService(), data, report, name);
	}

	public static long cCountDistinctRunning(object data, StiReport report, string name)
	{
		return CalculateInt64Running(new StiCountDistinctFunctionService(), data, report, name);
	}

	public static long CountDistinctAllLevels(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiCountDistinctFunctionService(), data, report, name, allLevels: true, onlyChilds: false);
	}

	public static long CountDistinctAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiCountDistinctFunctionService(), data, report, name, allLevels: true, onlyChilds: true);
	}

	public static long CountDistinctOnlyChilds(object data, StiReport report, string name)
	{
		return CalculateInt64(new StiCountDistinctFunctionService(), data, report, name, allLevels: false, onlyChilds: true);
	}

	public static DateTime MinDate(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMinDateFunctionService(), data, report, name, allLevels: false, onlyChilds: false, isPageTotal: false);
		return (DateTime)obj;
	}

	public static DateTime cMinDate(object data, StiReport report, string name)
	{
		return (DateTime)Calculate(new StiMinDateFunctionService(), data, report, name);
	}

	public static DateTime MinDateAllLevels(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMinDateFunctionService(), data, report, name, allLevels: true, onlyChilds: false, isPageTotal: false);
		return (DateTime)obj;
	}

	public static DateTime MinDateAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMinDateFunctionService(), data, report, name, allLevels: true, onlyChilds: true, isPageTotal: false);
		return (DateTime)obj;
	}

	public static DateTime MinDateOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMinDateFunctionService(), data, report, name, allLevels: false, onlyChilds: true, isPageTotal: false);
		return (DateTime)obj;
	}

	public static TimeSpan MinTime(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMinTimeFunctionService(), data, report, name, allLevels: false, onlyChilds: false, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static TimeSpan cMinTime(object data, StiReport report, string name)
	{
		return (TimeSpan)Calculate(new StiMinTimeFunctionService(), data, report, name);
	}

	public static TimeSpan MinTimeAllLevels(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMinTimeFunctionService(), data, report, name, allLevels: true, onlyChilds: false, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static TimeSpan MinTimeAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMinTimeFunctionService(), data, report, name, allLevels: true, onlyChilds: true, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static TimeSpan MinTimeOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMinTimeFunctionService(), data, report, name, allLevels: false, onlyChilds: true, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static string MinStr(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMinStrFunctionService(), data, report, name, allLevels: false, onlyChilds: false, isPageTotal: false);
		return (string)obj;
	}

	public static string cMinStr(object data, StiReport report, string name)
	{
		return (string)Calculate(new StiMinStrFunctionService(), data, report, name);
	}

	public static string MinStrAllLevels(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMinStrFunctionService(), data, report, name, allLevels: true, onlyChilds: false, isPageTotal: false);
		return (string)obj;
	}

	public static string MinStrAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMinStrFunctionService(), data, report, name, allLevels: true, onlyChilds: true, isPageTotal: false);
		return (string)obj;
	}

	public static string MinStrOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMinStrFunctionService(), data, report, name, allLevels: false, onlyChilds: true, isPageTotal: false);
		return (string)obj;
	}

	public static DateTime MaxDate(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMaxDateFunctionService(), data, report, name, allLevels: false, onlyChilds: false, isPageTotal: false);
		return (DateTime)obj;
	}

	public static DateTime cMaxDate(object data, StiReport report, string name)
	{
		return (DateTime)Calculate(new StiMaxDateFunctionService(), data, report, name);
	}

	public static DateTime MaxDateAllLevels(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMaxDateFunctionService(), data, report, name, allLevels: true, onlyChilds: false, isPageTotal: false);
		return (DateTime)obj;
	}

	public static DateTime MaxDateAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMaxDateFunctionService(), data, report, name, allLevels: true, onlyChilds: true, isPageTotal: false);
		return (DateTime)obj;
	}

	public static DateTime MaxDateOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMaxDateFunctionService(), data, report, name, allLevels: false, onlyChilds: true, isPageTotal: false);
		return (DateTime)obj;
	}

	public static TimeSpan MaxTime(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMaxTimeFunctionService(), data, report, name, allLevels: false, onlyChilds: false, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static TimeSpan cMaxTime(object data, StiReport report, string name)
	{
		return (TimeSpan)Calculate(new StiMaxTimeFunctionService(), data, report, name);
	}

	public static TimeSpan MaxTimeAllLevels(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMaxTimeFunctionService(), data, report, name, allLevels: true, onlyChilds: false, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static TimeSpan MaxTimeAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMaxTimeFunctionService(), data, report, name, allLevels: true, onlyChilds: true, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static TimeSpan MaxTimeOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMaxTimeFunctionService(), data, report, name, allLevels: false, onlyChilds: true, isPageTotal: false);
		return (TimeSpan)obj;
	}

	public static string MaxStr(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMaxStrFunctionService(), data, report, name, allLevels: false, onlyChilds: false, isPageTotal: false);
		return (string)obj;
	}

	public static string cMaxStr(object data, StiReport report, string name)
	{
		return (string)Calculate(new StiMaxStrFunctionService(), data, report, name);
	}

	public static string MaxStrAllLevels(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMaxStrFunctionService(), data, report, name, allLevels: true, onlyChilds: false, isPageTotal: false);
		return (string)obj;
	}

	public static string MaxStrAllLevelsOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMaxStrFunctionService(), data, report, name, allLevels: true, onlyChilds: true, isPageTotal: false);
		return (string)obj;
	}

	public static string MaxStrOnlyChilds(object data, StiReport report, string name)
	{
		object obj = Calculate(new StiMaxStrFunctionService(), data, report, name, allLevels: false, onlyChilds: true, isPageTotal: false);
		return (string)obj;
	}

	public static int Rank(object data, StiReport report, string name)
	{
		return Rank(data, report, name, dense: true, StiRankOrder.Asc);
	}

	public static int Rank(object data, StiReport report, string name, bool dense, StiRankOrder sortOrder)
	{
		object obj = Calculate(new StiRankFunctionService(runningTotal: false, dense, sortOrder), data, report, name, allLevels: false, onlyChilds: false, isPageTotal: false);
		if (obj is Hashtable hashtable && data != null)
		{
			object obj2 = null;
			if (data is StiParser.StiParserData { AsmList: var asmList, Parser: var parser })
			{
				obj2 = parser.ExecuteAsm(asmList);
			}
			else if (name != null)
			{
				MethodInfo method = GetMethod(report, name);
				if (method == null)
				{
					return 0;
				}
				StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
				method.Invoke(report, new object[2] { report, stiValueEventArgs });
				obj2 = stiValueEventArgs.Value;
			}
			if (obj2 != null && obj2 != DBNull.Value)
			{
				return (int)hashtable[obj2];
			}
			return 0;
		}
		return 0;
	}

	private static void SaveState(StiDataBand dataBand)
	{
		dataBand.SaveState("Totals");
		if (dataBand.DataSource != null && dataBand.DataSource is StiSqlSource)
		{
			(dataBand.DataSource as StiSqlSource).ReconnectOnEachRow = false;
		}
		if (dataBand.DataBandInfoV2 == null)
		{
			return;
		}
		if (dataBand.DataBandInfoV2.DetailDataBands != null)
		{
			foreach (StiDataBand detailDataBand in dataBand.DataBandInfoV2.DetailDataBands)
			{
				SaveState(detailDataBand);
			}
		}
		if (dataBand.DataBandInfoV2.DetailDataBandsFromSubReports == null)
		{
			return;
		}
		foreach (DictionaryEntry detailDataBandsFromSubReport in dataBand.DataBandInfoV2.DetailDataBandsFromSubReports)
		{
			if (detailDataBandsFromSubReport.Value == null)
			{
				SaveState(detailDataBandsFromSubReport.Key as StiDataBand);
			}
		}
	}

	private static void RestoreState(StiDataBand dataBand)
	{
		dataBand.RestoreState("Totals");
		if (dataBand.DataBandInfoV2 == null)
		{
			return;
		}
		if (dataBand.DataBandInfoV2.DetailDataBands != null)
		{
			foreach (StiDataBand detailDataBand in dataBand.DataBandInfoV2.DetailDataBands)
			{
				RestoreState(detailDataBand);
			}
		}
		if (dataBand.DataBandInfoV2.DetailDataBandsFromSubReports == null)
		{
			return;
		}
		foreach (DictionaryEntry detailDataBandsFromSubReport in dataBand.DataBandInfoV2.DetailDataBandsFromSubReports)
		{
			if (detailDataBandsFromSubReport.Value == null)
			{
				RestoreState(detailDataBandsFromSubReport.Key as StiDataBand);
			}
		}
	}

	private static bool GetCachedValue(StiAggregateFunctionService function, object data, StiReport report, string name, bool allLevels, bool onlyChilds, bool isPageTotal, ref object result, out string storeId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (data is StiParser.StiParserData stiParserData)
		{
			data = stiParserData.Data;
			if (stiParserData.AsmList != null)
			{
				foreach (StiParser.StiAsmCommand asm in stiParserData.AsmList)
				{
					stringBuilder.Append(asm);
					stringBuilder.Append("*");
				}
			}
		}
		storeId = string.Format("{0}_*_{1}_*_{2}_*_{3}_*_{4}_*_{5}", ((StiService)function).ServiceName, name, stringBuilder, allLevels, onlyChilds, isPageTotal ? ("PageTotal" + report.CurrentPrintPage) : "False");
		if (report.CachedTotals == null)
		{
			report.CachedTotals = new Hashtable();
		}
		if (report.CachedTotals[data] is Hashtable hashtable && hashtable.ContainsKey(storeId))
		{
			result = hashtable[storeId];
			return true;
		}
		return false;
	}

	private static void StoreCachedValue(object data, StiReport report, string storeId, object result)
	{
		if (data is StiParser.StiParserData stiParserData)
		{
			data = stiParserData.Data;
		}
		Hashtable hashtable = report.CachedTotals[data] as Hashtable;
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			report.CachedTotals[data] = hashtable;
		}
		hashtable[storeId] = result;
	}

	private Totals()
	{
	}
}
