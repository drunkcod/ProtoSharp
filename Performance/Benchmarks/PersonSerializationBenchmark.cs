using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProtoSharp.Performance.Messages;

namespace ProtoSharp.Performance.Benchmarks
{
    class PersonSerializationBenchmark : SerializationBenchmarkBase<Person>
    {
        public PersonSerializationBenchmark(int iterations)
            : base(4, 0, iterations)
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
            var four = new Person()
            {
                Id = 4,
                Name = "Some 1",
                Email = "some@one.com",
            };
            four.Phones.Add(new Person.PhoneNumber(){ Number = "555-111 222 333", Type = Person.PhoneType.Mobile});
            four.Phones.Add(new Person.PhoneNumber(){ Number = "555-222 333 444", Type = Person.PhoneType.Home});
            four.Phones.Add(new Person.PhoneNumber(){ Number = "555-333 444 555", Type = Person.PhoneType.Work});
            yield return four;
            yield break;
        }

        IEnumerator<Person> _testPersons = GetTestPersons();
    }
}
