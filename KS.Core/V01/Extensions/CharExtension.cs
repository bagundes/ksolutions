namespace KS.Core.V01.Extensions;

public static class CharExtension
{
    public static bool Contains(this char value, params char[] values)
    {
        foreach (var par in values)
        {
            if (value.Equals(par)) return true;
        }

        return false;
    }
}