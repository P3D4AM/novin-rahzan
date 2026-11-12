namespace Stimulsoft.Base;

public interface IStiReportPage : IStiReportComponent, IStiAppComponent, IStiAppCell
{
	string ParseExpression(string text, bool allowReturnNull = false);
}
