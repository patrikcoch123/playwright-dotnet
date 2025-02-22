/*
 * MIT License
 *
 * Copyright (c) Microsoft Corporation.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Microsoft.Playwright
{
    public class FrameLocatorLocatorOptions
    {
        public FrameLocatorLocatorOptions() { }

        public FrameLocatorLocatorOptions(FrameLocatorLocatorOptions clone)
        {
            if (clone == null)
            {
                return;
            }

            Above = clone.Above;
            Below = clone.Below;
            Has = clone.Has;
            HasTextString = clone.HasTextString;
            HasTextRegex = clone.HasTextRegex;
            LeftOf = clone.LeftOf;
            Near = clone.Near;
            RightOf = clone.RightOf;
        }

        /// <summary>
        /// <para>
        /// Matches elements that are above any of the elements matching the inner locator,
        /// at any horizontal position. Inner locator is queried against the same root as the
        /// outer one. More details in <a href="https://playwright.dev/dotnet/docs/selectors#selecting-elements-based-on-layout">layout
        /// selectors</a> guide.
        /// </para>
        /// <para>
        /// Note that outer and inner locators must belong to the same frame. Inner locator
        /// must not contain <see cref="IFrameLocator"/>s.
        /// </para>
        /// </summary>
        [JsonPropertyName("above")]
        public ILocator? Above { get; set; }

        /// <summary>
        /// <para>
        /// Matches elements that are below any of the elements matching the inner locator,
        /// at any horizontal position. Inner locator is queried against the same root as the
        /// outer one. More details in <a href="https://playwright.dev/dotnet/docs/selectors#selecting-elements-based-on-layout">layout
        /// selectors</a> guide.
        /// </para>
        /// <para>
        /// Note that outer and inner locators must belong to the same frame. Inner locator
        /// must not contain <see cref="IFrameLocator"/>s.
        /// </para>
        /// </summary>
        [JsonPropertyName("below")]
        public ILocator? Below { get; set; }

        /// <summary>
        /// <para>
        /// Matches elements containing an element that matches an inner locator. Inner locator
        /// is queried against the outer one. For example, <c>article</c> that has <c>text=Playwright</c>
        /// matches <c>&lt;article&gt;&lt;div&gt;Playwright&lt;/div&gt;&lt;/article&gt;</c>.
        /// </para>
        /// <para>
        /// Note that outer and inner locators must belong to the same frame. Inner locator
        /// must not contain <see cref="IFrameLocator"/>s.
        /// </para>
        /// </summary>
        [JsonPropertyName("has")]
        public ILocator? Has { get; set; }

        /// <summary>
        /// <para>
        /// Matches elements containing specified text somewhere inside, possibly in a child
        /// or a descendant element. When passed a <see cref="string"/>, matching is case-insensitive
        /// and searches for a substring. For example, <c>"Playwright"</c> matches <c>&lt;article&gt;&lt;div&gt;Playwright&lt;/div&gt;&lt;/article&gt;</c>.
        /// </para>
        /// </summary>
        [JsonPropertyName("hasTextString")]
        public string? HasTextString { get; set; }

        /// <summary>
        /// <para>
        /// Matches elements containing specified text somewhere inside, possibly in a child
        /// or a descendant element. When passed a <see cref="string"/>, matching is case-insensitive
        /// and searches for a substring. For example, <c>"Playwright"</c> matches <c>&lt;article&gt;&lt;div&gt;Playwright&lt;/div&gt;&lt;/article&gt;</c>.
        /// </para>
        /// </summary>
        [JsonPropertyName("hasTextRegex")]
        public Regex? HasTextRegex { get; set; }

        /// <summary>
        /// <para>
        /// Matches elements that are to the left of any element matching the inner locator,
        /// at any vertical position. Inner locator is queried against the same root as the
        /// outer one. More details in <a href="https://playwright.dev/dotnet/docs/selectors#selecting-elements-based-on-layout">layout
        /// selectors</a> guide.
        /// </para>
        /// <para>
        /// Note that outer and inner locators must belong to the same frame. Inner locator
        /// must not contain <see cref="IFrameLocator"/>s.
        /// </para>
        /// </summary>
        [JsonPropertyName("leftOf")]
        public ILocator? LeftOf { get; set; }

        /// <summary>
        /// <para>
        /// Matches elements that are near (&lt;= 50 css pixels) any of the elements matching
        /// the inner locator. Inner locator is queried against the same root as the outer one.
        /// More details in <a href="https://playwright.dev/dotnet/docs/selectors#selecting-elements-based-on-layout">layout
        /// selectors</a> guide.
        /// </para>
        /// <para>
        /// Note that outer and inner locators must belong to the same frame. Inner locator
        /// must not contain <see cref="IFrameLocator"/>s.
        /// </para>
        /// </summary>
        [JsonPropertyName("near")]
        public ILocator? Near { get; set; }

        /// <summary>
        /// <para>
        /// Matches elements that are to the right of any element matching the inner locator,
        /// at any vertical position. Inner locator is queried against the same root as the
        /// outer one. More details in <a href="https://playwright.dev/dotnet/docs/selectors#selecting-elements-based-on-layout">layout
        /// selectors</a> guide.
        /// </para>
        /// <para>
        /// Note that outer and inner locators must belong to the same frame. Inner locator
        /// must not contain <see cref="IFrameLocator"/>s.
        /// </para>
        /// </summary>
        [JsonPropertyName("rightOf")]
        public ILocator? RightOf { get; set; }
    }
}

#nullable disable
