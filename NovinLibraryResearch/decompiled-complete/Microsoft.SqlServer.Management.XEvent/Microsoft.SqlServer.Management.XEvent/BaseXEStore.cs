using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.XEvent;

public abstract class BaseXEStore : SfcInstance, ISfcDomain, ISfcDomainLite, ISfcHasConnection
{
	public sealed class Key : DomainRootKey
	{
		public Key()
			: base(null)
		{
		}

		internal Key(ISfcDomain root)
			: base(root)
		{
		}

		public override int GetHashCode()
		{
			return GetType().GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public new static bool Equals(object obj1, object obj2)
		{
			return obj1 as Key == obj2 as Key;
		}

		public override bool Equals(SfcKey key)
		{
			return this == key;
		}

		public static bool operator ==(object obj, Key rightOperand)
		{
			if (obj == null || obj is Key)
			{
				return (Key)obj == rightOperand;
			}
			return false;
		}

		public static bool operator ==(Key leftOperand, object obj)
		{
			if (obj == null || obj is Key)
			{
				return leftOperand == (Key)obj;
			}
			return false;
		}

		public static bool operator ==(Key leftOperand, Key rightOperand)
		{
			if ((object)leftOperand == (object)rightOperand)
			{
				return true;
			}
			if ((object)leftOperand == null || (object)rightOperand == null)
			{
				return false;
			}
			return leftOperand.IsEqual(rightOperand);
		}

		public static bool operator !=(object obj, Key rightOperand)
		{
			return !(obj == rightOperand);
		}

		public static bool operator !=(Key leftOperand, object obj)
		{
			return !(leftOperand == obj);
		}

		public static bool operator !=(Key leftOperand, Key rightOperand)
		{
			return !(leftOperand == rightOperand);
		}

		private bool IsEqual(Key other)
		{
			return base.Domain == other.Domain;
		}

		public override string GetUrnFragment()
		{
			if (base.Domain.ConnectionContext.Mode == SfcConnectionContextMode.Offline)
			{
				return "XEStore";
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}[@Name='{1}']", "XEStore", base.Domain.DomainInstanceName);
		}
	}

	public class ObjectMetadata
	{
		private BaseXEStore store;

		public ObjectMetadata(BaseXEStore store)
		{
			this.store = store;
		}

		public ICollection<T> GetAll<T>(string name) where T : SfcInstance, IXEObjectInfo
		{
			string[] array = ValidateAndSplitName(name);
			return GetAll<T>(array[0], array[1]);
		}

		public ICollection<T> GetAll<T>(string pkgName, string objName) where T : SfcInstance, IXEObjectInfo
		{
			List<T> list = new List<T>();
			foreach (Package package in store.Packages.GetPackages(pkgName))
			{
				T val = package.GetChildCollection<T>()[objName];
				if (val != null)
				{
					list.Add(val);
				}
			}
			return list;
		}

		public T Get<T>(string name) where T : SfcInstance, IXEObjectInfo
		{
			string[] array = ValidateAndSplitName(name);
			if (name.StartsWith("[", StringComparison.Ordinal))
			{
				return Get<T>(new Guid(array[0].Substring(1, array[0].Length - 2)), array[1]);
			}
			return Get<T>(array[0], array[1]);
		}

		internal T Get<T>(string pkgName, string objName) where T : SfcInstance, IXEObjectInfo
		{
			ICollection<T> all = GetAll<T>(pkgName, objName);
			IEnumerator<T> enumerator = all.GetEnumerator();
			if (!enumerator.MoveNext())
			{
				if (!store.Packages.Contains(pkgName))
				{
					throw new XEventException(ExceptionTemplates.PackageNotExist(pkgName));
				}
				throw new XEventException(ExceptionTemplates.ObjectNotExist(pkgName + "." + objName));
			}
			if (all.Count > 1)
			{
				throw new XEventException(ExceptionTemplates.ObjectNameNotUnique(pkgName + "." + objName));
			}
			return enumerator.Current;
		}

		internal T Get<T>(Guid moduleID, string name) where T : SfcInstance, IXEObjectInfo
		{
			string[] array = ValidateAndSplitName(name);
			return Get<T>(moduleID, array[0], array[1]);
		}

