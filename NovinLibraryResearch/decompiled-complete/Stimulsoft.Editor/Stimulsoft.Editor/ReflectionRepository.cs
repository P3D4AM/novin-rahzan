using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace Stimulsoft.Editor;

public class ReflectionRepository : NETRepository, IReflectionRepository, ICodeCompletionRepository
{
	public class DescriptionHelper
	{
		private static Hashtable assemblies = new Hashtable();

		private static string systemAssemblyFolder = string.Empty;

		private static Regex regex = new Regex("((\\r\\n)|(\\n)|(\\r))(\\s)+", RegexOptions.Multiline);

		private static bool enabled = true;

		public static string SystemAssemblyFolder
		{
			get
			{
				if (systemAssemblyFolder == string.Empty)
				{
					Assembly[] array = AppDomain.CurrentDomain.GetAssemblies();
					foreach (Assembly assembly in array)
					{
						AssemblyName name = assembly.GetName();
						if (name != null && string.Compare(name.Name, "mscorlib", ignoreCase: true) == 0)
						{
							systemAssemblyFolder = $"{Path.GetDirectoryName(assembly.Location)}\\";
						}
					}
				}
				return systemAssemblyFolder;
			}
		}

		public static bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		private static string GetInnerText(XmlNode node)
		{
			string text = string.Empty;
			if (node is XmlText)
			{
				text = node.InnerText;
				int num = text.IndexOf(":");
				if (num >= 0)
				{
					text = text.Substring(num + 1);
				}
				num = text.IndexOf(".#ctor");
				if (num >= 0)
				{
					text = text.Substring(0, num);
				}
			}
			else
			{
				if (node.Attributes != null)
				{
					foreach (XmlAttribute attribute in node.Attributes)
					{
						text += GetInnerText(attribute);
					}
				}
				if (node.HasChildNodes)
				{
					foreach (XmlNode childNode in node.ChildNodes)
					{
						text += GetInnerText(childNode);
					}
				}
			}
			return text;
		}

		private static XmlNode GetNodeByName(XmlNode node, string nodeName)
		{
			if (node != null)
			{
				XmlNode xmlNode = (node.Name.Equals(nodeName) ? node : null);
				if (xmlNode != null)
				{
					return xmlNode;
				}
				if (node.HasChildNodes)
				{
					foreach (XmlNode childNode in node.ChildNodes)
					{
						xmlNode = GetNodeByName(childNode, nodeName);
						if (xmlNode != null)
						{
							return xmlNode;
						}
					}
				}
			}
			return null;
		}

		private static string GetNodeName(XmlNode node)
		{
			if (node.Attributes != null)
			{
				foreach (XmlAttribute attribute in node.Attributes)
				{
					if (attribute.Name == "name")
					{
						return attribute.InnerText;
					}
				}
			}
			return string.Empty;
		}

		private static string GetNodeSummary(XmlNode node)
		{
			XmlNode nodeByName = GetNodeByName(node, "summary");
			if (nodeByName == null)
			{
				return string.Empty;
			}
			return GetInnerText(nodeByName);
		}

		private static void LoadParameters(string name, XmlNode node, Hashtable descriptions)
		{
			if (!node.HasChildNodes)
			{
				return;
			}
			foreach (XmlNode childNode in node.ChildNodes)
			{
				if (childNode.Name.Equals("param"))
				{
					XmlAttribute xmlAttribute = childNode.Attributes["name"];
					if (xmlAttribute != null)
					{
						descriptions[name + ".param:" + xmlAttribute.Value] = GetInnerText(childNode);
					}
				}
			}
		}

		private static void LoadXmlFile(string fileName, Hashtable descriptions)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(fileName);
			XmlNode nodeByName = GetNodeByName(xmlDocument, "members");
			if (nodeByName == null)
			{
				return;
			}
			for (XmlNode xmlNode = nodeByName.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				string nodeName = GetNodeName(xmlNode);
				if (nodeName.IndexOf(".Clear") >= 0)
				{
					descriptions[nodeName] = GetNodeSummary(xmlNode);
				}
				else
				{
					descriptions[nodeName] = GetNodeSummary(xmlNode);
				}
				if (xmlNode.HasChildNodes)
				{
					LoadParameters(nodeName, xmlNode, descriptions);
				}
			}
		}

		internal static object LoadAssembly(Assembly assembly)
		{
			object obj = assemblies[assembly];
			if (obj == null)
			{
				obj = new Hashtable();
				if (assembly.Location != null && assembly.Location != string.Empty)
				{
					try
					{
						FileInfo fileInfo = new FileInfo(Path.ChangeExtension(assembly.Location, "xml"));
						if (!fileInfo.Exists && SystemAssemblyFolder != string.Empty)
						{
							fileInfo = new FileInfo(SystemAssemblyFolder + Path.ChangeExtension(Path.GetFileName(assembly.Location), "xml"));
						}
						if (fileInfo.Exists)
						{
							LoadXmlFile(fileInfo.FullName, (Hashtable)obj);
						}
					}
					catch
					{
					}
				}
			}
			assemblies[assembly] = obj;
			return obj;
		}

