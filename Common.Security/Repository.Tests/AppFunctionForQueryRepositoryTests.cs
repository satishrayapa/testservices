using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TAGov.Common.Security.Repository;
using TAGov.Common.Security.Repository.Implementation;
using TAGov.Common.Security.Repository.Models;
using TAGov.Common.Security.Repository.Tests;
using Xunit;

namespace Repository.Tests
{
  public class AppFunctionForQueryRepositoryTests
  {
    private readonly AumentumSecurityQueryContext _aumentumSecurityQueryContext;

    public AppFunctionForQueryRepositoryTests()
    {
      var optionsBuilder = new DbContextOptionsBuilder<AumentumSecurityQueryContext>();
      optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString("N"));

      _aumentumSecurityQueryContext = new AumentumSecurityQueryContext(optionsBuilder.Options);

      TestDataBuilder.Build(_aumentumSecurityQueryContext);
    }

    [Fact]
    public void GetAllFieldsForApplicationsShouldReturnAllFields()
    {
      var allApplicationAppFunctionIds = new List<int> { TestDataBuilder.App1Id, TestDataBuilder.App4Id };
      var appFunctionForQueryRepository = new AppFunctionForQueryRepository(_aumentumSecurityQueryContext);
      IEnumerable<AppFunctionForQuery> fields =
        appFunctionForQueryRepository.GetAllFieldAppFunctionsForApplicationLevelAppFunctions(allApplicationAppFunctionIds)
                            .ToArray();

      fields.Count().ShouldBe(3);
      AppFunctionForQuery app1Field1 = fields.Single(x => x.Id == TestDataBuilder.App2Id);
      app1Field1.Name.ShouldBe("res1");
      app1Field1.ParentId.ShouldBe(TestDataBuilder.App1Id);
      app1Field1.App.ShouldBe("api.app1");
      app1Field1.AppFunctionType.ShouldBe("field");
      app1Field1.ParentName.ShouldBe("api.app1");

      AppFunctionForQuery app1Field2 = fields.Single(x => x.Id == TestDataBuilder.App3Id);
      app1Field2.Name.ShouldBe("res2");
      app1Field2.ParentId.ShouldBe(TestDataBuilder.App1Id);
      app1Field2.App.ShouldBe("api.app1");
      app1Field2.AppFunctionType.ShouldBe("field");
      app1Field2.ParentName.ShouldBe("api.app1");

      AppFunctionForQuery app2Field1 = fields.Single(x => x.Id == TestDataBuilder.App5Id);
      app2Field1.Name.ShouldBe("res3");
      app2Field1.ParentId.ShouldBe(TestDataBuilder.App4Id);
      app2Field1.App.ShouldBe("api.app2");
      app2Field1.AppFunctionType.ShouldBe("field");
      app2Field1.ParentName.ShouldBe("api.app2");
    }
  }
}
