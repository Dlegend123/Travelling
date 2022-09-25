using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Travelling
{
    [MemoryDiagnoser(false)]
    public class Benchmark
    {
        [Benchmark]
        public void RunOpen()
        {
            //Program.Open();
        }
        [Benchmark]
        public void RunStory()
        {
           // Program.StoryMode();
        }
    }
    
}
