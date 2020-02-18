using System;
using System.Collections.Generic;
using System.Linq;
using IBW.Model;
namespace IBW.Utilities
{
    public class TransferExtensionUtilities
    {
        // check transfer to same account
        public static bool IsSameAcc(Transaction transaction) {
            if (transaction.AccountNumber == transaction.DestinationAccountNumber)
                return true;
            return false;
        }

        // check if destination is null when trasaction
        public static bool InvalidTransfer(Transaction transaction) {
            if (transaction.TransactionType.Equals(TransactionType.Transfer) && transaction.DestinationAccountNumber is null) {
                return true;
            }
            return false;

        }

        // check if destination is null when not trasaction
        public static bool InvalidDepo(Transaction transaction)
        {
            if (!transaction.TransactionType.Equals(TransactionType.Transfer) && !(transaction.DestinationAccountNumber is null))
            {
                return true;
            }
            return false;

        }

        // check if the account has enough balance
        public static bool checkBalance(Transaction transaction, Account account) {
            decimal minimalAmount;
            decimal serviceFee = .0m;
            if (transaction.TransactionType.Equals(TransactionType.Deposit))
                return true;

            if (account.AccountType.Equals(AccountType.Saving))
                minimalAmount = 100;
            else
                minimalAmount = 500;

            if (transaction.TransactionType == TransactionType.Transfer)
                serviceFee = .2m;
            if (transaction.TransactionType == TransactionType.Withdraw)
                serviceFee = .1m;

            if ((account.Balance - transaction.Amount - serviceFee) >= minimalAmount)
                return true;
            else
                return false;
        }
    }

    // build transaction
    public static class TransactionBuilder
    {

        public static Transaction BuildTransaction(BillPay bill) {
            Transaction transaction = new Transaction()
            {
                TransactionType = TransactionType.Transfer,
                AccountNumber = bill.AccountNumber,
                DestinationAccountNumber = bill.PayeeID,
                Amount = bill.Amount,
                TransactionTimeUtc = DateTime.UtcNow,
                Comment = "Transfer for Bill pay"

            };
            return transaction;
        }
        public static Transaction BuildTransaction(Transaction transaction) {
            Transaction serviceTransaction = new Transaction()
            {
                TransactionType = TransactionType.ServiceCharge,
                AccountNumber = transaction.AccountNumber,
                TransactionTimeUtc = DateTime.UtcNow
            };

            if (transaction.TransactionType.Equals(TransactionType.Transfer))
            {
                serviceTransaction.Amount = .2m;
                serviceTransaction.Comment = "service charge for transfer";
            }


            if (transaction.TransactionType.Equals(TransactionType.Withdraw))
            {
                serviceTransaction.Amount = .1m;
                serviceTransaction.Comment = "service charge for WithDraw";
            }
            return serviceTransaction;
        }
    }
}