		protected internal static void UnloadAssembly(Assembly assembly)
		{
			if (assemblies.Contains(assembly))
			{
				assemblies.Remove(assembly);
			}
		}

		protected internal static object GetDescriptions(Assembly assembly)
		{
			object obj = assemblies[assembly];
			if (obj == null)
			{
				obj = LoadAssembly(assembly);
			}
			return obj;
		}

		protected static string GetDescription(string prefix, Type type, string postfix, string paramName)
		{
			if (type.Assembly == null)
			{
				return null;
			}
			Hashtable hashtable = (Hashtable)GetDescriptions(type.Assembly);
			object obj = hashtable[prefix + ":" + type.FullName + ((postfix != string.Empty) ? ("." + postfix) : string.Empty) + paramName];
			if (obj == null)
			{
				return string.Empty;
			}
			return regex.Replace(obj.ToString().Trim(), " ");
		}

		protected static string GetDescription(MemberInfo info, string paramName)
		{
			if (info.Name == null)
			{
				return string.Empty;
			}
			string empty = string.Empty;
			string text = info.Name.Replace(".ctor", "#ctor");
			switch (info.MemberType)
			{
			case MemberTypes.Constructor:
			case MemberTypes.Method:
				empty = "M";
				break;
			case MemberTypes.Field:
				empty = "F";
				break;
			case MemberTypes.Event:
				empty = "E";
				break;
			case MemberTypes.TypeInfo:
			case MemberTypes.NestedType:
				empty = "T";
				text = string.Empty;
				break;
			case MemberTypes.Property:
				empty = "P";
				break;
			default:
				return string.Empty;
			}
			if (info is MethodInfo)
			{
				System.Reflection.ParameterInfo[] parameters = ((MethodInfo)info).GetParameters();
				if (parameters.Length != 0)
				{
					string text2 = string.Empty;
					foreach (System.Reflection.ParameterInfo parameterInfo in parameters)
					{
						string text3 = parameterInfo.ParameterType.ToString().Replace('&', '@');
						text2 = ((text2 == string.Empty) ? text3 : (text2 + "," + text3));
					}
					text = text + "(" + text2 + ")";
				}
			}
			object obj = null;
			Type type = ((info is Type) ? ((Type)info) : info.ReflectedType);
			if (type != null)
			{
				obj = GetDescription(empty, type, text, paramName);
			}
			if (obj == null && info.DeclaringType != null)
			{
				obj = GetDescription(empty, info.DeclaringType, text, paramName);
			}
			if (obj == null)
			{
				while (type != null)
				{
					type = type.BaseType;
					if (type != null)
					{
						obj = GetDescription(empty, info.DeclaringType, text, paramName);
						if (obj != null)
						{
							break;
						}
					}
				}
			}
			if (obj == null)
			{
				return string.Empty;
			}
			return obj.ToString();
		}

		public static string GetDescription(System.Reflection.ParameterInfo pinfo)
		{
			if (!enabled || !(pinfo.Member != null))
			{
				return string.Empty;
			}
			return GetDescription(pinfo.Member, ".param:" + pinfo.Name);
		}

