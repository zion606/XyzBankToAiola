using OpenQA.Selenium;


namespace XyzBank.StepDefinitions
{
    internal class OpenAccount
    {
        private static string CUSTOMER_OBJECT_ID = "userSelect";
        private static string CURRENCY_OBJECT_ID = "currency";
        private static string PARTIAL_CUSTOMER_CLASS_NAME = "ng-binding";
        private static string CURRENCY = string.Empty;
        private static string CUSTOMER = string.Empty;
        private static string PROCESS_BUTTON_TEXT = "Process";
        private static string ACCOUNT_CREATED_SUCCESSFULLY_MESSAGE = "Account created successfully with account Number";
        
        General general = new();
        Gui gui = new();

        public OpenAccount(string customer, string currency)
        {
            CUSTOMER = customer;
            CURRENCY = currency;
        }
        internal bool Add()
        {
            IWebElement customerObject = Gui.driver.FindElement(By.Id(CUSTOMER_OBJECT_ID));                                
            Gui.SelectFromCombobox(customerObject, partialClassName: PARTIAL_CUSTOMER_CLASS_NAME, CUSTOMER);                
            
            IWebElement currencyObject = Gui.driver.FindElement(By.Id(CURRENCY_OBJECT_ID));
            string allOptions = Gui.SelectFromCombobox(currencyObject, CURRENCY);            

            IWebElement processButtonObject = gui.FindElementByText(PROCESS_BUTTON_TEXT);
            processButtonObject.Click();

            return HandleAlertMsg(allOptions);
            
        }

        private bool HandleAlertMsg(string allOptions)
        {
            try
            {
                string alertText = gui.GetAlertMessageText();
                if (alertText.Contains(ACCOUNT_CREATED_SUCCESSFULLY_MESSAGE))
                {
                    gui.ClickAlertOk();
                    return true;
                }
            }
            catch (Exception ex)
            {
                general.WriteToLog($"Failed to create account for {CUSTOMER} \n. Exception {ex}.\n Please chack that {CURRENCY} is one from those options: {allOptions}.");
            }            
            return false;        
        }
    }
}

