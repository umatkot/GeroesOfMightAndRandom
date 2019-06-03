namespace GeroesOfMightAndRandom.Statics
{
    public static class StringExtensions
    {
        /// <summary>
        /// Очищает строку от пробелов
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ClearSpaces(this string data)
        {
            return data.Replace(" ", "");
        }
    }
}
