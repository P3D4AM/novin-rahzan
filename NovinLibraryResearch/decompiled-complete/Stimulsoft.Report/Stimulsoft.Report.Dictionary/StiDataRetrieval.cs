using System.Collections;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dictionary;

public class StiDataRetrieval
{
	private class StiDictionaryBuilder
	{
		private void BuildColumns(Hashtable elements, StiDataColumnsCollection columns, string name)
		{
			foreach (StiDataColumn column in columns)
			{
				string key = name + "." + StiNameValidator.CorrectName(column.Name, columns.DataSource?.Dictionary?.Report);
				elements[key] = column;
			}
		}

		private void BuildRelations(Hashtable elements, StiDataRelationsCollection relations, StiDataSourcesCollection datas, string name)
		{
			if (relations.Count <= 0)
			{
				return;
			}
			foreach (StiDataRelation relation in relations)
			{
				string text = name + "." + StiNameValidator.CorrectName(relation.Name, relation.Dictionary?.Report);
				elements[text] = relation;
				BuildData(elements, relation.ParentSource, datas, text, skipName: true);
				if (relation.Name != relation.NameInSource)
				{
					text = name + "." + StiNameValidator.CorrectName(relation.NameInSource, relation.Dictionary?.Report);
					elements[text] = relation;
					elements[StiNameValidator.CorrectName(relation.NameInSource, relation.Dictionary?.Report)] = relation;
					BuildData(elements, relation.ParentSource, datas, text, skipName: true);
				}
			}
		}

		private void BuildData(Hashtable elements, StiDataSource dataSource, StiDataSourcesCollection datas, string name, bool skipName)
		{
			string name2 = string.Empty;
			if (name != string.Empty)
			{
				name2 = name + "." + StiNameValidator.CorrectName(dataSource.Name, dataSource.Dictionary?.Report);
			}
			else if (name == string.Empty)
			{
				name2 = StiNameValidator.CorrectName(dataSource.Name, dataSource.Dictionary?.Report);
			}
			if (!datas.Contains(dataSource))
			{
				datas.Add(dataSource);
				if (skipName)
				{
					BuildRelations(elements, dataSource.GetParentRelations(), datas, name);
				}
				else
				{
					BuildRelations(elements, dataSource.GetParentRelations(), datas, name2);
				}
				datas.Remove(dataSource);
			}
			if (skipName)
			{
				BuildColumns(elements, dataSource.Columns, name);
			}
			else
			{
				BuildColumns(elements, dataSource.Columns, name2);
			}
		}

		public void Build(Hashtable elements, StiDictionary dictionary)
		{
			StiDataSourcesCollection dataSources = dictionary.DataSources;
			StiDataSourcesCollection datas = new StiDataSourcesCollection(null);
			foreach (StiDataSource item in dataSources)
			{
				string key = StiNameValidator.CorrectName(item.Name, dictionary?.Report);
				elements[key] = item;
				BuildData(elements, item, datas, "", skipName: false);
			}
		}
	}

	private enum StiState
	{
		Lexem,
		Find
	}

	public virtual Hashtable UsedColumns { get; private set; } = new Hashtable();


	public virtual Hashtable UsedRelations { get; private set; } = new Hashtable();


	public virtual Hashtable UsedDataSources { get; private set; } = new Hashtable();


	public void Dispose()
	{
		UsedColumns.Clear();
		UsedColumns = null;
		UsedRelations.Clear();
		UsedRelations = null;
		UsedDataSources.Clear();
		UsedDataSources = null;
	}

	private Hashtable BuildTokens(string script)
	{
		Hashtable hashtable = new Hashtable();
		int i = 0;
		StiState stiState = StiState.Find;
		string text = "";
		for (; i < script.Length; i++)
		{
			char c = script[i];
			switch (stiState)
			{
			case StiState.Find:
				if (char.IsLetter(c) || c == '_')
				{
					stiState = StiState.Lexem;
					text += c;
				}
				if (c == '"')
				{
					int num = script.IndexOf('"', i + 1);
					if (num != -1)
					{
						string text2 = script.Substring(i + 1, num - i - 1);
						hashtable[text2] = text2;
						i = num + 1;
					}
				}
				break;
			case StiState.Lexem:
				if (char.IsLetterOrDigit(c) || c == '_' || c == '.')
				{
					stiState = StiState.Lexem;
					text += c;
				}
				else
				{
					if (!(text != ""))
					{
						break;
					}
					for (; i < script.Length && script[i] != '.' && script[i] == ' '; i++)
					{
					}
					if (i < script.Length && script[i] == '.')
					{
						text += ".";
						for (; i < script.Length && script[i] == ' '; i++)
						{
						}
						break;
					}
					i--;
					hashtable[text] = text;
					text = "";
					stiState = StiState.Find;
				}
				break;
			}
		}
		if (stiState == StiState.Lexem)
		{
			hashtable[text] = text;
		}
		return hashtable;
	}

