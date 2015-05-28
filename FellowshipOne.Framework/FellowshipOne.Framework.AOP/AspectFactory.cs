/*
 * AOP（Aspect-Oriented Programming）
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FellowshipOne.Framework.AOP
{
    /// <summary>
    /// 切向工厂 - aspect factory
    /// </summary>
    public class AspectFactory
    {
        #region Const

        /// <summary>
        /// 程序集名称 - assembly name
        /// </summary>
        private const string ASSEMBLY_NAME = "EmitWraper";

        /// <summary>
        /// 方法名称前缀 - prefix of module name
        /// </summary>
        private const string MODULE_NAME = "EmitModule_";

        /// <summary>
        /// 类型名称前缀 - prefix of type
        /// </summary>
        private const string TYPE_NAME = "Emit_";


        private const TypeAttributes TYPE_ATTRIBUTES = TypeAttributes.Public | TypeAttributes.Class;

        private const MethodAttributes METHOD_ATTRIBUTES = MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual;

        #endregion

        #region Core
        /// <summary>
        /// 返回一个线程安全的Hashtable
        /// </summary>
        private static readonly Hashtable typeCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// create instance from T
        /// </summary>
        /// <typeparam name="T">需要创建的类型</typeparam>
        /// <param name="parameters">类型参数</param>
        /// <returns></returns>
        public static T CreateInstance<T>(params object[] parameters) where T : class, new()
        {
            Type baseType = typeof(T);
            Type proxyType = typeCache[baseType] as Type;

            if (proxyType == null)
            {
                lock (typeCache.SyncRoot)
                {
                    proxyType = BuilderType(baseType);
                    typeCache.Add(baseType, proxyType);
                }
            }

            return (T)Activator.CreateInstance(proxyType, parameters);
        }
        #endregion

        #region build proxy type

        #region BuilderType

        private static Type BuilderType(Type baseType)
        {
            //Define Assembly and Module
            AssemblyName assemblyName = new AssemblyName { Name = ASSEMBLY_NAME };
            assemblyName.SetPublicKey(Assembly.GetExecutingAssembly().GetName().GetPublicKey());

            AssemblyBuilder assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder module = assembly.DefineDynamicModule(MODULE_NAME + baseType.Name);

            //创建TypeBuilder, 将要创建的类继承自 baseType<用户传入的Type>
            TypeBuilder typeBuilder = module.DefineType(TYPE_NAME + baseType.Name, TYPE_ATTRIBUTES, baseType);

            //构建：构造函数
            BuildConstructor(baseType, typeBuilder);

            //构建：方法（使用AOP的方法必须是虚方法）
            BuildMethod(baseType, typeBuilder);

            //创建类型
            Type type = typeBuilder.CreateType();
            return type;
        }
        #endregion

        #region BuildConstructor
        /// <summary>
        /// 构建构造函数
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="typeBuilder"></param>
        private static void BuildConstructor(Type baseType, TypeBuilder typeBuilder)
        {
            foreach (var ctor in baseType.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                var parameterTypes = ctor.GetParameters().Select(u => u.ParameterType).ToArray();
                var ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, parameterTypes);

                ILGenerator il = ctorBuilder.GetILGenerator();
                for (int i = 0; i <= parameterTypes.Length; ++i)
                {
                    LoadArgument(il, i);
                }
                il.Emit(OpCodes.Call, ctor);
                il.Emit(OpCodes.Ret);
            }
        }
        #endregion

        #region BuildMethod

        /// <summary>
        /// 构建方法
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="typeBuilder"></param>
        private static void BuildMethod(Type baseType, TypeBuilder typeBuilder)
        {
            foreach (var methodInfo in baseType.GetMethods())
            {
                if (!methodInfo.IsVirtual && !methodInfo.IsAbstract) continue;

                object[] attrs = methodInfo.GetCustomAttributes(typeof(AspectAttribute), true);
                int attrCount = attrs.Length;
                if (attrCount == 0) continue;

                ParameterInfo[] parameters = methodInfo.GetParameters();
                Type[] parameterTypes = parameters.Select(u => u.ParameterType).ToArray();

                MethodBuilder methodBuilder = typeBuilder.DefineMethod(
                    methodInfo.Name,
                    METHOD_ATTRIBUTES,
                    methodInfo.ReturnType,
                    parameterTypes);

                for (int i = 0; i < parameters.Length; i++)
                {
                    methodBuilder.DefineParameter(i + 1, parameters[i].Attributes, parameters[i].Name);
                }
                methodBuilder.SetParameters(parameterTypes);

                ILGenerator il = methodBuilder.GetILGenerator();

                // MethodContext context = new MethodContext();
                LocalBuilder localContext = il.DeclareLocal(typeof(MethodContext));
                #region init context
                il.Emit(OpCodes.Newobj, typeof(MethodContext).GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Stloc, localContext);
                
                // context.MethodName = m.Name;
                il.Emit(OpCodes.Ldloc, localContext);
                il.Emit(OpCodes.Ldstr, methodInfo.Name);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_MethodName"), new[] { typeof(string) });

                // context.ClassName = m.DeclaringType.Name;
                il.Emit(OpCodes.Ldloc, localContext);
                il.Emit(OpCodes.Ldstr, methodInfo.DeclaringType.Name);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_ClassName"), new[] { typeof(string) });
                
                // context.Executor = this;
                il.Emit(OpCodes.Ldloc, localContext);
                il.Emit(OpCodes.Ldarg_0);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_Executor"), new[] { typeof(object) });

                // context.ReturnType = m.ReturnType;
                il.Emit(OpCodes.Ldloc, localContext);
                il.Emit(OpCodes.Ldtoken, methodInfo.ReturnType);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_ReturnType"), new[] { typeof(Type) });
                
                #endregion

                // set context.Parameters
                #region context.Parameters = new object[Length];
                LocalBuilder tmpParameters = il.DeclareLocal(typeof(object[]));
                il.Emit(OpCodes.Ldc_I4, parameters.Length);
                il.Emit(OpCodes.Newarr, typeof(object));
                il.Emit(OpCodes.Stloc, tmpParameters);
                for (int i = 0; i < parameters.Length; ++i)
                {
                    il.Emit(OpCodes.Ldloc, tmpParameters);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldarg, i + 1);
                    il.Emit(OpCodes.Box, parameterTypes[i]);
                    il.EmitCall(OpCodes.Call, typeof(object).GetMethod("GetType", new Type[] { }), null);
                    il.Emit(OpCodes.Stelem_Ref);
                }
                il.Emit(OpCodes.Ldloc, localContext);
                il.Emit(OpCodes.Ldloc, tmpParameters);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_Parameters"), new[] { typeof(object[]) });
                #endregion

                LocalBuilder localReturnValue = null;
                if (methodInfo.ReturnType != typeof(void)) // has return value
                {
                    localReturnValue = il.DeclareLocal(methodInfo.ReturnType);
                }

                // ICallHandler[] handlers = new ICallHandler[attrCount];
                LocalBuilder localHandlers = il.DeclareLocal(typeof(ICallHandler[]));
                il.Emit(OpCodes.Ldc_I4, attrCount);
                il.Emit(OpCodes.Newarr, typeof(ICallHandler));
                il.Emit(OpCodes.Stloc, localHandlers);

                // create ICallHandler instance
                #region create ICallHandler instance
                for (int i = 0; i < attrCount; ++i)
                {
                    LocalBuilder tmpNameValueCollection = il.DeclareLocal(typeof(NameValueCollection));
                    il.Emit(OpCodes.Newobj, typeof(NameValueCollection).GetConstructor(Type.EmptyTypes));
                    il.Emit(OpCodes.Stloc, tmpNameValueCollection);

                    AspectAttribute attr = (attrs[i] as AspectAttribute);
                    NameValueCollection attrCollection = attr.GetAttrs();
                    foreach (var key in attrCollection.AllKeys)
                    {
                        il.Emit(OpCodes.Ldloc, tmpNameValueCollection);
                        il.Emit(OpCodes.Ldstr, key);
                        il.Emit(OpCodes.Ldstr, attrCollection[key]);
                        il.Emit(OpCodes.Callvirt, typeof(NameValueCollection).GetMethod("Add", new[] { typeof(string), typeof(string) }));
                    }

                    il.Emit(OpCodes.Ldloc, localHandlers);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldloc, tmpNameValueCollection);
                    il.Emit(OpCodes.Newobj, attr.CallHandlerType.GetConstructor(new[] { typeof(NameValueCollection) }));
                    //il.Emit(OpCodes.Newobj, attr.CallHandlerType.GetConstructor(Type.EmptyTypes));

                    il.Emit(OpCodes.Stelem_Ref);
                }
                #endregion

                // BeginInvoke
                for (int i = 0; i < attrCount; ++i)
                {
                    il.Emit(OpCodes.Ldloc, localHandlers);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Ldloc, localContext);
                    il.Emit(OpCodes.Callvirt, typeof(ICallHandler).GetMethod("BeginInvoke"));
                }

                Label endLabel = il.DefineLabel(); // if (context.Processed) goto: ...
                il.Emit(OpCodes.Ldloc, localContext);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("get_Processed"), Type.EmptyTypes);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Beq, endLabel);

                // excute base method
                LocalBuilder localException = il.DeclareLocal(typeof(Exception));
                il.BeginExceptionBlock(); // try {

                il.Emit(OpCodes.Ldloc, localContext);

                il.Emit(OpCodes.Ldarg_0);
                for (int i = 0; i < parameterTypes.Length; ++i)
                {
                    LoadArgument(il, i + 1);
                }
                il.EmitCall(OpCodes.Call, methodInfo, parameterTypes);
                // is has return value, save it
                if (methodInfo.ReturnType != typeof(void))
                {
                    if (methodInfo.ReturnType.IsValueType)
                    {
                        il.Emit(OpCodes.Box, methodInfo.ReturnType);
                    }
                    il.Emit(OpCodes.Stloc, localReturnValue);

                    il.Emit(OpCodes.Ldloc, localContext);
                    il.Emit(OpCodes.Ldloc, localReturnValue);
                    il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_ReturnValue"), new[] { typeof(object) });
                }

                il.BeginCatchBlock(typeof(Exception)); // } catch {
                // OnException
                il.Emit(OpCodes.Stloc, localException);
                il.Emit(OpCodes.Ldloc, localContext);
                il.Emit(OpCodes.Ldloc, localException);
                il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("set_Exception"), new[] { typeof(Exception) });

                for (int i = 0; i < attrCount; ++i)
                {
                    il.Emit(OpCodes.Ldloc, localHandlers);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Ldloc, localContext);
                    il.Emit(OpCodes.Callvirt, typeof(ICallHandler).GetMethod("OnException"));
                }

                il.EndExceptionBlock(); // }
                // end excute base method

                il.MarkLabel(endLabel);

                // EndInvoke
                for (int i = 0; i < attrCount; ++i)
                {
                    il.Emit(OpCodes.Ldloc, localHandlers);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Ldloc, localContext);
                    il.Emit(OpCodes.Callvirt, typeof(ICallHandler).GetMethod("EndInvoke"));
                }

                if (methodInfo.ReturnType != typeof(void))
                {
                    il.Emit(OpCodes.Ldloc, localContext);
                    il.EmitCall(OpCodes.Call, typeof(MethodContext).GetMethod("get_ReturnValue"), new[] { methodInfo.ReturnType.GetType() });
                    il.Emit(OpCodes.Stloc, localReturnValue);
                    il.Emit(OpCodes.Ldloc, localReturnValue);
                }
                //else
                //{
                //    il.Emit(OpCodes.Ldnull);
                //}

                il.Emit(OpCodes.Ret);
            }
        }
        #endregion

        #region LoadArgument
        /// <summary>
        /// LoadParameter
        /// </summary>
        /// <param name="il"></param>
        /// <param name="index"></param>
        public static void LoadArgument(ILGenerator il, int index)
        {
            switch (index)
            {
                case 0:
                    il.Emit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    il.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    il.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    il.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    if (index <= 127)
                    {
                        il.Emit(OpCodes.Ldarg_S, index);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarg, index);
                    }
                    break;
            }

        }
        #endregion

        public static void PushValue(ILGenerator il, object value)
        {
            // il  动态函数的 IL 生成器

            // 创建一个本地变量，主要用于 Object Type to Propety Type
            //var local = il.DeclareLocal(value.GetType(), true);
            // 加载第 2 个参数【(ILGenerator il, object value)】的 value
            il.Emit(OpCodes.Ldarg_2);
        }

        #endregion
    }
}
