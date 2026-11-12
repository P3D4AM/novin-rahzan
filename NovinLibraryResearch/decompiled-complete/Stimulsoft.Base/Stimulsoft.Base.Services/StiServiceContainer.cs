using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;

namespace Stimulsoft.Base.Services;

public class StiServiceContainer : CollectionBase, ICloneable
{
	private static Hashtable assemlyToSkip;

	private Hashtable keysHashtable = new Hashtable();

	private Hashtable typesHashtable = new Hashtable();

	private bool noTypes;

	private static Hashtable AssemblyToTypes;

	public StiService this[int index]
	{
		get
		{
			return (StiService)base.List[index];
		}
		set
		{
			lock (base.List)
			{
				base.List[index] = value;
				BuildTypes();
			}
		}
	}

	public static string[] StandardAssemblies { get; set; }

	public event StiServiceActionHandler BeforeGetService;

	public object Clone()
	{
		lock (this)
		{
			return MemberwiseClone();
		}
	}

	public new void Clear()
	{
		base.InnerList.Clear();
		keysHashtable = new Hashtable();
		typesHashtable = new Hashtable();
	}

	public void Add(StiService service)
	{
		Add(service, callBeforeGetService: true);
	}

	public void Add(StiService service, bool callBeforeGetService)
	{
		if (!(service.ServiceType != null))
		{
			return;
		}
		if (callBeforeGetService && this.BeforeGetService != null)
		{
			StiServiceActionEventArgs stiServiceActionEventArgs = new StiServiceActionEventArgs(StiServiceActionType.Add)
			{
				Services = new List<StiService> { service }
			};
			this.BeforeGetService(service.ServiceType, stiServiceActionEventArgs);
			if (stiServiceActionEventArgs.Processed)
			{
				return;
			}
		}
		service.PackService();
		lock (base.List)
		{
			lock (keysHashtable)
			{
				string stringFromService = GetStringFromService(service);
				if (keysHashtable[stringFromService] != null)
				{
					return;
				}
				keysHashtable.Add(stringFromService, service);
				base.List.Add(service);
				if (noTypes)
				{
					return;
				}
				lock (typesHashtable)
				{
					StiServiceContainer stiServiceContainer = typesHashtable[service.ServiceType] as StiServiceContainer;
					if (stiServiceContainer == null)
					{
						stiServiceContainer = new StiServiceContainer(noTypes: true);
						typesHashtable.Add(service.ServiceType, stiServiceContainer);
					}
					lock (stiServiceContainer)
					{
						stiServiceContainer.Add(service);
					}
				}
			}
		}
	}

	public List<T> ToList<T>()
	{
		List<T> list = new List<T>();
		foreach (T item in base.List)
		{
			list.Add(item);
		}
		return list;
	}

	public List<StiService> ToList()
	{
		return ToList<StiService>();
	}

	public void AddRange(StiServiceContainer services)
	{
		foreach (StiService service in services)
		{
			Add(service);
		}
	}

	public bool Contains(StiService service)
	{
		string stringFromService = GetStringFromService(service);
		return keysHashtable[stringFromService] != null;
	}

	public bool Contains(string service)
	{
		return keysHashtable[service] != null;
	}

	public int IndexOf(StiService service)
	{
		return base.List.IndexOf(service);
	}

	public void Insert(int index, StiService service)
	{
		lock (base.List)
		{
			service.PackService();
			base.List.Insert(index, service);
			lock (keysHashtable)
			{
				string stringFromService = GetStringFromService(service);
				if (keysHashtable[stringFromService] != null)
				{
					return;
				}
				keysHashtable.Add(stringFromService, service);
				if (noTypes)
				{
					return;
				}
				lock (typesHashtable)
				{
					StiServiceContainer stiServiceContainer = typesHashtable[service.ServiceType] as StiServiceContainer;
					if (stiServiceContainer == null)
					{
						stiServiceContainer = new StiServiceContainer(noTypes: true);
						typesHashtable.Add(service.ServiceType, stiServiceContainer);
					}
					lock (stiServiceContainer)
					{
						stiServiceContainer.Add(service);
					}
				}
			}
		}
	}

