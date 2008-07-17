using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoSharp.Core
{
    public static class Message
    {
        public static void ForEachField(Type messageType, Action<MessageField> action)
        {
            Array.ForEach(messageType.GetProperties(), field =>
            {
                var attributes = field.GetCustomAttributes(typeof(TagAttribute), false);
                if(attributes.Length == 0)
                    return;
                var tag = attributes[0] as TagAttribute;
                action(MessageField.Create(tag, field));
            });
        }

        public static int CountFields(object obj)
        {
            int count = 0;
            ForEachField(obj.GetType(), x => ++count);
            return count;
        }
    }
}
