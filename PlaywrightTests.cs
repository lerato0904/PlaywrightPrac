using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;
using Bogus;

namespace PlaywrightPrac
{
    [TestFixture]
    public class PlaywrightTests : PageTest
    {
        Faker faker;

        public PlaywrightTests()
        {
            faker = new Faker();
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("firefox")]
        public async Task NavigateToLandingPage(string browserType)
        {
            // Launch the specified browser
            // Set to true for headless mode
            //When set to True: Launches the browser in headless mode, meaning the browser runs in the background without a UI.
            //This is common for CI/CD pipelines and automation environments
            IBrowser browser = browserType switch
            {
                "chromium" => await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }),
                "firefox" => await Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }),
                "webkit" => await Playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }),
                _ => throw new ArgumentException($"Unsupported browser: {browserType}")
            };



            // Create an isolated session within the browser - Useful for running parallel tests or scenarios without interference.
            var context = await browser.NewContextAsync();

            // Creates a new tab (or page) within the browser context.
            // This page can be used to interact with the web application, including navigation, input simulation, and assertions.
            var page = await context.NewPageAsync();

            // Navigate to the page
            await page.GotoAsync("https://automationexercise.com");

            // Verify the page title
            await Expect(page).ToHaveTitleAsync(new Regex("Automation Exercise"));

            //Close broser
            await browser.CloseAsync();
        }

        [Test]
        public async Task SignUpNewUser()
        {
            string name = faker.Name.FirstName();
            string email = faker.Name.FirstName() + "@test.com";

            // Launch Chrome browser
            var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false // Set to true for headless mode
            });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            // Navigate to the page
            await page.GotoAsync("https://automationexercise.com");

            // Verify the page title
            await Expect(page).ToHaveTitleAsync(new Regex("Automation Exercise"));

            // Click on the "Signup/Login" link
            await page.Locator("//*[@id=\"header\"]/div/div/div/div[2]/div/ul/li[4]/a").ClickAsync();

            // Wait for the new page to load
            await page.WaitForLoadStateAsync(LoadState.Load);

            // Enter name in the signup form
            await page.Locator("//*[@id=\"form\"]/div/div/div[3]/div/form/input[2]").FillAsync(name);


            // Enter email in the signup form
            await page.Locator("//*[@id=\"form\"]/div/div/div[3]/div/form/input[3]").FillAsync(email);


            // Click the "Signup" button
            await page.Locator("//*[@id=\"form\"]/div/div/div[3]/div/form/button").ClickAsync();

            // Wait for the new page to load
            await page.WaitForLoadStateAsync(LoadState.Load);

            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded); // Or DOMContentLoaded

            // Assert that the new page contains the text "ENTER ACCOUNT INFORMATION"
            await Expect(page.Locator("//*[@id=\"form\"]/div/div/div/div[1]/h2/b")).ToHaveTextAsync("Enter Account Information");

        }
    }
}
