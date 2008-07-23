using ProtoSharp.Performance.Messages;
using System;

namespace ProtoSharp.Performance.Benchmarks
{
    class PersonDeserializationBenchmark : DeserializationBenchmarkBase<Person>
    {
        public PersonDeserializationBenchmark(int iterations) : base(iterations) { }

        protected override string Name { get { return "Person Deserialization"; } }

        protected override void SerializeExemplar(IBenchmarkAdapter target)
        {
            var person = new Person()
            {
                Id = 4,
                Name = "Some 1",
                Email = "some@one.com",
            };
            person.Phones.Add(new Person.PhoneNumber() { Number = "555-111 222 333", Type = Person.PhoneType.Mobile });
            person.Phones.Add(new Person.PhoneNumber() { Number = "555-222 333 444", Type = Person.PhoneType.Home });
            person.Phones.Add(new Person.PhoneNumber() { Number = "555-333 444 555", Type = Person.PhoneType.Work });
            target.Serialize(person);
        }

        protected override void Deserialize(IBenchmarkAdapter target, out Person item)
        {
            target.Deserialize(out item);
        }
    }
}
