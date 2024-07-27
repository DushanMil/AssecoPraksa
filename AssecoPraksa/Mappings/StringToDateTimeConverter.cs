using System.Globalization;
using System;
using AutoMapper;

namespace AssecoPraksa.Mappings
{
    public class StringToDateTimeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            // recimo da mi nije bitno sto vraca default
            // jer cu ionako vrsiti proveru formata datuma u servisu
            if (source == null)
            {
                return default(DateTime);
            }

            string[] formats = { "M/d/yyyy", "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy" };

            if (DateTime.TryParseExact(source, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out destination))
            {
                DateTime utcDateTime;
                if (destination.Kind == DateTimeKind.Unspecified)
                {
                    utcDateTime = DateTime.SpecifyKind(destination, DateTimeKind.Utc);
                }
                else
                {
                    utcDateTime = destination.ToUniversalTime();
                }

                // Console.WriteLine(utcDateTime);

                return utcDateTime;
            }
            

            return default(DateTime);
        }
    }
}
