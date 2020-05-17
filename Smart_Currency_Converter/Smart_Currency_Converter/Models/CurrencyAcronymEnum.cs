using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Model.Smart_Currency_Converter
{
    public class CurrencyAcronymEnum
    {
        public CurrencyAcronymEnum()
        {

            // Create a dynamic assembly in the current application domain
            AssemblyName enumAssemblyName = new AssemblyName("CurrencyEnumAssembly");
            AssemblyBuilder currencyEnumAssembly = AssemblyBuilder.DefineDynamicAssembly(enumAssemblyName, AssemblyBuilderAccess.RunAndCollect);

            ModuleBuilder moduleBuilder = currencyEnumAssembly.DefineDynamicModule(enumAssemblyName.Name);
            // Define a public enumeration called "CurrencyAcronym"
            EnumBuilder enumBuilder = moduleBuilder.DefineEnum("CurrencyAcronym", TypeAttributes.Public, typeof(int));

            enumBuilder.DefineLiteral("Low", 0);
            enumBuilder.DefineLiteral("High", 1);


            // Create the type and save the assembly
            Type currencyEnum = enumBuilder.CreateTypeInfo();
            
            Array array = Enum.GetValues(currencyEnum);
            string[] names = Enum.GetNames(currencyEnum);
            string name = Enum.GetName(currencyEnum, array.GetValue(1));
        }
    }
}
