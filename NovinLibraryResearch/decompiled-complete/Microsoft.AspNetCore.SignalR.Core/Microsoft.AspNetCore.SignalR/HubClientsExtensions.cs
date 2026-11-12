namespace Microsoft.AspNetCore.SignalR;

public static class HubClientsExtensions
{
	public static T AllExcept<T>(this IHubClients<T> hubClients, string excludedConnectionId1)
	{
		return hubClients.AllExcept(new string[1] { excludedConnectionId1 });
	}

	public static T AllExcept<T>(this IHubClients<T> hubClients, string excludedConnectionId1, string excludedConnectionId2)
	{
		return hubClients.AllExcept(new string[2] { excludedConnectionId1, excludedConnectionId2 });
	}

	public static T AllExcept<T>(this IHubClients<T> hubClients, string excludedConnectionId1, string excludedConnectionId2, string excludedConnectionId3)
	{
		return hubClients.AllExcept(new string[3] { excludedConnectionId1, excludedConnectionId2, excludedConnectionId3 });
	}

	public static T AllExcept<T>(this IHubClients<T> hubClients, string excludedConnectionId1, string excludedConnectionId2, string excludedConnectionId3, string excludedConnectionId4)
	{
		return hubClients.AllExcept(new string[4] { excludedConnectionId1, excludedConnectionId2, excludedConnectionId3, excludedConnectionId4 });
	}

	public static T AllExcept<T>(this IHubClients<T> hubClients, string excludedConnectionId1, string excludedConnectionId2, string excludedConnectionId3, string excludedConnectionId4, string excludedConnectionId5)
	{
		return hubClients.AllExcept(new string[5] { excludedConnectionId1, excludedConnectionId2, excludedConnectionId3, excludedConnectionId4, excludedConnectionId5 });
	}

	public static T AllExcept<T>(this IHubClients<T> hubClients, string excludedConnectionId1, string excludedConnectionId2, string excludedConnectionId3, string excludedConnectionId4, string excludedConnectionId5, string excludedConnectionId6)
	{
		return hubClients.AllExcept(new string[6] { excludedConnectionId1, excludedConnectionId2, excludedConnectionId3, excludedConnectionId4, excludedConnectionId5, excludedConnectionId6 });
	}

	public static T AllExcept<T>(this IHubClients<T> hubClients, string excludedConnectionId1, string excludedConnectionId2, string excludedConnectionId3, string excludedConnectionId4, string excludedConnectionId5, string excludedConnectionId6, string excludedConnectionId7)
	{
		return hubClients.AllExcept(new string[7] { excludedConnectionId1, excludedConnectionId2, excludedConnectionId3, excludedConnectionId4, excludedConnectionId5, excludedConnectionId6, excludedConnectionId7 });
	}

	public static T AllExcept<T>(this IHubClients<T> hubClients, string excludedConnectionId1, string excludedConnectionId2, string excludedConnectionId3, string excludedConnectionId4, string excludedConnectionId5, string excludedConnectionId6, string excludedConnectionId7, string excludedConnectionId8)
	{
		return hubClients.AllExcept(new string[8] { excludedConnectionId1, excludedConnectionId2, excludedConnectionId3, excludedConnectionId4, excludedConnectionId5, excludedConnectionId6, excludedConnectionId7, excludedConnectionId8 });
	}

	public static T Clients<T>(this IHubClients<T> hubClients, string connection1)
	{
		return hubClients.Clients(new string[1] { connection1 });
	}

	public static T Clients<T>(this IHubClients<T> hubClients, string connection1, string connection2)
	{
		return hubClients.Clients(new string[2] { connection1, connection2 });
	}

	public static T Clients<T>(this IHubClients<T> hubClients, string connection1, string connection2, string connection3)
	{
		return hubClients.Clients(new string[3] { connection1, connection2, connection3 });
	}

