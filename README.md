# National Wealth Bank of Australasia (NWBA)

NWBA is a simple Internet Banking System running on the web. User can log into this system and do the following actions
* Login/logout
* Deposit 
* Withdraw
* Transfer
* View Bank Statement
* Bill Pay (scheduled pay)
* View and Edit Customer Profile

## New Version

In addition to the above features, we also add extra features, including **auto-logout(1 min idle), account lock(3 failed logins), customised error page, admin features.**

Admin has the right to **update, modify, delete, view / filter transaction history, create graph for transaction history, lock/unlock user accounts and block/unblock a scheduled pay.** 


## Usage
This project is implemented with with .NET Core 3.1 framework, using Microsoft Visual Studio 2019 with a Cloud SQL Server 2019 backend. Database access is to be implemented with Entity Framework Core as the provider.

## Implementation Details
Essentially, we have 5 different options for users to choose from, including **Profile**, **Statement**, **ATM**, **Bill Pay** and **Logout**. Correspondingly, we created 5 different controllers to implement these features. 
* Customer controller works to allow user to view and edit profile page.
* Account controller works to allow user to view bank statement and account balance.
* ATM controller works to provide backend support for deposit, withdraw and transfer actions. 
* Bill Pay controller works to provide backend support for a scheduled pay for a specific payee. 
* Login controller works to closely monitor login and logout actions including userID and password authentication.

User can deposit, withdraw and transfer on the ATM page, but there are certain rules behind that. For example, user cannot withdraw money when there is insufficient balance in his account and he cannot deposit a negative number. Also, he cannot transfer money to his own account. 

Additionally, user can change his login password. However, the length of password needs to meet the requirement and when user enters a new password, he needs to re-enter the password so as to confirm the password that he has entered. If two passwords enters are inconsisitent, he will be asked to re-do this action.  

Bill Pay is a function that user can schedule a payment that might happen in the coming future like two to three days later or even longer. Upon clicking this option, user no longer needs to wait two to three days since our system can do this within just 30 seconds. Isn't that amazing!

Moreover, user can also change his own profile details. However, there are certains rules which need to be followed like address needs to be number + street name, phone number needs to be (61)-XXXXXXXX and state needs to be three uppercase letters etc. 

Most of pages, we have implemented a cancel button to make sure that user can return to the previous page, simply making it more user friendly.

