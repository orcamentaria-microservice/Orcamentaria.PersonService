using PersonService.BuildingBlocks.Enums;

namespace PersonService.BuildingBlocks.Reponses
{
    public class ResponseError
    {
        public ResponseErrorEnum ErrorCode { get; set; }
        public string ErrorName { get; set; }
        public IEnumerable<ResponseMessage> MessageErrors { get; set; }

        public ResponseError(ResponseErrorEnum errorType) 
        {
            this.ErrorCode = errorType;
            this.ErrorName = errorType.ToString();
        }

        public ResponseError(ResponseErrorEnum errorType, IEnumerable<ResponseMessage> messageErrors)
        {
            this.ErrorCode = errorType;
            this.ErrorName = errorType.ToString();
            this.MessageErrors = messageErrors;
        }

        public ResponseError(ResponseErrorEnum errorType, string message)
        {
            this.ErrorCode = errorType;
            this.ErrorName = errorType.ToString();
            this.MessageErrors = new List<ResponseMessage>() { new ResponseMessage(message) };
        }

        public ResponseError(ResponseErrorEnum errorType, string[] messages)
        {
            this.ErrorCode = errorType;
            this.ErrorName = errorType.ToString();
            this.MessageErrors = messages.Select((message, index) => new ResponseMessage(index, message));
        }
    }
}
