using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Dashboard.Interactions.Design;

public class StiOnlineMapDashboardInteractionConverter : StiDashboardInteractionConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiOnlineMapDashboardInteraction stiOnlineMapDashboardInteraction = value as StiOnlineMapDashboardInteraction;
			Type[] types = new Type[6]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(bool),
				typeof(bool)
			};
			object[] objs = new object[6] { stiOnlineMapDashboardInteraction.FileName, stiOnlineMapDashboardInteraction.HeaderText, stiOnlineMapDashboardInteraction.FooterText, stiOnlineMapDashboardInteraction.ShowFullScreenButton, stiOnlineMapDashboardInteraction.ShowSaveButton, stiOnlineMapDashboardInteraction.ShowViewDataButton };
			ConstructorInfo constructor = typeof(StiOnlineMapDashboardInteraction).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