	public void Remove(Type serviceType)
	{
		lock (base.List)
		{
			lock (keysHashtable)
			{
				StiServiceContainer services = GetServices(serviceType);
				foreach (StiService item in services)
				{
					base.List.Remove(item);
					keysHashtable.Remove(GetStringFromService(item));
				}
				if (!noTypes)
				{
					lock (typesHashtable)
					{
						typesHashtable.Remove(serviceType);
						return;
					}
				}
			}
		}
	}

	public void Remove(StiService service)
	{
		lock (base.List)
		{
			base.List.Remove(service);
			keysHashtable.Remove(GetStringFromService(service));
			if (noTypes)
			{
				return;
			}
			lock (typesHashtable)
			{
				StiServiceContainer stiServiceContainer = typesHashtable[service.ServiceType] as StiServiceContainer;
				lock (stiServiceContainer)
				{
					stiServiceContainer.Remove(service);
				}
			}
		}
	}

	public void Remove(StiServiceContainer services)
	{
		foreach (StiService service in services)
		{
			Remove(service);
		}
	}

	private void BuildTypes()
	{
		if (noTypes)
		{
			return;
		}
		lock (typesHashtable)
		{
			typesHashtable.Clear();
			foreach (StiService item in base.List)
			{
				StiServiceContainer stiServiceContainer = typesHashtable[item.ServiceType] as StiServiceContainer;
				if (stiServiceContainer == null)
				{
					stiServiceContainer = new StiServiceContainer(noTypes: true);
					typesHashtable.Add(item.ServiceType, stiServiceContainer);
				}
				lock (stiServiceContainer)
				{
					stiServiceContainer.Add(item);
				}
			}
		}
	}

	public static StiServiceContainer GetStandardServices()
	{
		StiServiceContainer stiServiceContainer = new StiServiceContainer();
		string[] standardAssemblies = StandardAssemblies;
		foreach (string assemblyName in standardAssemblies)
		{
			try
			{
				StiServiceContainer servicesFromAssembly = GetServicesFromAssembly(assemblyName);
				if (servicesFromAssembly != null)
				{
					stiServiceContainer.AddRange(servicesFromAssembly);
				}
			}
			catch
			{
			}
		}
		return stiServiceContainer;
	}

	public static ArrayList GetStandardServicesNames()
	{
		ArrayList arrayList = new ArrayList();
		string[] standardAssemblies = StandardAssemblies;
		foreach (string assemblyName in standardAssemblies)
		{
			ArrayList servicesNamesFromAssembly = GetServicesNamesFromAssembly(StiAssemblyFinder.GetAssembly(assemblyName));
			arrayList.Add(servicesNamesFromAssembly);
		}
		return arrayList;
	}

