using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaywrightPrac
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class PlaywrightTests : PageTest
    {
        //// Override the ContextOptions method to configure browser context
        //protected override async Task<IBrowserContext> NewBrowserContextAsync()
        //{
        //    var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        //    {
        //        Headless = false // Disable headless mode
        //    });

        //    return await browser.NewContextAsync();
        //}

        [Test]
        public async Task GetStartedLink()
        {
            // Navigate to the page
            await Page.GotoAsync("https://automationexercise.com");

            // Verify the page title
            await Expect(Page).ToHaveTitleAsync(new Regex("Automation Exercise"));
        }

        [Test]
        public async Task SignUpNewUser()
        {
            // Navigate to the page
            await Page.GotoAsync("https://automationexercise.com");

            // Verify the page title
            await Expect(Page).ToHaveTitleAsync(new Regex("Automation Exercise"));

            // Click on the "Signup/Login" link
            await Page.Locator("//*[@id=\"header\"]/div/div/div/div[2]/div/ul/li[4]/a").ClickAsync();

            // Wait for the new page to load
            await Page.WaitForLoadStateAsync(LoadState.Load);

            // Enter name in the signup form
            await Page.Locator("#form > div > div > div:nth-child(3) > div > form > input[type=text]:nth-child(2)").FillAsync("randy");

            // Click the "Signup" button
            await Page.Locator("#form > div > div > div:nth-child(3) > div > form > button").ClickAsync();

            // Wait for the new page to load
            await Page.WaitForLoadStateAsync(LoadState.Load);

            // Assert that the new page contains the text "ENTER ACCOUNT INFORMATION"
            await Expect(Page.Locator("#form > div > div > div > div.login-form > h2 > b")).ToHaveTextAsync("ENTER ACCOUNT INFORMATION");
        }
    }
}
