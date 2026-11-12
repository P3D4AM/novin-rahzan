namespace Stimulsoft.Controls.Win.DotNetBar;

public interface ICommandSource
{
	ICommand Command { get; set; }

	object CommandParameter { get; set; }
}
