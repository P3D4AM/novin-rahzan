using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.CodeDom;

namespace Stimulsoft.Report.Dictionary;

public class StiFunction : IComparable, IStiAppFunction, IStiAppCell
{
	public string Key { get; set; } = StiKeyHelper.GenerateKey();


	public bool UseFullPath { get; set; } = true;


	public string Category { get; set; }

	public string GroupFunctionName { get; set; }

	public string FunctionName { get; set; }

	public string Description { get; set; }

	public Type TypeOfFunction { get; set; }

	public Type ReturnType { get; set; }

	public string ReturnDescription { get; set; }

	public Type[] ArgumentTypes { get; set; }

	public string[] ArgumentNames { get; set; }

	public string[] ArgumentDescriptions { get; set; }

	public int CompareTo(object obj)
	{
		StiFunction stiFunction = obj as StiFunction;
		return FunctionName.CompareTo(stiFunction.FunctionName);
	}

	string IStiAppCell.GetKey()
	{
		return Key;
	}

	void IStiAppCell.SetKey(string key)
	{
		Key = key;
	}

	string IStiAppFunction.GetName()
	{
		return FunctionName;
	}

	object IStiAppFunction.Invoke(IEnumerable<object> arguments)
	{
		List<Type> list = new List<Type>();
		foreach (object argument in arguments)
		{
			list.Add(argument.GetType());
		}
		return TypeOfFunction.GetMethod(FunctionName, list.ToArray())?.Invoke(null, arguments?.ToArray());
	}

	public override string ToString()
	{
		return FunctionName;
	}

	public string GetLongFunctionString(StiReportLanguageType language)
	{
		if (language == StiReportLanguageType.CSharp || language == StiReportLanguageType.JS)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(ConvertTypeToString(ReturnType, language));
			stringBuilder.Append("  ");
			stringBuilder.Append(FunctionName);
			stringBuilder.Append(" (");
			int num = 0;
			if (ArgumentTypes != null)
			{
				Type[] argumentTypes = ArgumentTypes;
				foreach (Type type in argumentTypes)
				{
					string value = ArgumentNames[num];
					if (!type.IsArray)
					{
						stringBuilder.Append(ConvertTypeToString(type, language));
						stringBuilder.Append(" ");
					}
					stringBuilder.Append(value);
					num++;
					if (num != ArgumentTypes.Length)
					{
						stringBuilder.Append(", ");
					}
				}
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2.Append(FunctionName);
		stringBuilder2.Append("(");
		int num2 = 0;
		if (ArgumentTypes != null)
		{
			Type[] argumentTypes2 = ArgumentTypes;
			foreach (Type type2 in argumentTypes2)
			{
				string value2 = ArgumentNames[num2];
				stringBuilder2.Append(value2);
				stringBuilder2.Append(" As ");
				stringBuilder2.Append(ConvertTypeToString(type2, language));
				num2++;
				if (num2 != ArgumentTypes.Length)
				{
					stringBuilder2.Append(", ");
				}
			}
		}
		stringBuilder2.Append(")");
		if (ReturnType != typeof(void))
		{
			stringBuilder2.Append(" As " + ConvertTypeToString(ReturnType, language));
		}
		return stringBuilder2.ToString();
	}

	public string GetFunctionString(StiReportLanguageType language)
	{
		return GetFunctionString(language, addFunctionName: true);
	}

	public string GetFunctionString(StiReportLanguageType language, bool addFunctionName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (addFunctionName)
		{
			stringBuilder.Append(FunctionName);
		}
		stringBuilder.Append(" (");
		int num = 0;
		if (ArgumentTypes != null)
		{
			Type[] argumentTypes = ArgumentTypes;
			foreach (Type type in argumentTypes)
			{
				string value = ArgumentNames[num];
				if (type.IsArray)
				{
					stringBuilder.Append(value);
				}
				else
				{
					stringBuilder.Append(ConvertTypeToString(type, language));
				}
				num++;
				if (num != ArgumentTypes.Length)
				{
					stringBuilder.Append(", ");
				}
			}
		}
		stringBuilder.Append(")");
		if (ReturnType != typeof(void))
		{
			stringBuilder.Append(" : " + ConvertTypeToString(ReturnType, language));
		}
		return stringBuilder.ToString();
	}

	public string ConvertTypeToString(Type type, StiReportLanguageType language)
	{
		string text = StiCodeDomSerializator.ConvertTypeToString(type, language);
		if (type == typeof(Color))
		{
			return "Color";
		}
		if (type == typeof(StiSolidBrush))
		{
			return "SolidBrush";
		}
		if (type == typeof(StiGradientBrush))
		{
			return "GradientBrush";
		}
		if (type == typeof(StiGlareBrush))
		{
			return "GlareBrush";
		}
		if (type == typeof(StiGlassBrush))
		{
			return "GlassBrush";
		}
		if (type == typeof(StiHatchBrush))
		{
			return "HatchBrush";
		}
		if (type == typeof(HatchStyle))
		{
			return "HatchStyle";
		}
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return type.ToString();
	}

	internal StiFunction(string category, string functionName, Type typeOfFunction, Type returnType)
		: this(category, functionName, string.Empty, typeOfFunction, returnType, string.Empty, null, null)
	{
	}

	internal StiFunction(string category, string functionName, string description, Type typeOfFunction, Type returnType, string returnDescription)
		: this(category, functionName, description, typeOfFunction, returnType, returnDescription, null, null)
	{
	}

	internal StiFunction(string category, string functionName, string description, Type typeOfFunction, Type returnType, string returnDescription, Type[] argumentTypes, string[] argumentNames)
		: this(category, functionName, functionName, description, typeOfFunction, returnType, returnDescription, argumentTypes, argumentNames, null)
	{
	}

	internal StiFunction(string category, string groupFunctionName, string functionName, string description, Type typeOfFunction, Type returnType, string returnDescription, Type[] argumentTypes, string[] argumentNames, string[] argumentDescriptions)
	{
		Category = category;
		Description = description;
		ReturnDescription = returnDescription;
		GroupFunctionName = groupFunctionName;
		FunctionName = functionName;
		TypeOfFunction = typeOfFunction;
		ReturnType = returnType;
		ArgumentTypes = argumentTypes;
		ArgumentNames = argumentNames;
		ArgumentDescriptions = argumentDescriptions;
		if (argumentNames != null && argumentTypes != null && argumentNames.Length != argumentTypes.Length)
		{
			throw new ArgumentException("Length of array 'argumentNames' must be equal to length of array 'argumentTypes'!");
		}
	}
}
