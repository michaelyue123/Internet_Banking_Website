namespace IBW.Utilities
{
    // reference to week 7 tutorial
    public static class MiscellaneousExtensionUtilities
    {
        // check if user give too many decimal places
        public static bool HasMoreThanNDecimalPlaces(this decimal value, int n) => decimal.Round(value, n) != value;
        public static bool HasMoreThanTwoDecimalPlaces(this decimal value) => value.HasMoreThanNDecimalPlaces(2);
    }
}
