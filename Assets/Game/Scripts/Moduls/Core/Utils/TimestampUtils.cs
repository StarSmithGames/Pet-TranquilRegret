using System;

namespace Company.Module.Utils
{
    public class TimestampUtils
    {
        public static int DeltaTime = 0; //for cheats

        
        public static DateTime Epoch = new( 1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc );
        
        public static DateTime GetDateTimeLocal( int timestampLocal ) => Epoch.AddSeconds( Convert.ToDouble( timestampLocal ) );
        public static DateTime GetDateTimeLocal( double timestampLocal ) => Epoch.AddSeconds( timestampLocal );
        
        public static DateTime GetDateTimeUTC( int timestamp ) => Epoch.AddSeconds( Convert.ToDouble( timestamp ) ).ToUniversalTime();
        public static DateTime GetDateTimeUTC( double timestamp ) => Epoch.AddSeconds( timestamp ).ToUniversalTime();

        public static int GetTimestamp( DateTime date ) => Convert.ToInt32( ( date - Epoch ).TotalSeconds ) + DeltaTime;
        // return Convert.ToInt32( ( date - Epoch ).TotalMilliseconds ) + DeltaTime * 1000;
    }
}