using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class PersonSerializationBenchmark : SerializationBenchmarkBase<Person>
    {
        public PersonSerializationBenchmark(int iterations)
            : base(1, 0, iterations)
        { }

        protected override string Name { get { return "Person Serialization"; } }

        protected override void Serialize(IBenchmarkAdapter target, Person value)
        {
            target.Serialize(value);
        }

        protected override Person Generate(Random rand)
        {
            _testPersons.MoveNext();
            return _testPersons.Current;
        }

        static IEnumerator<Person> GetTestPersons()
        {
            yield return new Person()
            {
                Id = 1,
            };
            yield return new Person()
            {
                Id = 2,
                Name = "Jane Doe",
                Email = "jane@unkown.com"
            };
            yield return new Person()
            {
                Id = 3,
                Email = "random@example.com"
            };
            yield break;
        }

        IEnumerator<Person> _testPersons = GetTestPersons();
    }
}
