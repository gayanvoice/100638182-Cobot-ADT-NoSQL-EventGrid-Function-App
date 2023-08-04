using System.Collections.Generic;

namespace CobotADTNoSQLEventGridFunctionApp.Model
{
    public class RootModel
    {
        public DataModel Data { get; set; }
        public class DataModel
        {
            public string ModelId { get; set; }
            public List<PatchModel> Patch { get; set; }
            public class PatchModel
            {
                public double Value { get; set; }
                public string Path { get; set; }
                public string Op { get; set; }
            }
        }
    }
}