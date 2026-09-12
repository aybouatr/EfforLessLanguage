using DataAccesLayer;
using System;
using System.Collections.Generic;



namespace BusinessLogicLayer
{

    public class Interest
    {
        public long Id { get; set; }
        public string? Name { get; set; }

        public long ValueToExtractInterests { get; set; }

        public Interest(InterestsDTO? interestDTO)
        {
            Id = interestDTO?.Id ?? 0;
            Name = interestDTO?.Name;
            ValueToExtractInterests = interestDTO.ValueToExtractInterests;
        }


        static public Interest? GetInterestByName(string? name)
        {
            if (Interests.GetInterestByName(name, out InterestsDTO? interestDTO))
            {
                return new Interest(interestDTO);
            }
            else
            {
                return null;
            }
        }

        static public long AddInterest(string? name)
        {
            return Interests.AddNewInetrest(name);
        }


        static public List<Interest> GetAllInterests()
        {
            List<Interest> interests = new List<Interest>();

            if (Interests.GetAllInterests(out List<InterestsDTO> interestDTOs))
            {
                foreach (var interestDTO in interestDTOs)
                {
                    interests.Add(new Interest(interestDTO));
                }
            }

            return interests;
        }


    }


}

