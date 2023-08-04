using System;

namespace CobotADTNoSQLEventGridFunctionApp.Record
{
    public record PayloadRecord(
         string id,
         string deviceId,
         string timestamp,
         double mass,
         double cogx,
         double cogy,
         double cogz
    );
}
