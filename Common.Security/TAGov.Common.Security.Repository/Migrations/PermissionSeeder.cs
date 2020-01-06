using System.Linq;
using TAGov.Common.Security.Repository.Models;

namespace TAGov.Common.Security.Repository.Migrations
{
	public class PermissionSeeder
	{
		private readonly AumentumSecurityContext _aumentumSecurityContext;
		private int _id;

		public int NextId()
		{
			_id += 1;
			return _id;
		}

		public PermissionSeeder(AumentumSecurityContext aumentumSecurityContext)
		{
			_aumentumSecurityContext = aumentumSecurityContext;
			_id = _aumentumSecurityContext.Permissions.Max(x => x.Id);
		}

		public static string ToApplicationName(string name)
		{
			const string prefix = "api.";

			return prefix + name.Replace(" ", "") + "Service";
		}

		public bool IsExist(AppFunction appFunction)
		{
			return IsExist(appFunction.Name, appFunction.App, appFunction.AppFunctionType);
		}

		public bool IsExist(string name, string app, string appFunctionType)
		{
			return _aumentumSecurityContext.Permissions.Any(x => x.Name == name && x.App == app &&
																 x.AppFunctionType == appFunctionType);
		}

		public AppFunction CreateApplication(int id, string name)
		{
			var concatName = ToApplicationName(name);

			return new AppFunction
			{
				Id = id,
				App = concatName,
				AppFunctionType = "Application",
				LongDescription = $"This represents the application service {name}.",
				Name = concatName,
				ParentId = 0,
				ParentName = "",
				FieldValue = ""
			};
		}

		public static string ToResourceName(string name)
		{
			return name.Replace(" ", "");
		}

		public AppFunction[] CreateFields(int parentId, string parentName, string name)
		{
			var concatName = ToResourceName(name);

			return new[]{
				new AppFunction
				{
					Id = NextId(),
					App = parentName,
					AppFunctionType = "field",
					LongDescription = $"This represents the resource {name}.",
					Name = concatName,
					ParentId = parentId,
					ParentName = parentName,
					FieldValue = ""
				}
			};
		}
	}
}
