using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole.Tests
{
    /// <summary>
    /// This class allows us to track cross cutting concerns spread across multiple classes. 
    /// </summary>
    public static class CrossCuttingConcerns
    {
        public const string Scrolling = "Scrolling"; // everything related to scrolling and clipping
    }
}
