namespace CobotADTNoSQLEventGridFunctionApp.Record
{
    public record Wrist2Record(
         string id,
         string deviceId,
         string timestamp,
         double position,
         double temperature,
         double voltage
    );
}
