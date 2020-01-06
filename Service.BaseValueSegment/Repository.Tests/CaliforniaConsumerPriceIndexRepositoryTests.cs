using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TAGov.Services.Core.BaseValueSegment.Repository.Implementation.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;
using Xunit;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Tests
{
  public class CaliforniaConsumerPriceIndexRepositoryTests
  {
    private readonly AumentumContext _context;
    private readonly ISysTypeRepository _sysTypeRepository;
    private const int AssessmentYearValue = 1980;
    private const decimal InflationFactorValue = ( decimal ) 1.2;

    public CaliforniaConsumerPriceIndexRepositoryTests()
    {
      var optionsBuilder = new DbContextOptionsBuilder<AumentumContext>();
      optionsBuilder.UseInMemoryDatabase( Guid.NewGuid().ToString( "N" ) );

      _context = new AumentumContext( optionsBuilder.Options );
      _sysTypeRepository = new SysTypeRepository( _context );

      var effDate = new DateTime( 1776, 7, 4 );

      var stcCntyWide = new SysTypeCat
                        {
                          Id = 10600,
                          ShortDescr = "CntyWide",
                          BegEffDate = effDate,
                          EffStatus = "A"
                        };
      _context.Add( stcCntyWide );

      var stCntyWide = new SystemType
                       {
                         Id = 16051,
                         ShortDescr = "CntyWide",
                         SysTypeCatId = stcCntyWide.Id,
                         BeginEffectiveDate = effDate,
                         EffectiveStatus = "A"
                       };
      _context.Add( stCntyWide );

      var vtCPI = new Models.V1.ValueType
                  {
                    Id = 1010001,
                    ShortDescr = "CPI",
                    Descr = "CPI"
                  };
      _context.Add( vtCPI );

      _context.Add( new CaliforniaConsumerPriceIndex
                    {
                      Id = 1,
                      AssessmentYear = AssessmentYearValue,
                      InflationFactor = InflationFactorValue,
                      ValueTypeId = vtCPI.Id,
                      ObjectId = stCntyWide.Id
                    } );
      _context.Add( new CaliforniaConsumerPriceIndex
                    {
                      Id = 2,
                      AssessmentYear = AssessmentYearValue + 1,
                      InflationFactor = InflationFactorValue,
                      ValueTypeId = vtCPI.Id,
                      ObjectId = stCntyWide.Id

                    } );
      _context.SaveChanges();
    }

    #region Get Tests

    [Fact]
    public void GetCaConsumerPriceIndexByYear_MatchYear()
    {
      ICaliforniaConsumerPriceIndexRepository repository = new CaliforniaConsumerPriceIndexRepository( _context, _sysTypeRepository );
      var caConsumerPriceIndex = repository.GetByYear( AssessmentYearValue );
      caConsumerPriceIndex.ShouldNotBeNull();
      caConsumerPriceIndex.InflationFactor.ShouldBe( InflationFactorValue );
    }

    [Fact]
    public void GetCaConsumerPriceIndexByYear_DoesMatchYear()
    {
      ICaliforniaConsumerPriceIndexRepository repository = new CaliforniaConsumerPriceIndexRepository( _context, _sysTypeRepository );
      var caConsumerPriceIndex = repository.GetByYear( AssessmentYearValue - 1 );
      caConsumerPriceIndex.ShouldBeNull();
    }

    #endregion

    #region Get All Tests

    [Fact]
    public void GetAllCaConsumerPriceIndex()
    {
      ICaliforniaConsumerPriceIndexRepository repository = new CaliforniaConsumerPriceIndexRepository( _context, _sysTypeRepository );
      var caConsumerPriceIndexes = repository.List().ToList();
      caConsumerPriceIndexes.ShouldNotBeEmpty();
      caConsumerPriceIndexes.Count().ShouldBe( 2 );
    }

    #endregion

  }
}
