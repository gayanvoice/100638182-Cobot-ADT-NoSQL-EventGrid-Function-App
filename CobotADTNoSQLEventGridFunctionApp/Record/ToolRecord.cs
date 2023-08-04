namespace CobotADTNoSQLEventGridFunctionApp.Record
{
    public record ToolRecord(
         string id,
         string deviceId,
         string timestamp,
         double temperature,
         double voltage,
         double x,
         double y,
         double z,
         double rx,
         double ry,
         double rz
    );
}
