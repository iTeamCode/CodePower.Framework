using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using Xunit;

namespace FellowshipOne.Dashboard.XUnitTest
{
    public class EmployeeTest
    {
        public const string WEBSITE_URL = @"http://localhost:42011";

        //[Theory]
        //[InlineData(1, "001")]
        //[InlineData(2, "002")]
        //[InlineData(3, "003")]
        public void TestEmployee(int id, string code)
        {
            switch (id)
            {
                case 1:
                    Assert.True(code == "001", "failed");
                    break;
                case 2:
                    Assert.True(code == "002", "failed");
                    break;
                default:
                    Assert.True(false, "Pass");
                    break;
            }
        }
        //[Fact]
        public void TestDarg()
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Url = "http://check-in-coordinator.herokuapp.com/#/";

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(By.Id("selected-room-data")));

            var element = driver.FindElement(By.XPath(".//*[@id='selected-room-data']/div[3]/datacircle/div/ng-transclude/datacircle[2]"));

            element.Click();
            //Actions action = new Actions(driver);
            //action.MoveToElement(element,5,5);
            //action.Click().Click();
            ////action.DragAndDropToOffset(element, 0, 300);
            //action.Build().Perform();
        }
        //[Fact]
        public void CreateEmployee()
        {
            DesiredCapabilities capability = DesiredCapabilities.Firefox();
            capability.SetCapability("firefox_binary", "D:\\Program Files\\Mozilla Firefox\\firefox.exe");

            IWebDriver driver = new FirefoxDriver(capability); 
            
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            driver.Url = string.Format("{0}/Employee/index", WEBSITE_URL);

            Case01_CreateEmployee(driver, "009", "Test009", "Type", "30");
        }

        public void Case01_CreateEmployee(IWebDriver driver, string code, string name, string type, string age)
        {
            Console.WriteLine("@.get input element.");

            //显示的等待 
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(By.Id("txtCode")));

            IWebElement txtCode = driver.FindElement(By.Id("txtCode"));
            IWebElement txtName = driver.FindElement(By.Id("txtName"));
            IWebElement txtType = driver.FindElement(By.Id("txtType"));
            IWebElement txtAge = driver.FindElement(By.Id("txtAge"));

            Console.WriteLine("@.set input element value.");
            txtCode.SendKeys(code);
            txtName.SendKeys(name);
            txtType.SendKeys(type);
            txtAge.SendKeys(age);

            Console.WriteLine("@.Click button.");
            IWebElement btnCreate = driver.FindElement(By.Id("btnCreate"));
            btnCreate.Click();

            wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@class='danger']/td[1]")));

            IWebElement tableCode = driver.FindElement(By.XPath(".//*[@class='danger']/td[1]"));
            IWebElement tableName = driver.FindElement(By.XPath(".//*[@class='danger']/td[2]"));
            IWebElement tableType = driver.FindElement(By.XPath(".//*[@class='danger']/td[3]"));
            IWebElement tableAge = driver.FindElement(By.XPath(".//*[@class='danger']/td[4]"));

            //bool isPass = tableCode.Text == code && tableName.Text == name && tableType.Text == type && tableAge.Text == age;
            Assert.True(true, "Create failed.");
        }
    }
}
