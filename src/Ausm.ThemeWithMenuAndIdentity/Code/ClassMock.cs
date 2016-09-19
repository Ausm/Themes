using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Ausm.ThemeWithMenuAndIdentity
{
    static class ClassMockStatics
    {
        static ModuleBuilder _moduleBuilder = null;

        public static ModuleBuilder ModuleBuilder
        {
            get
            {
                if (_moduleBuilder != null)
                    return _moduleBuilder;

                AssemblyName assemblyName = new AssemblyName("MockClasses");
                AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("MockClasses"), AssemblyBuilderAccess.RunAndCollect);
                _moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name + ".dll");
                return _moduleBuilder;
            }
        }
    }

    class ClassMock<T>
        where T : class
    {
        #region Fields
        static TypeInfo _type = null;
        static ConstructorInfo _constructor = null;
        T _value;
        #endregion

        #region Constructors
        public ClassMock()
        {
            if (_type == null)
            {
                TypeBuilder typeBuilder = ClassMockStatics.ModuleBuilder.DefineType($"{typeof(T).Namespace}.MockClassesDynamic.{typeof(T).Name}");
                typeBuilder.SetParent(typeof(T));

                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                    .Where(x => x.GetGetMethod()?.IsAbstract == true))
                {
                    MethodInfo getBaseMethod = propertyInfo.GetGetMethod();
                    MethodInfo setBaseMethod = propertyInfo.CanWrite ? propertyInfo.GetSetMethod() : null;

                    if (setBaseMethod == null)
                    {
                        MethodBuilder getMethod = typeBuilder.DefineMethod("get_" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual, propertyInfo.PropertyType, Type.EmptyTypes);
                        ILGenerator generator = getMethod.GetILGenerator();

                        if (propertyInfo.PropertyType.GetTypeInfo().IsPrimitive)
                            generator.Emit(OpCodes.Ldc_I4_0);
                        else if (propertyInfo.PropertyType.GetTypeInfo().IsValueType)
                            generator.Emit(OpCodes.Initobj, propertyInfo.PropertyType);
                        else
                            generator.Emit(OpCodes.Ldnull);

                        generator.Emit(OpCodes.Ret);
                        typeBuilder.DefineMethodOverride(getMethod, getBaseMethod);
                    }
                    else
                    {
                        FieldBuilder field = typeBuilder.DefineField($"_{propertyInfo.Name}", propertyInfo.PropertyType, FieldAttributes.Private);
                        MethodBuilder getMethod = typeBuilder.DefineMethod("get_" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual, propertyInfo.PropertyType, Type.EmptyTypes);
                        MethodBuilder setMethod =  typeBuilder.DefineMethod("set_" + propertyInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual, null, new Type[] { propertyInfo.PropertyType });

                        ILGenerator generator = setMethod.GetILGenerator();
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldarg_1);
                        generator.Emit(OpCodes.Stfld, field);
                        generator.Emit(OpCodes.Ret);

                        typeBuilder.DefineMethodOverride(setMethod, setBaseMethod);

                        generator = getMethod.GetILGenerator();
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldfld, field);
                        generator.Emit(OpCodes.Ret);

                        typeBuilder.DefineMethodOverride(getMethod, getBaseMethod);
                    }
                }
                _type = typeBuilder.CreateTypeInfo();
                _constructor = _type.GetConstructors().First();
            }

            _value = (T)_constructor.Invoke(new object[] { });
        }
        #endregion

        #region Properties
        public T Value => _value;
        #endregion
    }
}
