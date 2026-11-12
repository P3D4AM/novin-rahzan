namespace Microsoft.SqlServer.Management.Assessment.Configuration;

public interface IPattern<in T>
{
	bool IsMatch(T value);
}
