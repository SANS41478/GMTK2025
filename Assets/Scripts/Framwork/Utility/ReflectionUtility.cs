//Create By 腾讯元宝 

namespace Space.Utility
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



public static class ReflectionUtility
{
    // 核心原理：泛型约束保证类型安全，延迟加载减少不必要的Assembly扫描
    public static IEnumerable<TAttribute> GetAttributesFromAssembly<TAttribute>(Assembly assembly)
        where TAttribute : Attribute
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));
        
        // 单次Assembly扫描复用设计（复杂度转移点）
        return assembly.GetTypes()
            .SelectMany(type => type.GetCustomAttributes<TAttribute>(false))
            .Distinct();
    }

    // 原理：分离加载策略与核心逻辑，支持未来扩展不同加载机制
    public static IEnumerable<TAttribute> GetAttributesFromAssemblies<TAttribute>(IEnumerable<string> assemblyNames)
        where TAttribute : Attribute
    {
        var loadedAssemblies = AssemblyLoader.LoadAssemblies(assemblyNames);
        return loadedAssemblies
            .SelectMany(assembly => GetAttributesFromAssembly<TAttribute>(assembly));
    }

    // 原理：提供快捷入口封装固定模式，保持API简洁
    public static IEnumerable<TAttribute> GetAttributesFromExecutingAssembly<TAttribute>()
        where TAttribute : Attribute
    {
        return GetAttributesFromAssembly<TAttribute>(Assembly.GetExecutingAssembly());
    }
    
    /// <summary>
    /// 创建指定类型的实例
    /// </summary>
    /// <param name="type">要实例化的类型</param>
    /// <param name="args">构造函数参数</param>
    /// <returns>创建的对象实例</returns>
    public static object CreateInstance(Type type, params object[] args)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        // 处理无参数情况
        if (args == null || args.Length == 0)
        {
            return Activator.CreateInstance(type);
        }
        // 尝试直接激活
        try
        {
            return Activator.CreateInstance(type, args);
        }
        catch (MissingMethodException)
        {
            throw new NotSupportedException();
        }
    }
}

// 分离加载器实现（核心与策略解耦）
internal static class AssemblyLoader
{
    // 原理：封装可变部分，为未来平台特殊加载逻辑保留扩展点
    public static IEnumerable<Assembly> LoadAssemblies(IEnumerable<string> assemblyNames)
    {
        foreach (var name in assemblyNames)
        {
            Assembly assembly = null;
            try
            {
                // 隔离平台依赖代码：此处可替换为Assembly.LoadFrom等跨平台方案
                assembly = Assembly.Load(name);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load assembly '{name}'", ex);
            }
            
            if (assembly != null) yield return assembly;
        }
    }
}
}