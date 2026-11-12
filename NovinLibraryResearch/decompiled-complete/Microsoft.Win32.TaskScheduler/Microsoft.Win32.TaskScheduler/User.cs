using System;
using System.Security.Claims;
using System.Security.Principal;

namespace Microsoft.Win32.TaskScheduler;

internal class User : IEquatable<User>, IDisposable
{
	private static readonly WindowsIdentity cur = WindowsIdentity.GetCurrent();

	private SecurityIdentifier sid;

	public static User Current => new User(cur);

	public WindowsIdentity Identity { get; private set; }

	public bool IsAdmin
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (Identity == null)
			{
				return false;
			}
			return new WindowsPrincipal(Identity).IsInRole((WindowsBuiltInRole)544);
		}
	}

	public bool IsCurrent
	{
		get
		{
			WindowsIdentity identity = Identity;
			if (identity == null)
			{
				return false;
			}
			return identity.User.Equals(cur.User);
		}
	}

	public bool IsServiceAccount
	{
		get
		{
			try
			{
				return sid != (SecurityIdentifier)null && (sid.IsWellKnown((WellKnownSidType)22) || sid.IsWellKnown((WellKnownSidType)24) || sid.IsWellKnown((WellKnownSidType)23));
			}
			catch
			{
			}
			return false;
		}
	}

	public bool IsSystem
	{
		get
		{
			if (sid != (SecurityIdentifier)null)
			{
				return sid.IsWellKnown((WellKnownSidType)22);
			}
			return false;
		}
	}

	public string SidString => ((object)sid)?.ToString();

	public string Name
	{
		get
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			object obj = ((ClaimsIdentity)(object)Identity)?.Name;
			if (obj == null)
			{
				SecurityIdentifier obj2 = sid;
				NTAccount val = (NTAccount)((obj2 != null) ? ((IdentityReference)obj2).Translate(typeof(NTAccount)) : null);
				if ((int)val == 0)
				{
					return null;
				}
				obj = ((IdentityReference)val).Value;
			}
			return (string)obj;
		}
	}

	public User(string userName = null)
	{
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		if (string.IsNullOrEmpty(userName))
		{
			userName = null;
		}
		if (userName == null || ((ClaimsIdentity)(object)cur).Name.Equals(userName, StringComparison.InvariantCultureIgnoreCase) || GetUser(((ClaimsIdentity)(object)cur).Name).Equals(userName, StringComparison.InvariantCultureIgnoreCase))
		{
			Identity = cur;
			sid = Identity.User;
		}
		else if (userName.Contains("\\") && !userName.StartsWith("NT AUTHORITY\\"))
		{
			try
			{
				using NativeMethods.DomainService domainService = new NativeMethods.DomainService();
				Identity = new WindowsIdentity(domainService.CrackName(userName));
				sid = Identity.User;
			}
			catch
			{
			}
		}
		if (Identity != null)
		{
			return;
		}
		if (userName != null && userName.Contains("@"))
		{
			Identity = new WindowsIdentity(userName);
			sid = Identity.User;
		}
		if (Identity == null && userName != null)
		{
			NTAccount val = new NTAccount(userName);
			try
			{
				sid = (SecurityIdentifier)((IdentityReference)val).Translate(typeof(SecurityIdentifier));
			}
			catch
			{
			}
		}
		static string GetUser(string domUser)
		{
			string[] array = domUser.Split(new char[1] { '\\' });
			if (array.Length != 2)
			{
				return domUser;
			}
			return array[1];
		}
	}

	internal User(WindowsIdentity wid)
	{
		Identity = wid;
		sid = wid.User;
	}

	public static User FromSidString(string sid)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		return new User(((IdentityReference)(NTAccount)((IdentityReference)new SecurityIdentifier(sid)).Translate(typeof(NTAccount))).Value);
	}

	public void Dispose()
	{
		WindowsIdentity identity = Identity;
		if (identity != null)
		{
			identity.Dispose();
		}
	}

	public override bool Equals(object obj)
	{
		if (obj is User other)
		{
			return Equals(other);
		}
		WindowsIdentity val = (WindowsIdentity)((obj is WindowsIdentity) ? obj : null);
		if (val != null && sid != (SecurityIdentifier)null)
		{
			return sid.Equals(val.User);
		}
		try
		{
			if (obj is string userName)
			{
				return Equals(new User(userName));
			}
		}
		catch
		{
		}
		return base.Equals(obj);
	}

	public bool Equals(User other)
	{
		if (other == null || !(sid != (SecurityIdentifier)null))
		{
			return false;
		}
		return sid.Equals(other.sid);
	}

	public override int GetHashCode()
	{
		return ((object)sid)?.GetHashCode() ?? 0;
	}
}
