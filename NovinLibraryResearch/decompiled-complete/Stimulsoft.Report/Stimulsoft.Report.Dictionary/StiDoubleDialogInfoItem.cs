using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiDoubleDialogInfoItem : StiDialogInfoItem
{
	public override StiComponentId ComponentId => StiComponentId.StiDoubleDialogInfoItem;

	[StiOrder(100)]
	[DefaultValue(0.0)]
	[Description("Gets or sets value which will be used as key of item in GUI.")]
	public double Key
	{
		get
		{
			return (double)base.KeyObject;
		}
		set
		{
			base.KeyObject = value;
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[2]
		{
			propertiesHelper.DoubleDialogInfoKey(),
			propertiesHelper.DialogInfoValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public StiDoubleDialogInfoItem()
	{
		base.KeyObject = 0.0;
	}
}
