using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

public class StiRecentConnections
{
	public class Item
	{
		public Connection Connection { get; }

		public string Name { get; }

		public Item(Connection connection, string name)
		{
			Connection = connection;
			Name = name;
		}
	}

	public class Connection
	{
		public int Id { get; internal set; }

		public string DataAdapterType { get; set; }

		public string Name { get; set; }

		public string Alias { get; set; }

		public string ConnectionString { get; set; }

		public string XmlType { get; set; }

		public string PathSchema { get; set; }

		public string PathData { get; set; }

		public int CodePage { get; set; }

		public string Separator { get; set; }

		public bool PromptUserNameAndPassword { get; set; }

		public bool Pinned { get; set; }
	}

	private const string id = "StiRecentConnections";

	private const byte maxCountRecentConnections = 15;

	private int countRecentConnections;

	private List<Item> connections;

	public static StiRecentConnections RecentConnections { get; set; }

	public List<Item> GetRecentConnections()
	{
		return connections;
	}

	public StiDatabase GetDatabase(Connection recentConnection)
	{
		if (recentConnection != null)
		{
			StiDatabase stiDatabase = null;
			if (recentConnection.DataAdapterType == "StiXmlDatabase")
			{
				stiDatabase = new StiXmlDatabase(recentConnection.Name, recentConnection.PathSchema, recentConnection.PathData);
				stiDatabase.Alias = recentConnection.Alias;
				if (!string.IsNullOrEmpty(recentConnection.XmlType))
				{
					((StiXmlDatabase)stiDatabase).XmlType = ((!(recentConnection.XmlType == StiXmlType.AdoNetXml.ToString())) ? StiXmlType.Xml : StiXmlType.AdoNetXml);
				}
			}
			else if (recentConnection.DataAdapterType == "StiCsvDatabase")
			{
				stiDatabase = new StiCsvDatabase(recentConnection.Name, recentConnection.PathData, recentConnection.CodePage, recentConnection.Separator);
				stiDatabase.Alias = recentConnection.Alias;
			}
			else if (recentConnection.DataAdapterType == "StiDBaseDatabase")
			{
				stiDatabase = new StiDBaseDatabase(recentConnection.Name, recentConnection.PathData, recentConnection.CodePage);
				stiDatabase.Alias = recentConnection.Alias;
			}
			else if (recentConnection.DataAdapterType == "StiODataDatabase")
			{
				stiDatabase = new StiODataDatabase(recentConnection.Name, recentConnection.ConnectionString);
				stiDatabase.Alias = recentConnection.Alias;
			}
			else if (recentConnection.DataAdapterType == "StiExcelDatabase")
			{
				stiDatabase = new StiExcelDatabase(recentConnection.Name, recentConnection.PathData);
				stiDatabase.Alias = recentConnection.Alias;
			}
			else if (recentConnection.DataAdapterType == "StiJsonDatabase")
			{
				stiDatabase = new StiJsonDatabase(recentConnection.Name, recentConnection.PathData);
				stiDatabase.Alias = recentConnection.Alias;
			}
			else if (recentConnection.DataAdapterType == "StiGisDatabase")
			{
				stiDatabase = new StiGisDatabase(recentConnection.Name, recentConnection.PathData);
				stiDatabase.Alias = recentConnection.Alias;
			}
			else
			{
				StiDatabase stiDatabase2 = StiOptions.Services.Databases.FirstOrDefault((StiDatabase d) => ((object)d).GetType().Name == recentConnection.DataAdapterType);
				if (stiDatabase2 != null)
				{
					stiDatabase = ((StiService)stiDatabase2).Clone() as StiDatabase;
					stiDatabase.Name = recentConnection.Name;
					stiDatabase.Alias = recentConnection.Alias;
					if (stiDatabase is StiSqlDatabase)
					{
						((StiSqlDatabase)stiDatabase).ConnectionString = recentConnection.ConnectionString;
						((StiSqlDatabase)stiDatabase).PromptUserNameAndPassword = recentConnection.PromptUserNameAndPassword;
					}
				}
			}
			return stiDatabase;
		}
		return null;
	}

