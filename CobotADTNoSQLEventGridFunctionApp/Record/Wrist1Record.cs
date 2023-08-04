namespace CobotADTNoSQLEventGridFunctionApp.Record
{
    public record Wrist1Record(
         string id,
         string deviceId,
         string timestamp,
         double position,
         double temperature,
         double voltage
    );
}
