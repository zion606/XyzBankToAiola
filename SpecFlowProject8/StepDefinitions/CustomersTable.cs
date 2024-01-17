using OpenQA.Selenium;

namespace XyzBank.StepDefinitions
{
    internal class CustomersTable
    {
        Gui gui = new();
        General general = new();
        private static string PARTIAL_CUSTOMER_TABLE_CLASS = "table";
        private static string ROW_IDENITIFIER = ".//tr";
        private static string COLUMN_IDENTIFIER = ".//td";
        public CustomersTable()
        {
            
        }

        internal class CustomerTableFields
        {
            public string FirstName;
            public string LastName;
            public string PostCode;
            public string AccountNumber;
        }

        internal bool IsCustomerShownInTable(string firstName, string lastName, string postCode)
        {
            bool returnValue = false;
            List<IWebElement> customersTableObjects = Gui.FindElementsByPartialClassName(PARTIAL_CUSTOMER_TABLE_CLASS);
            IWebElement customersTableObject = customersTableObjects[0];
            if (customersTableObject == null) 
            {
                return false;
            }
            IList<IWebElement> rows = customersTableObject.FindElements(By.XPath(ROW_IDENITIFIER));
            CustomerTableFields customerTableFields = new();
            List<CustomerTableFields> allTable = new();
            foreach (IWebElement row in rows)
            {
                IList<IWebElement> cells = row.FindElements(By.XPath(COLUMN_IDENTIFIER));
                customerTableFields.FirstName = cells[0].Text;
                customerTableFields.LastName = cells[1].Text;
                customerTableFields.PostCode = cells[2].Text;
                customerTableFields.AccountNumber = cells[3].Text;
                allTable.Add(customerTableFields);
                if (customerTableFields.FirstName.Equals(firstName) && customerTableFields.LastName.Equals(lastName) && (customerTableFields.PostCode.Equals(postCode)))
                {
                    returnValue = true;
                }
            }
            if (rows.Count != allTable.Count)
            {
                general.WriteToLog("The table not filled as needed.");
            }

            return returnValue;
        }
    }
}
