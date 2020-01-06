namespace TAGov.Search
{
  public class MyWorklistSearchProxy : IMyWorklistSearchProxy
  {
    public string Search(string searchCriteria)
    {
      return "MyWorklistSearch was called with criteria " + searchCriteria;
    }
  }
}
