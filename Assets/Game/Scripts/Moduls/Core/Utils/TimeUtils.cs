using System;

namespace Company.Module.Utils
{
    public static class TimeUtils
    {
        public static string GetTimeFormat( int t ) => GetTimeFormat( TimeSpan.FromSeconds( t ) );

        public static string GetTimerFormatHMS( TimeSpan timeSpan )
        {
            var output = "";
            if ( timeSpan.Hours > 0 )
            {
                output += timeSpan.Hours == 0 ? "" : timeSpan.Hours < 10 ? $"0{timeSpan.Hours}:" : $"{timeSpan.Hours}:";
            }
            output += $"{( timeSpan.Minutes < 10 ? $"0{timeSpan.Minutes}" : timeSpan.Minutes )}:{( timeSpan.Seconds < 10 ? $"0{timeSpan.Seconds}" : timeSpan.Seconds )}";
            return output;
        }
        
        public static string GetTimeFormat( TimeSpan timeSpan )
        {
            var output = "";
            if ( timeSpan.Days > 0 )
                output = $"Days {timeSpan.Days} - ";
            output += timeSpan.Hours == 0 ? "" : timeSpan.Hours < 10 ? $"0{timeSpan.Hours}:" : $"{timeSpan.Hours}:";
            output += $"{( timeSpan.Minutes < 10 ? $"0{timeSpan.Minutes}" : timeSpan.Minutes )}:{( timeSpan.Seconds < 10 ? $"0{timeSpan.Seconds}" : timeSpan.Seconds )}";
            return output;
        }

        public static string GetTimerFormat( TimeSpan timeSpan ) =>
            $"{( timeSpan.Minutes < 10 ? $"0{timeSpan.Minutes}" : timeSpan.Minutes )}:{( timeSpan.Seconds < 10 ? $"0{timeSpan.Seconds}" : timeSpan.Seconds )}";

        public static string GetTimerFormatWithText( string text, TimeSpan timeSpan ) =>
            string.Format( text, timeSpan.Minutes < 10 ? $"0{timeSpan.Minutes}" : timeSpan.Minutes,
                timeSpan.Seconds < 10 ? $"0{timeSpan.Seconds}" : timeSpan.Seconds );
        
        public static bool IsSameDay( DateTime date0, DateTime date1 )
        {
            if ( date0.DayOfYear != date1.DayOfYear || date0.Year != date1.Year )
                return false;
            return true;
        }
    }
}