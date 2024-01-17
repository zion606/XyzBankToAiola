using OpenQA.Selenium;

namespace XyzBank.StepDefinitions
{
    internal class Customer
    {
        private static string FIRST_NAME = String.Empty;
        private static string LAST_NAME = String.Empty;
        private static string POST_CODE = String.Empty;
        private static string ADD_CUSTOMER_BUTTON_TEXT = "Add Customer";
        private static string FIRST_NAME_PLACEHOLDER = "First Name";
        private static string LAST_NAME_PLACEHOLDER = "Last Name";
        private static string POST_CODE_PLACEHOLDER = "Post Code";
        private static string FORM_GROUP_CLASS_NAME= "form-group";        
        private static string CUSTOMER_ADDED_SUCCESSFULLY_MESSAGE = "Customer added successfully with customer id";
        private static string CUSTOMER_DUPLICATION_MESSAGE = "Please check the details. Customer may be duplicate";
        Gui gui = new();
        General general = new();        

        internal Customer(string firstName, string lastName, string postCode) 
        {            
            FIRST_NAME = firstName;
            LAST_NAME = lastName;
            POST_CODE = postCode;
        }

        internal bool Add()
        {
            bool isAllDataFilled = true;
            if (!FillCustomerDetails())
            {
                isAllDataFilled = false;
            }
            IWebElement addButton = gui.FindElementByText(ADD_CUSTOMER_BUTTON_TEXT);            
            gui.ClickButton(addButton);    
            if (isAllDataFilled)
            {
                return HandleAlertMsg();                
            }            
            return false;
          
        }

        private bool HandleAlertMsg()
        {
            string alertText = gui.GetAlertMessageText();
            if (alertText.Contains(CUSTOMER_ADDED_SUCCESSFULLY_MESSAGE))
            {
                gui.ClickAlertOk();
                return true;
            }
            else if (alertText.Contains(CUSTOMER_DUPLICATION_MESSAGE))
            {
                gui.ClickAlertOk();
                general.WriteToLog($"Failed to create customer since it is exist already.");
                return false;
            }
            else
            {
                general.WriteToLog($"Failed to create customer. Please check the fields was insert correctly. \n fyi: firstName :{FIRST_NAME}, lastName:{LAST_NAME},  postCode:{POST_CODE}.");
                return false;
            }

        }

        private bool FillCustomerDetails()
        {
            bool isFilledAllData = true;
            if (!CheckDataIsValid())
            {
                isFilledAllData = false;
            }
            List<IWebElement> allFormOptions = Gui.driver.FindElements(By.ClassName(FORM_GROUP_CLASS_NAME)).ToList();            
            List<IWebElement> firstNameObjects = gui.GetSubElementsWithLabel(allFormOptions, FIRST_NAME_PLACEHOLDER);
            firstNameObjects?.FirstOrDefault()?.SendKeys(FIRST_NAME);
            List<IWebElement> lastNameObjects = gui.GetSubElementsWithLabel(allFormOptions, LAST_NAME_PLACEHOLDER);
            lastNameObjects?.FirstOrDefault()?.SendKeys(LAST_NAME);
            List<IWebElement> postCodeObjects = gui.GetSubElementsWithLabel(allFormOptions, POST_CODE_PLACEHOLDER);
            postCodeObjects?.FirstOrDefault()?.SendKeys(POST_CODE);
            return isFilledAllData;
        }

        private bool CheckDataIsValid()
        {                    
            if (String.IsNullOrEmpty(FIRST_NAME))
            {
                general.WriteToLog("Missing customer first name.");
                return false;
            }
            if (String.IsNullOrEmpty(LAST_NAME))
            {
                general.WriteToLog("Missing customer last name.");
                return false;
            }
            if (String.IsNullOrEmpty(POST_CODE))
            {
                general.WriteToLog("Missing customer post code.");
                return false;
            }
            return true;
        }
    }
}



