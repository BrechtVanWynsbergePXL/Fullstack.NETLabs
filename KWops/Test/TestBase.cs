using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public abstract class TestBase
    {
        protected Random Random;

        protected TestBase()
        {
            Random = new Random();
        }
    }
}