	public StiService[] ToArray()
	{
		StiService[] array = new StiService[base.Count];
		int num = 0;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiService stiService = (StiService)enumerator.Current;
				array[num++] = stiService;
			}
			return array;
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}

	public StiService GetService(Type serviceType)
	{
		return GetService(serviceType, getEnabled: false);
	}

	public StiService GetService(Type serviceType, bool getEnabled)
	{
		return GetService(serviceType, getEnabled, callBeforeGetService: true);
	}

	public StiService GetService(Type serviceType, bool getEnabled, bool callBeforeGetService)
	{
		if (callBeforeGetService && this.BeforeGetService != null)
		{
			StiServiceActionEventArgs stiServiceActionEventArgs = new StiServiceActionEventArgs(StiServiceActionType.Get);
			this.BeforeGetService(serviceType, stiServiceActionEventArgs);
			if (stiServiceActionEventArgs.Processed)
			{
				if (stiServiceActionEventArgs.Services == null)
				{
					return null;
				}
				return stiServiceActionEventArgs.Services.FirstOrDefault();
			}
		}
		if (!noTypes)
		{
			if (typesHashtable[serviceType] is StiServiceContainer { Count: >0 } stiServiceContainer)
			{
				foreach (StiService item in stiServiceContainer)
				{
					if (item.ServiceEnabled || !getEnabled)
					{
						return item;
					}
				}
				return null;
			}
			return null;
		}
		foreach (StiService item2 in base.List)
		{
			if (item2.ServiceType == serviceType && (item2.ServiceEnabled || !getEnabled))
			{
				return item2;
			}
		}
		return null;
	}

	public StiServiceContainer GetServices(Type serviceType)
	{
		return GetServices(serviceType, getEnabled: true);
	}

	public StiServiceContainer GetServices(Type serviceType, bool getEnabled)
	{
		return GetServices(serviceType, getEnabled, callBeforeGetService: true);
	}

	public StiServiceContainer GetServices(Type serviceType, bool getEnabled, bool callBeforeGetService)
	{
		if (callBeforeGetService && this.BeforeGetService != null)
		{
			StiServiceActionEventArgs stiServiceActionEventArgs = new StiServiceActionEventArgs(StiServiceActionType.Get);
			this.BeforeGetService(serviceType, stiServiceActionEventArgs);
			if (stiServiceActionEventArgs.Processed)
			{
				StiServiceContainer stiServiceContainer = new StiServiceContainer();
				if (stiServiceActionEventArgs.Services != null)
				{
					foreach (StiService service in stiServiceActionEventArgs.Services)
					{
						stiServiceContainer.Add(service);
					}
				}
				return stiServiceContainer;
			}
		}
		if (!noTypes)
		{
			if (typesHashtable[serviceType] is StiServiceContainer stiServiceContainer2)
			{
				StiServiceContainer stiServiceContainer3 = new StiServiceContainer();
				lock (stiServiceContainer2)
				{
					foreach (StiService item in stiServiceContainer2)
					{
						if (item.ServiceEnabled || !getEnabled)
						{
							stiServiceContainer3.Add(item);
						}
					}
					return stiServiceContainer3;
				}
			}
			return new StiServiceContainer();
		}
		StiServiceContainer stiServiceContainer4 = new StiServiceContainer(noTypes: true);
		foreach (StiService item2 in base.List)
		{
			if (item2.ServiceType == serviceType && (item2.ServiceEnabled || !getEnabled))
			{
				stiServiceContainer4.Add(item2);
			}
		}
		return stiServiceContainer4;
	}

	public static string GetStringFromService(StiService service)
	{
		return GetStringFromService(service.GetType());
	}

	public static string GetStringFromService(Type serviceType)
	{
		Assembly assembly = serviceType.Assembly;
		string arg = (serviceType.ToString().StartsWithInvariant("Stimulsoft.") ? ".dll" : Path.GetExtension(assembly.CodeBase).ToLowerInvariant());
		return $"{assembly.GetName().Name}{arg},{serviceType}";
	}

	public static StiServiceContainer GetServicesFromAssembly(string assemblyName)
	{
		Assembly assembly = StiAssemblyFinder.GetAssembly(assemblyName);
		if (assembly == null)
		{
			return new StiServiceContainer();
		}
		return GetServicesFromAssembly(assembly);
	}

	private static Type[] GetTypesFromAssembly(Assembly a)
	{
		lock (AssemblyToTypes)
		{
			Type[] array = AssemblyToTypes[a] as Type[];
			if (array == null)
			{
				try
				{
					array = a.GetTypes();
				}
				catch (ReflectionTypeLoadException ex)
				{
					array = ex.Types;
				}
				if (AssemblyToTypes[a] == null)
				{
					AssemblyToTypes[a] = array;
				}
			}
			return array;
		}
	}

	public static StiServiceContainer GetServicesFromAssembly(Assembly a)
	{
		StiServiceContainer stiServiceContainer = new StiServiceContainer();
		if (a != null)
		{
			Type[] typesFromAssembly = GetTypesFromAssembly(a);
			Type typeFromHandle = typeof(StiService);
			Type[] array = typesFromAssembly;
			foreach (Type type in array)
			{
				if (!StiTypeFinder.FindType(type, typeFromHandle) || type.IsAbstract)
				{
					continue;
				}
				try
				{
					if (StiActivator.CreateObject(type) is StiService stiService && stiService.ServiceType != null)
					{
						stiServiceContainer.Add(stiService);
					}
				}
				catch
				{
				}
			}
		}
		return stiServiceContainer;
	}

	public static ArrayList GetServicesNamesFromAssembly(Assembly a)
	{
		ArrayList arrayList = new ArrayList();
		if (a != null)
		{
			try
			{
				Type[] typesFromAssembly = GetTypesFromAssembly(a);
				Type typeFromHandle = typeof(StiService);
				Type[] array = typesFromAssembly;
				foreach (Type type in array)
				{
					try
					{
						if (StiTypeFinder.FindType(type, typeFromHandle) && !type.IsAbstract)
						{
							arrayList.Add(GetStringFromService(type));
						}
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}
		return arrayList;
	}

	public static StiService CreateService(string assemblyName, string type)
	{
		Assembly assembly = StiAssemblyFinder.GetAssembly(assemblyName);
		if (assembly != null)
		{
			lock (AssemblyToTypes)
			{
				Type[] typesFromAssembly = GetTypesFromAssembly(assembly);
				lock (StiTypeFinder.TypeToString)
				{
					Type type2 = StiTypeFinder.TypeToString[type] as Type;
					if (type2 == null)
					{
						Type[] array = typesFromAssembly;
						foreach (Type type3 in array)
						{
							string text = type3.ToString();
							if (text == type)
							{
								type2 = type3;
								StiTypeFinder.TypeToString[type] = type3;
								break;
							}
						}
					}
					return (StiService)StiActivator.CreateObject(type2);
				}
			}
		}
		return null;
	}

	public void Save(XmlWriter tw)
	{
		StiObjectStringConverter stiObjectStringConverter = new StiObjectStringConverter();
		tw.WriteStartElement("Services");
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiService stiService = (StiService)enumerator.Current;
				tw.WriteStartElement("service");
				Assembly assembly = stiService.GetType().Assembly;
				string extension = Path.GetExtension(assembly.Location);
				tw.WriteAttributeString("assembly", assembly.GetName().Name + extension);
				tw.WriteAttributeString("type", stiService.GetType().ToString());
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(stiService);
				if (properties.Count != 0)
				{
					properties = properties.Sort();
					foreach (PropertyDescriptor item in properties)
					{
						if (!(item.Attributes[typeof(StiServiceParamAttribute)] is StiServiceParamAttribute))
						{
							continue;
						}
						DefaultValueAttribute defaultValueAttribute = item.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
						object value = item.GetValue(stiService);
						if (defaultValueAttribute != null && defaultValueAttribute.Value.Equals(value))
						{
							continue;
						}
						tw.WriteStartElement("property");
						if (value == null)
						{
							tw.WriteAttributeString("isNull", "True");
						}
						else if (value is Array)
						{
							tw.WriteAttributeString("name", item.Name);
							tw.WriteAttributeString("isList", "True");
							tw.WriteAttributeString("count", ((Array)value).Length.ToString());
							tw.WriteAttributeString("type", value.GetType().GetElementType().ToString());
							Array array = value as Array;
							foreach (object item2 in array)
							{
								tw.WriteStartElement("item");
								tw.WriteString(stiObjectStringConverter.ObjectToString(item2));
								tw.WriteEndElement();
							}
						}
						else
						{
							tw.WriteAttributeString("name", item.Name);
							tw.WriteAttributeString("type", value.GetType().ToString());
							tw.WriteAttributeString("value", stiObjectStringConverter.ObjectToString(value));
						}
						tw.WriteEndElement();
					}
				}
				tw.WriteEndElement();
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		tw.WriteEndElement();
	}

	public void Save(Stream stream)
	{
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
		xmlTextWriter.Formatting = Formatting.Indented;
		Save(xmlTextWriter);
	}

	public void Save(string fileName)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(fileName, Encoding.UTF8);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument(standalone: true);
			Save(xmlTextWriter);
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public void Load(XmlReader tr)
	{
		StiObjectStringConverter stiObjectStringConverter = new StiObjectStringConverter();
		StiService stiService = null;
		tr.Read();
		while (tr.Read())
		{
			if (!tr.IsStartElement())
			{
				continue;
			}
			if (tr.Name == "service")
			{
				string attribute = tr.GetAttribute("assembly");
				string attribute2 = tr.GetAttribute("type");
				if (assemlyToSkip[attribute.ToLower(CultureInfo.InvariantCulture)] != null)
				{
					continue;
				}
				try
				{
					stiService = CreateService(attribute, attribute2);
					if (stiService != null)
					{
						Add(stiService, callBeforeGetService: false);
					}
				}
				catch
				{
				}
			}
			else
			{
				if (!(tr.Name == "property") || stiService == null)
				{
					continue;
				}
				string attribute3 = tr.GetAttribute("name");
				string attribute4 = tr.GetAttribute("type");
				PropertyInfo property = stiService.GetType().GetProperty(attribute3);
				if (!(property != null))
				{
					continue;
				}
				if (tr.GetAttribute("isNull") != null)
				{
					property.SetValue(stiService, null, null);
				}
				else if (tr.GetAttribute("isList") != null)
				{
					int num = int.Parse(tr.GetAttribute("count"));
					Type type = StiTypeFinder.GetType(attribute4);
					Array array = Array.CreateInstance(type, num);
					int num2 = 0;
					while (tr.Read())
					{
						if (tr.IsStartElement())
						{
							string str = tr.ReadString();
							object obj2 = stiObjectStringConverter.StringToObject(str, type);
							if (obj2 != null)
							{
								array.SetValue(obj2, new int[1] { num2++ });
							}
							if (num2 >= num)
							{
								break;
							}
						}
					}
					property.SetValue(stiService, array, null);
				}
				else
				{
					string attribute5 = tr.GetAttribute("value");
					object obj3 = stiObjectStringConverter.StringToObject(attribute5, StiTypeFinder.GetType(attribute4));
					if (obj3 != null)
					{
						property.SetValue(stiService, obj3, null);
					}
				}
			}
		}
	}

	public void Load(Stream stream)
	{
		Clear();
		XmlTextReader xmlTextReader = new XmlTextReader(stream);
		xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
		Load(xmlTextReader);
		xmlTextReader.Close();
	}

	public void Load(string fileName)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			Clear();
			XmlTextReader xmlTextReader = new XmlTextReader(fileName);
			xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
			xmlTextReader.Read();
			xmlTextReader.Read();
			Load(xmlTextReader);
			xmlTextReader.Close();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public StiServiceContainer()
		: this(noTypes: false)
	{
	}

	private StiServiceContainer(bool noTypes)
	{
		this.noTypes = noTypes;
	}

	static StiServiceContainer()
	{
		assemlyToSkip = new Hashtable();
		AssemblyToTypes = new Hashtable();
		StandardAssemblies = new string[2] { "Stimulsoft.Base.dll", "Stimulsoft.Report.dll" };
		string value = "NotNull";
		assemlyToSkip["stimulsoft.report.export.images.dll"] = value;
		assemlyToSkip["stimulsoft.report.export.htmlexport.dll"] = value;
		assemlyToSkip["stimulsoft.report.export.txtexport.dll"] = value;
		assemlyToSkip["stimulsoft.report.export.excelxmlexport.dll"] = value;
		assemlyToSkip["stimulsoft.report.export.excelhtmlexport.dll"] = value;
		assemlyToSkip["stimulsoft.report.export.xmlexport.dll"] = value;
		assemlyToSkip["stimulsoft.report.export.csvexport.dll"] = value;
		assemlyToSkip["stimulsoft.report.export.rtfexport.dll"] = value;
		assemlyToSkip["stimulsoft.report.export.pdfexport.dll"] = value;
	}
}
