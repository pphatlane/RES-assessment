using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RES.Test.Unit
{
    [CollectionDefinition("Fixture collection")]
    public class TestCollection : ICollectionFixture<TestStartup<Startup>>
    {
    }
}
