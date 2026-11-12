using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Xml;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

public sealed class RegistrationProvider
{
	public enum RegistrationProviderStore
	{
		Default,
		SQLServerManagementStudio,
		ReplicationMonitor
	}

	private static RegistrationProviderStore _ProviderStore = RegistrationProviderStore.Default;

	private static string providerStoreCredentialVersion = "18";

	public const string FileExtension = "regsrvr";

	private const string regSqlServerFileName = "RegSrvr.xml";

	private const string regReplServerFileName = "RegReplSrvr.xml";

	private static volatile RegistrationProvider singleton;

	private static object syncRoot = new object();

	private ParentRegistrationInfo root;

	private HybridDictionary serverTypes;

	private bool saveDisabled;

	private const int GroupImageIndex = 0;

	private const int ServerImageIndex = 1;

	public static RegistrationProviderStore ProviderStore
	{
		set
		{
			if (_ProviderStore != 0)
			{
				throw new InvalidOperationException();
			}
			_ProviderStore = value;
		}
	}

	public static string ProviderStoreVersion { get; set; }

	public static string ProviderStoreCredentialVersion
	{
		get
		{
			return providerStoreCredentialVersion;
		}
		set
		{
			providerStoreCredentialVersion = value;
		}
	}

	private static RegistrationProvider Instance
	{
		get
		{
			if (singleton == null)
			{
				lock (syncRoot)
				{
					if (singleton == null)
					{
						singleton = new RegistrationProvider();
						if (_ProviderStore == RegistrationProviderStore.Default)
						{
							_ProviderStore = RegistrationProviderStore.SQLServerManagementStudio;
						}
						singleton.Load();
					}
				}
			}
			return singleton;
		}
	}

	public static bool SaveIsDisabled
	{
		get
		{
			return Instance.saveDisabled;
		}
		set
		{
			bool num = Instance.saveDisabled;
			Instance.saveDisabled = value;
			if (num && !value)
			{
				Instance.Save();
			}
		}
	}

	public static ParentRegistrationInfo RootNode => Instance.root;

	public static object SyncRoot => syncRoot;

	public static event RegistrationEventHandler AddedNode;

	public static event RegistrationEventHandler RemovedNode;

	public static event RegistrationEventHandler NodeModified;

