using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Initialize ChromeDriver
        IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://soundcloud.com/spaghetti-god/likes"); // Replace with your target URL

        // Initialize variables to control the search and scrolling
        bool moreElements = true;
        int i = 1; // Start with the first `li[1]`
        int previousElementCount = 0;

        try
        {
            while (moreElements)
            {
                try
                {
                    // Construct the XPath dynamically for the current `li[i]`
                    string xpath = $"//*[@id='content']/div/div/div[2]/div/div[2]/ul/li[{i}]/div/div/div/div[2]/div[1]/div/div/div[2]/a/span";

                    // Find the element using the dynamically generated XPath
                    IWebElement element = driver.FindElement(By.XPath(xpath));

                    // Extract and print the text content of the element
                    string text = element.Text;
                    Console.WriteLine($"Element {i}: {text}");

                    // Increment the index to move to the next element
                    i++;
                }
                catch (NoSuchElementException)
                {
                    // If the element is not found, scroll down to load more content
                    Console.WriteLine($"Element {i} not found. Scrolling down...");
                    ScrollDown(driver);

                    // Wait for new content to load
                    Thread.Sleep(2000);

                    // Check if new elements have been loaded
                    var elements = driver.FindElements(By.XPath("//*[@id='content']/div/div/div[2]/div/div[2]/ul/li"));
                    if (elements.Count == previousElementCount)
                    {
                        // If no new elements are loaded, stop the loop
                        Console.WriteLine("No more elements to load.");
                        moreElements = false;
                    }
                    else
                    {
                        // Update the element count and continue searching
                        previousElementCount = elements.Count;
                    }
                }
            }
        }
        finally
        {
            // Close the browser after completion
            driver.Quit();
        }
    }

    // Scroll to the bottom of the page to load more elements
    static void ScrollDown(IWebDriver driver)
    {
        var jsExecutor = (IJavaScriptExecutor)driver;
        jsExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
    }
}
