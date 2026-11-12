using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Server;

namespace Microsoft.SqlServer.Management.Facets;

public sealed class FacetRepository
{
	internal sealed class InterfaceObjectPair
	{
		private Type iface;

		private Type obj;

		public Type Interface => iface;

		public Type Object => obj;

		public InterfaceObjectPair(Type iface, Type obj)
		{
			this.iface = iface;
			this.obj = obj;
		}

		public override string ToString()
		{
			return $"{Interface.ToString()} ; {Object.ToString()}";
		}

		public override bool Equals(object obj)
		{
			if (obj is InterfaceObjectPair)
			{
				InterfaceObjectPair interfaceObjectPair = (InterfaceObjectPair)obj;
				if (Interface.Equals(interfaceObjectPair.Interface))
				{
					return Object.Equals(interfaceObjectPair.Object);
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Interface.GetHashCode() & Object.GetHashCode();
		}
	}

	private static TraceContext traceContext;

	private static readonly Hashtable registeredFacets;

	private static readonly Hashtable FacetNameAndObjectTypeToAdapterMap;

	private static readonly List<string> registeredAssemblies;

	internal static int RegisteredFacetsCount => registeredFacets.Count;

	public static IEnumerable RegisteredFacets => registeredFacets.Keys;

	static FacetRepository()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "FacetRepository");
		registeredFacets = new Hashtable();
		FacetNameAndObjectTypeToAdapterMap = new Hashtable();
		registeredAssemblies = new List<string>();
		BuildDmfLookupTables(Assembly.GetExecutingAssembly());
		BuildDmfLookupTables(typeof(SqlSmoObject).Assembly);
		if (!SqlContext.IsAvailable)
		{
			string[] array = Assembly.GetExecutingAssembly().FullName.Split(new char[1] { ',' }, 2);
			LoadAssemblyHandleErrors("Microsoft.SqlServer.Dmf.Adapters, " + array[1]);
			LoadAssemblyHandleErrors("Microsoft.SqlServer.Management.Utility, " + array[1]);
		}
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	private static void LoadAssemblyHandleErrors(string assemblyName)
	{
		try
		{
			BuildDmfLookupTables(Assembly.Load(assemblyName));
		}
		catch (FileNotFoundException)
		{
			traceContext.TraceError("Caught a general Exception of type System.IO.FileNotFoundException");
		}
		catch (FileLoadException)
		{
			traceContext.TraceError("Caught a general Exception of type System.IO.FileLoadException");
		}
		catch (BadImageFormatException)
		{
			traceContext.TraceError("Caught a general Exception of type System.BadImageFormatException");
		}
		catch (ReflectionTypeLoadException)
		{
			traceContext.TraceError("Caught a general Exception of type System.Reflection.ReflectionTypeLoadException");
		}
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static void BuildDmfLookupTables(Assembly assembly)
	{
		if (registeredAssemblies.Contains(assembly.FullName))
		{
			throw traceContext.TraceThrow(new AssemblyAlreadyRegisteredException(assembly.FullName));
		}
		registeredAssemblies.Add(assembly.FullName);
		BuildInterfaceLookup(assembly);
		BuildAdapterLookup(assembly);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	private static void BuildInterfaceLookup(Assembly assembly)
	{
		FacetType facetType = FacetType.Logical;
		Type[] types = assembly.GetTypes();
		foreach (Type type in types)
		{
			if (IsManagementFacet(type))
			{
				facetType = FacetType.Logical;
			}
			else
			{
				if (!IsPhysicalFacet(type))
				{
					continue;
				}
				facetType = FacetType.Physical;
			}
			FacetAttributes facetAttributes = new FacetAttributes();
			facetAttributes.FacetType = facetType;
			facetAttributes.IsReadOnly = IsReadOnlyFacet(type);
			Attribute customAttribute = Attribute.GetCustomAttribute(type, typeof(EvaluationModeAttribute));
			if (customAttribute != null)
			{
				facetAttributes.EvaluationMode = ((EvaluationModeAttribute)customAttribute).AutomatedPolicyEvaluationMode;
			}
			Attribute customAttribute2 = Attribute.GetCustomAttribute(type, typeof(RootFacetAttribute));
			if (customAttribute2 != null)
			{
				facetAttributes.RootType = ((RootFacetAttribute)customAttribute2).RootType;
			}
			registeredFacets.Add(type, facetAttributes);
		}
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	private static void BuildAdapterLookup(Assembly assembly)
	{
		Type[] types = assembly.GetTypes();
		foreach (Type type in types)
		{
			if (type.IsAbstract || type.IsInterface)
			{
				continue;
			}
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				if (!registeredFacets.ContainsKey(type2))
				{
					continue;
				}
				if (IsAdapter(type))
				{
					ConstructorInfo[] constructors = type.GetConstructors();
					for (int k = 0; k < constructors.Length; k++)
					{
						ParameterInfo[] parameters = constructors[k].GetParameters();
						Type parameterType = parameters[0].ParameterType;
						if (parameters.Length == 1)
						{
							InterfaceObjectPair interfaceObjectPair = new InterfaceObjectPair(type2, parameterType);
							if (!FacetNameAndObjectTypeToAdapterMap.ContainsKey(interfaceObjectPair))
							{
								FacetNameAndObjectTypeToAdapterMap.Add(interfaceObjectPair, type);
								FacetAttributes facetAttributes = (FacetAttributes)registeredFacets[type2];
								facetAttributes.FacetType = FacetType.Adapted;
								registeredFacets[type2] = facetAttributes;
								continue;
							}
							throw traceContext.TraceThrow(new AdapterAlreadyExistsException(interfaceObjectPair.Interface.Name, interfaceObjectPair.Object.Name));
						}
						throw traceContext.TraceThrow(new AdapterWrongNumberOfArgumentsException(type.Name));
					}
				}
				else
				{
					InterfaceObjectPair key = new InterfaceObjectPair(type2, type);
					if (!FacetNameAndObjectTypeToAdapterMap.ContainsKey(key))
					{
						FacetNameAndObjectTypeToAdapterMap.Add(key, null);
					}
				}
			}
		}
		foreach (Type key3 in registeredFacets.Keys)
		{
			if (((FacetAttributes)registeredFacets[key3]).FacetType == FacetType.Physical)
			{
				InterfaceObjectPair key2 = new InterfaceObjectPair(key3, key3);
				if (!FacetNameAndObjectTypeToAdapterMap.ContainsKey(key2))
				{
					FacetNameAndObjectTypeToAdapterMap.Add(key2, null);
				}
			}
		}
	}

	public static PropertyInfo[] GetFacetProperties(Type managementFacet)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetFacetProperties", TraceEventType.Information);
		methodTraceContext.TraceParameters(managementFacet);
		if (!registeredFacets.Contains(managementFacet))
		{
			methodTraceContext.TraceParameterOut("returnVal", null);
			return null;
		}
		if (((FacetAttributes)registeredFacets[managementFacet]).FacetType == FacetType.Physical)
		{
			return Utils.GetPhysicalFacetProperties(managementFacet);
		}
		return managementFacet.GetProperties();
	}

	private static PropertyInfo GetPropertyInfo(Type managementFacet, string propertyName)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetPropertyInfo");
		methodTraceContext.TraceParameters(managementFacet, propertyName);
		if (managementFacet == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("managementFacet"));
		}
		if (propertyName == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("propertyName"));
		}
		PropertyInfo property = managementFacet.GetProperty(propertyName);
		if (property == null)
		{
			throw methodTraceContext.TraceThrow(new MissingPropertyException(propertyName));
		}
		methodTraceContext.TraceParameterOut("returnVal", property);
		return property;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static bool IsPropertyConfigurable(Type managementFacet, string propertyName)
	{
		if (managementFacet == null)
		{
			throw new ArgumentNullException("managementFacet");
		}
		if (propertyName == null)
		{
			throw new ArgumentNullException("propertyName");
		}
		if (registeredFacets.Contains(managementFacet) && ((FacetAttributes)registeredFacets[managementFacet]).IsReadOnly)
		{
			return false;
		}
		PropertyInfo propertyInfo = GetPropertyInfo(managementFacet, propertyName);
		if (!propertyInfo.CanWrite)
		{
			return false;
		}
		SfcPropertyAttribute sfcPropertyAttribute = (SfcPropertyAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(SfcPropertyAttribute), inherit: true);
		if (sfcPropertyAttribute != null && sfcPropertyAttribute.ReadOnlyAfterCreation)
		{
			return false;
		}
		if ((DmfIgnorePropertyAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(DmfIgnorePropertyAttribute), inherit: true) != null)
		{
			return false;
		}
		return true;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static List<Type> GetFacetSupportedTypes(Type managementFacet)
	{
		List<Type> list = new List<Type>();
		foreach (InterfaceObjectPair key in FacetNameAndObjectTypeToAdapterMap.Keys)
		{
			if (key.Interface == managementFacet)
			{
				list.Add(key.Object);
			}
		}
		list.Sort((Type t1, Type t2) => t1.Name.CompareTo(t2.Name));
		return list;
	}

	public static List<Type> GetRootFacets(Type rootType)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetRootFacets", TraceEventType.Information);
		methodTraceContext.TraceParameters(rootType);
		List<Type> list = new List<Type>();
		if (null != rootType)
		{
			foreach (Type key in registeredFacets.Keys)
			{
				if (((FacetAttributes)registeredFacets[key]).RootType == rootType)
				{
					list.Add(key);
				}
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", list);
		return list;
	}

	public static bool IsRootFacet(Type rootType, Type facet)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("IsRootFacet", TraceEventType.Information);
		methodTraceContext.TraceParameters(rootType, facet);
		if (null == rootType)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("rootType"));
		}
		return rootType == ((FacetAttributes)registeredFacets[facet]).RootType;
	}

	public static Type GetFacetSupportedRootType(Type facet)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetFacetSupportedRootType", TraceEventType.Information);
		methodTraceContext.TraceParameters(facet);
		Type type = null;
		using (List<Type>.Enumerator enumerator = GetFacetSupportedTypes(facet).GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				type = SfcMetadataDiscovery.GetRootFromType(enumerator.Current);
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", type);
		return type;
	}

	internal static Type GetFacetImplementingType(Type target, Type facet)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetFacetImplementingType");
		methodTraceContext.TraceParameters(target, facet);
		if (!IsRegisteredFacet(facet))
		{
			throw methodTraceContext.TraceThrow(new NullFacetException(facet.Name));
		}
		switch (((FacetAttributes)registeredFacets[facet]).FacetType)
		{
		case FacetType.Physical:
			methodTraceContext.TraceParameterOut("returnVal", facet);
			return facet;
		case FacetType.Logical:
			methodTraceContext.TraceParameterOut("returnVal", target);
			return target;
		case FacetType.Adapted:
		{
			InterfaceObjectPair key = new InterfaceObjectPair(facet, target);
			Type type = (Type)FacetNameAndObjectTypeToAdapterMap[key];
			traceContext.DebugAssert(null != type, "No adapter registered for adapted facet");
			methodTraceContext.TraceParameterOut("returnVal", type);
			return type;
		}
		default:
			traceContext.DebugAssert(test: true, "Unknown Facet type");
			methodTraceContext.TraceParameterOut("returnVal", null);
			return null;
		}
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static FacetEvaluationContext GetAdapterObject(object target, Type facet)
	{
		object obj = null;
		FacetEvaluationContext facetEvaluationContext = null;
		if (!IsRegisteredFacet(facet))
		{
			throw traceContext.TraceThrow(new NullFacetException(facet.Name));
		}
		VerifyAssociation(target.GetType(), facet);
		FacetAttributes facetAttributes = (FacetAttributes)registeredFacets[facet];
		if (facetAttributes.FacetType == FacetType.Adapted)
		{
			Type facetImplementingType = GetFacetImplementingType(target.GetType(), facet);
			if (null == facetImplementingType)
			{
				throw traceContext.TraceThrow(new MissingTypeFacetAssociationException(target.GetType().Name, facet.Name));
			}
			obj = Activator.CreateInstance(facetImplementingType, target);
			return new FacetEvaluationContext(facet, obj, facetAttributes.FacetType, target);
		}
		return new FacetEvaluationContext(facet, target, facetAttributes.FacetType, target);
	}

	public static List<Type> GetFacetsForType(Type target)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetFacetsForType", TraceEventType.Information);
		methodTraceContext.TraceParameters(target);
		List<Type> list = new List<Type>();
		foreach (InterfaceObjectPair key in FacetNameAndObjectTypeToAdapterMap.Keys)
		{
			if (key.Object.Equals(target))
			{
				list.Add(key.Interface);
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", list);
		return list;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static Type GetFacetType(string facetShortName)
	{
		foreach (Type key in registeredFacets.Keys)
		{
			if (key.Name == facetShortName)
			{
				return key;
			}
		}
		return null;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static AutomatedPolicyEvaluationMode GetFacetEvaluationMode(Type facet)
	{
		if (registeredFacets.ContainsKey(facet))
		{
			return ((FacetAttributes)registeredFacets[facet]).EvaluationMode;
		}
		traceContext.TraceParameterOut("returnVal", AutomatedPolicyEvaluationMode.None);
		return AutomatedPolicyEvaluationMode.None;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static bool IsRegisteredFacet(Type facet)
	{
		return registeredFacets.ContainsKey(facet);
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	private static bool IsManagementFacet(Type managementFacet)
	{
		if ((managementFacet.IsPublic || (managementFacet.IsNested && managementFacet.IsNestedPublic)) && managementFacet.IsInterface)
		{
			return null != managementFacet.GetInterface("IDmfFacet");
		}
		return false;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static bool IsPhysicalFacet(Type type)
	{
		if ((type.IsPublic || (type.IsNested && type.IsNestedPublic)) && type.IsClass && !type.IsAbstract && null != type.GetInterface("ISfcPropertyProvider"))
		{
			return Attribute.GetCustomAttribute(type, typeof(PhysicalFacetAttribute)) != null;
		}
		return false;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	internal static bool IsReadOnlyFacet(Type type)
	{
		bool result = false;
		PhysicalFacetAttribute physicalFacetAttribute = (PhysicalFacetAttribute)Attribute.GetCustomAttribute(type, typeof(PhysicalFacetAttribute));
		if (physicalFacetAttribute != null)
		{
			result = physicalFacetAttribute.IsReadOnly;
		}
		return result;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	private static bool IsAdapter(Type type)
	{
		if (!type.IsInterface)
		{
			return null != type.GetInterface("IDmfAdapter");
		}
		return false;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	private static void VerifyAssociation(Type target, Type facet)
	{
		InterfaceObjectPair key = new InterfaceObjectPair(facet, target);
		if (!FacetNameAndObjectTypeToAdapterMap.ContainsKey(key))
		{
			throw traceContext.TraceThrow(new MissingTypeFacetAssociationException(target.Name, facet.Name));
		}
	}
}
