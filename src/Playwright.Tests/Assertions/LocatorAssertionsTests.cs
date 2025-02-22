/*
 * MIT License
 *
 * Copyright (c) Microsoft Corporation.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Microsoft.Playwright.Tests.Assertions
{
    public class LocatorAssertionsTests : PageTestEx
    {
        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toBeChecked")]
        public async Task ShouldSupportToBeChecked()
        {
            await Page.SetContentAsync("<input type=checkbox checked></input>");
            await Expect(Page.Locator("input")).ToBeCheckedAsync();

            await Expect(Page.Locator("input")).ToBeCheckedAsync(new() { Checked = true });
            await Expect(Page.Locator("input")).Not.ToBeCheckedAsync(new() { Checked = false });

            var exception = await PlaywrightAssert.ThrowsAsync<PlaywrightException>(() => Expect(Page.Locator("input")).ToBeCheckedAsync(new() { Checked = false, Timeout = 300 }));
            StringAssert.Contains("Locator expected not to be checked", exception.Message);
            StringAssert.Contains("LocatorAssertions.ToBeCheckedAsync with timeout 300ms", exception.Message);

            exception = await PlaywrightAssert.ThrowsAsync<PlaywrightException>(() => Expect(Page.Locator("input")).Not.ToBeCheckedAsync(new() { Timeout = 300 }));
            StringAssert.Contains("Locator expected not to be checked", exception.Message);
            StringAssert.Contains("LocatorAssertions.ToBeCheckedAsync with timeout 300ms", exception.Message);
        }

        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toBeEditable, toBeEnabled, toBeDisabled, toBeEmpty")]
        public async Task ShouldSupportToBeEditableToBeEnabledToBeDisabledToBeEmpty()
        {
            {
                await Page.SetContentAsync("<input></input>");
                await Expect(Page.Locator("input")).ToBeEditableAsync();
            }

            {
                await Page.SetContentAsync("<button>Text</button>");
                await Expect(Page.Locator("button")).ToBeEnabledAsync();
            }

            {
                await Page.SetContentAsync("<button disabled>Text</button>");
                var exception = await PlaywrightAssert.ThrowsAsync<PlaywrightException>(() => Expect(Page.Locator("button")).ToBeEnabledAsync(new() { Timeout = 500 }));
                StringAssert.Contains("Locator expected to be enabled", exception.Message);
                StringAssert.Contains("LocatorAssertions.ToBeEnabledAsync with timeout 500ms", exception.Message);
            }

            {
                await Page.SetContentAsync("<button disabled>Text</button>");
                var locator = Page.Locator("button");
                await locator.EvaluateAsync("e => setTimeout(() => { e.removeAttribute('disabled') }, 500);");
                await Expect(locator).ToBeEnabledAsync();
            }

            {
                await Page.SetContentAsync("<button>Text</button>");
                var locator = Page.Locator("button");
                await locator.EvaluateAsync("e => setTimeout(() => { e.setAttribute('disabled', '') }, 500);");
                await Expect(locator).Not.ToBeEnabledAsync();
            }

            {
                await Page.SetContentAsync("<button disabled>Text</button>");
                var locator = Page.Locator("button");
                await Expect(locator).ToBeDisabledAsync();
            }

            {
                await Page.SetContentAsync("<input></input>");
                var locator = Page.Locator("input");
                await Expect(locator).ToBeEmptyAsync();
            }

            {
                await Page.SetContentAsync("<input value=text></input>");
                var locator = Page.Locator("input");
                await Expect(locator).Not.ToBeEmptyAsync();
            }
        }

        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toBeVisible, toBeHidden")]
        public async Task ShouldSupportToBeVisibleToBeHidden()
        {
            {
                await Page.SetContentAsync("<input></input>");
                var locator = Page.Locator("input");
                await Expect(locator).ToBeVisibleAsync();
            }
            {
                await Page.SetContentAsync("<button style=\"display: none\"></button>");
                var locator = Page.Locator("button");
                await Expect(locator).Not.ToBeVisibleAsync();
            }
            {
                await Page.SetContentAsync("<button style=\"display: none\"></button>");
                var locator = Page.Locator("button");
                await Expect(locator).ToBeHiddenAsync();
            }
            {
                await Page.SetContentAsync("<div></div>");
                var locator = Page.Locator("div");
                await Expect(locator).ToBeHiddenAsync();
            }
            {
                await Page.SetContentAsync("<input></input>");
                var locator = Page.Locator("input");
                await Expect(locator).Not.ToBeHiddenAsync();
            }
        }

        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toBeVisible, toBeHidden fail")]
        public async Task ShouldSupportToBeVisibleToBeHiddenFail()
        {
            {
                await Page.SetContentAsync("<button style=\"display: none\"></button>");
                var locator = Page.Locator("button");
                var exception = await PlaywrightAssert.ThrowsAsync<PlaywrightException>(() => Expect(locator).ToBeVisibleAsync(new() { Timeout = 500 }));
                StringAssert.Contains("Locator expected to be visible", exception.Message);
                StringAssert.Contains("LocatorAssertions.ToBeVisibleAsync with timeout 500ms", exception.Message);
            }
            {
                await Page.SetContentAsync("<input></input>");
                var locator = Page.Locator("input");
                var exception = await PlaywrightAssert.ThrowsAsync<PlaywrightException>(() => Expect(locator).Not.ToBeVisibleAsync(new() { Timeout = 500 }));
                StringAssert.Contains("Locator expected not to be visible", exception.Message);
                StringAssert.Contains("LocatorAssertions.ToBeVisibleAsync with timeout 500ms", exception.Message);
            }
        }

        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toBeFocused")]
        public async Task ShouldSupportToBeFocused()
        {
            await Page.SetContentAsync("<input></input>");
            var locator = Page.Locator("input");
            await locator.FocusAsync();
            await Expect(locator).ToBeFocusedAsync();
        }

        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toContainText")]
        public async Task ShouldSupportToContainText()
        {
            {
                await Page.SetContentAsync("<div id=node>Text   content</div>");
                await Expect(Page.Locator("#node")).ToContainTextAsync(new Regex("ex"));
                // Should not normalize whitespace.
                await Expect(Page.Locator("#node")).ToContainTextAsync(new Regex("ext   cont"));
            }
            {
                await Page.SetContentAsync("<div id=node>Text content</div>");
                var exeption = await PlaywrightAssert.ThrowsAsync<PlaywrightException>(() => Expect(Page.Locator("#node")).ToContainTextAsync(new Regex("ex2"), new() { Timeout = 100 }));
                StringAssert.Contains("Locator expected text matching regex 'ex2'", exeption.Message);
                StringAssert.Contains("But was: 'Text content'", exeption.Message);
                StringAssert.Contains("LocatorAssertions.ToContainTextAsync with timeout 100ms", exeption.Message);
            }
            {
                await Page.SetContentAsync("<div id=node><span></span>Text \ncontent&nbsp;    </div>");
                var locator = Page.Locator("#node");
                // Should normalize whitespace.
                await Expect(locator).ToHaveTextAsync("Text                        content");
                // Should normalize zero width whitespace.
                await Expect(locator).ToHaveTextAsync("T\u200be\u200bx\u200bt content");
                await Expect(locator).ToHaveTextAsync(new Regex("Text\\s+content"));
            }
            {
                await Page.SetContentAsync("<div id=node>Text content</div>");
                var locator = Page.Locator("#node");
                await Expect(locator).ToContainTextAsync("Text");
                // Should normalize whitespace.
                await Expect(locator).ToContainTextAsync("   ext        cont\n  ");
            }
            {
                await Page.SetContentAsync("<div id=node>Text content</div>");
                var locator = Page.Locator("#node");
                var exeption = await PlaywrightAssert.ThrowsAsync<PlaywrightException>(() => Expect(locator).ToHaveTextAsync("Text", new() { Timeout = 100 }));
                StringAssert.Contains("Locator expected to have text 'Text'", exeption.Message);
                StringAssert.Contains("But was: 'Text content'", exeption.Message);
                StringAssert.Contains("LocatorAssertions.ToHaveTextAsync with timeout 100ms", exeption.Message);
            }
        }

        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toContainText w/ array")]
        public async Task ShouldSupportToContainTextWithArray()
        {
            await Page.SetContentAsync("<div>Text \n1</div><div>Text2</div><div>Text3</div>");
            var locator = Page.Locator("div");
            await Expect(locator).ToContainTextAsync(new string[] { "ext     1", "ext3" });
            await Expect(locator).ToContainTextAsync(new Regex[] { new Regex("ext \\s+1"), new Regex("ext3") });
        }

        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toHaveText w/ array")]
        public async Task ShouldSupportToHaveTextWithArray()
        {
            await Page.SetContentAsync("<div>Text    \n1</div><div>Text   2a</div>");
            var locator = Page.Locator("div");
            // Should normalize whitespace.
            await Expect(locator).ToHaveTextAsync(new string[] { "Text  1", "Text 2a" });
            // But not for Regex.
            await Expect(locator).ToHaveTextAsync(new Regex[] { new Regex("Text \\s+1"), new Regex("Text   \\d+a") });
        }

        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toHaveAttribute")]
        public async Task ShouldSupportToHaveAttribute()
        {
            await Page.SetContentAsync("<div id=node>Text content</div>");
            var locator = Page.Locator("#node");
            await Expect(locator).ToHaveAttributeAsync("id", "node");
            await Expect(locator).ToHaveAttributeAsync("id", new Regex("node"));
        }

        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toHaveCSS")]
        public async Task ShouldSupportToHaveCSS()
        {
            await Page.SetContentAsync("<div id=node style=\"color: rgb(255, 0, 0)\">Text content</div>");
            var locator = Page.Locator("#node");
            await Expect(locator).ToHaveCSSAsync("color", "rgb(255, 0, 0)");
            await Expect(locator).ToHaveCSSAsync("color", new Regex("rgb\\(\\d+, 0, 0\\)"));
        }

        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toHaveClass")]
        public async Task ShouldSupportToHaveClass()
        {
            {
                await Page.SetContentAsync("<div class=\"foo bar baz\"></div>");
                var locator = Page.Locator("div");
                await Expect(locator).ToHaveClassAsync("foo bar baz");
                await Expect(locator).ToHaveClassAsync(new Regex("foo bar baz"));
                var exception = await PlaywrightAssert.ThrowsAsync<PlaywrightException>(() => Expect(locator).ToHaveClassAsync("kektus", new() { Timeout = 300 }));
                StringAssert.Contains("Locator expected to have class 'kektus'", exception.Message);
                StringAssert.Contains("But was: 'foo bar baz'", exception.Message);
                StringAssert.Contains("LocatorAssertions.ToHaveClassAsync with timeout 300ms", exception.Message);
            }
            {
                await Page.SetContentAsync("<div class=\"foo\"></div><div class=\"bar\"></div><div class=\"baz\"></div>");
                var locator = Page.Locator("div");
                await Expect(locator).ToHaveClassAsync(new string[] { "foo", "bar", "baz" });
                await Expect(locator).ToHaveClassAsync(new Regex[] { new("^f.o$"), new("^b.r$"), new("^[a-z]az$") });
            }
        }

        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toHaveCount")]
        public async Task ShouldSupportToHaveCount()
        {
            await Page.SetContentAsync("<select><option>One</option></select>");
            var locator = Page.Locator("option");
            await PlaywrightAssert.ThrowsAsync<PlaywrightException>(() => Expect(locator).ToHaveCountAsync(2, new() { Timeout = 300 }));
            await Page.SetContentAsync("<select><option>One</option><option>Two</option></select>");
            await Expect(locator).ToHaveCountAsync(2);
        }

        [PlaywrightTest("playwright-test/playwright.expect.spec.ts", "should support toHaveId")]
        public async Task ShouldSupportToHaveId()
        {
            await Page.SetContentAsync("<div id=node>Text content</div>");
            var locator = Page.Locator("#node");
            await Expect(locator).ToHaveIdAsync("node");
            await Expect(locator).ToHaveIdAsync(new Regex("n.de"));
        }

        [PlaywrightTest("playwright-test/playwright.expect.misc.spec.ts", "should support toHaveJSProperty")]
        public async Task ShouldSupportToHaveJSProperty()
        {
            await Page.SetContentAsync("<div></div>");
            await Page.EvalOnSelectorAsync("div", "e => e.foo = { a: 1, b: 'string', c: new Date(1627503992000) }");
            var locator = Page.Locator("div");
            await Expect(locator).ToHaveJSPropertyAsync("foo", new Dictionary<string, object>
            {
                ["a"] = 1,
                ["b"] = "string",
                ["c"] = DateTime.Parse("2021-07-28T20:26:32.000Z"),
            });
        }

        [PlaywrightTest("playwright-test/playwright.expect.misc.spec.ts", "should support toHaveValue")]
        public async Task ShouldSupportToHaveValue()
        {
            {
                await Page.SetContentAsync("<input id=node></input>");
                var locator = Page.Locator("#node");
                await locator.FillAsync("Text content");
                await Expect(locator).ToHaveValueAsync("Text content");
                await Expect(locator).ToHaveValueAsync(new Regex("Text( |)content"));
            }
            {
                await Page.SetContentAsync("<label><input></input></label>");
                await Page.Locator("label input").FillAsync("Text content");
                await Expect(Page.Locator("label")).ToHaveValueAsync("Text content");
            }
        }
    }
}
