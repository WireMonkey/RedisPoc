using MessagePack;
using System;

namespace RedisPoc.poco
{
    [MessagePackObject]
    [Serializable]
    public class PersonData
    {
        [Key(0)]
        public string FirstName { get; set; }
        [Key(1)]
        public string LastName { get; set; }
        [Key(2)]
        public string Address { get; set; }
        [Key(3)]
        public string City { get; set; }
        [Key(4)]
        public string State { get; set; }
        [Key(5)]
        public string Zip { get; set; }
        [Key(6)]
        public string Ssn { get; set; }
        [Key(7)]
        public string Gender { get; set; }
        [Key(8)]
        public string Age { get; set; }
        [Key(9)]
        public string Birthday { get; set; }
    }
}
