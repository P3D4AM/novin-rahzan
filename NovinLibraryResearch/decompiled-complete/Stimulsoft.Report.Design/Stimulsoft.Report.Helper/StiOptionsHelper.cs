using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Helper;

public static class StiOptionsHelper
{
	public static void LoadOptions(string fileName)
	{
		using FileStream stream = File.OpenRead(fileName);
		LoadOptions(stream);
	}

	public static void LoadOptions(Stream stream)
	{
		if (stream == null || stream.Length == 0L)
		{
			return;
		}
		CultureInfo currentCulture = Application.CurrentCulture;
		try
		{
			Application.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			XmlTextReader xmlTextReader = new XmlTextReader(stream);
			xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
			xmlTextReader.Read();
			xmlTextReader.Read();
			xmlTextReader.Read();
			if (xmlTextReader.Name == "StiOptionsHelper" || xmlTextReader.Name == "StiOptions")
			{
				string propertyStr = null;
				string classTypeStr = null;
				bool flag = false;
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.IsStartElement())
					{
						if (xmlTextReader.Depth == 1)
						{
							if (flag)
							{
								ApplyNewValue(propertyStr, classTypeStr, null);
							}
							propertyStr = xmlTextReader.Name;
							classTypeStr = xmlTextReader.GetAttribute("type");
							flag = true;
						}
						else
						{
							string valueStr = xmlTextReader.ReadString();
							ApplyNewValue(propertyStr, classTypeStr, valueStr);
							flag = false;
						}
					}
					xmlTextReader.Read();
				}
			}
			xmlTextReader.Close();
			xmlTextReader = null;
		}
		finally
		{
			Application.CurrentCulture = currentCulture;
		}
	}

	private static void ApplyNewValue(string propertyStr, string classTypeStr, string valueStr)
	{
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		Type type = StiTypeFinder.GetType(classTypeStr + ", Stimulsoft.Report, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type == null)
		{
			return;
		}
		PropertyInfo property = type.GetProperty(propertyStr);
		FieldInfo field = type.GetField(propertyStr);
		if (property == null && field == null)
		{
			return;
		}
		object value = null;
		if (valueStr != null)
		{
			Type type2 = ((property != null) ? property.PropertyType : field.FieldType);
			if (type2.IsEnum)
			{
				value = Enum.Parse(type2, valueStr);
			}
			else if (type2 == typeof(string))
			{
				value = valueStr;
			}
			else if (type2 == typeof(double))
			{
				value = double.Parse(valueStr);
			}
			else if (type2 == typeof(bool))
			{
				value = bool.Parse(valueStr);
			}
			else if (type2 == typeof(int))
			{
				value = int.Parse(valueStr);
			}
			else if (type2 == typeof(float))
			{
				value = float.Parse(valueStr);
			}
			else if (type2 == typeof(char))
			{
				value = char.Parse(valueStr);
			}
			else if (type2 == typeof(Font))
			{
				string[] array = valueStr.Split(new char[1] { ',' });
				if (array.Length == 6)
				{
					float num = float.Parse(array[1]);
					FontStyle val = (FontStyle)Enum.Parse(typeof(FontStyle), array[2]);
					GraphicsUnit val2 = (GraphicsUnit)Enum.Parse(typeof(GraphicsUnit), array[3]);
					byte b = byte.Parse(array[4]);
					bool flag = bool.Parse(array[5]);
					value = (object)new Font(array[0], num, val, val2, b, flag);
				}
			}
			else if (type2 == typeof(StiLevel?))
			{
				value = Enum.Parse(typeof(StiLevel), valueStr);
			}
			else if (type2 == typeof(Size))
			{
				string[] array2 = valueStr.Split(new char[1] { ',' });
				value = new Size(int.Parse(array2[0]), int.Parse(array2[1]));
			}
		}
		if (property != null)
		{
			property.SetValue(null, value, null);
		}
		if (field != null)
		{
			field.SetValue(null, value);
		}
	}
}
