//using System;
//using System.IO;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using Genso.DDNS.Syntax;
//using Path = Genso.DDNS.Syntax.Path;

//namespace Genso.Framework
//{

//    /// <summary>
//    /// Manages user account at Genso for DDNS service
//    /// The client side user account does not validate the user account,
//    /// all validation is done per request on server side,
//    /// here only the needed KEY1 is generated to be sent with every request
//    ///
//    /// Notes:
//    /// Class can instantiated by user 
//    /// </summary>
//    public class AccountManager
//    {
//        private string _username;
//        private string _password;
//        private string _key1;

//        //details used for encryption & decryption of login details in local storage
//        private const string DecrypterKey = "12345";
//        private const string Splitter = "<SPLITTER>";


//        /** PUBLIC METHODS **/

//        /// <summary>
//        /// Used when instantiating from GUI, returns null if user account doesn't exist
//        /// </summary>
//        public static async Task<AccountManager> fromLogin(string username, string password)
//        {
//            //check with API server if user account exists 

//            //prepare the request
//            var request = new TransferData();
//            var key1 = generateKey1(username, password);
//            request.addData(TransferNames.ClientToApi.Key1, key1); //for validation

//            //send request to server and get response
//            var response = await Transfer.sendHttpData(Path.Client.CheckAccountApi, request);

//            //if account exist, return the account
//            if (response.getChildData<string>(TransferNames.ApiToClient.Status) == "Pass")
//            {
//                return new AccountManager { _username = username, _password = password, _key1 = key1 };
//            }
//            //else account doesn't exist, return null
//            else
//            {
//                return null;
//            }

//        }

//        /// <summary>
//        /// Makes an instance from encrypted username & password
//        /// Used when instantiating from config XML files
//        /// </summary>
//        public static AccountManager fromEncryptedLogin(string encryptedLogin)
//        {
//            throw new NotImplementedException();

//            ////todo needs testing
//            ////get username & password from encrypted string
//            //var usernameAndPass = StringCipher.Decrypt(encryptedLogin, DecrypterKey);

//            ////separate username & password
//            //var tempArray = usernameAndPass.Split(Splitter.ToCharArray());
//            //_username = tempArray[0];
//            //_password = tempArray[1];

//        }


//        /// <summary>
//        /// Generates KEY1 based on the username & password
//        /// </summary>
//        public string getKey1() => _key1;

//        /// <summary>
//        /// Gets the username & password as an encrypted string for storage in config (XML)
//        /// Done so that, login details are not visible to plain sight in config, adds a layer security
//        /// </summary>
//        public string getEncryptedLogin()
//        {
//            //combine username & password together for easy storage
//            var combined = _username + Splitter + _password;

//            //encrypt the string & return to caller
//            var encrypted = StringCipher.Encrypt(combined, DecrypterKey);

//            return encrypted;
//        }

//        /// <summary>
//        /// Generates KEY1 from the inputed username & password
//        /// Used when account is being created, doesn't yet exist in server
//        /// </summary>
//        public static string generateKey1(string username, string password)
//        {
//            //combine username & password
//            var combined = username + password;

//            //get the has of the combined text
//            var hash = stringToHash(combined);

//            //return hash as KEY1
//            return hash;

//        }



//        /** PRIVATE METHODS **/

//        /// <summary>
//        /// Converts any given string to its SHA256 hash in hexadecimal 
//        /// </summary>
//        private static string stringToHash(string input)
//        {
//            //change text to stream
//            var textStream = GenerateStreamFromString(input);

//            //get hash of this combined text
//            SHA256 mySHA256 = SHA256.Create();
//            byte[] hashValue = mySHA256.ComputeHash(textStream);

//            //return hex of hash as string
//            return getByteString(hashValue);



//            /** INTERNAL METHODS **/

//            // Display the byte array in a readable format.
//            void PrintByteArray(byte[] array)
//            {
//                //for each byte in array
//                for (int i = 0; i < array.Length; i++)
//                {
//                    //prints byte as 2 digit hexadecimal
//                    Console.Write($"{array[i]:X2}");
//                    //create a space every 4 bytes
//                    if ((i % 4) == 3) Console.Write(" ");
//                }
//                Console.WriteLine();
//            }

//            // converts an array of bytes to hex in string
//            string getByteString(byte[] array)
//            {
//                string returnString = "";

//                //for each byte in array
//                for (int i = 0; i < array.Length; i++)
//                {
//                    //add byte as 2 digit hexadecimal to string
//                    returnString = returnString + $"{array[i]:X2}";
//                }

//                //return compiled string to caller
//                return returnString;
//            }

//            MemoryStream GenerateStreamFromString(string value)
//            {
//                return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
//            }
//        }


//        /// <summary>
//        /// Class used to encrypt & decrypt strings
//        /// </summary>
//        private static class StringCipher
//        {
//            // This constant is used to determine the keysize of the encryption algorithm in bits.
//            // We divide this by 8 within the code below to get the equivalent number of bytes.
//            private const int Keysize = 256;

//            // This constant determines the number of iterations for the password bytes generation function.
//            private const int DerivationIterations = 1000;

//            public static string Encrypt(string plainText, string passPhrase)
//            {
//                // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
//                // so that the same Salt and IV values can be used when decrypting.  
//                var saltStringBytes = Generate256BitsOfRandomEntropy();
//                var ivStringBytes = Generate256BitsOfRandomEntropy();
//                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
//                using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
//                {
//                    var keyBytes = password.GetBytes(Keysize / 8);
//                    using (var symmetricKey = new RijndaelManaged())
//                    {
//                        symmetricKey.BlockSize = 256;
//                        symmetricKey.Mode = CipherMode.CBC;
//                        symmetricKey.Padding = PaddingMode.PKCS7;
//                        using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
//                        {
//                            using (var memoryStream = new MemoryStream())
//                            {
//                                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
//                                {
//                                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
//                                    cryptoStream.FlushFinalBlock();
//                                    // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
//                                    var cipherTextBytes = saltStringBytes;
//                                    cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
//                                    cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
//                                    memoryStream.Close();
//                                    cryptoStream.Close();
//                                    return Convert.ToBase64String(cipherTextBytes);
//                                }
//                            }
//                        }
//                    }
//                }
//            }

//            public static string Decrypt(string cipherText, string passPhrase)
//            {
//                // Get the complete stream of bytes that represent:
//                // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
//                var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
//                // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
//                var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
//                // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
//                var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
//                // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
//                var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

//                using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
//                {
//                    var keyBytes = password.GetBytes(Keysize / 8);
//                    using (var symmetricKey = new RijndaelManaged())
//                    {
//                        symmetricKey.BlockSize = 256;
//                        symmetricKey.Mode = CipherMode.CBC;
//                        symmetricKey.Padding = PaddingMode.PKCS7;
//                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
//                        {
//                            using (var memoryStream = new MemoryStream(cipherTextBytes))
//                            {
//                                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
//                                {
//                                    var plainTextBytes = new byte[cipherTextBytes.Length];
//                                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
//                                    memoryStream.Close();
//                                    cryptoStream.Close();
//                                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
//                                }
//                            }
//                        }
//                    }
//                }
//            }

//            private static byte[] Generate256BitsOfRandomEntropy()
//            {
//                var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
//                using (var rngCsp = new RNGCryptoServiceProvider())
//                {
//                    // Fill the array with cryptographically secure random bytes.
//                    rngCsp.GetBytes(randomBytes);
//                }
//                return randomBytes;
//            }
//        }

//    }
//}
