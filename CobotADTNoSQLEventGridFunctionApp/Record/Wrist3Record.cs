namespace CobotADTNoSQLEventGridFunctionApp.Record
{
    public record Wrist3Record(
         string id,
         string deviceId,
         string timestamp,
         double position,
         double temperature,
         double voltage
    );
}
