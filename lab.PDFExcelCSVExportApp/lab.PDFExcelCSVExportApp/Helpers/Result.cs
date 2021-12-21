namespace lab.PDFExcelCSVExportApp.Helpers
{
    public class Result
    {
        public bool Success { get; }

        public string Message { get; }

        public string MessageType { get; }

        public int ParentId { get; }

        public string ParentName { get; }

        public string Data { get; }

        public Result()
        {
        }

        private Result(bool success, string message, string messageType)
        {
            Success = success;
            Message = message;
            MessageType = messageType;
        }

        private Result(bool success, string message, string messageType, string data)
        {
            Success = success;
            Message = message;
            MessageType = messageType;
            Data = data;
        }

        private Result(bool success, string message, string messageType, int parentId, string parentName)
        {
            Success = success;
            Message = message;
            MessageType = messageType;
            ParentId = parentId;
            ParentName = parentName;
        }

        public static Result Info()
        {
            return new Result(false, MessageHelper.Info, MessageHelper.MessageTypeInfo);
        }

        public static Result Info(string message)
        {
            return new Result(false, message, MessageHelper.MessageTypeInfo);
        }

        public static Result Warning()
        {
            return new Result(false, MessageHelper.Warning, MessageHelper.MessageTypeWarning);
        }

        public static Result Warning(string message)
        {
            return new Result(false, message, MessageHelper.MessageTypeWarning);
        }

        public static Result Fail()
        {
            return new Result(false, MessageHelper.Error, MessageHelper.MessageTypeDanger);
        }

        public static Result Fail(string message)
        {
            return new Result(false, message, MessageHelper.MessageTypeDanger);
        }

        public static Result Ok()
        {
            return new Result(true, MessageHelper.Success, MessageHelper.MessageTypeSuccess);
        }

        public static Result Ok(string message)
        {
            return new Result(true, message, MessageHelper.MessageTypeSuccess);
        }

        public static Result Ok(string message, int parentId, string parentName)
        {
            return new Result(true, message, MessageHelper.MessageTypeSuccess, parentId, parentName);
        }
        
        public static Result Ok(string message, string data)
        {
            return new Result(true, message, MessageHelper.MessageTypeSuccess, data);
        }
    }
}
