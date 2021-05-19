using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace lab.PDFExcelCSVExportApp.Helpers
{
    public static class EnumExtensions
    {
        public static string ToDescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
        public static string ToDisplayNameAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DisplayAttribute[] attributes = (DisplayAttribute[])fi.GetCustomAttributes(
                typeof(DisplayAttribute), false);

            if (attributes.Length > 0) return attributes[0].Name;
            else return source.ToString();
        }
        public static DisplayAttribute ToDisplayAttr<T>(this T source)
        {
            var input = source.ToString();
            FieldInfo fi = source.GetType().GetField(input);

            DisplayAttribute[] attributes = (DisplayAttribute[])fi.GetCustomAttributes(
                typeof(DisplayAttribute), false);

            if (attributes.Length > 0) return attributes[0];
            else return new DisplayAttribute { Name = input, ShortName = input, Description = input };
        }
    }
    public static class TypeExtensions
    {

        //private static bool IsNumeric(this string val)
        //{
        //    int tmp;
        //    if (val == "")
        //        return false;
        //    bool t = Int32.TryParse(val, out tmp);
        //    return t;
        //}
        /// <summary>
        /// Perse date string to Datetime
        /// Format: dd/MMM/yyyy
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string IsValidDateThenParseString(this string dateTime)
        {
            if (!string.IsNullOrEmpty(dateTime))
            {
                DateTime outDateTimeTime;

                if (DateTime.TryParseExact(dateTime, "MM/dd/yyyy", null, DateTimeStyles.None, out outDateTimeTime) == true)
                {
                    return dateTime;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
        public static DateTime? IsValidDateThenParseDate(this string dateTime)
        {
            DateTime outDateTimeTime;
            if (DateTime.TryParse(dateTime, out outDateTimeTime))
            {
                return DateTime.Parse(outDateTimeTime.ToString("dd/MMM/yyyy"));
            }
            return null;
        }
        public static string IsValidDateThenParseDateToString(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString("dd/MMM/yyyy");
            }
            return null;
        }
        public static DateTime? IsValidDateThenParseDateTime(this DateTime? dateTime, string time)
        {
            if (dateTime.HasValue)
            {
                string format = "dd/MMM/yyyy";
                var datetime = dateTime.Value.ToString(format) + " " + time;
                DateTime outDateTimeTime;
                if (DateTime.TryParse(datetime, out outDateTimeTime))
                {
                    return DateTime.Parse(outDateTimeTime.ToString(format));
                }
            }
            return null;
        }
        public static DateTime? IsValidDateThenParseDateTime(this string dateTime)
        {
            DateTime outDateTimeTime;
            if (DateTime.TryParse(dateTime, out outDateTimeTime))
            {
                return DateTime.Parse(outDateTimeTime.ToString("dd/MMM/yyyy hh:mm tt"));
            }
            return null;
        }

        public static DateTime ToUtcDateTime(this DateTime utcDateTime)
        {
            utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Local);
            DateTime time = TimeZoneInfo.ConvertTimeToUtc(utcDateTime);
            return time;
        }
        public static TimeZoneInfo GetTimeZoneInfo(this string timeZoneId)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        public static string GetUtcTimeOffsetString(this string timeZoneId)
        {
            try
            {
                TimeZoneInfo utz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return utz.GetUtcOffset(DateTime.Now).ToString();
            }
            catch (TimeZoneNotFoundException)
            {
                return "";
            }
            catch (InvalidTimeZoneException)
            {
                return "";
            }
        }

        public static string GetFormattedString(this string input)
        {
            string str = System.Text.RegularExpressions.Regex.Replace(input, "<[^>]*>", string.Empty).Trim();
            return str;
        }
        public static bool IsValidEmailAddress(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        public static bool IsNumeric(this string str)
        {
            Regex _isNumber = new Regex(@"^\d+.\d+$");
            Match m = _isNumber.Match(str);
            return m.Success;
        }
        /// <summary>
        /// Return -99 when value is null or 0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Int32 ToInt3299(this object obj)
        {
            return Convert.ToInt32(obj == null || Convert.ToString(obj) == string.Empty ? 0 : obj);
        }
        public static int ToInt(this object obj, int defaultValue)
        {
            return ToInt32(obj, defaultValue);
        }
        public static int ToInt(this object obj)
        {
            return ToInt32(obj, 0);
        }
        public static Guid ToGuid(this object obj)
        {
            if (obj == null) return Guid.Empty;
            Guid outResult;
            bool result = Guid.TryParse(obj.ToString(), out outResult);
            if (result)
                return outResult;
            return Guid.Empty;
        }
        public static string ToGuidString(this Guid obj)
        {
            return obj.ToString("N").ToLower();
        }
        public static string ToGuidString(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
                return obj.ToGuid().ToString("N").ToLower();
            return null;
        }
        public static int ToInt32(this object obj, int defaultValue)
        {
            if (obj == null || Convert.ToString(obj) == string.Empty || obj.ToString() == "-99" || obj.ToString().ToLower() == "new" || obj.ToString().ToLower() == "false")
            {
                return defaultValue;
            }
            if (obj.ToString().ToLower() == "true")
            {
                return 1;
            }
            return Convert.ToInt32(obj);
        }
        public static Int64 ToInt64(this object obj)
        {
            var input = obj == null || Convert.ToString(obj).Trim() == string.Empty || obj.ToString() == "-99" || obj.ToString().ToLower() == "new" ? 0 : obj;
            long result = 0;
            long.TryParse(input.ToString(), out result);
            return result;
        }
        public static Int64 ToLong(this object obj)
        {
            return ToInt64(obj);
        }
        public static Decimal ToDecimal(this object obj)
        {
            return Convert.ToDecimal(obj == null || Convert.ToString(obj) == string.Empty || obj.ToString() == "-99" || obj.ToString().ToLower() == "new" ? 0 : obj);
        }
        public static double ToDouble(this decimal obj)
        {
            return Convert.ToDouble(obj);
        }
        public static Decimal? ToRound(this object obj)
        {
            Decimal? tmp = Convert.ToDecimal(obj == null || Convert.ToString(obj) == string.Empty || obj.ToString() == "-99" || obj.ToString().ToLower() == "new" ? null : obj);
            if (tmp != null)
            {
                return Math.Round(Convert.ToDecimal(tmp), 2);
            }
            else
            {
                return tmp;
            }
        }

        public static string ToProperCase(this string str)
        {
            string formattedText = null;

            if (!string.IsNullOrEmpty(str))
            {
                formattedText = new System.Globalization.CultureInfo("en").TextInfo.ToTitleCase(str.ToLower());
            }
            return formattedText;
        }
        //public static decimal ToRound(this decimal value)
        //{
        //    decimal output = 0;
        //    output = Math.Round(value, Periscope.Erp.Utility.Common.DecimalLength);
        //    return output;
        //}
        public static string MakeRoundToString(this object value)
        {
            decimal output = 0;
            if (value != null)
            {
                return decimal.TryParse(value.ToString(), out output) ? output.ToRound().ToString() : string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }
        public static string ToProperCaseNoS(this object obj)
        {
            if (obj != null)
                return ToProperCaseNoS(obj.ToString());
            else
                return null;
        }
        public static string ToProperCaseNoS(this string str)
        {
            string formattedText = null;

            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace('-', ' ');
                formattedText = new System.Globalization.CultureInfo("en").TextInfo.ToTitleCase(str.ToLower());
            }
            return formattedText;
        }
        public static string ToProperCaseLink(this string str)
        {
            string formattedText = null;

            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace(' ', '-');
                formattedText = new System.Globalization.CultureInfo("en").TextInfo.ToLower(str.ToLower());
            }
            return formattedText;
        }
        public static string ToDigit(this string str)
        {
            string formattedText = null;

            if (!string.IsNullOrEmpty(str))
            {
                formattedText = new string(str.Where(c => char.IsDigit(c)).ToArray());
            }
            return formattedText;
        }
        public static string ToUSAFormat(this string str)
        {
            string formattedText = null;

            if (!string.IsNullOrEmpty(str))
            {
                var value = Convert.ToInt64(str);
                formattedText = String.Format("{0:###-### ####}", value);
            }
            return formattedText;
        }
        public static bool IsValidDateTime(string dateTime)
        {
            DateTime tempDate;
            return DateTime.TryParse(dateTime, out tempDate);
        }
        public static string ToFormatedDateString(this object obj)
        {
            if (obj != null)
                return ToFormatedDateString(Convert.ToDateTime(obj));
            return null;
        }

        public static string ToFormatedDateTimeString(this object obj)
        {
            if (obj != null)
                return ToFormatedDateTimeString(Convert.ToDateTime(obj));
            return null;
        }
        public static string ToFormatedDateTimeWithAmPmString(this object obj)
        {
            if (obj != null)
                return ToFormatedDateTimeWithAmPmString(Convert.ToDateTime(obj));
            return null;
        }
        public static string ToFormatedScgDateTimeWithAmPmString(this object obj)
        {
            if (obj != null)
                return ToFormatedScgDateTimeWithAmPmString(Convert.ToDateTime(obj));
            return null;
        }
        public static string ToFormatedTimeInMinuteString(this object obj)
        {
            if (obj != null)
                return ToFormatedTimeInMinuteString(Convert.ToDateTime(obj));
            return null;
        }
        public static string ToFormatedTimeString(this object obj)
        {
            if (obj != null)
                return ToFormatedTimeString(Convert.ToDateTime(obj));
            return null;
        }
        public static string ToFormatedTimeWithAmPmString(this object obj)
        {
            if (obj != null)
                return ToFormatedTimeWithAmPmString(Convert.ToDateTime(obj));
            return null;
        }
        public static string ToDbFormatedDateString(this object obj)
        {
            if (obj != null)
            {
                var date = Convert.ToDateTime(obj);
                return date.ToString("dd-MMM-yyyy");
            }
            return null;
        }
        public static string ToDbFormatedDateTimeString(this object obj)
        {
            if (obj != null)
            {
                var date = Convert.ToDateTime(obj);
                return date.ToString("dd-MMM-yyyy HH:mm:ss");
            }
            return null;
        }
        public static string ToFormatedDateString(this DateTime obj, string separator)
        {
            return obj.ToString($"MM{separator}dd{separator}yyyy");
        }
        public static string ToFormatedDateString(this DateTime obj)
        {
            return obj.ToString("MM/dd/yyyy");
        }

        public static string ToFormatedDateTimeString(this DateTime obj)
        {
            return obj.ToString("MM/dd/yyyy HH:mm:ss");
        }
        public static string ToFormatedDateTimeWithAmPmString(this DateTime obj)
        {
            //return obj.ToString("MM/dd/yyyy HH:mm:ss tt");
            return obj.ToString("MM/dd/yyyy hh:mm:ss tt");
        }
        public static string ToFormatedDateTimeWithAmPmString(this DateTime? obj)
        {
            if (obj == null)
                return "";
            //return obj.ToString("MM/dd/yyyy HH:mm:ss tt");
            return obj.Value.ToString("MM/dd/yyyy hh:mm:ss tt");
        }
        /// <summary>
        /// Format : MM/dd/yyyy hh:mm tt
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToFormatedShortDateTimeString(this DateTime? obj)
        {
            if (obj == null)
                return "";
            //return obj.ToString("MM/dd/yyyy HH:mm:ss tt");
            return obj.Value.ToString("MM/dd/yyyy hh:mm tt");
        }
        /// <summary>
        /// Format : MM/dd/yyyy hh:mm tt
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToFormatedShortDateTimeString(this DateTime obj)
        {
            return obj.ToString("MM/dd/yyyy hh:mm tt");
        }
        public static string ToFormatedScgDateTimeWithAmPmString(this DateTime obj)
        {
            //return obj.ToString("MM/dd/yyyy HH:mm:ss tt");
            return obj.ToString("MMMM dd, yyyy hh:mm:ss tt");
        }
        public static string ToFormatedTimeInMinuteString(this DateTime obj)
        {
            return obj.ToString("HH:mm");
        }
        public static string ToFormatedTimeString(this DateTime obj)
        {
            return obj.ToString("HH:mm:ss");
        }
        public static string ToFormatedTimeWithAmPmString(this DateTime obj)
        {
            //return obj.ToString("HH:mm:ss tt");
            return obj.ToString("hh:mm:ss tt");
        }
        public static string ToFormatedShortTimeWithAmPmString(this DateTime obj)
        {
            //return obj.ToString("HH:mm:ss tt");
            return obj.ToString("hh:mm tt");
        }
        public static string ToFormatedDateTimeForFileString(this DateTime obj)
        {
            return obj.ToString("yyyy-MM-dd-HH-mm-ss");
        }
        public static string ToFormatedSub(this object obj)
        {
            if (obj != null)
                return ToFormatedSub(obj.ToString());
            return "";
        }
        public static string ToFormatedSub(this string obj)
        {
            if (obj != null)
            {
                List<string> str = obj.Split(' ').ToList<string>();
                if (str.Count > 7)
                {
                    string result = "";

                    for (int i = 0; i <= 7; i++)
                    {
                        result += str[i] + " ";
                    }
                    return result.TrimEnd(' ') + " ...";
                }
                else
                {
                    return obj;
                }
            }
            else
            {
                return "";
            }


        }
        public static string ToFormatedSub(this string obj, int count)
        {
            if (obj != null)
            {
                List<string> str = obj.Split(' ').ToList<string>();
                if (str.Count > count)
                {
                    string result = "";

                    for (int i = 0; i <= count; i++)
                    {
                        result += str[i] + " ";
                    }
                    return result.TrimEnd(' ') + " ...";
                }
                else
                {
                    return obj;
                }
            }
            else
            {
                return "";
            }
        }
        public static string ToTitleCase(this string mText)
        {
            if (mText == null) return "";
            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
            // TextInfo.ToTitleCase only operates on the string if is all lower case, otherwise it returns the string unchanged.
            return textInfo.ToTitleCase(mText.ToLower());
        }
        public static DateTime? ToFormatedDate(this string date)
        {
            if (string.IsNullOrEmpty(date))
                return null;
            var formatInfoinfo = new DateTimeFormatInfo();
            date = date.Replace("/", "-");
            List<string> lst = date.Split('-').ToList<string>();
            if (lst.Count == 3)
            {
                if (lst[1].Length != 3)
                {
                    lst[1] = formatInfoinfo.GetMonthName(lst[1].ToInt()).ToString();
                }
                return Convert.ToDateTime(lst.Aggregate((i, j) => i + "-" + j) + "  00:00:00.000");
            }
            else
                return null;
        }
        /// <summary>
        /// Input: MM/DD/YYYY output:DD/MMM/YYYY 00:00:00.000
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? ToFormatedDateFromStartWithMonth(this string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    return null;
                var formatInfoinfo = new DateTimeFormatInfo();
                date = date.Replace("/", "-");
                List<string> lst = date.Split('-').ToList<string>();
                string first = lst[1];
                lst[1] = lst[0];
                lst[0] = first;
                if (lst.Count == 3)
                {
                    if (lst[1].Length != 3)
                    {
                        lst[1] = formatInfoinfo.GetMonthName(lst[1].ToInt()).ToString();
                    }
                    return Convert.ToDateTime(lst.Aggregate((i, j) => i + "-" + j) + "  00:00:00.000");
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public static DateTime ConvertToUSACSTDateTime(this object obj)
        {
            if (obj != null)
            {
                var CSTDateTime = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(obj), TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
                return CSTDateTime;
            }
            else
            {
                return DateTime.UtcNow;
            }
        }

        public static DateTime? ConvertToUSACSTDateTime(this object obj, bool utsAsCst)
        {
            if (obj != null)
            {
                var CSTDateTime = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(obj), TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
                return CSTDateTime;
            }
            else
            {
                if (utsAsCst)
                {
                    return DateTime.UtcNow;
                }
            }

            return null;
        }

        /// <summary>
        /// Convert Date Format  Fri Jun 21 00:00:00 UTC+0100 2013 to 21/Fri/2013  </summary>
        /// <param name="date"> date in string</param>
        /// <param name="browser"></param>
        /// <seealso cref="String">
        ///  </seealso>
        public static string JSONToLocalDateString(this string date, string browser)
        {
            string output = null;
            if (!string.IsNullOrEmpty(date))
            {
                var inputSplit = date.Split(' ').ToList();
                output = string.Format("{0}/{1}/{2}", inputSplit[2], inputSplit[1], browser == "IE" ? inputSplit[5] : inputSplit[3]);
            }
            return output;
        }
        //public static string ToFormatedDateString(this string date)
        //{
        //    if (string.IsNullOrEmpty(date))
        //        return null;
        //    var formatInfoinfo = new DateTimeFormatInfo();
        //    date = date.Replace("/", "-");
        //    List<string> lst = date.Split('-').ToList<string>();
        //    if (lst.Count == 3)
        //    {
        //        if (lst[1].Length != 3)
        //        {
        //            lst[1] = formatInfoinfo.GetMonthName(lst[1].ToInt()).ToString();
        //        }
        //        return lst.Aggregate((i, j) => i + "-" + j).ToString();
        //    }
        //    else
        //        return null;
        //}

        public static string ToShortMonthName(this int monthValue)
        {
            var output = CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(monthValue);
            return output;
        }
        public static string ToMonthText(this int dateValue)
        {
            string[] monthDB = new string[] { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            string output = "";
            if (dateValue > 0 && dateValue < 13)
            {
                output = monthDB[dateValue];
            }
            return output;
        }
        public static string ToMonthAndYearText(this string date, string sep)
        {
            string[] monthDB = new string[] { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            string output = "";
            if (date != "")
            {
                DateTime? dt = Convert.ToDateTime(date);

                if (dt != null)
                {
                    int month = dt.Value.Month;
                    output = monthDB[month] + sep + " " + dt.Value.Year;
                }

            }
            return output;
        }

        public static string ToHexString(this string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += string.Format("{0:x2}", System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }
        public static string ToStringFromHex(this string hexValue)
        {
            string strValue = "";
            while (hexValue.Length > 0)
            {
                strValue += System.Convert.ToChar(System.Convert.ToUInt32(hexValue.Substring(0, 2), 16)).ToString();
                hexValue = hexValue.Substring(2, hexValue.Length - 2);
            }
            return strValue;
        }

        public static string ToConvertBase64Image(this IFormFile LogoImage)
        {
            if (LogoImage != null && LogoImage.Length > 0 && AppConstants.AllowedUploadedFileTypes.Where(m => m.Contains(LogoImage.ContentType)).Any())
            {
                var memoryStream = new MemoryStream();
                LogoImage.CopyTo(memoryStream);
                byte[] filebyte = memoryStream.ToArray();
                string imageconverted = AppConstants.Base64ImagePrefix + Convert.ToBase64String(filebyte);
                return imageconverted;
            }
            return null;
        }
        public static string GetDefaultImageIfNull(this string logoImage)
        {
            return logoImage != null ? logoImage : AppConstants.Base64DefaultImage;
        }


    }

    public static class Utilities
    {
        private static Random random = new Random();
        public static List<int> GenerateRandom(int count, int min, int max)
        {

            //  initialize set S to empty
            //  for J := N-M + 1 to N do
            //    T := RandInt(1, J)
            //    if T is not in S then
            //      insert T in S
            //    else
            //      insert J in S
            //
            // adapted for C# which does not have an inclusive Next(..)
            // and to make it from configurable range not just 1.

            if (max <= min || count < 0 ||
                    // max - min > 0 required to avoid overflow
                    (count > max - min && max - min > 0))
            {
                // need to use 64-bit to support big ranges (negative min, positive max)
                throw new ArgumentOutOfRangeException("Range " + min + " to " + max +
                        " (" + ((Int64)max - (Int64)min) + " values), or count " + count + " is illegal");
            }

            // generate count random values.
            HashSet<int> candidates = new HashSet<int>();

            // start count values before max, and end at max
            for (int top = max - count; top < max; top++)
            {
                // May strike a duplicate.
                // Need to add +1 to make inclusive generator
                // +1 is safe even for MaxVal max value because top < max
                if (!candidates.Add(random.Next(min, top + 1)))
                {
                    // collision, add inclusive max.
                    // which could not possibly have been added before.
                    candidates.Add(top);
                }
            }

            // load them in to a list, to sort
            List<int> result = candidates.ToList();

            // shuffle the results because HashSet has messed
            // with the order, and the algorithm does not produce
            // random-ordered results (e.g. max-1 will never be the first value)
            for (int i = result.Count - 1; i > 0; i--)
            {
                int k = random.Next(i + 1);
                int tmp = result[k];
                result[k] = result[i];
                result[i] = tmp;
            }
            return result;
        }
    }
}
