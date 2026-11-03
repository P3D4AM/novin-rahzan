using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiFunctionsXmlParser
{
	public static bool TryLoadDefaultFunctions()
	{
		try
		{
			LoadDefaultFunctions();
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static void LoadDefaultFunctions()
	{
		LoadFunctionsFromXml(typeof(StiReport).Assembly, new string[7] { "Stimulsoft.Report.Dictionary.Functions.FunctionsMath.xml", "Stimulsoft.Report.Dictionary.Functions.FunctionsDate.xml", "Stimulsoft.Report.Dictionary.Functions.FunctionsDrawing.xml", "Stimulsoft.Report.Dictionary.Functions.FunctionsProgramming.xml", "Stimulsoft.Report.Dictionary.Functions.FunctionsPrintState.xml", "Stimulsoft.Report.Dictionary.Functions.FunctionsStrings.xml", "Stimulsoft.Report.Dictionary.Functions.FunctionsTotals.xml" });
	}

	public static void LoadFunctionsFromXml(Assembly assembly, string[] resourceNames)
	{
		foreach (string resourceName in resourceNames)
		{
			LoadFunctionsFromXml(assembly, resourceName);
		}
	}

	public static void LoadFunctionsFromXml(Assembly assembly, string resourceName)
	{
		using Stream stream = assembly.GetManifestResourceStream(resourceName);
		if (stream != null)
		{
			LoadFunctionsFromXml(stream);
			stream.Close();
			return;
		}
		throw new Exception("Can't find xml file with function descriptions '" + resourceName + "' in resources");
	}

	public static void LoadFunctionsFromXml(string fileName)
	{
		using FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
		LoadFunctionsFromXml(fileStream);
		fileStream.Close();
	}

	public static void LoadFunctionsFromXml(Stream stream)
	{
		XmlTextReader xmlTextReader = new XmlTextReader(stream);
		xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
		string category = string.Empty;
		string groupFunctionName = string.Empty;
		string text = string.Empty;
		string description = string.Empty;
		Type type = null;
		Type returnType = null;
		string returnDescription = string.Empty;
		List<Type> list = new List<Type>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		xmlTextReader.ReadStartElement();
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element)
			{
				switch (xmlTextReader.Name.ToLower(CultureInfo.InvariantCulture))
				{
				case "category":
					category = xmlTextReader.GetAttribute(0);
					break;
				case "group":
					groupFunctionName = xmlTextReader.ReadString();
					break;
				case "name":
					text = xmlTextReader.ReadString();
					break;
				case "description":
					description = xmlTextReader.ReadString();
					break;
				case "typeoffunction":
				{
					string text2 = xmlTextReader.ReadString();
					if (text2.ToLower(CultureInfo.InvariantCulture) == "isnull")
					{
						type = null;
						break;
					}
					type = StiTypeFinder.GetType(text2);
					if (!(type == null))
					{
						break;
					}
					throw new Exception("Function Xml Parser: " + text + ", Type " + text2 + " is not founded!");
				}
				case "returntype":
					returnType = StiTypeFinder.GetType(xmlTextReader.ReadString());
					break;
				case "returndescription":
					returnDescription = xmlTextReader.ReadString();
					break;
				case "argumenttype":
					list.Add(StiTypeFinder.GetType(xmlTextReader.ReadString()));
					break;
				case "argumentname":
					list2.Add(xmlTextReader.ReadString());
					break;
				case "argumentdescription":
					list3.Add(xmlTextReader.ReadString());
					break;
				}
			}
			else if (xmlTextReader.NodeType == XmlNodeType.EndElement && xmlTextReader.Name.ToLower(CultureInfo.InvariantCulture) == "function")
			{
				Type[] argumentTypes = list.ToArray();
				string[] argumentNames = list2.ToArray();
				string[] argumentDescriptions = list3.ToArray();
				if (type == null)
				{
					StiFunctions.AddFunction(category, groupFunctionName, text, description, typeof(object), returnType, returnDescription, argumentTypes, argumentNames, argumentDescriptions).UseFullPath = false;
				}
				else
				{
					StiFunctions.AddFunction(category, groupFunctionName, text, description, type, returnType, returnDescription, argumentTypes, argumentNames, argumentDescriptions);
				}
				groupFunctionName = string.Empty;
				text = string.Empty;
				description = string.Empty;
				type = null;
				returnType = null;
				returnDescription = string.Empty;
				list.Clear();
				list2.Clear();
				list3.Clear();
			}
		}
		xmlTextReader.Close();
	}
}
