using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Serializing;

public class StiSerializing
{
	private StiObjectStringConverter converter;

	private Hashtable contentHashtable = new Hashtable();

	public StiGraphs Graphs;

	private int ItemIndex = 1;

	private Hashtable propertyNameHashtable = new Hashtable();

	public StiReferenceCollection References;

	private static Hashtable propertyToString = new Hashtable();

	private static Hashtable propertyToString2 = new Hashtable();

	private static Hashtable stringToProperty = new Hashtable();

	private static Hashtable stringToProperty2 = new Hashtable();

	public static bool AllowFixOldChartTitle { get; set; }

	public bool SortProperties { get; set; } = true;


	public bool CheckSerializable { get; set; }

	public bool IgnoreSerializableForContainer { get; set; }

	public static Hashtable TypeToString { get; set; } = new Hashtable();


	public static Hashtable StringToType { get; set; } = new Hashtable();


	public static Hashtable SourceTypeToDestinationType { get; set; } = new Hashtable();


	public Hashtable PropertyToString
	{
		get
		{
			if (IsDocument)
			{
				return propertyToString2;
			}
			return propertyToString;
		}
		set
		{
			if (IsDocument)
			{
				propertyToString2 = value;
			}
			else
			{
				propertyToString = value;
			}
		}
	}

	public Hashtable StringToProperty
	{
		get
		{
			if (IsDocument)
			{
				return stringToProperty2;
			}
			return stringToProperty;
		}
		set
		{
			if (IsDocument)
			{
				stringToProperty2 = value;
			}
			else
			{
				stringToProperty = value;
			}
		}
	}

	public bool IsDocument { get; set; }

	public static event StiTypeNotFoundEventHandler TypeNotFound;

	public static event StiPropertyNotFoundEventHandlers PropertyNotFound;

	public static event EventHandler GlobalSerializing;

	public static event EventHandler GlobalDeserializing;

	public event EventHandler Serializing;

	public event EventHandler Deserializing;

	public static void AddSourceTypeToDestinationType(string typeDestination, string typeSource)
	{
		SourceTypeToDestinationType[typeSource] = typeDestination;
	}

	private static string ConvertSourceTypeToDestinationType(string sourceType)
	{
		if (SourceTypeToDestinationType[sourceType] is string result)
		{
			return result;
		}
		return sourceType;
	}

	public static void AddStringType(Type type, string str)
	{
		StringToType[str] = type;
		TypeToString[type] = str;
	}

	public void AddStringProperty(string property, string str)
	{
		StringToProperty[str] = property;
		PropertyToString[property] = str;
	}

	private Type GetType(string typeStr)
	{
		return StringToType[ConvertSourceTypeToDestinationType(typeStr)] as Type;
	}

	private object GetObjectFromType(string typeStr)
	{
		Type type = GetType(typeStr);
		object obj = ((type == null) ? StiActivator.CreateObject(ConvertSourceTypeToDestinationType(typeStr)) : StiActivator.CreateObject(type));
		if (obj == null && typeStr == "Stimulsoft.Dashboard.Components.StiDashboard")
		{
			try
			{
				Assembly assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Dashboard");
				if (assembly != null)
				{
					obj = StiActivator.CreateObject(ConvertSourceTypeToDestinationType("Stimulsoft.Dashboard.Components.StiDashboard"));
					if (obj != null)
					{
						return obj;
					}
				}
			}
			catch
			{
				throw new StiDashboardAssemblyIsNotFoundException();
			}
			throw new StiDashboardAssemblyIsNotFoundException();
		}
		return obj;
	}

	public static void ClearStringType()
	{
		StringToType.Clear();
		TypeToString.Clear();
	}

	public void ClearPropertyString()
	{
		StringToProperty.Clear();
		PropertyToString.Clear();
	}

	public string GetStringFromProperty(string propertyName)
	{
		string text = PropertyToString[propertyName] as string;
		return text ?? propertyName;
	}

	public string GetPropertyFromString(string str)
	{
		string text = StringToProperty[str] as string;
		return text ?? str;
	}

	public void SetReferenceSerializing()
	{
		foreach (StiReference reference in References)
		{
			if (reference.PropInfo.Value != null)
			{
				int num = Graphs[reference.PropInfo.Value];
				if (num == -1)
				{
					reference.PropInfo.IsReference = false;
					reference.PropInfo.Value = null;
				}
				else
				{
					reference.PropInfo.ReferenceCode = num;
				}
			}
		}
	}

	public void SetReferenceDeserializing()
	{
		int num = 0;
		foreach (StiReference reference in References)
		{
			object obj = Graphs[reference.PropInfo.ReferenceCode];
			if (obj != null)
			{
				SetProperty(reference.PropertyInfo, reference.Object, obj);
			}
			num++;
		}
	}

