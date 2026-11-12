using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Stimulsoft.Base.Localization;
using Stimulsoft.Blockly.StiBlocks.Functions;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.BlocklyEditor.Wpf.Blocks;

public class StiBlocklyBlock
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Tooltip { get; set; }

	public string HelpUrl { get; set; }

	public bool PreviousStatement { get; set; }

	public bool NextStatement { get; set; }

	public bool InputsInline { get; set; }

	public bool Output { get; set; } = true;


	public string Color { get; set; } = "#2969b0";


	public List<StiBlocklyInput> ValueInputs { get; set; } = new List<StiBlocklyInput>();


	public StiFunction Function { get; set; }

	public string GetJsDefinition()
	{
		string inputJsDefinitions = GetInputJsDefinitions();
		return "Blockly.Blocks['" + Id + "'] = {init: function() {" + inputJsDefinitions + "this.setInputsInline(" + InputsInline.ToString().ToLower() + ");this.setPreviousStatement(" + PreviousStatement.ToString().ToLower() + ", null);this.setNextStatement(" + NextStatement.ToString().ToLower() + ", null);this.setOutput(" + Output.ToString().ToLower() + ", null);this.setColour(\"" + Color + "\");this.setTooltip(\"" + Tooltip?.Replace("\"", "\\\"") + "\");this.setHelpUrl(\"" + HelpUrl + "\");}};";
	}

	private string GetInputJsDefinitions()
	{
		string text = string.Empty;
		foreach (StiBlocklyInput valueInput in ValueInputs)
		{
			text += valueInput.GetDefinitions();
		}
		return text;
	}

	public string GetXmlToolboxDefinition()
	{
		return "<block type=\"" + Id + "\"></block>";
	}

	public string GetTooltip(StiFunction function)
	{
		string text = function.GetLongFunctionString(StiReportLanguageType.CSharp) + " \\n\\n ";
		string description = function.Description;
		string[] argumentDescriptions = function.ArgumentDescriptions;
		string[] argumentNames = function.ArgumentNames;
		Type[] argumentTypes = function.ArgumentTypes;
		string returnDescription = function.ReturnDescription;
		text = text + description + " \\n\\n ";
		if (argumentNames != null && argumentNames.Length != 0)
		{
			text = text + " " + Loc.Get("PropertyMain", "Parameters") + " \\n\\n ";
			int num = 0;
			string[] array = argumentNames;
			foreach (string text2 in array)
			{
				text = ((argumentDescriptions == null || num >= argumentDescriptions.Length) ? (text + text2 + " \\n\\n ") : ((argumentTypes == null || !argumentTypes[num].IsArray) ? (text + " " + text2 + " - " + argumentDescriptions[num++].Replace("\"", "").Replace("\r\n", "").Replace("\n", "")
					.ToLower() + "  \\n\\n ") : (text + argumentDescriptions[num++] + " \\n\\n ")));
			}
		}
		if (!string.IsNullOrEmpty(returnDescription))
		{
			text = text + " \\n\\n " + Loc.Get("PropertyMain", "ReturnValue") + " \\n\\n ";
			text += returnDescription;
		}
		RegexOptions options = RegexOptions.None;
		Regex regex = new Regex("[ ]{2,}", options);
		return regex.Replace(text, " ");
	}

	public StiBlocklyBlock(StiFunction function)
	{
		Id = StiBlocklyFunctionBlockKeyCache.CreateKey(function);
		Function = function;
		Name = function.FunctionName;
		string longFunctionString = function.GetLongFunctionString(StiReportLanguageType.CSharp);
		Tooltip = GetTooltip(function);
		StiBlocklyDummyInput item = new StiBlocklyDummyInput
		{
			Fields = 
			{
				new StiBlocklyField
				{
					Name = longFunctionString
				}
			}
		};
		ValueInputs.Add(item);
		string[] argumentNames = function.ArgumentNames;
		foreach (string name in argumentNames)
		{
			StiBlocklyValueInput stiBlocklyValueInput = new StiBlocklyValueInput
			{
				Name = name
			};
			stiBlocklyValueInput.Fields.Add(new StiBlocklyField
			{
				Name = name
			});
			ValueInputs.Add(stiBlocklyValueInput);
		}
	}
}
