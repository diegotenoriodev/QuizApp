using System;
using System.Collections.Generic;

namespace QuizAPI.Model
{
    public abstract class BaseModel
    {
        /// <summary>
        /// Should contain the errors that happened during the process
        /// </summary>
        protected List<string> messages;

        /// <summary>
        /// Default constructor, initializes the entities and links the ConnectionString 
        /// static property from repository to the value in Settings.
        /// </summary>
        public BaseModel()
        {
            messages = new List<string>();
            Repository.Repository.ConnectionString = Settings.ConnectionString;
        }

        /// <summary>
        /// Saves the exception in a xml file.
        /// </summary>
        /// <param name="ex"></param>
        public virtual void LogException(Exception ex)
        {
            //TODO: Log exceptions
        }

        /// <summary>
        /// Returns the default error message.
        /// </summary>
        /// <returns></returns>
        public virtual string DefaultErrorMessage()
        {
            return "An error accurred, please try again later. If the problem continues get in touch with our support. Thank you!";
        }
    }

    /// <summary>
    /// Generic BaseModel that describes some basic operations, like Save, Delete and Get
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseModel<T> : BaseModel
    {
        public abstract ResultOperation Save(T entity);
        public abstract ResultOperation Delete(int id);
        public abstract T Get(int id);
    }
}
