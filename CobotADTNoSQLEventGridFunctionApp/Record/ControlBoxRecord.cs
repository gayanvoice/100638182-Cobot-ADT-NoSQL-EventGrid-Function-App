namespace CobotADTNoSQLEventGridFunctionApp.Record
{
    public record ControlBoxRecord(
         string id,
         string deviceId,
         string timestamp,
         double voltage
    );
}
