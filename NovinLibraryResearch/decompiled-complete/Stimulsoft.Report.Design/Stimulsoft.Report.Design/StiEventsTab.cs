using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Design;

public class StiEventsTab : PropertyTab
{
	public override Bitmap Bitmap => StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Office2013.Events.png");

	public override string TabName => StiLocalization.Get("Report", "EventsTab");

	public override bool CanExtend(object extendee)
	{
		return true;
	}

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object component, Attribute[] attributes)
	{
		return ((PropertyTab)this).GetProperties(component, attributes);
	}

	public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
	{
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component);
		PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
		foreach (PropertyDescriptor item in properties)
		{
			if (!StiPropertyGrid.IsAllowedProperty(component.GetType(), item.Name))
			{
				continue;
			}
			StiReport stiReport = component as StiReport;
			if ((stiReport == null || stiReport.Pages.ContainsDashboards || !(item.Name == "RefreshingEvent")) && (stiReport == null || !stiReport.Pages.ContainsOnlyDashboards || (!(item.Name == "PrintingEvent") && !(item.Name == "PrintedEvent") && !(item.Name == "BeginRenderEvent") && !(item.Name == "EndRenderEvent") && !(item.Name == "RenderingEvent") && !(item.Name == "ReportCacheProcessingEvent"))) && StiTypeFinder.FindType(item.PropertyType, typeof(StiEvent)))
			{
				StiEventHideAttribute stiEventHideAttribute = item.Attributes[typeof(StiEventHideAttribute)] as StiEventHideAttribute;
				if (stiEventHideAttribute == null)
				{
					propertyDescriptorCollection.Add(new StiPropertyDescriptor(item, isEventDescriptor: true));
				}
			}
		}
		return propertyDescriptorCollection;
	}
}
