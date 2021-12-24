//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;
//using DnsClient;

//namespace Genso.Framework
//{

//    /// <summary>
//    /// A set of statitc functions to send & receive data 
//    /// Class to handle all communications between Client, API & Server
//    /// </summary>
//    public static class Transfer
//    {
//        //IP of DNS server to query for domain resolution
//        const string DnsQueryAddress = "8.8.8.8";
//        const int DnsQueryPort = 53;



//        /** PUBLIC METHODS **/

//        /// <summary>
//        /// Transfer data from client to API server & returns the reply data.
//        /// </summary>
//        public static async Task<TransferData> sendHttpData(string receiverAddress, TransferData data)
//        {
//            //send data to server & get reply
//            var rawReply = await _sendHttpData(receiverAddress, data);

//            //process reply from server (extracting needed data)
//            var reply = new TransferData();
//            reply.addData(rawReply);

//            //return processed reply to caller
//            return reply;
//        }

//        /// <summary>
//        /// Example use is API to Server
//        /// </summary>
//        public static bool sendTcpData(string receiverAddress, int receiverPort, TransferData request)
//        {
//            //resolve domain name to a list of IP addresses
//            var ipAddressList = getIpAddressList(receiverAddress);

//            //default send result is false
//            bool sendResult = false;

//            //format data for DNS server (string to byte array)
//            Byte[] dataByte = Encoding.ASCII.GetBytes(request.getDataAsString());

//            //try to send data to each ip until success
//            foreach (var address in ipAddressList)
//            {
//                //send data to server
//                sendResult = _sendTcpData(address, receiverPort, dataByte);

//                //if sending was successful, no need to use other IPs
//                if (sendResult) { break; }
//            }

//            //return sending result to caller
//            return sendResult;
//        }

//        /// <summary>
//        /// Receive TCP data, example use is in DNS server waiting for updates from API
//        /// </summary>
//        /// <param name="listenPort"></param>
//        /// <returns></returns>
//        public static TransferData receiveTcpData(int listenPort)
//        {
//            var MaxIncoming = 1024; //1kB todo remove size limit

//            //sender can be from any IP address
//            IPAddress senderIp = IPAddress.Any;

//            //create a listener for data from sender
//            TcpListener server = new TcpListener(senderIp, listenPort);

//            //set received data as empty first
//            String receivedData = null;

//        ListenForData:

//            try
//            {
//                //start the listener
//                server.Start();

//                //create empty bytes holder for receiving data
//                Byte[] bytes = new Byte[MaxIncoming];

//                // Perform a blocking call to accept requests.
//                TcpClient client = server.AcceptTcpClient();

//                // Get a stream object for reading and writing
//                NetworkStream stream = client.GetStream();

//                //receive all the data sent by the client
//                int i = stream.Read(bytes, 0, bytes.Length);

//                //translate data bytes to a ASCII string.
//                receivedData = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

//                //close connection
//                client.Close();

//            }
//            catch (Exception e)
//            {
//                //if error, show message to user
//                Console.WriteLine("Error when receiving data:\n {0}", e);
//                //go back to listeing for data
//                goto ListenForData;
//            }
//            finally
//            {
//                //stop listening for new commands.
//                server.Stop();
//            }

//            //try to parse the received data
//            var parsedData = TransferData.fromXml(receivedData);

//            //if data could NOT be read (parsed), then go back to listeing for data
//            if (parsedData == null) { goto ListenForData; }

//            //return parsed data to caller
//            return parsedData;
//        }


//        /** PRIVATE METHODS **/

//        /// <summary>
//        /// Sends data via HTTP, and returns full response
//        /// If no internet HttpRequestException is raised
//        /// </summary>
//        private static async Task<HttpResponseMessage> _sendHttpData(string receiverAddress, TransferData payload)
//        {
//            //prepare the data to be sent
//            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, receiverAddress);
//            httpRequestMessage.Content = payload.toStringContent();

//            //get the data sender
//            using var client = new HttpClient();

//            //tell sender to wait for complete reply before exiting
//            var waitForContent = HttpCompletionOption.ResponseContentRead;

//            //send the data on its way
//            var response = await client.SendAsync(httpRequestMessage, waitForContent);

//            //return the raw reply to caller
//            return response;
//        }

//        /// <summary>
//        /// Sends data via TCP, return success/fail of sending
//        /// </summary>
//        private static bool _sendTcpData(string receiverIp, int receiverPort, byte[] data)
//        {
//            //todo might need using for better performance & no closing call
//            TcpClient client = null;
//            NetworkStream stream = null;

//            try
//            {
//                //make connection to receiver
//                client = new TcpClient(receiverIp, receiverPort);

//                //get a path to send data
//                stream = client.GetStream();

//                //send the data to the receiver
//                stream.Write(data, 0, data.Length);

//                //close everything.
//                stream.Close();
//                client.Close();

//                //let user know data has been sent
//                Console.WriteLine("Data sent to server");

//                //tell caller sending succeeded
//                return true;

//                // Buffer to store the response bytes.
//                //byteData = new Byte[256];

//                // Read the first batch of the TcpServer response bytes.
//                //Int32 bytes = stream.Read(byteData, 0, byteData.Length);

//                //save the response data
//                //responseData = System.Text.Encoding.ASCII.GetString(byteData, 0, bytes);
//            }
//            catch (SocketException)
//            {
//                //let user know server did not respond
//                Console.WriteLine("No response from server, try again later.");

//                //tell caller sending failed
//                return false;
//            }
//            //if unexpected failure
//            catch (Exception e)
//            {
//                //show error message to user
//                Console.WriteLine("Error when sending data:\n {0}", e);

//                //tell caller sending failed
//                return false;
//            }

//        }

//        /// <summary>
//        /// Gets a list of IP addresses pointing to a domain by querying a specified DNS
//        /// </summary>
//        private static List<string> getIpAddressList(string domain)
//        {
//            //specify DNS server
//            var returnList = new List<string>();

//            try
//            {
//                var endpoint = new IPEndPoint(IPAddress.Parse(DnsQueryAddress), DnsQueryPort);
//                var client = new LookupClient(endpoint);
//                var aRecords = client.Query(domain, QueryType.A).Answers.ARecords();
//                foreach (var aRecord in aRecords) { returnList.Add(aRecord.Address.ToString()); }

//            }
//            //if getting of IP address from above method fails try another way
//            catch (Exception)
//            {
//                var ipAddressList = Dns.GetHostAddressesAsync(domain).Result;
//                foreach (var address in ipAddressList) { returnList.Add(address.ToString()); }
//            }

//            //return list of address to caller
//            return returnList;
//        }

//    }
//}