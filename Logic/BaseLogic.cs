using Data;

namespace Logic
{
    public class BaseLogic
    {
        protected FunkoPopContext _context;

        public BaseLogic()
        {
            _context = new FunkoPopContext();
        }
    }
}
