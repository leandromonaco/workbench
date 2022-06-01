using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

class Program
{
    public static async Task Main()
    {
        //Code Generation --> pwsh bin\Debug\net6.0\playwright.ps1 codegen wikipedia.org

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        // Open new page
        var page = await context.NewPageAsync();

        // Go to https://www.wikipedia.org/
        await page.GotoAsync("https://www.wikipedia.org/");

        // Click input[name="search"]
        await page.Locator("input[name=\"search\"]").ClickAsync();

        // Fill input[name="search"]
        await page.Locator("input[name=\"search\"]").FillAsync("test");

        // Press Enter
        await page.Locator("input[name=\"search\"]").PressAsync("Enter");
        // Assert.AreEqual("https://en.wikipedia.org/wiki/Test", page.Url);

        Console.ReadLine();
        
        await page.CloseAsync();

        Console.ReadLine();
    }
}
