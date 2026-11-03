using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public abstract class StiNoSqlSource : StiSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiNoSqlSource;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a SQL statement to execute at the Data Source.")]
	[StiOrder(1000)]
	public override string SqlCommand
	{
		get
		{
			return base.SqlCommand;
		}
		set
		{
			base.SqlCommand = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a query statement to execute at the Data Source.")]
	[StiOrder(850)]
	public string Query
	{
		get
		{
			return SqlCommand;
		}
		set
		{
			SqlCommand = value;
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[10]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.NameInSource(),
			propertiesHelper.ConnectOnStart(),
			propertiesHelper.AllowExpressions(),
			propertiesHelper.DataSourceType(),
			propertiesHelper.CommandTimeout(),
			propertiesHelper.ReconnectOnEachRow(),
			propertiesHelper.SqlCommand(),
			propertiesHelper.Query()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public StiNoSqlSource()
		: this("", "", "")
	{
	}

	public StiNoSqlSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiNoSqlSource(string nameInSource, string name, string alias)
		: this(nameInSource, name, alias, string.Empty)
	{
	}

	public StiNoSqlSource(string nameInSource, string name, string alias, string sqlCommand)
		: base(nameInSource, name, alias, sqlCommand)
	{
	}

	public StiNoSqlSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiNoSqlSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiNoSqlSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiNoSqlSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
