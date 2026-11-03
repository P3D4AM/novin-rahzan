using System;
using System.ComponentModel;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Viewer;

public class StiDotMatrixViewerConfigService : StiService
{
	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryRender");

	public sealed override Type ServiceType => typeof(StiDotMatrixViewerConfigService);

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? SaveEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowSaveButton;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowSaveButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? OpenEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowOpenButton;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowOpenButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? PrintEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowPrintButton;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowPrintButton = value;
		}
	}

	[StiServiceParam]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool? CloseEnabled
	{
		get
		{
			return StiOptions.Viewer.Windows.ShowCloseButton;
		}
		set
		{
			StiOptions.Viewer.Windows.ShowCloseButton = value;
		}
	}
}
