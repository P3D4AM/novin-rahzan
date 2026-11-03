using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Global;
using EntityCache.Cache.Repository.Global;
using EntityCache.Core;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Global;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Global;

internal class PostPersistenceRepository : PersistenceRepository<PostBusiness, Post>, IPostRepository, IRepository<PostBusiness>, INoGuidRepository<PostBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Post, PostBusiness>, IItemAsyncMapper<PostBusiness, Post>
	{
		public Task<PostBusiness> MapAsync(string contextGuid, Post source, PostBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
					return Task.FromResult(destination);
				}
				if (destination == null)
				{
					destination = new PostBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Startdate = source.Startdate;
				destination.Status = source.Status;
				destination.EndDate = source.EndDate;
				destination.Title = source.Title;
				destination.Type = source.Type;
				destination.Modified = source.Modified;
				destination.Link = source.Link;
				destination.Image = source.Image;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PostPersistenceRepository.cs", 66);
			}
			return Task.FromResult(destination);
		}

		public Task<Post> MapAsync(string contextGuid, PostBusiness source, Post destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
					return Task.FromResult(destination);
				}
				if (destination == null)
				{
					destination = new Post();
				}
				destination.Startdate = source.Startdate;
				destination.Guid = source.Guid;
				destination.Status = source.Status;
				destination.EndDate = source.EndDate;
				destination.Title = source.Title;
				destination.Type = source.Type;
				destination.Modified = source.Modified;
				destination.Link = source.Link;
				destination.Image = source.Image;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PostPersistenceRepository.cs", 90);
			}
			return Task.FromResult(destination);
		}

		public Task<List<PostBusiness>> MapListAsync(string contextGuid, IEnumerable<Post> source, List<PostBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Post>> MapListAsync(string contextGuid, IEnumerable<PostBusiness> source, List<Post> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Post, PostBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Post, PostBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PostBusiness, Post> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	internal PostPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\PostPersistenceRepository.cs", 39);
		}
	}
}
