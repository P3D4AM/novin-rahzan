namespace Stimulsoft.Controls;

public class StiBeforePageChangedEventArgs
{
	public bool Cancel { get; set; }

	public StiWizardPage OldWizardPage { get; }

	public StiWizardPage NewWizardPage { get; }

	public StiBeforePageChangedEventArgs(StiWizardPage oldWizardPage, StiWizardPage newWizardPage)
	{
		OldWizardPage = oldWizardPage;
		NewWizardPage = newWizardPage;
	}
}
