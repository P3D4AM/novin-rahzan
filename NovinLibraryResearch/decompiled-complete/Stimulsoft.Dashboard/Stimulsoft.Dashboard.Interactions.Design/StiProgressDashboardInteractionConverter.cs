using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Dashboard.Interactions.Design;

public class StiProgressDashboardInteractionConverter : StiDashboardInteractionConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiProgressDashboardInteraction stiProgressDashboardInteraction = value as StiProgressDashboardInteraction;
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
			object[] objs = new object[7] { stiProgressDashboardInteraction.FileName, stiProgressDashboardInteraction.HeaderText, stiProgressDashboardInteraction.FooterText, stiProgressDashboardInteraction.ShowFullScreenButton, stiProgressDashboardInteraction.ShowSaveButton, stiProgressDashboardInteraction.ShowViewDataButton, stiProgressDashboardInteraction.AllowUserSorting };
			ConstructorInfo constructor = typeof(StiProgressDashboardInteraction).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