		internal T Get<T>(Guid moduleID, string pkgName, string objName) where T : SfcInstance, IXEObjectInfo
		{
			return (store.Packages[moduleID, pkgName] ?? throw new XEventException(ExceptionTemplates.PackageNotExist(string.Format(CultureInfo.InstalledUICulture, "[{0}].{1}", moduleID, pkgName)))).GetChildCollection<T>()[objName] ?? throw new XEventException(ExceptionTemplates.ObjectNotExist(string.Format(CultureInfo.InstalledUICulture, "[{0}].{1}.{2}", moduleID, pkgName, objName)));
		}

		private string[] ValidateAndSplitName(string name)
		{
			using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("SplitName");
			if (name == null)
			{
				methodTraceContext.TraceError("the object name is null.");
				throw new ArgumentNullException("name");
			}
			string text = null;
			string text2 = null;
			string[] array = name.Split(new char[1] { '.' }, 2);
			switch (array.Length)
			{
			case 1:
				text = string.Empty;
				text2 = array[0];
				break;
			case 2:
				text = array[0];
				text2 = array[1];
				break;
			default:
				methodTraceContext.Assert(false, "unexpected number of parts");
				break;
			}
			return new string[2] { text, text2 };
		}
	}

	private const string NameSpace = "http://schemas.microsoft.com/sqlserver/2008/07/extendedeventsconfig";

	private const string DomainName = "XEvent";

	private const string typeName = "XEStore";

	private static Guid package0Guid = new Guid("60AA9FBF-673B-4553-B7ED-71DCA7F5E972");

	private ISfcConnection originalConnection;

	private ISfcConnection connection;

	private SfcConnectionContext context;

	private IComparer<string> comparer;

	private SessionCollection sessions;

	private PackageCollection packages;

	private IXEStoreProvider storeProvider;

	private SfcObjectQuery objectQuery;

	private ISfcExecutionEngine execEngine;

	private ObjectMetadata objectInfoSet;

	public virtual string Name
	{
		get
		{
			if (SfcConnection == null)
			{
				return null;
			}
			return SfcConnection.ServerInstance;
		}
		protected set
		{
		}
	}

	public virtual string ServerName
	{
		get
		{
			if (SfcConnection == null)
			{
				return null;
			}
			return SfcConnection.ServerInstance;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.ZeroToAny, typeof(Session))]
	public SessionCollection Sessions
	{
		get
		{
			if (sessions == null)
			{
				sessions = new SessionCollection(this);
			}
			return sessions;
		}
	}

	[SfcObject(SfcContainerRelationship.ChildContainer, SfcContainerCardinality.OneToAny, typeof(Package))]
	public PackageCollection Packages
	{
		get
		{
			if (packages == null)
			{
				packages = new PackageCollection(this);
			}
			return packages;
		}
	}

	[SfcProperty(Data = true)]
	public int RunningSessionCount => (int)base.Properties["RunningSessionCount"].Value;

	[SfcIgnore]
	public Package Package0Package => Packages["package0"];

	[SfcIgnore]
	public TargetInfo HistogramTargetInfo => Packages["package0"].TargetInfoSet["histogram"];

	[SfcIgnore]
	public TargetInfo EventFileTargetInfo => Packages["package0"].TargetInfoSet["event_file"];

	[SfcIgnore]
	public TargetInfo EtwClassicSyncTargetInfo => Packages["package0"].TargetInfoSet["etw_classic_sync_target"];

	[SfcIgnore]
	public TargetInfo PairMatchingTargetInfo => Packages["package0"].TargetInfoSet["pair_matching"];

	[SfcIgnore]
	public TargetInfo RingBufferTargetInfo => Packages["package0"].TargetInfoSet["ring_buffer"];

	[SfcIgnore]
	public TargetInfo EventCounterTargetInfo => Packages["package0"].TargetInfoSet["event_counter"];

	SfcConnectionContext ISfcHasConnection.ConnectionContext
	{
		get
		{
			if (context == null)
			{
				context = new SfcConnectionContext(this);
			}
			return context;
		}
	}

	[SfcIgnore]
	public virtual SfcKey IdentityKey => (Key)base.AbstractIdentityKey;

	public ISfcConnection OriginalConnection
	{
		get
		{
			return originalConnection;
		}
		protected set
		{
			originalConnection = value;
		}
	}

	[SfcIgnore]
	public ISfcConnection SfcConnection => connection;

	[SfcIgnore]
	internal SfcObjectQuery SfcObjectQuery
	{
		get
		{
			if (objectQuery == null)
			{
				objectQuery = new SfcObjectQuery(this);
			}
			return objectQuery;
		}
	}

