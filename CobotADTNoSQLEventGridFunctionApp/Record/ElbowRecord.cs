namespace CobotADTNoSQLEventGridFunctionApp.Record
{
    public record ElbowRecord(
         string id,
         string deviceId,
         string timestamp,
         double position,
         double temperature,
         double voltage,
         double x,
         double y,
         double z
    );
}