	public static T Clients<T>(this IHubClients<T> hubClients, string connection1, string connection2, string connection3, string connection4)
	{
		return hubClients.Clients(new string[4] { connection1, connection2, connection3, connection4 });
	}

	public static T Clients<T>(this IHubClients<T> hubClients, string connection1, string connection2, string connection3, string connection4, string connection5)
	{
		return hubClients.Clients(new string[5] { connection1, connection2, connection3, connection4, connection5 });
	}

	public static T Clients<T>(this IHubClients<T> hubClients, string connection1, string connection2, string connection3, string connection4, string connection5, string connection6)
	{
		return hubClients.Clients(new string[6] { connection1, connection2, connection3, connection4, connection5, connection6 });
	}

	public static T Clients<T>(this IHubClients<T> hubClients, string connection1, string connection2, string connection3, string connection4, string connection5, string connection6, string connection7)
	{
		return hubClients.Clients(new string[7] { connection1, connection2, connection3, connection4, connection5, connection6, connection7 });
	}

	public static T Clients<T>(this IHubClients<T> hubClients, string connection1, string connection2, string connection3, string connection4, string connection5, string connection6, string connection7, string connection8)
	{
		return hubClients.Clients(new string[8] { connection1, connection2, connection3, connection4, connection5, connection6, connection7, connection8 });
	}

	public static T Groups<T>(this IHubClients<T> hubClients, string group1)
	{
		return hubClients.Groups(new string[1] { group1 });
	}

	public static T Groups<T>(this IHubClients<T> hubClients, string group1, string group2)
	{
		return hubClients.Groups(new string[2] { group1, group2 });
	}

	public static T Groups<T>(this IHubClients<T> hubClients, string group1, string group2, string group3)
	{
		return hubClients.Groups(new string[3] { group1, group2, group3 });
	}

	public static T Groups<T>(this IHubClients<T> hubClients, string group1, string group2, string group3, string group4)
	{
		return hubClients.Groups(new string[4] { group1, group2, group3, group4 });
	}

	public static T Groups<T>(this IHubClients<T> hubClients, string group1, string group2, string group3, string group4, string group5)
	{
		return hubClients.Groups(new string[5] { group1, group2, group3, group4, group5 });
	}

	public static T Groups<T>(this IHubClients<T> hubClients, string group1, string group2, string group3, string group4, string group5, string group6)
	{
		return hubClients.Groups(new string[6] { group1, group2, group3, group4, group5, group6 });
	}

	public static T Groups<T>(this IHubClients<T> hubClients, string group1, string group2, string group3, string group4, string group5, string group6, string group7)
	{
		return hubClients.Groups(new string[7] { group1, group2, group3, group4, group5, group6, group7 });
	}

	public static T Groups<T>(this IHubClients<T> hubClients, string group1, string group2, string group3, string group4, string group5, string group6, string group7, string group8)
	{
		return hubClients.Groups(new string[8] { group1, group2, group3, group4, group5, group6, group7, group8 });
	}

	public static T GroupExcept<T>(this IHubClients<T> hubClients, string groupName, string excludedConnectionId1)
	{
		return hubClients.GroupExcept(groupName, new string[1] { excludedConnectionId1 });
	}

	public static T GroupExcept<T>(this IHubClients<T> hubClients, string groupName, string excludedConnectionId1, string excludedConnectionId2)
	{
		return hubClients.GroupExcept(groupName, new string[2] { excludedConnectionId1, excludedConnectionId2 });
	}

	public static T GroupExcept<T>(this IHubClients<T> hubClients, string groupName, string excludedConnectionId1, string excludedConnectionId2, string excludedConnectionId3)
	{
		return hubClients.GroupExcept(groupName, new string[3] { excludedConnectionId1, excludedConnectionId2, excludedConnectionId3 });
	}

