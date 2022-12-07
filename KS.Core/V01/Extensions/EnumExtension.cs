using System.Reflection;

namespace KS.Core.V01.Extensions;

public static class EnumExtension
{
   
    /// <summary>
    /// Get attribute from Enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enum"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T? GetAttribute<T>(this Enum @enum) where T : Attribute
    {
        MemberInfo info = @enum.GetType().GetMember(@enum.ToString()).First();

        if (info != null && info.CustomAttributes.Any())
        {
            return info.GetCustomAttribute<T>();

        }
        else
        {
            return null;
        }
    }
}