	public static void Export(RegistrationInfo reg, string filename, bool includeNames)
	{
		if (reg == null)
		{
			throw new ArgumentNullException("reg");
		}
		try
		{
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(new StreamWriter(File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.None)));
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument();
			xmlTextWriter.WriteStartElement("Export", null);
			xmlTextWriter.WriteAttributeString("serverType", reg.ServerType.ToString());
			reg.SaveNodeToXml(xmlTextWriter, includeNames);
			xmlTextWriter.WriteEndElement();
		}
		catch (NotSupportedException previous)
		{
			throw new RegisteredServerException(SRError.PathNotSupported(filename), previous);
		}
		catch (DirectoryNotFoundException previous2)
		{
			throw new RegisteredServerException(SRError.DirectoryNotFound(filename), previous2);
		}
		catch (UnauthorizedAccessException previous3)
		{
			throw new RegisteredServerException(SRError.AccessDenied(filename), previous3);
		}
		catch (Exception previous4)
		{
			throw new RegisteredServerException(SRError.ErrUnableToExport(filename), previous4);
		}
	}

	public static RegistrationInfo Import(string fileName)
	{
		if (fileName == null)
		{
			throw new ArgumentNullException("fileName");
		}
		using FileStream input = File.OpenRead(fileName);
		XmlTextReader xmlTextReader = new XmlTextReader(input);
		xmlTextReader.DtdProcessing = DtdProcessing.Prohibit;
		xmlTextReader.MoveToContent();
		return RegistrationInfo.LoadNodeFromXml(new XmlTextReader(new StringReader(xmlTextReader.ReadInnerXml()))
		{
			DtdProcessing = DtdProcessing.Prohibit
		});
	}

	private void Save()
	{
		if (saveDisabled)
		{
			return;
		}
		Utils.EnsureSettingsDirExists(ProviderStoreVersion ?? "90");
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StreamWriter(File.Open(GetProviderFilePath(), FileMode.Create, FileAccess.Write, FileShare.None)))
		{
			IndentChar = '\t',
			Indentation = 1,
			Formatting = Formatting.Indented
		};
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("RegisteredServers");
		try
		{
			foreach (RegistrationInfo child in RootNode.Children)
			{
				child.SaveNodeToXml(xmlTextWriter, saveName: true);
			}
			xmlTextWriter.WriteEndElement();
		}
		catch (Exception previous)
		{
			throw new RegisteredServerException(SRError.NotSaveRegisteredServers, previous);
		}
		finally
		{
			xmlTextWriter.Close();
		}
	}

	private void Load()
	{
		new ArrayList();
		XmlTextReader xmlTextReader = null;
		try
		{
			string providerFilePath = GetProviderFilePath();
			if (!File.Exists(providerFilePath))
			{
				return;
			}
			xmlTextReader = new XmlTextReader(new StreamReader(File.OpenRead(providerFilePath)))
			{
				DtdProcessing = DtdProcessing.Prohibit,
				WhitespaceHandling = WhitespaceHandling.None
			};
			while (xmlTextReader.Read())
			{
				if (xmlTextReader.NodeType != XmlNodeType.Element || !(xmlTextReader.LocalName == "RegisteredServers"))
				{
					continue;
				}
				RegistrationInfo registrationInfo = null;
				xmlTextReader.Read();
				while ((registrationInfo = RegistrationInfo.LoadNodeFromXml(xmlTextReader)) != null)
				{
					if (!(registrationInfo is ServerTypeRegistrationInfo))
					{
						throw new RegisteredServerException(SRError.RegSvrDatafileInvalid(providerFilePath));
					}
					AddServerTypeNode(registrationInfo as ServerTypeRegistrationInfo);
				}
			}
		}
		catch (XmlException)
		{
		}
		catch (FileNotFoundException)
		{
		}
		catch (DirectoryNotFoundException)
		{
		}
		finally
		{
			xmlTextReader?.Close();
		}
	}

	public static string GetProviderFilePath()
	{
		string path = ((_ProviderStore == RegistrationProviderStore.ReplicationMonitor) ? "RegReplSrvr.xml" : "RegSrvr.xml");
		return Path.Combine(Utils.GetYukonSettingsDirName(ProviderStoreVersion ?? "90"), path);
	}

	public static void Refresh()
	{
		lock (syncRoot)
		{
			Instance.root.Children.Clear();
			Instance.serverTypes.Clear();
			Instance.Load();
		}
	}

	public static bool ServerTypeNodeExists(Guid serverType)
	{
		return Instance.serverTypes.Contains(serverType);
	}

	public static void MarkNodeModified(RegistrationInfo node)
	{
		if (RegistrationProvider.NodeModified != null)
		{
			RegistrationProvider.NodeModified(node, new RegistrationEventArgs(node));
		}
		Instance.Save();
	}

	public static RegistrationInfo AddServerTypeNode(ServerTypeRegistrationInfo node)
	{
		RegistrationInfo registrationInfo = null;
		lock (syncRoot)
		{
			return Instance.AddServerTypeNodeInternal(node);
		}
	}

	public static RegistrationInfo AddGroupNode(string name, string desc, ParentRegistrationInfo parent, RegistrationAddBehavior behavior)
	{
		RegistrationInfo registrationInfo = null;
		lock (syncRoot)
		{
			registrationInfo = Instance.AddGroupNodeInternal(name, desc, parent, behavior);
		}
		if (registrationInfo != null)
		{
			OnAddedNode(registrationInfo);
		}
		return registrationInfo;
	}

	public static RegistrationInfo AddNode(RegistrationInfo reg, ParentRegistrationInfo parent, RegistrationAddBehavior behavior)
	{
		RegistrationInfo registrationInfo = null;
		lock (syncRoot)
		{
			registrationInfo = Instance.AddNodeInternal(reg, parent, behavior);
		}
		if (registrationInfo != null)
		{
			OnAddedNode(registrationInfo);
		}
		return registrationInfo;
	}

	public static void RemoveNode(RegistrationInfo reg, ParentRegistrationInfo parent)
	{
		lock (syncRoot)
		{
			Instance.RemoveNodeInternal(reg, parent);
		}
		OnRemovedNode(reg);
	}

	public static bool MoveNode(RegistrationInfo reg, ParentRegistrationInfo newParent, RegistrationAddBehavior behavior)
	{
		if (reg == newParent)
		{
			throw new RegisteredServerException(SRError.ErrCannotMoveToSelf);
		}
		foreach (RegistrationInfo ancestor in newParent.Ancestors)
		{
			if (ancestor == reg)
			{
				throw new RegisteredServerException(SRError.RecursiveMove);
			}
		}
		lock (syncRoot)
		{
			ParentRegistrationInfo parent = reg.Parent;
			if (parent == newParent)
			{
				return true;
			}
			if (newParent.HasChildWithName(reg.FriendlyName) && behavior == RegistrationAddBehavior.Prompt)
			{
				return false;
			}
			RemoveNode(reg, parent);
			AddNode(reg, newParent, behavior);
			return true;
		}
	}

	private static void OnRemovedNode(RegistrationInfo node)
	{
		if (RegistrationProvider.RemovedNode != null)
		{
			RegistrationProvider.RemovedNode(null, new RegistrationEventArgs(node));
		}
		Instance.Save();
	}

	private static void OnAddedNode(RegistrationInfo node)
	{
		if (RegistrationProvider.AddedNode != null)
		{
			RegistrationProvider.AddedNode(null, new RegistrationEventArgs(node));
		}
		Instance.Save();
	}

	private RegistrationProvider()
	{
		root = new ServerTypeRegistrationInfo();
		serverTypes = new HybridDictionary();
	}

	private RegistrationInfo AddServerTypeNodeInternal(ServerTypeRegistrationInfo node)
	{
		if (!serverTypes.Contains(node.ServerType))
		{
			root.AddChild(node);
			serverTypes[node.ServerType] = node;
			return node;
		}
		ServerTypeRegistrationInfo serverTypeRegistrationInfo = serverTypes[node.ServerType] as ServerTypeRegistrationInfo;
		foreach (RegistrationInfo child in node.Children)
		{
			serverTypeRegistrationInfo.AddChild(child);
		}
		return null;
	}

	private RegistrationInfo AddGroupNodeInternal(string name, string desc, ParentRegistrationInfo parent, RegistrationAddBehavior behavior)
	{
		GroupRegistrationInfo groupRegistrationInfo = new GroupRegistrationInfo();
		groupRegistrationInfo.FriendlyName = name;
		groupRegistrationInfo.Description = desc;
		groupRegistrationInfo.ServerType = parent.ServerType;
		return AddNodeInternal(groupRegistrationInfo, parent, behavior);
	}

	private RegistrationInfo AddNodeInternal(RegistrationInfo reg, ParentRegistrationInfo parent, RegistrationAddBehavior behavior)
	{
		if (reg.ServerType == Guid.Empty)
		{
			reg.ServerType = parent.ServerType;
		}
		RegistrationInfo registrationInfo = parent.Children[reg.FriendlyName];
		if (registrationInfo != null)
		{
			switch (behavior)
			{
			case RegistrationAddBehavior.Prompt:
				if (registrationInfo != null)
				{
					return null;
				}
				break;
			case RegistrationAddBehavior.CreateCopy:
				reg.FriendlyName = parent.GenerateUniqueName(reg.FriendlyName);
				break;
			case RegistrationAddBehavior.Overwrite:
				RemoveNode(registrationInfo, parent);
				break;
			}
		}
		parent.AddChild(reg);
		return reg;
	}

	private void RemoveNodeInternal(RegistrationInfo reg, ParentRegistrationInfo parent)
	{
		parent.RemoveChild(reg);
	}
}
