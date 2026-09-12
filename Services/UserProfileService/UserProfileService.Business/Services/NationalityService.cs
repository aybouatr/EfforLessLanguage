using System;
using DataAccesLayer;
using System.Collections.Generic;


namespace BusinessLogicLayer
{

    public class nationalitY
    {
   
        static public bool GetNationalityById(long id, out NationalityDTO? nationalit)
        {
            if (Nationality.GetNationalityById(id, out NationalityDTO? nationalityDTO))
            {
                nationalit = nationalityDTO;
            }
            else
            {
                nationalit = null;
            }
            return nationalit != null;
        }

        static public List<NationalityDTO>? GetAllNationalities()
        {
            List<NationalityDTO> nationalities = new List<NationalityDTO>(); 

            if (Nationality.GetAllNationalities(out List<NationalityDTO> nationalityDTOs))
                return nationalityDTOs;
            return null;

        }

        static public bool AddNewNationality(ref NationalityDTO? nationalityDTO)
        {
          long newId =  Nationality.AddNewNationality(nationalityDTO.Name);
            if (newId > 0)
            {
                nationalityDTO.Id = newId;
                return true;
            }
            return false;
        }

    }
}