using System.Drawing;

namespace FormCustom.ExtendedControls.Notification;

public class NotificationModel
{
	public string Message { get; set; } = "";


	public string Title { get; set; } = "";


	public Image Image { get; set; }

	public short DurationTime { get; set; } = 5;

}
