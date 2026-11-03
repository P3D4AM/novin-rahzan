using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Extensions;

namespace Stimulsoft.Report.Dictionary;

public class StiDataSourceHelper
{
	public static StiDatabase GetDatabaseFromDataSource(StiDataSource dataSource)
	{
		if (dataSource == null || dataSource.Dictionary == null)
		{
			return null;
		}
		string category = dataSource.GetCategoryName();
		if (string.IsNullOrWhiteSpace(category))
		{
			return null;
		}
		category = category.ToLowerInvariant();
		return dataSource.Dictionary.Databases.ToList().FirstOrDefault((StiDatabase d) => d.Name != null && d.Name.ToLowerInvariant() == category);
	}

	public static List<StiDataSource> GetDataSourcesFromDatabase(StiReport report, StiDatabase database)
	{
		if (database == null || database.Name == null)
		{
			return null;
		}
		return (from d in report.Dictionary.DataSources.ToList()
			where d.GetCategoryName() != null
			where database.Name.ToLowerInvariant() == d.GetCategoryName().ToLowerInvariant()
			select d).ToList();
	}

	public static List<string> GetUsedDataSourcesNamesList(StiReport report)
	{
		return (from DictionaryEntry d in GetUsedDataSourcesNames(report)
			select d.Key).Cast<string>().ToList();
	}

