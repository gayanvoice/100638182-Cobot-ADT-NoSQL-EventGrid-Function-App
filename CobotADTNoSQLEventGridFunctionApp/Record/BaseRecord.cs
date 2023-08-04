namespace CobotADTNoSQLEventGridFunctionApp.Record
{
    public record BaseRecord(
         string id,
         string deviceId,
         string timestamp,
         double position,
         double temperature,
         double voltage
    );
}
