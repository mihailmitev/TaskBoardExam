using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;




namespace WebDriverTaskBoardTests
{
    public class UITests
    {

        private WebDriver driver;
        [SetUp]
        public void OpenBrowser()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [TearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_ListTasks_CheckFirstTask()
        {
            driver.Navigate().GoToUrl("https://taskboard.nakov.repl.co/");
            var contactLink = driver.FindElement(By.LinkText("Task Board"));
            contactLink.Click();
            var task = driver.FindElement(By.CssSelector("div:nth-of-type(3) > h1")).Text;
            var name = driver.FindElement(By.CssSelector("div:nth-of-type(3) > table:nth-of-type(1)  .title > td")).Text;
            Assert.That(task, Is.EqualTo("Done"));
            Assert.That(name, Is.EqualTo("Project skeleton"));


        }
        
        [Test]
        public void Test_SearchTasks_CheckFirstResult()
        {
            driver.Navigate().GoToUrl("https://taskboard.nakov.repl.co/");
            driver.FindElement(By.LinkText("Search")).Click();
            var searchField = driver.FindElement(By.Id("Keyword"));
            searchField.SendKeys("home");
            driver.FindElement(By.Id("Search")).Click();
            var title = driver.FindElement(By.CssSelector(".title > th")).Text;
            var name = driver.FindElement(By.CssSelector(".title > td")).Text;
            Assert.That(title, Is.EqualTo("Title"));
            Assert.That(name, Is.EqualTo("Home page"));


        }
        
        [Test]
        public void Test_SearchTasks_EmptyResult()
        {
            driver.Navigate().GoToUrl("https://taskboard.nakov.repl.co/");
            driver.FindElement(By.LinkText("Search")).Click();
            var searchField = driver.FindElement(By.Id("Keyword"));
            searchField.SendKeys("oha1234ohoh");
            driver.FindElement(By.Id("Search")).Click();
            var resultLabel = driver.FindElement(By.Id("searchResult")).Text;
            Assert.That(resultLabel, Is.EqualTo("No tasks found."));
        }
        
        [Test]
        public void Test_CreateTask_InvalidData()
        {
            driver.Navigate().GoToUrl("https://taskboard.nakov.repl.co/");
            driver.FindElement(By.LinkText("Create")).Click();
            var description = driver.FindElement(By.Id("description"));
            description.SendKeys("nestavaopitaipak");
            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();
            var errorMessage = driver.FindElement(By.CssSelector(".err")).Text;
            Assert.That(errorMessage, Is.EqualTo("Error: Title cannot be empty!"));

        }
        
        [Test]
        public void Test_CreateTask_ValidData()
        {
            driver.Navigate().GoToUrl("https://taskboard.nakov.repl.co/");
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 832);
            driver.FindElement(By.LinkText("Create")).Click();
            driver.FindElement(By.Id("title")).Click();
            driver.FindElement(By.Id("title")).SendKeys("misho" + DateTime.Now.Ticks);
            driver.FindElement(By.Id("description")).Click();
            driver.FindElement(By.Id("description")).SendKeys("12345");
            driver.FindElement(By.Id("create")).Click();
            Assert.That(driver.FindElement(By.CssSelector("#task414 .title > td")).Text, Is.EqualTo("misho"));
            Assert.That(driver.FindElement(By.CssSelector("#task414 .description .description")).Text, Is.EqualTo("12345"));
            
            /* 
             driver.Navigate().GoToUrl("https://taskboard.nakov.repl.co/");
            driver.FindElement(By.LinkText("Create")).Click();

            var title = "FirstName"; //+ DateTime.Now.Ticks;
            var description = "LastName"; //+ DateTime.Now.Ticks;
           
  
            driver.FindElement(By.Id("Title")).SendKeys(title);
            driver.FindElement(By.Id("Description")).SendKeys(description);
            

            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();
            var allTasks = driver.FindElements(By.CssSelector("table.task-entry"));
            var lasttaks = allTasks.Last();

            var titleLable = lasttaks.FindElement(By.XPath("//div[@class='tasks-grid']/div[1]/table[1]//tr[@class='title']/th[.='Title']")).Text;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            Assert.That(titleLable, Is.EqualTo(title));
            */

        }
        
    }
}

