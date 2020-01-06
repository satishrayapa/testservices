using System.Collections.Generic;
using System.Linq;
using TAGov.Services.Core.BaseValueSegment.Repository.Interfaces.V1;
using TAGov.Services.Core.BaseValueSegment.Repository.Models.V1;

namespace TAGov.Services.Core.BaseValueSegment.Repository.Implementation.V1
{
  public class CaliforniaConsumerPriceIndexRepository : ICaliforniaConsumerPriceIndexRepository
  {
    private readonly AumentumContext _aumentumContext;
    private readonly int _stCntyWide;

    public CaliforniaConsumerPriceIndexRepository(AumentumContext aumentumContext,
                                                  ISysTypeRepository sysTypeRepository)
    {
      _aumentumContext = aumentumContext;
      _stCntyWide = sysTypeRepository.GetSysTypeId("CntyWide", "CntyWide");
    }

    public IEnumerable<CaliforniaConsumerPriceIndex> List()
    {
      return _aumentumContext.CaliforniaConsumerPriceIndexes.Where(x => x.ValueType.ShortDescr == "CPI" && x.ObjectId == _stCntyWide);
    }

    public CaliforniaConsumerPriceIndex GetByYear(int assessmentYear)
    {
      var priceIndexes = _aumentumContext.CaliforniaConsumerPriceIndexes;
      var californiaConsumerPriceIndexRow = ( from pi in priceIndexes
                                              where pi.ValueType.ShortDescr == "CPI"
                                                    && pi.ObjectId == _stCntyWide
                                                    && pi.AssessmentYear == assessmentYear

                                                    && pi.EffStatus == "A"
                                                    && pi.BeginEffectiveYear == (
                                                                                  from sub in priceIndexes
                                                                                  where sub.Id == pi.Id
                                                                                        && sub.BeginEffectiveYear <= assessmentYear
                                                                                  select sub
                                                                                ).Max( y => y.BeginEffectiveYear )

                                              select pi ).SingleOrDefault();

      return californiaConsumerPriceIndexRow;
    }
  }
}
