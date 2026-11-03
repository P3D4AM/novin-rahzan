using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiGuidDialogInfoItem : StiDialogInfoItem
{
	public override StiComponentId ComponentId => StiComponentId.StiGuidDialogInfoItem;

	[StiOrder(100)]
	public Guid Key
	{
		get
		{
			return (Guid)base.KeyObject;
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
			propertiesHelper.GuidDialogInfoKey(),
			propertiesHelper.DialogInfoValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public StiGuidDialogInfoItem()
	{
		base.KeyObject = Guid.NewGuid();
	}
}
