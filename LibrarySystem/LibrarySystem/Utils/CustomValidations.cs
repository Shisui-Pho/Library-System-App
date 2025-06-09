namespace LibrarySystem.Utils;
public static class CustomValidations
{
    public static bool IsValidSANumber(string number, out string message, out string standadisedNumber)
    {
        standadisedNumber = "";
        if(number == null)
        {
            message = "Phone number not provided";
            return false;
        }
        //Default message
        message = "Phone number not valid";

        //Remove all spaces
        number = number.Replace(" ","");
        //Should be between 10 and 14 characters without the white spaces
        if (number.Length < 10 || number.Length > 14)
        {
            return false;
        }
        //Here the number has length >= 10 and not null
        //-Three cases: 
        //---- 1.  (+27)123456789 
        //---- 2.  +27123456789
        //---- 3.  0123456789
        if (number.StartsWith("(+27)") || number.StartsWith("+27"))
        {
            //Replace number with 0
            number = number.StartsWith("(+27)") ? number.Replace("(+27)", "0")
                                                : number.Replace("+27", "0");
        }

        //Here we assuming the number is clean 
        if(number.Length == 10 && number.StartsWith('0'))
        {
            standadisedNumber = number;
            message = "";
            return true;
        }

        return false;
    }
}//class
