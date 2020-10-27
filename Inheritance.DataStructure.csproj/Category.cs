using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public readonly string Name;
        public readonly MessageType MessageType;
        public readonly MessageTopic MessageTopic;

        public Category(string name, MessageType messageType, MessageTopic messageTopic)
        {
            Name = name;
            MessageType = messageType;
            MessageTopic = messageTopic;
        }
        
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case Category otherCategory:
                    return this.CompareTo(otherCategory) == 0;
                default:
                    return false;
            }
        }

        public override int GetHashCode()
        {
            return Tuple.Create(this.Name, this.MessageType, this.MessageTopic).GetHashCode();
        }

        public override string ToString()
        {
            var sB = new StringBuilder();
            sB.Append(this.Name);
            sB.Append('.');
            sB.Append(this.MessageType);
            sB.Append('.');
            sB.Append(this.MessageTopic);
            return sB.ToString();
        }

        public int CompareTo(object obj)
        {
            switch (obj)
            {
                case null:
                    return 1;
                case Category otherCategory:
                    var nameCompare = this.Name?.CompareTo(otherCategory.Name) ?? 0;
                    if (nameCompare != 0) return nameCompare;
                    var typeCompare = this.MessageType.CompareTo(otherCategory.MessageType);
                    if (typeCompare != 0) return typeCompare;
                    var topicCompare = this.MessageTopic.CompareTo(otherCategory.MessageTopic);
                    return topicCompare;
                default:
                    throw new ArgumentException("Object is not a Category");
            }
        }

        public static bool operator >(Category cat1, Category cat2)
        {
            return cat1.CompareTo(cat2) > 0;
        }
        
        public static bool operator <(Category cat1, Category cat2)
        {
            return cat1.CompareTo(cat2) < 0;
        }
        
        public static bool operator >=(Category cat1, Category cat2)
        {
            return cat1.CompareTo(cat2) >= 0;
        }
        
        public static bool operator <=(Category cat1, Category cat2)
        {
            return cat1.CompareTo(cat2) <= 0;
        }
    }
}