		public static string GetDescription(MemberInfo info)
		{
			if (!enabled)
			{
				return string.Empty;
			}
			return GetDescription(info, string.Empty);
		}
	}

	private IList assemblies;

	private Hashtable types;

	private Hashtable internalTypes;

	private Hashtable restrictedTypes;

	public Hashtable Types => types;

	public IList Assemblies => assemblies;

	public ReflectionRepository(bool caseSensitive, ISyntaxTree syntaxTree)
		: base(caseSensitive, syntaxTree)
	{
		assemblies = new ArrayList();
		types = new Hashtable();
		internalTypes = new Hashtable();
		restrictedTypes = new Hashtable();
	}

	private int IndexOfString(IList list, string str)
	{
		for (int i = 0; i < list.Count; i++)
		{
			string strA = (string)list[i];
			if (string.Compare(strA, str, !base.CaseSensitive) == 0)
			{
				return i;
			}
		}
		return -1;
	}

	protected BindingFlags GetBindingFlags(StaticScope scope)
	{
		BindingFlags bindingFlags = BindingFlags.Default;
		if ((scope & StaticScope.Instance) != 0)
		{
			bindingFlags |= BindingFlags.Instance | BindingFlags.Public;
		}
		if ((scope & StaticScope.Static) != 0)
		{
			bindingFlags |= BindingFlags.Static | BindingFlags.Public;
		}
		if ((scope & StaticScope.Protected) != 0)
		{
			bindingFlags |= BindingFlags.Static | BindingFlags.NonPublic;
		}
		if (!base.CaseSensitive)
		{
			bindingFlags |= BindingFlags.IgnoreCase;
		}
		return bindingFlags;
	}

	protected virtual string GetDataType(Type type, bool useHtmlFormatting)
	{
		return GetDataType(type, shortName: false, checkShortName: true, useGenericArgs: false, useHtmlFormatting);
	}

	protected virtual string GetDataType(Type type, bool shortName, bool useHtmlFormatting)
	{
		return GetDataType(type, shortName, checkShortName: true, useGenericArgs: false, useHtmlFormatting);
	}

	protected virtual bool IsSystemType(Type type)
	{
		if (type != null && type.Assembly != null)
		{
			AssemblyName name = type.Assembly.GetName();
			if (name != null)
			{
				return name.Name == "mscorlib";
			}
			return false;
		}
		return false;
	}

	protected virtual string GetDataType(Type type, bool shortName, bool checkShortName, bool useGenericArgs, bool useHtmlFormatting)
	{
		object obj = internalTypes[type];
		if (obj == null)
		{
			return GetTypeName(type, shortName || (checkShortName && (type.IsPrimitive || IsSystemType(type))), useGenericArgs, useHtmlFormatting);
		}
		return (string)obj;
	}

	protected virtual Type GetMemberType(MemberInfo member)
	{
		if (member is PropertyInfo)
		{
			return ((PropertyInfo)member).PropertyType;
		}
		if (member is EventInfo)
		{
			return ((EventInfo)member).EventHandlerType;
		}
		if (member is MethodInfo)
		{
			return ((MethodInfo)member).ReturnType;
		}
		if (member is FieldInfo)
		{
			return ((FieldInfo)member).FieldType;
		}
		if (member is Type)
		{
			return (Type)member;
		}
		return null;
	}

	protected virtual string GetMemberDataType(MemberInfo member, bool useHtmlFormatting)
	{
		Type memberType = GetMemberType(member);
		if (!(memberType != null))
		{
			return string.Empty;
		}
		return GetDataType(memberType, shortName: false, checkShortName: true, useGenericArgs: true, useHtmlFormatting);
	}

	protected bool IsSpecialName(MemberInfo member)
	{
		if (member is MethodBase)
		{
			return ((MethodBase)member).IsSpecialName;
		}
		if (member is FieldInfo)
		{
			return ((FieldInfo)member).IsSpecialName;
		}
		if (member is EventInfo)
		{
			return ((EventInfo)member).IsSpecialName;
		}
		return false;
	}

	protected virtual bool IsPublicMember(MemberInfo member)
	{
		if (member is MethodBase)
		{
			return ((MethodBase)member).IsPublic;
		}
		if (member is FieldInfo)
		{
			return ((FieldInfo)member).IsPublic;
		}
		if (member is Type)
		{
			return ((Type)member).IsPublic;
		}
		return true;
	}

	protected virtual bool IsPrivateMember(MemberInfo member)
	{
		if (member is MethodBase)
		{
			return ((MethodBase)member).IsPrivate;
		}
		if (member is FieldInfo)
		{
			return ((FieldInfo)member).IsPrivate;
		}
		if (member is Type)
		{
			return ((Type)member).IsNotPublic;
		}
		return false;
	}

	protected virtual int GetMemberImageIndex(MemberInfo member)
	{
		int num = -1;
		if (member is PropertyInfo)
		{
			num = 11;
		}
		else if (member is EventInfo)
		{
			num = 7;
		}
		else if (member is MethodInfo)
		{
			num = 10;
		}
		else if (member is FieldInfo)
		{
			num = 8;
		}
		else if (member is Type)
		{
			Type type = (Type)member;
			num = (type.IsClass ? 3 : (type.IsEnum ? 6 : (type.IsInterface ? 9 : ((!type.IsValueType) ? 3 : 12))));
		}
		if (num >= 0)
		{
			if (IsPrivateMember(member))
			{
				num += 10;
			}
			else if (IsPublicMember(member))
			{
				num += 30;
			}
		}
		return num;
	}

	protected virtual string GetTypeQualifier(Type type)
	{
		if (type.IsClass)
		{
			return SyntaxConsts.ClassDataType;
		}
		if (type.IsEnum)
		{
			return SyntaxConsts.EnumDataType;
		}
		if (type.IsInterface)
		{
			return SyntaxConsts.InterfaceDataType;
		}
		if (type.IsValueType)
		{
			return SyntaxConsts.StructDataType;
		}
		return SyntaxConsts.TypeDataType;
	}

	protected virtual string GetMemberQualifier(MemberInfo member)
	{
		string text = string.Empty;
		if (member is Type)
		{
			return GetTypeQualifier((Type)member);
		}
		if (IsPrivateMember(member))
		{
			text = "private";
		}
		else if (IsPublicMember(member))
		{
			text = "public";
		}
		if (member is MethodBase)
		{
			MethodBase methodBase = (MethodBase)member;
			if (methodBase.IsAbstract)
			{
				text += " abstract";
			}
			if (methodBase.IsVirtual)
			{
				text += " virtual";
			}
		}
		return text.Trim();
	}

	protected virtual string GetParamQualifier(System.Reflection.ParameterInfo param)
	{
		if (!param.IsOut)
		{
			if (!param.IsRetval)
			{
				return string.Empty;
			}
			return "ref";
		}
		return "out";
	}

	protected virtual System.Reflection.ParameterInfo[] GetMemberParameterInfos(MemberInfo member)
	{
		System.Reflection.ParameterInfo[] array = null;
		try
		{
			if (member is MethodBase)
			{
				array = ((MethodBase)member).GetParameters();
			}
			else if (member is PropertyInfo)
			{
				array = ((PropertyInfo)member).GetIndexParameters();
				if (array != null && array.Length == 0)
				{
					array = null;
				}
			}
		}
		catch
		{
		}
		return array;
	}

	protected virtual IParameterMember[] GetMemberParameters(MemberInfo member, bool useHtmlFormatting)
	{
		System.Reflection.ParameterInfo[] memberParameterInfos = GetMemberParameterInfos(member);
		if (memberParameterInfos != null)
		{
			IParameterMember[] array = new ParameterMember[memberParameterInfos.Length];
			IParameterMember[] array2 = array;
			for (int i = 0; i < memberParameterInfos.Length; i++)
			{
				System.Reflection.ParameterInfo parameterInfo = memberParameterInfos[i];
				IParameterMember parameterMember = new ParameterMember();
				parameterMember.Name = parameterInfo.Name;
				parameterMember.DataType = GetDataType(parameterInfo.ParameterType, useHtmlFormatting);
				parameterMember.Qualifier = GetParamQualifier(parameterInfo);
				parameterMember.Description = GetDescription(null, null, parameterInfo, XmlCommentType.Param.ToString(), fullDescription: false);
				parameterMember.Modifiers = ParameterModifer.None;
				if (parameterInfo.IsOptional)
				{
					parameterMember.Modifiers |= ParameterModifer.Optional;
				}
				if (parameterInfo.IsIn)
				{
					parameterMember.Modifiers |= ParameterModifer.In;
				}
				if (parameterInfo.IsOut)
				{
					parameterMember.Modifiers |= ParameterModifer.Out;
				}
				if (parameterInfo.IsRetval)
				{
					parameterMember.Modifiers |= ParameterModifer.Retval;
				}
				array2[i] = parameterMember;
			}
			return array2;
		}
		return null;
	}

	protected override object GetElementAccessType(object member)
	{
		object obj = base.GetElementAccessType(member);
		if (obj == null)
		{
			if (member is ISyntaxNode)
			{
				ISyntaxNode syntaxNode = (ISyntaxNode)member;
				if (NETRepository.IsDeclarationNode(syntaxNode))
				{
					string baseType = GetBaseType(syntaxNode);
					if (baseType != string.Empty)
					{
						member = GetMemberType(syntaxNode, syntaxNode.Position, null, baseType, out var _);
					}
				}
			}
			if (member is Type)
			{
				obj = ((Type)member).GetElementType();
				if (obj == null)
				{
					MemberInfo[] defaultMembers = ((Type)member).GetDefaultMembers();
					if (defaultMembers != null && defaultMembers.Length != 0)
					{
						MemberInfo[] member2 = ((Type)member).GetMember(defaultMembers[0].Name);
						if (member2 != null && member2.Length != 0)
						{
							obj = GetMemberType(member2[0]);
						}
					}
				}
			}
		}
		return obj;
	}

	protected override object FindMember(string text, Point position, ISyntaxNode node, string name, int paramCount)
	{
		object obj = base.FindMember(text, position, node, name, paramCount);
		if (obj != null)
		{
			return obj;
		}
		string name2 = name;
		Point endPos = position;
		obj = GetMemberType(text, node, ref name2, ref position, ref endPos, out var scope);
		if (obj is Type)
		{
			Type type = (Type)obj;
			MemberInfo[] array = null;
			if (!(name == ".ctor"))
			{
				array = ((!(name == ".ElementAccess")) ? type.GetMember(name, GetBindingFlags(scope)) : type.GetDefaultMembers());
			}
			else
			{
				MemberInfo[] constructors = type.GetConstructors();
				array = constructors;
			}
			if (array != null && array.Length != 0)
			{
				return array[(paramCount >= 0 && paramCount < array.Length) ? paramCount : 0];
			}
		}
		return null;
	}

	protected override object GetParameterType(object member, int param)
	{
		object parameterType = base.GetParameterType(member, param);
		if (parameterType != null)
		{
			return parameterType;
		}
		if (member is MemberInfo)
		{
			System.Reflection.ParameterInfo[] memberParameterInfos = GetMemberParameterInfos((MemberInfo)member);
			if (memberParameterInfos != null && param < memberParameterInfos.Length)
			{
				return memberParameterInfos[param].ParameterType;
			}
		}
		return null;
	}

	protected virtual void FillTypeMembers(IListMembers members, Type type, string name, StaticScope scope)
	{
		if (!(name == ".ctor"))
		{
			if (name == ".ElementAccess")
			{
				FillTypeMembers(members, type.GetDefaultMembers(), type, GetDataType(type, members.UseHtmlFormatting), scope);
				return;
			}
			MemberInfo[] member = type.GetMember(name, GetBindingFlags(scope));
			if (member == null || member.Length == 0)
			{
				Type[] interfaces = type.GetInterfaces();
				foreach (Type type2 in interfaces)
				{
					member = type2.GetMember(name);
					if (member != null && member.Length != 0)
					{
						break;
					}
				}
			}
			FillTypeMembers(members, member, type, ((scope & StaticScope.TypeName) != 0) ? GetDataType(type, (scope & StaticScope.ShortType) != 0, members.UseHtmlFormatting) : string.Empty, name, scope);
		}
		else
		{
			MemberInfo[] constructors = type.GetConstructors();
			FillTypeMembers(members, constructors, type, GetDataType(type, members.UseHtmlFormatting), scope);
		}
	}

	protected virtual bool IsRestrictedType(Type type)
	{
		if (type != null)
		{
			return restrictedTypes.Contains(type);
		}
		return false;
	}

	protected virtual void FillTypeMembers(IListMembers members, Type type, StaticScope scope)
	{
		BindingFlags bindingFlags = GetBindingFlags(scope);
		string type2 = (((scope & StaticScope.TypeName) != 0 && type != typeof(BoolEnum)) ? GetDataType(type, useHtmlFormatting: true) : string.Empty);
		MemberInfo[] properties = type.GetProperties(bindingFlags);
		FillTypeMembers(members, properties, type, type2, string.Empty, scope);
		properties = type.GetMethods(bindingFlags);
		FillTypeMembers(members, properties, type, type2, string.Empty, scope);
		properties = type.GetEvents(bindingFlags);
		FillTypeMembers(members, properties, type, type2, string.Empty, scope);
		properties = type.GetFields(bindingFlags);
		FillTypeMembers(members, properties, type, type2, string.Empty, scope);
		properties = type.GetNestedTypes(bindingFlags);
		FillTypeMembers(members, properties, type, type2, string.Empty, scope);
		if (type.IsInterface)
		{
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type3 in interfaces)
			{
				FillTypeMembers(members, type3, scope);
			}
		}
	}

	protected virtual void FillTypeMembers(IListMembers provider, MemberInfo[] members, Type declaringType, string name, StaticScope scope)
	{
		FillTypeMembers(provider, members, declaringType, string.Empty, name, scope);
	}

	protected virtual void FillTypeMembers(IListMembers provider, MemberInfo[] members, Type declaringType, string type, string name, StaticScope scope)
	{
		foreach (MemberInfo memberInfo in members)
		{
			if ((!IsPrivateMember(memberInfo) || (memberInfo.DeclaringType == declaringType && (scope & StaticScope.Private) != 0)) && (memberInfo.DeclaringType == memberInfo.ReflectedType || !IsRestrictedType(memberInfo.DeclaringType)) && (name != string.Empty || !IsSpecialName(memberInfo)))
			{
				string text = ((name != string.Empty) ? name : memberInfo.Name);
				if (declaringType == typeof(BoolEnum) && base.CaseSensitive)
				{
					text = text.ToLower(CultureInfo.InvariantCulture);
				}
				if (type != string.Empty)
				{
					text = type + "." + text;
				}
				FillMember(provider, memberInfo, text);
			}
		}
	}

	protected virtual void FillMember(IListMembers members, MemberInfo info, string name)
	{
		int memberImageIndex = GetMemberImageIndex(info);
		if (CanAddMember(members, name, memberImageIndex, !(info is Type)))
		{
			IListMember listMember = AddMember(members, name, memberImageIndex, showDesc: true);
			listMember.Qualifier = GetMemberQualifier(info);
			listMember.DataType = GetMemberDataType(info, members.UseHtmlFormatting);
			if (info is Type)
			{
				listMember.OwnDescriptionText = listMember.DataType;
			}
			listMember.Parameters = GetMemberParameters(info, members.UseHtmlFormatting);
			listMember.Description = GetDescription(null, null, info, XmlCommentType.Summary.ToString(), fullDescription: false);
			listMember.UpdateParamText();
		}
	}

	protected override void DoFillMembers(ISyntaxNode node, Point position, IListMembers members, IList types, object member, string name, StaticScope scope, ref int selIndex)
	{
		base.DoFillMembers(node, position, members, types, member, name, scope, ref selIndex);
		if (member is Type)
		{
			if ((scope & StaticScope.Delegate) != 0)
			{
				FillDelegate(members, (Type)member, name);
			}
			else
			{
				FillTypeMembers(members, (Type)member, scope);
			}
		}
	}

	protected virtual void FillDelegate(IListMembers members, Type type, string name)
	{
		IListMember listMember = AddMember(members, string.Format(StringConsts.InsertDelegateHint, type.Name, name), -1, showDesc: true);
		listMember.DisplayText = string.Format(StringConsts.InsertDelegateExtendedHint, type.Name, name);
	}

	protected override void FillGlobalTypes(IListMembers members, IList namespaces)
	{
		base.FillGlobalTypes(members, namespaces);
		ArrayList arrayList = new ArrayList();
		foreach (INetNamespace @namespace in namespaces)
		{
			if (NetTypes.GetGlobalTypes(arrayList, @namespace.Namespace, base.CaseSensitive) <= 0)
			{
				continue;
			}
			foreach (Type item in arrayList)
			{
				FillTypeMembers(members, item, StaticScope.Static);
			}
		}
		FillInternalTypes(members);
	}

	protected virtual void FillInternalTypes(IListMembers members)
	{
		IDictionaryEnumerator enumerator = internalTypes.GetEnumerator();
		enumerator.Reset();
		while (enumerator.MoveNext())
		{
			string text = (string)enumerator.Value;
			if (text != string.Empty && !text.StartsWith("__"))
			{
				FillMember(members, (Type)enumerator.Key, text);
			}
		}
	}

	protected override void FillNamespaceTypes(IListMembers members, INetNamespace nspace, ISyntaxNode node)
	{
		base.FillNamespaceTypes(members, nspace, node);
		string name = nspace.GetName();
		foreach (Type value in NetTypes.Types.Values)
		{
			if (string.Compare(value.Namespace, name, !base.CaseSensitive) == 0 && !value.IsNestedPublic && !value.IsNestedPrivate && !value.IsNestedAssembly && !value.IsNestedFamily)
			{
				FillMember(members, value, value.Name);
			}
		}
	}

	protected virtual string GetTypeName(Type type, bool shortName, bool useGenericArgs, bool useHtmlFormatting)
	{
		string text = ((shortName || type.FullName == null) ? type.Name : type.FullName);
		if (type.IsGenericType && text != null)
		{
			int num = text.IndexOf('`');
			if (num >= 0)
			{
				text = text.Substring(0, num);
				string text2 = string.Empty;
				if (useGenericArgs)
				{
					Type[] genericArguments = type.GetGenericArguments();
					foreach (Type type2 in genericArguments)
					{
						text2 = ((text2 == string.Empty) ? string.Empty : (text2 + ",")) + GetTypeName(type2, shortName: true, useGenericArgs: true, useHtmlFormatting);
					}
				}
				text += (useHtmlFormatting ? ("&lt;" + text2 + "&gt;") : ("<" + text2 + ">"));
			}
		}
		return text;
	}

	protected override void FillNamespaceTypes(IListMembers members, IList namespaces)
	{
		base.FillNamespaceTypes(members, namespaces);
		foreach (Type value in NetTypes.Types.Values)
		{
			if (value.Namespace == null || value.Namespace == string.Empty || IndexOfNamespace(namespaces, value.Namespace) >= 0)
			{
				FillMember(members, value, GetTypeName(value, shortName: true, useGenericArgs: false, members.UseHtmlFormatting));
			}
		}
	}

	protected virtual object FindTypeDeclaration(Type type, string name, StaticScope scope)
	{
		MemberInfo[] member = type.GetMember(name, GetBindingFlags(scope));
		object obj = ((member != null && member.Length != 0) ? member[0] : null);
		if (obj == null && type.IsInterface)
		{
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				obj = FindTypeDeclaration(type2, name, scope);
				if (obj != null)
				{
					return obj;
				}
			}
		}
		return obj;
	}

	protected virtual object FindNamespaceDeclaration(INetNamespace nspace, ISyntaxNode node, string name)
	{
		string text = ((nspace != null) ? (nspace.GetName() + "." + name) : name);
		foreach (INetNamespace namespace2 in GetNamespaces(null, system: true))
		{
			string @namespace = namespace2.Namespace;
			if ((base.CaseSensitive ? @namespace.StartsWith(text) : @namespace.ToLower(CultureInfo.InvariantCulture).StartsWith(text.ToLower())) && (@namespace.Length == text.Length || @namespace[text.Length] == '.'))
			{
				return new NetNamespace(text, system: true);
			}
		}
		return GetTypeByName(node, (nspace != null) ? (nspace.GetName() + "." + name) : name);
	}

	protected override object GetExpressionType(ISyntaxNode node, Point position, string name, out StaticScope scope)
	{
		object expressionType = base.GetExpressionType(node, position, name, out scope);
		if (expressionType != null)
		{
			return expressionType;
		}
		ISyntaxNode declarationNode = NETRepository.GetDeclarationNode(node);
		if (declarationNode != null)
		{
			string baseType = GetBaseType(declarationNode);
			if (baseType != string.Empty)
			{
				expressionType = GetMemberType(declarationNode, declarationNode.Position, null, baseType, out scope);
				if (expressionType is Type)
				{
					MemberInfo[] member = ((Type)expressionType).GetMember(name, (BindingFlags)(0x1C | ((!base.CaseSensitive) ? 1 : 0)));
					if (member != null && member.Length != 0)
					{
						scope = StaticScope.Instance;
						return GetMemberType(member[0]);
					}
				}
			}
		}
		return null;
	}

	protected override object FindDeclaration(object member, IList types, Point position, string name, StaticScope scope, int paramCount)
	{
		object obj = base.FindDeclaration(member, types, position, name, scope, paramCount);
		if (obj == null)
		{
			if (member is ISyntaxNode)
			{
				ISyntaxNode node = (ISyntaxNode)member;
				if (!NETRepository.IsDeclarationNode(node) && !NETRepository.IsXmlCommentNode(node))
				{
					obj = FindNamespaceDeclaration(null, node, name);
				}
			}
			if (member is Type)
			{
				obj = FindTypeDeclaration((Type)member, name, scope);
			}
			else if (member is INetNamespace)
			{
				obj = FindNamespaceDeclaration((INetNamespace)member, null, name);
			}
		}
		return obj;
	}

	protected virtual void FillCastExpressionMember(IListMembers members, ISyntaxNode node)
	{
		if (CanAddMember(members, node.Name, 10, overloads: false))
		{
			IListMember listMember = AddMember(members, node.Name, 10, showDesc: false);
			listMember.Qualifier = "public";
			ISyntaxAttribute syntaxAttribute = node.FindAttribute("Type");
			if (syntaxAttribute != null)
			{
				listMember.DataType = (string)syntaxAttribute.Value;
			}
			IParameterMember[] parameters = new ParameterMember[(node.NodeType == 146) ? 1 : 2];
			listMember.Parameters = parameters;
			IParameterMember parameterMember = new ParameterMember();
			parameterMember.DataType = typeof(object).Name;
			parameterMember.Name = SyntaxConsts.ExpressionTag;
			listMember.Parameters[0] = parameterMember;
			if (listMember.Parameters.Length == 2)
			{
				parameterMember = new ParameterMember();
				parameterMember.DataType = typeof(object).Name;
				parameterMember.Name = SyntaxConsts.TypeTag;
				listMember.Parameters[1] = parameterMember;
			}
			listMember.UpdateParamText();
		}
	}

	public override object GetMemberType(ISyntaxNode node, Point position, object member, string name, out StaticScope scope)
	{
		object memberType = base.GetMemberType(node, position, member, name, out scope);
		if (memberType == null && member is Type)
		{
			MemberInfo[] member2 = ((Type)member).GetMember(name);
			if (member2 == null || member2.Length == 0)
			{
				Type[] interfaces = ((Type)member).GetInterfaces();
				foreach (Type type in interfaces)
				{
					member2 = type.GetMember(name);
					if (member2 != null && member2.Length != 0)
					{
						break;
					}
				}
			}
			if (member2 != null && member2.Length != 0)
			{
				MemberInfo memberInfo = member2[0];
				scope = ((memberInfo is Type) ? StaticScope.Static : StaticScope.Instance);
				if (memberInfo is MethodBase)
				{
					scope |= StaticScope.Method;
				}
				if (memberInfo is PropertyInfo)
				{
					scope |= StaticScope.Property;
				}
				if (memberInfo is FieldInfo)
				{
					scope |= StaticScope.Field;
				}
				memberType = GetMemberType(memberInfo);
			}
		}
		return memberType;
	}

	public override void FillMember(IListMembers members, object member, string name, StaticScope scope)
	{
		base.FillMember(members, member, name, scope);
		if (member is ISyntaxNode)
		{
			ISyntaxNode syntaxNode = member as ISyntaxNode;
			if ((scope & StaticScope.Global) != 0)
			{
				ArrayList arrayList = new ArrayList();
				foreach (INetNamespace @namespace in GetNamespaces(syntaxNode, system: false))
				{
					NetTypes.GetGlobalTypes(arrayList, @namespace.Namespace, base.CaseSensitive);
					foreach (Type item in arrayList)
					{
						FillTypeMembers(members, item, name, scope);
					}
				}
			}
			if (syntaxNode.NodeType == 146 || syntaxNode.NodeType == 145)
			{
				FillCastExpressionMember(members, syntaxNode);
			}
		}
		if (member is Type)
		{
			FillTypeMembers(members, (Type)member, name, scope);
		}
	}

	public override string GetDescription(IListMembers members, ISyntaxNode node, object member, string name, bool fullDescription)
	{
		string text = base.GetDescription(members, node, member, name, fullDescription);
		string text2 = string.Empty;
		if (member is MemberInfo)
		{
			text2 = DescriptionHelper.GetDescription((MemberInfo)member);
		}
		else if (member is System.Reflection.ParameterInfo)
		{
			text2 = DescriptionHelper.GetDescription((System.Reflection.ParameterInfo)member);
		}
		else if (member is INetNamespace)
		{
			text2 = SyntaxConsts.NamespaceDataType + " " + ((INetNamespace)member).Namespace;
		}
		if (text == null || text == string.Empty)
		{
			text = text2;
		}
		else if (text2 != string.Empty)
		{
			text = text + SyntaxConsts.DefaultBrTag + text2;
		}
		return text;
	}

	protected virtual string FixupArrayType(string name)
	{
		int num = name.IndexOf("[");
		if (num >= 0)
		{
			object registeredType = GetRegisteredType(name.Substring(0, num));
			if (registeredType != null)
			{
				return ((Type)registeredType).FullName + name.Substring(num);
			}
		}
		return name;
	}

	protected override object GetTypeByName(ISyntaxNode node, string name)
	{
		object typeByName = GetTypeByName(name);
		if (typeByName == null)
		{
			foreach (INetNamespace @namespace in GetNamespaces(node, system: false))
			{
				typeByName = GetTypeByName(@namespace.GetName() + "." + name);
				if (typeByName != null)
				{
					break;
				}
			}
		}
		return typeByName;
	}

	protected override bool IsEnumTypeRegistered()
	{
		return types.Contains("__enum");
	}

	protected override object GetEnumType(object member)
	{
		object obj = base.GetEnumType(member);
		if (obj == null && member is Type)
		{
			if (((Type)member).IsEnum)
			{
				obj = member;
			}
			else if (((Type)member).FullName == "System.Boolean")
			{
				obj = typeof(BoolEnum);
			}
		}
		return obj;
	}

	protected override IList GetNamespaces()
	{
		IList list = new ArrayList(base.Namespaces);
		foreach (string @namespace in NetTypes.GetNamespaces(assemblies))
		{
			list.Add(@namespace);
		}
		return list;
	}

	protected virtual bool FindAssembly(string name, out Assembly assembly)
	{
		Assembly[] array = AppDomain.CurrentDomain.GetAssemblies();
		foreach (Assembly assembly2 in array)
		{
			if (string.Compare(assembly2.GetName().Name, name, ignoreCase: true) == 0)
			{
				assembly = assembly2;
				return true;
			}
		}
		assembly = null;
		return false;
	}

	public virtual object GetRegisteredType(string name)
	{
		return types[base.CaseSensitive ? name : name.ToLower(CultureInfo.InvariantCulture)];
	}

	public override object GetTypeByName(string type)
	{
		if (type == null || type == string.Empty)
		{
			return null;
		}
		object registeredType = GetRegisteredType(type);
		if (registeredType == null)
		{
			return NetTypes.GetTypeByName(FixupArrayType(type), assemblies, base.CaseSensitive);
		}
		return registeredType;
	}

	public void RegisterType(string name, Type type)
	{
		RegisterType(name, type, global: false);
	}

	public void RegisterType(string name, Type type, bool global)
	{
		types[base.CaseSensitive ? name : name.ToLower(CultureInfo.InvariantCulture)] = type;
		internalTypes[type] = name;
		NetTypes.AddType(type, global);
		if (global && type.Namespace != null && type.Namespace != string.Empty && base.Namespaces.IndexOf(type.Namespace) < 0)
		{
			RegisterNamespace(type.Namespace);
		}
	}

	public bool UnregisterType(string name)
	{
		if (internalTypes.Contains(name))
		{
			internalTypes.Remove(name);
		}
		name = (base.CaseSensitive ? name : name.ToLower(CultureInfo.InvariantCulture));
		object obj = types[name];
		if (obj != null)
		{
			types.Remove(name);
			NetTypes.RemoveType((Type)obj);
		}
		return obj != null;
	}

	public virtual void RegisterAssembly(Assembly assembly)
	{
		assemblies.Add(assembly);
		NetTypes.AddAssembly(assembly);
	}

	public virtual bool UnregisterAssembly(Assembly assembly, bool removeReferences)
	{
		int num = assemblies.IndexOf(assembly);
		if (num >= 0)
		{
			assemblies.RemoveAt(num);
			if (removeReferences)
			{
				NetTypes.RemoveAssembly(assembly);
				DescriptionHelper.UnloadAssembly(assembly);
			}
		}
		return num >= 0;
	}

	public virtual bool RegisterAssembly(string name)
	{
		if (name == null || name == string.Empty)
		{
			return false;
		}
		Assembly assembly = null;
		if (!FindAssembly(name, out assembly))
		{
			FileInfo fileInfo = new FileInfo(name);
			assembly = ((!fileInfo.Exists) ? Assembly.Load(name) : Assembly.LoadFrom(name));
		}
		if (assembly != null)
		{
			RegisterAssembly(assembly);
		}
		return assembly != null;
	}

	public virtual bool UnregisterAssembly(string name, bool removeReferences)
	{
		if (!FindAssembly(name, out var assembly))
		{
			return false;
		}
		return UnregisterAssembly(assembly, removeReferences);
	}

	public void RegisterAllAssemblies()
	{
		Assembly[] array = AppDomain.CurrentDomain.GetAssemblies();
		foreach (Assembly assembly in array)
		{
			RegisterAssembly(assembly);
		}
	}

	public void RegisterDefaultAssemblies()
	{
		RegisterAssembly("mscorlib");
		RegisterAssembly("System");
		RegisterAssembly("System.Drawing");
		RegisterAssembly("System.Windows.Forms");
	}

	public virtual void ClearAssemblies()
	{
		assemblies.Clear();
	}

	public void ClearTypes()
	{
		types.Clear();
		internalTypes.Clear();
	}

	public void RestrictTypeMembers(Type type)
	{
		restrictedTypes.Add(type, type);
	}

	public void AllowTypeMembers(Type type)
	{
		if (restrictedTypes.Contains(type))
		{
			restrictedTypes.Remove(type);
		}
	}
}