	private void Load()
	{
		StiSettings.Load();
		countRecentConnections = StiSettings.GetInt("StiRecentConnections", "Count", 0);
		if (countRecentConnections == 0)
		{
			return;
		}
		connections = new List<Item>();
		IEnumerable<StiDatabase> source = StiOptions.Services.Databases.Where((StiDatabase s) => ((StiService)s).ServiceEnabled);
		for (int i = 0; i < countRecentConnections; i++)
		{
			Connection connection = new Connection
			{
				Id = i,
				DataAdapterType = StiSettings.GetStr("StiRecentConnections", $"dataAdapterType{i}", string.Empty)
			};
			if (string.IsNullOrEmpty(connection.DataAdapterType))
			{
				continue;
			}
			StiDatabase stiDatabase = source.FirstOrDefault((StiDatabase x) => ((object)x).GetType().Name == connection.DataAdapterType);
			if (stiDatabase != null)
			{
				connection.Name = StiSettings.GetStr("StiRecentConnections", $"name{i}", string.Empty);
				connection.Alias = StiSettings.GetStr("StiRecentConnections", $"alias{i}", string.Empty);
				connection.Pinned = StiSettings.GetBool("StiRecentConnections", $"pinned{i}", nullValue: false);
				if (connection.DataAdapterType == "StiXmlDatabase")
				{
					connection.PathData = StiSettings.GetStr("StiRecentConnections", "pathData" + i, string.Empty);
					connection.PathSchema = StiSettings.GetStr("StiRecentConnections", "pathSchema" + i, string.Empty);
					connection.XmlType = StiSettings.GetStr("StiRecentConnections", "xmlType" + i, string.Empty);
					AddObjects(connection, "[" + ((StiService)stiDatabase).ServiceName + "]." + connection.Name);
				}
				else if (connection.DataAdapterType == "StiJsonDatabase")
				{
					connection.PathData = StiSettings.GetStr("StiRecentConnections", "pathData" + i, string.Empty);
					AddObjects(connection, "[" + ((StiService)stiDatabase).ServiceName + "]." + connection.Name);
				}
				else if (connection.DataAdapterType == "StiGisDatabase")
				{
					connection.PathData = StiSettings.GetStr("StiRecentConnections", "pathData" + i, string.Empty);
					AddObjects(connection, "[" + ((StiService)stiDatabase).ServiceName + "]." + connection.Name);
				}
				else if (connection.DataAdapterType == "StiCsvDatabase")
				{
					connection.PathData = StiSettings.GetStr("StiRecentConnections", $"pathData{i}", string.Empty);
					connection.CodePage = StiSettings.GetInt("StiRecentConnections", $"codePage{i}", 0);
					connection.Separator = StiSettings.GetStr("StiRecentConnections", $"separator{i}", string.Empty);
					AddObjects(connection, "[" + ((StiService)stiDatabase).ServiceName + "]." + connection.Name);
				}
				else if (connection.DataAdapterType == "StiExcelDatabase")
				{
					connection.PathData = StiSettings.GetStr("StiRecentConnections", $"pathData{i}", string.Empty);
					AddObjects(connection, "[" + ((StiService)stiDatabase).ServiceName + "]." + connection.Name);
				}
				else
				{
					connection.ConnectionString = StiSettings.GetStr("StiRecentConnections", $"connectionString{i}", string.Empty);
					connection.PromptUserNameAndPassword = StiSettings.GetBool("StiRecentConnections", $"promptUserNameAndPassword{i}", nullValue: false);
					AddObjects(connection, "[" + ((StiService)stiDatabase).ServiceName + "]." + connection.Name);
				}
				if (connections.Count >= 15)
				{
					break;
				}
			}
		}
	}

	private void AddObjects(Connection connection, string name)
	{
		Item item = new Item(connection, name);
		connections.Add(item);
	}

	public void Save()
	{
		if (connections == null)
		{
			connections = new List<Item>();
		}
		StiSettings.Load();
		StiSettings.ClearKey("StiRecentConnections");
		StiSettings.Set("StiRecentConnections", "Count", connections.Count);
		int num = 0;
		foreach (Item connection2 in connections)
		{
			Connection connection = connection2.Connection;
			StiSettings.Set("StiRecentConnections", "dataAdapterType" + num, connection.DataAdapterType);
			StiSettings.Set("StiRecentConnections", "name" + num, connection.Name);
			StiSettings.Set("StiRecentConnections", "alias" + num, connection.Alias);
			StiSettings.Set("StiRecentConnections", "pinned" + num, connection.Pinned);
			StiSettings.Set("StiRecentConnections", "pathData" + num, connection.PathData);
			StiSettings.Set("StiRecentConnections", "pathSchema" + num, connection.PathSchema);
			StiSettings.Set("StiRecentConnections", "xmlType" + num, connection.XmlType);
			StiSettings.Set("StiRecentConnections", "codePage" + num, connection.CodePage);
			StiSettings.Set("StiRecentConnections", "separator" + num, connection.Separator);
			StiSettings.Set("StiRecentConnections", "connectionString" + num, connection.ConnectionString);
			StiSettings.Set("StiRecentConnections", "promptUserNameAndPassword" + num, connection.PromptUserNameAndPassword);
			num++;
			if (num > 15)
			{
				break;
			}
		}
		StiSettings.Save();
	}

