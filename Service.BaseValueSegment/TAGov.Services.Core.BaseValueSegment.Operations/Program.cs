﻿using TAGov.Common.Operations;

namespace TAGov.Services.Core.BaseValueSegment.Operations
{
  public class Program
  {
    public static void Main( string[] args )
    {
      Bootstrap.Execute( args, new Operations() );
    }
  }
}
