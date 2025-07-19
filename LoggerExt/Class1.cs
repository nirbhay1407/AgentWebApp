using Azure.Core;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.RegularExpressions;

namespace LoggerExt
{
   /* public class Class1
    {
        public HttpResponseMessage PostSubmitOrder([FromBody] CommonClasses.processCart requestObj)
        {
            try
            {
                string requestbody = JsonConvert.SerializeObject(requestObj);
                api_service_id = 81;
                CreateServiceLog(Request.RequestUri.LocalPath, requestbody);  //Send Request.RequestUri.ToString() for GET method

                if (!AuthorizeUser()) //Check token expiry, method permission(account level) and set DB varibale to objAccountSettings
                {
                    return responseMessage;
                }

                #region CheckBody
                string orderNo = null, accountNumber = null, emailAddress = null, type = null, acceptDocumentId = "", acceptDocumentTimestamp = "";
                if (requestObj == null)
                {
                    return BadRequestResponse(serviceLog);
                }

                if (string.IsNullOrEmpty(requestObj.orderNo))
                {
                    return BadRequestResponse(serviceLog, "Missing required field - orderNo");
                }
                else
                    orderNo = requestObj.orderNo.ToUpper();

                if (string.IsNullOrEmpty(requestObj.accountNumber))
                {
                    return BadRequestResponse(serviceLog, "Missing required field - accountNumber");
                }
                else
                {
                    accountNumber = requestObj.accountNumber;
                    ban = accountNumber;
                    if (ban != "" && !is_ban_9_digit("Invalid accountNumber"))
                    {
                        return responseMessage;
                    }
                }


                if (string.IsNullOrEmpty(requestObj.emailAddress))
                {
                    return BadRequestResponse(serviceLog, "Missing required field - emailAddress");
                }
                else
                {
                    emailAddress = requestObj.emailAddress;

                    if (!Regex.Match(emailAddress, "^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$").Success)
                    {
                        return BadRequestResponse(serviceLog, "Invalid emailAddress.");
                    }
                }
                if (string.IsNullOrEmpty(requestObj.type))
                {
                    return BadRequestResponse(serviceLog, "Missing required field - type");
                }
                else
                {
                    type = requestObj.type.ToUpper();

                    if (!(type == "SUBMIT" || type == "CANCEL"))
                    {
                        return BadRequestResponse(serviceLog, "type must be SUBMIT or CANCEL");
                    }
                }

                if (!string.IsNullOrEmpty(requestObj.acceptDocumentId))
                {
                    acceptDocumentId = requestObj.acceptDocumentId;
                }

                if (!string.IsNullOrEmpty(requestObj.acceptDocumentTimestamp))
                {
                    acceptDocumentTimestamp = requestObj.acceptDocumentTimestamp;
                }
                banNotWhiteListErrorMsg = "accountNumber is not register";
                if (!Request_Validate())
                {
                    return responseMessage;
                }

                #endregion
                CartProcess cartProcess = new CartProcess();
                int status_id = 0;
                int cart_id = cartProcess.get_cart_id(objAccountSettings.account_id, accountNumber, orderNo, emailAddress, ref status_id);
                if (cart_id == 0)
                {
                    return BadRequestResponse(serviceLog);
                }

                string serviceStatusCode = cartProcess.validate_submit_order(cart_id, type, acceptDocumentId, acceptDocumentTimestamp, objAccountSettings.user_id, serviceLog.request_log_id); //CANCEL update also done here with SUCCESS return

                string responseString = "";
                if (serviceStatusCode == "")
                {
                    CommonClasses.ServiceStatus respServiceStatus = new CommonClasses.ServiceStatus();
                    serviceStatusCode = cartProcess.authAndSubmitOrder(cart_id, orderNo, accountNumber, acceptDocumentId, acceptDocumentTimestamp, serviceLog, objAccountSettings);
                    respServiceStatus.code = serviceStatusCode;
                    if (serviceStatusCode == "100")
                    {
                        respServiceStatus.description = "Order submited successfully";
                    }
                    else
                    {
                        respServiceStatus.description = "Error occured during process your order";
                    }

                    responseString = Serializer.JSONSerializeObject(respServiceStatus);
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, respServiceStatus);

                }
                else if (serviceStatusCode == "SUCCESS")
                {
                    CommonClasses.ServiceStatus respServiceStatus = new CommonClasses.ServiceStatus() { code = "100", description = "Order cancelled successfully" };
                    responseString = Serializer.JSONSerializeObject(respServiceStatus);
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, respServiceStatus);
                }
                else
                {
                    CommonClasses.ServiceStatus respServiceStatus = new CommonClasses.ServiceStatus() { code = "101", description = serviceStatusCode };
                    responseString = Serializer.JSONSerializeObject(respServiceStatus);
                    responseMessage = Request.CreateResponse(HttpStatusCode.OK, respServiceStatus);
                }

                responseMessage.Headers.Add("unique-id", serviceLog.unique_Id);
                API_insert_response_log(responseString);
                return responseMessage;

            }
            catch (Exception ex)
            {
                ServiceStatus.code = "M500";
                ServiceStatus.description = "Internal Server Error";
                responseMessage = Request.CreateResponse(HttpStatusCode.InternalServerError, ServiceStatus);
                responseMessage.Headers.Add("unique-id", serviceLog.unique_Id);
                API_insert_response_log(Serializer.JSONSerializeObject(ServiceStatus));

                Mailer.SendMail(ex);
                Logger.LogException(ex);

                return responseMessage;
            }
        }

        public string validate_submit_order(int cart_id, string type, string acceptDocumentId, string acceptDocumentTimestamp, int user_id, int request_log_id)
        {
            string ret_msg = "";
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TMTunnel"].ToString());
                SqlCommand com = new SqlCommand("sc_validate_submit_order", con);
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 0;
                com.Parameters.AddWithValue("@cart_id", cart_id);
                com.Parameters.AddWithValue("@type", type);
                com.Parameters.AddWithValue("@acceptDocumentId", acceptDocumentId);
                com.Parameters.AddWithValue("@acceptDocumentTimestamp", acceptDocumentTimestamp);
                com.Parameters.AddWithValue("@row_update_user_id", user_id);
                com.Parameters.AddWithValue("@request_log_id", request_log_id);

                com.Parameters.Add("@ret_msg", SqlDbType.VarChar, 50);
                com.Parameters["@ret_msg"].Direction = ParameterDirection.Output;

                con.Open();
                com.ExecuteNonQuery();

                ret_msg = Convert.ToString(com.Parameters["@ret_msg"].Value);

                con.Close();

            }
            catch (Exception ex)
            {
                Helpers.Mailer.SendMail(ex);
                Helpers.Logger.LogException(ex);
            }

            return ret_msg;
        }


    }*/
}
