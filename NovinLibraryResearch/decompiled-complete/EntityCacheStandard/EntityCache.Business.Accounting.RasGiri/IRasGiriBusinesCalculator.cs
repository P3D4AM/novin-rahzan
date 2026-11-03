namespace EntityCache.Business.Accounting.RasGiri;

public interface IRasGiriBusinesCalculator
{
	decimal CalcTotalSoodCalculator(decimal Percent);

	int CalCulateRasDaysCalculator();

	decimal CalcTotalSood_AghsatCalculator(decimal Totalcurrency, decimal Percent, decimal Price, int repeat);
}
