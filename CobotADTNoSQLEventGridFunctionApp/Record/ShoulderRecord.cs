namespace CobotADTNoSQLEventGridFunctionApp.Record
{
    public record ShoulderRecord(
         string id,
         string deviceId,
         string timestamp,
         double position,
         double temperature,
         double voltage
    );
}
