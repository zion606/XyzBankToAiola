using OpenQA.Selenium;

namespace XyzBank.StepDefinitions
{
    internal class BankManager
    {
        private static string BANK_MANAGER_TEXT = "Bank Manager Login";
        private static string ADD_CUSTOMER_TEXT = "Add Customer";
        private static string OPEN_ACCOUNT_TEXT = "Open Account";
        private static string CUSTOMERS_TEXT = "Customers";
        private static string ADD_CUSTOMRT_CLASS_NAME = "btn-lg";
        private static string OPEN_ACCOUNT_CLASS_NAME = "btn-lg";
        private static string OPEN_CUSTOMERS_TABLE_CLASS_NAME = "btn-lg";
        Gui gui = new();   
        General general = new();
        internal bool Login()
        {
            try
            {
                Thread.Sleep(1000);
                IWebElement bankManagerObject = gui.FindElementByText(BANK_MANAGER_TEXT);                
                gui.ClickButton(bankManagerObject,2);                
                return true;
            }
            catch (Exception ex)
            {
                general.WriteToLog($"Failed to login to Bank Manager. Exception is: {ex}.");
                return false;
            }            
        }
        
        internal bool AddCustomer(string firstName, string lastName, string postCode)
        {
            Customer customer = new(firstName,lastName,postCode);
            return customer.Add();            
        }

        internal bool AddCustomer(Customer customer)
        {
            List<IWebElement> buttons = Gui.driver.FindElements(By.XPath($"//button[contains(@class, '{ADD_CUSTOMRT_CLASS_NAME}')]")).ToList();
            foreach (IWebElement button in buttons)
            {
                if (button.Text.ToString().Equals(ADD_CUSTOMER_TEXT))
                {
                    gui.ClickButton(button,1);                    
                    break;
                }
            }
            return customer.Add();            
        }

        internal bool OpenAccount(OpenAccount account)
        {
            List<IWebElement> buttons = Gui.driver.FindElements(By.XPath($"//button[contains(@class, '{OPEN_ACCOUNT_CLASS_NAME}')]")).ToList();
            foreach (IWebElement button in buttons)
            {
                if (button.Text.ToString().Equals(OPEN_ACCOUNT_TEXT))
                {
                    gui.ClickButton(button, 1);
                    break;
                }
            }
            return account.Add();
        }

        internal void OpenCustomersTable(CustomersTable customerTable)
        {
            List<IWebElement> buttons = Gui.driver.FindElements(By.XPath($"//button[contains(@class, '{OPEN_CUSTOMERS_TABLE_CLASS_NAME}')]")).ToList();
            foreach (IWebElement button in buttons)
            {
                if (button.Text.ToString().Equals(CUSTOMERS_TEXT))
                {
                    gui.ClickButton(button, 1);
                    break;
                }
            }
            
        }
    }
}

