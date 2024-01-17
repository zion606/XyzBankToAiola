Feature: Add Customer

Background:
#Kill chrome process to avoid from many opened chrome browser.
	Given I clean env	

Scenario Outline:tests of add customer			
	Given I add a customer with details:
	 | testDescription | firstName | lastName | postCode |
	 | positiveTest    | Moshe     | Cohen    | 999      | 
	 | positiveTest    | Moshe2    | Cohen    | 999      |
	 | positiveTest    | Moshe     | Cohen2   | 999      |
	 | positiveTest    | ^%*%$*    | Cohen    | 999      |
	 | positiveTest    | Moshe     | ^%*%$*   | 999      | 
	 | negativeTest    | Moshe     | Cohen    | 999      |
	 | negativeTest    |           | Cohen    | 999      |
	 | negativeTest    | Moshe     |          | 999      |
	 | negativeTest    | Moshe     | Cohen    |          |	
	When bank manager add cusotmer to accounts
	| testDescription | firstName | lastName | postCode | currency |
	| positiveTest    | Moshe     | Cohen    | 999      | Dollar   |
	| negativeTest    | Moshe     | Cohen    | 999      | Nis      |
	Then cusotmer was added to accounts
	| testDescription | firstName | lastName | postCode | currency |
	| positiveTest    | Moshe     | Cohen    | 999      | Dollar   |

 	

@volumnTest
Scenario: volume test of 100 times input data 	
	Given I add "100" times first name "Moshe" last name "Cohen" customer with post code "999"
	

