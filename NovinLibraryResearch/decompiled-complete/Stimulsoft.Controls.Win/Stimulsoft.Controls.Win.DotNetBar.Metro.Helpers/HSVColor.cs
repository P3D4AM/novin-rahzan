namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Helpers;

internal struct HSVColor
{
	public double Hue;

	public double Saturation;

	public double Value;

	public HSVColor(double hue, double saturation, double value)
	{
		Hue = hue;
		Saturation = saturation;
		Value = value;
	}

	public override string ToString()
	{
		return $"Hue={Hue}, Saturation={Saturation}, Value={Value}";
	}
}
