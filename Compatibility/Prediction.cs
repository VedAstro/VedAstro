using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genso.Astrology.Library;

namespace Compatibility
{
    /// <summary>
    /// simple data type to contain info on a kuta prediction
    /// Note : properties can only be set once
    /// </summary>
    internal class Prediction
    {
        //DATA FIELDS
        private string _info = "";
        private string _maleInfo = "";
        private string _femaleInfo = "";
        private string _name = "";
        private string _description = "";


        //PUBLIC PROPERTIES
        public string Name
        {
            get => _name;
            set
            {
                //if value already set, raise alarm
                if (_name != "") throw new InvalidOperationException("Only set once!");
                _name = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                //if value already set, raise alarm
                if (_description != "") throw new InvalidOperationException("Only set once!");
                _description = value;
            }
        }

        public EventNature Nature { get; set; }

        public string Info
        {
            get => _info;
            set
            {
                //if value already set, raise alarm
                if (_info != "") throw new InvalidOperationException("Only set once!");
                _info = value;
            }
        }

        public string MaleInfo
        {
            get => _maleInfo;
            set
            {
                //if value already set, raise alarm
                if (_maleInfo != "") throw new InvalidOperationException("Only set once!");
                _maleInfo = value;
            }
        }

        public string FemaleInfo
        {
            get => _femaleInfo;
            set
            {
                //if value already set, raise alarm
                if (_femaleInfo != "") throw new InvalidOperationException("Only set once!");
                _femaleInfo = value;
            }
        }


        //PRIVATE METHODS
    }
}