	protected ISfcExecutionEngine ExecutionEngine => execEngine;

	[SfcIgnore]
	string ISfcDomainLite.DomainName => "XEvent";

	[SfcIgnore]
	string ISfcDomainLite.DomainInstanceName
	{
		get
		{
			if (((ISfcHasConnection)this).ConnectionContext.Mode == SfcConnectionContextMode.Offline)
			{
				return "XEvent";
			}
			return StoreProvider.DomainInstanceName;
		}
	}

	protected IXEStoreProvider StoreProvider
	{
		get
		{
			if (storeProvider == null)
			{
				storeProvider = GetStoreProvider();
			}
			return storeProvider;
		}
	}

	public ObjectMetadata ObjectInfoSet
	{
		get
		{
			if (objectInfoSet == null)
			{
				objectInfoSet = new ObjectMetadata(this);
			}
			return objectInfoSet;
		}
	}

	protected abstract IXEStoreProvider GetStoreProvider();

	protected abstract ISessionProvider GetSessionProivder(Session session);

	protected abstract ITargetProvider GetTargetProvider(Target target);

	protected abstract IEventProvider GetEventProvider(Event xevent);

	internal ISessionProvider GetSessionProviderInternal(Session session)
	{
		return GetSessionProivder(session);
	}

	internal ITargetProvider GetTargetProviderInternal(Target target)
	{
		return GetTargetProvider(target);
	}

	internal IEventProvider GetEventProviderInternal(Event xevent)
	{
		return GetEventProvider(xevent);
	}

	public BaseXEStore(ISfcConnection connection)
		: this()
	{
		TraceHelper.TraceContext.TraceParameterIn("Connection", connection);
		if (connection == null)
		{
			throw new ArgumentNullException("connection");
		}
		originalConnection = connection;
		InitConnection();
	}

	protected BaseXEStore()
	{
	}

	ISfcConnection ISfcHasConnection.GetConnection()
	{
		return SfcConnection;
	}

	void ISfcHasConnection.SetConnection(ISfcConnection connection)
	{
		SetConnection(connection);
	}

	protected void SetConnection(ISfcConnection connection)
	{
		ISfcConnection sfcConnection = connection as SqlStoreConnection;
		if (sfcConnection == null)
		{
			sfcConnection = connection as ServerConnection;
		}
		if (sfcConnection == null)
		{
			throw new ArgumentException("", "connection");
		}
		originalConnection = sfcConnection;
		storeProvider = GetStoreProvider();
		InitConnection();
	}

	ISfcConnection ISfcHasConnection.GetConnection(SfcObjectQueryMode mode)
	{
		return StoreProvider.GetConnection(mode);
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} (Server='{1}')", "XEStore", (SfcConnection != null) ? SfcSecureString.EscapeSquote(SfcConnection.ServerInstance) : "");
	}

	protected override SfcKey CreateIdentityKey()
	{
		return new Key(this);
	}

	protected override ISfcCollection GetChildCollection(string elementType)
	{
		TraceHelper.TraceContext.TraceParameterIn("elementType", elementType);
		if (!(elementType == "Session"))
		{
			if (elementType == "Package")
			{
				return Packages;
			}
			TraceHelper.TraceContext.TraceError("No such collection for type {0}", elementType);
			throw new XEventException(ExceptionTemplates.NoSuchCollection(elementType));
		}
		return Sessions;
	}

	public IComparer<string> GetComparer()
	{
		return comparer;
	}

	protected void InitConnection()
	{
		comparer = StoreProvider.GetComparer();
		execEngine = StoreProvider.GetExecutionEngine();
		connection = StoreProvider.GetConnection(SfcObjectQueryMode.SingleActiveQuery);
		try
		{
			MarkRootAsConnected();
		}
		catch (InvalidVersionEnumeratorException innerException)
		{
			throw new XEventException(ExceptionTemplates.InvalidVersion(connection.ServerVersion.ToString()), innerException);
		}
	}

	Type ISfcDomain.GetType(string typeName)
	{
		return null;
	}

	SfcKey ISfcDomain.GetKey(IUrnFragment urnFragment)
	{
		return null;
	}

	ISfcExecutionEngine ISfcDomain.GetExecutionEngine()
	{
		return execEngine;
	}

	SfcTypeMetadata ISfcDomain.GetTypeMetadata(string typeName)
	{
		return null;
	}

