using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.BuildingBlocks.Reponses
{
    public class ResponseMessage
    {
        public int Index { get; set; }
        public string Message { get; set; }

        public ResponseMessage(int index, string message)
        {
            this.Index = index;
            this.Message = message;
        }

        public ResponseMessage(string message)
        {
            Index = 0;
            Message = message;
        }
    }
}
