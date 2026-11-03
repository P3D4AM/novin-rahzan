using System.Collections;
using System.Globalization;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design;

public class StiExpressionPacker
{
	private class Word
	{
		public string First;

		public string Second;

		public Word(string first, string second)
		{
			First = first;
			Second = second;
		}
	}

	public static string PackExpression(string expressionStr, IStiDesignerBase designer, bool useBraces)
	{
		if (designer != null && designer.UseAliases)
		{
			return PackExpression(expressionStr, designer.Report, useBraces);
		}
		return expressionStr;
	}

	public static string UnPackExpression(string expressionStr, IStiDesignerBase designer, bool useBraces)
	{
		if (designer != null && designer.UseAliases)
		{
			return UnPackExpression(expressionStr, designer.Report, useBraces);
		}
		return expressionStr;
	}

	public static string PackExpression(string expressionStr, StiReport report, bool useBraces)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Invalid comparison between Unknown and I4
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Invalid comparison between Unknown and I4
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Invalid comparison between Unknown and I4
		Hashtable hashFirst = new Hashtable();
		Hashtable hashSecond = new Hashtable();
		BuildDictionary(report, ref hashFirst, ref hashSecond, isPack: true);
		int num = 0;
		StiLexer val = new StiLexer(expressionStr);
		StiToken val2 = null;
		StiToken val3 = null;
		StringBuilder stringBuilder = new StringBuilder(expressionStr);
		int num2 = 0;
		while (true)
		{
			StiToken token = val.GetToken();
			if ((int)token.Type == 42)
			{
				break;
			}
			if (useBraces && (int)token.Type == 35)
			{
				num2++;
			}
			else if (useBraces && (int)token.Type == 36)
			{
				num2--;
			}
			else if ((int)token.Type == 40 && (!useBraces || (useBraces && num2 > 0)))
			{
				string key = token.Data.ToString().Replace(" ", "").ToLowerInvariant();
				string text = null;
				if (val2 != null && (int)val2.Type == 1)
				{
					if (hashSecond[key] is ArrayList arrayList)
					{
						foreach (object item in arrayList)
						{
							if (!(item is Word word))
							{
								continue;
							}
							if (val3 != null)
							{
								if ((int)val3.Type == 40 && val3.Data.ToString().ToLowerInvariant() == word.First.ToLowerInvariant())
								{
									text = word.Second;
								}
							}
							else
							{
								text = word.Second;
							}
						}
					}
				}
				else
				{
					text = hashFirst[key] as string;
				}
				if (text != null)
				{
					int num3 = text.Length - token.Length;
					stringBuilder = stringBuilder.Remove(token.Index + num, token.Length);
					stringBuilder = stringBuilder.Insert(token.Index + num, text);
					num += num3;
				}
			}
			val3 = val2;
			val2 = token;
		}
		return stringBuilder.ToString();
	}

	public static string UnPackExpression(string expressionStr, StiReport report, bool useBraces)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Invalid comparison between Unknown and I4
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Invalid comparison between Unknown and I4
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Invalid comparison between Unknown and I4
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Invalid comparison between Unknown and I4
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Invalid comparison between Unknown and I4
		Hashtable hashFirst = new Hashtable();
		Hashtable hashSecond = new Hashtable();
		BuildDictionary(report, ref hashFirst, ref hashSecond, isPack: false);
		int num = 0;
		StiLexer val = new StiLexer(expressionStr);
		StiToken val2 = null;
		StiToken val3 = null;
		StringBuilder stringBuilder = new StringBuilder(expressionStr);
		int num2 = 0;
		string text = null;
		while (true)
		{
			StiToken val4 = val.GetToken();
			if ((int)val4.Type == 42)
			{
				break;
			}
			if (useBraces && (int)val4.Type == 35)
			{
				num2++;
			}
			else if (useBraces && (int)val4.Type == 36)
			{
				num2--;
			}
			else if ((!useBraces || (useBraces && num2 > 0)) && ((int)val4.Type == 37 || (int)val4.Type == 40))
			{
				int num3 = -1;
				int num4 = -1;
				string text2 = null;
				if ((int)val4.Type == 37)
				{
					num3 = val4.Index + num;
					num4 = num3;
					while (true)
					{
						StiToken token = val.GetToken();
						if (val4 == null)
						{
							val4 = token;
						}
						if ((int)token.Type == 42)
						{
							num3 = -1;
							break;
						}
						if ((int)token.Type == 38)
						{
							break;
						}
						num4 = token.Index + token.Length + num;
					}
					if (num3 == -1)
					{
						break;
					}
					num3 -= num;
					num4 -= num;
					text2 = expressionStr.Substring(num3, num4 - num3 + 1);
				}
				else
				{
					num3 = val4.Index;
					num4 = val4.Index + val4.Length;
					text2 = val4.Data.ToString();
				}
				int length = text2.Length;
				text2 = text2.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
				string text3 = null;
				if (val2 != null && (int)val2.Type == 1)
				{
					if (hashSecond[text2] is ArrayList arrayList)
					{
						foreach (object item in arrayList)
						{
							if (!(item is Word word))
							{
								continue;
							}
							if (val3 != null)
							{
								if (text != null && text.ToLowerInvariant() == word.First.ToLowerInvariant())
								{
									text3 = word.Second;
								}
							}
							else
							{
								text3 = word.Second;
							}
						}
					}
				}
				else
				{
					text3 = hashFirst[text2] as string;
				}
				if (text3 != null)
				{
					int num5 = text3.Length - length;
					stringBuilder = stringBuilder.Remove(val4.Index + num, length);
					stringBuilder = stringBuilder.Insert(val4.Index + num, text3);
					num += num5;
				}
				text = text3;
			}
			val3 = val2;
			val2 = val4;
		}
		return stringBuilder.ToString();
	}

	private static bool IsValidName(string name)
	{
		if (string.IsNullOrEmpty(name) || (!char.IsLetter(name[0]) && name[0] != '_'))
		{
			return false;
		}
		foreach (char c in name)
		{
			if (!char.IsLetterOrDigit(c) && c != '_')
			{
				return false;
			}
		}
		return true;
	}

	public static string GetCorrectedAlias(StiReport report, string alias)
	{
		if (!IsValidName(alias))
		{
			return "[" + alias + "]";
		}
		return alias;
	}

	private static void AddWord(Hashtable hash, string item, object obj)
	{
		if (hash.ContainsKey(item))
		{
			ArrayList arrayList = hash[item] as ArrayList;
			arrayList.Add(obj);
		}
		else
		{
			ArrayList arrayList2 = new ArrayList();
			arrayList2.Add(obj);
			hash[item] = arrayList2;
		}
	}

	private static void BuildDictionary(StiReport report, ref Hashtable hashFirst, ref Hashtable hashSecond, bool isPack)
	{
		hashFirst = new Hashtable();
		hashSecond = new Hashtable();
		if (report != null)
		{
			foreach (StiDataSource dataSource in report.Dictionary.DataSources)
			{
				string text = dataSource.Name;
				string correctedAlias = GetCorrectedAlias(report, dataSource.Alias);
				if (isPack)
				{
					text = text.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
					if (dataSource.Name != dataSource.Alias)
					{
						hashFirst[text] = correctedAlias;
					}
					if (StiNameValidator.CorrectName(dataSource.Name, report) != dataSource.Alias)
					{
						hashFirst[StiNameValidator.CorrectName(text, report)] = correctedAlias;
						hashFirst[StiNameValidator.CorrectName(dataSource.Name, report).ToLower(CultureInfo.InvariantCulture)] = correctedAlias;
					}
				}
				else
				{
					correctedAlias = correctedAlias.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
					hashFirst[correctedAlias] = StiNameValidator.CorrectName(text, report);
				}
				foreach (StiDataColumn column in dataSource.Columns)
				{
					string name = column.Name;
					string correctedAlias2 = GetCorrectedAlias(report, column.Alias);
					if (isPack)
					{
						name = name.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
						string text2 = StiNameValidator.CorrectName(name, report);
						if (column.Name != column.Alias)
						{
							AddWord(hashSecond, name, new Word(text, correctedAlias2));
						}
						if (StiNameValidator.CorrectName(column.Name, report) != column.Alias)
						{
							AddWord(hashSecond, StiNameValidator.CorrectName(name, report), new Word(text, correctedAlias2));
							AddWord(hashSecond, StiNameValidator.CorrectName(column.Name, report).ToLower(CultureInfo.InvariantCulture), new Word(text, correctedAlias2));
						}
					}
					else
					{
						correctedAlias2 = correctedAlias2.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
						AddWord(hashSecond, correctedAlias2, new Word(text, StiNameValidator.CorrectName(name, report)));
					}
				}
			}
			foreach (StiBusinessObject businessObject in report.Dictionary.BusinessObjects)
			{
				BuildBusinessObject(report, businessObject, ref hashFirst, ref hashSecond, isPack);
			}
		}
		if (report != null)
		{
			foreach (StiDataRelation relation in report.Dictionary.Relations)
			{
				string name2 = relation.Name;
				string correctedAlias3 = GetCorrectedAlias(report, relation.Alias);
				if (isPack)
				{
					name2 = name2.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
					if (relation.Name != relation.Alias)
					{
						AddWord(hashSecond, name2, correctedAlias3);
					}
					if (StiNameValidator.CorrectName(relation.Name, report) != relation.Alias)
					{
						AddWord(hashSecond, StiNameValidator.CorrectName(name2, report), correctedAlias3);
						AddWord(hashSecond, StiNameValidator.CorrectName(relation.Name, report).ToLower(CultureInfo.InvariantCulture), correctedAlias3);
					}
				}
				else
				{
					correctedAlias3 = correctedAlias3.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
					AddWord(hashSecond, correctedAlias3, StiNameValidator.CorrectName(name2, report));
				}
			}
		}
		if (report == null)
		{
			return;
		}
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			string name3 = variable.Name;
			string correctedAlias4 = GetCorrectedAlias(report, variable.Alias);
			if (isPack)
			{
				name3 = name3.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
				if (variable.Name != variable.Alias)
				{
					hashFirst[name3] = correctedAlias4;
				}
				if (StiNameValidator.CorrectName(variable.Name, report) != variable.Alias)
				{
					hashFirst[StiNameValidator.CorrectName(name3, report)] = correctedAlias4;
					hashFirst[StiNameValidator.CorrectName(variable.Name, report).ToLower(CultureInfo.InvariantCulture)] = correctedAlias4;
				}
			}
			else
			{
				correctedAlias4 = correctedAlias4.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
				hashFirst[correctedAlias4] = StiNameValidator.CorrectName(name3, report);
			}
		}
	}

	private static void BuildBusinessObject(StiReport report, StiBusinessObject businesObject, ref Hashtable hashFirst, ref Hashtable hashSecond, bool isPack)
	{
		string text = businesObject.Name;
		string correctedAlias = GetCorrectedAlias(report, businesObject.Alias);
		if (isPack)
		{
			text = text.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
			if (businesObject.Name != businesObject.Alias)
			{
				hashFirst[text] = correctedAlias;
			}
			if (StiNameValidator.CorrectName(businesObject.Name, report) != businesObject.Alias)
			{
				hashFirst[StiNameValidator.CorrectName(text, report)] = correctedAlias;
				hashFirst[StiNameValidator.CorrectName(businesObject.Name, report).ToLower(CultureInfo.InvariantCulture)] = correctedAlias;
			}
		}
		else
		{
			correctedAlias = correctedAlias.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
			hashFirst[correctedAlias] = StiNameValidator.CorrectName(text, report);
		}
		foreach (StiDataColumn column in businesObject.Columns)
		{
			string name = column.Name;
			string correctedAlias2 = GetCorrectedAlias(report, column.Alias);
			if (isPack)
			{
				name = name.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
				if (column.Name != column.Alias)
				{
					AddWord(hashSecond, name, new Word(text, correctedAlias2));
				}
				if (StiNameValidator.CorrectName(column.Name, report) != column.Alias)
				{
					AddWord(hashSecond, StiNameValidator.CorrectName(name, report), new Word(text, correctedAlias2));
					AddWord(hashSecond, StiNameValidator.CorrectName(column.Name, report).ToLower(CultureInfo.InvariantCulture), new Word(text, correctedAlias2));
				}
			}
			else
			{
				correctedAlias2 = correctedAlias2.Replace(" ", "").ToLower(CultureInfo.InvariantCulture);
				AddWord(hashSecond, correctedAlias2, new Word(text, StiNameValidator.CorrectName(name, report)));
			}
		}
		foreach (StiBusinessObject businessObject in businesObject.BusinessObjects)
		{
			BuildBusinessObject(report, businessObject, ref hashFirst, ref hashSecond, isPack);
		}
	}
}
