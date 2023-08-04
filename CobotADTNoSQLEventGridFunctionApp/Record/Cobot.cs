using System;

namespace CobotADTNoSQLEventGridFunctionApp.Record
{
    public record Cobot(
        string id,
        string timestamp,
        double elapsedTime
    );
}
