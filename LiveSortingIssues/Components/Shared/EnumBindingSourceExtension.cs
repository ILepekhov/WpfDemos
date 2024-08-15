using System.Windows.Markup;

namespace LiveSortingIssues.Components.Shared;

/*
 * Source: https://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf
 * Example: ItemsSource="{Binding Source={namespaceAlias:EnumBindingSource {x:Type enumNamespaceAlias:VariableValueType}}}"
 */

/// <summary>
/// Provides a way to bind Enum items without ObjectDataProvider (and without memory leaks)
/// </summary>
public sealed class EnumBindingSourceExtension : MarkupExtension
{
    private readonly Type _enumType;

    public EnumBindingSourceExtension(Type enumType)
    {
        if (enumType is null) throw new ArgumentNullException(nameof(enumType));

        CheckEnumType(enumType);

        _enumType = enumType;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (_enumType is null)
            throw new InvalidOperationException("The EnumType must be specified.");

        Type actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
        Array enumValues = Enum.GetValues(actualEnumType);

        if (actualEnumType == _enumType)
        {
            return enumValues;
        }

        // it seems that first array element would be indicate Null value
        var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
        enumValues.CopyTo(tempArray, 1);
        return tempArray;
    }

    private static void CheckEnumType(Type enumType)
    {
        Type actualEnumType = Nullable.GetUnderlyingType(enumType) ?? enumType;

        if (actualEnumType.IsEnum is false)
        {
            throw new ArgumentException("Type must be for an Enum.");
        }
    }
}
