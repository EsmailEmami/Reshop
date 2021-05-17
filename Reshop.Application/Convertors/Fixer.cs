namespace Reshop.Application.Convertors
{
    public static class Fixer
    {
        public static string FixedText(string email)
        {
            return email.Trim().ToLower();
        }

        public static string ToToman(this decimal value)
        {
            return value.ToString("#,0 تومان");
        }

        public static string FixedToString(object text)
        {
            return text.ToString()?.ToLower();
        }
    }
}