namespace Intake.Common
{
    public static class MessagesDictionary
    {
        public static string ReturnExplanationOfError(int errorNumber)
        {
            return Header_prefix +" MSG_"+ errorNumber + " - " + Properties.ExceptionMessages.ResourceManager.GetString(errorNumber.ToString());
        }
        public const string Header_prefix = "X-INTAKEMAP-ERROR";
        private const int ERR_INTAKE_ERROR_PREFIX = 1000;


        //Serverless.Function
        public const int ERR_APIGatewayProxyRequestIsNull = ERR_INTAKE_ERROR_PREFIX + 1; //APIGatewayProxyRequest is NULL
        public const int ERR_RequestBodyIsNull = ERR_INTAKE_ERROR_PREFIX + 2; //Body of APIGatewayProxyRequest is NULL
        public const int ERR_RequestDeserializingFailed = ERR_INTAKE_ERROR_PREFIX + 3; //Deserialization of ConversionRequest has failed
        public const int ERR_ResponseSerializingFailed = ERR_INTAKE_ERROR_PREFIX + 4; //Serialization of Response has failed
        public const int ERR_UnknownFunctionHandlerError = ERR_INTAKE_ERROR_PREFIX + 5; //FunctionHandler has failed. Details: 

        //Serverless.Manager
        public const int ERR_ProcessResultFailed = ERR_INTAKE_ERROR_PREFIX + 6; //Processing Result Data has failed

        //Conversion.Engine
       
        public const int ERR_GatewayTimeOut = ERR_INTAKE_ERROR_PREFIX + 14; //lambda function time out
        public const int ERR_TokenValidationFailed = ERR_INTAKE_ERROR_PREFIX + 15; //Token validation failed
        public const string Info_INTAKE_TokenValidationFailed = "Token validation failed";
        public const string Info_INTAK_GatewayTimeOut = "Request Time-Out 29.9 seconds !";

     
    }
}
