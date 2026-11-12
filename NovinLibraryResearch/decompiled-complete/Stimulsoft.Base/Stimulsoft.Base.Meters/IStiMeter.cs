namespace Stimulsoft.Base.Meters;

public interface IStiMeter
{
	string Key { get; set; }

	string Expression { get; set; }

	string Label { get; set; }

	int GetUniqueCode();
}
