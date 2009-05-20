namespace ProtoSharp.Core
{
    using System;
    using System.Reflection.Emit;
    using System.Text;

    class MethodMissingException : Exception
    {
        readonly string methodName;
        readonly Type[] args;

        public MethodMissingException(string methodName, Type[] args)
        {
            this.methodName = methodName;
            this.args = args;
        }

        public override string  Message
        {
	        get 
            {
                var message = new StringBuilder("Could not find method \"");
                message.AppendFormat("{0}(", methodName);
                var format = "{0}";
                for (int i = 0; i != args.Length; ++i)
                {
                    message.AppendFormat(format, args[i].Name);
                    format = ", {0}";
                }
                message.Append(")\"");
                return message.ToString(); 
            }
        }
    }

    static class ILGeneratorExtensions
    {
        public static ILGenerator Call<T>(this ILGenerator il, string methodName, params Type[] types)
        {
            var method = typeof(T).GetMethod(methodName, types);
            if (method == null)
                throw new MethodMissingException(methodName, types);
            il.Emit(OpCodes.Call, method);
            return il;
        }

        public static ILGenerator Pop(this ILGenerator il) 
        { 
            il.Emit(OpCodes.Pop);
            return il;
        }
    }
}