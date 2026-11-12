using System;

namespace Stimulsoft.Base.Server;

public interface IStiAccountCreator
{
	IStiWpfDashboardViewerControl GetWpfDashboardViewerControl(IStiReport report, int startPage);

	IStiAccountControl GetAccountControl(IntPtr intPtr);

	IStiNotificationsControl GetNotificationsControl(IntPtr intPtr);

	IStiNotificationMenuControl GetNotificationMenuControl(IntPtr intPtr);

	IStiLoginMenuControl GetLoginMenuContol();

	IStiAccountLoginMenuControl GetAccountLoginMenuControl();

	IStiShareMenuControl GetShareMenuControl(object viewer);

	IStiSubscriptionsWindow GetSubscriptionsWindow();

	IStiChangePasswordWindow GetChangePasswordWindow();

	IStiUpdateWindow GetUpdateWindow();

	IStiProfileWindow GetProfileWindow();

	IStiPublishWindow GetPublishWindow(IStiReport report);

	IStiGetStartedWindow GetStartedWindow();

	IStiChooseSkillLevelWindow GetChooseSkillLevelWindow(StiDesignerSpecification designerSpecification);

	IStiTrialDaysLeftWindow GetTrialDaysLeftWindow();

	IStiCloudShareWindow GetCloudShareWindow(IStiReport report, string reportItemKey);

	IStiAuthorizationWindow GetAuthorizationWindow();

	IStiCloudSaveWindow GetCloudSaveWindow(IStiReport report, string password, string fileName);

	IStiCloudOpenControl GetCloudOpenControl(IntPtr intPtr);

	IStiCloudOpenWindow GetCloudOpenWindow();

	IStiCustomMapEditorWindow GetCustomMapEditorWindow(string fileData);

	IStiMessageWindow GetNotificationCollectionWindow(StiAccountNotificationCollectionEventArgs accountMessage);

	IStiMessageWindow GetMessageWindow(StiAccountMessageEventArgs accountMessage);

	IStiNuGetWindow GetNuGetWindow(StiDataConnector connector);

	IStiNuGetDownloader GetNuGetDownloader();

	IStiReportCommandsService GetReportCommandsService();

	IStiUnsaveReportWindow GetUnsaveReportWindow();
}
