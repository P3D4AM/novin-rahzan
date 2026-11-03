using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[StiServiceBitmap(typeof(StiDatabase), "Stimulsoft.Report.Bmp.Dictionary.Database.bmp")]
[StiServiceCategoryBitmap(typeof(StiDatabase), "Stimulsoft.Report.Bmp.Dictionary.Database.bmp")]
public abstract class StiDatabase : StiService, IStiInherited, IStiPropertyGridObject, IStiJsonReportObject, IStiAppConnection, IStiAppCell, IStiName, IStiAlias
{
	public enum Order
	{
		Name = 100,
		Alias = 200,
		ConnectionString = 300,
		FirstRowIsHeader = 350,
		PathSchema = 400,
		PathData = 500,
		XmlType = 600,
		PromptUserNameAndPassword = 700,
		SaveDataInReportResources = 800
	}

	[Browsable(false)]
	public virtual StiComponentId ComponentId => StiComponentId.StiDatabase;

	[Browsable(false)]
	public string PropName => Name;

	[Browsable(false)]
	[DefaultValue(false)]
	[StiSerializable]
	public bool Inherited { get; set; }

	[StiSerializable]
	[StiOrder(100)]
	[Description("Gets or sets a name of the database.")]
	public string Name { get; set; }

	[StiSerializable]
	[StiOrder(200)]
	[Description("Gets or sets an alias of the database.")]
	public string Alias { get; set; }

	public sealed override string ServiceCategory => Loc.Get("PropertyMain", "CategoryConnections");

	public sealed override Type ServiceType => typeof(StiDatabase);

	public override string ServiceName => CreateConnector().Name;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when connection is activating.")]
	public StiConnectingEvent ConnectingEvent { get; set; } = new StiConnectingEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when connection is activated.")]
	public StiConnectedEvent ConnectedEvent { get; set; } = new StiConnectedEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when connection is deactivating.")]
	public StiDisconnectingEvent DisconnectingEvent { get; set; } = new StiDisconnectingEvent();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when connection is deactivated.")]
	public StiDisconnectedEvent DisconnectedEvent { get; set; } = new StiDisconnectedEvent();


	[DefaultValue(null)]
	[StiSerializable]
	[Browsable(false)]
	public string Key { get; set; }

	[Browsable(false)]
	public virtual StiConnectionType ConnectionType => StiConnectionType.Other;

	[Browsable(false)]
	public virtual StiConnectionOrder ConnectionOrder => CreateConnector().ConnectionOrder;

	[Browsable(false)]
	public bool AllowException { get; set; }

	public event EventHandler Connecting;

	public event EventHandler Connected;

	public event EventHandler Disconnecting;

	public event EventHandler Disconnected;

	public virtual StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[2]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public virtual StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = new StiEventsCollection();
		if (ConnectingEvent != null)
		{
			stiEventsCollection.Add(ConnectingEvent);
		}
		if (ConnectedEvent != null)
		{
			stiEventsCollection.Add(ConnectedEvent);
		}
		if (DisconnectingEvent != null)
		{
			stiEventsCollection.Add(DisconnectingEvent);
		}
		if (DisconnectedEvent != null)
		{
			stiEventsCollection.Add(DisconnectedEvent);
		}
		return stiEventsCollection;
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", ((object)this).GetType().Name);
		JsonExtensions.AddPropertyBool(val, "Inherited", Inherited, false, false);
		JsonExtensions.AddPropertyJObject(val, "ConnectingEvent", ConnectingEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ConnectedEvent", ConnectedEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DisconnectingEvent", DisconnectingEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DisconnectedEvent", DisconnectedEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Alias", Alias);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Key", Key);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Inherited":
				Inherited = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ConnectingEvent":
				ConnectingEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ConnectedEvent":
				ConnectedEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "DisconnectingEvent":
				DisconnectingEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "DisconnectedEvent":
				DisconnectedEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Name":
				Name = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Alias":
				Alias = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Key":
				Key = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	string IStiAppConnection.GetName()
	{
		return Name;
	}

	string IStiAppCell.GetKey()
	{
		Key = StiKeyHelper.GetOrGeneratedKey(Key);
		return Key;
	}

	void IStiAppCell.SetKey(string key)
	{
		Key = key;
	}

	protected virtual void OnConnecting(EventArgs e)
	{
	}

	public void InvokeConnecting()
	{
		OnConnecting(EventArgs.Empty);
		this.Connecting?.Invoke(this, EventArgs.Empty);
		StiBlocklyHelper.InvokeBlockly(null, this, ConnectingEvent);
	}

	protected virtual void OnConnected(EventArgs e)
	{
	}

	public void InvokeConnected()
	{
		OnConnected(EventArgs.Empty);
		this.Connected?.Invoke(this, EventArgs.Empty);
		StiBlocklyHelper.InvokeBlockly(null, this, ConnectedEvent);
	}

	protected virtual void OnDisconnecting(EventArgs e)
	{
	}

	public void InvokeDisconnecting()
	{
		OnDisconnecting(EventArgs.Empty);
		this.Disconnecting?.Invoke(this, EventArgs.Empty);
		StiBlocklyHelper.InvokeBlockly(null, this, DisconnectingEvent);
	}

	protected virtual void OnDisconnected(EventArgs e)
	{
	}

	public void InvokeDisconnected()
	{
		OnDisconnected(EventArgs.Empty);
		this.Disconnected?.Invoke(this, EventArgs.Empty);
		StiBlocklyHelper.InvokeBlockly(null, this, DisconnectedEvent);
	}

	public abstract StiDatabase CreateNew();

	public void Synchronize(StiReport report)
	{
		ApplyDatabaseInformation(GetDatabaseInformation(), report);
	}

	public abstract StiDataConnector CreateConnector(string connectionString = null);

	public abstract void ApplyDatabaseInformation(StiDatabaseInformation information, StiReport report);

	public abstract void ApplyDatabaseInformation(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll);

	public virtual StiDatabaseInformation GetDatabaseInformation(StiReport report)
	{
		return null;
	}

	public virtual StiDatabaseInformation GetDatabaseInformation()
	{
		return GetDatabaseInformation(null);
	}

	public override string ToString()
	{
		return ToString(onlyAlias: false);
	}

	public string ToString(bool onlyAlias)
	{
		if (onlyAlias && !string.IsNullOrWhiteSpace(Alias))
		{
			return Alias + " [" + ((StiService)this).ServiceName + "]";
		}
		if (Name == Alias || string.IsNullOrWhiteSpace(Alias))
		{
			return Name + " [" + ((StiService)this).ServiceName + "]";
		}
		return Name + " [" + Alias + "]";
	}

	public abstract DialogResult Edit(StiDictionary dictionary, bool newDatabase);

	public abstract Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase);

	public abstract void RegData(StiDictionary dictionary, bool loadData);

	public StiDatabase()
		: this(string.Empty)
	{
	}

	public StiDatabase(string name)
		: this(name, name)
	{
	}

	public StiDatabase(string name, string alias)
	{
		Name = name;
		Alias = alias;
	}

	public StiDatabase(string name, string alias, string key)
		: this(name, alias)
	{
		Key = key;
	}
}
