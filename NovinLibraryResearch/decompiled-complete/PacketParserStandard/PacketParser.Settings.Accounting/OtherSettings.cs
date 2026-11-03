using System.ComponentModel;

namespace PacketParser.Settings.Accounting;

public class OtherSettings
{
	public ClockSettings ClockSettings { get; set; } = new ClockSettings();


	public BackUpSettings BackUpSettings { get; set; } = new BackUpSettings();


	public NumberFormatSettings NumberFormat { get; set; } = new NumberFormatSettings();


	public ShowDialogWindowSettings ShowDialogWindow { get; set; } = new ShowDialogWindowSettings();


	public WizardConfigSettings WizardConfig { get; set; } = new WizardConfigSettings();


	[DefaultValue(400)]
	public int MaxImageSize { get; set; } = 400;


	[DefaultValue(400)]
	public int jpegQuality { get; set; } = 30;


	public int TicketCancelStatus { get; set; }
}
