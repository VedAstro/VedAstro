using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    public static class PersonManagerTools
    {
        /// <summary>
        /// Uses name and birth year to generate human readable ID for a new person record
        /// created so that user can type ID direct into URL based on only memory of name and birth year
        /// </summary>
        public static async Task<string> GeneratePersonId(string ownerId, string personName, string birthYear, bool failIfDuplicate = false)
        {
            //remove all space from name : Jamés Brown > JamésBrown
            var spaceLessName = Tools.RemoveWhiteSpace(personName);

            //almost done, name with birth year at the back
            var humanId = spaceLessName + birthYear;

            //check if ID is really unique, else it would need a number at the back 
            //try to find a person, if null then no new id is unique
            //jamesbrown and JamesBrown, both should by common sense work
            var idIsSafe = IsUniquePersonId(humanId);

            //if duplicate not allowed by called, then END here!
            if (failIfDuplicate && !idIsSafe) { throw new Exception("Person ID already Exist!"); }

            //if id NOT safe, add nonce and try again, possible nonce has been used
            //JamésBrown > JamésBrown1
            var nonceCount = 1; //start nonce at 1
        TryAgain:
            var noncedId = humanId; //clear pre nonce if any 
            if (!idIsSafe)
            {
                //make unique
                noncedId += nonceCount;
                nonceCount++; //increment for next if needed
                //try again
                idIsSafe = IsUniquePersonId(noncedId);

                //if unique found, end here
                if (idIsSafe) { return noncedId; }

                //try again with higher nonce
                else { goto TryAgain; }
            }

            //once control reaches here id should be all good
            return noncedId;


            //---------------LOCAL FUNCTIONS-------------------------------

            //returns true if no other record exist with same id
            //note search without owner ID, thus making person ID unique without owner ID for easy linking with images & other data
            bool IsUniquePersonId(string personId)
            {
                var findPersonXmlById = AzureTable.PersonList?.Query<PersonListEntity>(row => row.RowKey == personId);
                var x = !findPersonXmlById.Any();

                return x;
            }




        }
    }
}
