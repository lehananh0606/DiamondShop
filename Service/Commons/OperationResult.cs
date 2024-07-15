using System.Collections.Generic;

namespace Service.Commons
{
    public class OperationResult<T>
    {
        public StatusCode StatusCode { get; set; } = StatusCode.Ok;
        public string? Message { get; set; }
        public bool IsError { get; set; }
        public T? Payload { get; set; }
        public List<Error> Errors { get; set; } = new List<Error>();
        public object? MetaData { get; set; }

        public void AddError(StatusCode code, string field, string description)
        {
            HandleError((int)code, field, description);
        }
        
        public void AddResponseStatusCode(
            StatusCode code, 
            string message, 
            T? payload,
            object? metaData = null
            )
        {
            HandleResponse(code, message, payload, metaData);
        }
        
        public void AddUnknownError(string field, string description)
        {
            HandleError((int)StatusCode.UnknownError, field, description);
        }

        public void ResetIsErrorFlag()
        {
            IsError = false;
        }

        private void HandleResponse(StatusCode code, string message,
            T? payload, object? metaData)
        {
            StatusCode = code;
            IsError = false;
            Message = message;
            Payload = payload;
            MetaData ??= metaData;


        }

        private void HandleError(int code, string field, string description)
        {
            var error = Errors.Find(e => e.StatusCode == code);
            if (error == null)
            {
                error = new Error { StatusCode = code };
                Errors.Add(error);
            }
            var errorDetail = error.Message.Find(ed => ed.FieldNameError == field);
            if (errorDetail == null)
            {
                errorDetail = new ErrorDetail { FieldNameError = field };
                error.Message.Add(errorDetail);
            }
            errorDetail.DescriptionError.Add(description);
            IsError = true;
        }
        
        public void AddValidationError(string field, string description)
        {
            HandleError((int)StatusCode.UnknownError, field, description);
        }

        public static OperationResult<T> Success(T payload, StatusCode statusCode = StatusCode.Ok, string? message = null)
        {
            return new OperationResult<T>
            {
                StatusCode = statusCode,
                Message = message ?? "Success",
                IsError = false,
                Payload = payload
            };
        }

        public static OperationResult<T> Fail(StatusCode statusCode, string field, string description)
        {
            var result = new OperationResult<T>
            {
                StatusCode = statusCode,
                Message = description,
                IsError = true
            };
            result.Errors.Add(new Error
            {
                StatusCode = (int)statusCode,
                Message = new List<ErrorDetail>
                {
                    new ErrorDetail
                    {
                        FieldNameError = field,
                        DescriptionError = new List<string> { description }
                    }
                }
            });
            return result;
        }
    }

   
}
