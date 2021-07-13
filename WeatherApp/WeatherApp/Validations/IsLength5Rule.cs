namespace WeatherApp.Validations
{
    public class IsLength5Rule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return true;
            }

            var str = value as string;

            return str == null ? false : !((str.Trim().Length > 5));
        }
    }
}
