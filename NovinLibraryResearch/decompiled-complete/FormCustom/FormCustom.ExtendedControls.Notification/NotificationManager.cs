using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.ExtendedControls.Notification;

public class NotificationManager
{
	private static string _lastMessage = "";

	private static readonly Queue<NotificationModel> QueueNotifs = new Queue<NotificationModel>();

	public static void MessageBoxShowByNotif(bool showPopUp, EnNotification notificationEnum, string msg, string title = "", short secondTimeForDisplay = 3)
	{
		try
		{
			Bitmap image = (Bitmap)(notificationEnum switch
			{
				EnNotification.Information => FormCustom.Properties.Resources.information.ToBitmap(), 
				EnNotification.Error => FormCustom.Properties.Resources.error1.ToBitmap(), 
				EnNotification.Exclamtion => FormCustom.Properties.Resources.exclamtion.ToBitmap(), 
				EnNotification.Cancel => FormCustom.Properties.Resources.cancel.ToBitmap(), 
				EnNotification.question => FormCustom.Properties.Resources.question.ToBitmap(), 
				EnNotification.Ok => FormCustom.Properties.Resources.ok.ToBitmap(), 
				EnNotification.EndSync => FormCustom.Properties.Resources.EndSync.ToBitmap(), 
				EnNotification.Syncing => FormCustom.Properties.Resources.Syncing.ToBitmap(), 
				EnNotification.StartSync => FormCustom.Properties.Resources.StartSync.ToBitmap(), 
				EnNotification.Buffer => FormCustom.Properties.Resources.Buffer.ToBitmap(), 
				_ => FormCustom.Properties.Resources.StartSync.ToBitmap(), 
			});
			if (string.IsNullOrEmpty(title))
			{
				title = " ";
			}
			MessageBoxShowByNotifNew(showPopUp, msg, title, secondTimeForDisplay, (Image)(object)image);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MessageBoxShowByNotif", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\Notification\\NotificationManager.cs", 41);
		}
	}

	private static async Task MessageBoxShowByNotifNew(bool showPopup, string msg, string title, short secondTimeForDisplay = 5, Image image = null)
	{
		Image val = image;
		if (val == null)
		{
			val = (Image)(object)FormCustom.Properties.Resources.information.ToBitmap();
		}
		string text = msg.Trim();
		if (msg.Contains("[") && msg.Contains("]"))
		{
			text = msg.GetTranslate().Trim();
		}
		if (text != "" && _lastMessage == text)
		{
			return;
		}
		_lastMessage = text;
		try
		{
			if (text != "")
			{
				QueueNotifs.Enqueue(new NotificationModel
				{
					Message = text,
					Title = title.GetTranslate().Trim(),
					Image = val,
					DurationTime = secondTimeForDisplay
				});
			}
			if (!showPopup)
			{
				return;
			}
			while (QueueNotifs.Count > 0)
			{
				NotificationModel notificationModel = QueueNotifs.Dequeue();
				ClsNotif clsNotif = new ClsNotif
				{
					Image = notificationModel.Image,
					TitleText = notificationModel.Title,
					ContentText = notificationModel.Message,
					IsRightToLeft = true,
					ImageSize = new Size(50, 50),
					TitleFont = new Font(new FontFamily("PeydaWeb"), 8f),
					AnimationDuration = 1000,
					BodyColor = Color.FromArgb(187, 226, 243),
					BorderColor = Color.FromArgb(187, 226, 243),
					ContentFont = new Font(new FontFamily("PeydaWeb ExtBd"), 10f),
					ContentPadding = new Padding(5, 5, 5, 5),
					HeaderColor = Color.FromArgb(187, 226, 243),
					HeaderHeight = 15,
					ImagePadding = new Padding(10, 0, 0, 5),
					ShowCloseButton = true,
					ShowGrip = true,
					Size = new Size(300, 75 + (notificationModel.Message.Length / 22 + 1) * 12),
					TitleColor = Color.Black,
					ContentColor = Color.Black,
					Delay = notificationModel.DurationTime * 1000,
					AnimationInterval = 10
				};
				if (clsNotif.TitleText.Length < 2 && ResourceManagement.TextBrandResourceManagement != null)
				{
					clsNotif.TitleText = "شرکت نرم افزاری " + ResourceManagement.TextBrandResourceManagement.CompanyName;
				}
				clsNotif.Popup();
				await Task.Delay(notificationModel.DurationTime * 1000);
			}
			_lastMessage = "";
		}
		catch (Exception ex)
		{
			if (!ex.Message.Contains("Adding the specified count to the semaphore would cause it to exceed"))
			{
				WebErrorLog.Instance.StartLog(ex, "", "MessageBoxShowByNotifNew", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\Notification\\NotificationManager.cs", 102);
			}
		}
	}
}