	public void AddAndSave(StiDatabase dataBase)
	{
		if (dataBase == null)
		{
			return;
		}
		if (connections == null)
		{
			connections = new List<Item>();
		}
		if (connections.Count >= 15)
		{
			for (int i = 0; i < connections.Count; i++)
			{
				Connection connection = connections[i].Connection;
				if (!connection.Pinned)
				{
					connections.RemoveAt(i);
					break;
				}
			}
		}
		Connection connection2 = new Connection();
		if (dataBase is StiXmlDatabase)
		{
			StiXmlDatabase stiXmlDatabase = (StiXmlDatabase)dataBase;
			if (string.IsNullOrEmpty(stiXmlDatabase.PathData) && string.IsNullOrEmpty(stiXmlDatabase.PathSchema))
			{
				return;
			}
			connection2.DataAdapterType = "StiXmlDatabase";
			connection2.PathData = stiXmlDatabase.PathData;
			connection2.PathSchema = stiXmlDatabase.PathSchema;
			connection2.XmlType = stiXmlDatabase.XmlType.ToString();
		}
		else if (dataBase is StiExcelDatabase)
		{
			StiExcelDatabase stiExcelDatabase = (StiExcelDatabase)dataBase;
			if (string.IsNullOrEmpty(stiExcelDatabase.PathData))
			{
				return;
			}
			connection2.DataAdapterType = "StiExcelDatabase";
			connection2.PathData = stiExcelDatabase.PathData;
		}
		else if (dataBase is StiCsvDatabase)
		{
			StiCsvDatabase stiCsvDatabase = (StiCsvDatabase)dataBase;
			if (string.IsNullOrEmpty(stiCsvDatabase.PathData))
			{
				return;
			}
			connection2.DataAdapterType = "StiCsvDatabase";
			connection2.PathData = stiCsvDatabase.PathData;
			connection2.CodePage = stiCsvDatabase.CodePage;
			connection2.Separator = stiCsvDatabase.Separator;
		}
		else if (dataBase is StiJsonDatabase)
		{
			StiJsonDatabase stiJsonDatabase = (StiJsonDatabase)dataBase;
			if (string.IsNullOrEmpty(stiJsonDatabase.PathData))
			{
				return;
			}
			connection2.DataAdapterType = "StiJsonDatabase";
			connection2.PathData = stiJsonDatabase.PathData;
		}
		else if (dataBase is StiGisDatabase)
		{
			StiGisDatabase stiGisDatabase = (StiGisDatabase)dataBase;
			if (string.IsNullOrEmpty(stiGisDatabase.PathData))
			{
				return;
			}
			connection2.DataAdapterType = "StiGisDatabase";
			connection2.PathData = stiGisDatabase.PathData;
		}
		else
		{
			if (!(dataBase is StiSqlDatabase))
			{
				Save();
				return;
			}
			StiSqlDatabase stiSqlDatabase = (StiSqlDatabase)dataBase;
			if (string.IsNullOrEmpty(stiSqlDatabase.ConnectionString))
			{
				return;
			}
			if (dataBase is StiUndefinedDatabase)
			{
				connection2.DataAdapterType = "Undefined SqlDatabase";
			}
			else
			{
				connection2.DataAdapterType = ((object)dataBase).GetType().Name;
			}
			connection2.PromptUserNameAndPassword = stiSqlDatabase.PromptUserNameAndPassword;
			connection2.ConnectionString = stiSqlDatabase.ConnectionString;
		}
		connection2.Name = dataBase.Name;
		connection2.Alias = dataBase.Alias;
		if (CheckingForAMatch(connection2))
		{
			AddObjects(connection2, string.Empty);
		}
		Save();
	}

	public void Remove(Connection recentConn)
	{
		if (connections == null)
		{
			return;
		}
		foreach (Item connection in connections)
		{
			if (connection.Connection.Id == recentConn.Id)
			{
				connections.Remove(connection);
				break;
			}
		}
	}

	public void SetPin(int ID, bool pinValue)
	{
		if (connections == null)
		{
			return;
		}
		foreach (Item connection2 in connections)
		{
			Connection connection = connection2.Connection;
			if (connection.Id == ID)
			{
				connection.Pinned = pinValue;
				break;
			}
		}
	}

	private bool CheckingForAMatch(Connection addRecentConn)
	{
		for (int i = 0; i < connections.Count; i++)
		{
			Connection connection = connections[i].Connection;
			if (!(connection.DataAdapterType == addRecentConn.DataAdapterType))
			{
				continue;
			}
			if (connection.DataAdapterType == "StiXmlDatabase")
			{
				if (addRecentConn.PathData == connection.PathData && addRecentConn.PathSchema == connection.PathSchema)
				{
					return false;
				}
			}
			else if (connection.DataAdapterType == "StiJsonDatabase")
			{
				if (addRecentConn.PathData == connection.PathData)
				{
					return false;
				}
			}
			else if (connection.DataAdapterType == "StiGisDatabase")
			{
				if (addRecentConn.PathData == connection.PathData)
				{
					return false;
				}
			}
			else if (addRecentConn.ConnectionString == connection.ConnectionString)
			{
				return false;
			}
		}
		return true;
	}

	public StiRecentConnections()
	{
		Load();
	}
}
