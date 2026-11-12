using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Security.AccessControl;

public static class AccessControlExtension
{
	public static void Canonicalize(this RawAcl acl)
	{
		if (acl == null)
		{
			throw new ArgumentNullException("acl");
		}
		List<GenericAce> list = new List<GenericAce>(((IEnumerable)acl).Cast<GenericAce>());
		list.Sort((GenericAce a, GenericAce b) => Comparer<byte>.Default.Compare(GetComparisonValue(a), GetComparisonValue(b)));
		while (((GenericAcl)acl).Count > 0)
		{
			acl.RemoveAce(0);
		}
		int aceIndex = 0;
		list.ForEach(delegate(GenericAce ace)
		{
			acl.InsertAce(aceIndex++, ace);
		});
	}

	public static void CanonicalizeAccessRules(this ObjectSecurity objectSecurity)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (objectSecurity == null)
		{
			throw new ArgumentNullException("objectSecurity");
		}
		if (!objectSecurity.AreAccessRulesCanonical)
		{
			RawSecurityDescriptor val = new RawSecurityDescriptor(objectSecurity.GetSecurityDescriptorBinaryForm(), 0);
			val.DiscretionaryAcl.Canonicalize();
			objectSecurity.SetSecurityDescriptorBinaryForm(((GenericSecurityDescriptor)(object)val).GetBinaryForm());
		}
	}

	public static byte[] GetBinaryForm(this GenericSecurityDescriptor sd)
	{
		if (sd == null)
		{
			throw new ArgumentNullException("sd");
		}
		byte[] array = new byte[sd.BinaryLength];
		sd.GetBinaryForm(array, 0);
		return array;
	}

	private static byte GetComparisonValue(GenericAce ace)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected I4, but got Unknown
		if ((ace.AceFlags & 0x10) != 0)
		{
			return 5;
		}
		AceType aceType = ace.AceType;
		switch ((int)aceType)
		{
		case 1:
		case 2:
		case 3:
		case 10:
		case 13:
		case 14:
			return 0;
		case 6:
		case 7:
		case 8:
		case 12:
		case 15:
		case 16:
			return 1;
		case 0:
		case 9:
			return 2;
		case 5:
		case 11:
			return 3;
		default:
			return 4;
		}
	}
}