	public virtual void Retrieval(StiReport report)
	{
		if (report == null)
		{
			return;
		}
		StiCodeDomSerializator stiCodeDomSerializator = new StiCodeDomSerializator();
		string script = report.Script;
		string script2 = stiCodeDomSerializator.Serialize(report, report.GetReportName(), report.Language, serializeData: false, saveForInheritedReports: false);
		report.Script = StiLanguage.ReplaceGeneratedCode(report.Script, script2, report.Language);
		Hashtable hashtable = BuildTokens(report.Script);
		report.Script = script;
		StiDictionaryBuilder stiDictionaryBuilder = new StiDictionaryBuilder();
		Hashtable hashtable2 = new Hashtable();
		stiDictionaryBuilder.Build(hashtable2, report.Dictionary);
		foreach (string key2 in hashtable2.Keys)
		{
			if (hashtable[key2] == null)
			{
				continue;
			}
			object obj = hashtable2[key2];
			if (obj is StiDataSource && UsedDataSources[obj] == null)
			{
				UsedDataSources[obj] = obj;
			}
			else if (obj is StiDataColumn && UsedColumns[obj] == null)
			{
				UsedColumns[obj] = obj;
			}
			else if (obj is StiDataRelation && UsedRelations[obj] == null)
			{
				UsedRelations[obj] = obj;
			}
			string[] array = key2.Split(new char[1] { '.' });
			if (array.Length == 0)
			{
				continue;
			}
			string[] array2 = array;
			foreach (string key in array2)
			{
				object obj2 = hashtable2[key];
				if (obj2 != null)
				{
					if (obj2 is StiDataSource && UsedDataSources[obj2] == null)
					{
						UsedDataSources[obj2] = obj2;
					}
					else if (obj2 is StiDataColumn && UsedColumns[obj2] == null)
					{
						UsedColumns[obj2] = obj2;
					}
					else if (obj2 is StiDataRelation && UsedRelations[obj2] == null)
					{
						UsedRelations[obj2] = obj2;
					}
				}
			}
		}
		StiComponentsCollection components = report.GetComponents();
		foreach (StiComponent item in components)
		{
			if (item is IStiDataSource { DataSource: not null } stiDataSource)
			{
				UsedDataSources[stiDataSource.DataSource] = stiDataSource.DataSource;
			}
			if (item is IStiDataRelation { DataRelation: not null } stiDataRelation)
			{
				UsedRelations[stiDataRelation.DataRelation] = stiDataRelation.DataRelation;
			}
			if (!(item is IStiSort { Sort: not null } stiSort))
			{
				continue;
			}
			string text2 = string.Empty;
			if (item is IStiDataSource { DataSource: not null } stiDataSource2)
			{
				text2 = StiNameValidator.CorrectName(stiDataSource2.DataSource.Name, report) + ".";
			}
			int j = 1;
			while (j < stiSort.Sort.Length)
			{
				string text3 = string.Empty;
				for (; j < stiSort.Sort.Length && stiSort.Sort[j] != "ASC" && stiSort.Sort[j] != "DESC"; j++)
				{
					text3 = ((text3.Length != 0) ? (text3 + "." + stiSort.Sort[j]) : stiSort.Sort[j]);
				}
				j++;
				text3 = StiNameValidator.CorrectName(text3, report);
				if (hashtable2[text2 + text3] is StiDataColumn stiDataColumn)
				{
					UsedColumns[stiDataColumn] = stiDataColumn;
				}
			}
		}
		ICollection values = UsedColumns.Values;
		foreach (StiDataColumn item2 in values)
		{
			if (item2.DataSource != null && !UsedDataSources.Contains(item2.DataSource))
			{
				UsedDataSources[item2.DataSource] = item2.DataSource;
			}
		}
		ICollection values2 = UsedRelations.Values;
		foreach (StiDataRelation item3 in values2)
		{
			UsedDataSources[item3.ParentSource] = item3.ParentSource;
			UsedDataSources[item3.ChildSource] = item3.ChildSource;
		}
		ICollection values3 = UsedDataSources.Values;
		foreach (StiDataSource item4 in values3)
		{
			StiDataRelationsCollection parentRelations = item4.GetParentRelations();
			foreach (StiDataRelation item5 in parentRelations)
			{
				if (UsedDataSources[item5.ParentSource] != null)
				{
					UsedRelations[item5] = item5;
				}
			}
			parentRelations = item4.GetChildRelations();
			foreach (StiDataRelation item6 in parentRelations)
			{
				if (UsedDataSources[item6.ChildSource] != null)
				{
					UsedRelations[item6] = item6;
				}
			}
		}
		values2 = UsedRelations.Values;
		foreach (StiDataRelation item7 in values2)
		{
			if (item7.ParentSource != null)
			{
				string[] parentColumns = item7.ParentColumns;
				foreach (string name in parentColumns)
				{
					StiDataColumn stiDataColumn3 = item7.ParentSource.Columns[name];
					if (stiDataColumn3 != null)
					{
						UsedColumns[stiDataColumn3] = stiDataColumn3;
					}
				}
			}
			if (item7.ChildSource == null)
			{
				continue;
			}
			string[] childColumns = item7.ChildColumns;
			foreach (string name2 in childColumns)
			{
				StiDataColumn stiDataColumn4 = item7.ChildSource.Columns[name2];
				if (stiDataColumn4 != null)
				{
					UsedColumns[stiDataColumn4] = stiDataColumn4;
				}
			}
		}
	}
}
