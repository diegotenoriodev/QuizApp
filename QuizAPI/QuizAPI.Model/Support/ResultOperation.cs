using System.Collections.Generic;
using System.Linq;

namespace QuizAPI.Model
{
    /// <summary>
    /// This class represents the handled result of an operation.
    /// It is returned to the layers above in order to give a common interface for exposing the 
    /// results of an operation.
    /// This entity does not belong to the model layer, but to the support layer.
    /// </summary>
    public class ResultOperation
    {
        /// <summary>
        /// True if the operation was completed successifuly
        /// False if there is any error
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// List of errors returned by the entity.
        /// Usually it returns one error, however there are cases where multiple evaluations
        /// must be done, and there is a possibility of multiple messages being returned.
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Default constructor, initializes the entities.
        /// </summary>
        public ResultOperation()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// Receives a boolean, depending on its value, it initializes the list of error or not.
        /// </summary>
        /// <param name="success"></param>
        public ResultOperation(bool success)
        {
            Success = success;

            if (!Success)
            {
                Errors = new List<string>();
            }
        }

        /// <summary>
        /// Receives only one error as parameter. Initializes the success as false
        /// and the list of errors with the given error.
        /// </summary>
        /// <param name="error"></param>
        public ResultOperation(string error)
        {
            Success = false;
            Errors = new List<string>() { error };
        }

        /// <summary>
        /// Overloads the operatior to cast a boolean into a resultOperation
        /// </summary>
        /// <param name="success"></param>
        public static implicit operator ResultOperation(bool success)
        {
            return new ResultOperation(success);
        }

        /// <summary>
        /// Returns a result operation with error and success = false;
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator ResultOperation(string message)
        {
            return new ResultOperation(message);
        }

        /// <summary>
        /// Returns a result operation based on a list of strings.
        /// Success = false;
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator ResultOperation(List<string> message)
        {
            return new ResultOperation(!message.Any()) { Errors = message };
        }
    }

    /// <summary>
    /// This class inherits from ResultOperation and adds a property where the object 
    /// will in question can be added.
    /// </summary>
    public class ResultOperationWithObject : ResultOperation
    {
        /// <summary>
        /// Object that is been operated
        /// </summary>
        public object Object { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ResultOperationWithObject()
        {
        }

        /// <summary>
        /// Constructor with a result operation and an object.
        /// Just matches the properties.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="obj"></param>
        public ResultOperationWithObject(Model.ResultOperation result, object obj)
        {
            this.Object = obj;
            this.Success = result.Success;
            this.Errors = result.Errors;
        }

        /// <summary>
        /// Result operation for a single error.
        /// </summary>
        /// <param name="error"></param>
        public ResultOperationWithObject(string error)
        {
            this.Object = null;
            this.Success = false;
            this.Errors = new System.Collections.Generic.List<string>() { error };
        }

        /// <summary>
        /// Result operation for a list of errors and an object to be returned.
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="obj"></param>
        public ResultOperationWithObject(List<string> errors, object obj)
        {
            this.Object = obj;
            this.Success = false;
            this.Errors = errors;
        }

        /// <summary>
        /// Result operation that was successiful and is returning the changed object.
        /// </summary>
        /// <param name="obj"></param>
        public ResultOperationWithObject(object obj)
        {
            this.Object = obj;
            this.Success = true;
            this.Errors = null;
        }
    }
}
