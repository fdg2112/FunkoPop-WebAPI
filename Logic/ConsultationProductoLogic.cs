using System;
using System.Collections.Generic;
using System.Linq;
using Entities;

namespace Logic
{
    public class ConsultationProductLogic : BaseLogic
    {
        public Consultation_product Get(int id)
        {
            var consultationProduct = _context.Consultation_product.FirstOrDefault(cp => cp.Id == id && cp.Active) ?? throw new Exception("Consultation product not found.");
            return consultationProduct;
        }

        public List<Consultation_product> GetList(int product_id, int user_id)
        {
            return _context.Consultation_product
                .Where(cp => cp.Product_id == product_id && cp.User_id == user_id && cp.Active)
                .OrderByDescending(cp => cp.Id)
                .ToList();
        }

        public Consultation_product NewConsultation(Consultation_product consultationProduct)
        {
            var currentProduct = _context.Product.FirstOrDefault(p => p.Id == consultationProduct.Product_id && p.Active) ?? throw new Exception("Product not found.");
            consultationProduct.Active = true;
            _context.Consultation_product.Add(consultationProduct);
            _context.SaveChanges();

            return consultationProduct;
        }

        public Consultation_product NewResponse(int consultation_product_id, string response)
        {
            var currentConsultation = _context.Consultation_product
                .FirstOrDefault(cp => cp.Id == consultation_product_id && cp.Response == null) ?? throw new Exception("Consultation product not found.");
            currentConsultation.Response = response;
            _context.SaveChanges();

            return currentConsultation;
        }

        public List<Consultation_product> GetUnanswered(User user)
        {
            return _context.Consultation_product
                .Where(cp => cp.User_id == user.Id && cp.Response == null && cp.Active)
                .ToList();
        }
    }
}
