using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiExpressionDialogInfoItem : StiDialogInfoItem
{
	public override StiComponentId ComponentId => StiComponentId.StiExpressionDialogInfoItem;

	[StiOrder(100)]
	[Description("Gets or sets expression which will be used as key of item in GUI.")]
	public string Key
	{
		get
		{
			return (string)base.KeyObject;
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
			propertiesHelper.StringDialogInfoKey(),
			propertiesHelper.DialogInfoValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public StiExpressionDialogInfoItem()
	{
		base.KeyObject = string.Empty;
	}
}
