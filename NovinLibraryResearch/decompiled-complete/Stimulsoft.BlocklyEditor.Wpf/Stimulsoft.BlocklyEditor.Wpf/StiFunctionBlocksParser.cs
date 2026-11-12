using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.BlocklyEditor.Wpf.Blocks;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.BlocklyEditor.Wpf;

public class StiFunctionBlocksParser
{
	public static string GetInitFunctionBlocks()
	{
		string text = string.Empty;
		StiFunction[] functions = StiFunctions.GetFunctions(isCompile: false);
		int num = 0;
		StiFunction[] array = functions;
		foreach (StiFunction function in array)
		{
			StiBlocklyBlock stiBlocklyBlock = new StiBlocklyBlock(function);
			text += stiBlocklyBlock.GetJsDefinition();
			num++;
		}
		return text;
	}

	public static string GetFunctionsGrouppedInCategoriesBlocks()
	{
		Hashtable functionsGrouppedInCategories = StiFunctions.GetFunctionsGrouppedInCategories();
		string[] array = new string[functionsGrouppedInCategories.Keys.Count];
		functionsGrouppedInCategories.Keys.CopyTo(array, 0);
		Array.Sort(array);
		string text = string.Empty;
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			List<StiFunction> source = functionsGrouppedInCategories[text2] as List<StiFunction>;
			string text3 = string.Empty;
			foreach (StiFunction item in source.OrderBy((StiFunction f) => f.FunctionName).ToList())
			{
				StiBlocklyBlock stiBlocklyBlock = new StiBlocklyBlock(item);
				text3 += stiBlocklyBlock.GetXmlToolboxDefinition();
			}
			text += GetCategoryBlock(text2.ToString(), "#4e85c3", text3);
		}
		return text;
	}

	private static string GetCategoryBlock(string name, string color, string content)
	{
		return "<category name=\"" + name + "\" colour=\"" + color + "\">" + content + "</category>";
	}
}
