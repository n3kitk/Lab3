
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace Lab3
{
    public class Variant3Tests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        // 1. Checkboxes
        [Test]
        public void CheckboxesTest()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/checkboxes");

            var checkboxes = driver.FindElements(By.CssSelector("input[type='checkbox']"));

            if (!checkboxes[0].Selected)
            {
                checkboxes[0].Click();
            }

            if (checkboxes[1].Selected)
            {
                checkboxes[1].Click();
            }

            Assert.That(checkboxes[0].Selected, Is.True);
            Assert.That(checkboxes[1].Selected, Is.False);
        }

        // 2. Add/Remove Elements
        [Test]
        public void AddRemoveElementsTest()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/add_remove_elements/");

            var addButton = driver.FindElement(By.XPath("//button[text()='Add Element']"));

            addButton.Click();
            addButton.Click();

            var deleteButtons = driver.FindElements(By.ClassName("added-manually"));
            Assert.That(deleteButtons.Count, Is.EqualTo(2));

            deleteButtons[0].Click();
            System.Threading.Thread.Sleep(500);
            deleteButtons = driver.FindElements(By.ClassName("added-manually"));

            Assert.That(deleteButtons.Count, Is.EqualTo(1));

            deleteButtons[0].Click();
            System.Threading.Thread.Sleep(500);
            deleteButtons = driver.FindElements(By.ClassName("added-manually"));

            Assert.That(deleteButtons, Is.Empty);
        }

        // 3. Dropdown
        [Test]
        public void DropdownTest()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dropdown");

            var dropdownElement = driver.FindElement(By.Id("dropdown"));
            var selectElement = new SelectElement(dropdownElement);

            selectElement.SelectByValue("1");
            Assert.That(selectElement.SelectedOption.Text, Is.EqualTo("Option 1"));
        }

        // 4. Inputs
        [Test]
        public void InputsTest()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/inputs");

            var inputField = driver.FindElement(By.XPath("//input[@type='number']"));
            inputField.SendKeys("19");

            Assert.That(inputField.GetAttribute("value"), Is.EqualTo("19"));
        }

        // 5. Status Codes
        [Test]
        public void StatusCodesTest()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/status_codes");
            driver.FindElement(By.LinkText("200")).Click();

            string pageText = driver.FindElement(By.CssSelector(".example p")).Text;
            Assert.That(pageText, Does.Contain("This page returned a 200 status code"));
        }

        // 6. Nested Frames
        [Test]
        public void NestedFramesTest()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/nested_frames");
            driver.SwitchTo().Frame("frame-top");
            driver.SwitchTo().Frame("frame-middle");

            string content = driver.FindElement(By.Id("content")).Text;
            Assert.That(content, Is.EqualTo("MIDDLE"));

            driver.SwitchTo().DefaultContent();
        }

        // 7. Form Authentication
        [Test]
        public void FormAuthenticationTest()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

            driver.FindElement(By.Id("username")).SendKeys("tomsmith");
            driver.FindElement(By.Id("password")).SendKeys("SuperSecretPassword!");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            string flashMessage = driver.FindElement(By.Id("flash")).Text;
            Assert.That(flashMessage, Does.Contain("You logged into a secure area!"));
        }

        // 8. Dynamic Content
        [Test]
        public void DynamicContentTest()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_content");

            string initialContent = GetDynamicContent();

            Assert.That(initialContent, Is.Not.Empty);

            bool contentChanged = false;

            for (int i = 0; i < 10; i++)
            {
                driver.Navigate().Refresh();

                string newContent = GetDynamicContent();

                if (initialContent != newContent)
                {
                    contentChanged = true;
                    break;
                }
            }

            Assert.That(contentChanged, Is.True);
        }

        private string GetDynamicContent()
        {
            string content = "";
            var textElements = driver.FindElements(By.CssSelector("#content .large-10"));
            foreach (IWebElement element in textElements)
            {
                content += element.Text;
            }

            var images = driver.FindElements(By.CssSelector("#content .large-2 img"));
            foreach (IWebElement image in images)
            {
                content += image.GetAttribute("src");
            }

            return content;
        }

        // 9. Disappearing Elements
        [Test]
        public void DisappearingElementsTest()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/disappearing_elements");

            var menuLinks = driver.FindElements(By.CssSelector(".example ul li a"));

            Assert.That(menuLinks.Count, Is.InRange(4, 5));

            Assert.That(driver.FindElements(By.LinkText("Home")), Is.Not.Empty);
            Assert.That(driver.FindElements(By.LinkText("About")), Is.Not.Empty);
            Assert.That(driver.FindElements(By.LinkText("Contact Us")), Is.Not.Empty);
            Assert.That(driver.FindElements(By.LinkText("Portfolio")), Is.Not.Empty);

            var galleryLinks = driver.FindElements(By.LinkText("Gallery"));

            if (galleryLinks.Count > 0)
            {
                Assert.That(galleryLinks[0].Displayed, Is.True);
            }
        }

        // 10. Hover
        [Test]
        public void HoverTest()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/hovers");

            Actions actions = new Actions(driver);

            var firstAvatar = driver.FindElement(By.ClassName("figure"));
            actions.MoveToElement(firstAvatar).Perform();
            var figCaption = firstAvatar.FindElement(By.ClassName("figcaption"));

            Assert.That(figCaption.Displayed, Is.True);
            Assert.That(figCaption.Text, Does.Contain("name: user1"));
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}
