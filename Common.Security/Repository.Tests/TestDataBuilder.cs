using TAGov.Common.Security.Repository.Models;

namespace TAGov.Common.Security.Repository.Tests
{
  public static class TestDataBuilder
  {
    public const int User1Id = 1;
    public const int User2Id = 2;
    public const int User3Id = 3;
    public const int User4Id = 4;
    public const int User5Id = 5;

    public const int App1Id = 1;
    public const int App2Id = 2;
    public const int App3Id = 3;
    public const int App4Id = 4;
    public const int App5Id = 5;

    public static void Build(AumentumSecurityQueryContext aumentumSecurityQueryContext)
    {
      // app1 has 2 resources
      // app2 only has 1 resource.

      aumentumSecurityQueryContext.Permissions.Add(new AppFunctionForQuery
      {
        Id = App1Id,
        App = "api.app1",
        Name = "api.app1",
        AppFunctionType = "application",
        ParentName = null,
        ParentId = 0
      });

      aumentumSecurityQueryContext.Permissions.Add(new AppFunctionForQuery
      {
        Id = App2Id,
        App = "api.app1",
        Name = "res1",
        AppFunctionType = "field",
        ParentName = "api.app1",
        ParentId = App1Id
      });

      // This should be unused as it is not assigned to any roles.
      aumentumSecurityQueryContext.Permissions.Add(new AppFunctionForQuery
      {
        Id = App3Id,
        App = "api.app1",
        Name = "res2",
        AppFunctionType = "field",
        ParentName = "api.app1",
        ParentId = App1Id
      });


      aumentumSecurityQueryContext.Permissions.Add(new AppFunctionForQuery
      {
        Id = App4Id,
        App = "api.app2",
        Name = "api.app2",
        AppFunctionType = "application",
        ParentName = null,
        ParentId = 0
      });

      aumentumSecurityQueryContext.Permissions.Add(new AppFunctionForQuery
      {
        Id = App5Id,
        App = "api.app2",
        Name = "res3",
        AppFunctionType = "field",
        ParentName = "api.app2",
        ParentId = App4Id
      });

      // role 1 has permission on application level only for app1.
      // role 2 has permssion on field level only for app1.
      // role 3 has permissions on both application and field levels for app1.
      // role 4 has permissions on both application and field levels for app2.

      aumentumSecurityQueryContext.RolesPermissions.Add(new RoleFunction
      {
        Id = 1,
        AppFunctionId = 1,
        CanCreate = 1,
        CanModify = 1,
        CanView = 1,
        CanDelete = 1,
        RoleId = 1
      });

      aumentumSecurityQueryContext.RolesPermissions.Add(new RoleFunction
      {
        Id = 2,
        AppFunctionId = 2,
        CanCreate = 1,
        CanModify = 1,
        CanView = 1,
        CanDelete = 1,
        RoleId = 2
      });

      aumentumSecurityQueryContext.RolesPermissions.Add(new RoleFunction
      {
        Id = 3,
        AppFunctionId = 1,
        CanCreate = 1,
        CanModify = 1,
        CanView = 1,
        CanDelete = 1,
        RoleId = 3
      });

      aumentumSecurityQueryContext.RolesPermissions.Add(new RoleFunction
      {
        Id = 4,
        AppFunctionId = 2,
        CanCreate = 1,
        CanModify = 1,
        CanView = 1,
        CanDelete = 1,
        RoleId = 3
      });

      aumentumSecurityQueryContext.RolesPermissions.Add(new RoleFunction
      {
        Id = 5,
        AppFunctionId = 4,
        CanCreate = 1,
        CanModify = 1,
        CanView = 1,
        CanDelete = 1,
        RoleId = 4
      });

      aumentumSecurityQueryContext.RolesPermissions.Add(new RoleFunction
      {
        Id = 6,
        AppFunctionId = 5,
        CanCreate = 1,
        CanModify = 1,
        CanView = 1,
        CanDelete = 1,
        RoleId = 4
      });

      // User 1 is assigned role 1.
      // User 2 is assigned role 2.
      // User 3 is assigned role 3.
      // User 4 is assigned role 1 and 2.
      // User 5 is assigned role 3 and role 4.

      aumentumSecurityQueryContext.UsersRoles.Add(new UserProfileRole
      {
        Id = 1,
        RoleId = 1,
        UserProfileId = User1Id
      });

      aumentumSecurityQueryContext.UsersRoles.Add(new UserProfileRole
      {
        Id = 2,
        RoleId = 2,
        UserProfileId = User2Id
      });

      aumentumSecurityQueryContext.UsersRoles.Add(new UserProfileRole
      {
        Id = 3,
        RoleId = 3,
        UserProfileId = User3Id
      });

      aumentumSecurityQueryContext.UsersRoles.Add(new UserProfileRole
      {
        Id = 4,
        RoleId = 1,
        UserProfileId = User4Id
      });

      aumentumSecurityQueryContext.UsersRoles.Add(new UserProfileRole
      {
        Id = 5,
        RoleId = 2,
        UserProfileId = User4Id
      });

      aumentumSecurityQueryContext.UsersRoles.Add(new UserProfileRole
      {
        Id = 6,
        RoleId = 3,
        UserProfileId = User5Id
      });

      aumentumSecurityQueryContext.UsersRoles.Add(new UserProfileRole
      {
        Id = 7,
        RoleId = 4,
        UserProfileId = User5Id
      });

      aumentumSecurityQueryContext.UserLogins.Add(new UserProfileLogin
      {
        Id = User1Id,
        UserProfileId = User1Id
      });

      aumentumSecurityQueryContext.UserLogins.Add(new UserProfileLogin
      {
        Id = User2Id,
        UserProfileId = User2Id
      });

      aumentumSecurityQueryContext.UserLogins.Add(new UserProfileLogin
      {
        Id = User3Id,
        UserProfileId = User3Id
      });

      aumentumSecurityQueryContext.UserLogins.Add(new UserProfileLogin
      {
        Id = User4Id,
        UserProfileId = User4Id
      });

      aumentumSecurityQueryContext.UserLogins.Add(new UserProfileLogin
      {
        Id = User5Id,
        UserProfileId = User5Id
      });

      aumentumSecurityQueryContext.SaveChanges();
    }
  }
}