	public static Type GetTypeOfArrayElement(object array)
	{
		if (array != null)
		{
			Type type = array.GetType();
			if (type.GetElementType() != null)
			{
				return type.GetElementType();
			}
			MethodInfo[] methods = type.GetMethods();
			MethodInfo[] array2 = methods;
			foreach (MethodInfo methodInfo in array2)
			{
				if (methodInfo.Name == "get_Item")
				{
					return methodInfo.ReturnType;
				}
			}
			string fullName = array.GetType().FullName;
			return StiTypeFinder.GetType(fullName.Substring(0, fullName.Length - 2));
		}
		return typeof(object);
	}

	public object GetDefaultValue(MemberDescriptor prop)
	{
		if (prop.Attributes[typeof(DefaultValueAttribute)] is DefaultValueAttribute defaultValueAttribute)
		{
			return defaultValueAttribute.Value;
		}
		if (prop.Attributes[typeof(StiDefaultStringValueAttribute)] is StiDefaultStringValueAttribute stiDefaultStringValueAttribute)
		{
			return stiDefaultStringValueAttribute.Value;
		}
		return null;
	}

	public bool IsContent(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		Type type = obj.GetType();
		object obj2 = contentHashtable[type];
		if (obj2 != null)
		{
			return (bool)obj2;
		}
		object[] customAttributes = type.GetCustomAttributes(typeof(StiSerializableAttribute), inherit: true);
		if (customAttributes.Length != 0 && ((StiSerializableAttribute)customAttributes[0]).Visibility == StiSerializationVisibility.Content)
		{
			contentHashtable.Add(type, true);
			return true;
		}
		contentHashtable.Add(type, false);
		return false;
	}

	private int SerializeList(StiPropertyInfoCollection props, object list, StiSerializeTypes serializeType)
	{
		if (list == null)
		{
			return 0;
		}
		int num = 0;
		foreach (object item in (IList)list)
		{
			if (item == null)
			{
				continue;
			}
			InvokeSerializing();
			if (item.GetType().IsPrimitive || item is string || IsContent(item))
			{
				props.Add(new StiPropertyInfo("value", item, null, isDefaultValueSpecified: false, isKey: false, isReference: false, isList: false, null));
			}
			else
			{
				int num2 = Graphs[item];
				if (num2 == -1)
				{
					Graphs.Add(item);
					StiPropertyInfo stiPropertyInfo;
					if (CheckSerializable && item is IStiSerializable && !(item is IStiNonSerialized) && (!IgnoreSerializableForContainer || (IgnoreSerializableForContainer && item.GetType().ToString().IndexOf("Container", StringComparison.InvariantCulture) == -1)))
					{
						stiPropertyInfo = new StiPropertyInfo($"item{num}", item, null, isDefaultValueSpecified: false, isKey: true, isReference: false, isList: false, item.GetType().ToString());
						stiPropertyInfo.IsSerializable = true;
					}
					else
					{
						if (item is IStiSerializableCustomControl && ((IStiSerializableCustomControl)item).Control != null)
						{
							object control = ((IStiSerializableCustomControl)item).Control;
							Control val = (Control)((control is Control) ? control : null);
							Graphs.Add(val);
							string name = $"Item{ItemIndex++}";
							PropertyInfo property = ((object)val).GetType().GetProperty("Name");
							if (property != null)
							{
								name = (string)property.GetValue(val, null);
							}
							StiPropertyInfo stiPropertyInfo2 = new StiPropertyInfo(name, val, null, isDefaultValueSpecified: false, isKey: true, isReference: false, isList: false, ((object)val).GetType().ToString());
							stiPropertyInfo2.Properties.AddRange(SerializeControl(val, serializeType));
							stiPropertyInfo2.ReferenceCode = Graphs[val];
							props.Add(stiPropertyInfo2);
						}
						string name2 = $"Item{ItemIndex++}";
						PropertyInfo property2 = item.GetType().GetProperty("Name");
						if (property2 != null)
						{
							name2 = (string)property2.GetValue(item, null);
						}
						stiPropertyInfo = new StiPropertyInfo(name2, item, null, isDefaultValueSpecified: false, isKey: true, isReference: false, isList: false, item.GetType().ToString());
						stiPropertyInfo.Properties.AddRange(SerializeObject(item, serializeType));
					}
					stiPropertyInfo.ReferenceCode = Graphs[item];
					props.Add(stiPropertyInfo);
				}
				else
				{
					string name3 = $"Item{ItemIndex++}";
					StiPropertyInfo stiPropertyInfo3 = new StiPropertyInfo(name3, item, null, isDefaultValueSpecified: false, isKey: true, isReference: true, isList: false, item.GetType().ToString());
					stiPropertyInfo3.ReferenceCode = num2;
					props.Add(stiPropertyInfo3);
				}
			}
			num++;
		}
		return num;
	}

