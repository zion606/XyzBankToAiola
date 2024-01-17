
using NUnit.Framework;
using System.Diagnostics;
using TechTalk.SpecFlow;
using static XyzBank.StepDefinitions.CustomersTable;

namespace XyzBank.StepDefinitions
{
    [Binding]
    public sealed class XyzBankSteps
    {
        
        internal static string LOGIN_URL = "https://www.globalsqa.com/angularJs-protractor/BankingProject/#/login";
        internal static General general = new();
        BankManager bankManager = new();
        [Given(@"I clean env")]
        public void GivenICleanEnv()
        {
#if DEBUG            
            //clean env from previous runs
            Process[] allProcess = Process.GetProcessesByName("chrome");
            foreach (var process in allProcess)
            {
                process.Kill();
            }
#endif
        }

        [Given(@"I add a customer with details:")]
        public void GivenIAddACustomerWithDetails(Table table)
        {            
            foreach (TableRow row in table.Rows)
            {                
                string testDescription = row["testDescription"];                
                string firstName = row["firstName"];
                string lastName = row["lastName"];
                string postCode = row["postCode"];
                general.WriteToLog($"***************** Starting {testDescription} with parameters firstName:{firstName},   lastName:{lastName} ,   postCode:{postCode} *****************************.");
                general.WriteToLog("**********************************************************************************************************************************************************.");

                bool addCustomerResult = AddCustomerFromBankManager(firstName, lastName, postCode);
                general.CheckResult(addCustomerResult, testDescription);
                
            }
        }

        [Then(@"cusotmer was added to accounts")]
        public void ThenCusotmerWasAddedToAccounts(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                string testDescription = row["testDescription"];
                string firstName = row["firstName"];
                string lastName = row["lastName"];
                string postCode = row["postCode"];                
                general.WriteToLog($"***************** Starting {testDescription} with parameters firstName:{firstName},   lastName:{lastName} ,   postCode:{postCode},  ****************************.");
                general.WriteToLog("*****************************************************************************************************************************************************************.");

                bool result = CheckCustomerWasAdded(firstName, lastName, postCode);
                general.CheckResult(result, testDescription);
            }
        }

        private bool CheckCustomerWasAdded(string firstName, string lastName, string postCode)
        {
            CustomersTable customersTable = new();
            BankManager bankManager = new();
            bankManager.OpenCustomersTable(customersTable);            
            return customersTable.IsCustomerShownInTable(firstName, lastName, postCode);
        }
        

        [When(@"bank manager add cusotmer to accounts")]
        public void ThenBankManagerAddCusotmerToAccounts(Table table)
        {           
            foreach (TableRow row in table.Rows)
            {
                string testDescription = row["testDescription"];
                string firstName = row["firstName"];
                string lastName = row["lastName"];
                string postCode = row["postCode"];
                string currency = row["currency"];

                general.WriteToLog($"***************** Starting {testDescription} with parameters firstName:{firstName},   lastName:{lastName} ,   postCode:{postCode},      currency:{currency}****************************.");
                general.WriteToLog("****************************************************************************************************************************************************************************************.");

                bool result = AddCustomerToAccount(firstName, lastName, currency);
                general.CheckResult(result, testDescription);
            }
        }

        private bool AddCustomerToAccount(string firstName, string lastName, string currency)
        {            
            OpenAccount account = new($"{ firstName } { lastName}", currency);
            return bankManager.OpenAccount(account);
        }

        private bool AddCustomerFromBankManager(string firstName, string lastName, string postCode)
        {            
            Login();
            Customer customer = new(firstName, lastName, postCode);
            bankManager.Login();
            bool addCustomerResult = bankManager.AddCustomer(customer);            
            return addCustomerResult;
        }

        [Given(@"I add ""([^""]*)"" times first name ""([^""]*)"" last name ""([^""]*)"" customer with post code ""([^""]*)""")]
        public void GivenIAddTimesFirstNameLastNameCustomerWithPostCode(string loopNumber, string firstName, string lastName, string postCode)
        {            
            Int32.TryParse(loopNumber, out int loopInt);
            for (int i=0; i< loopInt; i++)
            {
                if (!AddCustomerFromBankManager($"{firstName}{i}", lastName, postCode))
                {
                    Assert.Fail($"Failed to add customer {firstName}{i} {lastName} {postCode}");
                }
            }
        }

        private static void Login(int delayAfterNavigate = 1)
        {
            Gui.driver.Navigate().GoToUrl(LOGIN_URL);
            Thread.Sleep(delayAfterNavigate * 1000);
        }

    }
}