	public static T GroupExcept<T>(this IHubClients<T> hubClients, string groupName, string excludedConnectionId1, string excludedConnectionId2, string excludedConnectionId3, string excludedConnectionId4)
	{
		return hubClients.GroupExcept(groupName, new string[4] { excludedConnectionId1, excludedConnectionId2, excludedConnectionId3, excludedConnectionId4 });
	}

	public static T GroupExcept<T>(this IHubClients<T> hubClients, string groupName, string excludedConnectionId1, string excludedConnectionId2, string excludedConnectionId3, string excludedConnectionId4, string excludedConnectionId5)
	{
		return hubClients.GroupExcept(groupName, new string[5] { excludedConnectionId1, excludedConnectionId2, excludedConnectionId3, excludedConnectionId4, excludedConnectionId5 });
	}

	public static T GroupExcept<T>(this IHubClients<T> hubClients, string groupName, string excludedConnectionId1, string excludedConnectionId2, string excludedConnectionId3, string excludedConnectionId4, string excludedConnectionId5, string excludedConnectionId6)
	{
		return hubClients.GroupExcept(groupName, new string[6] { excludedConnectionId1, excludedConnectionId2, excludedConnectionId3, excludedConnectionId4, excludedConnectionId5, excludedConnectionId6 });
	}

	public static T GroupExcept<T>(this IHubClients<T> hubClients, string groupName, string excludedConnectionId1, string excludedConnectionId2, string excludedConnectionId3, string excludedConnectionId4, string excludedConnectionId5, string excludedConnectionId6, string excludedConnectionId7)
	{
		return hubClients.GroupExcept(groupName, new string[7] { excludedConnectionId1, excludedConnectionId2, excludedConnectionId3, excludedConnectionId4, excludedConnectionId5, excludedConnectionId6, excludedConnectionId7 });
	}

	public static T GroupExcept<T>(this IHubClients<T> hubClients, string groupName, string excludedConnectionId1, string excludedConnectionId2, string excludedConnectionId3, string excludedConnectionId4, string excludedConnectionId5, string excludedConnectionId6, string excludedConnectionId7, string excludedConnectionId8)
	{
		return hubClients.GroupExcept(groupName, new string[8] { excludedConnectionId1, excludedConnectionId2, excludedConnectionId3, excludedConnectionId4, excludedConnectionId5, excludedConnectionId6, excludedConnectionId7, excludedConnectionId8 });
	}

	public static T Users<T>(this IHubClients<T> hubClients, string user1)
	{
		return hubClients.Users(new string[1] { user1 });
	}

	public static T Users<T>(this IHubClients<T> hubClients, string user1, string user2)
	{
		return hubClients.Users(new string[2] { user1, user2 });
	}

	public static T Users<T>(this IHubClients<T> hubClients, string user1, string user2, string user3)
	{
		return hubClients.Users(new string[3] { user1, user2, user3 });
	}

	public static T Users<T>(this IHubClients<T> hubClients, string user1, string user2, string user3, string user4)
	{
		return hubClients.Users(new string[4] { user1, user2, user3, user4 });
	}

	public static T Users<T>(this IHubClients<T> hubClients, string user1, string user2, string user3, string user4, string user5)
	{
		return hubClients.Users(new string[5] { user1, user2, user3, user4, user5 });
	}

	public static T Users<T>(this IHubClients<T> hubClients, string user1, string user2, string user3, string user4, string user5, string user6)
	{
		return hubClients.Users(new string[6] { user1, user2, user3, user4, user5, user6 });
	}

	public static T Users<T>(this IHubClients<T> hubClients, string user1, string user2, string user3, string user4, string user5, string user6, string user7)
	{
		return hubClients.Users(new string[7] { user1, user2, user3, user4, user5, user6, user7 });
	}

	public static T Users<T>(this IHubClients<T> hubClients, string user1, string user2, string user3, string user4, string user5, string user6, string user7, string user8)
	{
		return hubClients.Users(new string[8] { user1, user2, user3, user4, user5, user6, user7, user8 });
	}
}
