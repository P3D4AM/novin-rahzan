namespace Stimulsoft.Report.Win.Parameters.List;

public interface IStiListItemControl
{
	object Value { get; }

	void SetFocus();

	string GetValueStr();
}
