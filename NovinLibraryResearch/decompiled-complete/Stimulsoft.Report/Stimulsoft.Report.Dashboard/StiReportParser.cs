using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Dashboard;

public static class StiReportParser
{
	private static Dictionary<string, object> cache = new Dictionary<string, object>();

	private static Dictionary<string, object> wrongCache = new Dictionary<string, object>();

	public static object LockObject = new object();

	public static string Parse(string expression, IStiReport report, bool allowCache = true, Dictionary<string, object> constants = null, bool allowDataLoading = true, bool onlyExpression = false)
	{
		return Parse(expression, (IStiReportComponent)(object)report.FetchPages().FirstOrDefault(), allowCache, constants, allowDataLoading, onlyExpression);
	}

	public static string Parse(string expression, IStiReportComponent component, bool allowCache = true, Dictionary<string, object> constants = null, bool allowDataLoading = true, bool onlyExpression = false)
	{
		object obj = ParseObject(expression, component, allowCache, constants, allowDataLoading, onlyExpression);
		if (obj == null)
		{
			return "";
		}
		return obj.ToString();
	}

	public static object ParseObject(string expression, IStiReportComponent component, bool allowCache = true, Dictionary<string, object> constants = null, bool allowDataLoading = true, bool onlyExpression = false)
	{
		if (component == null)
		{
			return expression;
		}
		lock (LockObject)
		{
			if (onlyExpression && expression != null)
			{
				expression = PrepareExpression(expression);
			}
			if (expression != null && !expression.Contains("{"))
			{
				return expression;
			}
			object fromWrongCache = GetFromWrongCache(expression, component);
			if (fromWrongCache != null)
			{
				return fromWrongCache;
			}
			fromWrongCache = ((allowCache && constants != null) ? GetFromCache(expression, component) : null);
			if (fromWrongCache != null)
			{
				return fromWrongCache;
			}
			fromWrongCache = ParseOrDefault(expression, component, constants, allowDataLoading);
			if (fromWrongCache as string == expression && expression != null && expression.Contains("{"))
			{
				AddToWrongCache(expression, fromWrongCache, component);
				return fromWrongCache;
			}
			if (allowCache && constants != null)
			{
				AddToCache(expression, fromWrongCache, component);
			}
			return fromWrongCache;
		}
	}

	private static object ParseOrDefault(string expression, IStiReportComponent component, Dictionary<string, object> constants = null, bool allowDataLoading = true)
	{
		if (!TryParse(expression, out var result, component, constants, allowDataLoading))
		{
			return expression;
		}
		return result;
	}

	private static bool TryParse(string expression, out object result, IStiReportComponent component, Dictionary<string, object> constants = null, bool allowDataLoading = true)
	{
		lock (LockObject)
		{
			try
			{
				if (expression == null || !expression.Contains("{"))
				{
					result = expression;
					return true;
				}
				result = StiParser.ParseTextValue(expression, component as StiComponent, new StiParserParameters
				{
					SyntaxCaseSensitive = false,
					Constants = PrepareConstants(constants),
					GetDataFieldValue = (allowDataLoading ? new StiParser.StiParserGetDataFieldValueDelegate(GetDataFieldValueProcessor) : null)
				});
				return true;
			}
			catch
			{
				result = null;
				return false;
			}
		}
	}

	private static Hashtable PrepareConstants(Dictionary<string, object> constants)
	{
		Hashtable constantsHash = null;
		if (constants != null)
		{
			constantsHash = new Hashtable();
			constants.Keys.Where((string k) => k != null).ToList().ForEach(delegate(string k)
			{
				constantsHash[k] = constants[k];
			});
		}
		return constantsHash;
	}

	public static void GetDataFieldValueProcessor(object sender, StiParser.StiParserGetDataFieldValueEventArgs e)
	{
		StiReport stiReport = sender as StiReport;
		DataTable dataTable = StiDataPicker.Fetch((IStiApp)(object)stiReport, e.DataSourceName, (StiDataRequestOption)3);
		if (dataTable == null)
		{
			return;
		}
		string dataName = e.DataSourceName + "." + e.DataColumnName;
		DataRow dataRow = dataTable.Rows.Cast<DataRow>().FirstOrDefault();
		if (dataRow == null)
		{
			return;
		}
		DataColumn dataColumn = (from DataColumn c in dataTable.Columns
			where c.ColumnName == dataName || c.ColumnName.Replace(" ", "") == dataName.Replace(" ", "")
			select c).FirstOrDefault();
		if (dataColumn != null)
		{
			int num = dataTable.Columns.IndexOf(dataColumn);
			if (num >= 0 && num < dataRow.ItemArray.Length)
			{
				e.Value = dataRow[dataName];
				e.Processed = true;
			}
		}
	}

	private static string PrepareExpression(string expression)
	{
		expression = expression.Trim();
		if (!expression.Contains("{"))
		{
			expression = "{" + expression;
		}
		if (!expression.Contains("}"))
		{
			expression += "}";
		}
		return expression;
	}

	private static string GetCacheKey(string expression, IStiReportComponent component)
	{
		string text = StiAppKey.GetOrGeneratedKey(component) ?? string.Empty;
		string text2 = expression ?? string.Empty;
		return text + "." + text2;
	}

	internal static void AddToCache(string expression, object result, IStiReportComponent component)
	{
		lock (cache)
		{
			if (result == null)
			{
				result = string.Empty;
			}
			string cacheKey = GetCacheKey(expression, component);
			cache[cacheKey] = result;
		}
	}

	internal static void AddToWrongCache(string expression, object result, IStiReportComponent component)
	{
		lock (cache)
		{
			if (result == null)
			{
				result = string.Empty;
			}
			string cacheKey = GetCacheKey(expression, component);
			wrongCache[cacheKey] = result;
		}
	}

	internal static object GetFromCache(string expression, IStiReportComponent component)
	{
		lock (cache)
		{
			if (string.IsNullOrEmpty(expression))
			{
				return string.Empty;
			}
			if (string.IsNullOrWhiteSpace(expression))
			{
				return expression;
			}
			string cacheKey = GetCacheKey(expression, component);
			return cache.ContainsKey(cacheKey) ? cache[cacheKey] : null;
		}
	}

	internal static object GetFromWrongCache(string expression, IStiReportComponent component)
	{
		lock (wrongCache)
		{
			if (string.IsNullOrEmpty(expression))
			{
				return string.Empty;
			}
			if (string.IsNullOrWhiteSpace(expression))
			{
				return expression;
			}
			string cacheKey = GetCacheKey(expression, component);
			return wrongCache.ContainsKey(cacheKey) ? wrongCache[cacheKey] : null;
		}
	}

	public static void CleanCache(string reportKey)
	{
		lock (cache)
		{
			if (reportKey == null)
			{
				cache.Clear();
			}
			else
			{
				cache.Keys.Where((string k) => k.StartsWith(reportKey)).ToList().ForEach(delegate(string k)
				{
					cache.Remove(k);
				});
			}
		}
		lock (wrongCache)
		{
			if (reportKey == null)
			{
				wrongCache.Clear();
				return;
			}
			wrongCache.Keys.Where((string k) => k.StartsWith(reportKey)).ToList().ForEach(delegate(string k)
			{
				wrongCache.Remove(k);
			});
		}
	}
}
