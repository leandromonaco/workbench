using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace CodeKata.CSharp.Helpers.Benchmark
{
    [MemoryDiagnoser(false)]
    public class Kata3_BenchmarkHelper
    {
        private static readonly Random Rng = new(100);

        public int Size { get; set; } = 100;

        private List<int> _items;

        [GlobalSetup]
        public void Setup()
        {
            _items = Enumerable.Range(1, Size).Select(x => Rng.Next()).ToList();
        }

        [Benchmark]
        public void ForEach()
        {
            foreach (var item in _items)
            {
            }
        }
    }
}
