namespace ProtoSharp.Core
{
    using System;
    using System.Reflection.Emit;

    class MethodMissingException : Exception
    {
        readonly string methodName; 

        public MethodMissingException(string methodName)
        {
            this.methodName = methodName;
        }

        public override string  Message
        {
	        get { return string.Format("Could not find method \"{0}\".", methodName); }
        }
    }

    static class ILGeneratorExtensions
    {
        public static void Call<T>(this ILGenerator il, string methodName, params Type[] types)
        {
            var method = typeof(T).GetMethod(methodName, types);
            if (method == null)
                throw new MethodMissingException(methodName);
            il.Emit(OpCodes.Call, method);
        }
    }
}