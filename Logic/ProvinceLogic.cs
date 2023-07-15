using Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Logic
{
    public class ProvinceLogic : BaseLogic
    {
        public ProvinceLogic() { }

        public List<Province> GetAll()
        {
            try
            {
                return _context.Province.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de provincias.", ex);
            }
        }

        public Province Get(int id)
        {
            try
            {
                return _context.Province.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la provincia.", ex);
            }
        }
    }
}