	bool ISfcDomain.UseSfcStateManagement()
	{
		return true;
	}

	int ISfcDomainLite.GetLogicalVersion()
	{
		return 3;
	}

	public virtual string FormatFieldValue(string fieldValue, Guid typePackageID, string typeName)
	{
		int result;
		if (Packages[typePackageID].MapInfoSet.Contains(typeName))
		{
			return string.Format("({0}{1}{0})", int.TryParse(fieldValue, out result) ? "" : "'", fieldValue);
		}
		if (typePackageID.Equals(package0Guid))
		{
			switch (typeName)
			{
			case "boolean":
				if (!fieldValue.Equals("true", StringComparison.OrdinalIgnoreCase) && !(fieldValue == "1"))
				{
					return "(0)";
				}
				return "(1)";
			case "activity_id":
			case "activity_id_xfer":
			case "cpu_cycle":
			case "float32":
			case "float64":
			case "int8":
			case "int16":
			case "int32":
			case "int64":
			case "uint8":
			case "uint16":
			case "uint32":
			case "uint64":
			case "ptr":
				return "(" + fieldValue + ")";
			case "ansi_string":
			case "ansi_string_ptr":
			case "callstack":
			case "char":
			case "guid":
			case "guid_ptr":
			case "filetime":
			case "sos_context":
				return "'" + SfcTsqlProcFormatter.EscapeString(fieldValue, '\'') + "'";
			case "binary_data":
				if (fieldValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
				{
					return fieldValue;
				}
				return "0x" + fieldValue;
			case "unicode_string":
			case "unicode_string_ptr":
			case "wchar":
			case "xml":
				return "N'" + SfcTsqlProcFormatter.EscapeString(fieldValue, '\'') + "'";
			}
		}
		throw new XEventException(ExceptionTemplates.UnknownType, new ArgumentOutOfRangeException("typeName", typeName, "Unknown type."));
	}

	public virtual string FormatPredicateExpression(PredExpr predExpr)
	{
		switch (predExpr.GetType().Name)
		{
		case "PredLogicalExpr":
		{
			PredLogicalExpr predLogicalExpr = (PredLogicalExpr)predExpr;
			switch (predLogicalExpr.Operator)
			{
			case PredLogicalExpr.LogicalOperatorType.Not:
				return string.Format(CultureInfo.InvariantCulture, "(NOT {0})", FormatPredicateExpression(predLogicalExpr.LeftExpr));
			case PredLogicalExpr.LogicalOperatorType.And:
				return string.Format(CultureInfo.InvariantCulture, "({0} AND {1})", FormatPredicateExpression(predLogicalExpr.LeftExpr), FormatPredicateExpression(predLogicalExpr.RightExpr));
			case PredLogicalExpr.LogicalOperatorType.Or:
				return string.Format(CultureInfo.InvariantCulture, "({0} OR {1})", FormatPredicateExpression(predLogicalExpr.LeftExpr), FormatPredicateExpression(predLogicalExpr.RightExpr));
			}
			break;
		}
		case "PredFunctionExpr":
		{
			PredFunctionExpr predFunctionExpr = (PredFunctionExpr)predExpr;
			string text = predFunctionExpr.Operand.ToString();
			string text2 = FormatFieldValue(predFunctionExpr.Value.ToString(), predFunctionExpr.Operand.TypePackageId, predFunctionExpr.Operand.TypeName);
			if (ObjectInfoSet.GetAll<PredCompareInfo>(predFunctionExpr.Operator.Parent.Name, predFunctionExpr.Operator.Name).Count > 1)
			{
				return string.Format(CultureInfo.InvariantCulture, "([{4}].{0}.{1}({2},{3}))", SfcTsqlProcFormatter.MakeSqlBracket(predFunctionExpr.Operator.Parent.Name), SfcTsqlProcFormatter.MakeSqlBracket(predFunctionExpr.Operator.Name), text, text2, predFunctionExpr.Operator.Parent.ModuleID.ToString());
			}
			return string.Format(CultureInfo.InvariantCulture, "({0}.{1}({2},{3}))", SfcTsqlProcFormatter.MakeSqlBracket(predFunctionExpr.Operator.Parent.Name), SfcTsqlProcFormatter.MakeSqlBracket(predFunctionExpr.Operator.Name), text, text2);
		}
		case "PredCompareExpr":
		{
			PredCompareExpr predCompareExpr = (PredCompareExpr)predExpr;
			StringBuilder stringBuilder = new StringBuilder(50);
			stringBuilder.Append("(");
			stringBuilder.Append(predCompareExpr.Operand.ToString());
			switch (predCompareExpr.Operator)
			{
			case PredCompareExpr.ComparatorType.EQ:
				stringBuilder.Append("=");
				break;
			case PredCompareExpr.ComparatorType.NE:
				stringBuilder.Append("<>");
				break;
			case PredCompareExpr.ComparatorType.GT:
				stringBuilder.Append(">");
				break;
			case PredCompareExpr.ComparatorType.GE:
				stringBuilder.Append(">=");
				break;
			case PredCompareExpr.ComparatorType.LT:
				stringBuilder.Append("<");
				break;
			case PredCompareExpr.ComparatorType.LE:
				stringBuilder.Append("<=");
				break;
			}
			stringBuilder.Append(FormatFieldValue(predCompareExpr.Value.ToString(), predCompareExpr.Operand.TypePackageId, predCompareExpr.Operand.TypeName));
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
		}
		TraceHelper.TraceContext.Assert(false, "Unrecognized PredExpr");
		return null;
	}

	public static void SaveSessionToTemplate(Session session, string fileName, bool overwrite)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("SaveSessionToTemplate");
		methodTraceContext.TraceParameterIn("session", session);
		methodTraceContext.TraceParameterIn("fileName", fileName);
		methodTraceContext.TraceParameterIn("overwrite", overwrite);
		if (string.IsNullOrEmpty(fileName))
		{
			throw new XEventException(ExceptionTemplates.InvalidParameter(fileName));
		}
		if (!overwrite && File.Exists(fileName))
		{
			throw new XEventException(ExceptionTemplates.FileAlreadyExists);
		}
		if (session.State != SfcObjectState.Existing)
		{
			throw new XEventException(ExceptionTemplates.SaveTemplateForExistingSession);
		}
		XmlDocument xmlDocument = new XmlDocument();
		XmlElement xmlElement = xmlDocument.CreateElement("event_sessions");
		XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("xmlns");
		xmlAttribute.InnerText = "http://schemas.microsoft.com/sqlserver/2008/07/extendedeventsconfig";
		xmlElement.Attributes.Append(xmlAttribute);
		XmlElement xmlElement2 = xmlDocument.CreateElement("event_session");
		XmlAttribute xmlAttribute2 = xmlDocument.CreateAttribute("name");
		xmlAttribute2.InnerText = session.Name;
		xmlElement2.Attributes.Append(xmlAttribute2);
		XmlAttribute xmlAttribute3 = xmlDocument.CreateAttribute("maxMemory");
		xmlAttribute3.InnerText = (session.MaxMemory / 1024).ToString(CultureInfo.InvariantCulture);
		xmlElement2.Attributes.Append(xmlAttribute3);
		XmlAttribute xmlAttribute4 = xmlDocument.CreateAttribute("eventRetentionMode");
		xmlAttribute4.InnerText = XEUtils.ConvertToXsdEnumerationValue(session.EventRetentionMode);
		xmlElement2.Attributes.Append(xmlAttribute4);
		XmlAttribute xmlAttribute5 = xmlDocument.CreateAttribute("trackCausality");
		xmlAttribute5.InnerText = (session.TrackCausality ? "true" : "false");
		xmlElement2.Attributes.Append(xmlAttribute5);
		XmlAttribute xmlAttribute6 = xmlDocument.CreateAttribute("dispatchLatency");
		xmlAttribute6.InnerText = session.MaxDispatchLatency.ToString(CultureInfo.InvariantCulture);
		xmlElement2.Attributes.Append(xmlAttribute6);
		XmlAttribute xmlAttribute7 = xmlDocument.CreateAttribute("maxEventSize");
		xmlAttribute7.InnerText = (session.MaxEventSize / 1024).ToString(CultureInfo.InvariantCulture);
		xmlElement2.Attributes.Append(xmlAttribute7);
		XmlAttribute xmlAttribute8 = xmlDocument.CreateAttribute("memoryPartitionMode");
		xmlAttribute8.InnerText = XEUtils.ConvertToXsdEnumerationValue(session.MemoryPartitionMode);
		xmlElement2.Attributes.Append(xmlAttribute8);
		foreach (Event @event in session.Events)
		{
			XmlElement xmlElement3 = xmlDocument.CreateElement("event");
			AppendObjectAttributes<EventInfo>(session, xmlDocument, xmlElement3, @event.ModuleID, @event.PackageName, @event.Name);
			foreach (Action action in @event.Actions)
			{
				XmlElement xmlElement4 = xmlDocument.CreateElement("action");
				AppendObjectAttributes<ActionInfo>(session, xmlDocument, xmlElement4, action.ModuleID, action.PackageName, action.Name);
				xmlElement3.AppendChild(xmlElement4);
			}
			foreach (EventField eventField in @event.EventFields)
			{
				if (eventField.Value != null)
				{
					XmlElement xmlElement5 = xmlDocument.CreateElement("parameter");
					AppendParameterAttributes(xmlDocument, xmlElement5, eventField.Name, eventField.Value);
					xmlElement3.AppendChild(xmlElement5);
				}
			}
			string predicateXml = @event.PredicateXml;
			if (predicateXml != null && predicateXml.Trim() != string.Empty)
			{
				XmlElement xmlElement6 = xmlDocument.CreateElement("predicate");
				xmlElement6.InnerXml = predicateXml;
				xmlElement3.AppendChild(xmlElement6);
			}
			xmlElement2.AppendChild(xmlElement3);
		}
		foreach (Target target in session.Targets)
		{
			XmlElement xmlElement7 = xmlDocument.CreateElement("target");
			AppendObjectAttributes<TargetInfo>(session, xmlDocument, xmlElement7, target.ModuleID, target.PackageName, target.Name);
			foreach (TargetField targetField in target.TargetFields)
			{
				if (targetField.Value != null)
				{
					XmlElement xmlElement8 = xmlDocument.CreateElement("parameter");
					AppendParameterAttributes(xmlDocument, xmlElement8, targetField.Name, targetField.Value);
					xmlElement7.AppendChild(xmlElement8);
				}
			}
			xmlElement2.AppendChild(xmlElement7);
		}
		xmlElement.AppendChild(xmlElement2);
		xmlDocument.AppendChild(xmlElement);
		XmlTextWriter xmlTextWriter = new XmlTextWriter(fileName, Encoding.UTF8);
		xmlDocument.WriteTo(xmlTextWriter);
		xmlTextWriter.Close();
	}

	public virtual Session CreateSession(string sessionName)
	{
		return new Session(this, sessionName);
	}

	public virtual Session CreateSessionFromTemplate(string sessionName, string fileName)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("CreateSessionFromTemplate");
		methodTraceContext.TraceParameterIn("sessionName", sessionName);
		methodTraceContext.TraceParameterIn("fileName", fileName);
		if (string.IsNullOrEmpty(sessionName))
		{
			throw new XEventException(ExceptionTemplates.InvalidParameter(sessionName));
		}
		if (!File.Exists(fileName))
		{
			throw new XEventException(ExceptionTemplates.FileNotExist);
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(fileName);
		XmlSchema schema = XmlSchema.Read(new StringReader(XsdResource.xeconfig), null);
		xmlDocument.Schemas.Add(schema);
		xmlDocument.Validate(null);
		XmlElement xmlElement = (XmlElement)((XmlElement)xmlDocument.FirstChild).FirstChild;
		Session session = new Session(this, sessionName);
		session.MaxMemory = int.Parse(xmlElement.Attributes["maxMemory"].InnerText, CultureInfo.InvariantCulture) * 1024;
		session.EventRetentionMode = (Session.EventRetentionModeEnum)Enum.Parse(typeof(Session.EventRetentionModeEnum), xmlElement.Attributes["eventRetentionMode"].InnerText, ignoreCase: true);
		session.TrackCausality = ((xmlElement.Attributes["trackCausality"].InnerText == "true" || xmlElement.Attributes["trackCausality"].InnerText == "1") ? true : false);
		session.MaxDispatchLatency = int.Parse(xmlElement.Attributes["dispatchLatency"].InnerText, CultureInfo.InvariantCulture);
		session.MaxEventSize = int.Parse(xmlElement.Attributes["maxEventSize"].InnerText, CultureInfo.InvariantCulture) * 1024;
		session.MemoryPartitionMode = (Session.MemoryPartitionModeEnum)Enum.Parse(typeof(Session.MemoryPartitionModeEnum), xmlElement.Attributes["memoryPartitionMode"].InnerText, ignoreCase: true);
		foreach (XmlNode childNode in xmlElement.ChildNodes)
		{
			if (childNode.Name == "event")
			{
				XmlElement xmlElement2 = (XmlElement)childNode;
				string text = xmlElement2.Attributes["package"].InnerText + "." + xmlElement2.Attributes["name"].InnerText;
				if (xmlElement2.Attributes["module"] != null)
				{
					text = string.Format(CultureInfo.InvariantCulture, "[{0}].{1}", xmlElement2.Attributes["module"].InnerText, text);
				}
				Event @event = session.AddEvent(text);
				foreach (XmlNode childNode2 in xmlElement2.ChildNodes)
				{
					if (childNode2.Name == "action")
					{
						XmlElement xmlElement3 = (XmlElement)childNode2;
						string text2 = xmlElement3.Attributes["package"].InnerText + "." + xmlElement3.Attributes["name"].InnerText;
						if (xmlElement3.Attributes["module"] != null)
						{
							text2 = string.Format(CultureInfo.InvariantCulture, "[{0}].{1}", xmlElement3.Attributes["module"].InnerText, text2);
						}
						@event.AddAction(text2);
					}
					else if (childNode2.Name == "parameter")
					{
						XmlElement xmlElement4 = (XmlElement)childNode2;
						string innerText = xmlElement4.Attributes["name"].InnerText;
						if (!ObjectInfoSet.Get<EventInfo>(text).EventColumnInfoSet.Contains(innerText))
						{
							throw new XEventException(ExceptionTemplates.InvalidParameter(innerText));
						}
						@event.EventFields[innerText].Value = xmlElement4.Attributes["value"].InnerText;
					}
					else if (childNode2.Name == "predicate")
					{
						PredExpr predicate = PredExpr.ParsePredicateXml(this, ((XmlElement)childNode2).InnerXml);
						@event.Predicate = predicate;
					}
				}
			}
			else
			{
				if (!(childNode.Name == "target"))
				{
					continue;
				}
				XmlElement xmlElement5 = (XmlElement)childNode;
				string text3 = xmlElement5.Attributes["package"].InnerText + "." + xmlElement5.Attributes["name"].InnerText;
				if (xmlElement5.Attributes["module"] != null)
				{
					text3 = string.Format(CultureInfo.InvariantCulture, "[{0}].{1}", xmlElement5.Attributes["module"].InnerText, text3);
				}
				Target target = session.AddTarget(text3);
				foreach (XmlNode childNode3 in xmlElement5.ChildNodes)
				{
					string innerText2 = childNode3.Attributes["name"].InnerText;
					if (!ObjectInfoSet.Get<TargetInfo>(text3).TargetColumnInfoSet.Contains(innerText2))
					{
						throw new XEventException(ExceptionTemplates.InvalidParameter(innerText2));
					}
					target.TargetFields[innerText2].Value = childNode3.Attributes["value"].InnerText;
				}
			}
		}
		return session;
	}

	private static void AppendObjectAttributes<T>(Session session, XmlDocument doc, XmlElement element, Guid moduleID, string packageName, string objFullName) where T : SfcInstance, IXEObjectInfo
	{
		if (session.Parent.ObjectInfoSet.GetAll<T>(objFullName).Count > 1)
		{
			XmlAttribute xmlAttribute = doc.CreateAttribute("module");
			xmlAttribute.InnerText = moduleID.ToString("D", CultureInfo.InvariantCulture);
			element.Attributes.Append(xmlAttribute);
		}
		XmlAttribute xmlAttribute2 = doc.CreateAttribute("package");
		xmlAttribute2.InnerText = packageName;
		element.Attributes.Append(xmlAttribute2);
		XmlAttribute xmlAttribute3 = doc.CreateAttribute("name");
		xmlAttribute3.InnerText = objFullName.Substring(objFullName.LastIndexOf('.') + 1);
		element.Attributes.Append(xmlAttribute3);
	}

	private static void AppendParameterAttributes(XmlDocument doc, XmlElement element, string name, object value)
	{
		XmlAttribute xmlAttribute = doc.CreateAttribute("name");
		xmlAttribute.InnerText = name;
		element.Attributes.Append(xmlAttribute);
		XmlAttribute xmlAttribute2 = doc.CreateAttribute("value");
		if (value is bool)
		{
			xmlAttribute2.InnerText = (((bool)value) ? "1" : "0");
		}
		else
		{
			xmlAttribute2.InnerText = value.ToString();
		}
		element.Attributes.Append(xmlAttribute2);
	}
}
