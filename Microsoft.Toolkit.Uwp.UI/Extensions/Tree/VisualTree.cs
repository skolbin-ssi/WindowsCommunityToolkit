// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Defines a collection of extensions methods for UI.
    /// </summary>
    public static class VisualTree
    {
        /// <summary>
        /// Find the first descendant of type <see cref="FrameworkElement"/> with a given name, using a depth-first search.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindDescendant(this DependencyObject element, string name, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return FindDescendant<FrameworkElement, (string Name, StringComparison ComparisonType)>(
                element,
                (name, comparisonType),
                static (e, s) => s.Name.Equals(e.Name, s.ComparisonType));
        }

        /// <summary>
        /// Find the first descendant element of a given type, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendant<T>(this DependencyObject element)
            where T : notnull, DependencyObject
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (var i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(element, i);

                if (child is T result)
                {
                    return result;
                }

                T? descendant = FindDescendant<T>(child);

                if (descendant is not null)
                {
                    return descendant;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the first descendant element of a given type, using a depth-first search.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static DependencyObject? FindDescendant(this DependencyObject element, Type type)
        {
            return FindDescendant<DependencyObject, Type>(element, type, static (e, t) => e.GetType() == t);
        }

        /// <summary>
        /// Find the first descendant element matching a given predicate, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="predicate">The predicatee to use to match the descendant nodes.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendant<T>(this DependencyObject element, Func<T, bool> predicate)
            where T : notnull, DependencyObject
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (var i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(element, i);

                if (child is T result && predicate(result))
                {
                    return result;
                }

                T? descendant = FindDescendant(child, predicate);

                if (descendant is not null)
                {
                    return descendant;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the first descendant element matching a given predicate, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the descendant nodes.</param>
        /// <returns>The descendant that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendant<T, TState>(this DependencyObject element, TState state, Func<T, TState, bool> predicate)
            where T : notnull, DependencyObject
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (var i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(element, i);

                if (child is T result && predicate(result, state))
                {
                    return result;
                }

                T? descendant = FindDescendant(child, state, predicate);

                if (descendant is not null)
                {
                    return descendant;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the first descendant (or self) of type <see cref="FrameworkElement"/> with a given name, using a depth-first search.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindDescendantOrSelf(this DependencyObject element, string name, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (element is FrameworkElement result && name.Equals(result.Name, comparisonType))
            {
                return result;
            }

            return FindDescendant(element, name, comparisonType);
        }

        /// <summary>
        /// Find the first descendant (or self) element of a given type, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendantOrSelf<T>(this DependencyObject element)
            where T : notnull, DependencyObject
        {
            if (element is T result)
            {
                return result;
            }

            return FindDescendant<T>(element);
        }

        /// <summary>
        /// Find the first descendant (or self) element of a given type, using a depth-first search.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static DependencyObject? FindDescendantOrSelf(this DependencyObject element, Type type)
        {
            if (element.GetType() == type)
            {
                return element;
            }

            return FindDescendant(element, type);
        }

        /// <summary>
        /// Find the first descendant (or self) element matching a given predicate, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="predicate">The predicatee to use to match the descendant nodes.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendantOrSelf<T>(this DependencyObject element, Func<T, bool> predicate)
            where T : notnull, DependencyObject
        {
            if (element is T result && predicate(result))
            {
                return result;
            }

            return FindDescendant(element, predicate);
        }

        /// <summary>
        /// Find the first descendant (or self) element matching a given predicate, using a depth-first search.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The root element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the descendant nodes.</param>
        /// <returns>The descendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindDescendantOrSelf<T, TState>(this DependencyObject element, TState state, Func<T, TState, bool> predicate)
            where T : notnull, DependencyObject
        {
            if (element is T result && predicate(result, state))
            {
                return result;
            }

            return FindDescendant(element, state, predicate);
        }

        /// <summary>
        /// Find all descendant controls of the specified type.
        /// </summary>
        /// <typeparam name="T">Type to search for.</typeparam>
        /// <param name="element">Parent element.</param>
        /// <returns>Descendant controls or empty if not found.</returns>
        public static IEnumerable<T> FindDescendants<T>(this DependencyObject element)
            where T : DependencyObject
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                var type = child as T;
                if (type != null)
                {
                    yield return type;
                }

                foreach (T childofChild in child.FindDescendants<T>())
                {
                    yield return childofChild;
                }
            }
        }

        /// <summary>
        /// Find the first ascendant of type <see cref="FrameworkElement"/> with a given name.
        /// </summary>
        /// <param name="element">The starting element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <returns>The ascendant that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindAscendant(this DependencyObject element, string name, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return FindAscendant<FrameworkElement, (string Name, StringComparison ComparisonType)>(
                element,
                (name, comparisonType),
                static (e, s) => s.Name.Equals(e.Name, s.ComparisonType));
        }

        /// <summary>
        /// Find the first ascendant element of a given type.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <returns>The ascendant that was found, or <see langword="null"/>.</returns>
        public static T? FindAscendant<T>(this DependencyObject element)
            where T : notnull, DependencyObject
        {
            while (true)
            {
                DependencyObject? parent = VisualTreeHelper.GetParent(element);

                if (parent is null)
                {
                    return null;
                }

                if (parent is T result)
                {
                    return result;
                }

                element = parent;
            }
        }

        /// <summary>
        /// Find the first ascendant element of a given type.
        /// </summary>
        /// <param name="element">The starting element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <returns>The ascendant that was found, or <see langword="null"/>.</returns>
        public static DependencyObject? FindAscendant(this DependencyObject element, Type type)
        {
            return FindAscendant<DependencyObject, Type>(element, type, static (e, t) => e.GetType() == t);
        }

        /// <summary>
        /// Find the first ascendant element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="predicate">The predicatee to use to match the ascendant nodes.</param>
        /// <returns>The ascendant that was found, or <see langword="null"/>.</returns>
        public static T? FindAscendant<T>(this DependencyObject element, Func<T, bool> predicate)
            where T : notnull, DependencyObject
        {
            while (true)
            {
                DependencyObject? parent = VisualTreeHelper.GetParent(element);

                if (parent is null)
                {
                    return null;
                }

                if (parent is T result && predicate(result))
                {
                    return result;
                }

                element = parent;
            }
        }

        /// <summary>
        /// Find the first ascendant element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the ascendant nodes.</param>
        /// <returns>The ascendant that was found, or <see langword="null"/>.</returns>
        public static T? FindAscendant<T, TState>(this DependencyObject element, TState state, Func<T, TState, bool> predicate)
            where T : notnull, DependencyObject
        {
            while (true)
            {
                DependencyObject? parent = VisualTreeHelper.GetParent(element);

                if (parent is null)
                {
                    return null;
                }

                if (parent is T result && predicate(result, state))
                {
                    return result;
                }

                element = parent;
            }
        }

        /// <summary>
        /// Find the first ascendant (or self) of type <see cref="FrameworkElement"/> with a given name.
        /// </summary>
        /// <param name="element">The starting element.</param>
        /// <param name="name">The name of the element to look for.</param>
        /// <param name="comparisonType">The comparison type to use to match <paramref name="name"/>.</param>
        /// <returns>The ascendant (or self) that was found, or <see langword="null"/>.</returns>
        public static FrameworkElement? FindAscendantOrSelf(this DependencyObject element, string name, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (element is FrameworkElement result && name.Equals(result.Name, comparisonType))
            {
                return result;
            }

            return FindAscendant(element, name, comparisonType);
        }

        /// <summary>
        /// Find the first ascendant (or self) element of a given type.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <returns>The ascendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindAscendantOrSelf<T>(this DependencyObject element)
            where T : notnull, DependencyObject
        {
            if (element is T result)
            {
                return result;
            }

            return FindAscendant<T>(element);
        }

        /// <summary>
        /// Find the first ascendant (or self) element of a given type.
        /// </summary>
        /// <param name="element">The starting element.</param>
        /// <param name="type">The type of element to match.</param>
        /// <returns>The ascendant (or self) that was found, or <see langword="null"/>.</returns>
        public static DependencyObject? FindAscendantOrSelf(this DependencyObject element, Type type)
        {
            if (element.GetType() == type)
            {
                return element;
            }

            return FindAscendant(element, type);
        }

        /// <summary>
        /// Find the first ascendant (or self) element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="predicate">The predicatee to use to match the ascendant nodes.</param>
        /// <returns>The ascendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindAscendantOrSelf<T>(this DependencyObject element, Func<T, bool> predicate)
            where T : notnull, DependencyObject
        {
            if (element is T result && predicate(result))
            {
                return result;
            }

            return FindAscendant(element, predicate);
        }

        /// <summary>
        /// Find the first ascendant (or self) element matching a given predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements to match.</typeparam>
        /// <typeparam name="TState">The type of state to use when matching nodes.</typeparam>
        /// <param name="element">The starting element.</param>
        /// <param name="state">The state to give as input to <paramref name="predicate"/>.</param>
        /// <param name="predicate">The predicatee to use to match the ascendant nodes.</param>
        /// <returns>The ascendant (or self) that was found, or <see langword="null"/>.</returns>
        public static T? FindAscendantOrSelf<T, TState>(this DependencyObject element, TState state, Func<T, TState, bool> predicate)
            where T : notnull, DependencyObject
        {
            if (element is T result && predicate(result, state))
            {
                return result;
            }

            return FindAscendant(element, state, predicate);
        }

        /// <summary>
        /// Find all visual ascendants for the element.
        /// </summary>
        /// <param name="element">Child element.</param>
        /// <returns>A collection of parent elements or null if none found.</returns>
        public static IEnumerable<DependencyObject> FindAscendants(this DependencyObject element)
        {
            var parent = VisualTreeHelper.GetParent(element);

            while (parent != null)
            {
                yield return parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }
    }
}
