using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Xml.Linq;


namespace XyzBank.StepDefinitions
{
    internal class Gui
    {
        General general = new();
        public static ChromeDriver driver = new();        

        internal void ClickAlertOk()
        {
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();
        }

        internal void ClickButton(IWebElement button, int waitAfterClick = 0)
        {
            if (button.Displayed)
            {
                if (button.Enabled)
                {
                    button.Click();
                    if (waitAfterClick>0)
                    {
                        //measure in ms so need to mutlipy by 1000;
                        Thread.Sleep(waitAfterClick*1000);
                    }
                }
                else
                {
                    general.WriteToLog("Button is disabled. can't click on it.");
                }
            }
            else
            {
                general.WriteToLog("Button is not displayed. can't click on it.");
            }

        }

        internal IWebElement FindElementByText(string expectedText)
        {
            IWebElement element = null; 
            try            
            {
                element = driver.FindElement(By.XPath($"//*[text()='{expectedText}']"));                
                if (element == null)
                {
                    general.WriteToLog($"Element {expectedText} was not found.");
                }               
            }            
            catch (Exception ex)
            {
                general.WriteToLog($"Exception while try find element by text {expectedText}. exception {ex}.");                
            }
            return element;
        }

        internal string GetAlertMessageText()
        {
            IAlert alert = driver.SwitchTo().Alert();
            string alertText = alert.Text;
            if (String.IsNullOrEmpty(alertText))
            {
                general.WriteToLog($"Alert text is empty");
            }
            return alertText;
        }

        internal List<IWebElement> GetSubElementsWithLabel(List<IWebElement> parentElement, string expectedPlaceHolder)
        {
            string cssSelector = $"[placeholder='{expectedPlaceHolder}']";
            List<IWebElement> subElements = new() { };
            foreach (IWebElement element in parentElement)
            {
                subElements = element.FindElements(By.CssSelector(cssSelector)).ToList();
                if (subElements.Any())
                {
                    return subElements;
                }
            }
            return subElements;
        }

        internal static List<IWebElement> FindElementsByPartialClassName(string partialClassName)
        {                       
            List<IWebElement> options = driver.FindElements(By.CssSelector($"[class*='{partialClassName}']")).ToList(); ;
            return options;
        }

        internal static string SelectFromCombobox(IWebElement comboObject, string option)
        {
            comboObject.Click();
            string allOptionsAvailable = string.Empty;
            IList<IWebElement> childElements = comboObject.FindElements(By.XPath(".//*"));
            foreach (IWebElement element in childElements) 
            {
                allOptionsAvailable += element.Text + ",";
                if (element.Text.Equals(option))
                {
                    element.Click();
                    break;
                }
            }
            return allOptionsAvailable;
        }

        internal static void SelectFromCombobox(IWebElement comboObject, string partialClassName, string customer, int delayAfterClick = 2)
        {
            try
            {
                comboObject.Click();
                List<IWebElement> customerOptions = FindElementsByPartialClassName(partialClassName);
                foreach (IWebElement customerOption in customerOptions)
                {
                    if (customerOption.Text.Equals(customer))
                    {                        
                        customerOption.Click();                        
                        customerOption.SendKeys(Keys.Enter);
                        Thread.Sleep(delayAfterClick * 1000);
                        break;
                    }
                }                
            }
            catch (Exception) 
            {
                
            }
            
        }
    }
}
