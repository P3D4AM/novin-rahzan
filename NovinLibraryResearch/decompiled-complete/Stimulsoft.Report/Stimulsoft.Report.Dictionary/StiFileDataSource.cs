using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public abstract class StiFileDataSource : StiDataStoreSource
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFileDataSource;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets a path to the data file.")]
	public string Path
	{
		get
		{
			return base.NameInSource;
		}
		set
		{
			base.NameInSource = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets a code page.")]
	public int CodePage { get; set; }

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[6]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.NameInSource(),
			propertiesHelper.ConnectOnStart(),
			propertiesHelper.Path(),
			propertiesHelper.CodePage()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public StiFileDataSource()
		: this("", "", "", 0, null)
	{
	}

	public StiFileDataSource(string path, string name, string alias, int codePage)
		: this(path, name, alias, codePage, null)
	{
	}

	public StiFileDataSource(string path, string name, string alias, int codePage, string key)
		: base(name, name, alias, key)
	{
		Path = path;
		CodePage = codePage;
	}
}
