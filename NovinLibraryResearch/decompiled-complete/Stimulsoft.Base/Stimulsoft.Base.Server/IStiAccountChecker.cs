using System;

namespace Stimulsoft.Base.Server;

public interface IStiAccountChecker
{
	string ProductsExpired();

	string ProductsExpired(bool shotMessage);

	string ProductsExpired(int outDays);

	string ProductsExpired(int outDays, bool shotMessage);

	bool GetProductExpiresInfo(out string longMessage, out string shotMessage, out string renewalUrl);

	bool GetProductExpiresInfo(int outDays, out string longMessage, out string shotMessage, out string renewalUrl);

	bool CheckSessionNotice(StiNotice notice);

	bool CheckIsLogin(IntPtr? handler);

	bool CheckUserActivated(IntPtr handler);

	bool CheckTrialExpired();

	bool CheckProductExpired();
}
