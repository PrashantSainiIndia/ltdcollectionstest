using CommonComponents;
using LtdDomain.Models.UserProfile;
using LtdService.Services.UserProfileServices;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LtdInfrastructure.DataAccess.Repositories.UserProfile
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private string _connectionString;

        enum TypeOfExistenceCheck
        {
            DoesExistsInDB,
            DoesNotExistInDB
        }

        enum RequestType
        {
            Add,
            Update,
            Read,
            Delete,
            ConfirmAdd,
            ConfirmDelete
        }

        public UserProfileRepository()
        { }

        public UserProfileRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<UserProfileModel> GetAll()
        {
            List<UserProfileModel> userProfileModelList = new List<UserProfileModel>();
            DataAccessStatus dataAccessStatus = new DataAccessStatus();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
            {
                try
                {
                    string sql = "SELECT * from UserProfile";
                    connection.Open();
                    using (OdbcCommand cmd = new OdbcCommand(sql, connection))
                    {
                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserProfileModel userProfileModel = new UserProfileModel();
                                userProfileModel.UserId = reader["UserId"].ToString();
                                userProfileModel.Password = reader["Password"].ToString();

                                userProfileModelList.Add(userProfileModel);
                            }
                        }
                        connection.Close();
                    }
                }
                catch (OdbcException e)
                {
                    dataAccessStatus.SetValues(status: "Error", operationSucceeded: false, exceptionMessage: e.Message, customMessage: "Unable to get User Profile Model list from database", helpLink: e.HelpLink, errorCode: e.ErrorCode, stackTrace: e.StackTrace);

                    throw new DataAccessException(e.Message, e.InnerException, dataAccessStatus);
                }
                return userProfileModelList;
            }
        }
        public UserProfileModel GetById(int id)
        {
            UserProfileModel userProfileModel = new UserProfileModel();
            DataAccessStatus dataAccessStatus = new DataAccessStatus();

            bool MatchingRecordFound = false;

            string sql = "SELECT UserId, Password" +
                         "FROM UserProfile WHERE UserId = @UserId";

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (OdbcCommand cmd = new OdbcCommand(sql, connection))
                    {
                        cmd.CommandText = sql;
                        cmd.Prepare();
                        cmd.Parameters.Add(new OdbcParameter("@UserId", id));

                        using (OdbcDataReader reader = cmd.ExecuteReader())
                        {
                            MatchingRecordFound = reader.HasRows;
                            while (reader.Read())
                            {
                                userProfileModel.UserId = reader["UserId"].ToString();
                                userProfileModel.Password = reader["Password"].ToString();
                            }
                        }
                        connection.Close();
                    }
                }
                catch (OdbcException e)
                {
                    dataAccessStatus.SetValues(status: "Error", operationSucceeded: false, exceptionMessage: e.Message, customMessage: "Unable to get User Profile Model for requested ID", helpLink: e.HelpLink, errorCode: e.ErrorCode, stackTrace: e.StackTrace);

                    throw new DataAccessException(e.Message, e.InnerException, dataAccessStatus);
                }

                if (!MatchingRecordFound)
                {
                    dataAccessStatus.SetValues(status: "Error", operationSucceeded: false, exceptionMessage: "", customMessage: $"Record not found. Unable to get User Profile Model for requested ID {id}. Id {id} does not exist in the database", helpLink: "", errorCode: 0, stackTrace: "");

                    throw new DataAccessException(dataAccessStatus);
                }
                return userProfileModel;
            }
        }
        public void Add(IUserProfileModel userProfileModel)
        {
            DataAccessStatus dataAccessStatus = new DataAccessStatus();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (OdbcException e)
                {
                    dataAccessStatus.SetValues(status: "Error", operationSucceeded: false, exceptionMessage: e.Message, customMessage: "Unable to add User Profile. Could not open database connection", helpLink: e.HelpLink, errorCode: e.ErrorCode, stackTrace: e.StackTrace);

                    throw new DataAccessException(e.Message, e.InnerException, dataAccessStatus);
                }
                string sql = "INSERT INTO UserProfile(UserId, Password) " +
                         "VALUES(@UserId, @Password)";

                using(OdbcCommand cmd = new OdbcCommand(sql, connection))
                {
                    try
                    {
                        RecordExistsCheck(cmd, userProfileModel, TypeOfExistenceCheck.DoesNotExistInDB, RequestType.Add);
                    }
                    catch(DataAccessException ex)
                    {
                        ex.DataAccessStatusInfo.CustomMessage = "User profile could not be added because it is already in the database.";
                        ex.DataAccessStatusInfo.ExceptionMessage = string.Copy(ex.Message);
                        ex.DataAccessStatusInfo.StackTrace = string.Copy(ex.StackTrace);
                        throw ex;
                    }

                    cmd.CommandText = sql;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@UserId", userProfileModel.UserId);
                    cmd.Parameters.AddWithValue("@Password", userProfileModel.Password);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch(OdbcException e)
                    {
                        dataAccessStatus.SetValues(status: "Error", operationSucceeded: false, exceptionMessage: e.Message, customMessage: "Unable to add User Profile", helpLink: e.HelpLink, errorCode: e.ErrorCode, stackTrace: e.StackTrace);
                        throw new DataAccessException(e.Message, e.InnerException, dataAccessStatus);
                    }

                    try // Confirm the User Profile Model was added to the database
                    {
                        RecordExistsCheck(cmd, userProfileModel, TypeOfExistenceCheck.DoesExistsInDB, RequestType.ConfirmAdd);
                    }
                    catch (DataAccessException ex)
                    {
                        ex.DataAccessStatusInfo.Status = "Error";
                        ex.DataAccessStatusInfo.OperationSucceeded = false;
                        ex.DataAccessStatusInfo.CustomMessage = "Failed to find User Profile in database after add operation completed";
                        ex.DataAccessStatusInfo.ExceptionMessage = string.Copy(ex.Message);
                        ex.DataAccessStatusInfo.StackTrace = string.Copy(ex.StackTrace);
                        throw ex;
                    }
                    connection.Close();
                }

            }
        }
        public void Update(IUserProfileModel userProfileModel)
        {
            int result = -1;
            DataAccessStatus dataAccessStatus = new DataAccessStatus();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (OdbcException e)
                {
                    dataAccessStatus.SetValues(status: "Error", operationSucceeded: false, exceptionMessage: e.Message, customMessage: "Unable to update User Profile. Could not open database connection", helpLink: e.HelpLink, errorCode: e.ErrorCode, stackTrace: e.StackTrace);
                    throw new DataAccessException(e.Message, e.InnerException, dataAccessStatus);
                }
                string sql = "UPDATE UserProfile " +
                         "SET UserId = @UserId, " +
                         "SET Password = @Password, " +
                         "WHERE UserId = @UserId";

                using (OdbcCommand cmd = new OdbcCommand(sql, connection))
                {
                    try
                    {
                        RecordExistsCheck(cmd, userProfileModel, TypeOfExistenceCheck.DoesExistsInDB, RequestType.Update);
                    }
                    catch (DataAccessException ex)
                    {
                        ex.DataAccessStatusInfo.CustomMessage = "User profile could not be updated because it is not in the database.";
                        ex.DataAccessStatusInfo.ExceptionMessage = string.Copy(ex.Message);
                        ex.DataAccessStatusInfo.StackTrace = string.Copy(ex.StackTrace);
                        throw ex;
                    }

                    cmd.CommandText = sql;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@UserId", userProfileModel.UserId);
                    cmd.Parameters.AddWithValue("@Password", userProfileModel.Password);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        dataAccessStatus.SetValues(status: "Error", operationSucceeded: false, exceptionMessage: e.Message, customMessage: "Unable to update User Profile", helpLink: e.HelpLink, errorCode: e.ErrorCode, stackTrace: e.StackTrace);
                        throw new DataAccessException(e.Message, e.InnerException, dataAccessStatus);
                    }
                    connection.Close();
                }

            }
        }
        public void Delete(IUserProfileModel userProfileModel)
        {
            DataAccessStatus dataAccessStatus = new DataAccessStatus();

            using (OdbcConnection connection = new OdbcConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (OdbcException e)
                {
                    dataAccessStatus.SetValues(status: "Error", operationSucceeded: false, exceptionMessage: e.Message, customMessage: "Unable to delete User Profile. Could not open database connection", helpLink: e.HelpLink, errorCode: e.ErrorCode, stackTrace: e.StackTrace);
                    throw new DataAccessException(e.Message, e.InnerException, dataAccessStatus);
                }
                string sql = "DELETE from UserProfile where UserId=@UserId";

                using (OdbcCommand cmd = new OdbcCommand(sql, connection))
                {
                    try
                    {
                        RecordExistsCheck(cmd, userProfileModel, TypeOfExistenceCheck.DoesExistsInDB, RequestType.Update);
                    }
                    catch (DataAccessException ex)
                    {
                        ex.DataAccessStatusInfo.CustomMessage = "User profile could not be deleted because it is not in the database.";
                        ex.DataAccessStatusInfo.ExceptionMessage = string.Copy(ex.Message);
                        ex.DataAccessStatusInfo.StackTrace = string.Copy(ex.StackTrace);
                        throw ex;
                    }

                    cmd.CommandText = sql;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@UserId", userProfileModel.UserId);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OdbcException e)
                    {
                        dataAccessStatus.SetValues(status: "Error", operationSucceeded: false, exceptionMessage: e.Message, customMessage: "Unable to delete User Profile", helpLink: e.HelpLink, errorCode: e.ErrorCode, stackTrace: e.StackTrace);
                        throw new DataAccessException(e.Message, e.InnerException, dataAccessStatus);
                    }
                    try // Confirm the User Profile was deleted from the database
                    {
                        RecordExistsCheck(cmd, userProfileModel, TypeOfExistenceCheck.DoesNotExistInDB, RequestType.ConfirmDelete);
                    }
                    catch (DataAccessException ex)
                    {
                        ex.DataAccessStatusInfo.Status = "Error";
                        ex.DataAccessStatusInfo.OperationSucceeded = false;
                        ex.DataAccessStatusInfo.CustomMessage = "Failed to delete User Profile in database";
                        ex.DataAccessStatusInfo.ExceptionMessage = string.Copy(ex.Message);
                        ex.DataAccessStatusInfo.StackTrace = string.Copy(ex.StackTrace);
                        throw ex;
                    }
                    connection.Close();
                }

            }
        }
        private bool RecordExistsCheck(OdbcCommand cmd, IUserProfileModel userProfileModel, TypeOfExistenceCheck typeOfExistenceCheck, RequestType requestType)
        {
            Int32 countOfRecsFound = 0;
            bool RecordExistsCheckPassed = true;

            DataAccessStatus dataAccessStatus = new DataAccessStatus();

            cmd.Prepare();

            if ((requestType == RequestType.Add) || (requestType == RequestType.ConfirmAdd))
            {
                cmd.CommandText = "SELECT count(*) from UserProfile where UserId=@UserId";
                cmd.Parameters.AddWithValue("UserId", userProfileModel.UserId);
            }
            else if ((requestType == RequestType.Update) || (requestType == RequestType.ConfirmDelete) || (requestType == RequestType.Delete))
            {
                cmd.CommandText = "SELECT count(*) from UserProfile where UserId=@UserId";
                cmd.Parameters.AddWithValue("UserId", userProfileModel.UserId);
            }

            try
            {
                countOfRecsFound = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (OdbcException e)
            {
                string msg = e.Message;
                throw;
            }

            if ((typeOfExistenceCheck == TypeOfExistenceCheck.DoesNotExistInDB) && (countOfRecsFound > 0))
            {
                dataAccessStatus.Status = "Error";
                RecordExistsCheckPassed = false;

                throw new DataAccessException(dataAccessStatus);
            }
            else if ((typeOfExistenceCheck == TypeOfExistenceCheck.DoesExistsInDB) && (countOfRecsFound == 0))
            {
                dataAccessStatus.Status = "Error";
                RecordExistsCheckPassed = false;

                throw new DataAccessException(dataAccessStatus);
            }
            return RecordExistsCheckPassed;
        }
    }
}
