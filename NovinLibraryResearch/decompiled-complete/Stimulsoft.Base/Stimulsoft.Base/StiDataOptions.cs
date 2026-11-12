using System;
using System.ComponentModel;

namespace Stimulsoft.Base;

public class StiDataOptions
{
	[Obsolete("StiDataOptions.RetriveColumnsMode was obsoleted. Please use StiDataOptions.RetrieveColumnsMode.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static StiRetrieveColumnsMode RetriveColumnsMode
	{
		get
		{
			return RetrieveColumnsMode;
		}
		set
		{
			RetrieveColumnsMode = value;
		}
	}

	public static StiRetrieveColumnsMode RetrieveColumnsMode { get; set; } = StiRetrieveColumnsMode.SchemaOnly;


	[Obsolete("StiDataOptions.WizardStoredProcRetriveMode was obsoleted. Please use StiDataOptions.WizardStoredProcRetrieveMode.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static StiWizardStoredProcRetriveMode WizardStoredProcRetriveMode
	{
		get
		{
			return (StiWizardStoredProcRetriveMode)WizardStoredProcRetrieveMode;
		}
		set
		{
			WizardStoredProcRetrieveMode = (StiWizardStoredProcRetrieveMode)value;
		}
	}

	public static StiWizardStoredProcRetrieveMode WizardStoredProcRetrieveMode { get; set; } = StiWizardStoredProcRetrieveMode.All;


	public static bool AllowUseOracleClientConnector { get; set; }
}