	public StiPropertyInfoCollection SerializeControl(Control obj, StiSerializeTypes serializeType)
	{
		StiPropertyInfoCollection stiPropertyInfoCollection = new StiPropertyInfoCollection();
		Attribute[] attributes = new Attribute[2]
		{
			new SerializableAttribute(),
			new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)
		};
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj, attributes);
		foreach (PropertyDescriptor item in properties)
		{
			if (item.ShouldSerializeValue(obj))
			{
				object defaultValue = GetDefaultValue(item);
				bool isDefaultValueSpecified = item.Attributes[typeof(DefaultValueAttribute)] != null;
				StiPropertyInfo propertyInfo = new StiPropertyInfo(item.Name, item.GetValue(obj), defaultValue, isDefaultValueSpecified, isKey: false, isReference: false, isList: false, item.PropertyType.ToString());
				stiPropertyInfoCollection.Add(propertyInfo);
			}
		}
		return stiPropertyInfoCollection;
	}

	public StiPropertyInfoCollection SerializeObject(object obj, StiSerializeTypes serializeType)
	{
		if (IsContent(obj))
		{
			return new StiPropertyInfoCollection
			{
				new StiPropertyInfo(string.Empty, obj, null, isDefaultValueSpecified: false, isKey: false, isReference: false, isList: false, null)
			};
		}
		if (obj == null)
		{
			return null;
		}
		if (Graphs[obj] == -1)
		{
			Graphs.Add(obj);
		}
		StiPropertyInfoCollection stiPropertyInfoCollection = new StiPropertyInfoCollection();
		PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(obj);
		if (propertyDescriptorCollection.Count == 0)
		{
			return null;
		}
		if (SortProperties)
		{
			propertyDescriptorCollection = propertyDescriptorCollection.Sort();
		}
		int count = propertyDescriptorCollection.Count;
		for (int i = 0; i < count; i++)
		{
			InvokeSerializing();
			PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
			if (!(propertyDescriptor.Attributes[typeof(StiSerializableAttribute)] is StiSerializableAttribute stiSerializableAttribute))
			{
				continue;
			}
			bool flag = false;
			if ((serializeType & StiSerializeTypes.SerializeToCode) > (StiSerializeTypes)0 && (stiSerializableAttribute.SerializeType & StiSerializeTypes.SerializeToCode) > (StiSerializeTypes)0)
			{
				flag = true;
			}
			else if ((serializeType & StiSerializeTypes.SerializeToDesigner) > (StiSerializeTypes)0 && (stiSerializableAttribute.SerializeType & StiSerializeTypes.SerializeToDesigner) > (StiSerializeTypes)0)
			{
				flag = true;
			}
			else if ((serializeType & StiSerializeTypes.SerializeToSaveLoad) > (StiSerializeTypes)0 && (stiSerializableAttribute.SerializeType & StiSerializeTypes.SerializeToSaveLoad) > (StiSerializeTypes)0)
			{
				flag = true;
			}
			else if ((serializeType & StiSerializeTypes.SerializeToDocument) > (StiSerializeTypes)0 && (stiSerializableAttribute.SerializeType & StiSerializeTypes.SerializeToDocument) > (StiSerializeTypes)0)
			{
				flag = true;
			}
			if (!flag || stiSerializableAttribute.Visibility == StiSerializationVisibility.None || propertyDescriptor.Attributes[typeof(StiNonSerializedAttribute)] != null)
			{
				continue;
			}
			StiSerializationVisibility visibility = stiSerializableAttribute.Visibility;
			object defaultValue = GetDefaultValue(propertyDescriptor);
			bool isDefaultValueSpecified = propertyDescriptor.Attributes[typeof(DefaultValueAttribute)] != null || propertyDescriptor.Attributes[typeof(StiDefaultStringValueAttribute)] != null;
			object value = propertyDescriptor.GetValue(obj);
			StiPropertyInfo stiPropertyInfo = null;
			if (value == null)
			{
				stiPropertyInfo = new StiPropertyInfo(propertyDescriptor.Name, null, defaultValue, isDefaultValueSpecified, isKey: true, isReference: false, isList: false, propertyDescriptor.PropertyType.ToString());
			}
			else
			{
				switch (visibility)
				{
				case StiSerializationVisibility.Reference:
					stiPropertyInfo = new StiPropertyInfo(propertyDescriptor.Name, value, defaultValue, isDefaultValueSpecified, isKey: true, isReference: true, isList: false, propertyDescriptor.PropertyType.ToString());
					References.Add(stiPropertyInfo);
					break;
				case StiSerializationVisibility.Content:
				{
					bool flag3 = false;
					if ((serializeType & StiSerializeTypes.SerializeToCode) > (StiSerializeTypes)0)
					{
						if (value is StiStyleBrush && defaultValue as string == "StyleBrush")
						{
							flag3 = true;
						}
						else if (value is StiDefaultBrush && defaultValue as string == "DefaultBrush")
						{
							flag3 = true;
						}
					}
					if (!flag3 && !IsDefaultValue(value, null))
					{
						stiPropertyInfo = new StiPropertyInfo(propertyDescriptor.Name, value, defaultValue, isDefaultValueSpecified, isKey: false, isReference: false, isList: false, propertyDescriptor.PropertyType.ToString());
					}
					break;
				}
				case StiSerializationVisibility.List:
					if (value is IList)
					{
						stiPropertyInfo = new StiPropertyInfo(propertyDescriptor.Name, value, defaultValue, isDefaultValueSpecified, isKey: false, isReference: false, isList: true, propertyDescriptor.PropertyType.ToString());
						stiPropertyInfo.Count = SerializeList(stiPropertyInfo.Properties, value, serializeType);
						break;
					}
					throw new Exception($"{value} is not list");
				case StiSerializationVisibility.Class:
					if (value.GetType().IsClass)
					{
						bool flag2 = propertyDescriptor.Name == "Interaction" && serializeType == StiSerializeTypes.SerializeToCode;
						if (IsDefaultValue(value, propertyDescriptor) && !flag2)
						{
							break;
						}
						StiReferenceIgnoreAttribute[] array = (StiReferenceIgnoreAttribute[])propertyDescriptor.PropertyType.GetCustomAttributes(typeof(StiReferenceIgnoreAttribute), inherit: false);
						int num = Graphs[value];
						if (propertyDescriptor.Name == "TextFormat" && serializeType == StiSerializeTypes.SerializeToCode)
						{
							num = -1;
						}
						if (num == -1 || array.Length != 0)
						{
							Graphs.Add(value);
							int referenceCode2 = Graphs[value];
							if (CheckSerializable && value is IStiSerializable && !(value is IStiNonSerialized))
							{
								stiPropertyInfo = new StiPropertyInfo(propertyDescriptor.Name, value, defaultValue, isDefaultValueSpecified, isKey: true, isReference: false, isList: false, propertyDescriptor.PropertyType.ToString());
								stiPropertyInfo.IsSerializable = true;
							}
							else
							{
								stiPropertyInfo = new StiPropertyInfo(propertyDescriptor.Name, value, defaultValue, isDefaultValueSpecified, isKey: true, isReference: false, isList: false, propertyDescriptor.PropertyType.ToString());
								StiPropertyInfoCollection stiPropertyInfoCollection2 = SerializeObject(value, serializeType);
								if (stiPropertyInfoCollection2 != null)
								{
									stiPropertyInfo.Properties.AddRange(stiPropertyInfoCollection2);
								}
							}
							stiPropertyInfo.ReferenceCode = referenceCode2;
						}
						else
						{
							stiPropertyInfo = new StiPropertyInfo(propertyDescriptor.Name, value, defaultValue, isDefaultValueSpecified, isKey: true, isReference: true, isList: false, propertyDescriptor.PropertyType.ToString());
							stiPropertyInfo.ReferenceCode = num;
						}
						break;
					}
					throw new Exception($"{value} is not class");
				case StiSerializationVisibility.Control:
				{
					int referenceCode = Graphs[value];
					stiPropertyInfo = new StiPropertyInfo(propertyDescriptor.Name, value, defaultValue, isDefaultValueSpecified, isKey: true, isReference: true, isList: false, propertyDescriptor.PropertyType.ToString());
					stiPropertyInfo.ReferenceCode = referenceCode;
					break;
				}
				}
			}
			if (stiPropertyInfo != null)
			{
				stiPropertyInfoCollection.Add(stiPropertyInfo);
			}
		}
		return stiPropertyInfoCollection;
	}

	private static bool IsDefaultValue(object valueObject, PropertyDescriptor prop)
	{
		if (!(valueObject is IStiDefault stiDefault))
		{
			return false;
		}
		if (prop != null && prop.Name == "ValueFormat" && prop.PropertyType != null && prop.PropertyType.Name == "StiFormatService")
		{
			return false;
		}
		return stiDefault.IsDefault;
	}

	private void SerializeProperty(XmlTextWriter tw, StiPropertyInfo prop)
	{
		string text = propertyNameHashtable[prop.Name] as string;
		if (text == null)
		{
			text = XmlConvert.EncodeName(GetStringFromProperty(prop.Name));
			StringBuilder stringBuilder = new StringBuilder();
			string text2 = text;
			foreach (char c in text2)
			{
				if (char.IsPunctuation(c))
				{
					stringBuilder.Append("_");
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			text = stringBuilder.ToString();
			propertyNameHashtable[prop.Name] = text;
		}
		if (prop.Value == null)
		{
			if (prop.IsDefaultValueSpecified)
			{
				tw.WriteStartElement(text);
				tw.WriteAttributeString("isNull", "true");
				tw.WriteEndElement();
			}
		}
		else if (prop.IsKey)
		{
			if (prop.IsReference)
			{
				tw.WriteStartElement(text);
				tw.WriteAttributeString("isRef", prop.ReferenceCode.ToString());
				tw.WriteEndElement();
				return;
			}
			tw.WriteStartElement(text);
			if (prop.ReferenceCode != -1)
			{
				tw.WriteAttributeString("Ref", prop.ReferenceCode.ToString());
			}
			if (prop.Value != null)
			{
				string text3 = TypeToString[prop.Value.GetType()] as string;
				if (text3 == null)
				{
					text3 = prop.Value.GetType().FullName;
				}
				tw.WriteAttributeString("type", text3);
			}
			if (prop.IsSerializable)
			{
				tw.WriteAttributeString("isSer", "true");
				IStiSerializable stiSerializable = prop.Value as IStiSerializable;
				stiSerializable.Serialize(converter, tw);
				Graphs.Add(stiSerializable, prop.ReferenceCode);
			}
			else
			{
				tw.WriteAttributeString("isKey", "true");
				SerializeObject(tw, prop.Properties);
			}
			tw.WriteEndElement();
		}
		else if (prop.IsList)
		{
			tw.WriteStartElement(text);
			tw.WriteAttributeString("isList", "true");
			if (prop.Value != null)
			{
				tw.WriteAttributeString("count", prop.Count.ToString());
			}
			SerializeObject(tw, prop.Properties);
			tw.WriteEndElement();
		}
		else if (prop.Value is Metafile)
		{
			tw.WriteStartElement(text);
			object value = prop.Value;
			tw.WriteString(StiMetafileConverter.MetafileToString((Metafile)((value is Metafile) ? value : null)));
			tw.WriteEndElement();
		}
		else if (prop.Value is Image)
		{
			tw.WriteStartElement(text);
			object value2 = prop.Value;
			tw.WriteString(StiImageConverter.ImageToString((Image)((value2 is Image) ? value2 : null)));
			tw.WriteEndElement();
		}
		else
		{
			string text4 = converter.ObjectToString(prop.Value);
			if (text4 != null && (!(prop.DefaultValue is string text5) || (text5 != null && text4 != text5)))
			{
				tw.WriteStartElement(text);
				tw.WriteString(text4);
				tw.WriteEndElement();
			}
		}
	}

	public void SerializeObject(XmlTextWriter tw, StiPropertyInfoCollection props)
	{
		if (props == null)
		{
			return;
		}
		foreach (StiPropertyInfo prop in props)
		{
			InvokeSerializing();
			if (!prop.IsDefaultValueSpecified || !object.Equals(prop.DefaultValue, prop.Value))
			{
				SerializeProperty(tw, prop);
			}
		}
	}

	public StiPropertyInfoCollection Serialize(object obj, StiSerializeTypes serializeType)
	{
		ItemIndex = 1;
		Graphs = new StiGraphs();
		References = new StiReferenceCollection();
		StiPropertyInfoCollection result = SerializeObject(obj, serializeType);
		SetReferenceSerializing();
		return result;
	}

	public void Serialize(object obj, Stream stream, string application)
	{
		Serialize(obj, stream, application, StiSerializeTypes.SerializeToSaveLoad);
	}

	public void Serialize(object obj, Stream stream, string application, StiSerializeTypes serializeType)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument(standalone: true);
			xmlTextWriter.WriteStartElement("StiSerializer");
			xmlTextWriter.WriteAttributeString("version", "1.02");
			xmlTextWriter.WriteAttributeString("type", "Net");
			xmlTextWriter.WriteAttributeString("application", application);
			Graphs = new StiGraphs();
			References = new StiReferenceCollection();
			StiPropertyInfoCollection props = SerializeObject(obj, serializeType);
			SetReferenceSerializing();
			SerializeObject(xmlTextWriter, props);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public void Serialize(object obj, TextWriter textWriter, string application, StiSerializeTypes serializeType)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(textWriter);
			xmlTextWriter.WriteStartDocument(standalone: true);
			xmlTextWriter.WriteStartElement("StiSerializer");
			xmlTextWriter.WriteAttributeString("version", "1.02");
			xmlTextWriter.WriteAttributeString("type", "Net");
			xmlTextWriter.WriteAttributeString("application", application);
			Graphs = new StiGraphs();
			References = new StiReferenceCollection();
			StiPropertyInfoCollection props = SerializeObject(obj, serializeType);
			SetReferenceSerializing();
			SerializeObject(xmlTextWriter, props);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public void Serialize(object obj, string path, string application)
	{
		StiFileUtils.ProcessReadOnly(path);
		FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		Serialize(obj, fileStream, application);
		fileStream.Flush();
		fileStream.Close();
	}

	public void SetProperty(PropertyInfo p, object obj, object value)
	{
		if (p != null && p.CanWrite)
		{
			p.SetValue(obj, value, null);
		}
	}

	private void DeserializeObject(object obj, StiPropertyInfoCollection props)
	{
		foreach (StiPropertyInfo prop in props)
		{
			InvokeDeserializing();
			PropertyInfo propertyInfo = null;
			if (obj != null)
			{
				Type type = obj.GetType();
				propertyInfo = ((prop.Name == "Border" && type.Name == "StiPanelElement") ? type.GetProperties().FirstOrDefault((PropertyInfo c) => c.Name == "Border" && c.PropertyType == typeof(StiSimpleBorder)) : ((!(prop.Name == "Border") || !(type.Name == "StiPanelUI")) ? type.GetProperty(prop.Name) : type.GetProperties().FirstOrDefault((PropertyInfo c) => c.Name == "Border" && c.PropertyType == typeof(StiSimpleBorder))));
			}
			if (propertyInfo == null && obj != null)
			{
				StiPropertyNotFoundEventArgs stiPropertyNotFoundEventArgs = new StiPropertyNotFoundEventArgs(prop.Name, obj.GetType());
				InvokePropertyNotFound(this, stiPropertyNotFoundEventArgs);
				if (stiPropertyNotFoundEventArgs.PropertyName != prop.Name)
				{
					prop.Name = stiPropertyNotFoundEventArgs.PropertyName;
					propertyInfo = obj.GetType().GetProperty(prop.Name);
				}
			}
			if (propertyInfo == null)
			{
				continue;
			}
			if (!prop.IsReference && prop.ReferenceCode != -1)
			{
				Graphs.Add(prop.Value, prop.ReferenceCode);
			}
			if (prop.IsKey)
			{
				SetProperty(propertyInfo, obj, prop.Value);
				if (propertyInfo != null)
				{
					DeserializeObject(propertyInfo.GetValue(obj, null), prop.Properties);
				}
				else
				{
					DeserializeObject(null, prop.Properties);
				}
				converter.SetProperty(propertyInfo, obj, prop.Value);
			}
			else if (prop.IsList)
			{
				int count = prop.Count;
				if (!(propertyInfo != null))
				{
					continue;
				}
				IList list = propertyInfo.GetValue(obj, null) as IList;
				if (list == null)
				{
					try
					{
						list = StiActivator.CreateObject(propertyInfo.PropertyType) as IList;
						propertyInfo.SetValue(obj, list);
					}
					catch
					{
					}
				}
				if (list == null)
				{
					continue;
				}
				Type typeOfArrayElement = GetTypeOfArrayElement(list);
				if (list is Array)
				{
					list = Array.CreateInstance(typeOfArrayElement, count);
					SetProperty(propertyInfo, obj, list);
				}
				else
				{
					list.Clear();
				}
				string str = list.GetType().ToString();
				int num = 0;
				foreach (StiPropertyInfo property in prop.Properties)
				{
					if (property.Value == null)
					{
						StiTypeNotFoundEventArgs stiTypeNotFoundEventArgs = new StiTypeNotFoundEventArgs(property.TypeName);
						InvokeTypeNotFound(this, stiTypeNotFoundEventArgs);
						if (stiTypeNotFoundEventArgs.CreatedObject != null)
						{
							property.Value = stiTypeNotFoundEventArgs.CreatedObject;
						}
					}
					if (property.Value == null)
					{
						if (str.EndsWithInvariant("StiComponentsCollection"))
						{
							property.Value = StiActivator.CreateObject("Stimulsoft.Report.Components.StiUndefinedComponent");
						}
						else if (str.EndsWithInvariant("StiPagesCollection"))
						{
							property.Value = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage");
						}
						else if (str.EndsWithInvariant("StiDatabaseCollection"))
						{
							property.Value = StiActivator.CreateObject("Stimulsoft.Report.Dictionary.StiUndefinedDatabase");
						}
						else if (str.EndsWithInvariant("StiDataSourcesCollection"))
						{
							property.Value = StiActivator.CreateObject("Stimulsoft.Report.Dictionary.StiUndefinedDataSource");
						}
					}
					InvokeDeserializing();
					object obj3;
					if (property.IsReference && property.ReferenceCode != -1)
					{
						property.Value = Graphs[property.ReferenceCode];
						obj3 = property.Value;
					}
					else if (typeOfArrayElement == typeof(string) || !(property.Value is string))
					{
						obj3 = property.Value;
					}
					else
					{
						try
						{
							obj3 = converter.StringToObject((string)property.Value, typeOfArrayElement);
						}
						catch
						{
							obj3 = (string)property.Value;
						}
					}
					if (!property.IsReference && property.ReferenceCode != -1 && property.Value != null)
					{
						Graphs.Add(property.Value, property.ReferenceCode);
					}
					if (list is Array)
					{
						object value = null;
						if (typeOfArrayElement == obj3.GetType())
						{
							value = obj3;
						}
						else
						{
							converter.StringToObject((string)obj3, typeOfArrayElement);
						}
						((Array)list).SetValue(value, num++);
					}
					else if (obj3 != null)
					{
						list.Add(obj3);
						if (!property.IsReference && !property.IsSerializable)
						{
							DeserializeObject(obj3, property.Properties);
						}
					}
				}
			}
			else if (prop.IsReference)
			{
				object obj5 = Graphs[prop.ReferenceCode];
				SetProperty(propertyInfo, obj, obj5);
				if (obj5 == null)
				{
					References.Add(prop, obj, propertyInfo);
				}
			}
			else if (prop.Value == null)
			{
				SetProperty(propertyInfo, obj, null);
			}
			else if (propertyInfo != null)
			{
				object value2 = prop.Value;
				if (prop.Value is string && propertyInfo.PropertyType == typeof(Color[]) && string.IsNullOrWhiteSpace(prop.Value as string))
				{
					value2 = new Color[0];
				}
				else if (prop.Value is string && propertyInfo.PropertyType != typeof(object))
				{
					value2 = converter.StringToObject((string)prop.Value, propertyInfo.PropertyType);
				}
				SetProperty(propertyInfo, obj, value2);
			}
		}
	}

	private StiPropertyInfoCollection DeserializeObject(XmlTextReader tr, string parentPropName)
	{
		StiPropertyInfoCollection stiPropertyInfoCollection = new StiPropertyInfoCollection();
		tr.Read();
		while (!tr.EOF)
		{
			if (tr.IsStartElement())
			{
				InvokeDeserializing();
				StiPropertyInfo stiPropertyInfo = new StiPropertyInfo(XmlConvert.DecodeName(GetPropertyFromString(tr.Name)), null, tr.GetAttribute("isKey") == "true", tr.MoveToAttribute("isRef"), tr.MoveToAttribute("isList"), null);
				if (tr.GetAttribute("isNull") == "true")
				{
					stiPropertyInfo.Value = null;
				}
				else if (tr.GetAttribute("isSer") != null)
				{
					string attribute = tr.GetAttribute("Ref");
					if (attribute != null)
					{
						stiPropertyInfo.ReferenceCode = int.Parse(attribute);
					}
					string typeStr = (stiPropertyInfo.TypeName = tr.GetAttribute("type"));
					stiPropertyInfo.Value = GetObjectFromType(typeStr);
					stiPropertyInfo.IsSerializable = true;
					IStiSerializable stiSerializable = stiPropertyInfo.Value as IStiSerializable;
					stiSerializable.Deserialize(converter, tr);
					Graphs.Add(stiSerializable, stiPropertyInfo.ReferenceCode);
				}
				else if (stiPropertyInfo.IsReference)
				{
					stiPropertyInfo.ReferenceCode = int.Parse(tr.GetAttribute("isRef"));
				}
				else if (stiPropertyInfo.IsList)
				{
					stiPropertyInfo.Count = int.Parse(tr.GetAttribute("count"));
					if (stiPropertyInfo.Count > 0 && !tr.IsEmptyElement)
					{
						stiPropertyInfo.Properties = DeserializeObject(tr, stiPropertyInfo.Name);
					}
				}
				else if (!stiPropertyInfo.IsKey)
				{
					if (tr.GetAttribute("isImage") == "true")
					{
						stiPropertyInfo.Value = StiImageConverter.StringToImage(tr.ReadString());
						stiPropertyInfo.TypeName = typeof(Image).ToString();
					}
					else if (tr.GetAttribute("isEmfImage") == "true")
					{
						stiPropertyInfo.Value = StiMetafileConverter.StringToMetafile(tr.ReadString());
						stiPropertyInfo.TypeName = typeof(Image).ToString();
					}
					else
					{
						stiPropertyInfo.Value = tr.ReadString();
					}
				}
				else if (stiPropertyInfo.IsKey)
				{
					string attribute3 = tr.GetAttribute("Ref");
					if (attribute3 != null)
					{
						stiPropertyInfo.ReferenceCode = int.Parse(attribute3);
					}
					string typeStr2 = (stiPropertyInfo.TypeName = tr.GetAttribute("type"));
					stiPropertyInfo.Value = GetObjectFromType(typeStr2);
					if (!tr.IsEmptyElement)
					{
						stiPropertyInfo.Properties = DeserializeObject(tr, stiPropertyInfo.Name);
					}
					if (AllowFixOldChartTitle && (parentPropName == "YAxis" || parentPropName == "YRightAxis") && stiPropertyInfo.Name == "Title")
					{
						bool flag = false;
						foreach (StiPropertyInfo property2 in stiPropertyInfo.Properties)
						{
							if (property2.Name == "Direction")
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							PropertyInfo property = stiPropertyInfo.Value.GetType().GetProperty("Direction");
							if (parentPropName == "YAxis")
							{
								property.SetValue(stiPropertyInfo.Value, 3, new object[0]);
							}
							if (parentPropName == "YRightAxis")
							{
								property.SetValue(stiPropertyInfo.Value, 2, new object[0]);
							}
						}
					}
				}
				stiPropertyInfoCollection.Add(stiPropertyInfo);
			}
			else if (tr.NodeType == XmlNodeType.EndElement)
			{
				break;
			}
			tr.Read();
		}
		return stiPropertyInfoCollection;
	}

	public void Deserialize(object obj, Stream stream, string application)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			XmlTextReader xmlTextReader = new XmlTextReader(stream);
			xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
			xmlTextReader.Read();
			xmlTextReader.Read();
			if (xmlTextReader.IsStartElement() && xmlTextReader.Name == "StiSerializer")
			{
				Graphs = new StiGraphs();
				References = new StiReferenceCollection();
				Graphs.Add(obj);
				string attribute = xmlTextReader.GetAttribute("application");
				if (attribute == application)
				{
					DeserializeObject(obj, DeserializeObject(xmlTextReader, null));
					SetReferenceDeserializing();
				}
			}
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public void Deserialize(object obj, TextReader textReader, string application)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			XmlTextReader xmlTextReader = new XmlTextReader(textReader);
			xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
			xmlTextReader.Read();
			xmlTextReader.Read();
			if (xmlTextReader.IsStartElement() && xmlTextReader.Name == "StiSerializer")
			{
				Graphs = new StiGraphs();
				References = new StiReferenceCollection();
				Graphs.Add(obj);
				string attribute = xmlTextReader.GetAttribute("application");
				if (attribute == application)
				{
					DeserializeObject(obj, DeserializeObject(xmlTextReader, null));
					SetReferenceDeserializing();
				}
			}
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public void Deserialize(object obj, string path, string application)
	{
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		Deserialize(obj, fileStream, application);
		fileStream.Close();
	}

	internal static void InvokeTypeNotFound(StiSerializing serializing, StiTypeNotFoundEventArgs e)
	{
		StiSerializing.TypeNotFound?.Invoke(serializing, e);
	}

	internal static void InvokePropertyNotFound(StiSerializing serializing, StiPropertyNotFoundEventArgs e)
	{
		StiSerializing.PropertyNotFound?.Invoke(serializing, e);
	}

	public static void InvokeGlobalSerializing(object sender, EventArgs e)
	{
		StiSerializing.GlobalSerializing?.Invoke(sender, e);
	}

	public static void InvokeGlobalDeserializing(object sender, EventArgs e)
	{
		StiSerializing.GlobalDeserializing?.Invoke(sender, e);
	}

	protected virtual void OnSerializing(EventArgs e)
	{
		InvokeGlobalSerializing(this, e);
	}

	public void InvokeSerializing()
	{
		OnSerializing(EventArgs.Empty);
		this.Serializing?.Invoke(null, EventArgs.Empty);
	}

	protected virtual void OnDeserializing(EventArgs e)
	{
		InvokeGlobalDeserializing(this, e);
	}

	public void InvokeDeserializing()
	{
		OnDeserializing(EventArgs.Empty);
		this.Deserializing?.Invoke(null, EventArgs.Empty);
	}

	public StiSerializing()
		: this(new StiObjectStringConverter())
	{
	}

	public StiSerializing(StiObjectStringConverter converter)
	{
		this.converter = converter;
	}
}
