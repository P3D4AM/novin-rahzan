using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiFunctions
{
	public static void RemoveFunction(string functionName)
	{
		if (StiAppFunctions.FunctionsToCompile[functionName] != null)
		{
			StiAppFunctions.FunctionsToCompile.Remove(functionName);
		}
		if (StiAppFunctions.FunctionsToCompileLower[functionName.ToLowerInvariant()] != null)
		{
			StiAppFunctions.FunctionsToCompileLower.Remove(functionName.ToLowerInvariant());
		}
		if (StiAppFunctions.Functions[functionName] != null)
		{
			StiAppFunctions.Functions.Remove(functionName);
		}
		if (StiAppFunctions.FunctionsLower[functionName.ToLowerInvariant()] != null)
		{
			StiAppFunctions.FunctionsLower.Remove(functionName.ToLowerInvariant());
		}
	}

	public static List<StiFunction> GetFunctionsList(string functionName)
	{
		if (StiAppFunctions.Functions[functionName] == null)
		{
			return null;
		}
		return (StiAppFunctions.Functions[functionName] as List<IStiAppFunction>)?.Cast<StiFunction>().ToList();
	}

	public static Hashtable GetFunctionsGrouppedInCategories()
	{
		Hashtable hashtable = new Hashtable();
		StiFunction[] functions = GetFunctions(isCompile: false);
		StiFunction[] array = functions;
		foreach (StiFunction stiFunction in array)
		{
			List<StiFunction> list = hashtable[stiFunction.Category] as List<StiFunction>;
			if (list == null)
			{
				list = new List<StiFunction>();
				hashtable[stiFunction.Category] = list;
			}
			list.Add(stiFunction);
		}
		return hashtable;
	}

	public static List<StiFunction> GetFunctions(string category)
	{
		StiFunction[] functions = GetFunctions(isCompile: false);
		List<StiFunction> list = new List<StiFunction>();
		StiFunction[] array = functions;
		foreach (StiFunction stiFunction in array)
		{
			if (stiFunction.Category == category)
			{
				list.Add(stiFunction);
			}
		}
		return list;
	}

	public static List<string> GetCategories()
	{
		Hashtable hashtable = new Hashtable();
		StiFunction[] functions = GetFunctions(isCompile: false);
		List<string> list = new List<string>();
		StiFunction[] array = functions;
		foreach (StiFunction stiFunction in array)
		{
			if (hashtable[stiFunction.Category] == null)
			{
				list.Add(stiFunction.Category);
				hashtable[stiFunction.Category] = stiFunction.Category;
			}
		}
		return list;
	}

	public static string[] GetAssebliesOfFunctions()
	{
		StiFunction[] functions = GetFunctions(isCompile: true);
		Hashtable hashtable = new Hashtable();
		StiFunction[] array = functions;
		foreach (StiFunction stiFunction in array)
		{
			if (stiFunction != null && stiFunction.TypeOfFunction.Assembly != null && !string.IsNullOrWhiteSpace(stiFunction.TypeOfFunction.Assembly.Location))
			{
				hashtable[stiFunction.TypeOfFunction.Assembly.Location] = stiFunction.TypeOfFunction.Assembly.FullName;
			}
		}
		string[] array2 = new string[hashtable.Count];
		hashtable.Keys.CopyTo(array2, 0);
		return array2;
	}

	public static StiFunction[] GetFunctions(bool isCompile)
	{
		List<StiFunction> list = new List<StiFunction>();
		Hashtable hashtable = (isCompile ? StiAppFunctions.FunctionsToCompile : StiAppFunctions.Functions);
		foreach (string key in hashtable.Keys)
		{
			StiFunction[] functions = GetFunctions(null, key, isCompile);
			StiFunction[] array = functions;
			foreach (StiFunction item in array)
			{
				list.Add(item);
			}
		}
		return list.ToArray();
	}

	public static StiFunction[] GetFunctions(StiReport report, string functionName, bool isCompile)
	{
		return StiAppFunctions.GetFunctions(functionName, isCompile, report == null || report.ScriptLanguage != StiReportLanguageType.VB)?.Cast<StiFunction>()?.ToArray();
	}

	public static StiFunction AddFunction(string category, string functionName, Type typeOfFunction, Type returnType)
	{
		return AddFunction(category, functionName, string.Empty, typeOfFunction, returnType, string.Empty, null, null);
	}

	public static StiFunction AddFunction(string category, string functionName, string description, Type typeOfFunction, Type returnType, string returnDescription)
	{
		return AddFunction(category, functionName, description, typeOfFunction, returnType, returnDescription, null, null);
	}

	public static StiFunction AddFunction(string category, string functionName, string description, Type typeOfFunction, Type returnType, string returnDescription, Type[] argumentTypes, string[] argumentNames)
	{
		return AddFunction(category, functionName, functionName, description, typeOfFunction, returnType, returnDescription, argumentTypes, argumentNames, null);
	}

	public static StiFunction AddFunction(string category, string functionName, string description, Type typeOfFunction, Type returnType, string returnDescription, Type[] argumentTypes, string[] argumentNames, string[] argumentDescriptions)
	{
		return AddFunction(category, functionName, functionName, description, typeOfFunction, returnType, returnDescription, argumentTypes, argumentNames, null);
	}

	public static StiFunction AddFunction(string category, string groupFunctionName, string functionName, string description, Type typeOfFunction, Type returnType, string returnDescription, Type[] argumentTypes, string[] argumentNames, string[] argumentDescriptions)
	{
		if (string.IsNullOrEmpty(groupFunctionName))
		{
			groupFunctionName = functionName;
		}
		StiFunction stiFunction = new StiFunction(category, groupFunctionName, functionName, description, typeOfFunction, returnType, returnDescription, argumentTypes, argumentNames, argumentDescriptions);
		List<IStiAppFunction> list = StiAppFunctions.Functions[groupFunctionName] as List<IStiAppFunction>;
		if (list == null)
		{
			list = new List<IStiAppFunction>();
			StiAppFunctions.Functions[groupFunctionName] = list;
			StiAppFunctions.FunctionsLower[groupFunctionName.ToLowerInvariant()] = list;
		}
		else
		{
			foreach (IStiAppFunction item in list)
			{
				if (item is StiFunction stiFunction2 && stiFunction2.FunctionName == stiFunction.FunctionName && IsEqual(stiFunction2, stiFunction))
				{
					return stiFunction;
				}
			}
		}
		list.Add((IStiAppFunction)(object)stiFunction);
		list = StiAppFunctions.FunctionsToCompile[functionName] as List<IStiAppFunction>;
		if (list == null)
		{
			list = new List<IStiAppFunction>();
			StiAppFunctions.FunctionsToCompile[functionName] = list;
			StiAppFunctions.FunctionsToCompileLower[functionName.ToLowerInvariant()] = list;
		}
		list.Add((IStiAppFunction)(object)stiFunction);
		return stiFunction;
	}

	private static bool IsEqual(StiFunction func1, StiFunction func2)
	{
		if (func1.Category == func2.Category && func1.GroupFunctionName == func2.GroupFunctionName && func1.FunctionName == func2.FunctionName && func1.Description == func2.Description && func1.TypeOfFunction.ToString() == func2.TypeOfFunction.ToString() && func1.ReturnType.ToString() == func2.ReturnType.ToString() && func1.ReturnDescription == func2.ReturnDescription && IsEqualArray(func1.ArgumentTypes, func2.ArgumentTypes) && IsEqualArray(func1.ArgumentNames, func2.ArgumentNames))
		{
			return IsEqualArray(func1.ArgumentDescriptions, func2.ArgumentDescriptions);
		}
		return false;
	}

	private static bool IsEqualArray(Array arr1, Array arr2)
	{
		if (arr1 == null && arr2 == null)
		{
			return true;
		}
		if (arr1 == null || arr2 == null)
		{
			return false;
		}
		if (arr1.Length != arr2.Length)
		{
			return false;
		}
		for (int i = 0; i < arr1.Length; i++)
		{
			if (arr1.GetValue(i) != arr2.GetValue(i))
			{
				return false;
			}
		}
		return true;
	}

	static StiFunctions()
	{
	}
}
