using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Represents an XML file that contains data. This calls encapsulates the XML files
    /// </summary>
    public class Data
    {
        private readonly string _filePath;
        private readonly Stream _writeStream;
        private XDocument _document;
        private IEnumerable<XElement> _allRecords;

        //mark which data underlying type is used (default is false)
        private bool _fileType = false;
        private bool _blobType = false;
        private bool _readOnly = false; //if marked data cannot be edited, if violated raises alarm

        /// <summary>
        /// Used when initializing from Azure functions, files in blob
        /// </summary>
        public Data(Stream readStream, Stream writeStream)
        {
            //stream to write data
            _writeStream = writeStream;

            //document for reading data
            _document = XDocument.Load(readStream);

            //get all records in document
            _allRecords = _document.Root.Elements();

            //underlying type is blob
            _blobType = true;

        }

        /// <summary>
        /// Used when initializing from Azure functions, READ-ONLY files in blob
        /// </summary>
        public Data(Stream readStream)
        {
            //document for reading data
            _document = XDocument.Load(readStream);

            //get all records in document
            _allRecords = _document.Root.Elements();

            //underlying type is blob
            _blobType = true;

            //since no write stream, enable read only access
            _readOnly = true;
        }


        /// <summary>
        /// Used when initialing locally, with files on disk
        /// </summary>
        public Data(string filePath)
        {
            //save file path for later use (saving changes)
            _filePath = filePath;

            //document for reading data
            _document = XDocument.Load(filePath);

            //get all records in document
            _allRecords = _document.Root.Elements();

            //underlying type is file
            _fileType = true;
        }



        /** PUBLIC METHODS **/

        /// <summary>
        /// Gets the value of a data by its element name in the specified Type
        /// </summary>
        /// TODO 1 Possibly via generics can make method return list or single values or no value
        public T GetValue<T>(string elementName)
        {
            //get elements with matching name
            var foundElements =
                from record in _allRecords
                where record.Name.ToString() == elementName
                select record;

            //if no element found, throw error
            if (!foundElements.Any()) { throw new Exception("Element doesn't exist!"); }

            //if more than 1 element found throw error
            if (foundElements.Count() > 1) { throw new Exception("More than 1 element found!"); }

            //get value of the data
            var value = foundElements.FirstOrDefault().Value;

            //convert value to specified type
            return (T)Convert.ChangeType(value, typeof(T));

        }

        public Stream GetStream()
        {
            Stream stream = new MemoryStream();
            _document.Save(stream);
            // Rewind the stream ready to read from it elsewhere
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Gets the record by its name & its value
        /// </summary>
        public XElement GetRecord(string elementName, string elementValue)
        {
            //find record with matching child
            var foundElements =
                from record in _allRecords
                where record.Element(elementName)?.Value == elementValue
                select record;

            //if more than 1 record found throw error
            if (foundElements.Count() > 1) { throw new Exception("More than 1 record found!"); }

            //return the found record
            return foundElements.FirstOrDefault();
        }

        /// <summary>
        /// Gets the record by its name
        /// </summary>
        public XElement GetRecord(string elementName)
        {
            //find record with name
            var foundElements =
                from record in _allRecords
                where record.Name == elementName
                select record;

            //if more than 1 record found throw error
            if (foundElements.Count() > 1) { throw new Exception("More than 1 record found!"); }

            //return the found record
            return foundElements.FirstOrDefault();
        }


        /// <summary>
        /// Check if an element with matching name & value exits
        /// </summary>
        public bool IsExist(string elementName, string elementValue)
        {
            //try to look for element
            var found = GetRecord(elementName, elementValue);

            //if none is found, no element exist
            return found != null;
        }

        /// <summary>
        /// Inserts record into XML document
        /// </summary>set
        public void InsertRecord(XElement record)
        {
            if (_readOnly) { throw new InvalidOperationException("Data is read-only!"); }

            //add new record to the document
            _document.Root.Add(record);

            //save changes underlying file
            UpdateUnderlyingFile();

        }

        /// <summary>
        /// Check if record with same domain name exists in the list
        /// </summary>
        //public bool isExist(string domain)
        //{
        //    //search for record with matching domain in list
        //    var foundRecords =
        //        from record in _allRecords
        //        where (string)record.Element(Data.DomainList.Domain) == domain
        //        select record;

        //    //if no record found, return false
        //    if (!foundRecords.Any())
        //    {
        //        return false;
        //    }
        //    //record found, return true (exist)
        //    else
        //    {
        //        return true;
        //    }
        //}

        /// <summary>
        /// Deletes the specified record from the XML document
        /// TODO can be improved with expereminetation
        /// NOTE : make sure update underlying file method is called after calling this method
        /// </summary>
        public void DeleteRecord(XElement record)
        {
            if (_readOnly) { throw new InvalidOperationException("Data is read-only!"); }

            //remove node from document
            record.Remove();

            //save changes made to document (push changes to server)
            //_document.Save(_writeStream);
            //await _writeStream.FlushAsync();

            //byte[] dataBuffer = Encoding.UTF8.GetBytes(_document.ToString());

            //_writeStream.WriteAsync(dataBuffer, 0, dataBuffer.Length);

        }

        /// <summary>
        /// Gets all records in list
        /// </summary>
        public IEnumerable<XElement> GetAllRecords() => _allRecords;

        /// <summary>
        /// Updates an already existing record by name of record
        /// </summary>
        public void UpdateRecord(string name, string value)
        {
            if (_readOnly) { throw new InvalidOperationException("Data is read-only!"); }

            //get the record that needs to be updated
            var foundRecords = from record in _allRecords
                               where record.Name.ToString() == name
                               select record;

            //if not found, stop here & throw error
            if (!foundRecords.Any()) { throw new Exception("Record not found in XML!"); }

            //TODO if found more than 1, throw error

            //change value of record
            var element = foundRecords.FirstOrDefault();
            element.Value = value;

            //save changes underlying file
            UpdateUnderlyingFile();

            return;
        }

        //TODO 2 Implement UploadChanges data method, because actual file is only updated when azure function (fucntion in entry point returns)

        /// <summary>
        /// Save changes underlying XML file
        /// based on underlying file type save data accordingly
        /// </summary>
        public void UpdateUnderlyingFile()
        {
            if (_readOnly) { throw new InvalidOperationException("Data is read-only!"); }

            if (_blobType)
            {
                _document.Save(_writeStream, SaveOptions.None);
            }
            else if (_fileType)
            {
                _document.Save(_filePath);
            }
        }
    }
}
