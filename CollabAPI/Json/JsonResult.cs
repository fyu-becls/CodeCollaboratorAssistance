using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
 
namespace CollabAPI
{
    // Holds the deserialized result from a call to Collaborator Json API
    public class JsonResult
    {
        // Non specific, as we have a bunch of different response class definitions
        // Don't try to access this directly, unless you want raw JSON, instead use
        // the GetResponse method below to serialize it into the appropriate request type
        public object result = null;
        // Errors always look the same, use our JsonError class to hold them
        public List<JsonError> errors = null;

        // Handy method for determining if we've received an error response
        public Boolean IsError()
        {
            return (errors != null);
        }

        // Helper method to deserialize JsonResult objects, caller should check isError() first
        // T should be a *Response class from a service definition. Caller needs to trap exceptions.
        public T GetResponse<T>()
        {
            // Caller should have checked for errors, keyword: should
            if (IsError())
                return default(T); // aka null

            // Serialize the object to the requested type.
            // TODO: Better error handling? We just throw them up for now.
            return JsonConvert.DeserializeObject<T>(result.ToString());
        }

        //// Gets a string of errors returned by an API call, 1 per line.
        //public String GetErrorString()
        //{
        //    string ret = "";

        //    if (!isError())
        //        return ret; // No errors, return an empty string

        //    foreach(JsonError err in errors)
        //    {
        //        if (ret.Length > 0) // If this isn't the first error
        //            ret += "/r/n";  // then add a line break

        //        ret += err.message; // Append the error message
        //    }

        //    return ret;
        //}


        public string GetErrorString(bool isErrorCodeNeeded = true)
        {
            if (!this.IsError())
                return string.Empty;
            string str = string.Empty;
            foreach (JsonError error in this.errors)
            {
                if (str.Length > 0)
                    str += Environment.NewLine;
                if (isErrorCodeNeeded && !string.IsNullOrWhiteSpace(error.code))
                    str = str + error.code + " - ";
                str += error.message;
                if (!string.IsNullOrWhiteSpace(error.data))
                {
                    str += Environment.NewLine;
                    str += error.data;
                }
            }
            return str;
        }
    }

#pragma warning disable 0649 // Expected warnings in JSON classes
    // Holds an error code and friendly message returned from an API call, data is optional.
    public class JsonError
    {
        public string code;
        public string message;
        public string data;
    }
#pragma warning restore 0649  // Expected warnings in JSON classes

}
