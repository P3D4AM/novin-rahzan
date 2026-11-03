using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Stimulsoft.Report.Dictionary;

[Serializable]
public class StiDataCollection : CollectionBase, ICloneable
{
	public StiData this[int index]
	{
		get
		{
			return (StiData)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiData this[string name]
	{
		get
		{
			name = name.ToLowerInvariant();
			foreach (StiData item in base.List)
			{
				if (item?.Name?.ToLowerInvariant() == name)
				{
					return item;
				}
			}
			return null;
		}
		set
		{
			name = name.ToLowerInvariant();
			for (int i = 0; i < base.List.Count; i++)
			{
				if (((!(base.List[i] is StiData stiData)) ? null : stiData.Name?.ToLowerInvariant()) == name)
				{
					base.List[i] = value;
					return;
				}
			}
			Add(value);
		}
	}

	public StiData[] Items => (StiData[])base.InnerList.ToArray(typeof(StiData));

	public List<StiData> ToList()
	{
		return this.Cast<StiData>().ToList();
	}

	public void Add(StiData data)
	{
		base.List.Add(data);
	}

	public void AddRange(StiData[] data)
	{
		base.InnerList.AddRange(data);
	}

	public bool Contains(StiData data)
	{
		return base.List.Contains(data);
	}

	public bool Contains(string name)
	{
		return ToList().FirstOrDefault((StiData x) => x.Name == name) != null;
	}

	public int IndexOf(StiData data)
	{
		return base.List.IndexOf(data);
	}

	public void Insert(int index, StiData data)
	{
		base.List.Insert(index, data);
	}

	public void Remove(StiData data)
	{
		base.List.Remove(data);
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void RegData(string name, object data)
	{
		RegData(name, name, data);
	}

	public void RegData(string name, DataTable dataTable)
	{
		RegData(name, name, dataTable);
	}

	public void RegData(string name, DataSet dataSet)
	{
		RegData(name, name, dataSet);
	}

	public void RegData(string name, DataView dataView)
	{
		RegData(name, name, dataView);
	}

	public void RegData(string name, SqlConnection connection)
	{
		RegData(name, name, connection);
	}

	public void RegData(string name, OleDbConnection connection)
	{
		RegData(name, name, connection);
	}

	public void RegData(string name, OdbcConnection connection)
	{
		RegData(name, name, connection);
	}

	public void RegData(string name, string alias, object data)
	{
		if (data == null)
		{
			return;
		}
		if (data is DataView)
		{
			RegData(name, alias, data as DataView);
			return;
		}
		if (data is DataTable)
		{
			RegData(name, alias, data as DataTable);
			return;
		}
		if (data is DataSet)
		{
			RegData(name, alias, data as DataSet);
			return;
		}
		if (data is IDbConnection)
		{
			Add(new StiData(name, data));
			this[base.Count - 1].OriginalConnectionState = ((IDbConnection)data).State;
			this[base.Count - 1].Alias = alias;
			return;
		}
		if (data is StiUserData)
		{
			Add(new StiData(name, data));
			this[base.Count - 1].Alias = alias;
			return;
		}
		if (data is BindingSource)
		{
			BindingSource val = (BindingSource)((data is BindingSource) ? data : null);
			RegData(name, (object)val.List);
			this[base.Count - 1].Name = name;
			return;
		}
		if (data is IDataReader)
		{
			throw new Exception("IDataReader is not supported!");
		}
		if (data is IDataAdapter)
		{
			throw new Exception("IDataAdapter is not supported!");
		}
		StiBusinessObjectToDataSet stiBusinessObjectToDataSet = new StiBusinessObjectToDataSet();
		DataSet dataSet = stiBusinessObjectToDataSet.ConvertBusinessObjectToDataSet(name, data);
		if (dataSet != null)
		{
			foreach (DataTable table in dataSet.Tables)
			{
				RegData(table.TableName, alias, table);
				this[base.Count - 1].Data = data;
				this[base.Count - 1].IsBusinessObjectData = true;
			}
			return;
		}
		Add(new StiData(name, data));
		this[base.Count - 1].Alias = alias;
		this[base.Count - 1].IsBusinessObjectData = true;
	}

	public void RegData(string name, string alias, DataTable dataTable)
	{
		if (dataTable != null)
		{
			if (this[name] == null)
			{
				Add(new StiData(name, dataTable, dataTable));
				this[base.Count - 1].Alias = alias;
			}
			else
			{
				this[name].Data = dataTable;
				this[name].ViewData = dataTable;
				this[name].Alias = alias;
			}
		}
	}

	public void RegData(string name, string alias, DataSet dataSet)
	{
		if (dataSet == null)
		{
			return;
		}
		foreach (DataTable table in dataSet.Tables)
		{
			RegData(name + "." + table.TableName, alias, table);
		}
	}

	public void RegData(string name, string alias, DataView dataView)
	{
		if (dataView != null)
		{
			if (this[name] == null)
			{
				Add(new StiData(name, dataView, dataView));
				this[base.Count - 1].Alias = alias;
			}
			else
			{
				this[name].Data = dataView;
				this[name].Alias = alias;
			}
		}
	}

	public void RegData(string name, string alias, SqlConnection connection)
	{
		RegData(name, alias, (object)connection);
	}

	public void RegData(string name, string alias, OleDbConnection connection)
	{
		RegData(name, alias, (object)connection);
	}

	public void RegData(string name, string alias, OdbcConnection connection)
	{
		RegData(name, alias, (object)connection);
	}

	public void RegData(DataTable dataTable)
	{
		string text = dataTable.TableName;
		if (string.IsNullOrEmpty(text))
		{
			text = "Table";
		}
		RegData(text, dataTable);
	}

	public void RegData(DataSet dataSet)
	{
		RegData(dataSet.DataSetName, dataSet);
	}

	public void RegData(DataView dataView)
	{
		RegData("view" + dataView.Table.TableName, dataView);
	}

	public void RegData(StiDataCollection datas)
	{
		if (datas == this)
		{
			return;
		}
		foreach (StiData data in datas)
		{
			Add(data);
		}
	}

	public void ClearReportDatabase()
	{
		int num = 0;
		while (num < base.Count)
		{
			if (this[num].IsReportData)
			{
				if (this[num].ViewData is IDisposable)
				{
					try
					{
						((IDisposable)this[num].ViewData).Dispose();
					}
					catch
					{
					}
				}
				RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
	}

	public StiDataCollection GetData(Type typeData)
	{
		StiDataCollection stiDataCollection = new StiDataCollection();
		foreach (StiData item in base.List)
		{
			if (item.ViewData.GetType() == typeData)
			{
				stiDataCollection.Add(item);
			}
		}
		return stiDataCollection;
	}
}
