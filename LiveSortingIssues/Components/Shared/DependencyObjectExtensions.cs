using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LiveSortingIssues.Components.Shared;

public static class DependencyObjectExtensions
{
    public static T? FindVisualParent<T>(this DependencyObject child) where T : DependencyObject
    {
        while (true)
        {
            DependencyObject? candidate = child;

            while (candidate is not (Visual or Visual3D))
            {
                if (candidate is T quicklyFoundParent)
                {
                    return quicklyFoundParent;
                }

                candidate = (candidate as FrameworkContentElement)?.Parent;
            }

            candidate = VisualTreeHelper.GetParent(candidate);

            switch (candidate)
            {
                case T foundParent: return foundParent;
                case null: return null;
                default: child = candidate;
                    break;
            }
        }
    }

    public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject? parent) where T : DependencyObject
    {
        if (parent is null)
        {
            yield break;
        }

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);

            if (child is T target)
            {
                yield return target;
            }

            foreach (var grandChild in FindVisualChildren<T>(child))
            {
                yield return grandChild;
            }
        }
    }
}
