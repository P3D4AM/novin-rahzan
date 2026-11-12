using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Dashboard.Interactions.Design;

public class StiGaugeDashboardInteractionConverter : StiDashboardInteractionConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiGaugeDashboardInteraction stiGaugeDashboardInteraction = value as StiGaugeDashboardInteraction;
			Type[] types = new Type[7]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(bool),
				typeof(bool),
				typeof(bool)
			};
			object[] objs = new object[7] { stiGaugeDashboardInteraction.FileName, stiGaugeDashboardInteraction.HeaderText, stiGaugeDashboardInteraction.FooterText, stiGaugeDashboardInteraction.ShowFullScreenButton, stiGaugeDashboardInteraction.ShowSaveButton, stiGaugeDashboardInteraction.ShowViewDataButton, stiGaugeDashboardInteraction.AllowUserSorting };
			ConstructorInfo constructor = typeof(StiGaugeDashboardInteraction).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
