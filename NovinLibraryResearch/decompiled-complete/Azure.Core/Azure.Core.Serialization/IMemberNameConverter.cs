using System.Reflection;

namespace Azure.Core.Serialization;

public interface IMemberNameConverter
{
	string? ConvertMemberName(MemberInfo member);
}
