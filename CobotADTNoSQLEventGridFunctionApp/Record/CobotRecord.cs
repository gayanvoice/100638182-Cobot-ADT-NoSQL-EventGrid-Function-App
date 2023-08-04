using System;

namespace CobotADTNoSQLEventGridFunctionApp.Record
{
    public record CobotRecord(
         string id,
         string deviceId,
         string timestamp,
         double elapsedTime
    );
}
