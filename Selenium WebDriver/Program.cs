using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Selenium_WebDriver
{
	class Program
	{
		static void SimpleTest_Google()
		{
			IWebDriver driver = new ChromeDriver();

			driver.Navigate().GoToUrl("http://google.com");

			IWebElement element = driver.FindElement(By.Name("q"));
			element.SendKeys("Selenium");
			element.Submit();

			Console.WriteLine("Page title is: {0}", driver.Title);

			WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

			Func<IWebDriver, bool> waitCondition = new Func<IWebDriver, bool>((IWebDriver web) =>
			{
				return web.Title.ToLower().StartsWith("selenium");
			});

			waiter.Until(waitCondition);

			Console.WriteLine("Page title is: {0}", driver.Title);
			Console.ReadKey();

			driver.Quit();
		}

		static void TestPhptravelLogin()
		{
			IWebDriver driver = new ChromeDriver();

			driver.Navigate().GoToUrl("http://www.phptravels.net/login");

			WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

			Func<IWebDriver, bool> waitCondition = new Func<IWebDriver, bool>((IWebDriver web) =>
			{
				driver.FindElement(By.Name("username"));
				driver.FindElement(By.Name("password"));
				//driver.FindElements(By.TagName("button")).Where(x => x.Text == "Login");

				return true;
			});

			waiter.Until(waitCondition);

			driver.FindElement(By.Name("username")).SendKeys("user@phptravels.com");
			driver.FindElement(By.Name("password")).SendKeys("demouser");
			foreach (var webElement in driver.FindElements(By.TagName("button")))
				if (webElement.Text == "Login") webElement.Click();
		}

		static void TestYouTube()
		{
			var driver = new ChromeDriver();

			driver.Navigate().GoToUrl("http://youtube.com");

			driver.FindElement(By.Name("search_query")).SendKeys("Rick Astley - Never Gonna Give You Up");
			driver.FindElement(By.Name("search_query")).Submit();

			WebDriverWait waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(1));

			Func<IWebDriver, bool> waitCondition1 = new Func<IWebDriver, bool>((IWebDriver web) =>
			{
				driver.FindElement(By.Id("video-title"));

				return true;
			});

			waiter.Until(waitCondition1);

			driver.FindElement(By.Id("video-title")).Click();

			IWebElement fullscreen = new ChromeWebElement(driver, "button");

			Func<IWebDriver, bool> waitCondition2 = new Func<IWebDriver, bool>((IWebDriver web) =>
			{
				foreach (var button in driver.FindElements(By.TagName("button")))
				{
					if (button.GetAttribute("title") == "Во весь экран")
					{
						if (button.Displayed && button.Enabled)
						{
							button.Click();
							return true;
						}
					}
				}

				return false;
			});

			waiter.Until(waitCondition2);
		}

		static void Main(string[] args)
		{
			int menuChoose;

			Console.WriteLine("Enter type of test (WebDriver):\n" +
			                  "1)				Try to login on phptravels.com\n" +
			                  "2)				YouTube test\n" +
			                  "Any other num)			Simple test of Google");

			while (true)
			{
				string input = Console.ReadLine();

				if (Int32.TryParse(input, out menuChoose))
				{
					break;					
				}
				else
				{
					Console.WriteLine("Ender valid input!");
				}
			}

			switch (menuChoose)
			{
				case 1:
					Console.WriteLine("Let's try to login...");
					TestPhptravelLogin();
					break;
				case 2:
					Console.WriteLine("Let's sing along!");
					TestYouTube();
					break;
				default:
					Console.WriteLine("Boring test..............");
					SimpleTest_Google();
					break;
			}
		}
	}
}
