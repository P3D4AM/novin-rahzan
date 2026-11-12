using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ElementSerializer
{
	private static string XmlImageName = "image";

	private static string XmlIconName = "icon";

	private static string XmlAttributeName = "name";

	public static void Serialize(object element, XmlElement xmlElem)
	{
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Expected O, but got Unknown
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Expected O, but got Unknown
		PropertyInfo[] properties = element.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
		PropertyInfo[] array = properties;
		foreach (PropertyInfo propertyInfo in array)
		{
			if (propertyInfo.GetCustomAttributes(typeof(DevCoSerialize), inherit: true).Length == 0)
			{
				continue;
			}
			object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute), inherit: true);
			if (customAttributes.Length != 0)
			{
				DefaultValueAttribute defaultValueAttribute = customAttributes[0] as DefaultValueAttribute;
				object value = propertyInfo.GetValue(element, null);
				if (value == defaultValueAttribute.Value || (value != null && defaultValueAttribute.Value != null && value.Equals(defaultValueAttribute.Value)))
				{
					continue;
				}
			}
			else if (element.GetType().GetMethod("ShouldSerialize" + propertyInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null)
			{
				MethodInfo method = element.GetType().GetMethod("ShouldSerialize" + propertyInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				object obj = method.Invoke(element, null);
				if (obj is bool && !(bool)obj)
				{
					continue;
				}
			}
			string text = XmlConvert.EncodeName(propertyInfo.Name);
			Type propertyType = propertyInfo.PropertyType;
			if (propertyType.IsPrimitive || propertyType == typeof(string) || propertyType.IsEnum)
			{
				if (propertyType == typeof(string))
				{
					xmlElem.SetAttribute(text, propertyInfo.GetValue(element, null).ToString());
					continue;
				}
				if (propertyType.IsEnum)
				{
					xmlElem.SetAttribute(text, Enum.Format(propertyType, propertyInfo.GetValue(element, null), "g"));
					continue;
				}
				if (propertyType == typeof(bool))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((bool)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(int))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((int)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(double))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((double)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(float))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((float)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(short))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((short)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(long))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((long)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(byte))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((byte)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(char))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((char)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(decimal))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((decimal)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(Guid))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((Guid)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(sbyte))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((sbyte)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(TimeSpan))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((TimeSpan)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(ushort))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((ushort)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (propertyType == typeof(uint))
				{
					xmlElem.SetAttribute(text, XmlConvert.ToString((uint)propertyInfo.GetValue(element, null)));
					continue;
				}
				if (!(propertyType == typeof(ulong)))
				{
					throw new ApplicationException("Unsupported serialization type '" + propertyType.ToString() + "' on property '" + text + "'");
				}
				xmlElem.SetAttribute(text, XmlConvert.ToString((ulong)propertyInfo.GetValue(element, null)));
			}
			else if (propertyType == typeof(Color))
			{
				xmlElem.SetAttribute(text, ColorToString((Color)propertyInfo.GetValue(element, null)));
			}
			else if (propertyType == typeof(Image))
			{
				XmlElement xmlElement = xmlElem.OwnerDocument.CreateElement(XmlImageName);
				xmlElement.SetAttribute(XmlAttributeName, text);
				xmlElem.AppendChild(xmlElement);
				SerializeImage((Image)propertyInfo.GetValue(element, null), xmlElement);
			}
			else if (propertyType == typeof(Icon))
			{
				XmlElement xmlElement2 = xmlElem.OwnerDocument.CreateElement(XmlIconName);
				xmlElement2.SetAttribute(XmlAttributeName, text);
				xmlElem.AppendChild(xmlElement2);
				SerializeIcon((Icon)propertyInfo.GetValue(element, null), xmlElement2);
			}
			else
			{
				if (!(propertyType == typeof(Font)))
				{
					throw new ApplicationException("Unsupported serialization type '" + propertyType.ToString() + "'' on property '" + text + "'");
				}
				xmlElem.SetAttribute(text, FontToString((Font)propertyInfo.GetValue(element, null)));
			}
		}
	}

	private static string ColorToString(Color clr)
	{
		if (clr.IsSystemColor || clr.IsNamedColor || clr.IsKnownColor)
		{
			return "." + clr.Name;
		}
		return clr.ToArgb().ToString();
	}

	private static Color ColorFromString(string sclr)
	{
		if (sclr == "")
		{
			return Color.Empty;
		}
		if (sclr[0] == '.')
		{
			return Color.FromName(sclr.Substring(1));
		}
		return Color.FromArgb(XmlConvert.ToInt32(sclr));
	}

	private static string FontToString(Font font)
	{
		if (font == null)
		{
			return "";
		}
		return font.Name + "," + XmlConvert.ToString(font.Size) + "," + XmlConvert.ToString(font.Bold) + "," + XmlConvert.ToString(font.Italic) + "," + XmlConvert.ToString(font.Underline) + "," + XmlConvert.ToString(font.Strikeout);
	}

	private static Font FontFromString(string fontString)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		if (fontString == "")
		{
			return null;
		}
		string[] array = fontString.Split(new char[1] { ',' });
		FontStyle val = (FontStyle)0;
		if (XmlConvert.ToBoolean(array[2]))
		{
			val = (FontStyle)(val | 1);
		}
		if (XmlConvert.ToBoolean(array[3]))
		{
			val = (FontStyle)(val | 2);
		}
		if (XmlConvert.ToBoolean(array[4]))
		{
			val = (FontStyle)(val | 4);
		}
		if (XmlConvert.ToBoolean(array[5]))
		{
			val = (FontStyle)(val | 8);
		}
		return new Font(array[0], XmlConvert.ToSingle(array[1]), val);
	}

	public static void Deserialize(object element, XmlElement xmlElem)
	{
		Type type = element.GetType();
		foreach (XmlAttribute attribute in xmlElem.Attributes)
		{
			string name = XmlConvert.DecodeName(attribute.Name);
			PropertyInfo property = type.GetProperty(name);
			if (!(property != null))
			{
				continue;
			}
			Type propertyType = property.PropertyType;
			if (propertyType.IsPrimitive)
			{
				if (propertyType == typeof(bool))
				{
					property.SetValue(element, XmlConvert.ToBoolean(attribute.Value), null);
				}
				else if (propertyType == typeof(int))
				{
					property.SetValue(element, XmlConvert.ToInt32(attribute.Value), null);
				}
				else if (propertyType == typeof(double))
				{
					property.SetValue(element, XmlConvert.ToDouble(attribute.Value), null);
				}
				else if (propertyType == typeof(float))
				{
					property.SetValue(element, XmlConvert.ToSingle(attribute.Value), null);
				}
				else if (propertyType == typeof(short))
				{
					property.SetValue(element, XmlConvert.ToInt16(attribute.Value), null);
				}
				else if (propertyType == typeof(long))
				{
					property.SetValue(element, XmlConvert.ToInt64(attribute.Value), null);
				}
				else if (propertyType == typeof(byte))
				{
					property.SetValue(element, XmlConvert.ToByte(attribute.Value), null);
				}
				else if (propertyType == typeof(char))
				{
					property.SetValue(element, XmlConvert.ToChar(attribute.Value), null);
				}
				else if (propertyType == typeof(decimal))
				{
					property.SetValue(element, XmlConvert.ToDecimal(attribute.Value), null);
				}
				else if (propertyType == typeof(Guid))
				{
					property.SetValue(element, XmlConvert.ToGuid(attribute.Value), null);
				}
				else if (propertyType == typeof(sbyte))
				{
					property.SetValue(element, XmlConvert.ToSByte(attribute.Value), null);
				}
				else if (propertyType == typeof(TimeSpan))
				{
					property.SetValue(element, XmlConvert.ToTimeSpan(attribute.Value), null);
				}
				else if (propertyType == typeof(ushort))
				{
					property.SetValue(element, XmlConvert.ToUInt16(attribute.Value), null);
				}
				else if (propertyType == typeof(uint))
				{
					property.SetValue(element, XmlConvert.ToUInt32(attribute.Value), null);
				}
				else if (propertyType == typeof(ulong))
				{
					property.SetValue(element, XmlConvert.ToUInt64(attribute.Value), null);
				}
			}
			else if (propertyType == typeof(string))
			{
				property.SetValue(element, attribute.Value, null);
			}
			else if (propertyType.IsEnum)
			{
				property.SetValue(element, Enum.Parse(propertyType, attribute.Value), null);
			}
			else if (propertyType == typeof(Color))
			{
				property.SetValue(element, ColorFromString(attribute.Value), null);
			}
			else if (propertyType == typeof(Font))
			{
				property.SetValue(element, FontFromString(attribute.Value), null);
			}
		}
		foreach (XmlElement childNode in xmlElem.ChildNodes)
		{
			if (childNode.Name == XmlImageName)
			{
				string name2 = XmlConvert.DecodeName(childNode.GetAttribute(XmlAttributeName));
				PropertyInfo property2 = type.GetProperty(name2);
				if (property2 != null)
				{
					property2.SetValue(element, DeserializeImage(childNode), null);
				}
			}
			else if (childNode.Name == XmlIconName)
			{
				string name3 = XmlConvert.DecodeName(childNode.GetAttribute(XmlAttributeName));
				PropertyInfo property3 = type.GetProperty(name3);
				if (property3 != null)
				{
					property3.SetValue(element, DeserializeIcon(childNode), null);
				}
			}
		}
	}

	public static void SerializeImage(Image image, XmlElement xml)
	{
		if (image != null)
		{
			MemoryStream memoryStream = new MemoryStream(1024);
			image.Save((Stream)memoryStream, ImageFormat.Png);
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter w = new StringWriter(stringBuilder);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
			xmlTextWriter.WriteBase64(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			xml.InnerText = stringBuilder.ToString();
		}
	}

	public static void SerializeIcon(Icon icon, XmlElement xml)
	{
		if (icon != null)
		{
			MemoryStream memoryStream = new MemoryStream(1024);
			icon.Save((Stream)memoryStream);
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter w = new StringWriter(stringBuilder);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
			xml.SetAttribute("encoding", "binhex");
			xmlTextWriter.WriteBinHex(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			xml.InnerText = stringBuilder.ToString();
		}
	}

	public static Image DeserializeImage(XmlElement xml)
	{
		Image val = null;
		if (xml == null || xml.InnerText == "")
		{
			return null;
		}
		StringReader input = new StringReader(xml.OuterXml);
		XmlTextReader xmlTextReader = new XmlTextReader(input);
		xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
		MemoryStream memoryStream = new MemoryStream(1024);
		xmlTextReader.Read();
		byte[] array = new byte[1024];
		int num = 0;
		do
		{
			num = xmlTextReader.ReadBase64(array, 0, 1024);
			if (num > 0)
			{
				memoryStream.Write(array, 0, num);
			}
		}
		while (num != 0);
		return Image.FromStream((Stream)memoryStream);
	}

	public static Icon DeserializeIcon(XmlElement xml)
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		Icon val = null;
		if (xml == null || xml.InnerText == "")
		{
			return null;
		}
		bool flag = false;
		if (xml.HasAttribute("encoding") && xml.GetAttribute("encoding") == "binhex")
		{
			flag = true;
		}
		StringReader input = new StringReader(xml.OuterXml);
		XmlTextReader xmlTextReader = new XmlTextReader(input);
		xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
		MemoryStream memoryStream = new MemoryStream(1024);
		xmlTextReader.Read();
		byte[] array = new byte[1024];
		int num = 0;
		if (flag)
		{
			do
			{
				num = xmlTextReader.ReadBinHex(array, 0, 1024);
				if (num > 0)
				{
					memoryStream.Write(array, 0, num);
				}
			}
			while (num != 0);
		}
		else
		{
			do
			{
				num = xmlTextReader.ReadBase64(array, 0, 1024);
				if (num > 0)
				{
					memoryStream.Write(array, 0, num);
				}
			}
			while (num != 0);
		}
		memoryStream.Position = 0L;
		return new Icon((Stream)memoryStream);
	}
}