	public static Hashtable GetUsedDataSourcesNames(StiReport report)
	{
		Hashtable hashtable = new Hashtable();
		StiPage page = ((report.RenderedPages.Count > 0) ? report.RenderedPages[0] : new StiPage
		{
			Report = report
		});
		StiText stiText = new StiText
		{
			Page = page,
			Name = "*GetUsedDataSourcesNames*"
		};
		StiComponentsCollection components = report.GetComponents();
		for (int i = 0; i < components.Count; i++)
		{
			StiComponent stiComponent = components[i];
			StiImage stiImage = stiComponent as StiImage;
			try
			{
				if (stiComponent is StiDataBand { DataSourceName: not null } stiDataBand && stiDataBand.DataSourceName.Length > 0)
				{
					AddDataSourceName(hashtable, stiDataBand.DataSourceName);
					AddRelation(hashtable, stiDataBand.DataRelation);
					AddSort(hashtable, stiText, stiDataBand.Sort, stiDataBand.DataSourceName);
				}
				if (stiComponent is StiCrossTab { DataSourceName: not null } stiCrossTab && stiCrossTab.DataSourceName.Length > 0)
				{
					AddDataSourceName(hashtable, stiCrossTab.DataSourceName);
					AddSort(hashtable, stiText, stiCrossTab.Sort, stiCrossTab.DataSourceName);
				}
				if (stiComponent is StiGroupHeaderBand stiGroupHeaderBand)
				{
					CheckExpression(stiGroupHeaderBand.Condition.Value, stiComponent, hashtable);
				}
				if (stiComponent is StiCrossCell stiCrossCell)
				{
					CheckExpression(stiCrossCell.Value.Value, stiComponent, hashtable);
				}
				if (stiComponent is StiCrossHeader stiCrossHeader)
				{
					CheckExpression(stiCrossHeader.DisplayValue.Value, stiComponent, hashtable);
				}
				if (stiComponent is StiSimpleText stiSimpleText)
				{
					CheckExpression(stiSimpleText.Text.Value, stiComponent, hashtable);
				}
				if (stiComponent is StiText stiText2)
				{
					CheckExpression(stiText2.ExcelValue.Value, stiComponent, hashtable);
				}
				if (stiComponent is StiRichText stiRichText)
				{
					RichTextBox val = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
					try
					{
						val.Rtf = stiRichText.RtfText;
						CheckExpression(((Control)val).Text, stiComponent, hashtable);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				if (stiImage != null)
				{
					CheckExpression(stiImage.ImageData.Value, stiComponent, hashtable);
					CheckExpression("{" + stiImage.DataColumn + "}", stiComponent, hashtable);
				}
				if (stiComponent is StiBarCode stiBarCode)
				{
					CheckExpression(stiBarCode.Code.Value, stiComponent, hashtable);
				}
				if (stiComponent is StiZipCode stiZipCode)
				{
					CheckExpression(stiZipCode.Code.Value, stiComponent, hashtable);
				}
				if (stiComponent is StiCheckBox stiCheckBox)
				{
					CheckExpression(stiCheckBox.Checked.Value, stiComponent, hashtable);
				}
				if (stiComponent is StiChart stiChart)
				{
					if (!string.IsNullOrEmpty(stiChart.DataSourceName))
					{
						AddDataSourceName(hashtable, stiChart.DataSourceName);
					}
					foreach (StiSeries item3 in stiChart.Series)
					{
						CheckExpression(item3.Argument.Value, stiComponent, hashtable);
						CheckExpression("{" + item3.ArgumentDataColumn + "}", stiComponent, hashtable);
					}
				}
				if (stiComponent is StiPage stiPage)
				{
					CheckExpression(stiPage.ExcelSheet.Value, stiComponent, hashtable);
				}
				IStiConditions stiConditions = stiComponent;
				if (stiConditions != null && stiConditions.Conditions.Count > 0)
				{
					List<DictionaryEntry> list = new List<DictionaryEntry>();
					foreach (StiBaseCondition condition in stiConditions.Conditions)
					{
						StiCondition stiCondition = condition as StiCondition;
						if (condition is StiMultiCondition)
						{
							StiMultiCondition stiMultiCondition = condition as StiMultiCondition;
							if (!stiMultiCondition.FilterOn || stiMultiCondition.Filters.Count <= 0)
							{
								continue;
							}
							StringBuilder stringBuilder = new StringBuilder("{");
							for (int j = 0; j < stiMultiCondition.Filters.Count; j++)
							{
								StiFilter stiFilter = stiMultiCondition.Filters[j];
								stringBuilder.Append("(");
								stringBuilder.Append(StiDataHelper.GetFilterExpression(stiFilter, stiFilter.Column, report));
								stringBuilder.Append(")");
								if (j < stiMultiCondition.Filters.Count - 1)
								{
									stringBuilder.Append((stiMultiCondition.FilterMode == StiFilterMode.And) ? " && " : " || ");
								}
							}
							stringBuilder.Append("}");
							DictionaryEntry item = new DictionaryEntry(stiMultiCondition, stringBuilder.ToString());
							list.Add(item);
						}
						else if (stiCondition != null)
						{
							string value = "{" + StiDataHelper.GetFilterExpression(stiCondition, stiCondition.Column, report) + "}";
							DictionaryEntry item2 = new DictionaryEntry(stiCondition, value);
							list.Add(item2);
						}
					}
					if (list.Count > 0)
					{
						foreach (DictionaryEntry item4 in list)
						{
							CheckExpression((string)item4.Value, stiComponent, hashtable);
						}
					}
				}
				if (!(stiComponent is IStiFilter stiFilter2) || stiFilter2.Filters.Count <= 0)
				{
					continue;
				}
				string text = "ds";
				if (stiComponent is IStiDataSource stiDataSource && !string.IsNullOrWhiteSpace(stiDataSource.DataSourceName))
				{
					text = stiDataSource.DataSourceName;
				}
				StringBuilder stringBuilder2 = new StringBuilder("{");
				for (int k = 0; k < stiFilter2.Filters.Count; k++)
				{
					StiFilter stiFilter3 = stiFilter2.Filters[k];
					stringBuilder2.Append("(");
					stringBuilder2.Append(StiDataHelper.GetFilterExpression(stiFilter3, text + "." + stiFilter3.Column, report));
					stringBuilder2.Append(")");
					if (k < stiFilter2.Filters.Count - 1)
					{
						stringBuilder2.Append((stiFilter2.FilterMode == StiFilterMode.And) ? " && " : " || ");
					}
				}
				stringBuilder2.Append("}");
				CheckExpression(stringBuilder2.ToString(), stiComponent, hashtable);
			}
			catch
			{
			}
		}
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			if (variable.InitBy == StiVariableInitBy.Expression)
			{
				CheckExpression("{" + variable.Value + "}", stiText, hashtable);
				if (variable.Type == typeof(Range))
				{
					CheckExpression("{" + variable.InitByExpressionFrom + "}", stiText, hashtable);
					CheckExpression("{" + variable.InitByExpressionTo + "}", stiText, hashtable);
				}
			}
			if (variable.RequestFromUser && variable.DialogInfo.ItemsInitializationType == StiItemsInitializationType.Columns)
			{
				CheckExpression("{" + variable.DialogInfo.KeysColumn + "}", stiText, hashtable);
				CheckExpression("{" + variable.DialogInfo.ValuesColumn + "}", stiText, hashtable);
			}
		}
		List<string> list2 = new List<string>();
		foreach (DictionaryEntry item5 in hashtable)
		{
			string text2 = (string)item5.Key;
			StiDataSource stiDataSource2 = report.Dictionary.DataSources[text2];
			if (stiDataSource2 != null && stiDataSource2 is StiVirtualSource stiVirtualSource && !string.IsNullOrEmpty(stiVirtualSource.NameInSource))
			{
				list2.Add(text2);
			}
		}
		foreach (string item6 in list2)
		{
			StiVirtualSource stiVirtualSource2 = report.Dictionary.DataSources[item6] as StiVirtualSource;
			StiDataSource stiDataSource3 = report.Dictionary.DataSources[stiVirtualSource2.NameInSource];
			AddDataSourceName(hashtable, stiDataSource3.Name);
			string[] groupColumns = stiVirtualSource2.GroupColumns;
			foreach (string columnName in groupColumns)
			{
				AddDataSourceColumn(hashtable, stiDataSource3.Name, columnName);
			}
			for (int m = 0; m < stiVirtualSource2.Results.Length; m += 3)
			{
				AddDataSourceColumn(hashtable, stiDataSource3.Name, stiVirtualSource2.Results[m]);
			}
			AddSort(hashtable, stiText, stiVirtualSource2.Sort, stiDataSource3.Name);
		}
		Hashtable hashtable2 = hashtable.Clone() as Hashtable;
		foreach (DictionaryEntry item7 in hashtable2)
		{
			string name = (string)item7.Key;
			StiDataSource stiDataSource4 = report.Dictionary.DataSources[name];
			if (stiDataSource4 == null)
			{
				continue;
			}
			Hashtable hashtable3 = (item7.Value as Hashtable).Clone() as Hashtable;
			foreach (object key in hashtable3.Keys)
			{
				StiDataColumn stiDataColumn = stiDataSource4.Columns[(string)key];
				if (stiDataColumn is StiCalcDataColumn stiCalcDataColumn)
				{
					CheckExpression("{" + stiCalcDataColumn.Expression + "}", stiText, hashtable);
				}
			}
		}
		if (report.ListOfUsedData != null)
		{
			string[] listOfUsedData = report.ListOfUsedData;
			foreach (string dataSourceName in listOfUsedData)
			{
				AddDataSourceName(hashtable, dataSourceName);
			}
		}
		return hashtable;
	}

	internal static void CheckExpression(string expression, StiComponent component, Hashtable datasourcesNames, bool throwException = false)
	{
		try
		{
			bool storeToPrint = false;
			object obj = StiParser.ParseTextValue(expression, component, ref storeToPrint, executeIfStoreToPrint: false, returnAsmList: true);
			if (obj != null && obj is List<StiParser.StiAsmCommand>)
			{
				CheckAsmCommandList(obj as List<StiParser.StiAsmCommand>, component, datasourcesNames);
			}
		}
		catch (Exception ex)
		{
			if (throwException)
			{
				throw ex;
			}
		}
	}

	private static void CheckAsmCommandList(List<StiParser.StiAsmCommand> list, StiComponent component, Hashtable datasourcesNames)
	{
		try
		{
			foreach (StiParser.StiAsmCommand item in list)
			{
				if (item.Type == StiParser.StiAsmCommandType.PushValue && item.Parameter1 is List<StiParser.StiAsmCommand>)
				{
					CheckAsmCommandList(item.Parameter1 as List<StiParser.StiAsmCommand>, component, datasourcesNames);
				}
				else
				{
					if (item.Type != StiParser.StiAsmCommandType.PushDataSourceField)
					{
						continue;
					}
					List<string> list2 = new List<string>(((string)item.Parameter1).Split(new char[1] { '.' }));
					StiDataSource stiDataSource = component.Report.Dictionary.DataSources[list2[0]];
					if (stiDataSource is StiVirtualSource && list2.Count > 2)
					{
						string text = list2[1] + "." + list2[2];
						if (stiDataSource.Columns.Contains(text))
						{
							list2[1] = text;
							list2.RemoveAt(2);
						}
						else if (list2.Count > 3)
						{
							text = text + "." + list2[3];
							if (stiDataSource.Columns.Contains(text))
							{
								list2[1] = text;
								list2.RemoveAt(2);
								list2.RemoveAt(2);
							}
						}
					}
					AddDataSourceName(datasourcesNames, stiDataSource.Name);
					if (list2.Count > 2)
					{
						string nameInSource = list2[1];
						AddRelation(datasourcesNames, stiDataSource.ParentRelationList().First((StiDataRelation rel) => rel.NameInSource == nameInSource));
						stiDataSource = stiDataSource.GetParentDataSource(nameInSource);
						AddDataSourceName(datasourcesNames, stiDataSource.Name);
						for (int i = 2; i < list2.Count - 1; i++)
						{
							nameInSource = list2[i];
							AddRelation(datasourcesNames, stiDataSource.ParentRelationList().First((StiDataRelation rel) => rel.NameInSource == nameInSource));
							stiDataSource = stiDataSource.GetParentDataSource(nameInSource);
							AddDataSourceName(datasourcesNames, stiDataSource.Name);
						}
					}
					if (list2.Count > 1)
					{
						AddDataSourceColumn(datasourcesNames, stiDataSource.Name, list2[list2.Count - 1]);
					}
				}
			}
		}
		catch
		{
		}
	}

	private static void AddDataSourceName(Hashtable hashtable, string dataSourceName)
	{
		if (!hashtable.ContainsKey(dataSourceName))
		{
			hashtable[dataSourceName] = new Hashtable();
		}
	}

	private static void AddDataSourceColumn(Hashtable hashtable, string dataSourceName, string columnName)
	{
		Hashtable hashtable2 = hashtable[dataSourceName] as Hashtable;
		if (hashtable2 == null)
		{
			hashtable2 = (Hashtable)(hashtable[dataSourceName] = new Hashtable());
		}
		hashtable2[columnName] = columnName;
	}

	private static void AddRelation(Hashtable hashtable, StiDataRelation relation)
	{
		if (relation == null)
		{
			return;
		}
		if (relation.ChildSource != null)
		{
			AddDataSourceName(hashtable, relation.ChildSource.Name);
			string[] childColumns = relation.ChildColumns;
			foreach (string columnName in childColumns)
			{
				AddDataSourceColumn(hashtable, relation.ChildSource.Name, columnName);
			}
		}
		if (relation.ParentSource != null)
		{
			AddDataSourceName(hashtable, relation.ParentSource.Name);
			string[] parentColumns = relation.ParentColumns;
			foreach (string columnName2 in parentColumns)
			{
				AddDataSourceColumn(hashtable, relation.ParentSource.Name, columnName2);
			}
		}
	}

	private static void AddSort(Hashtable hashtable, StiComponent tempComp, string[] sortArray, string dataSourceName)
	{
		for (int i = 1; i < sortArray.Length; i += 2)
		{
			string text = sortArray[i];
			if (text.StartsWith("{"))
			{
				CheckExpression(text, tempComp, hashtable);
			}
			else
			{
				AddDataSourceColumn(hashtable, dataSourceName, text);
			}
		}
	}

	public static Hashtable GetDataSourcesUsedInRequestFromUsersVariables(StiReport report)
	{
		Hashtable hashtable = new Hashtable();
		StiText component = new StiText
		{
			Name = "*RequestFromUserVariable*",
			Page = new StiPage(report)
		};
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			if (!variable.RequestFromUser)
			{
				continue;
			}
			try
			{
				if (variable.DialogInfo.ItemsInitializationType == StiItemsInitializationType.Columns)
				{
					CheckExpression("{" + variable.DialogInfo.KeysColumn + "}", component, hashtable, throwException: true);
				}
				if (variable.InitBy == StiVariableInitBy.Expression)
				{
					CheckExpression("{" + variable.Value + "}", component, hashtable, throwException: true);
					if (variable.Type == typeof(Range))
					{
						CheckExpression("{" + variable.InitByExpressionFrom + "}", component, hashtable, throwException: true);
						CheckExpression("{" + variable.InitByExpressionTo + "}", component, hashtable, throwException: true);
					}
				}
			}
			catch (Exception ex)
			{
				report.WriteToReportRenderingMessages("A problem with the '" + variable.Name + "' variable parsing! " + ex.Message);
			}
		}
		return hashtable;
	}
}
